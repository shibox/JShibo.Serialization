using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace JShibo.Serialization.BenchMark.Entitiy
{
    //[Serializable]
    public class TestClassAllType
    {
        Random rd = null;
        int count = 100;

        #region 基础类型

        bool boolValue = true;
        bool boolValueFalse = false;
        char charValue = '中';
        short shortValue = 23547;
        ushort ushortValue = 36877;
        int intValue = -9796;
        uint uintValue = 457;
        long longValue = 457789544;
        ulong ulongValue = 457789544;
        float floatValue = 4588;
        double doubleValue = 54615555;
        decimal decimalValue = 54615555;

        DateTime time = DateTime.Now;
        public DateTime Time { get { return time; } set { time = value; } }

        public DateTimeOffset dateTimeOffsetValue = new DateTimeOffset(DateTime.Now);
        public Guid guidValue = new Guid("337c7f2b-7a34-4f50-9141-bab9e6478cc8");
        public TimeSpan timeSpanValue = new TimeSpan(0, 45, 0);
        public Uri uriValue = new Uri("http://shibox.org/");

        public bool BoolValue { get { return boolValue; } set { boolValue = value; } }
        public bool BoolValueFalse { get { return boolValueFalse; } set { boolValueFalse = value; } }
        public int IntValue { get { return intValue; } set { intValue = value; } }
        public uint UintValue { get { return uintValue; } set { uintValue = value; } }
        public short ShortValue { get { return shortValue; } set { shortValue = value; } }
        public ushort UshortValue { get { return ushortValue; } set { ushortValue = value; } }

        public long LongValue { get { return longValue; } set { longValue = value; } }
        public ulong UlongValue { get { return ulongValue; } set { ulongValue = value; } }
        public float FloatValue { get { return floatValue; } set { floatValue = value; } }
        public double DoubleValue { get { return doubleValue; } set { doubleValue = value; } }
        public decimal DecimalValue { get { return decimalValue; } set { decimalValue = value; } }

        #endregion

        #region array or list

        //public long Name2 = 87821003254;
        //public float Name3 = 63679942.5F;
        //public double? Name4 = 45785545.55556;
        //public decimal Name5 = 646876955578;
        //public DateTime Time = DateTime.Now;
        //public char charValue = '中';
        //DateTime time = DateTime.Now;
        //public DateTime Time { get { return time; } set { time = value; } }
        //public TestClass2 class2 = new TestClass2();
        //public float Name3 = 63679942.5F;
        //public double Name4 = 45785545.55556;
        

        public IList<int> ilistInt32 = new List<int>();
        public IList<int> IListInt32 { get { return ilistInt32; } set { ilistInt32 = value; } }

        public List<int> listInt32 = new List<int>();
        public List<int> ListInt32 { get { return listInt32; } set { listInt32 = value; } }

        public int[] int32Array = new int[] { 457, 568 }; //new List<int>();
        public int[] Int32Array { get { return int32Array; } set { int32Array = value; } }

        //public TestClass3 t33 = null;
        //public TestClass3 t3 = new TestClass3();
        //public TestClass3 T3 { get { return t3; } set { t3 = value; } }

        public List<TestClass3> listTClass = new List<TestClass3>();
        public List<TestClass3> ListTClass { get { return listTClass; } set { listTClass = value; } }

        public List<object> listObjectClass = new List<object>();
        public List<object> ListObjectClass { get { return listObjectClass; } set { listObjectClass = value; } }

        //internal IDictionary dic = new Dictionary<string, string>();
        //public IDictionary Dic { get { return dic; } set { dic = value; } }

        public IDictionary objDic = new Dictionary<int, string>();
        public IDictionary Dic { get { return objDic; } set { objDic = value; } }

        //public string[] stringArray = new string[] { "图片是电子商务网站非常重要的一个数据", "异常规律的发现和识别并运用到排序中", "那么如何衡量不同算法之间的优劣呢？" };
        //public string[] stringArray = new string[] { "a", "b", "c" };
        public string[] stringArray = new string[] { "类似的商品，卖家，买家，搜索词等方面的特征因子有很多，一个排序模型就是把各种各样不同的特征因子组合起来，给出一个最终的关键词到商品的相关性 分数。只用其中的一到两个特征因子，已经可以对商品做一些最基本的排序。如果有更多的特征参与到排序，我们就可能得到一个更好的排序算法。组合的方法可以 有简单的人工配置到复杂的类似Learning to Rank等的学习模型。", "算法模型的评估一般分为线下的评估和线上的评估，线下的评估很多都体现在搜索中常用的相关性（Relevance）指标。相关性的定义可以分为狭义 相关性和广义相关性两方面，狭义相关性一般指检索结果和用户查询的相关程度。而从广义的层面，相关性可以理解为用户查询的综合满意度。当用户在搜索框输入 关键词，到需求获得满足，这之间经历的过程越顺畅，越便捷，搜索相关性就越好。", "广义的相关性线下评测比较困难，受人工主观因素的影响更大，一般使用SBS（Side by Side）的评测方法，针对一个关键词，把两个不同算法模型产出的结果同时展示在屏幕上，每次新模型和对比模型展示的位置关系都是随即的，人工判断的时候 不知道哪一边的数据是新模型的结果，人工判断那一边的搜索结果好，以最终的统计结果综合来衡量新模型和老模型的搜索表现。" };


        #endregion
        
        //public int[] Li = new int[] { 12, 34 };
        //public int[] LiNull = null;

        // int[] li2 = new int[] { 12, 34 };
        // public int[] Li2 { get { return li2; } set { li2 = value; } }



        public byte[] byteArrayValue = new byte[2] { 0x45, 0x23 };
        //public byte[] ByteArray { get { return byteArray; } set { byteArray = value; } }

        //public DateTime time = DateTime.Now;
        //public DateTime Time { get { return time; } set { time = value; } }

        //public DateTime[] times = new DateTime[] { DateTime.Now, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-50) };
        //public DateTime[] Times { get { return times; } set { times = value; } }

        public TestClassAllType()
            :this(DateTime.Now.Millisecond)
        {
            
        }

        public TestClassAllType(int seed)
        {
            rd = new Random(seed);

            //dic.Add("12", "34");
            //objDic.Add(45, "asd");

            //ilistInt32.Add(457);
            //ilistInt32.Add(568);

            for (int i = 0; i < count; i++)
            {
                ilistInt32.Add(i + 65432);
            }
            for (int i = 0; i < count; i++)
            {
                listInt32.Add(i + 65432);
            }

            int32Array = new int[count];
            for (int i = 0; i < int32Array.Length; i++)
            {
                int32Array[i] = i + 65432;
            }

            count = rd.Next(1,100);

            for (int i = 0; i < count; i++)
                listTClass.Add(new TestClass3());

            listObjectClass.Add(new TestClass3());
            listObjectClass.Add(new TestClass4());
            listObjectClass.Add(new TestClass5());
            listObjectClass.Add(new TestClass6());

            //listObjectClass.Add(new TestClass3());

        }
    }

    public class TestClassAllValueType
    {
        //public long Name2 = 87821003254;
        //public float Name3 = 63679942.5F;
        //public double? Name4 = 45785545.55556;
        //public decimal Name5 = 646876955578;
        //public DateTime Time = DateTime.Now;
        //public char charValue = '中';
        //DateTime time = DateTime.Now;
        //public DateTime Time { get { return time; } set { time = value; } }
        //public TestClass2 class2 = new TestClass2();
        //public float Name3 = 63679942.5F;
        //public double Name4 = 45785545.55556;
        //public TimeSpan timeSpanValue = new TimeSpan(0, 12324567, 0);
        //public DateTimeOffset dateTimeOffsetValue = new DateTimeOffset(DateTime.Now);
        //public Guid guidValue = new Guid("337c7f2b-7a34-4f50-9141-bab9e6478cc8");
        //public Uri uriValue = new Uri("http://www.shibox.org/");
        //public object objectValue = new TestClass3();
        //public object objectValue1 = new TestClass3();
        //public Guid guidValue = new Guid("337c7f2b-7a34-4f50-9141-bab9e6478cc8");
        //public Uri uriValue = new Uri("http://www.shibox.org/");
        public object objectValue2 = new TestClass4();
        public object objectValue3 = null;
        public object objectValue4 = new TestClass4();
    }

    //[Serializable]
    [ProtoContract]
    public class TestClass3
    {
        //public string className = "   def";
        int name = 51247895;
        short name1 = 9796;

        [ProtoMember(1)]  
        public int Name { get { return name; } set { name = value; } }
        //public short Name1 { get { return name1; } set { name1 = value; } }

        //public long Name2 = 87821003254;
        //public float Name3 = 63679942.5F;
        //TestClass TestClass1 = new TestClass();
        //public double Name4 = 45785545.55556;
        //public decimal Name5 = 646876955578;
    }

    //[Serializable]
    public class TestClass4
    {
        //public string className = "   def";
        int name = 51247895;
        short name1 = 9796;

        public TestClass6 t6 = new TestClass6();

        public TestClass5 t5 = new TestClass5();

        public int Name { get { return name; } set { name = value; } }
        //public short Name1 { get { return name1; } set { name1 = value; } }

        //public long Name2 = 87821003254;
        //public float Name3 = 63679942.5F;
        //TestClass TestClass1 = new TestClass();
        //public double Name4 = 45785545.55556;
        //public decimal Name5 = 646876955578;
    }

    //[Serializable]
    public class TestClass5
    {
        //public string className = "   def";
        int name = 51247895;
        short name1 = 9796;

        List<TestClass3> listTClass = new List<TestClass3>();
        public List<TestClass3> ListTClass { get { return listTClass; } set { listTClass = value; } }

        public int Name { get { return name; } set { name = value; } }
        //public short Name1 { get { return name1; } set { name1 = value; } }

        //public long Name2 = 87821003254;
        //public float Name3 = 63679942.5F;
        //TestClass TestClass1 = new TestClass();
        //public double Name4 = 45785545.55556;
        //public decimal Name5 = 646876955578;

        public TestClass5()
        {
            listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            listTClass.Add(null);
            listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
            //listTClass.Add(new TestClass3());
        }
    }

    //[Serializable]
    public class TestClass6
    {
        internal IDictionary dic = new Dictionary<string, string>();
        //public IDictionary Dic { get { return dic; } set { dic = value; } }

        //IList<int> ilistInt32 = new int[] { 457, 568 }; //new List<int>();
        //public IList<int> IListInt32 { get { return ilistInt32; } set { ilistInt32 = value; } }

        IList ilistInt32 = new int[] { 51247895, 51247895, 51247895, 51247895, 51247895, 51247895 }; //new List<int>();
        //IList ilistInt32 = new List<int>();
        public IList IListInt32 { get { return ilistInt32; } set { ilistInt32 = value; } }

        //public VVAA? va = null;

        public TestClass6()
        {
            //ilistInt32.Add(48);
            dic.Add("51247895", "51247895");
        }
    }

    public class TestClass7
    {
        //[JsonAttribute(Name = "vvv")]
        //[NotSerialized]
        //public int int32Value = 123;
        public bool boolValue = true;
    }

    public class EnumClass
    {
        public AB V = AB.B;
        public int va = 123;
    }

    public class ArraySegmentClass
    {
        public ArraySegment<int> V;

        public ArraySegmentClass()
        {
            int[] array = new int[10];
            array[1] = 12;
            V = new ArraySegment<int>(array,0,2);
        }
    }

    public enum AB
    { 
        A,
        B,
    }

    public struct ValueTypeTestA
    {
        int a;
        int b;

        public int A
        {
            get { return a; }
        }
        public int B
        {
            get { return b; }
        }

        public ValueTypeTestA(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
    }

    public struct ValueTypeTestB
    {
        public int[] a;
        public int b;

        //public ValueTypeTestB(int[] a)
        //{
        //    this.a = a;
        //}

        public ValueTypeTestB(int[] a, int b)
        {
            this.a = a;
            this.b = b;
        }
    }

    public struct ValueTypeTestC
    {
        int a;
        string b;

        public int A
        {
            get { return a; }
        }
        public string B
        {
            get { return b; }
        }

        public ValueTypeTestC(int a, string b)
        {
            this.a = a;
            this.b = b;
        }
    }

    public struct ValueTypeTestD
    {
        int a;
        byte b;

        public int A
        {
            get { return a; }
        }
        public byte B
        {
            get { return b; }
        }

        public ValueTypeTestD(int a, byte b)
        {
            this.a = a;
            this.b = b;
        }
    }

    public struct ValueTypeTestE
    {
        public int[] a;

        public ValueTypeTestE(int[] a)
        {
            this.a = a;
        }
    }

    public struct ValueTypeTestF
    {
        public string a;

        public ValueTypeTestF(string a)
        {
            this.a = a;
        }
    }

    public struct ValueTypeTestG
    {
        public string a;
        public string b;

        public ValueTypeTestG(string a, string b)
        {
            this.a = a;
            this.b = b;
        }
    }

    public struct ValueTypeTestH
    {
        public int a;
        public string b;

        public ValueTypeTestH(int a, string b)
        {
            this.a = a;
            this.b = b;
        }
    }

    #region [   data objects   ]

    public class baseclass
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class class1 : baseclass
    {
        public class1() { }
        public class1(string name, string code, Guid g)
        {
            Name = name;
            Code = code;
            guid = g;
        }
        public Guid guid { get; set; }
    }

    public class class2 : baseclass
    {
        public class2() { }
        public class2(string name, string code, string desc)
        {
            Name = name;
            Code = code;
            description = desc;
        }
        public string description { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public class colclass
    {
        public colclass()
        {
            items = new List<baseclass>();
            date = DateTime.Now;
            multilineString = "abc";
//            multilineString = @"
//            AJKLjaskljLA
//       ahjksjkAHJKS سلام فارسی
//       AJKHSKJhaksjhAHSJKa
//       AJKSHajkhsjkHKSJKash
//       ASJKhasjkKASJKahsjk
//            ";
            isNew = true;
            booleanValue = true;
            ordinaryDouble = 0.001;
            //gender = Gender.Female;
            intarray = new int[5] { 1, 2, 3, 4, 5 };
        }
        public bool booleanValue { get; set; }
        public DateTime date { get; set; }
        public string multilineString { get; set; }
        public List<baseclass> items { get; set; }
        public decimal ordinaryDecimal { get; set; }
        public double ordinaryDouble { get; set; }
        public bool isNew { get; set; }
        public string laststring { get; set; }
        public Gender gender { get; set; }

//        //public DataSet dataset { get; set; }
        public Dictionary<string, baseclass> stringDictionary { get; set; }
        public Dictionary<baseclass, baseclass> objectDictionary { get; set; }
        public Dictionary<int, baseclass> intDictionary { get; set; }
        public Guid? nullableGuid { get; set; }
        public decimal? nullableDecimal { get; set; }
        public double? nullableDouble { get; set; }

        public Hashtable hash { get; set; }
        public baseclass[] arrayType { get; set; }
        public byte[] bytes { get; set; }
        public int[] intarray { get; set; }

    }
    #endregion



}
