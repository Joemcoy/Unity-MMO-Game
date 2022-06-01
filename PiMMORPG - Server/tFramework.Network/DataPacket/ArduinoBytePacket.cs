using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Network.DataPacket
{
    using Extensions;
    using Interfaces;

    public class ArduinoBytePacket : IDataPacket
    {
        List<byte> _buffer;
        int _position;

        public ushort ID { get; set; }
        public int Length { get { return _position; } }
        public int HeaderLength { get { return 1 + sizeof(ushort) * 2; } }
        public byte[] Buffer { get { return _buffer.ToArray(); } set { Clear(); _buffer.AddRange(value); _position = value.Length; } }

        public ArduinoBytePacket()
        {
            _buffer = new List<byte>();
            Clear();
        }

        public ArduinoBytePacket(ushort id) : this() { this.ID = id; }

        public void Reset() { _position = 0; }
        public void Clear() { _buffer.Clear(); Reset(); }

        public int LoadHeader(byte[] header)
        {
            ID = Convert.ToUInt16(header[1] | (header[2] << 8));
            return Convert.ToUInt16(header[3] | (header[4] << 8));
        }

        public void CopyHeader(ref byte[] header)
        {
            var bytes = BitConverter.GetBytes(ID);
            header[0] = SocketConstants.HandshakeByte;
            header[1] = Convert.ToByte(bytes[0]);
            header[2] = Convert.ToByte(bytes[1]);
            header[3] = Convert.ToByte(Length);
            header[4] = Convert.ToByte(Length >> 8);
        }

        public byte ReadByte()
        {
            return _buffer[_position++];
        }

        public byte[] ReadBytes(int length)
        {
            for (int i = 0; i < length; i++)
                Buffer[i] = ReadByte();
            return Buffer;
        }

        public sbyte ReadSByte() { return (sbyte)ReadByte(); }
        public char ReadChar() { return (char)ReadByte(); }
        public bool ReadBool() { return ReadByte() == 0x1; }
        public ushort ReadUShort() { return Convert.ToUInt16(ReadByte() | ReadByte() << 8); }
        public uint ReadUInt() { return ReadUShort(); }
        public ulong ReadULong() { return BitConverter.ToUInt32(ReadBytes(sizeof(ulong)), 0); }
        public short ReadShort() { return (short)ReadUShort(); }
        public int ReadInt() { return ReadShort(); }
        public long ReadLong() { return BitConverter.ToInt32(ReadBytes(sizeof(long)), 0); }
        public float ReadFloat() { return BitConverter.ToSingle(ReadBytes(sizeof(float)), 0); }
        public double ReadDouble() { return BitConverter.ToDouble(ReadBytes(sizeof(double)), 0); }
        public string ReadString()
        {
            int len = ReadInt();
            string value = string.Empty;

            for (int i = 0; i < len; i++)
                value += ReadChar();

            return value;
        }
        public Guid ReadGuid() { return new Guid(ReadBytes(16)); }

        public TEnum ReadEnum<TEnum>()
        {
            if (!typeof(TEnum).IsEnum)
                throw new InvalidCastException();

            var type = Enum.GetUnderlyingType(typeof(TEnum));
            object eValue = 0x0;

            if (type == typeof(byte))
                eValue = ReadByte();
            else if (type == typeof(ushort))
                eValue = ReadUShort();
            else if (type == typeof(uint))
                eValue = ReadUInt();
            else if (type == typeof(ulong))
                eValue = ReadULong();
            else if (type == typeof(short))
                eValue = ReadShort();
            else if (type == typeof(int))
                eValue = ReadInt();
            else if (type == typeof(long))
                eValue = ReadLong();

            return (TEnum)Enum.ToObject(typeof(TEnum), eValue);
        }

        public DateTime ReadDateTime() { return new DateTime(ReadLong()); }
        public TimeSpan ReadTimeSpan() { return new TimeSpan(ReadLong()); }

        public TPacketWrapper ReadWrapper<TPacketWrapper>() where TPacketWrapper : APacketWrapper, new()
        {
            var wrapper = new TPacketWrapper();
            wrapper.ReadPacket(this);
            return wrapper;
        }
        public TPacketWrapper[] ReadWrappers<TPacketWrapper>() where TPacketWrapper : APacketWrapper, new()
        {
            var wrappers = new TPacketWrapper[ReadInt()];
            for (int i = 0; i < wrappers.Length; i++)
                wrappers[i] = ReadWrapper<TPacketWrapper>();
            return wrappers;
        }

        public void WriteByte(byte value)
        {
            _buffer.Add(value);
            _position++;
        }

        public void WriteBytes(byte[] value) { value.ForEach(WriteByte); }
        public void WriteSByte(sbyte value) { WriteByte((byte)value); }
        public void WriteChar(char value) { WriteByte(Convert.ToByte(value)); }
        public void WriteBool(bool value) { WriteByte((byte)(value ? 0x1 : 0x0)); }
        public void WriteUShort(ushort value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteUInt(uint value) { WriteUShort((ushort)value); }
        public void WriteULong(ulong value) { WriteBytes(BitConverter.GetBytes((uint)value)); }
        public void WriteShort(short value) { WriteUShort((ushort)value); }
        public void WriteInt(int value) { WriteShort((short)value); }
        public void WriteLong(long value) { WriteBytes(BitConverter.GetBytes((int)value)); }
        public void WriteFloat(float value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteDouble(double value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteString(string value)
        {
            WriteInt(value.Length);
            value.ForEach(c => WriteChar(c));
        }
        public void WriteGuid(Guid value) { WriteBytes(value.ToByteArray()); }

        public void WriteEnum<TEnum>(TEnum value)
        {
            if (!typeof(TEnum).IsEnum)
                throw new InvalidCastException();

            var type = Enum.GetUnderlyingType(typeof(TEnum));
            if (type == typeof(byte))
                WriteByte(Convert.ToByte(value));
            else if (type == typeof(ushort))
                WriteUShort(Convert.ToUInt16(value));
            else if (type == typeof(uint))
                WriteUInt(Convert.ToUInt32(value));
            else if (type == typeof(ulong))
                WriteULong(Convert.ToUInt64(value));
            else if (type == typeof(short))
                WriteShort(Convert.ToInt16(value));
            else if (type == typeof(int))
                WriteInt(Convert.ToInt32(value));
            else if (type == typeof(long))
                WriteLong(Convert.ToInt64(value));
        }

        public void WriteDateTime(DateTime value)
        {
            WriteLong(value.Ticks);
        }

        public void WriteTimeSpan(TimeSpan value)
        {
            WriteLong(value.Ticks);
        }

        public void WriteWrapper<TPacketWrapper>(TPacketWrapper wrapper) where TPacketWrapper : APacketWrapper, new()
        {
            wrapper.WritePacket(this);
        }

        public void WriteWrappers<TPacketWrapper>(TPacketWrapper[] wrappers) where TPacketWrapper : APacketWrapper, new()
        {
            WriteInt(wrappers.Length);
            foreach (var wrapper in wrappers)
                wrapper.WritePacket(this);
        }
    }
}