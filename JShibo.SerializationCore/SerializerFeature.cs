using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization
{
    /// <summary>
    /// 序列化的一些选项
    /// </summary>
    enum SerializerFeature
    {
        /// <summary>
        /// 序列化输出字段，使用引号。
        /// </summary>
        QuoteFieldNames,
        /// <summary>
        /// 使用单引号而不是双引号
        /// </summary>
        UseSingleQuotes,
        /// <summary>
        /// 空值是否输出。大多数情况，值为null的属性输出是没有意义的，缺省这个特性是打开的。
        /// </summary>
        WriteMapNullValue,
        /// <summary>
        /// 对于枚举类型的格式化使用ToString方法
        /// </summary>
        WriteEnumUsingToString,
        /// <summary>
        /// 使用ISO8601方式格式化时间
        /// </summary>
        UseISO8601DateFormat,
        /// <summary>
        /// 对于空集合使用0集合
        /// </summary>
        WriteNullListAsEmpty,
        /// <summary>
        /// 对于空字符串使用""
        /// </summary>
        WriteNullStringAsEmpty,
        /// <summary>
        /// 对于值类型的空值使用0
        /// </summary>
        WriteNullNumberAsZero,
        /// <summary>
        /// 对空bool类型使用false默认值
        /// </summary>
        WriteNullBooleanAsFalse,
        /// <summary>
        /// 类中的Get方法对应的Field是transient，序列化时将会被忽略 
        /// </summary>
        SkipTransientField,
        /// <summary>
        /// 按字段名称排序后输出 
        /// </summary>
        SortField,
        /// <summary>
        /// 把\t做转义输出。
        /// </summary>
        WriteTabAsSpecial,
        /// <summary>
        /// 格式化
        /// </summary>
        PrettyFormat,
        /// <summary>
        /// 写入类的名称
        /// </summary>
        WriteClassName,
        /// <summary>
        /// 禁用循环引用检测
        /// </summary>
        DisableCircularReferenceDetect,
        /// <summary>
        /// 斜线特殊处理
        /// </summary>
        WriteSlashAsSpecial,
        /// <summary>
        /// 浏览器兼容模式
        /// </summary>
        BrowserCompatible,
        /// <summary>
        /// 使用时间格式格式化时间，而非long
        /// </summary>
        WriteDateUseDateFormat,
        /// <summary>
        /// 是否写入根类名称
        /// </summary>
        NotWriteRootClassName,
        /// <summary>
        /// 不检查特殊字符
        /// </summary>
        DisableCheckSpecialChar,
        /// <summary>
        /// 忽略不支持的数据类型
        /// </summary>
        IgnoreNonSupportType
    }

    //private SerializerFeature(){
    //    mask = (1 << ordinal());
    //}

    //private int mask;

    //public int getMask() {
    //    return mask;
    //}

    //public static  isEnabled(int features, SerializerFeature feature) {
    //    return (features & feature.getMask()) != 0;
    //}

    //public static int config(int features, SerializerFeature feature, boolean state) {
    //    if (state) {
    //        features |= feature.getMask();
    //    } else {
    //        features &= ~feature.getMask();
    //    }

    //    return features;
    //}

}
