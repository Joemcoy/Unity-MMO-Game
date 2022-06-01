using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;
using PiMMORPG.Models;
using tFramework.Network.Interfaces;

using UnityEngine;

namespace Scripts.Network.Responses.GameClient
{
    using Local.Control;
    using Local.Locomotion;

    public class SyncCharacterResponse : PiBaseResponse
    {
        public override ushort ID { get { return PacketID.SyncCharacter; } }

        uint cID;
        Position Position;
        float h, v;

        public override bool Read(IDataPacket Packet)
        {
            cID = Packet.ReadUInt();
            (Position = new Position()).ReadPacket(Packet);
            h = Packet.ReadFloat();
            v = Packet.ReadFloat();

            return true;
        }

        public override void Execute()
        {
            var Object = WorldControl.GetPlayer(cID);
            if (Object != null)
            {
                var Locomotor = Object.GetComponent<KeyboardLocomotor>();
                //var Current = Object.transform.position;
                //var Difference = (Position.Vector - Current);

                //if(Difference.sqrMagnitude > .3f)

                Locomotor.H = h;
                Locomotor.V = v;
                Locomotor.Position = Position.Vector;
                Locomotor.Rotation = Position.Quaternion;

                /*if (Difference.sqrMagnitude > .2f)
                {
                    Difference.Normalize();
                    Object.transform.rotation = Position.Quaternion;
                    Locomotor.Move(Difference.x, Difference.z, Position.Quaternion);
                }*/
            }
            else
                UnityEngine.Debug.LogError("Failed to find a player!");
        }
    }
}