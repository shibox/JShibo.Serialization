using JShibo.Serialization.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.Json
{
    internal class JsonILBuilder:IBuilder
    {
        internal override string CreateReaderMethod(Type type)
        {
            return base.CreateReaderMethod(type);
        }

        internal override Serialize<T> GenerateSerializationType<T>(Type type)
        {
            return base.GenerateSerializationType<T>(type);
        }

        internal override Deserialize<T> GenerateDeserializationType<T>(Type type)
        {
            return base.GenerateDeserializationType<T>(type);
        }

        internal override int GetSize(Type type)
        {
            if (type == TypeConsts.Boolean)
                return 8;
            if (type == TypeConsts.Char)
                return 4;
            if (type == TypeConsts.SByte)
                return 4;
            if (type == TypeConsts.Byte)
                return 5;
            if (type == TypeConsts.Int16)
                return 7;
            if (type == TypeConsts.UInt16)
                return 6;
            if (type == TypeConsts.Int32)
                return 12;
            if (type == TypeConsts.UInt32)
                return 11;
            if (type == TypeConsts.Int64)
                return 21;
            if (type == TypeConsts.UInt64)
                return 20;
            if (type == TypeConsts.Single)
                return 12;
            if (type == TypeConsts.Double)
                return 20;
            if (type == TypeConsts.Decimal)
                return 20;

            if (type == TypeConsts.DateTime)
                return 36;
            if (type == TypeConsts.DateTimeOffset)
                return 36;
            if (type == TypeConsts.TimeSpan)
                return 36;
            if (type == TypeConsts.Guid)
                return 39;

            return 0;
        }

        /// <summary>
        /// 默认原生支持的数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal override bool IsBaseType(Type type)
        {
            if (type.GetInterface("IList") == typeof(IList) ||
                    type.GetInterface("IDictionary") == typeof(IDictionary) ||
                    type.GetInterface("ICollection") == typeof(ICollection))
                return true;
            if (type.IsPrimitive)
                return true;

            if (type == typeof(decimal))
                return true;

            if (type == typeof(ArraySegment<bool>))
                return true;
            if (type == typeof(ArraySegment<byte>))
                return true;
            if (type == typeof(ArraySegment<sbyte>))
                return true;
            if (type == typeof(ArraySegment<short>))
                return true;
            if (type == typeof(ArraySegment<ushort>))
                return true;
            if (type == typeof(ArraySegment<int>))
                return true;
            if (type == typeof(ArraySegment<uint>))
                return true;
            if (type == typeof(ArraySegment<long>))
                return true;
            if (type == typeof(ArraySegment<ulong>))
                return true;
            if (type == typeof(ArraySegment<float>))
                return true;
            if (type == typeof(ArraySegment<double>))
                return true;
            if (type == typeof(ArraySegment<decimal>))
                return true;
            if (type == typeof(ArraySegment<char>))
                return true;
            if (type == typeof(ArraySegment<DateTime>))
                return true;
            if (type == typeof(ArraySegment<DateTimeOffset>))
                return true;
            if (type == typeof(ArraySegment<TimeSpan>))
                return true;
            if (type == typeof(ArraySegment<Uri>))
                return true;
            if (type == typeof(ArraySegment<Guid>))
                return true;

            if (type == typeof(Uri))
                return true;
            if (type == typeof(Enum))
                return true;
            if (type == typeof(DateTime))
                return true;
            if (type == typeof(TimeSpan))
                return true;
            if (type == typeof(DateTimeOffset))
                return true;
            if (type == typeof(Guid))
                return true;
            if (type == typeof(DataTable))
                return true;
            if (type == typeof(DataSet))
                return true;
            return false;
        }

        internal override bool IsFixedSizeType(Type type)
        {
            if (type.IsPrimitive)
                return true;
            return false;
        }

    }
}
