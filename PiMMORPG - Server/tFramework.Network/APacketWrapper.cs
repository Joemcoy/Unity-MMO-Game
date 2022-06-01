using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using tFramework.Interfaces;
namespace tFramework.Network
{
    using Interfaces;
    using Factories;
    using System.Linq.Expressions;
    using tFramework.Helper;

    /*public abstract class APacketWrapper<T> : APacketWrapper
        where T : APacketWrapper, new()
    {
        new Type Target { get { return typeof(T); } }
        List<PropertyInfo> toIgnore = new List<PropertyInfo>();

        internal override IEnumerable<PropertyInfo> Properties
        {
            get
            {
                if (properties == null)
                    properties = base.Properties.Where(p => !toIgnore.Any(ip => ip.Name == p.Name));
                return properties;
            }
        }

        public void IgnoreProperty<TProperty>(Expression<Func<T, TProperty>> expr)
        {
            var property = ReflectionHelper.ExtractProperty(expr);
            toIgnore.Add(property);
        }
    }*/

    public abstract class APacketWrapper
    {
        public abstract void ReadPacket(IDataPacket packet);
        public abstract void WritePacket(IDataPacket packet);

        /*internal Type Target { get { return GetType(); } }
        ILogger logger { get { return LoggerFactory.GetLogger(this); } }

        internal IEnumerable<PropertyInfo> properties;
        internal virtual IEnumerable<PropertyInfo> Properties
        {
            get
            {
                if (properties == null)
                    properties = Target
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                return properties;
            }
        }

        public void WritePacket(IDataPacket packet)
        {
            foreach (var property in Properties)
            {
                var type = property.PropertyType;
                var value = property.GetValue(this, null);

                if (value == null && type.IsPrimitive)
                    value = Activator.CreateInstance(type);
                WriteValue(packet, type, value);
            }
        }

        bool WriteValue(IDataPacket packet, Type target, object value)
        {
            if (target == typeof(byte))
            {
                packet.WriteByte(Convert.ToByte(value));
                return true;
            }
            else if (target == typeof(sbyte))
            {
                packet.WriteSByte(Convert.ToSByte(value));
                return true;
            }
            else if (target == typeof(char))
            {
                packet.WriteChar(Convert.ToChar(value));
                return true;
            }
            else if (target == typeof(bool))
            {
                packet.WriteBool(Convert.ToBoolean(value));
                return true;
            }
            else if (target == typeof(ushort))
            {
                packet.WriteUShort(Convert.ToUInt16(value));
                return true;
            }
            else if (target == typeof(uint))
            {
                packet.WriteUInt(Convert.ToUInt32(value));
                return true;
            }
            else if (target == typeof(ulong))
            {
                packet.WriteULong(Convert.ToUInt64(value));
                return true;
            }
            else if (target == typeof(short))
            {
                packet.WriteShort(Convert.ToInt16(value));
                return true;
            }
            else if (target == typeof(int))
            {
                packet.WriteInt(Convert.ToInt32(value));
                return true;
            }
            else if (target == typeof(long))
            {
                packet.WriteLong(Convert.ToInt64(value));
                return true;
            }
            else if (target == typeof(double))
            {
                packet.WriteDouble(Convert.ToDouble(value));
                return true;
            }
            else if (target == typeof(float))
            {
                packet.WriteFloat(Convert.ToSingle(value));
                return true;
            }
            else if (target == typeof(string))
            {
                if (value == null) packet.WriteBool(false);
                else
                {
                    packet.WriteBool(true);
                    packet.WriteString(Convert.ToString(value));
                }
                return true;
            }
            else if (target == typeof(DateTime))
            {
                if (value == null) packet.WriteBool(false);
                else
                {
                    packet.WriteBool(true);
                    packet.WriteDateTime(Convert.ToDateTime(value));
                }
                return true;
            }
            else if (target == typeof(TimeSpan))
            {
                if (value == null) packet.WriteBool(false);
                else
                {
                    packet.WriteBool(true);
                    packet.WriteTimeSpan((TimeSpan)value);
                }
                return true;
            }
            else if (target == typeof(Guid))
            {
                if (value == null) packet.WriteBool(false);
                else
                {
                    packet.WriteBool(true);
                    packet.WriteGuid((Guid)value);
                }
                return true;
            }
            else if (target.IsEnum)
            {
                packet.WriteEnum(value);
                return true;
            }
            else if (typeof(APacketWrapper).IsAssignableFrom(target))
            {
                if (value == null) packet.WriteBool(false);
                else
                {
                    packet.WriteBool(true);
                    //var method = packet.GetType().GetMethod("WriteWrapper").MakeGenericMethod(target);
                    //method.Invoke(packet, new[] { value });
                    (value as APacketWrapper).WritePacket(packet);
                }
                return true;
            }
            return false;
        }

        public void ReadPacket(IDataPacket packet)
        {
            foreach (var property in Properties)
            {
                var type = property.PropertyType;
                var value = ReadValue(packet, type);
                property.SetValue(this, value, null);
            }
        }

        object ReadValue(IDataPacket packet, Type target)
        {
            if (target == typeof(byte))
                return packet.ReadByte();
            else if (target == typeof(sbyte))
                return packet.ReadSByte();
            else if (target == typeof(char))
                return packet.ReadChar();
            else if (target == typeof(bool))
                return packet.ReadBool();
            else if (target == typeof(ushort))
                return packet.ReadUShort();
            else if (target == typeof(uint))
                return packet.ReadUInt();
            else if (target == typeof(ulong))
                return packet.ReadULong();
            else if (target == typeof(short))
                return packet.ReadShort();
            else if (target == typeof(int))
                return packet.ReadInt();
            else if (target == typeof(long))
                return packet.ReadLong();
            else if (target == typeof(double))
                return packet.ReadDouble();
            else if (target == typeof(float))
                return packet.ReadFloat();
            else if (target == typeof(string))
                return !packet.ReadBool() ? null : packet.ReadString();
            else if (target == typeof(DateTime))
                return !packet.ReadBool() ? DateTime.MinValue : packet.ReadDateTime();
            else if (target == typeof(TimeSpan))
                return !packet.ReadBool() ? TimeSpan.Zero : packet.ReadTimeSpan();
            else if (target == typeof(Guid))
                return !packet.ReadBool() ? Guid.Empty : packet.ReadGuid();
            else if (target.IsEnum)
            {
                var method = packet.GetType().GetMethod("ReadEnum").MakeGenericMethod(target);
                return method.Invoke(packet, null);
            }
            else if (typeof(APacketWrapper).IsAssignableFrom(target))
            {
                if (packet.ReadBool())
                {
                    //var method = packet.GetType().GetMethod("ReadWrapper").MakeGenericMethod(target);
                    //return method.Invoke(packet, null);

                    var wrapper = Activator.CreateInstance(target) as APacketWrapper;
                    wrapper.ReadPacket(packet);
                    return wrapper;
                }
                else
                    return null;
            }
            else
                return null;
        }*/

        public T Clone<T>() where T : APacketWrapper, new()
        {
            var target = new T();
            foreach (var property in typeof(T).GetProperties())
            {
                var value = property.GetValue(this, null);
                property.SetValue(target, value, null);
            }

            return target;
        }
    }
}