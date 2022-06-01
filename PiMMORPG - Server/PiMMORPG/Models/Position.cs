using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Data.Interfaces;
using tFramework.Factories;
using tFramework.Network;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class Position : ModelBase
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float RotationW { get; set; }

        public void Copy(Position other)
        {
            PositionX = other.PositionX;
            PositionY = other.PositionY;
            PositionZ = other.PositionZ;
            RotationX = other.RotationX;
            RotationY = other.RotationY;
            RotationZ = other.RotationZ;
            RotationW = other.RotationW;
        }

        public double Distance(Position other)
        {
            if (other != null)
            {
                var dX = other.PositionX - PositionX;
                var dY = other.PositionY - PositionY;
                var dZ = other.PositionZ - PositionZ;
                var d = Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2) + Math.Pow(dZ, 2));

                LoggerFactory.GetLogger(this).LogInfo("Distance of {0} to {1} is {2}", ID, other.ID, d);
                return d;
            }
            else
                return 0;
        }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            PositionX = packet.ReadFloat();
            PositionY = packet.ReadFloat();
            PositionZ = packet.ReadFloat();
            RotationX = packet.ReadFloat();
            RotationY = packet.ReadFloat();
            RotationZ = packet.ReadFloat();
            RotationW = packet.ReadFloat();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteFloat(PositionX);
            packet.WriteFloat(PositionY);
            packet.WriteFloat(PositionZ);
            packet.WriteFloat(RotationX);
            packet.WriteFloat(RotationY);
            packet.WriteFloat(RotationZ);
            packet.WriteFloat(RotationW);
        }

#if UNITY_STANDALONE
        public UnityEngine.Vector3 Vector
        {
            get { return new UnityEngine.Vector3(PositionX, PositionY, PositionZ); }
            set
            {
                PositionX = value.x;
                PositionY = value.y;
                PositionZ = value.z;
            }
        }

        public UnityEngine.Quaternion Quaternion
        {
            get { return new UnityEngine.Quaternion(RotationX, RotationY, RotationZ, RotationW); }
            set
            {
                RotationX = value.x;
                RotationY = value.y;
                RotationZ = value.z;
                RotationW = value.w;
            }
        }

        public Position() { }
        public Position(UnityEngine.Vector3 position, UnityEngine.Quaternion rotation)
        {
            PositionX = position.x;
            PositionY = position.y;
            PositionZ = position.z;
            RotationX = rotation.x;
            RotationY = rotation.y;
            RotationZ = rotation.z;
            RotationW = rotation.w;
        }

		public Position(UnityEngine.Transform transform) : this(transform.position, transform.rotation)
        {
            
        }
#endif
    }
}