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
    using Local.Control;
    using Local.Locomotion;

    public class ToggleRunningResponse : PiBaseResponse
    {
        public override ushort ID
        {
            get { return PacketID.ToggleRunning; }
        }

        uint CID;
        bool Running;
        public override bool Read(IDataPacket Packet)
        {
            CID = Packet.ReadUInt();
            Running = Packet.ReadBool();
            return true;
        }

        public override void Execute()
        {
            var Player = WorldControl.GetPlayer(CID);
            var Locomotor = Player.GetComponent<KeyboardLocomotor>();
            Locomotor.Running = Running;
        }
    }
}