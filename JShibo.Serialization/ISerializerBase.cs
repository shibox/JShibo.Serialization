using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization
{
    internal interface ISerializerBase
    {
        void Write(bool value);
        void Write(byte value);
        void Write(sbyte value);
        void Write(short value);
        void Write(ushort value);
        void Write(int value);
        void Write(uint value);
        void Write(long value);
        void Write(ulong value);
        void Write(float value);
        void Write(double value);
        void Write(decimal value);
        void Write(char value);
        void Write(string value);
        void Write(DateTime value);
        void Write(DateTimeOffset value);
        void Write(TimeSpan value);
        void Write(Guid value);
        void Write(Uri value);


        bool ReadBoolean();
        byte ReadByte();
        sbyte ReadSByte();
        short ReadShort();
        ushort ReadUShort();
        int ReadInt();
        uint ReadUInt();
        long ReadLong();
        ulong ReadULong();
        float ReadFloat();
        double ReadDouble();
        decimal ReadDecimal();
        char ReadChar();
        string ReadString();
        DateTime ReadDateTime();
        DateTimeOffset ReadDateTimeOffset();
        TimeSpan ReadTimeSpan();
        Guid ReadGuid();
        Uri ReadUri();

    }
}
