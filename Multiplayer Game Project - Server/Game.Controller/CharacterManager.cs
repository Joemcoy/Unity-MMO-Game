using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data.Models;
using Game.Data.Information;
using Base.Data.Interfaces;
using Base.Factories;
using Server.Configuration;

namespace Game.Controller
{
    public class CharacterManager : ISingleton
    {
        IBaseController Characters, CharacterPosition, CharacterCurrency, CharacterStats, CharacterStyle;

        void ISingleton.Create()
        {
            Characters = ControllerFactory.GetBaseController("characters");
            CharacterPosition = ControllerFactory.GetBaseController("character_position");
            CharacterCurrency = ControllerFactory.GetBaseController("character_currency");
            CharacterStats = ControllerFactory.GetBaseController("character_stats");
            CharacterStyle = ControllerFactory.GetBaseController("character_style");

            Characters.RegisterAfterLoadModelCallback(LoadPositions);
            Characters.RegisterAfterSaveModelCallback(SavePositions);
        }

        void ISingleton.Destroy()
        {

        }

        private void LoadPositions(IBaseController BaseController)
        {
            var Characters = BaseController.GetModels<CharacterModel>();
            foreach (CharacterModel Character in Characters)
            {
                Character.Position = CharacterPosition.GetModel<PositionModel>(Character.ID);
                Character.Stats = CharacterStats.GetModel<CharacterStatsModel>(Character.ID);
                Character.Style = CharacterStyle.GetModel<CharacterStyleModel>(Character.ID);
                Character.Currency = CharacterCurrency.GetModel<CharacterCurrencyModel>(Character.ID);
                                
                if (Character.Stats == null)
                {
                    Character.Stats = new CharacterStatsModel();
                    Character.Stats.ID = Character.ID;
                    Character.Stats.Level = GameConfiguration.InitialLevel;
                    Character.Stats.Experience = 0;
                    Character.Stats.Health = GameConfiguration.InitialHealth;
                    Character.Stats.MaxHealth = GameConfiguration.InitialHealth;
                    Character.Stats.Stamina = GameConfiguration.InitialStamina;
                    Character.Stats.Mana = GameConfiguration.InitialMana;
                    Character.Stats.SpeedMultipler = GameConfiguration.SpeedMultiplier;
                }

                if (Character.Style == null)
                {
                    Character.Style = new CharacterStyleModel();
                    Character.Style.ID = Character.ID;
                }

                if(Character.Currency == null)
                {
                    Character.Currency = new CharacterCurrencyModel();
                    Character.Currency.ID = Character.ID;
                    Character.Currency.Gold = GameConfiguration.InitialGold;
                    Character.Currency.Copper = GameConfiguration.InitialCopper;
                    Character.Currency.Silver = GameConfiguration.InitialSilver;
                    Character.Currency.Ruby = GameConfiguration.InitialRuby;
                }
            }
        }

        private void SavePositions(IBaseController BaseController, IModel Model)
        {
            CharacterModel Character = (CharacterModel)Model;

            Character.Position.ID = Character.ID;
            CharacterPosition.UpdateModel(Character.Position);

            Character.Stats.ID = Character.ID;
            CharacterStats.UpdateModel(Character.Stats);

            Character.ID = Character.ID;
            CharacterStyle.UpdateModel(Character.Style);

            Character.Currency.ID = Character.ID;
            Character.Currency.Gold = GameConfiguration.InitialGold;
            Character.Currency.Copper = GameConfiguration.InitialCopper;
            Character.Currency.Silver = GameConfiguration.InitialSilver;
            Character.Currency.Ruby = GameConfiguration.InitialRuby;

            CharacterCurrency.UpdateModel(Character.Currency);
        }

        public static void CreateCharacter(CharacterModel Character)
        {
            var Map = MapManager.GetMapByID(GameConfiguration.DefaultMapID);
            var Manager = SingletonFactory.GetInstance<CharacterManager>();

            Character.Position = Map.Spawn;
            Character.Position.ID = Character.ID;

            Character.Stats.ID = Character.ID;
            Character.Stats.Health = GameConfiguration.InitialHealth;
            Character.Stats.MaxHealth = GameConfiguration.InitialHealth;
            Character.Stats.Stamina = GameConfiguration.InitialStamina;
            Character.Stats.Mana = GameConfiguration.InitialMana;
            Character.Stats.SpeedMultipler = GameConfiguration.SpeedMultiplier;
            Character.Stats.Level = 1;
            Character.Stats.Experience = 0;

            Character.Currency.ID = Character.ID;
            Character.Currency.Gold = GameConfiguration.InitialGold;
            Character.Currency.Silver = GameConfiguration.InitialSilver;
            Character.Currency.Copper = GameConfiguration.InitialCopper;
            Character.Currency.Ruby = GameConfiguration.InitialRuby;

            Manager.Characters.AddModel(Character);

            var Slots = new Dictionary<int, uint>();
            foreach (var Item in CharacterStartItemsManager.GetItemsByClass(Character.Class))
            {
                var RealItem = ItemManager.GetItemByID(Item.ItemID);
                if (!Slots.ContainsKey(RealItem.GetInventoryID()))
                    Slots[RealItem.GetInventoryID()] = 0;

                var CharacterItem = new CharacterItemModel();
                CharacterItem.ItemID = Item.ItemID;
                CharacterItem.Amount = Item.Amount;
                CharacterItem.OwnerID = Character.ID;
                CharacterItem.Slot = Slots[RealItem.GetInventoryID()]++;
                CharacterItem.HotbarSlot = -1;
                CharacterItem.Serial = Guid.NewGuid();

                var ItemControl = ControllerFactory.GetBaseController("character_items");
                ItemControl.AddModel(CharacterItem);
            }
        }

        public static int CountCharacterByAID(int AccountID)
        {
            var Manager = SingletonFactory.GetInstance<CharacterManager>();
            return Manager.Characters.GetModels<CharacterModel>(C => C.AID == AccountID).Count();
        }

        public static CharacterModel GetCharacterByName(string Name)
        {
            var Manager = SingletonFactory.GetInstance<CharacterManager>();
            return Manager.Characters.GetModels<CharacterModel>(C => C.Name == Name).FirstOrDefault();
        }

        public static CharacterModel GetCharacterByID(int ID)
        {
            var Manager = SingletonFactory.GetInstance<CharacterManager>();
            return Manager.Characters.GetModel<CharacterModel>(ID);
        }

        public static CharacterModel[] GetCharactersByAID(int AccountID)
        {
            var Manager = SingletonFactory.GetInstance<CharacterManager>();
            return Manager.Characters.GetModels<CharacterModel>(C => C.AID == AccountID).ToArray();
        }

        public static void UpdateCharacter(CharacterModel Character)
        {
            var Manager = SingletonFactory.GetInstance<CharacterManager>();
            Manager.Characters.UpdateModel(Character);
        }
    }
}