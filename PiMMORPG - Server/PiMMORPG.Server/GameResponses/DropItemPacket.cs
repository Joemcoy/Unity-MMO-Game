using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.GameResponses
{
    using Client;
    using Models;
    using Manager;

    public class DropItemPacket : PiGameResponse
    {
        public override ushort ID => PacketID.DropItem;

        Drop drop;
        public override bool Read(IDataPacket packet)
        {
            drop = packet.ReadWrapper<Drop>();
            drop.Map = Client.Character.Map;
            return true;
        }

        public override void Execute()
        {
            WorldManager.DropItem(Client, drop);   
        }
    }
}