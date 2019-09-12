using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JShibo.Serialization
{

    //Empty = 0,
    //Object = 1,
    //DBNull = 2,
    //Boolean = 3,
    //Char = 4,
    //SByte = 5,
    //Byte = 6,
    //Int16 = 7,
    //UInt16 = 8,
    //Int32 = 9,
    //UInt32 = 10,
    //Int64 = 11,
    //UInt64 = 12,
    //Single = 13,
    //Double = 14,
    //Decimal = 15,
    //DateTime = 16,
    //String = 18,
    //DateTimeOffset
    //TimeSpan
    //Guid
    //Stream
    //IList
    //IDictionary
    //IEnumerable

    interface IReader
    {
        Object ReadObject();
        Boolean ReadBoolean();
        Char ReadChar();
        SByte ReadSByte();
        Byte ReadByte();
        Int16 ReadInt16();
        UInt16 ReadUInt16();
        Int32 ReadInt32();
        UInt32 ReadUInt32();
        Int64 ReadInt64();
        UInt64 ReadUInt64();
        Single ReadSingle();
        Double ReadDouble();
        Decimal ReadDecimal();
        DateTime ReadDateTime();
        String ReadString();
        DateTimeOffset ReadDateTimeOffset();
        TimeSpan ReadTimeSpan();
        Guid ReadGuid();
        Stream ReadStream();
        IList ReadIList();
        IDictionary ReadIDictionary();
        IEnumerable ReadIEnumerable();
    }

    interface IJsonReader
    {

    }

    interface ISocReader
    {

    }
}
