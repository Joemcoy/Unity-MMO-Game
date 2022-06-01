using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace tFramework.Network.DataPacket
{
    using Interfaces;

    public class StreamPacket : IDataPacket
    {
        private MemoryStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        public ushort ID { get; set; }
        public int Length { get { return Convert.ToInt32(stream.Length); } }
        public int HeaderLength { get { return 1 + sizeof(ushort) + sizeof(int); } }
        public byte[] Buffer
        {
            get { return stream.ToArray(); }
            set { Clear(); stream.Write(value, 0, value.Length); Reset(); }
        }

        public StreamPacket()
        {
            stream = new MemoryStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }

        public void Reset() { stream.Position = 0; }
        public void Clear() { stream.SetLength(0); }

        public int LoadHeader(byte[] header)
        {
            ID = BitConverter.ToUInt16(header, 1);
            return BitConverter.ToInt32(header, 1 + sizeof(ushort));
        }

        public void CopyHeader(ref byte[] header)
        {
            var id = BitConverter.GetBytes(ID);
            var length = BitConverter.GetBytes(Length);

            header = new byte[HeaderLength];
            header[0] = SocketConstants.HandshakeByte;
            System.Buffer.BlockCopy(id, 0, header, 1, sizeof(ushort));
            System.Buffer.BlockCopy(length, 0, header, 1 + sizeof(ushort), sizeof(int));
        }

        public byte ReadByte() { return reader.ReadByte(); }
        public byte[] ReadBytes(int length) { return reader.ReadBytes(length); }
        public sbyte ReadSByte() { return reader.ReadSByte(); }
        public char ReadChar() { return reader.ReadChar(); }
        public bool ReadBool() { return reader.ReadBoolean(); }
        public ushort ReadUShort() { return reader.ReadUInt16(); }
        public uint ReadUInt() { return reader.ReadUInt32(); }
        public ulong ReadULong() { return reader.ReadUInt64(); }
        public short ReadShort() { return reader.ReadInt16(); }
        public int ReadInt() { return reader.ReadInt32(); }
        public long ReadLong() { return reader.ReadInt64(); }
        public float ReadFloat() { return reader.ReadSingle(); }
        public double ReadDouble() { return reader.ReadDouble(); }
        public string ReadString() { return reader.ReadString(); }
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
            for (int i = 0; i < wrappers.Length; i++)
                wrappers[i] = ReadWrapper<TPacketWrapper>();
            return wrappers;
        }

        public void WriteByte(byte value) { writer.Write(value); }
        public void WriteBytes(byte[] value) { writer.Write(value); }

        public void WriteSByte(sbyte value) { writer.Write(value); }
        public void WriteChar(char value) { writer.Write(value); }
        public void WriteBool(bool value) { writer.Write(value); }
        public void WriteUShort(ushort value) { writer.Write(value); }
        public void WriteUInt(uint value) { writer.Write(value); }
        public void WriteULong(ulong value) { writer.Write(value); }
        public void WriteShort(short value) { writer.Write(value); }
        public void WriteInt(int value) { writer.Write(value); }
        public void WriteLong(long value) { writer.Write(value); }
        public void WriteFloat(float value) { writer.Write(value); }
        public void WriteDouble(double value) { writer.Write(value); }
        public void WriteString(string value) { writer.Write(value); }
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

        public void WriteDateTime(DateTime value) { WriteLong(value.Ticks); }
        public void WriteTimeSpan(TimeSpan value) { WriteLong(value.Ticks); }
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