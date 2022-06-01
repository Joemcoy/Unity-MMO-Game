using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Network.DataPacket
{
    using Extensions;
    using Interfaces;

    public class BitPacket : IDataPacket
    {
        List<byte> _buffer;
        int _position;

        public ushort ID { get; set; }
        public int Length { get { return _buffer.Count; } }
        public int HeaderLength { get { return 1 + sizeof(ushort) + sizeof(int); } }
        public byte[] Buffer
        {
            get { return _buffer.ToArray(); }
            set { Clear(); _buffer.AddRange(value); _position = value.Length; }
        }

        public BitPacket()
        {
            _buffer = new List<byte>();
            Clear();
        }
        public BitPacket(ushort id) : this() { this.ID = id; }

        public void Reset() { _position = 0; }
        public void Clear() { _buffer.Clear(); Reset(); }

        public int LoadHeader(byte[] header)
        {
            ID = BitConverter.ToUInt16(header, 1);
            return BitConverter.ToInt32(header, 1 + sizeof(ushort));
        }

        public void CopyHeader(ref byte[] header)
        {
            var id = BitConverter.GetBytes(this.ID);
            var length = BitConverter.GetBytes(this.Length);

            header = new byte[HeaderLength];
            header[0] = SocketConstants.HandshakeByte;
            System.Buffer.BlockCopy(id, 0, header, 1, sizeof(ushort));
            System.Buffer.BlockCopy(length, 0, header, 1 + sizeof(ushort), sizeof(int));
        }

        public byte ReadByte()
        {
            return _buffer[_position++];
        }

        public byte[] ReadBytes(int length)
        {
            if (length > _buffer.Count - _position)
                throw new OverflowException(string.Format("DL: {0} | B:{1} | P:{2} | B-P:{3}", length, _buffer.Count, _position, _buffer.Count - _position));

            var buffer = new byte[length];
            for (int i = 0; i < length; i++)
                buffer[i] = ReadByte();
            return buffer;
        }

        public sbyte ReadSByte() { return (sbyte)ReadByte(); }
        public char ReadChar() { return BitConverter.ToChar(ReadBytes(sizeof(char)), 0); }
        public bool ReadBool() { return BitConverter.ToBoolean(ReadBytes(sizeof(bool)), 0); }
        public ushort ReadUShort() { return BitConverter.ToUInt16(ReadBytes(sizeof(ushort)), 0); }
        public uint ReadUInt() { return BitConverter.ToUInt32(ReadBytes(sizeof(uint)), 0); }
        public ulong ReadULong() { return BitConverter.ToUInt64(ReadBytes(sizeof(ulong)), 0); }
        public short ReadShort() { return BitConverter.ToInt16(ReadBytes(sizeof(short)), 0); }
        public int ReadInt() { return BitConverter.ToInt32(ReadBytes(sizeof(int)), 0); }
        public long ReadLong() { return BitConverter.ToInt64(ReadBytes(sizeof(long)), 0); }
        public float ReadFloat() { return BitConverter.ToSingle(ReadBytes(sizeof(float)), 0); }
        public double ReadDouble() { return BitConverter.ToDouble(ReadBytes(sizeof(double)), 0); }
        public string ReadString() { return Encoding.UTF8.GetString(ReadBytes(ReadInt())); }
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
            var total = ReadInt();
            var wrappers = new TPacketWrapper[total];
            for(int i = 0; i < wrappers.Length; i++)
                wrappers[i] = ReadWrapper<TPacketWrapper>();
            return wrappers;
        }

        public void WriteByte(byte value) { _buffer.Insert(_position++, value); }
        public void WriteBytes(byte[] value) { value.ForEach(WriteByte); }

        public void WriteSByte(sbyte value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteChar(char value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteBool(bool value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteUShort(ushort value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteUInt(uint value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteULong(ulong value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteShort(short value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteInt(int value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteLong(long value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteFloat(float value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteDouble(double value) { WriteBytes(BitConverter.GetBytes(value)); }
        public void WriteString(string value)
        {
            var buffer = Encoding.UTF8.GetBytes(value);
            WriteInt(buffer.Length);
            WriteBytes(buffer);
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