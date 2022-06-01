using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.Responses
{
    using Requests;
    using Client;

    public class ChatPacket : PiGameResponse
    {
        public override ushort ID => PacketID.Chat;

        string message;
        public override bool Read(IDataPacket packet)
        {
            message = packet.ReadString();
            return true;
        }

        public override void Execute()
        {
           
        }
    }
}