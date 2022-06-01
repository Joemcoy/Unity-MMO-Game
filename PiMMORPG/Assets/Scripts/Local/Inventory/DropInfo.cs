using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.General;
using Devdog.InventoryPro;
using PiMMORPG.Client;
using UnityEngine;
using tFramework.Factories;

namespace Scripts.Local.Inventory
{
    using Interfaces;
    using Network.Requests.GameClient;
    public class DropInfo : MonoBehaviour, ITriggerCallbacks
    {
        public Guid DropSerial;

        void Start()
        {
            if (DropSerial == null)
                DropSerial = Guid.NewGuid();
        }

        public bool OnTriggerUsed(Player player)
        {
            var client = PiBaseClient.Current;
            if (client.Socket.Connected)
            {
                var Packet = new RemoveDropRequest();
                Packet.Serial = DropSerial;
                client.Socket.Send(Packet);

                /*var Motion = new PlayMotionWriter();
                Motion.TriggerName = "Pick Item";
                Client.Socket.Send(Motion);*/
            }

            var Item = GetComponent<ItemTrigger>().itemPrefab as INetworkItem;
            if (Item != null)
                Item.Serial = DropSerial;
            return true;
        }

        public bool OnTriggerUnUsed(Player player)
        {
            return true;
        }
    }
}
