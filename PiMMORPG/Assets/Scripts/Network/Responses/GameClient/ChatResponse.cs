using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Enums;
using PiMMORPG.Models;
using PiMMORPG.Client;

using tFramework.Network.Interfaces;

using UnityEngine;

namespace Scripts.Network.Responses.GameClient
{
    public class ChatResponse : PiBaseResponse
    {
        public override bool ThreadSafe { get { return false; } }
        public override ushort ID
        {
            get { return PacketID.Chat; }
        }

        string Message;
        public override bool Read(IDataPacket Packet)
        {
            Message = Packet.ReadString();
            return true;
        }

        public override void Execute()
        {
            Local.UI.ChatAppender.AppendMessage(Message);
        }
    }
}