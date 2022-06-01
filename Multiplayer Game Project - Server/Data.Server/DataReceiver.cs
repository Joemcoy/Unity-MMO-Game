//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Network.Data.Interfaces;
//using Network.Data;using Network.Data.Interfaces;
//using Game.Data;

//using Data.Client;
//using Game.Controller;
//using Game.Data.Models;
//using Game.Data.Results;

//namespace Data.Server
//{
//    public class DataReceiver : IClientReceiver
//    {
//        DataClient Client;

//        public DataReceiver(DataClient Client)
//        {
//            this.Client = Client;
//        }

//        public bool ExecutePacket(ISocketPacket Packet)
//        {
//            if(Packet.ID == PacketID.DataDataPacket.ID)
//            {
//                switch(Packet.Type)
//                {
//                /*case PacketID.DataDataPacket.SendAccounts:
//                        return HandleSendAccounts(Packet);*/
//                    case PacketID.DataDataPacket.SendAccountByUsername:
//                        return HandleSendAccountByUsername(Packet);
//                    case PacketID.DataDataPacket.SendAccountByID:
//                        return HandleSendAccountByAccountID(Packet);
//                    case PacketID.DataDataPacket.SendLogin:
//                        return HandleSendLogin(Packet);
//                    case PacketID.DataDataPacket.SendRegister:
//                        return HandleSendRegister(Packet);
//                    case PacketID.DataDataPacket.SendCharacters:
//                        return HandleSendCharacters(Packet);
//                    default:
//                        return false;
//                }
//            }
//            return false;
//        }

//        /*private bool HandleSendAccounts(ISocketPacket Packet)
//        {
//            AccountModel[] Models = AccountController.Models;
//            if(Packet.ReadBool()) //Maximum
//            {
//                Models = Models.Take(Packet.ReadInt()).ToArray();
//            }

//            Packet.Clear();

//            Packet.WriteInt(Models.Length);
//            foreach(AccountModel Model in Models)
//            {
//                Packet.WriteInt(Model.ID);
//                Packet.WriteString(Model.Username);
//                Packet.WriteString(Model.Password);
//                Packet.WriteString(Model.Nickname);
//                Packet.WriteInt(Model.Cash);
//                Packet.WriteByte((byte)Model.Access);
//                Packet.WriteInt(Model.LoginCount);
//                Packet.WriteLong(Model.RegisterDate.Ticks);
//            }

//            Client.Socket.Send(Packet);
//            return true;
//        }*/

//        private bool HandleSendAccountByUsername(ISocketPacket Packet)
//        {
//            string Username = Packet.ReadString();
//            Packet.Clear();

//            AccountModel Model = AccountController.GetAccountByUsername(Username);
//            if(Model != null)
//            {
//                Packet.WriteBool(true);

//                Packet.WriteInt(Model.ID);
//                Packet.WriteString(Model.Username);
//                Packet.WriteString(Model.Password);
//                Packet.WriteString(Model.Username);
//                Packet.WriteInt(Model.Cash);
//                Packet.WriteByte((byte)Model.Access);
//                Packet.WriteInt(Model.LoginCount);
//                Packet.WriteLong(Model.RegisterDate.Ticks);
//            }
//            else
//            {
//                Packet.WriteBool(false);
//            }
//            Client.Socket.Send(Packet);
//            return true;
//        }

//        private bool HandleSendAccountByAccountID(ISocketPacket Packet)
//        {
//            int ID = Packet.ReadInt();
//            Packet.Clear();

//            AccountModel Model = AccountController.GetAccountByID(ID);
//            if (Model != null)
//            {
//                Packet.WriteBool(true);

//                Packet.WriteInt(Model.ID);
//                Packet.WriteString(Model.Username);
//                Packet.WriteString(Model.Password);
//                Packet.WriteString(Model.Nickname);
//                Packet.WriteInt(Model.Cash);
//                Packet.WriteEnum(Model.Access);
//                Packet.WriteInt(Model.LoginCount);
//                Packet.WriteLong(Model.RegisterDate.Ticks);
//            }
//            else
//            {
//                Packet.WriteBool(false);
//            }
//            Client.Socket.Send(Packet);
//            return true;
//        }

