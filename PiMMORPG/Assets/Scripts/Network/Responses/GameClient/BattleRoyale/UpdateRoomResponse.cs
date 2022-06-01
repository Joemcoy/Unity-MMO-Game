using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using PiMMORPG;
using PiMMORPG.Client.BattleRoyale;
using PiMMORPG.Client.BattleRoyale.Enums;

using tFramework.Network.Interfaces;

namespace Scripts.Network.Responses.GameClient.BattleRoyale
{
    using Local.UI;

    public class UpdateRoomResponse : PiBRResponse
    {
        public override ushort ID { get { return PacketID.UpdateRoom; } }

        RoomState state;
        int waterLevel, playerCount;
        TimeSpan timeout;

        public override bool Read(IDataPacket packet)
        {
            state = packet.ReadEnum<RoomState>();
            waterLevel = packet.ReadInt();
            playerCount = packet.ReadInt();

            if(state != RoomState.WaitingForPlayer)
                timeout = packet.ReadTimeSpan();
            return true;
        }

        public override void Execute()
        {
            var viewer = GameObject.FindObjectOfType<RoomInfoViewer>();
            viewer.WaterLevel = waterLevel;
            viewer.PlayerCount = playerCount;
            
            switch(state)
            {
                case RoomState.WaitingForPlayer:
                    viewer.Status = "Aguardando outro jogador..";
                    break;
                case RoomState.WaitingTimeout:
                    viewer.Status = string.Format("Esperando por mais {0:00}:{1:00}", timeout.Minutes, timeout.Seconds);
                    break;
                case RoomState.Starting:
                    viewer.Status = string.Format("Iniciando em {0:00}:{1:00}", timeout.Minutes, timeout.Seconds);
                    break;
                case RoomState.Running:
                    viewer.Status = string.Format("Maré subindo em {0:00}:{1:00}", timeout.Minutes, timeout.Seconds);
                    break;
            }
        }
    }
}