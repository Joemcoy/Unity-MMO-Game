using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;

using UnityEngine;
using tFramework.Network.Interfaces;
namespace Scripts.Network.Responses.GameClient
{
    public class UpdateTimeResponse : PiBaseResponse
    {
        public override ushort ID
        {
            get { return PacketID.UpdateTime; }
        }

        TimeSpan time;
        public override bool Read(IDataPacket Packet)
        {
            time = Packet.ReadTimeSpan();
            return true;
        }

        public override void Execute()
        {
            var climate = GameObject.FindGameObjectWithTag("Climate");
            if (climate != null)
            {
                var unistorm = climate.GetComponentInChildren<UniStormWeatherSystem_C>();
                //unistorm.timeStopped = true;
                //unistorm.hourCounter = time.Hours;
                //unistorm.Hour = time.Hours;
                unistorm.startTimeHour = time.Hours;
                unistorm.startTimeMinute = time.Minutes;
                unistorm.LoadTime();
            }
        }
    }
}