//        private bool HandleSendLogin(ISocketPacket Packet)
//        {
//            string Username = Packet.ReadString();
//            string Password = Packet.ReadString();
//            Packet.Clear();

//            AccountModel Model = AccountController.GetAccountByUsername(Username);
//            if(Model == null)
//            {
//                Packet.WriteByte(LoginResult.InvalidUsername);
//            }
//            else if(Model.Password != Password)
//            {
//                Packet.WriteByte(LoginResult.InvalidPassword);
//            }
//            else
//            {
//                Packet.WriteByte(LoginResult.Success);
//                Packet.WriteInt(Model.ID);
//                Packet.WriteString(Username);
//                Packet.WriteString(Password);

//                Model.LoginCount++;
//            }
//            Client.Socket.Send(Packet);
//            return true;
//        }

//        private bool HandleSendRegister(ISocketPacket Packet)
//        {
//            string Username = Packet.ReadString();
//            string Password = Packet.ReadString();
//            string Nickname = Packet.ReadString();
//            Packet.Clear();

//            if(AccountController.CheckUser(Username, Nickname))
//            {
//                Packet.WriteByte(RegisterResult.InvalidRegister);
//            }
//            else
//            {
//                AccountController.RegisterAccount(Username, Password, Nickname);
//                Packet.WriteByte(RegisterResult.Success);
//            }
//            Client.Socket.Send(Packet);
//            return true;
//        }

//        private bool HandleSendCharacters(ISocketPacket Packet)
//        {
//            int AccountID = Packet.ReadInt();
//            int GatePort = Packet.ReadInt();

//            CharacterModel[] Characters = CharacterController.GetCharactersByAID(AccountID, GatePort);

//            Packet.Clear();
//            Packet.WriteBool(Characters.Length > 0);

//            if (Characters.Length > 0)
//            {
//                Packet.WriteInt(Characters.Length);
//                foreach (CharacterModel Character in Characters)
//                {
//                    Packet.WriteInt(Character.ID);
//                    Packet.WriteInt(Character.AID);
//                    Packet.WriteInt(Character.GatePort);
//                    Packet.WriteString(Character.Name);
//                    //Packet.WriteInt(Character.Slot);
//                    Packet.WriteInt(Character.Level);
//                    Packet.WriteInt(Character.Experience);
//                    Packet.WriteBool(Character.IsFemale);
//                    Packet.WriteInt(Character.Class);
//                    Packet.WriteInt(Character.MapID);
//                    Packet.WriteFloat(Character.PositionX);
//                    Packet.WriteFloat(Character.PositionY);
//                    Packet.WriteFloat(Character.PositionZ);
//                    Packet.WriteFloat(Character.RotationX);
//                    Packet.WriteFloat(Character.RotationY);
//                    Packet.WriteFloat(Character.RotationZ);
//                }
//            }
//            Client.Socket.Send(Packet);

//            return true;
//        }

//        private bool HandleCreateCharacter(ISocketPacket Packet)
//        {
//            int AccountID = Packet.ReadInt();
//            int GatePort = Packet.ReadInt();

//            string Name = Packet.ReadString();
//            bool IsFemale = Packet.ReadBool();
//            Packet.Clear();

//            CharacterModel Character = CharacterController.GetCharacterByName(Name);
//            if(Character != null)
//            {
//                Packet.WriteByte(CreateCharacterResult.NameExists);
//            }
//            else
//            {
//                Character = new CharacterModel();
//                Character.Name = Name;
//                Character.IsFemale = IsFemale;
//                Character.AID = AccountID;
//                Character.GatePort = GatePort;

//                CharacterController.AddModel(Character);
//                Packet.WriteByte(CreateCharacterResult.Successful);
//            }
//            Client.Socket.Send(Packet);

//            return true;
//        }
//    }
//}
