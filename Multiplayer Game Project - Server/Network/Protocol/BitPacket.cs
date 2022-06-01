using System;
using Network.Data.Interfaces;
using System.Collections.Generic;
using Network.Data;

using System.Text;

namespace Network.Protocol
{
    public class BitPacket : ISocketPacket
    {
        List<byte> BufferList;
        object syncLock;
        int Position = 0;

        public uint ID { get; set; }
        public int Length { get { return BufferList.Count; } }

        public byte[] Buffer
        {
            get
            {
                lock (syncLock)
                    return BufferList.ToArray();
            }
            set
            {
                lock (syncLock)
                    BufferList = new List<byte>(value);
            }
        }

        public BitPacket(uint ID)
        {
            this.ID = ID;

            BufferList = new List<byte>();
            syncLock = new object();
            Position = 0;
        }

        public void Clear()
        {
            lock (syncLock)
            {
                BufferList.Clear();
                Position = 0;
            }
        }

        public void Reset()
        {
            lock (syncLock)
            {
                Position = 0;
            }
        }

        public byte ReadByte()
        {
            lock (syncLock)
            {
                if (Position + 1 >= SocketConstants.MaximumPacketLength)
                    throw new Exception("This operation exceeds the packet limit!");

                return BufferList[Position++];
            }
        }

        byte[] Iterate(int Length)
        {
            byte[] Buffer = new byte[Length];
            for (int i = 0; i < Buffer.Length; i++)
                Buffer[i] = ReadByte();
            return Buffer;
        }

        public byte[] ReadBuffer()
        {
            byte[] Size = Iterate(sizeof(int));
            return Iterate(BitConverter.ToInt32(Size, 0));
        }

        public char ReadChar() { return BitConverter.ToChar(ReadBuffer(), 0); }
        public bool ReadBool() { return BitConverter.ToBoolean(ReadBuffer(), 0); }
        public short ReadShort() { return BitConverter.ToInt16(ReadBuffer(), 0); }
        public int ReadInt() { return BitConverter.ToInt32(ReadBuffer(), 0); }
        public long ReadLong() { return BitConverter.ToInt64(ReadBuffer(), 0); }
        public ushort ReadUShort() { return BitConverter.ToUInt16(ReadBuffer(), 0); }
        public uint ReadUInt() { return BitConverter.ToUInt32(ReadBuffer(), 0); }
        public ulong ReadULong() { return BitConverter.ToUInt64(ReadBuffer(), 0); }
        public double ReadDouble() { return BitConverter.ToDouble(ReadBuffer(), 0); }
        public float ReadFloat() { return BitConverter.ToSingle(ReadBuffer(), 0); }

        public decimal ReadDecimal()
        {
            int[] Parts = new int[4];
            for (int i = 0; i < 4; i++)
                Parts[i] = ReadInt();

            return new decimal(Parts);
        }

        public string ReadString() { return Encoding.UTF8.GetString(ReadBuffer()); }
        public DateTime ReadDateTime() { return new DateTime(ReadLong()); }
        public TimeSpan ReadTimeSpan() { return new TimeSpan(ReadLong()); }
        public Guid ReadGuid() { return new Guid(ReadBuffer()); }
        public TEnum ReadEnum<TEnum>() { return (TEnum)ReadEnum(typeof(TEnum)); }
        public object ReadEnum(Type EnumType) { return Enum.ToObject(EnumType, ReadInt()); }

        public void WriteByte(byte Value)
        {
            lock (syncLock)
            {
                if (Position + 1 >= SocketConstants.MaximumPacketLength)
                    throw new Exception("This operation exceeds the packet limit!");

                BufferList.Insert(Position++, Value);
            }
        }

        public void WriteBuffer(byte[] Buffer)
        {
            Iterate(BitConverter.GetBytes(Buffer.Length));
            Iterate(Buffer);
        }

        void Iterate(byte[] Buffer)
        {
            for (int i = 0; i < Buffer.Length; i++)
                WriteByte(Buffer[i]);
        }

        public void WriteChar(char Value) { WriteBuffer(BitConverter.GetBytes(Value)); }
        public void WriteBool(bool Value) { WriteBuffer(BitConverter.GetBytes(Value)); }
        public void WriteShort(short Value) { WriteBuffer(BitConverter.GetBytes(Value)); }
        public void WriteInt(int Value) { WriteBuffer(BitConverter.GetBytes(Value)); }
        public void WriteLong(long Value) { WriteBuffer(BitConverter.GetBytes(Value)); }
        public void WriteUShort(ushort Value) { WriteBuffer(BitConverter.GetBytes(Value)); }
        public void WriteUInt(uint Value) { WriteBuffer(BitConverter.GetBytes(Value)); }
        public void WriteULong(ulong Value) { WriteBuffer(BitConverter.GetBytes(Value)); }
        public void WriteDouble(double Value) { WriteBuffer(BitConverter.GetBytes(Value)); }
        public void WriteFloat(float Value) { WriteBuffer(BitConverter.GetBytes(Value)); }

        public void WriteDecimal(decimal Value)
        {
            foreach (int Bit in decimal.GetBits(Value))
                WriteInt(Bit);
        }

        public void WriteString(string Value) { WriteBuffer(Encoding.UTF8.GetBytes(Value)); }
        public void WriteDateTime(DateTime Value) { WriteLong(Value.Ticks); }
        public void WriteTimeSpan(TimeSpan Value) { WriteLong(Value.Ticks); }
        public void WriteGuid(Guid Value) { WriteBuffer(Value.ToByteArray()); }
        public void WriteEnum<TEnum>(TEnum Value) { WriteEnum(typeof(TEnum), Value); }
        public void WriteEnum(Type EnumType, object Value) { WriteInt(Convert.ToInt32(Value)); }
    }
}