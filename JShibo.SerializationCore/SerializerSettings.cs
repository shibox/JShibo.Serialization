using JShibo.Serialization.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace JShibo.Serialization
{
    /// <summary>
    /// 序列化设置器
    /// </summary>
    public class SerializerSettings
    {
        public static SerializerSettings Default = new SerializerSettings();

        internal const FormatterAssemblyStyle DefaultFormatterAssemblyStyle = FormatterAssemblyStyle.Simple;
        internal const string DefaultDateFormatString = @"yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

        /// <summary>
        /// 使用单引号而不是双引号
        /// </summary>
        public bool UseSingleQuotes = false;
        /// <summary>
        /// 序列化时是否写入类型标记，这样可以支持不使用类型进行反序列化
        /// </summary>
        public bool WriteType = false;
        /// <summary>
        /// 时间序列化属性，采用“o”类型序列化性能很差
        /// </summary>
        public string TimeFormatType = "yyyy-MM-dd hh:mm:ss";
        /// <summary>
        /// 序列化是否忽略空类型
        /// </summary>
        public bool NullValueIgnore = false;
        /// <summary>
        /// 序列化时字符串使用的编码
        /// </summary>
        public Encoding StringEncoding = Encoding.Unicode;
        /// <summary>
        /// 是否在Json格式化的时候将首字母转化成大写
        /// </summary>
        public bool CamelCase = false;
        /// <summary>
        /// 反序列化bool类型数据的是否使用完整的数据校验
        /// 完整的校验将可以检查出数据中的错误
        /// </summary>
        public bool BoolFullCheck = true;
        /// <summary>
        /// 是有采用缩进的格式化化输出
        /// </summary>
        public bool Pretty = false;
        /// <summary>
        /// 序列化的属性，可以设置序列化私有属性
        /// </summary>
        public BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public;
        /// <summary>
        /// 是否检查Json转义符，如“"”
        /// </summary>
        public CheckEscape CheckJsonEscape = CheckEscape.None;
        /// <summary>
        /// 字符串转义的模式
        /// </summary>
        public StringEscape Escape = StringEscape.Default;
        internal Formatting DefaultFormatting = Formatting.None;
        /// <summary>
        /// 对于值类型的数据是采用中间判断还是从最大最小值开始判断
        /// </summary>
        public NumericCheckType NumericCheck = NumericCheckType.Middle;
        /// <summary>
        /// 默认情况下不序列化null值
        /// </summary>
        public bool WriteMapNullValue = false;
        /// <summary>
        /// 对于枚举的序列化是否使用字符串形式表示
        /// </summary>
        public bool WriteEnumUsingToString = false;
        /// <summary>
        /// 对于序列化的字段数据是否进行排序处理
        /// </summary>
        public bool SortField = false;
    }
}
