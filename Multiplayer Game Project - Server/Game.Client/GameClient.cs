using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.v1;
using Network.Data.Interfaces;


using Base.Factories;
using Game.Data.Models;
using System.Net;
using Network.Data;
using Game.Data;
using Base.Data.Interfaces;

using Network.Data.Enums;
using Network.Data.Dispatchers;
using Network.Data.EventArgs;
using Base.Data.Abstracts;
using Game.Data.Enums;
using Base.Configurations;
using Network.Bases;

namespace Game.Client
{
    public class GameClient : ClientBase<GameClient>
#if UNITY_BUILD || UNITY_STANDALONE || UNITY_EDITOR
        , ISingleton
#endif
    {
        private Dictionary<ItemType, CharacterItemModel> EquipedItems;
        private List<CharacterItemModel> cItems;

        public AccountModel Account { get; set; }
        public CharacterModel[] Characters { get; set; }

        public CharacterModel CurrentCharacter { get; set; }
        public MapModel CurrentMap { get; set; }

        public GameClient()
        {
            cItems = new List<CharacterItemModel>();
            EquipedItems = new Dictionary<ItemType, CharacterItemModel>();
        }

#if !UNITY_5
        public DateTime? LastChatTime { get; set; }
        public DateTime? LastAudioTime { get; set; }
        public DateTime? LastGodAudioTime { get; set; }
#endif

#if !UNITY_5
        public bool CanSendMessage(MessageType Type, ref int Total)
        {
            if (Account.IsBanned)
                return false;
            else if (LastChatTime == null || Account.Access == AccessLevel.Administrator)
                return true;
            else
            {
                var Differ = (DateTime.Now - LastChatTime.Value).Seconds;
                switch (Type)
                {
                    case MessageType.Shout:
                        Total = IntervalConfiguration.ShoutInterval - Differ;
                        return Differ > IntervalConfiguration.ShoutInterval;
                    default:
                        Total = IntervalConfiguration.MessageInterval - Differ;
                        return Differ > IntervalConfiguration.MessageInterval;
                }
            }
        }

        public bool CanSendAudio(byte Type, ref int Total)
        {
            switch(Type)
            {
                case 0:
                    return CanSendNormalAudio(ref Total);
                case 1:
                    return CanSendGodAudio(ref Total);
                default:
                    return false;
            }
        }

        public bool CanSendNormalAudio(ref int Total)
        {
            LoggerFactory.GetLogger(this).LogWarning("TN: {0}", LastAudioTime.HasValue ? LastAudioTime.Value.ToString() : "NULL");
            if (Account.IsBanned)
                return false;
            else if (!LastAudioTime.HasValue || Account.Access == AccessLevel.Administrator)
            {
                LastAudioTime = DateTime.Now;
                return true;
            }
            else
            {
                var Differ = (DateTime.Now - LastAudioTime.Value).Seconds;
                LoggerFactory.GetLogger(this).LogWarning("D: {0}", Differ);

                Total = IntervalConfiguration.AudioInterval - Differ;
                if (Differ > IntervalConfiguration.AudioInterval)
                {
                    LastAudioTime = new DateTime?(DateTime.Now);
                    return true;
                }
                return false;
            }
        }

        public bool CanSendGodAudio(ref int Total)
        {
            LoggerFactory.GetLogger(this).LogWarning("TG: {0}", LastGodAudioTime);
            if (Account.IsBanned)
                return false;
            else if (LastGodAudioTime == null || Account.Access == AccessLevel.Administrator)
            {
                LastGodAudioTime = DateTime.Now;
                return true;
            }
            else
            {
                var Differ = (DateTime.Now - LastGodAudioTime.Value).Seconds;
                LoggerFactory.GetLogger(this).LogWarning("D: {0}", Differ);

                Total = IntervalConfiguration.AudioInterval - Differ;
                if (Differ > IntervalConfiguration.AudioInterval)
                {
                    LastGodAudioTime = DateTime.Now;
                    return true;
                }
                return false;
            }
        }
#else
        void ISingleton.Create() { }
        void ISingleton.Destroy() { }
#endif

        public bool HasEquiped(ItemType Type)
        {
            return EquipedItems.ContainsKey(Type);
        }

        public void AddItem(CharacterItemModel Item)
        {
            cItems.Add(Item);
        }

        public void RemoveItem(CharacterItemModel Item)
        {
            cItems.Remove(Item);
        }

        public void EquipItem(ItemType Type, CharacterItemModel Item)
        {
            EquipedItems[Type] = Item;
        }

        public void UnequipItem(ItemType Type)
        {
            EquipedItems.Remove(Type);
        }

        public CharacterItemModel[] Items { get { return cItems.ToArray(); } }
    }
}