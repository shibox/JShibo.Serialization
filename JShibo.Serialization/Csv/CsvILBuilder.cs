using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JShibo.Serialization.Common;
using System.Collections;

namespace JShibo.Serialization.Csv
{
    internal class CsvILBuilder : IBuilder
    {
        /// <summary>
        /// 获取对于csv方式序列化，该类型最小分配长度
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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

        
    }
}
