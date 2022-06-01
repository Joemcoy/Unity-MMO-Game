using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data.Interfaces;

using Network.Data;
using Network.v1;
using Network.Data.EventArgs;

using Game.Data;

using Base.Data.Interfaces;
using Base.Factories;
using Game.Data.Enums;
using Server.Configuration;

using System.Net;
using Game.Data.Models;
using Network.Bases;
using Data.Client.Requests;
using Data.Client.Request;
using Data.Client.Writers;

namespace Data.Client
{
	public class DataClient : ClientBase<DataClient>, IComponent, ISingleton
	{
		internal Queue<object> States;
        
        public DataClient() : base()
        {
            States = new Queue<object>();
        }        

        public bool Enable()
		{
			try
			{
				if(!ComponentFactory.Enable<PortsConfiguration>())
					return false;

				Socket.EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), PortsConfiguration.DataPort);
                return Socket.Connect();
			}
			catch(Exception ex)
			{
				LoggerFactory.GetLogger(this).LogFatal(ex);
				return false;
			}
		}

        public bool Disable()
        {
            try
            {
                Socket.Disconnect();
                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }

		public void Create()
		{

		}

		public void Destroy()
		{

		}

        public TState Dequeue<TState>()
        {
            return States.Count > 0 ? (TState)States.Dequeue() : default(TState);
        }

		public void SendAccountsRequest(object State, uint Maximum = 0)
		{
            var Packet = new AccountsRequest();
            Packet.Maximum = Maximum;

            States.Enqueue(State);
			Socket.Send(Packet);
		}

        public void SendAccountRequest(object State, int AccountID)
        {
            var Packet = new AccountRequest();
            Packet.AccountID = AccountID;

            States.Enqueue(State);
            Socket.Send(Packet);
        }

		public void SendLoginRequest(object State, string Username, string Password, uint Server)
		{
            var Packet = new LoginRequest();
            Packet.Username = Username;
            Packet.Password = Password;
            Packet.Server = Server;

            States.Enqueue(State);
            Socket.Send(Packet);
		}

        public void SendWorldItems()
        {
            var Packet = new WorldItemsRequest();
            Socket.Send(Packet);
        }

        public void SendRegisterRequest(object State, string Username, string Password, string Nickname, string Email, uint Server)
		{
            var Packet = new RegisterRequest();
            Packet.Username = Username;
            Packet.Password = Password;
            Packet.Nickname = Nickname;
            Packet.Email = Email;
            Packet.Server = Server;

            States.Enqueue(State);
            Socket.Send(Packet);
		}

        public void SendCharacterListRequest(object State, int AccountID)
		{
            var Packet = new CharacterListRequest();
            Packet.AccountID = AccountID;

            States.Enqueue(State);
            Socket.Send(Packet);
		}

        public void SendCreateCharacterRequest(object State, CharacterModel Character)
        {
            var Packet = new CreateCharacterRequest();
            Packet.Character = Character;

            States.Enqueue(State);
            Socket.Send(Packet);
        }

        public void SendDeleteCharacterRequest(object State, int ID)
        {
            var Packet = new DeleteCharacterRequest();
            Packet.CharacterID = ID;

            States.Enqueue(State);
            Socket.Send(Packet);
        }

        public void SendMapRequest(object State, int MapID, bool SendItems)
        {
            var Packet = new MapRequest();
            Packet.MapID = MapID;
            Packet.SendItems = SendItems;

            States.Enqueue(State);
            Socket.Send(Packet);
        }

        public void SendUpdateCharacterPosition(CharacterModel Character)
        {
            var Packet = new UpdateCharacterPositionRequest();
            Packet.Character = Character;

            Socket.Send(Packet);
        }

        public void SendMessage(MessageModel Message)
        {
            var Packet = new MessageRequest();
            Packet.Message = Message;

            Socket.Send(Packet);
        }

        public void SendMessages(object State)
        {
            var Packet = new MessagesRequest();

            States.Enqueue(State);
            Socket.Send(Packet);
        }

        public void SendItemsInWorld(object State, int MapID)
        {
            var Packet = new ItemsInWorldRequest();
            Packet.MapID = MapID;

            States.Enqueue(State);
            Socket.Send(Packet);
        }

        public void SendPutItemInWorld(object State, int ClaimLeader, int ItemID, WorldItemGroupModel Group, PositionModel Position)
        {
            var Packet = new PutItemInWorldRequest();
            Packet.ClaimLeader = ClaimLeader;
            Packet.ItemID = ItemID;
            Packet.Group = Group;
            Packet.Position = Position;

            States.Enqueue(State);
            Socket.Send(Packet);
        }

        public void SendLauncherFilesRequest(object State)
        {
            var Packet = new LauncherFilesRequest();

            //States.Enqueue(State);
            Socket.Send(Packet);
        }

        public void SendMapsRequest()
        {
            var Packet = new MapsRequest();
            Socket.Send(Packet);
        }

        public void SendCharacterItemsRequest(object State, int CharacterID)
        {
            var Packet = new CharacterItemsRequest();
            Packet.CharacterID = CharacterID;

            States.Enqueue(State);
            Socket.Send(Packet);
        }

        public void SendUpdateCharacterItem(CharacterItemModel Item)
        {
            var Packet = new UpdateCharacterItemRequest();
            Packet.Item = Item;

            Socket.Send(Packet);
        }

        public void SendUpdateAccount(AccountModel Account)
        {
            var Packet = new UpdateAccountRequest();
            Packet.Account = Account;

            Socket.Send(Packet);
        }

        public void SendRemoveItem(CharacterItemModel Item)
        {
            var Packet = new RemoveItemRequest();
            Packet.Serial = Item.Serial;

            Socket.Send(Packet);
        }

        public void SendAddDrop(DropModel Drop)
        {
            var Packet = new AddDropRequest();
            Packet.Drop = Drop;

            Socket.Send(Packet);
        }

        public void SendDrops()
        {
            var Packet = new SendDropsRequest();
            Socket.Send(Packet);
        }

        public void RemoveDrop(Guid Serial)
        {
            var Packet = new RemoveDropRequest();
            Packet.Serial = Serial;

            Socket.Send(Packet);
        }

        public void AddItem(CharacterItemModel Item)
        {
            var Packet = new AddItemRequest();
            Packet.Item = Item;

            Socket.Send(Packet);
        }

        public void SendEquips(object State, int AccountID)
        {
            var Packet = new SendEquipsRequest();
            Packet.AccountID = AccountID;

            Socket.Send(Packet);
            States.Enqueue(State);
        }

        public void SendMobs(int Server)
        {
            var Packet = new SendMobsRequest();
            Packet.Server = Server;

            Socket.Send(Packet);
        }

        public void SendTrees(int Server)
        {
            var Packet = new SendTreesRequest();
            Packet.Server = Server;

            Socket.Send(Packet);
        }

        public void SendNPCs(int Server)
        {
            var Packet = new SendNPCsRequest();
            Packet.Server = Server;

            Socket.Send(Packet);
        }

        public void SendBanRequest(object State, int Type, string Name)
        {
            var Packet = new BanRequest();
            Packet.Type = Type;
            Packet.Name = Name;

            Socket.Send(Packet);
            States.Enqueue(State);
        }
    }
}