#if !(UNITY_BUILD || UNITY_STANDALONE || UNITY_EDITOR)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;
using Base.Helpers;

namespace Base.Factories
{
    public class ControllerFactory : ISingleton, IComponent, IUpdater
    {
        private Dictionary<string, IBaseController> BaseControllers;

        public static Type BaseControllerType { get; set; }
        public int Interval { get; set; }

        public void Create()
        {
            BaseControllers = new Dictionary<string, IBaseController>();
        }

        public void Destroy()
        {
            
        }

        /*public static bool RegisterController<TController>() where TController : IController
        {
            ControllerFactory Factory = SingletonFactory.GetInstance<ControllerFactory>();
            int ID = typeof(TController).GetHashCode();
            
            if(!Factory.Controllers.ContainsKey(ID))
            {
                IBaseController BaseController = (IBaseController)Activator.CreateInstance(BaseControllerType);
                Factory.BaseControllers.Add(ID, BaseController);

                IController Controller = Activator.CreateInstance<TController>();
                Factory.Controllers.Add(ID, Controller);

                BaseController.Controller = Controller;

                return BaseController.Open();
            }

            return true;
        }*/

        public static void RegisterController<TModel>(params string[] TableNames) where TModel : IModel
        {
            ControllerFactory Factory = SingletonFactory.GetInstance<ControllerFactory>();

            foreach (var TableName in TableNames)
            {
                if (!Factory.BaseControllers.ContainsKey(TableName))
                {
                    IBaseController BaseController = (IBaseController)Activator.CreateInstance(BaseControllerType);
                    BaseController.TableName = TableName;
                    BaseController.ModelType = typeof(TModel);
                    Factory.BaseControllers.Add(TableName, BaseController);                    
                }
            }
        }

        public static bool EnableControllers()
        {
            ControllerFactory Factory = SingletonFactory.GetInstance<ControllerFactory>();
            foreach(var Base in Factory.BaseControllers.Values)
            {
                if (!Base.Open())
                    return false;
            }
            return true;
        }

        public static IBaseController GetBaseController(string TableName)
        {
            ControllerFactory Factory = SingletonFactory.GetInstance<ControllerFactory>();
            IBaseController Controller = null;

            return Factory.BaseControllers.TryGetValue(TableName, out Controller) ? Controller : null;
        }

        void IUpdater.Start()
        {
            LoggerFactory.GetLogger(this).LogInfo("Controller Factory has been started! Updating all models on {0}....", new TimeSpan(0, 0, 0, 0, Interval));
        }

        void IUpdater.Loop()
        {
            SaveAll();
        }

        void SaveAll()
        {
            try
            {
                foreach (IBaseController BaseController in BaseControllers.Values)
                {
                    BaseController.SaveData();
                }
                LoggerFactory.GetLogger(this).LogSuccess("All controllers has been saved data!");
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
            }
        }

        void IUpdater.End()
        {
            SaveAll();
            BaseControllers.Clear();
        }

        public bool Enable()
        {
            foreach (string ID in BaseControllers.Keys)
            {
                IBaseController BaseController = BaseControllers[ID];
                if (!BaseController.LoadData())
                    return false;
            }
            UpdaterFactory.Start(this);
            return true;
        }

        public bool Disable()
        {
            UpdaterFactory.Stop(this);
            return false;
        }
    }
}
#endif