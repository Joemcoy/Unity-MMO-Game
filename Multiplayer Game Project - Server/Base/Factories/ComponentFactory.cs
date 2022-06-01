using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Helpers;
using Base.Data.Interfaces;

namespace Base.Factories
{
    public class ComponentFactory : ISingleton
    {
        class ComponentState
        {
            public IComponent Component { get; set; }
            public bool Enabled { get; set; }

            public ComponentState(IComponent Component) : this(Component, false)
            {

            }

            public ComponentState(IComponent Component, bool Enabled)
            {
                this.Component = Component;
                this.Enabled = Enabled;
            }
        }

        private Dictionary<int, ComponentState> Components;

        public void Create()
        {
            Components = new Dictionary<int, ComponentState>();
        }

        public void Destroy()
        {
            foreach (int ID in Components.Keys)
            {
                ComponentState State = Components[ID];
                if (State.Enabled)
                {
                    IComponent Component = State.Component;

                    if (Component.Disable())
                    {
                        //LoggerFactory.GetLogger(this).LogSuccess("Component {0} disabled successfully!", Component.GetType().Name);
                    }
                    else
                    {
                        //LoggerFactory.GetLogger(this).LogWarning("Failed to disable component {0}!", Component.GetType().Name);
                    }
                }
            }
        }

        private static bool Enable(int ID)
        {
            try
            {
                ComponentFactory Factory = SingletonFactory.GetInstance<ComponentFactory>();
                if (Factory.Components.ContainsKey(ID))
                {
                    if (!Factory.Components[ID].Enabled)
                    {
                        ComponentState State = Factory.Components[ID];
                        if (State.Component.Enable())
                        {
                            Factory.Components[ID].Enabled = true;

                            LoggerFactory.GetLogger(Factory).LogSuccess("Component {0} enabled successfully!", State.Component.GetType().Name);
                            return true;
                        }
                        else
                        {
                            LoggerFactory.GetLogger(Factory).LogWarning("Failed to enable component {0}!", State.Component.GetType().Name);
                            Factory.Components.Remove(ID);
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    LoggerFactory.GetLogger(Factory).LogWarning("??");
                    return false;
                }
            }
            catch(Exception ex)
            {
                LoggerFactory.GetLogger<ComponentFactory>().LogFatal(ex);
                return false;
            }
        }

        public static bool Enable(Type ComponentType)
        {
            var ID = ComponentType.GetHashCode();
            ComponentFactory Factory = SingletonFactory.GetInstance<ComponentFactory>();

            if (!Factory.Components.ContainsKey(ID) || !Factory.Components[ID].Enabled)
            {
                Factory.Components[ID] = new ComponentState((IComponent)InstanceHelper.GetInstance(ComponentType));
                return Enable(ID);
            }
            else
                return true;
        }

        public static bool Enable<TComponent>() where TComponent : IComponent
        {
            return Enable(typeof(TComponent));
        }

        public static bool Enable(IComponent Component)
        {
            var ID = Component.GetHashCode();
            ComponentFactory Factory = SingletonFactory.GetInstance<ComponentFactory>();

            if (!Factory.Components.ContainsKey(ID) || !Factory.Components[ID].Enabled)
            {
                Factory.Components[ID] = new ComponentState(Component);
                return Enable(ID);
            }
            else
                return true;
        }

        private static bool Disable(int ID)
        {
            try
            {
                ComponentFactory Factory = SingletonFactory.GetInstance<ComponentFactory>();

                if (Factory.Components.ContainsKey(ID))
                {
                    if (Factory.Components[ID].Enabled)
                    {
                        IComponent Component = Factory.Components[ID].Component;
                        if (Component.Disable())
                        {
                            Factory.Components[ID].Enabled = false;
                            //LoggerFactory.GetLogger(Factory).LogInfo("Component {0} disabled successfully!", Component.GetType().Name);
                            return true;
                        }
                        else
                        {
                            //LoggerFactory.GetLogger(Factory).LogWarning("Failed to disable component {0}!", Component.GetType().Name);
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger<ComponentFactory>().LogFatal(ex);
                return false;
            }
        }

        public static bool Disable(Type ComponentType)
        {
            var ID = ComponentType.GetHashCode();
            ComponentFactory Factory = SingletonFactory.GetInstance<ComponentFactory>();

            if (!Factory.Components.ContainsKey(ID))
            {
                return false;
            }
            else
            {
                return Disable(ID);
            }
        }

        public static bool Disable<TComponent>() where TComponent : IComponent
        {
            return Disable(typeof(TComponent));
        }

        public static bool Disable(IComponent Component)
        {
            var ID = Component.GetHashCode();
            ComponentFactory Factory = SingletonFactory.GetInstance<ComponentFactory>();

            if (!Factory.Components.ContainsKey(ID))
            {
                return false;
            }
            else
            {
                return Enable(ID);
            }
        }

        private static void RemoveComponent(int ID)
        {
            try
            {
                ComponentFactory Factory = SingletonFactory.GetInstance<ComponentFactory>();
                if (Factory.Components.ContainsKey(ID))
                {
                    var State = Factory.Components[ID];
                    if (State.Enabled)
                    {
                        if (State.Component.Disable())
                        {
                            //LoggerFactory.GetLogger(Factory).LogInfo("Component {0} disabled successfully!", State.Component.GetType().Name);
                        }
                        else
                        {
                            //LoggerFactory.GetLogger(Factory).LogWarning("Failed to disable component {0}!", State.Component.GetType().Name);
                        }
                    }
                    Factory.Components.Remove(ID);
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger<ComponentFactory>().LogFatal(ex);
            }
        }

        public static void RemoveComponent(Type ComponentType)
        {
            if (typeof(IComponent).IsAssignableFrom(ComponentType))
            {
                var ID = ComponentType.GetHashCode();
                RemoveComponent(ID);
            }
        }

        public static void RemoveComponent<TComponent>() where TComponent : IComponent
        {
            RemoveComponent(typeof(TComponent));
        }

        public static void RemoveComponent(IComponent Component)
        {
            var ID = Component.GetHashCode();
            RemoveComponent(ID);
        }
    }
}