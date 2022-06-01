using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client.BattleRoyale;

using UnityEngine;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Responses.GameClient.BattleRoyale
{
    using Local.Control;

    public class ElevateWaterResponse : PiBRResponse
    {
        public override ushort ID { get { return PacketID.ElevateWater; } }

        int level;
        public override bool Read(IDataPacket packet)
        {
            level = packet.ReadInt();
            return true;
        }

        public override void Execute()
        {
            var elevator = GameObject.FindObjectOfType<WaterElevator>();
            elevator.currentLevel = level;
        }
    }
}