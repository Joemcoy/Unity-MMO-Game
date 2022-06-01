using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using Base.Data.Interfaces;
using Server.Configuration;

using Network.v1;
using Network.Data.Interfaces;
using Network.Data.Dispatchers;

using Base.Factories;
using Data.Client;
using Game.Controller;
using Data.Server.BaseControllers;
using Network.Data.EventArgs;
using Base.Configurations;
using Game.Data.Models;
using Network.Bases;
using System.Reflection;

namespace Data.Server
{
    public class DataServer : ServerBase<DataClient>, ISingleton, IComponent
    {
        Dictionary<IPEndPoint, DataClient> ClientDict = null;
        protected override Assembly ResponsesAssembly { get { return typeof(DataServer).Assembly; } }

        public void Create()
        {
            ControllerFactory.BaseControllerType = typeof(BaseMysqlController);
            ClientDict = new Dictionary<IPEndPoint, DataClient>();
        }       

        public void Destroy()
        {

        }

        public bool Enable()
        {
            try
            {
                if (!ComponentFactory.Enable<GameConfiguration>())
                    return false;
                else if (!ComponentFactory.Enable<PortsConfiguration>())
                    return false;
                else if (!ComponentFactory.Enable<IntervalConfiguration>())
                    return false;
                else
                {
                    SingletonFactory.GetInstance<ControllerFactory>().Interval = IntervalConfiguration.ControllerInterval;

                    ControllerFactory.RegisterController<AccountModel>("accounts");
                    ControllerFactory.RegisterController<PositionModel>("character_position", "map_spawns", "world_item_position", "drop_position");
                    ControllerFactory.RegisterController<CharacterStatsModel>("character_stats");
                    ControllerFactory.RegisterController<CharacterStyleModel>("character_style");
                    ControllerFactory.RegisterController<CharacterCurrencyModel>("character_currency");
                    ControllerFactory.RegisterController<CharacterStartItemModel>("character_start_items");
                    ControllerFactory.RegisterController<CharacterModel>("characters");
                    ControllerFactory.RegisterController<CharacterItemModel>("character_items");    
                    ControllerFactory.RegisterController<MapModel>("maps");
                    ControllerFactory.RegisterController<MessageModel>("chat_log");
                    ControllerFactory.RegisterController<ItemModel>("items");
                    ControllerFactory.RegisterController<WorldItemGroupModel>("world_item_group");
                    ControllerFactory.RegisterController<WorldItemModel>("world_items");                        
                    ControllerFactory.RegisterController<NPCModel>("npcs");
                    ControllerFactory.RegisterController<NPCPositionModel>("npc_spawns");
                    ControllerFactory.RegisterController<VendorItemModel>("npc_vendor_items");
                    ControllerFactory.RegisterController<LauncherFileModel>("launcher_files");
                    ControllerFactory.RegisterController<DropModel>("drops");
                    ControllerFactory.RegisterController<MobModel>("mobs");
                    ControllerFactory.RegisterController<MobPositionModel>("mob_spawns");
                    ControllerFactory.RegisterController<TreeModel>("trees");          
                    ControllerFactory.RegisterController<TreePositionModel>("tree_spawns");

                    if (ControllerFactory.EnableControllers())
                    {
                        //Hack do tDarkFall pra registrar os gerenciadores
                        foreach (var Type in typeof(AccountManager).Assembly.GetTypes().Where(T => typeof(ISingleton).IsAssignableFrom(T)))
                        {
                            SingletonFactory.GetInstance(Type);
                        }

                        if (!ComponentFactory.Enable<ControllerFactory>())
                            return false;
                        else
                        {
                            Socket.EndPoint.Port = PortsConfiguration.DataPort;

                            return Socket.Open();
                        }
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }

        public bool Disable()
        {
            try
            {
                Socket.Close();
                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }
    }
}