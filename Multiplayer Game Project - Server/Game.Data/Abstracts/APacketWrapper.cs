using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Base.Factories;
using Game.Data.Attributes;
using Network.Data.Interfaces;

namespace Game.Data.Abstracts
{
    public abstract class APacketWrapper
    {
        static volatile Dictionary<Type, List<PropertyInfo>> Properties;
        static volatile Dictionary<Type, Action<object, ISocketPacket>> TypeWriters;
        static volatile Dictionary<Type, Func<ISocketPacket, object>> TypeReaders;
        static object syncLock = new object();

        private void Load()
        {
            lock (syncLock)
            {
                if (TypeWriters == null)
                {
                    TypeWriters = new Dictionary<Type, Action<object, ISocketPacket>>();
                    TypeWriters.Add(typeof(float), (V, P) => P.WriteFloat((float)V));
                    TypeWriters.Add(typeof(double), (V, P) => P.WriteDouble((double)V));
                    TypeWriters.Add(typeof(short), (V, P) => P.WriteShort((short)V));
                    TypeWriters.Add(typeof(int), (V, P) => P.WriteInt((int)V));
                    TypeWriters.Add(typeof(long), (V, P) => P.WriteLong((long)V));
                    TypeWriters.Add(typeof(ushort), (V, P) => P.WriteUShort((ushort)V));
                    TypeWriters.Add(typeof(uint), (V, P) => P.WriteUInt((uint)V));
                    TypeWriters.Add(typeof(ulong), (V, P) => P.WriteULong((ulong)V));
                    TypeWriters.Add(typeof(char), (V, P) => P.WriteChar((char)V));
                    TypeWriters.Add(typeof(bool), (V, P) => P.WriteBool((bool)V));
                    TypeWriters.Add(typeof(decimal), (V, P) => P.WriteDecimal((decimal)V));
                    TypeWriters.Add(typeof(string), (V, P) => P.WriteString((string)(V ?? string.Empty)));
                    TypeWriters.Add(typeof(DateTime), (V, P) => P.WriteDateTime((DateTime)V));
                    TypeWriters.Add(typeof(TimeSpan), (V, P) => P.WriteTimeSpan((TimeSpan)V));
                    TypeWriters.Add(typeof(Guid), (V, P) => P.WriteGuid((Guid)V));
                }

                if (TypeReaders == null)
                {
                    TypeReaders = new Dictionary<Type, Func<ISocketPacket, object>>();
                    TypeReaders.Add(typeof(float), (P) => P.ReadFloat());
                    TypeReaders.Add(typeof(double), (P) => P.ReadDouble());
                    TypeReaders.Add(typeof(short), (P) => P.ReadShort());
                    TypeReaders.Add(typeof(int), (P) => P.ReadInt());
                    TypeReaders.Add(typeof(long), (P) => P.ReadLong());
                    TypeReaders.Add(typeof(ushort), (P) => P.ReadUShort());
                    TypeReaders.Add(typeof(uint), (P) => P.ReadUInt());
                    TypeReaders.Add(typeof(ulong), (P) => P.ReadULong());
                    TypeReaders.Add(typeof(char), (P) => P.ReadChar());
                    TypeReaders.Add(typeof(bool), (P) => P.ReadBool());
                    TypeReaders.Add(typeof(decimal), (P) => P.ReadDecimal());
                    TypeReaders.Add(typeof(string), (P) => P.ReadString());
                    TypeReaders.Add(typeof(DateTime), (P) => P.ReadDateTime());
                    TypeReaders.Add(typeof(TimeSpan), (P) => P.ReadTimeSpan());
                    TypeReaders.Add(typeof(Guid), (P) => P.ReadGuid());
                }

                if (Properties == null)
                    Properties = new Dictionary<Type, List<PropertyInfo>>();

                if (!Properties.ContainsKey(GetType()))
                {
                    Properties.Add(GetType(), new List<PropertyInfo>());
                    foreach (var T in GetType().GetProperties().Where(P => P.CanRead && P.CanWrite).OrderBy(P => P.DeclaringType == GetType()))
                    {
                        var Attr = T.GetCustomAttributes(typeof(NonWrapAttribute), true);

                        if (Attr == null || Attr.Length == 0)
                            if (T != null)
                                Properties[GetType()].Add(T);
                            else
                                LoggerFactory.GetLogger(this).LogWarning("Null property? {0}", T.Name);
                    }
                }
            }
        }

        public virtual void WritePacket(ISocketPacket Packet)
        {
            Load();

            lock (syncLock)
            {
                foreach (var Property in Properties[GetType()])
                {
                    object Value = Property.GetValue(this, null);
                    Action<object, ISocketPacket> Writer = null;

                    if (TypeWriters.TryGetValue(Property.PropertyType, out Writer))
                    {
                        Writer(Value, Packet);
                    }
                    else if (typeof(APacketWrapper).IsAssignableFrom(Property.PropertyType))
                    {
                        (Value as APacketWrapper).WritePacket(Packet);
                    }
                    else if (Property.PropertyType.IsEnum)
                    {
                        Packet.WriteEnum(Property.PropertyType, Value);
                    }
                    else
                        LoggerFactory.GetLogger(this).LogWarning("Invalid property type ({0} as {1}) on wrapper {2}!", Property.Name, Property.PropertyType.Name, GetType().Name);
                }
            }
        }

        public virtual void ReadPacket(ISocketPacket Packet)
        {
            Load();

            lock (syncLock)
            {
                foreach (var Property in Properties[GetType()])
                {
                    Func<ISocketPacket, object> Reader = null;
                    object Value = null;

                    if (TypeReaders.TryGetValue(Property.PropertyType, out Reader))
                    {
                        Value = Reader(Packet);
                    }
                    else if (typeof(APacketWrapper).IsAssignableFrom(Property.PropertyType))
                    {
                        Value = Activator.CreateInstance(Property.PropertyType);
                        (Value as APacketWrapper).ReadPacket(Packet);
                    }
                    else if (Property.PropertyType.IsEnum)
                    {
                        Value = Packet.ReadEnum(Property.PropertyType);
                    }
                    else
                        LoggerFactory.GetLogger(this).LogWarning("Invalid property type ({0} as {1}) on wrapper {2}!", Property.Name, Property.PropertyType.Name, GetType().Name);

                    //Se eu ler isso um dia, eu esqueci do código abaixo, e estou me sentindo bem burro neste momento
                    //At. Gabriel
                    if (Value != null)
                        Property.SetValue(this, Value, null);
                }
            }
        }

        public virtual void CopyTo(APacketWrapper Wrapper)
        {
            Load();

            lock (syncLock)
            {
                foreach (var Property in Properties[GetType()])
                {
                    var Value = Property.GetValue(this, null);
                    Property.SetValue(Wrapper, Value, null);
                }
            }
        }
    }
}