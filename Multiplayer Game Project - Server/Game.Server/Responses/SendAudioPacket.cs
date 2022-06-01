using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Client;
using Game.Server;

using Network.Data.Interfaces;
using Base.Factories;
using Game.Server.Writers;

namespace Game.Server.Responses
{
    public class SendAudioPacket : GCResponse
    {
        public override uint ID { get { return PacketID.SendAudio; } }

        int Channels, Frequency;
        byte Type;
        byte[] Buffer;

        public override bool Read(ISocketPacket Packet)
        {
            Type = Packet.ReadByte();
            Channels = Packet.ReadInt();
            Frequency = Packet.ReadInt();

            Buffer = Packet.ReadBuffer();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Server = SingletonFactory.GetInstance<GameServer>();
            var Packet = new SendAudioWriter();

            Packet.CID = Client.CurrentCharacter.ID;
            Packet.Channels = Channels;
            Packet.Frequency = Frequency;
            Packet.Buffer = Buffer;

            LoggerFactory.GetLogger(this).LogWarning("Received user {3} audio, channels: {0}, frequency: {1}, buffer: {2}", Channels, Frequency, Buffer.Length, Client.CurrentCharacter.ID);

            foreach (var Client in Server.Clients.Where(C => C.CurrentCharacter != null && C.CurrentMap.ID == Client.CurrentMap.ID))// && !C.Socket.Equals(Client.Socket)))
            {
                if (Type == 0)
                {
                    var Distance = this.Client.CurrentCharacter.Position.Distance(Client.CurrentCharacter.Position);
                    LoggerFactory.GetLogger(this).LogWarning("Distance of client {0} and {1} is: {2}", Client.CurrentCharacter.Name, this.Client.CurrentCharacter.Name, Distance);

                    if(Distance <= 30f)
                        Client.Socket.Send(Packet);
                }
                else if(Type == 1)
                {
                    Client.Socket.Send(Packet);
                }
            }
        }
    }
}