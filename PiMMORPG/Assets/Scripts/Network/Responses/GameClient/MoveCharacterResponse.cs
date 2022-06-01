using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Models;
using PiMMORPG.Client;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Responses.GameClient
{
    using Local.Control;
    using Local.Locomotion;

    public class MoveCharacterResponse : PiBaseResponse
    {
        public override ushort ID { get { return PacketID.MoveCharacter; } }

        uint cid;
        Position pos;

        public override bool Read(IDataPacket packet)
        {
            cid = packet.ReadUInt();
            pos = packet.ReadWrapper<Position>();
            return true;
        }

        public override void Execute()
        {
            var player = WorldControl.GetPlayer(cid);
            if(player != null)
            {
                var locomotor = player.GetComponent<KeyboardLocomotor>();
                locomotor.IsLoaded = false;
                locomotor.Position = pos.Vector;
                locomotor.Rotation = pos.Quaternion;
                //player.transform.localPosition = pos.Vector;
                //player.transform.localRotation = pos.Quaternion;
                locomotor.IsLoaded = true;

                UnityEngine.Debug.LogFormat("Moved character to {0} on {1}", pos.Vector, player.transform.position);
            }
        }
    }
}