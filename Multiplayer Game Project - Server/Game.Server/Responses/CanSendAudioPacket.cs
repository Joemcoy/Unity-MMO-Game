using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Client;
using Data.Client;
using Base.Factories;

using Game.Data.Information;
using Game.Data.Enums;
using Game.Server.Manager;
using Game.Data.Models;
using Game.Server.Writers;


namespace Game.Server.Responses
{
    public class CanSendAudioPacket : GCResponse
    {
        public override uint ID { get { return PacketID.CanSendAudio; } }

        byte Type;
        public override bool Read(ISocketPacket Packet)
        {
            Type = Packet.ReadByte();
            return Client != null && Client.CurrentCharacter != null;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new CanSendAudioWriter();
            int Total = 0;

            if (Client.CanSendAudio(Type, ref Total))
            {
                Packet.CanSend = true;
            }
            else
            {
                Packet.CanSend = false;
                Packet.RemainSeconds = Total;
            }
            
            LoggerFactory.GetLogger(this).LogWarning("Player {0} trying to send audio, response: {1}!", Client.CurrentCharacter.Name, Packet.CanSend);
            Socket.Send(Packet);
        }
    }
}
