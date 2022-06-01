using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
namespace PiMMORPG.Server.RPG.Manager
{
    using Models;
    using Client.RPG;
    using Requests;
    using General.Drivers;

    public static class ItemManager
    {
        public static CharacterItem GetItem(Guid serial)
        {
            using (var ctx = new CharacterItemDriver())
                return ctx.GetModel(ctx.CreateBuilder().Where(m => m.Serial).Equal(serial));
        }

        public static void AddItem(PiRPGClient client, CharacterItem item)
        {
            using (var ctx = new CharacterItemDriver())
            {
                for (uint i = 0; i < 100; i++)
                {
                    var query = ctx.CreateBuilder().Where(m => m.Character).Equal(item.Character).And(m => m.Slot).Equal(i);
                    if (!ctx.HasModel(query))
                        item.Slot = i;
                }
                ctx.AddModel(item);
            }
            client.Character.Items = client.Character.Items.Concat(new[] { item }).ToArray();

            var packet = new GiveItemRequest { Item = item };
            client.Socket.Send(packet);
        }

        public static void AddItem(PiRPGClient client, uint inventoryID, Guid serial, uint quantity)
        {
            var item = new CharacterItem
            {
                Quantity = quantity,
                Serial = serial,
                Slot = 0,
                HotbarSlot = -1,
                Equipped = false,
                Character = client.Character.ID
            };

            using (var ctx = new ItemDriver())
                item.Info = ctx.GetModel(ctx.CreateBuilder().Where(m => m.InventoryID).Equal(inventoryID));
            
            AddItem(client, item);
        }

        public static void RemoveItem(PiRPGClient client, Guid serial)
        {
            client.Character.Items = client.Character.Items.Where(i => i.Serial != serial).ToArray();
            using (var ctx = new CharacterItemDriver())
                ctx.RemoveModel(ctx.CreateBuilder().Where(m => m.Serial).Equal(serial));
        }

        public static void SetItemQuantity(PiRPGClient client, Guid serial, uint quantity, bool add = false)
        {
            var item = client.Character.Items.First(i => i.Serial == serial);
            item.Quantity = add ? item.Quantity + quantity : quantity;

            using (var ctx = new CharacterItemDriver())
            {
                var model = ctx.GetModel(ctx.CreateBuilder().Where(m => m.Serial).Equal(serial));
                model.Quantity = add ? model.Quantity + quantity : quantity;
                ctx.UpdateModel(model);
            }
        }

        public static void DecrementItemQuantity(PiRPGClient client, Guid serial, uint quantity)
        {
            var item = client.Character.Items.First(i => i.Serial == serial);
            item.Quantity -= quantity;

            using (var ctx = new CharacterItemDriver())
            {
                var model = ctx.GetModel(ctx.CreateBuilder().Where(m => m.Serial).Equal(serial));
                model.Quantity = model.Quantity - quantity;
                ctx.UpdateModel(model);
            }
        }

        public static void SetItemSlot(PiRPGClient client, Guid serial, uint slot)
        {
            var item = client.Character.Items.First(i => i.Serial == serial);
            var old = item.Slot;
            item.Slot = slot;

            using (var ctx = new CharacterItemDriver())
            {
                var model = ctx.GetModel(ctx.CreateBuilder().Where(m => m.Serial).Equal(serial));
                model.Slot = slot;
                ctx.UpdateModel(model);
            }

            LoggerFactory.GetLogger("ItemManager").LogInfo("Player {0} moved item from slot {1} to {2}", client.Character.Name, old, slot);
        }

        public static void SetItemHotbarSlot(PiRPGClient client, Guid serial, int slot)
        {
            var item = client.Character.Items.First(i => i.Serial == serial);
            item.HotbarSlot = slot;

            using (var ctx = new CharacterItemDriver())
            {
                var model = ctx.GetModel(ctx.CreateBuilder().Where(m => m.Serial).Equal(serial));
                model.HotbarSlot = slot;
                ctx.UpdateModel(model);
            }
        }

        public static void MergeItems(PiRPGClient client, Guid from, Guid to, uint quantity)
        {
            RemoveItem(client, from);
            SetItemQuantity(client, to, quantity);
        }

        public static void UnstackItem(PiRPGClient client, Guid from, uint fromSlot, Guid to, uint toSlot, uint quantity)
        {
            SetItemSlot(client, from, fromSlot);
            DecrementItemQuantity(client, from, quantity);

            var To = GetItem(from).Clone<CharacterItem>();
            To.ID = 0;
            To.Slot = toSlot;
            To.Quantity = quantity;
            To.Serial = to;
            AddItem(client, To);
        }
    }
}