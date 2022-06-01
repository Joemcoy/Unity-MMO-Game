using Base.Factories;
using Game.Client;
using Gate.Client.Responses.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Data.Models;
using Game.Data.Enums;
using Game.Server.Manager;
using Game.Server.Writers;

namespace Game.Server.Commands
{
    public class DropItemCommand : GCommand
    {
        public override string Name
        {
            get
            {
                return "drop";
            }
        }

        public override bool Execute(params string[] Arguments)
        {
            GameClient Client = GetParameter<GameClient>("Client");
            if (Client != null && Client.Account.Access == AccessLevel.Administrator && Arguments.Length >= 1)
            {
                var Server = SingletonFactory.GetInstance<GameServer>();
                var Manager = SingletonFactory.GetInstance<ItemCacheManager>();

                var Message = new MessageModel();
                Message.Type = MessageType.System;
                Message.Access = AccessLevel.Server;

                int ItemID;
                uint Amount = Arguments.Length == 1 ? 1u : 0u;
                ItemModel Item;

                if (!int.TryParse(Arguments[0], out ItemID) || (Item = Manager.GetItem(ItemID)) == null)
                {
                        Message.Content = "LM:Messages.Invalid Item ID";
                        ChatManager.SendToClient(Client, Message);
                }
                else if(Arguments.Length > 1 && !uint.TryParse(Arguments[1], out Amount))
                {
                    Message.Content = "LM:Messages.Invalid Amount";
                    ChatManager.SendToClient(Client, Message);
                }
                else if(!Item.CanDrop())
                {
                    Message.Content = "LM:Messages.Item Cannot be Dropped";
                    ChatManager.SendToClient(Client, Message);
                }
                else
                {
                    var Drop = new DropModel();
                    Drop.Serial = Guid.NewGuid();
                    Drop.Amount = Amount;
                    Drop.ItemID = Item.ID;
                    Client.CurrentCharacter.Position.CopyTo(Drop.Position);
                    WorldManager.AddDrop(Drop);

                    var Packet = new DropItemWriter();
                    Packet.Drop = Drop;
                    Packet.InventoryID = Convert.ToUInt32(Item.UniqueID);

                    foreach (var Remote in Server.Clients.Where(C => C.CurrentCharacter != null))
                    {
                        Remote.Socket.Send(Packet);
                    }

                    Message.Content = "LM:Messages.Item Dropped";
                    ChatManager.SendToAllInMap(Client.CurrentMap.ID, Message, Item.UniqueID);
                }
                return true;
            }
            else return false;
        }
    }
}