using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Responses
{
    using Client;
    using Requests;

    public class ChatResponse : PiBaseResponse
    {
        public override ushort ID => PacketID.Chat;

        protected string message;
        public override bool Read(IDataPacket packet)
        {
            message = packet.ReadString();
            return true;
        }

        public override void Execute()
        {
            var server = ServerControl.GetServer(Client.Socket.Server.EndPoint.Port);

            if (message.StartsWith("/"))
            {
                if (!CommandFactory.ExecuteCommand(message.Substring(1), Client))
                {
                    server.SendSystemMessage("Not valid command!", c => c.Character.ID == Client.Character.ID);
                }
            }
            else
            {
                var packet = new ChatRequest
                { Message = string.Format("<b><color={0}>{1}</color></b>: {2}", Client.Account.Access.LevelColor, Client.Character.Name, message) };
                server.SendToAll(packet, c => !c.SwitchingMap && c.Character.Map.ID == Client.Character.Map.ID);
            }
        }
    }
}