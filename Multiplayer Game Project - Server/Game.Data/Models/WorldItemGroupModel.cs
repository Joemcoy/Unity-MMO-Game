using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Data.Interfaces;
using Game.Data.Abstracts;
using Network.Data;
using Network.Data.Interfaces;

namespace Game.Data.Models
{
    public class WorldItemGroupModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public float AOIX { get; set; }
        public float AOIY { get; set; }
        public float AOIZ { get; set; }

        public float TotalVectorsX { get; set; }
        public float TotalVectorsY { get; set; }
        public float TotalVectorsZ { get; set; }

        public float MaxPointInGroup { get; set; }
        public bool InRange { get; set; }
        public int ClaimLeader { get; set; }
    }
}
