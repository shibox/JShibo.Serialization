using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.BenchMark.Entitiy
{
    [Serializable]
    public class Int8Class
    {
        public byte V0 { get; set; }
        public byte V1 { get; set; }
        public byte V2 { get; set; }
        public byte V3 { get; set; }
        public byte V4 { get; set; }
        public byte V5 { get; set; }
        public byte V6 { get; set; }
        public byte V7 { get; set; }
        public byte V8 { get; set; }
        public byte V9 { get; set; }

        public Int8Class()
        {

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('{');

            WriteName(sb, "V0");
            sb.Append(V0.ToString() + ",");
            WriteName(sb, "V1");
            sb.Append(V1.ToString() + ",");
            WriteName(sb, "V2");
            sb.Append(V2.ToString() + ",");
            WriteName(sb, "V3");
            sb.Append(V3.ToString() + ",");
            WriteName(sb, "V4");
            sb.Append(V4.ToString() + ",");
            WriteName(sb, "V5");
            sb.Append(V5.ToString() + ",");
            WriteName(sb, "V6");
            sb.Append(V6.ToString() + ",");
            WriteName(sb, "V7");
            sb.Append(V7.ToString() + ",");
            WriteName(sb, "V8");
            sb.Append(V8.ToString() + ",");
            WriteName(sb, "V9");
            sb.Append(V9.ToString() + ",");
            sb.Length--;
            
            sb.Append('}');
            return sb.ToString();
        }

        private static void WriteName(StringBuilder sb, string name)
        {
            sb.Append("\"" + name + "\":");
        }
    }

    [Serializable]
    public class Int16Class
    {
        public short V0 { get; set; }
        public short V1 { get; set; }
        public short V2 { get; set; }
        public short V3 { get; set; }
        public short V4 { get; set; }
        public short V5 { get; set; }
        public short V6 { get; set; }
        public short V7 { get; set; }
        public short V8 { get; set; }
        public short V9 { get; set; }

        public Int16Class()
        {

        }
    }

    [Serializable]
    [ProtoBuf.ProtoContract]
    public class Int32Class
    {
        [ProtoBuf.ProtoMember(1)]
        public int V0 { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public int V1 { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public int V2 { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public int V3 { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public int V4 { get; set; }
        [ProtoBuf.ProtoMember(6)]
        public int V5 { get; set; }
        [ProtoBuf.ProtoMember(7)]
        public int V6 { get; set; }
        [ProtoBuf.ProtoMember(8)]
        public int V7 { get; set; }
        [ProtoBuf.ProtoMember(9)]
        public int V8 { get; set; }
        [ProtoBuf.ProtoMember(10)]
        public int V9 { get; set; }

        public Int32Class()
        { 
        
        }
    }

    [Serializable]
    public class Int32FieldClass
    {
        public int V0 ;
        public int V1 ;
        public int V2 ;
        public int V3 ;
        public int V4 ;
        public int V5 ;
        public int V6 ;
        public int V7 ;
        public int V8 ;
        public int V9 ;

        public Int32FieldClass()
        {

        }
    }

    [Serializable]
    public class Int64Class
    {
        public long V0 { get; set; }
        public long V1 { get; set; }
        public long V2 { get; set; }
        public long V3 { get; set; }
        public long V4 { get; set; }
        public long V5 { get; set; }
        public long V6 { get; set; }
        public long V7 { get; set; }
        public long V8 { get; set; }
        public long V9 { get; set; }

        public Int64Class()
        {

        }
    }

    [Serializable]
    public class FloatClass
    {
        public float V0 { get; set; }
        public float V1 { get; set; }
        public float V2 { get; set; }
        public float V3 { get; set; }
        public float V4 { get; set; }
        public float V5 { get; set; }
        public float V6 { get; set; }
        public float V7 { get; set; }
        public float V8 { get; set; }
        public float V9 { get; set; }

        public FloatClass()
        {

        }
    }

    [Serializable]
    public class DoubleClass
    {
        public double V0 { get; set; }
        public double V1 { get; set; }
        public double V2 { get; set; }
        public double V3 { get; set; }
        public double V4 { get; set; }
        public double V5 { get; set; }
        public double V6 { get; set; }
        public double V7 { get; set; }
        public double V8 { get; set; }
        public double V9 { get; set; }

        public DoubleClass()
        {

        }
    }

    [Serializable]
    public class GuidClass
    {
        public Guid V0 { get; set; }
        public Guid V1 { get; set; }
        public Guid V2 { get; set; }
        public Guid V3 { get; set; }
        public Guid V4 { get; set; }
        public Guid V5 { get; set; }
        public Guid V6 { get; set; }
        public Guid V7 { get; set; }
        public Guid V8 { get; set; }
        public Guid V9 { get; set; }

        public GuidClass()
        {

        }
    }

    [Serializable]
    public class DateTimeClass
    {
        public DateTime V0 { get; set; }
        public DateTime V1 { get; set; }
        public DateTime V2 { get; set; }
        public DateTime V3 { get; set; }
        public DateTime V4 { get; set; }
        public DateTime V5 { get; set; }
        public DateTime V6 { get; set; }
        public DateTime V7 { get; set; }
        public DateTime V8 { get; set; }
        public DateTime V9 { get; set; }

        public DateTimeClass()
        {

        }
    }

    public class ValueTypeClass : PrimitiveTypeClass
    {
        public DateTimeOffset V12 { get; set; }
        public TimeSpan V13 { get; set; }
        public Guid V14 { get; set; }
        public Uri V15 { get; set; }

        public ValueTypeClass()
        { 
        
        }
    }

    public class PrimitiveTypeClass
    {
        public byte V0 { get; set; }
        public sbyte V1 { get; set; }
        public short V2 { get; set; }
        public ushort V3 { get; set; }
        public int V4 { get; set; }
        public uint V5 { get; set; }
        public long V6 { get; set; }
        public ulong V7 { get; set; }
        //public float V8 { get; set; }
        //public double V9 { get; set; }
        public string V10 { get; set; }
        //public DateTime V11 { get; set; }
        public bool V12 { get; set; }
        public char V13 { get; set; }

        public PrimitiveTypeClass()
        {
            
        }
    }

    public class Int32ArrayClass
    {
        //public uint[] V1 { get; set; }
        //public uint[] V2 { get; set; }
        //public int V3 { get; set; }
    }

    public class Int32ArrayClassB
    {
        public List<int> V1 { get; set; }
        public List<int> V2 { get; set; }

        //public IEnumerable<int> V1 { get; set; }
        //public IEnumerable<int> V2 { get; set; }
    }

    public class Int32ArrayClassC
    {
        //public int[] V1 { get; set; }
        public ArraySegment<int> V1 { get; set; }
        public ArraySegment<int> V2 { get; set; }
    }

    public class StringListClass
    {
        public List<string> V1 { get; set; }
        public List<string> V2 { get; set; }
        //public List<int> V3 { get; set; }
        //public IEnumerable<int> V1 { get; set; }
        //public IEnumerable<int> V2 { get; set; }
    }

    public class ValueTypeArrayClass
    {
        Random rd = null;

        public byte[] V0 { get; set; }
        public sbyte[] V1 { get; set; }
        public short[] V2 { get; set; }
        public ushort[] V3 { get; set; }
        public int[] V4 { get; set; }
        public uint[] V5 { get; set; }
        public long[] V6 { get; set; }
        public ulong[] V7 { get; set; }
        public float[] V8 { get; set; }
        public double[] V9 { get; set; }
        public string[] V10 { get; set; }
        public DateTime[] V11 { get; set; }
        public DateTimeOffset[] V12 { get; set; }
        public TimeSpan[] V13 { get; set; }
        public Guid[] V14 { get; set; }
        public Uri[] V15 { get; set; }

        public ValueTypeArrayClass()
        { 
        
        }

        //public ValueTypeArrayClass(int seed)
        //{
        //    rd = new Random(seed);
        //    V0 = new byte[100];
        //    rd.NextBytes(V0);

        //    V1 = new sbyte[rd.Next(TestBaseConfig.ArrayMinSize, TestBaseConfig.ArrayMaxSize)];
        //    for (int i = 0; i < V1.Length; i++)
        //        V1[i] = (sbyte)rd.Next(sbyte.MinValue, sbyte.MaxValue);

        //    V2 = new short[rd.Next(TestBaseConfig.ArrayMinSize, TestBaseConfig.ArrayMaxSize)];
        //    for (int i = 0; i < V2.Length; i++)
        //        V2[i] = (short)rd.Next(sbyte.MinValue, sbyte.MaxValue);

        //    V3 = new ushort[rd.Next(TestBaseConfig.ArrayMinSize, TestBaseConfig.ArrayMaxSize)];
        //    for (int i = 0; i < V3.Length; i++)
        //        V3[i] = (ushort)rd.Next(sbyte.MinValue, sbyte.MaxValue);

        //    V4 = new int[rd.Next(TestBaseConfig.ArrayMinSize, TestBaseConfig.ArrayMaxSize)];
        //    for (int i = 0; i < V4.Length; i++)
        //        V4[i] = (int)rd.Next(sbyte.MinValue, sbyte.MaxValue);

        //    V5 = new uint[rd.Next(TestBaseConfig.ArrayMinSize, TestBaseConfig.ArrayMaxSize)];
        //    for (int i = 0; i < V5.Length; i++)
        //        V5[i] = (uint)rd.Next(sbyte.MinValue, sbyte.MaxValue);

        //    V6 = new long[rd.Next(TestBaseConfig.ArrayMinSize, TestBaseConfig.ArrayMaxSize)];
        //    for (int i = 0; i < V6.Length; i++)
        //        V6[i] = (long)rd.Next(sbyte.MinValue, sbyte.MaxValue);

        //    V7 = new ulong[rd.Next(TestBaseConfig.ArrayMinSize, TestBaseConfig.ArrayMaxSize)];
        //    for (int i = 0; i < V7.Length; i++)
        //        V7[i] = (ulong)rd.Next(sbyte.MinValue, sbyte.MaxValue);
        //}

        //public static ValueTypeArrayClass Init()
        //{
        //    return new ValueTypeArrayClass(TestBaseConfig.Seed);
        //}
    }

    public class StringClass
    {
        public string V1 { get; set; }
        public string V2 { get; set; }
        public string V3 { get; set; }
        public string V4 { get; set; }
        public string V5 { get; set; }

        public StringClass()
        {
           

        }

        //public static StringClass Init()
        //{
        //    StringClass v = new StringClass();
        //    v.V1 = "除了以上提到的企业，Gurin还从OpenData500提供的首批企业列表中挑选了五家典型企业进行了详细介绍。这些企业不仅对开放数据进行了创新应用，还进一步通过OpenAPI公开了自己的数据。";
        //    v.V2 = "在不久以前，如果要创建一个新的编程语言还是比较麻烦的，因为这需要将代码转换成bit才能构建各种程序。然而后来有人想出了更好的方法：那就是在着手步骤三的时候可以提前处理步骤四的工作。只不过现在只要编写一个预处理器就能将新的代码转换成一组带有多个类库和API的旧代码。";
        //    v.V3 = "在相当长的一段时间里，每个程序员都要学会如何利用JavaScript来编写弹出一个警告框或查看包含@符号的电子邮件之类的程序。而现如今，HTML AJAX App变得复杂了，以至于很少有人从头开始来学习它们。相反，像使用一个精心设计的框架、编写一些粘合代码来实现业务逻辑的方式更容易让人们接受。类似的框架如： Kendo、Sencha、jQuery Mobile、AngularJS、Ember、Backbone、Meteor JS等等，这些都可以帮助你处理Web App和网页上的事件和内容，大大的节省了时间。";
        //    v.V4 = "曾几何时，只要是在Web页面花点时间就能打开CSS文件，还包括一个新的命令，像font-style:italic，接下来只需要利用一上午的时间就能把所有事情搞定。而现在的网页设计则相对复杂些，而且也不可能利用这么简单的命令就可以填补一个文件。";
        //    v.V5 = "可以这么说，CSS框架是SASS和Compass最坚实、最牢固的基础，CSS框架能够提供类似于实际变量、嵌套模块和混合之类的组件，这样有助于创建高质量、更稳定的编码程序。这听起来并不像是编程领域里的新奇事物，但是这在设计领域里几乎是一个巨大的飞跃。";
        //    return v;
        //}
    }

    [Contain]
    public class MixClass
    {
        Random rd = null;
        //public byte V0 { get; set; }
        //public sbyte V1 { get; set; }
        //public short V2 { get; set; }
        //public ushort V3 { get; set; }
        public int V4 { get; set; }
        public Int32Class V5 { get; set; }
        //public int[] V6 { get; set; }
        [Contain]
        public int V7 { get; set; }
        public string V8 { get; set; }

        public MixClass()
        { 

        }

        public MixClass(int seed)
        {
            rd = new Random(seed);
            //V0 = (byte)rd.Next(int.MinValue, int.MaxValue);
            //V1 = (sbyte)rd.Next(int.MinValue, int.MaxValue);
            //V2 = (short)rd.Next(int.MinValue, int.MaxValue);
            //V3 = (ushort)rd.Next(int.MinValue, int.MaxValue);
            V4 = rd.Next(int.MinValue, int.MaxValue);
            V5 = ShiboSerializer.Initialize<Int32Class>(); //Int32Class.Init();
            //V6 = new int[rd.Next(TestBaseConfig.ArrayMinSize, TestBaseConfig.ArrayMaxSize)];
            //for (int i = 0; i < V6.Length; i++)
            //    V6[i] = (int)rd.Next(sbyte.MinValue, sbyte.MaxValue);
            V7 = rd.Next(int.MinValue, int.MaxValue);
        }

        public static MixClass Init()
        {
            return new MixClass(TestBaseConfig.Seed);
        }
    }

}
