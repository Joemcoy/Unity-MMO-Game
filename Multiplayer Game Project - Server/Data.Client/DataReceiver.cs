//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Network.Data;using Network.Data.Interfaces;
//using Network.Data.Interfaces;

//using Game.Data;
//using Game.Data.Models;
//using Game.Data.Enums;
//using Base.Data.Enums;
//using Base.Factories;
//using System.Net;
//using Server.Data.EventArgs;

//namespace Data.Client
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
//            if (Packet.ID == PacketID.DataDataPacket.ID)
//            {
//                /*switch (Packet.Type)
//                {
//                    case PacketID.DataDataPacket.SendAccounts:
//                        return HandleSendAccounts(Packet);
//                    case PacketID.DataDataPacket.SendAccountByUsername:
//                        return HandleSendAccountByUsername(Packet);
//                    case PacketID.DataDataPacket.SendLogin:
//                        return HandleSendLogin(Packet);
//                    case PacketID.DataDataPacket.SendRegister:
//                        return HandleSendRegister(Packet);
//                    default:
//                        return false;
//                }*/
                
//                if(Client.DataResponses.Count > 0)
//                {
//                    Tuple<byte, byte, IPEndPoint> DataPacket = Client.DataResponses.Dequeue();

//                    byte ID = DataPacket.Item1, Type = DataPacket.Item2;
//                    IPEndPoint ClientEndPoint = DataPacket.Item3;
//                    LoggerFactory.GetLogger(this).LogInfo($"Executing packet {0},{1} to client {2}!", ID, Type, StateEndPoint);

//                    Client.FireDataCallback(new DataPacketEventArgs(ID, Type, StateEndPoint, Packet));
//                }

//                return true;
//            }
//            return false;
//        }

//        private bool HandleSendAccounts(ISocketPacket Packet)
//        {
//            int Total = Packet.ReadInt();
//            for(int i = 0; i < Total; i++)
//            {
//                AccountModel Model = new AccountModel();
//                Model.ID = Packet.ReadInt();
//                Model.Username = Packet.ReadString();
//                Model.Password = Packet.ReadString();
//                Model.Nickname = Packet.ReadString();
//                Model.Cash = Packet.ReadInt();
//                Model.Access = (AccessLevel)Packet.ReadByte();
//                Model.LoginCount = Packet.ReadInt();
//                Model.RegisterDate = new DateTime(Packet.ReadLong());

//                LoggerFactory.GetLogger(this).LogInfo($"-- {0}", Model.ID);
//                LoggerFactory.GetLogger(this).LogInfo($"--- Username: {0}", Model.Username);
//                LoggerFactory.GetLogger(this).LogInfo($"--- Nickname: {0}", Model.Nickname);
//                LoggerFactory.GetLogger(this).LogInfo($"--- Cash: {0}", Model.Cash);
//                LoggerFactory.GetLogger(this).LogInfo($"--- Access: {0}", Model.Access);
//                LoggerFactory.GetLogger(this).LogInfo($"--- Login Count: {0}", Model.LoginCount);
//                LoggerFactory.GetLogger(this).LogInfo($"--- Register Date: {0}", Model.RegisterDate);
//            }
//            return true;
//        }

//        private bool HandleSendAccountByUsername(ISocketPacket Packet)
//        {
//            if (Packet.ReadBool())
//            {
//                AccountModel Model = new AccountModel();
//                Model.ID = Packet.ReadInt();
//                Model.Username = Packet.ReadString();
//                Model.Password = Packet.ReadString();
//                Model.Nickname = Packet.ReadString();
//                Model.Cash = Packet.ReadInt();
//                Model.Access = (AccessLevel)Packet.ReadByte();
//                Model.LoginCount = Packet.ReadInt();
//                Model.RegisterDate = new DateTime(Packet.ReadLong());

//                return true;
//            }
//            return false;
//        }

//        private bool HandleSendLogin(ISocketPacket Packet)
//        {
//            throw new NotImplementedException();
//        }

//        private bool HandleSendRegister(ISocketPacket Packet)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
