using System;
using System.Collections.Generic;

using Base.Data.Interfaces;
using Base.Data.Exceptions;
using Base.Data.Abstracts;

namespace Base.Factories
{
    public class QueueFactory : ASingleton<QueueFactory>
    {
        Dictionary<int, Queue<object>> Queues;
        static object syncLock = new object();

        protected override void Created()
        {
            Queues = new Dictionary<int, Queue<object>>();
        }

        protected override void Destroyed()
        {
            lock (syncLock)
            {
                Queues.Clear();
                Queues = null;

                GC.WaitForPendingFinalizers();
            }
        }

        public static void Enqueue<TQueue>(object Value)
            where TQueue : IQueue
        {
            Enqueue(typeof(TQueue), Value);
        }

        public static void Enqueue(Type QueueType, object Value)
        {
            if (typeof(IQueue).IsAssignableFrom(QueueType))
            {
                Enqueue(QueueType.GetHashCode(), Value);
            }
            else
                throw new NotImplementedInterfaceException(QueueType, typeof(IQueue));
        }

        public static void Enqueue(IQueue Queue, object Value)
        {
            Enqueue(Queue.GetHashCode(), Value);
        }

        static void Enqueue(int ID, object Value)
        {
            if (!Instance.Queues.ContainsKey(ID))
                Instance.Queues.Add(ID, new Queue<object>());

            Instance.Queues[ID].Enqueue(Value);
        }

        public static object Dequeue<TQueue>()
            where TQueue : IQueue
        {
            return Dequeue(typeof(TQueue));
        }

        public static object Dequeue(Type QueueType)
        {
            if (typeof(IQueue).IsAssignableFrom(QueueType))
            {
                return Dequeue(QueueType.GetHashCode());
            }
            throw new NotImplementedInterfaceException(QueueType, typeof(IQueue));
        }

        public static object Dequeue(IQueue Queue)
        {
            return Dequeue(Queue.GetHashCode());
        }

        static object Dequeue(int ID)
        {
            if (Instance.Queues.ContainsKey(ID) && Instance.Queues[ID].Count > 0)
            {
                lock (syncLock)
                    return Instance.Queues[ID].Dequeue();
            }
            return null;
        }

        public static TValue Dequeue<TQueue, TValue>()
            where TQueue : IQueue
        {
            object Value = Dequeue<TQueue>();
            return Value == null || !(Value is TValue)
                ? default(TValue) : (TValue)Value;
        }

        public static TValue Dequeue<TValue>(IQueue Queue)
        {
            object Value = Dequeue(Queue);
            return Value == null || !(Value is TValue)
                ? default(TValue) : (TValue)Value;
        }

        public static void Clear(Type QueueType)
        {
            if (typeof(IQueue).IsAssignableFrom(QueueType))
            {
                Clear(QueueType.GetHashCode());
            }
            else
            {
                throw new NotImplementedInterfaceException(QueueType, typeof(IQueue));
            }
        }

        public static void Clear<TQueue>()
            where TQueue: IQueue
        {
            Clear(typeof(TQueue));
        }

        public static void Clear(IQueue Queue)
        {
            Clear(Queue.GetHashCode());
        }

        static void Clear(int ID)
        {
            if (Instance.Queues.ContainsKey(ID))
            {
                lock (syncLock)
                    Instance.Queues[ID].Clear();
            }
        }
    }
}