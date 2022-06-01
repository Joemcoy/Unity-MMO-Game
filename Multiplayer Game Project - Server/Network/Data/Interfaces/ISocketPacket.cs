using System;

namespace Network.Data.Interfaces
{
    public interface ISocketPacket
    {
        uint ID { get; set; }
        int Length { get; }
        byte[] Buffer { get; set; }

        void Clear();
        void Reset();

        byte ReadByte();
        byte[] ReadBuffer();
        char ReadChar();
        bool ReadBool();
        short ReadShort();
        int ReadInt();
        long ReadLong();
        ushort ReadUShort();
        uint ReadUInt();
        ulong ReadULong();
        double ReadDouble();
        float ReadFloat();
        decimal ReadDecimal();
        string ReadString();
        DateTime ReadDateTime();
        TimeSpan ReadTimeSpan();
        Guid ReadGuid();
        TEnum ReadEnum<TEnum>();
        object ReadEnum(Type EnumType);

        void WriteByte(byte Value);
        void WriteBuffer(byte[] Buffer);
        void WriteChar(char Value);
        void WriteBool(bool Value);
        void WriteShort(short Value);
        void WriteInt(int Value);
        void WriteLong(long Value);
        void WriteUShort(ushort Value);
        void WriteUInt(uint Value);
        void WriteDouble(double Value);
        void WriteFloat(float Value);
        void WriteDecimal(decimal Value);
        void WriteULong(ulong Value);
        void WriteString(string Value);
        void WriteDateTime(DateTime Value);
        void WriteTimeSpan(TimeSpan Value);
        void WriteGuid(Guid Value);
        void WriteEnum<TEnum>(TEnum Value);
        void WriteEnum(Type EnumType, object Value);
    }
}