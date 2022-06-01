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
    public class AddItemCommand : GCommand
    {
        public override string Name
        {
            get
            {
                return "additem";
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
                int TargetID = Client.CurrentCharacter.ID;
                GameClient TargetClient = Client;

                ItemModel Item;

                if (!int.TryParse(Arguments[0], out ItemID) || (Item = Manager.GetItem(ItemID)) == null)
                {
                    Message.Content = "LM:Messages.Invalid Item ID";
                    ChatManager.SendToClient(Client, Message);
                }
                else if (Arguments.Length > 1 && !uint.TryParse(Arguments[1], out Amount))
                {
                    Message.Content = "LM:Messages.Invalid Amount";
                    ChatManager.SendToClient(Client, Message);
                }
                else if(Arguments.Length > 2 && (!int.TryParse(Arguments[2], out TargetID) || (TargetClient = Server.Clients.FirstOrDefault(C => C.CurrentCharacter != null && C.CurrentCharacter.ID == TargetID)) == null))
                {
                    Message.Content = "LM:Messages.PM1";
                    ChatManager.SendToClient(Client, Message);
                }
                else
                {
                    var CItem = new CharacterItemModel();
                    CItem.ID = -1;
                    CItem.Serial = Guid.NewGuid();
                    CItem.Amount = Amount;
                    CItem.ItemID = Item.ID;
                    CItem.OwnerID = TargetID;
                    CItem.HotbarSlot = -1;
                    CItem.IsDirty = true;
                    CItem.Equiped = false;

                    Manager.AddCharacterItem(TargetID, CItem, true);

                    var Packet = new AddItemWriter();
                    Packet.Item = CItem;
                    Packet.RealItem = Item;

                    TargetClient.Socket.Send(Packet);

                    Message.Content = "LM:Messages.Item Received";
                    ChatManager.SendToClient(Client, Message, Item.UniqueID);
                }
                return true;
            }
            else return false;
        }
    }
}