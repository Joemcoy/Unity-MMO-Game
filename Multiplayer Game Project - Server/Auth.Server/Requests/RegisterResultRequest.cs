using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Game.Data.Results;
using Auth.Client;
using Base.Factories;
using Network.Data.Interfaces;
using Gate.Server;
using Game.Data.Models;

namespace Auth.Server.Requests
{
    public class RegisterResultRequest : IRequest
    {
        public uint ID { get { return PacketID.Register; } }
        public RegisterResult Result { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteEnum(Result);
            return true;
        }
    }
}