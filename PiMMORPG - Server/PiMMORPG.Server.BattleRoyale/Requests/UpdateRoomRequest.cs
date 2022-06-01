using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.BattleRoyale.Requests
{
    using Client.BattleRoyale;
    using Client.BattleRoyale.Enums;

    public class UpdateRoomRequest : PiBRRequest
    {
        public override ushort ID => PacketID.UpdateRoom;

        public RoomState State { get; set; }
        public int WaterLevel { get; set; }
        public int PlayerCount { get; set; }
        public TimeSpan Timeout { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteEnum(State);
            packet.WriteInt(WaterLevel);
            packet.WriteInt(PlayerCount);

            if (State != RoomState.WaitingForPlayer)
                packet.WriteTimeSpan(Timeout);
            return true;
        }
    }
}