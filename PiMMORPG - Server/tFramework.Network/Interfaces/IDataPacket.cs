using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Network.Interfaces
{
    public interface IDataPacket
    {
        ushort ID { get; set; }
        int Length { get; }
        int HeaderLength { get; }
        byte[] Buffer { get; set; }

        void Reset();
        void Clear();

        /// <summary>
        /// Get the constant buffer size of header information
        /// </summary>
        /// <param name="header">The buffer with header information</param>
        /// <returns>The length of packet buffer</returns>
        int LoadHeader(byte[] header);


        void CopyHeader(ref byte[] header);

        byte ReadByte();
        byte[] ReadBytes(int length);
        sbyte ReadSByte();
        char ReadChar();
        bool ReadBool();
        ushort ReadUShort();
        uint ReadUInt();
        ulong ReadULong();
        short ReadShort();
        int ReadInt();
        long ReadLong();
        float ReadFloat();
        double ReadDouble();
        string ReadString();
        Guid ReadGuid();
        TEnum ReadEnum<TEnum>();
        DateTime ReadDateTime();
        TimeSpan ReadTimeSpan();
        TPacketWrapper ReadWrapper<TPacketWrapper>() where TPacketWrapper : APacketWrapper, new();
        TPacketWrapper[] ReadWrappers<TPacketWrapper>() where TPacketWrapper : APacketWrapper, new();

        void WriteByte(byte value);
        void WriteBytes(byte[] value);
        void WriteSByte(sbyte value);
        void WriteChar(char value);
        void WriteBool(bool value);
        void WriteUShort(ushort value);
        void WriteUInt(uint value);
        void WriteULong(ulong value);
        void WriteShort(short value);
        void WriteInt(int value);
        void WriteLong(long value);
        void WriteFloat(float value);
        void WriteDouble(double value);
        void WriteString(string value);
        void WriteGuid(Guid value);
        void WriteEnum<TEnum>(TEnum value);
        void WriteDateTime(DateTime value);
        void WriteTimeSpan(TimeSpan value);
        void WriteWrapper<TPacketWrapper>(TPacketWrapper wrapper) where TPacketWrapper : APacketWrapper, new();
        void WriteWrappers<TPacketWrapper>(TPacketWrapper[] wrappers) where TPacketWrapper : APacketWrapper, new();
    }
}