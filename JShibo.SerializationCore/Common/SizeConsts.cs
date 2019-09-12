using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Common
{
    internal class SizeConsts
    {
        /// <summary>
        /// 值类型默认需要使用4个位置的额外空间
        /// </summary>
        public const int VALUETYPE_LEN  = 5;

        /// <summary>
        /// bool类型的最大长度5
        /// </summary>
        public const int VALUETYPE_BOOLEAN_MAX_LENGTH = 5;
        /// <summary>
        /// 字符类型最大长度2
        /// </summary>
        public const int VALUETYPE_CHAR_MAX_LENGTH = 2;
        /// <summary>
        /// 字节类型最大长度3
        /// </summary>
        public const int VALUETYPE_BYTE_MAX_LENGTH = 3;
        /// <summary>
        /// 有符号字节类型最大长度4
        /// </summary>
        public const int VALUETYPE_SBYTE_MAX_LENGTH = 4;
        /// <summary>
        /// short类型最大长度6
        /// </summary>
        public const int VALUETYPE_SHORT_MAX_LENGTH = 6;
        /// <summary>
        /// ushort类型最大长度5
        /// </summary>
        public const int VALUETYPE_USHORT_MAX_LENGTH = 5;
        /// <summary>
        /// int类型最大长度11
        /// </summary>
        public const int VALUETYPE_INT_MAX_LENGTH = 11;
        /// <summary>
        /// uint类型最大长度10
        /// </summary>
        public const int VALUETYPE_UINT_MAX_LENGTH = 10;
        /// <summary>
        /// long类型最大长度20
        /// </summary>
        public const int VALUETYPE_LONG_MAX_LENGTH = 20;
        /// <summary>
        /// ulong类型最大长度19
        /// </summary>
        public const int VALUETYPE_ULONG_MAX_LENGTH = 19;
        /// <summary>
        /// 浮点类型最大长度20
        /// </summary>
        public const int VALUETYPE_FLOAT_MAX_LENGTH = 20;
        /// <summary>
        /// double类型最大长度30
        /// </summary>
        public const int VALUETYPE_DOUBLE_MAX_LENGTH = 30;
        /// <summary>
        /// decimal类型最大长度30
        /// </summary>
        public const int VALUETYPE_DECIMAL_MAX_LENGTH = 30;

        /// <summary>
        /// 时间类型最大长度33
        /// </summary>
        public const int VALUETYPE_DATETIME_MAX_LENGTH = 33;
        /// <summary>
        /// timespan最大长度测试为25
        /// </summary>
        public const int VALUETYPE_TIMESPAN_MAX_LENGTH = 25;
        /// <summary>
        /// DateTimeOffset类型最大长度33，和DateTime一样长
        /// </summary>
        public const int VALUETYPE_DATETIMEOFFSET_MAX_LENGTH = 33;
        /// <summary>
        /// 枚举最大长度5，可能是一个不确定的值，需要动态计算 
        /// </summary>
        public const int VALUETYPE_ENUM_MAX_LENGTH = 5;
        /// <summary>
        /// guid类型的最大长度40
        /// </summary>
        public const int VALUETYPE_GUID_MAX_LENGTH = 40;
        public const int VALUETYPE_URI_MAX_LENGTH = 2;
        public const int VALUETYPE_STRING_MAX_LENGTH = 2;

        /// <summary>
        /// 空值数据的长度8
        /// </summary>
        public const int NULL_SIZE = 8;
        /// <summary>
        /// 空数组的最大长度6
        /// </summary>
        public const int ZERO_ARRAY_SIZE = 6;
        /// <summary>
        /// 数组的基础长度6
        /// </summary>
        public const int ARRAY_BASE_SIZE = 6;
        /// <summary>
        /// 类的基础长度6
        /// </summary>
        public const int CLASSTYPE_LEN = 6;
        /// <summary>
        /// 字符串的基本长度
        /// </summary>
        public const int STRING_BASE_SIZE = 3;
        /// <summary>
        /// Soc协议数组数据的长度最大使用多少字节表示，可变长度最大5个字节表示
        /// </summary>
        public const int SOC_ARRAY_BASE_SIZE = 5;
    }
}
