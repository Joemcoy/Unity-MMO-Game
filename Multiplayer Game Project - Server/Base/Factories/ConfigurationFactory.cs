using System;
using System.Collections.Generic;
using System.IO;

using Base.Data.Interfaces;
using Base.Data.Abstracts;
using Base.Data.Exceptions;
using Base.Data.DispatcherBases;
using Base.Helpers;

namespace Base.Factories
{
    public class ConfigurationFactory : ADispatcher<IConfigurationDispatcher>, IUpdater, ISingleton
    {
        Dictionary<Type, IConfiguration> Configurations;
        object syncLock;

        public int Interval { get; private set; }

        public void Create()
        {
			if (Interval == 0)
				Interval = 10000;
			
            Configurations = new Dictionary<Type, IConfiguration>();
            syncLock = new object();

            UpdaterFactory.Start(this);
        }

        public void Destroy()
        {
            UpdaterFactory.Stop(this);
        }

        public void Start()
        {

        }

        public void Loop()
        {
            End();
        }

        public void End()
        {
            lock (syncLock)
            {
                foreach (var Configuration in Configurations.Values)
                {
                    DispatchBase(d => d.Save(Configuration));
                }
            }
        }

        public static bool RegisterConfiguration<TConfiguration>()
            where TConfiguration : IConfiguration
        {
            return RegisterConfiguration(typeof(TConfiguration));
        }

        public static bool RegisterConfiguration(Type ConfigurationType)
        {
            if (typeof(IConfiguration).IsAssignableFrom(ConfigurationType))
            {
                var Factory = SingletonFactory.GetInstance<ConfigurationFactory>();

                if (Factory.Count > 0)
                {
                    var Configuration = (IConfiguration)InstanceHelper.GetInstance(ConfigurationType);

                    Factory.DispatchBase(d => d.Load(Configuration));
                    Factory.Configurations.Add(ConfigurationType, Configuration);


                    return true;
                }
                return false;
			}
            throw new NotImplementedInterfaceException(ConfigurationType, typeof(IConfiguration));
        }
    }
}