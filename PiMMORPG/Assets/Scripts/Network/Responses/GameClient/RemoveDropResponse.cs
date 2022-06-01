using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;

using UnityEngine;
using tFramework.Network;
using tFramework.Network.Interfaces;
using Scripts.Local.Inventory;

namespace Scripts.Network.Responses.GameClient
{
    public class RemoveDropResponse : PiBaseResponse
    {
        public override ushort ID { get { return PacketID.RemoveDrop; } }

        Guid serial;
        public override bool Read(IDataPacket packet)
        {
            serial = packet.ReadGuid();
            return serial != Guid.Empty;
        }

        public override void Execute()
        {
            foreach (var drop in GameObject.FindGameObjectsWithTag("Drop"))
            {
                var info = drop.GetComponent<DropInfo>();
                if (info.DropSerial == serial)
                    GameObject.Destroy(drop);
            }
        }
    }
}