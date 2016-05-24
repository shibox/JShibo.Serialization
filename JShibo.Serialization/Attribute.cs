using JShibo.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization
{
    /// <summary>
    /// 统一通用的包含标记，如果在类中标记了该属性，表示下面的字段或属性必须有该标记才会
    /// 被序列化
    /// </summary>
    public class ContainAttribute : Attribute
    {
        public ContainAttribute()
        {

        }
    }

    /// <summary>
    /// 统一通用的忽略标记，如果在类中标记了该属性，表示下面的字段或属性必须有该标记才会
    /// 被忽略
    /// </summary>
    public class IgnoreAttribute : Attribute
    {
        public IgnoreAttribute()
        {

        }
    }

    /// <summary>
    /// 名称属性
    /// </summary>
    public class NameAttribute : Attribute
    {
        public string Name { get; set; }

        public NameAttribute()
        {

        }

        public NameAttribute(string name)
        {
            this.Name = name;
        }
    }

    /// <summary>
    /// 数据检查属性
    /// </summary>
    public class CheckAttribute : Attribute
    {
        public static CheckAttribute Default = new CheckAttribute();

        public CheckEscape Check { get; set; }

        public CheckAttribute()
        {
            Check = CheckEscape.None;
        }
    }

    /// <summary>
    /// 字符串的编码属性
    /// </summary>
    public class EncodingAttribute : Attribute
    {
        public string Encoder { get; set; }

        public EncodingAttribute()
        {
            //Encoder = Encoding.Unicode;
            Encoder = Encoding.Unicode.BodyName;
        }
    }



}
