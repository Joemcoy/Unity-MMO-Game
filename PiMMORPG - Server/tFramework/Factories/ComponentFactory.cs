using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Factories
{
    using Interfaces;
    using Helper;
    using Exceptions;

    public class ComponentFactory : ISingleton
    {
        class ComponentState
        {
            public IComponent Component { get; set; }
            public bool Enabled { get; set; }

            public ComponentState()
            {
                Enabled = false;
            }

            bool SetState(bool target)
            {
                if (target ? (!Enabled && Component.Enable()) : (Enabled && Component.Disable()))
                {
                    Enabled = target;
                    return true;
                }
                else
                    return false;
            }

            public bool Enable() { return SetState(true); }
            public bool Disable() { return SetState(false); }
        }

        Dictionary<int, ComponentState> _components;
        static volatile object _syncLock = new object();
        static ILogger Logger { get { return LoggerFactory.GetLogger<ComponentFactory>(); } }

        void ISingleton.Created()
        {
            _components = new Dictionary<int, ComponentState>();
        }

        void ISingleton.Destroyed()
        {
            lock (_syncLock)
            {
                foreach (ComponentState state in _components.Values)
                    if (!state.Disable())
                        Logger.LogError("Failed to disable component {0}!", IdHelper.GetName(state.Component));
                    else
                        Logger.LogSuccess("Component {0} disabled successfully!", IdHelper.GetName(state.Component));
            }
        }

        public static bool Enable<TComponent>() where TComponent : class, IComponent, new()
        { return Enable(typeof(TComponent)); }

        public static bool Enable(Type componentType)
        {
            NotAssignableException<IComponent>.Test(componentType);
            return Enable(componentType.GetHashCode(), (IComponent)SingletonFactory.GetSafeSingleton(componentType));
        }

        public static bool Enable(IComponent component)
        { return Enable(component.GetHashCode(), component); }

        private static bool Enable(int id, IComponent component)
        {
            try
            {
                var factory = SingletonFactory.GetSingleton<ComponentFactory>();

                ComponentState state;
                if (!factory._components.TryGetValue(id, out state))
                    factory._components.Add(id, state = new ComponentState { Component = component });

                if(state.Enabled)
                {
                    Logger.LogWarning("Component {0} as already enabled!", IdHelper.GetName(state.Component));
                    return true;
                }
                else if(state.Enable())
                {
                    Logger.LogSuccess("Component {0} as been enabled!", IdHelper.GetName(state.Component));
                    return true;
                }
                else
                {
                    Logger.LogWarning("Failed to enable the component {0}!", IdHelper.GetName(state.Component));
                    return false;
                }
            }
            catch(Exception ex)
            {
                LoggerFactory.GetLogger<ComponentFactory>().LogFatal(ex);
                return false;
            }
        }

        public static bool Disable<TComponent>() where TComponent : class, IComponent, new()
        { return Disable(typeof(TComponent)); }

        public static bool Disable(Type componentType)
        {
            NotAssignableException<IComponent>.Test(componentType);
            return Disable(componentType.GetHashCode(), (IComponent)SingletonFactory.GetSafeSingleton(componentType));
        }

        public static bool Disable(IComponent component)
        { return Disable(component.GetHashCode(), component); }

        private static bool Disable(int id, IComponent component)
        {
            try
            {
                var factory = SingletonFactory.GetSingleton<ComponentFactory>();

                ComponentState state;
                if (!factory._components.TryGetValue(id, out state))
                {
                    Logger.LogWarning("Component {0} as not been initalized!", IdHelper.GetName(component));
                    return true;
                }
                else if (!state.Enabled)
                {
                    Logger.LogWarning("Component {0} as already disabled!", IdHelper.GetName(state.Component));
                    return true;
                }
                else if (state.Disable())
                {
                    Logger.LogSuccess("Component {0} as been disabled!", IdHelper.GetName(state.Component));
                    return true;
                }
                else
                {
                    Logger.LogWarning("Failed to disable the component {0}!", IdHelper.GetName(state.Component));
                    return false;
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger<ComponentFactory>().LogFatal(ex);
                return false;
            }
        }

        public static TComponent RegisterComponent<TComponent>(TComponent component) where TComponent : IComponent
        {
            var registered = RegisterComponent(typeof(TComponent), component);
            return registered == null ? default(TComponent) : (TComponent)registered;
        }

        public static IComponent RegisterComponent(Type componentType, IComponent component)
        {
            var id = componentType.GetHashCode();
            var factory = SingletonFactory.GetSingleton<ComponentFactory>();

            ComponentState state;
            if (!factory._components.TryGetValue(id, out state))
            {
                factory._components[id] = new ComponentState { Component = component };
                return component;
            }
            else
            {
                Logger.LogWarning("Component {0} already has been registered!", componentType.Name);
                return state.Component;
            }
        }

        public static TComponent GetComponent<TComponent>() where TComponent : IComponent
        { return (TComponent)GetComponent(typeof(TComponent)); }

        public static IComponent GetComponent(Type componentType)
        {
            NotAssignableException<IComponent>.Test(componentType);

            var id = componentType.GetHashCode();
            var factory = SingletonFactory.GetSingleton<ComponentFactory>();

            ComponentState state;
            return factory._components.TryGetValue(id, out state) ? state.Component : null;
        }

        public static bool IsEnabled<TComponent>() where TComponent : IComponent
        { return IsEnabled(typeof(TComponent)); }

        public static bool IsEnabled(Type componentType)
        {
            NotAssignableException<IComponent>.Test(componentType);

            var id = componentType.GetHashCode();
            return IsEnabled(id);
        }

        public static bool IsEnabled(IComponent component)
        { return IsEnabled(component.GetHashCode()); }

        static bool IsEnabled(int id)
        {
            var factory = SingletonFactory.GetSingleton<ComponentFactory>();

            ComponentState state;
            return factory._components.TryGetValue(id, out state) ? state.Enabled : false;
        }
    }
}