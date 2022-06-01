using Network.Data;
using Network.Data.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;
using Game.Data.Abstracts;
using Game.Data.Attributes;

namespace Game.Data.Models
{
    public class PositionModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public int MapID { get; set; }

        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float RotationW { get; set; }

#if UNITY_5
        public PositionModel() : this(UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity)
        { }

        public PositionModel(UnityEngine.Vector3 Position, UnityEngine.Quaternion Rotation)
        {
            this.Position = Position;
            this.Rotation = Rotation;
        }

        [NonWrap]
        public UnityEngine.Vector3 Position
        {
            get { return new UnityEngine.Vector3(PositionX, PositionY, PositionZ); }
            set
            {
                PositionX = value.x;
                PositionY = value.y;
                PositionZ = value.z;
            }
        }

        [NonWrap]
        public UnityEngine.Quaternion Rotation
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

        public static implicit operator PositionModel(UnityEngine.Transform transform)
        {
            return new PositionModel(transform.position, transform.rotation);
        }
#endif
        public float Distance(PositionModel Position)
        {
            if (MapID != Position.MapID)
                return float.NaN;
            else
            {
                float D = Convert.ToSingle(Math.Sqrt
                    (
                    Math.Pow(Position.PositionX - PositionX, 2) +
                    Math.Pow(Position.PositionY - PositionY, 2) +
                    Math.Pow(Position.PositionZ - PositionZ, 2)
                    ));
                Base.Factories.LoggerFactory.GetLogger(this).LogInfo("Distance = {0}", D);
                return D;
            }
        }

        public override string ToString()
        {
            return string.Format("P:{0}x{1}x{2} | R:{3}x{4}x{5}x{6}", PositionX, PositionY, PositionZ, RotationX, RotationY, RotationZ, RotationW);
        }

        public override bool Equals(object obj)
        {
            if(obj is PositionModel)
            {
                var Position = obj as PositionModel;
                return
                    Position.MapID == MapID &&
                    Position.PositionX == PositionX &&
                    Position.PositionY == PositionY &&
                    Position.PositionZ == PositionZ &&
                    Position.RotationX == RotationX &&
                    Position.RotationY == RotationY &&
                    Position.RotationZ == RotationZ &&
                    Position.RotationW == RotationW;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
