using JShibo.Serialization;
using JShibo.Serialization.Json;
using JShibo.Serialization.Soc;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using JShibo.Serialization.Csv;
using System.IO;

namespace JShibo.Serialization
{

    public class XType<Data, Info, Size>
    {
        #region old
        //internal static Dictionary<Type, Type[]> deTypes;
        //internal static Dictionary<Type, Serialize<Data>> jsonTypeMap;
        //internal static Dictionary<Type, Serialize<Size>> jsonTypeSizeMap;
        //internal static Dictionary<Type, Deserialize<Data>> jsonDeTypeMap;
        //internal static Dictionary<Type, Info> jsonTypes;
        //internal static Dictionary<Type, string[]> namesMap;
        #endregion

        internal Type[] deTypes;
        internal Serialize<Data> jsonTypeMap;
        internal Estimate<Size> jsonTypeSizeMap;
        internal Deserialize<Data> jsonDeTypeMap;
        internal Info jsonTypes;
        internal string[] namesMap;

        public XType()
        { 
        
        }
    }

    public class ConvertContext<Data, UData>
    {
        internal SerializerSettings Seting = SerializerSettings.Default;
        internal Serialize<Data> Serializer;
        internal Deserialize<UData> Deserializer;
        internal List<string> NamesList;
        internal string[] Names;
        internal List<int> NameCountsList;
        internal int[] NameCounts;

        internal virtual void ToArray()
        {
            Names = NamesList.ToArray();
            NameCounts = NameCountsList.ToArray();
        }

    }

    public class ObjectContext<Data, UData, Size>:ConvertContext<Data, UData>
    {
        internal Estimate<Size> EstimateSize;
        internal int ObjectCount = 0;
        /// <summary>
        /// 序列化的对象是否是基础类型，如int
        /// </summary>
        internal bool IsBaseType = false;
        /// <summary>
        /// 被序列化的对象是否全是基元类型，如int
        /// </summary>
        internal bool IsAllBaseType = true;
        internal bool IsHaveObjectType = false;
        /// <summary>
        /// 被序列化的对象是否全是固定可估算出大小的类型，如int，datetime等
        /// </summary>
        internal bool IsAllFixedSize = true;
        
        internal List<Serialize<Data>> SerializeList;
        internal List<Deserialize<UData>> DeserializeList;
        internal List<Estimate<Size>> EstimateSizeList;
        internal List<Type> TypesList;
        internal List<int> TypeCountsList;
        
        internal Serialize<Data>[] Serializers;
        internal Estimate<Size>[] SizeSerializers;
        internal Deserialize<UData>[] Deserializers;
        internal Type[] Types;
        internal int[] TypeCounts;
        /// <summary>
        /// 该类型最小占用的容量
        /// </summary>
        internal int MinSize = 0;
        /// <summary>
        /// 定义的容量大小
        /// </summary>
        internal int DefineSize = 16;

        internal ObjectContext()
        {
            SerializeList = new List<Serialize<Data>>();
            EstimateSizeList = new List<Estimate<Size>>();
            DeserializeList = new List<Deserialize<UData>>();
            TypesList = new List<Type>();
            TypeCountsList = new List<int>();
            
        }

        internal override void ToArray()
        {
            base.ToArray();
            Serializers = SerializeList.ToArray();
            SizeSerializers = EstimateSizeList.ToArray();
            Deserializers = DeserializeList.ToArray();
            Types = TypesList.ToArray();
            TypeCounts = TypeCountsList.ToArray();
        }

    }

    public class TextContext<Data, UData, Size> : ObjectContext<Data, UData, Size>
    {
        internal List<CheckAttribute> ChecksList;
        internal CheckAttribute[] checks;
        private string[] NamesCamelCase;
        /// <summary>
        /// 是否存在大小写名称相同的情况
        /// </summary>
        internal bool IsHaveUpperLowerSame = false;

        internal TextContext()
        {
            NamesList = new List<string>();
            ChecksList = new List<CheckAttribute>();
            NameCountsList = new List<int>();

            //MinSize = 32;
        }

        internal override void ToArray()
        {
            base.ToArray();
            //Names = NamesList.ToArray();
            checks = ChecksList.ToArray();
            //NameCounts = NameCountsList.ToArray();
        }

        internal string[] GetNamesCamelCase()
        {
            if (NamesCamelCase == null)
            {
                List<string> namesCamelCaseList = new List<string>(NamesList.Count);
                foreach (string name in NamesList)
                {
                    if (name[0] > 96 && name[0] < 123)
                    {
                        char[] chs = name.ToCharArray();
                        chs[0] = (char)(chs[0] - 32);
                        namesCamelCaseList.Add(new string(chs));
                    }
                    else
                        namesCamelCaseList.Add(name);
                }
                NamesCamelCase = namesCamelCaseList.ToArray();
            }
            return NamesCamelCase;
        }
    }

    public class JsonContext<Data, UData, Size> : TextContext<Data, UData, Size>
    {
        internal int[] NameLens;
        internal byte[] NamesBlock;

        internal void CreateBlock()
        {
            List<int> nameLensList = new List<int>();
            int totalLen = 0, cur = 0;
            foreach (string name in NamesList)
                totalLen += Encoding.UTF8.GetByteCount(name);
            NamesBlock = new byte[totalLen];
            foreach (string name in NamesList)
            {
                int len = Encoding.UTF8.GetBytes(name, 0, name.Length, NamesBlock, cur);
                cur += len;
                nameLensList.Add(len);
            }
            NameLens = nameLensList.ToArray();
        }
    }

    public class CsvContext<Data, UData, Size> : TextContext<Data, UData, Size>
    {
        public string NamesCommaString;
        public string NamesSplitString;
        public string NamesSpaceString;

        public byte[] NamesCommaBytes;
        public byte[] NamesSplitBytes;
        public byte[] NamesSpaceBytes;

        public void FormatNames()
        {
            NamesCommaString = string.Join(',', Names);
            NamesSplitString = string.Join('|', Names);
            NamesSpaceString = string.Join('\t', Names);

            NamesCommaBytes = Encoding.UTF8.GetBytes(NamesCommaString);
            NamesSplitBytes = Encoding.UTF8.GetBytes(NamesSplitString);
            NamesSpaceBytes = Encoding.UTF8.GetBytes(NamesSpaceString);
        }
    }

    public class XmlContext<Data, UData, Size> : TextContext<Data, UData, Size>
    {

    }

    //public class PivotContext<Data, UData> : ConvertContext<Data, UData>
    //{

    //}

    

    public class ObjectInitializeContext<UData>
    {
        internal SerializerSettings Seting = SerializerSettings.Default;
        internal Deserialize<UData> Deserializer;
        internal List<Deserialize<UData>> DesList;
        internal Deserialize<UData>[] Deserializes;
        /// <summary>
        /// 定义的容量大小
        /// </summary>
        internal int DefineSize = 16;

        internal ObjectInitializeContext()
        {
            DesList = new List<Deserialize<UData>>();
        }

        internal virtual void ToArray()
        {
            Deserializes = DesList.ToArray();
        }

    }

    //public class ConvertContext<Data, UData,Size>: TextContext<Data, UData, Size>
    //{
        

    //}

}
