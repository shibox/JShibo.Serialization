using System;
using System.Collections.Generic;
using System.Data;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Common
{
    internal class TypeConsts
    {
        internal static readonly Type Boolean = typeof(bool);
        internal static readonly Type Char = typeof(char);
        internal static readonly Type Byte = typeof(byte);
        internal static readonly Type SByte = typeof(sbyte);
        internal static readonly Type Int16 = typeof(short);
        internal static readonly Type UInt16 = typeof(ushort);
        internal static readonly Type Int32 = typeof(int);
        internal static readonly Type UInt32 = typeof(uint);
        internal static readonly Type Int64 = typeof(long);
        internal static readonly Type UInt64 = typeof(ulong);
        internal static readonly Type Single = typeof(float);
        internal static readonly Type Double = typeof(double);
        internal static readonly Type Decimal = typeof(decimal);
        internal static readonly Type String = typeof(string);
        internal static readonly Type Object = typeof(object);

        internal static readonly Type BooleanArray = typeof(bool[]);
        internal static readonly Type CharArray = typeof(char[]);
        internal static readonly Type ByteArray = typeof(byte[]);
        internal static readonly Type SByteArray = typeof(sbyte[]);
        internal static readonly Type Int16Array = typeof(short[]);
        internal static readonly Type UInt16Array = typeof(ushort[]);
        internal static readonly Type Int32Array = typeof(int[]);
        internal static readonly Type UInt32Array = typeof(uint[]);
        internal static readonly Type Int64Array = typeof(long[]);
        internal static readonly Type UInt64Array = typeof(ulong[]);
        internal static readonly Type SingleArray = typeof(float[]);
        internal static readonly Type DoubleArray = typeof(double[]);
        internal static readonly Type DecimalArray = typeof(decimal[]);
        internal static readonly Type StringArray = typeof(string[]);
        internal static readonly Type ObjectArray = typeof(object[]);

        internal static readonly Type DateTime = typeof(DateTime);
        internal static readonly Type DateTimeOffset = typeof(DateTimeOffset);
        internal static readonly Type TimeSpan = typeof(TimeSpan);
        internal static readonly Type Guid = typeof(Guid);
        internal static readonly Type Enum = typeof(Enum);
        internal static readonly Type Uri = typeof(Uri);
        internal static readonly Type DataTable = typeof(DataTable);
        internal static readonly Type DataSet = typeof(DataSet);
        internal static readonly Type DBNull = typeof(DBNull);

        
    }

    internal enum TypeCodes
    {
        Empty = 0,
        Object = 1,
        DBNull = 2,
        Boolean = 3,
        Char = 4,
        SByte = 5,
        Byte = 6,
        Int16 = 7,
        UInt16 = 8,
        Int32 = 9,
        UInt32 = 10,
        Int64 = 11,
        UInt64 = 12,
        Single = 13,
        Double = 14,
        Decimal = 15,
        DateTime = 16,
        String = 18,

        DateTimeOffset=19,
        TimeSpan=20,
        Guid=21,
        Uri=22,
        DataTable=23,
        DataSet=24,
        Enum = 25,
    }

}
