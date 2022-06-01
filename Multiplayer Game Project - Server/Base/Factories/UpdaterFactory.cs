using System;
using System.Linq;
using System.Collections.Generic;

using Base.Helpers;
using Base.Data.Exceptions;
using Base.Data.Interfaces;
using Base.Factories.Tasks;
using Base.Data.Abstracts;

namespace Base.Factories
{
    public class UpdaterFactory : ISingleton
    {
        Dictionary<int, UpdaterTask> Tasks;
        object syncLock;

        public static IUpdater[] Updaters
        {
            get
            {
                return SingletonFactory.GetInstance<UpdaterFactory>().Tasks.Values.Select(C => C.Updater).ToArray();
            }
        }

        public void Create()
        {
            Tasks = new Dictionary<int, UpdaterTask>();
            syncLock = new object();
        }

        public void Destroy()
        {
            lock (syncLock)
            {
                foreach (UpdaterTask Task in Tasks.Values.Where(V => V.Running))
                    Task.Stop();
                Tasks.Clear();
            }
            GC.WaitForPendingFinalizers();
        }

        public static void Start<TUpdater>()
            where TUpdater : IUpdater
        {
            Start(typeof(TUpdater));
        }

        public static void Start(Type UpdaterType)
        {
            if (typeof(IUpdater).IsAssignableFrom(UpdaterType))
            {
                var Factory = SingletonFactory.GetInstance<UpdaterFactory>();
                var ID = UpdaterType.GetHashCode();

                if (!Factory.Tasks.ContainsKey(ID))
                {
                    lock (Factory.syncLock)
                        Factory.Tasks.Add(ID, new UpdaterTask((IUpdater)InstanceHelper.GetInstance(UpdaterType)));
                }

                Start(ID);
            }
            else
                throw new NotImplementedInterfaceException(UpdaterType, typeof(IUpdater));
        }

        public static void Start(IUpdater Updater)
        {
            var Factory = SingletonFactory.GetInstance<UpdaterFactory>();
            var ID = Updater.GetHashCode();

            if (!Factory.Tasks.ContainsKey(ID))
            {
                lock (Factory.syncLock)
                    Factory.Tasks.Add(ID, new UpdaterTask(Updater));
            }

            Start(ID);
        }

        static void Start(int ID)
        {
            var Factory = SingletonFactory.GetInstance<UpdaterFactory>();
            var Task = Factory.Tasks[ID];

            if (!Task.Running)
                Task.Start();
            else
                throw new Exception("Updater as already started!");
        }

        public static void Stop<TUpdater>()
            where TUpdater : IUpdater
        {
            Stop(typeof(TUpdater));
        }

        public static void Stop(Type UpdaterType)
        {
            if (typeof(IUpdater).IsAssignableFrom(UpdaterType))
            {
                Stop(UpdaterType.GetHashCode());
            }
            else
                throw new NotImplementedInterfaceException(UpdaterType, typeof(IUpdater));
        }

        public static void Stop(IUpdater Updater)
        {
            Stop(Updater.GetHashCode());
        }

        static void Stop(int ID)
        {
            var Factory = SingletonFactory.GetInstance<UpdaterFactory>();

            if (Factory.Tasks.ContainsKey(ID))
            {
                var Task = Factory.Tasks[ID];

                if (Task.Running)
                    Task.Stop();
            }
            else
                throw new Exception("Updater as not been created!");
        }

        public static bool HasStarted<TUpdater>()
            where TUpdater:IUpdater
        {
            return HasStarted(typeof(TUpdater));
        }

        public static bool HasStarted(Type UpdaterType)
        {
            if (typeof(IUpdater).IsAssignableFrom(UpdaterType))
            {
                return HasStarted(UpdaterType.GetHashCode());
            }
            else
                throw new NotImplementedInterfaceException(UpdaterType, typeof(IUpdater));
        }

        public static bool HasStarted(IUpdater Updater)
        {
            return HasStarted(Updater.GetHashCode());
        }

        static bool HasStarted(int ID)
        {
            var Factory = SingletonFactory.GetInstance<UpdaterFactory>();

            return Factory.Tasks.ContainsKey(ID) && Factory.Tasks[ID].Running;
        }
    }
}