using System;
using System.IO;

using Network.Data;
using Network.Data.Interfaces;

namespace Network.Protocol
{
    public class StreamPacket : ISocketPacket
    {
        BinaryReader Reader;
        BinaryWriter Writer;

        public uint ID { get; set; }
        public int Length { get { return -1; } }

        public byte[] Buffer { get; set; }

        public StreamPacket(uint ID, BinaryReader Reader, BinaryWriter Writer)
        {
            this.ID = ID;
            this.Reader = Reader;
            this.Writer = Writer;

            Clear();
        }

        ~StreamPacket()
        {
            Writer.Close();
            Reader.Close();
        }

        public void Clear()
        {

        }

        public void Reset()
        {

        }

        public byte ReadByte() { return Reader.ReadByte(); }
        public byte[] ReadBuffer() { return Reader.ReadBytes(ReadInt()); }
        public char ReadChar() { return Reader.ReadChar(); }
        public bool ReadBool() { return Reader.ReadBoolean(); ; }
        public short ReadShort() { return Reader.ReadInt16(); }
        public int ReadInt() { return Reader.ReadInt32(); }
        public long ReadLong() { return Reader.ReadInt64(); }
        public ushort ReadUShort() { return Reader.ReadUInt16(); }
        public uint ReadUInt() { return Reader.ReadUInt32(); }
        public ulong ReadULong() { return Reader.ReadUInt64(); }
        public double ReadDouble() { return Reader.ReadDouble(); }
        public float ReadFloat() { return Reader.ReadSingle(); }
        public decimal ReadDecimal() { return Reader.ReadDecimal(); }
        public string ReadString() { return Reader.ReadString(); }
        public DateTime ReadDateTime() { return new DateTime(ReadLong()); }
        public TimeSpan ReadTimeSpan() { return new TimeSpan(ReadLong()); }
        public Guid ReadGuid() { return new Guid(ReadBuffer()); }
        public TEnum ReadEnum<TEnum>() { return (TEnum)ReadEnum(typeof(TEnum)); }
        public object ReadEnum(Type EnumType) { return Enum.Parse(EnumType, ReadString()) as Enum; }

        public void WriteByte(byte Value) { Writer.Write(Value); }
        public void WriteBuffer(byte[] Buffer) { Writer.Write(Buffer); }
        public void WriteChar(char Value) { Writer.Write(Value); }
        public void WriteBool(bool Value) { Writer.Write(Value); }
        public void WriteShort(short Value) { Writer.Write(Value); }
        public void WriteInt(int Value) { Writer.Write(Value); }
        public void WriteLong(long Value) { Writer.Write(Value); }
        public void WriteUShort(ushort Value) { Writer.Write(Value); }
        public void WriteUInt(uint Value) { Writer.Write(Value); }
        public void WriteULong(ulong Value) { Writer.Write(Value); }
        public void WriteDouble(double Value) { Writer.Write(Value); }
        public void WriteFloat(float Value) { Writer.Write(Value); }
        public void WriteDecimal(decimal Value) { Writer.Write(Value); }
        public void WriteString(string Value) { Writer.Write(Value); }
        public void WriteDateTime(DateTime Value) { WriteLong(Value.Ticks); }
        public void WriteTimeSpan(TimeSpan Value) { WriteLong(Value.Ticks); }
        public void WriteGuid(Guid Value) { WriteBuffer(Value.ToByteArray()); }
        public void WriteEnum<TEnum>(TEnum Value) { WriteEnum(typeof(TEnum), Value); }
        public void WriteEnum(Type EnumType, object Value) { WriteString(Enum.GetName(EnumType, Value)); }
    }
}