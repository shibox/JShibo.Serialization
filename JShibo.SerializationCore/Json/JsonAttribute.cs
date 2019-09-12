using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Json
{
    /// <summary>
    /// Json映射属性
    /// </summary>
    public class JsonAttribute:Attribute
    {
        /// <summary>
        /// 映射的名称
        /// </summary>
        public string Name { get; set; }
        ///// <summary>
        ///// 是否忽略该字段
        ///// </summary>
        //public bool Ignore { get; set; }
        ///// <summary>
        ///// 是否是自定义转换
        ///// </summary>
        //public bool Converter { get; set; }
        
    }

    /// <summary>
    /// 忽略的字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NotSerialized : Attribute
    {
    }

    /// <summary>
    /// 需要跟踪装配的程序集
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class TraceAssembly : Attribute
    {
    }


}
