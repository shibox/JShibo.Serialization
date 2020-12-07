using JShibo.Serialization.Common;
using JShibo.Serialization.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace JShibo.Serialization
{
    internal abstract class IBuilder
    {
        #region 字段

        Dictionary<Type, Dictionary<Type, MethodInfo>> typeMethods = new Dictionary<Type, Dictionary<Type, MethodInfo>>();
        //默认的绑定标记
        static BindingFlags flag = BindingFlags.NonPublic |
                                   BindingFlags.Instance |
                                   BindingFlags.Public;

        #endregion

        #region 读写方法

        /// <summary>
        /// 创建读取数据的方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal virtual string CreateReaderMethod(Type type)
        {
            if (type == typeof(Int32))
                return "ReadInt32";

            if (type == typeof(UInt32))
                return "ReadUInt32";

            if (type == typeof(UInt64))
                return "ReadUInt64";

            if (type == typeof(Int64))
                return "ReadInt64";

            if (type == typeof(char))
                return "ReadChar";

            if (type == typeof(char[]))
                return "ReadChars";

            if (type == typeof(UInt16))
                return "ReadUInt16";

            if (type == typeof(Int16))
                return "ReadInt16";

            else if (type == typeof(string))
                return "ReadString";

            else if (type == typeof(DateTime))
                return "ReadDateTime";

            else if (type == typeof(long))
                return "ReadInt64";

            else if (type == typeof(ulong))
                return "ReadUInt64";

            else if (type == typeof(bool))
                return "ReadBoolean";

            else if (type == typeof(byte))
                return "ReadByte";

            else if (type == typeof(sbyte))
                return "ReadSByte";

            else if (type == typeof(decimal))
                return "ReadDecimal";

            else if (type == typeof(float))
                return "ReadSingle";

            else if (type == typeof(double))
                return "ReadDouble";

            
            else if (type == typeof(TimeSpan))
                return "ReadTimeSpan";
            else if (type == typeof(DateTimeOffset))
                return "ReadDateTimeOffset";
            else if (type == typeof(Guid))
                return "ReadGuid";
            //else if (type == typeof(IDictionary))//currenly supports a string dictionary only
            //    return "ReadString";
            //else if (type == typeof(IList))
            //    return "ReadString";
            //else if (type == typeof(Enum))
            //    return "ReadEnum";
            //else if (type == typeof(DataTable))
            //    return "ReadString";
            //else if (type == typeof(DataSet))
            //    return "ReadString";
            //else if (type == typeof(Hashtable))
            //    return "ReadString";


            else if (type == typeof(bool[]))
                return "ReadBools";
            else if (type == typeof(byte[]))
                return "ReadBytes";
            else if (type == typeof(sbyte[]))
                return "ReadSBytes";
            else if (type == typeof(short[]))
                return "ReadShorts";
            else if (type == typeof(ushort[]))
                return "ReadUShorts";
            else if (type == typeof(char[]))
                return "ReadChars";
            else if (type == typeof(int[]))
                return "ReadInts";
            else if (type == typeof(uint[]))
                return "ReadUInts";
            else if (type == typeof(float[]))
                return "ReadFloats";
            else if (type == typeof(double[]))
                return "ReadDoubles";
            else if (type == typeof(decimal[]))
                return "ReadDecimals";
            else if (type == typeof(string[]))
                return "ReadStrings";
            else if (type == typeof(DateTime[]))
                return "ReadDateTimes";
            else if (type == typeof(TimeSpan[]))
                return "ReadTimeSpans";
            else if (type == typeof(DateTimeOffset[]))
                return "ReadDateTimeOffsets";
            else if (type == typeof(Enum[]))
                return "ReadEnums";
            else if (type == typeof(Guid[]))
                return "ReadGuids";


            else if (type == typeof(bool[][]))
                return "ReadArrayBools";
            else if (type == typeof(byte[][]))
                return "ReadArrayBytes";


            else if (type == typeof(ArraySegment<bool>))
                return "ReadBoolArraySegment";
            else if (type == typeof(ArraySegment<byte>))
                return "ReadByteArraySegment";
            else if (type == typeof(ArraySegment<sbyte>))
                return "ReadSByteArraySegment";
            else if (type == typeof(ArraySegment<short>))
                return "ReadShortArraySegment";
            else if (type == typeof(ArraySegment<ushort>))
                return "ReadUShortArraySegment";
            else if (type == typeof(ArraySegment<char>))
                return "ReadCharArraySegment";
            else if (type == typeof(ArraySegment<int>))
                return "ReadIntArraySegment";
            else if (type == typeof(ArraySegment<uint>))
                return "ReadUIntArraySegment";
            else if (type == typeof(ArraySegment<float>))
                return "ReadFloatArraySegment";
            else if (type == typeof(ArraySegment<double>))
                return "ReadDoubleArraySegment";
            else if (type == typeof(ArraySegment<decimal>))
                return "ReadDecimalArraySegment";
            else if (type == typeof(ArraySegment<string>))
                return "ReadStringArraySegment";
            else if (type == typeof(ArraySegment<DateTime>))
                return "ReadDateTimeArraySegment";
            else if (type == typeof(ArraySegment<TimeSpan>))
                return "ReadTimeSpanArraySegment";
            else if (type == typeof(ArraySegment<DateTimeOffset>))
                return "ReadDateTimeOffsetArraySegment";
            else if (type == typeof(ArraySegment<Enum>))
                return "ReadEnumArraySegment";
            else if (type == typeof(ArraySegment<Guid>))
                return "ReadGuidArraySegment";
            

                //读取数组数据
            else if (type == typeof(List<bool>) || type == typeof(IList<bool>) || type == typeof(IEnumerable<bool>))
                return "ReadBoolList";
            else if (type == typeof(List<byte>) || type == typeof(IList<byte>) || type == typeof(IEnumerable<byte>))
                return "ReadByteList";
            else if (type == typeof(List<sbyte>) || type == typeof(IList<sbyte>) || type == typeof(IEnumerable<sbyte>))
                return "ReadSByteList";
            else if (type == typeof(List<short>) || type == typeof(IList<short>) || type == typeof(IEnumerable<short>))
                return "ReadShortList";
            else if (type == typeof(List<ushort>) || type == typeof(IList<ushort>) || type == typeof(IEnumerable<ushort>))
                return "ReadUShortList";
            else if (type == typeof(List<char>) || type == typeof(IList<char>) || type == typeof(IEnumerable<char>))
                return "ReadCharList";
            else if (type == typeof(List<int>) || type == typeof(IList<int>) || type == typeof(IEnumerable<int>))
                return "ReadIntList";
            else if (type == typeof(List<uint>) || type == typeof(IList<uint>) || type == typeof(IEnumerable<uint>))
                return "ReadUIntList";
            else if (type == typeof(List<float>) || type == typeof(IList<float>) || type == typeof(IEnumerable<float>))
                return "ReadFloatList";
            else if (type == typeof(List<double>) || type == typeof(IList<double>) || type == typeof(IEnumerable<double>))
                return "ReadDoubleList";
            else if (type == typeof(List<decimal>) || type == typeof(IList<decimal>) || type == typeof(IEnumerable<decimal>))
                return "ReadDecimalList";
            else if (type == typeof(List<string>) || type == typeof(IList<string>) || type == typeof(IEnumerable<string>))
                return "ReadStringList";
            else if (type == typeof(List<DateTime>) || type == typeof(IList<DateTime>) || type == typeof(IEnumerable<DateTime>))
                return "ReadDateTimeList";
            else if (type == typeof(List<TimeSpan>) || type == typeof(IList<TimeSpan>) || type == typeof(IEnumerable<TimeSpan>))
                return "ReadTimeSpanList";
            else if (type == typeof(List<DateTimeOffset>) || type == typeof(IList<DateTimeOffset>) || type == typeof(IEnumerable<DateTimeOffset>))
                return "ReadDateTimeOffsetList";
            else if (type == typeof(List<Enum>) || type == typeof(IList<Enum>) || type == typeof(IEnumerable<Enum>))
                return "ReadEnumList";
            else if (type == typeof(List<Guid>) || type == typeof(IList<Guid>) || type == typeof(IEnumerable<Guid>))
                return "ReadGuidList";


            else if (type == typeof(Dictionary<int, bool>) || type == typeof(IDictionary<int, bool>))
                return "ReadIntBoolDictionary";
            else if (type == typeof(Dictionary<int, byte>) || type == typeof(IDictionary<int, byte>))
                return "ReadIntByteDictionary";
            else if (type == typeof(Dictionary<int, sbyte>) || type == typeof(IDictionary<int, sbyte>))
                return "ReadIntSbyteDictionary";
            else if (type == typeof(Dictionary<int, short>) || type == typeof(IDictionary<int, short>))
                return "ReadIntShortDictionary";
            else if (type == typeof(Dictionary<int, ushort>) || type == typeof(IDictionary<int, ushort>))
                return "ReadIntUShortDictionary";
            else if (type == typeof(Dictionary<int, char>) || type == typeof(IDictionary<int, char>))
                return "ReadIntCharDictionary";
            else if (type == typeof(Dictionary<int, int>) || type == typeof(IDictionary<int, int>))
                return "ReadIntIntDictionary";
            else if (type == typeof(Dictionary<int, uint>) || type == typeof(IDictionary<int, uint>))
                return "ReadIntUIntDictionary";
            else if (type == typeof(Dictionary<int, float>) || type == typeof(IDictionary<int, float>))
                return "ReadIntFloatDictionary";
            else if (type == typeof(Dictionary<int, long>) || type == typeof(IDictionary<int, long>))
                return "ReadIntLongDictionary";
            else if (type == typeof(Dictionary<int, ulong>) || type == typeof(IDictionary<int, ulong>))
                return "ReadIntULongDictionary";
            else if (type == typeof(Dictionary<int, double>) || type == typeof(IDictionary<int, double>))
                return "ReadIntDoubleDictionary";
            else if (type == typeof(Dictionary<int, decimal>) || type == typeof(IDictionary<int, decimal>))
                return "ReadIntDecimalDictionary";
            else if (type == typeof(Dictionary<int, string>) || type == typeof(IDictionary<int, string>))
                return "ReadIntStringDictionary";
            else if (type == typeof(Dictionary<int, DateTime>) || type == typeof(IDictionary<int, DateTime>))
                return "ReadIntDateTimeDictionary";
            else if (type == typeof(Dictionary<int, DateTimeOffset>) || type == typeof(IDictionary<int, DateTimeOffset>))
                return "ReadIntDateTimeOffsetDictionary";
            else if (type == typeof(Dictionary<int, TimeSpan>) || type == typeof(IDictionary<int, TimeSpan>))
                return "ReadIntTimeSpanDictionary";
            else if (type == typeof(Dictionary<int, Guid>) || type == typeof(IDictionary<int, Guid>))
                return "ReadIntGuidDictionary";




            else if (type == typeof(Dictionary<long, bool>) || type == typeof(IDictionary<long, bool>))
                return "ReadLongBoolDictionary";
            else if (type == typeof(Dictionary<long, byte>) || type == typeof(IDictionary<long, byte>))
                return "ReadLongByteDictionary";
            else if (type == typeof(Dictionary<long, sbyte>) || type == typeof(IDictionary<long, sbyte>))
                return "ReadLongSbyteDictionary";
            else if (type == typeof(Dictionary<long, short>) || type == typeof(IDictionary<long, short>))
                return "ReadLongShortDictionary";
            else if (type == typeof(Dictionary<long, ushort>) || type == typeof(IDictionary<long, ushort>))
                return "ReadLongUShortDictionary";
            else if (type == typeof(Dictionary<long, char>) || type == typeof(IDictionary<long, char>))
                return "ReadLongCharDictionary";
            else if (type == typeof(Dictionary<long, int>) || type == typeof(IDictionary<long, int>))
                return "ReadLongIntDictionary";
            else if (type == typeof(Dictionary<long, uint>) || type == typeof(IDictionary<long, uint>))
                return "ReadLongUIntDictionary";
            else if (type == typeof(Dictionary<long, float>) || type == typeof(IDictionary<long, float>))
                return "ReadLongFloatDictionary";
            else if (type == typeof(Dictionary<long, long>) || type == typeof(IDictionary<long, long>))
                return "ReadLongLongDictionary";
            else if (type == typeof(Dictionary<long, ulong>) || type == typeof(IDictionary<long, ulong>))
                return "ReadLongULongDictionary";
            else if (type == typeof(Dictionary<long, double>) || type == typeof(IDictionary<long, double>))
                return "ReadLongDoubleDictionary";
            else if (type == typeof(Dictionary<long, decimal>) || type == typeof(IDictionary<long, decimal>))
                return "ReadLongDecimalDictionary";
            else if (type == typeof(Dictionary<long, string>) || type == typeof(IDictionary<long, string>))
                return "ReadLongStringDictionary";
            else if (type == typeof(Dictionary<long, DateTime>) || type == typeof(IDictionary<long, DateTime>))
                return "ReadLongDateTimeDictionary";
            else if (type == typeof(Dictionary<int, DateTimeOffset>) || type == typeof(IDictionary<long, DateTimeOffset>))
                return "ReadLongDateTimeOffsetDictionary";
            else if (type == typeof(Dictionary<long, TimeSpan>) || type == typeof(IDictionary<long, TimeSpan>))
                return "ReadLongTimeSpanDictionary";
            else if (type == typeof(Dictionary<long, Guid>) || type == typeof(IDictionary<long, Guid>))
                return "ReadLongGuidDictionary";

            else if (type is object)
                return "ReadObject";
            else
                throw new Exception("类型无法解析到指定的方法");

        }

        internal virtual MethodInfo CreateWriterMethod<T>(Type type)
        {
            string method = "Write";
            MethodInfo brWrite = null;

            //if (m == null)
            //{
            //    //Stopwatch w = Stopwatch.StartNew();
            //    m = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(int) }, null);
            //    //Console.WriteLine("CreateWriterMethod " + w.ElapsedMilliseconds);
            //}
            //return m;

            Dictionary<Type, MethodInfo> methods;
            Type baseType = typeof(T);
            if (typeMethods.TryGetValue(baseType, out methods) == false)
            {
                methods = new Dictionary<Type, MethodInfo>();
                typeMethods.Add(baseType, methods);
            }
            if (methods.TryGetValue(type, out brWrite) == true)
                return brWrite;

            //-----------------------------集合类型
            //char
            if (type == typeof(char[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(char[]) }, null);
            else if (type == typeof(List<char>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<char>) }, null);
            else if (type == typeof(IList<char>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<char>) }, null);

            //bool
            else if (type == typeof(bool[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(bool[]) }, null);
            else if (type == typeof(List<bool>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<bool>) }, null);
            else if (type == typeof(IList<bool>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<bool>) }, null);

            //byte
            else if (type == typeof(byte[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(byte[]) }, null);
            else if (type == typeof(List<byte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<byte>) }, null);
            else if (type == typeof(IList<byte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<byte>) }, null);

            //sbyte
            else if (type == typeof(sbyte[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(sbyte[]) }, null);
            else if (type == typeof(List<sbyte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<sbyte>) }, null);
            else if (type == typeof(IList<sbyte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<sbyte>) }, null);

            //short
            else if (type == typeof(short[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(short[]) }, null);
            else if (type == typeof(List<short>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<short>) }, null);
            else if (type == typeof(IList<short>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<short>) }, null);

            //ushort
            else if (type == typeof(ushort[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(ushort[]) }, null);
            else if (type == typeof(List<ushort>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<ushort>) }, null);
            else if (type == typeof(IList<ushort>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ushort>) }, null);

            //int
            else if (type == typeof(int[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(int[]) }, null);
            else if (type == typeof(List<int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<int>) }, null);
            else if (type == typeof(IList<int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<int>) }, null);

            //uint
            else if (type == typeof(uint[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(uint[]) }, null);
            else if (type == typeof(List<uint>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<uint>) }, null);
            else if (type == typeof(IList<uint>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<uint>) }, null);

            //long
            else if (type == typeof(long[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(long[]) }, null);
            else if (type == typeof(List<long>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<long>) }, null);
            else if (type == typeof(IList<long>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<long>) }, null);

            //ulng
            else if (type == typeof(ulong[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(ulong[]) }, null);
            else if (type == typeof(List<ulong>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<ulong>) }, null);
            else if (type == typeof(IList<ulong>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ulong>) }, null);

            //float
            else if (type == typeof(float[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(float[]) }, null);
            else if (type == typeof(List<float>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<float>) }, null);
            else if (type == typeof(IList<float>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<float>) }, null);

            //double
            else if (type == typeof(double[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(double[]) }, null);
            else if (type == typeof(List<double>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<double>) }, null);
            else if (type == typeof(IList<double>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<double>) }, null);

            //decimal
            else if (type == typeof(decimal[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(decimal[]) }, null);
            else if (type == typeof(List<decimal>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<decimal>) }, null);
            else if (type == typeof(IList<decimal>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<decimal>) }, null);

            //string
            else if (type == typeof(string[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(string[]) }, null);
            else if (type == typeof(List<string>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<string>) }, null);
            else if (type == typeof(IList<string>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<string>) }, null);

            //DateTime
            else if (type == typeof(DateTime[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(DateTime[]) }, null);
            else if (type == typeof(List<DateTime>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<DateTime>) }, null);
            else if (type == typeof(IList<DateTime>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTime>) }, null);

            //DateTimeOffset
            else if (type == typeof(DateTimeOffset[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(DateTimeOffset[]) }, null);
            else if (type == typeof(List<DateTimeOffset>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<DateTimeOffset>) }, null);
            else if (type == typeof(IList<DateTimeOffset>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTimeOffset>) }, null);

            //TimeSpan
            else if (type == typeof(TimeSpan[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(TimeSpan[]) }, null);
            else if (type == typeof(List<TimeSpan>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<TimeSpan>) }, null);
            else if (type == typeof(IList<TimeSpan>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<TimeSpan>) }, null);

            //Uri
            else if (type == typeof(Uri[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(Uri[]) }, null);
            else if (type == typeof(List<Uri>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<Uri>) }, null);
            else if (type == typeof(IList<Uri>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Uri>) }, null);

            //Guid
            else if (type == typeof(Guid[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(Guid[]) }, null);
            else if (type == typeof(List<Guid>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<Guid>) }, null);
            else if (type == typeof(IList<Guid>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Guid>) }, null);

            //Enum
            else if (type == typeof(Enum[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(Enum[]) }, null);
            else if (type == typeof(List<Enum>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<Enum>) }, null);
            else if (type == typeof(IList<Enum>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Enum>) }, null);

            //-----------------------------词典类型
            else if (type == typeof(Dictionary<int, bool>) || type == typeof(IDictionary<int, bool>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<int, bool>) }, null);

            else if (type == typeof(Dictionary<int, int>) || type == typeof(IDictionary<int, int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<int, int>) }, null);

            else if (type == typeof(Dictionary<string, bool>) || type == typeof(IDictionary<string, bool>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, bool>) }, null);

            else if (type == typeof(Dictionary<string, byte>) || type == typeof(IDictionary<string, byte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, byte>) }, null);

            else if (type == typeof(Dictionary<string, sbyte>) || type == typeof(IDictionary<string, sbyte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, sbyte>) }, null);

            else if (type == typeof(Dictionary<string, char>) || type == typeof(IDictionary<string, char>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, char>) }, null);

            else if (type == typeof(Dictionary<string, short>) || type == typeof(IDictionary<string, short>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, short>) }, null);

            else if (type == typeof(Dictionary<string, ushort>) || type == typeof(IDictionary<string, ushort>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, ushort>) }, null);

            else if (type == typeof(Dictionary<string, int>) || type == typeof(IDictionary<string, int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, int>) }, null);

            else if (type == typeof(Dictionary<string, uint>) || type == typeof(IDictionary<string, uint>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, uint>) }, null);

            else if (type == typeof(Dictionary<string, long>) || type == typeof(IDictionary<string, long>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, long>) }, null);

            else if (type == typeof(Dictionary<string, ulong>) || type == typeof(IDictionary<string, ulong>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, ulong>) }, null);

            else if (type == typeof(Dictionary<string, float>) || type == typeof(IDictionary<string, float>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, float>) }, null);

            else if (type == typeof(Dictionary<string, double>) || type == typeof(IDictionary<string, double>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, double>) }, null);

            else if (type == typeof(Dictionary<string, decimal>) || type == typeof(IDictionary<string, decimal>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, decimal>) }, null);

            else if (type == typeof(Dictionary<string, string>) || type == typeof(IDictionary<string, string>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, string>) }, null);

            else if (type == typeof(Dictionary<string, object>) || type == typeof(IDictionary<string, object>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, object>) }, null);

                //-----------------------------其它类型
            //对于纯Object类型，使用该方法
            else if (type == typeof(object))
                brWrite = typeof(T).GetMethod(method + "Object", flag, null, new Type[] { typeof(object) }, null);
            //else if (type == typeof(List<object>))
            //    brWrite = typeof(TStream).GetMethod(method, BF, null, new Type[] { typeof(object[]) }, null);
            else if (type.IsEnum)
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(int) }, null);
            else
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { type }, null);

            methods.Add(type, brWrite);

            return brWrite;

            #region old
            //MethodInfo brWrite = null;

            //if (type == typeof(DateTime))
            //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(Int64) });

            //if (type == typeof(IDictionary))
            //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(IDictionary) });
            //else if (type == typeof(IList))
            //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(string) });
            //else if (type == typeof(List<object>))
            //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(object[]) });
            //else if (type == typeof(IEnumerable))
            //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(IEnumerable) });
            //else
            //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { type });

            //if (type == typeof(IDictionary))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IDictionary) }, new ParameterModifier[0]);
            //else if (type == typeof(IList))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string) }, new ParameterModifier[0]);
            //else if (type == typeof(List<object>))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(object[]) }, new ParameterModifier[0]);
            //else if (type == typeof(IEnumerable))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IEnumerable) }, new ParameterModifier[0]);
            //else
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { type }, new ParameterModifier[0]);


            //MethodInfo brWrite = null;

            //if (type == typeof(IDictionary))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IDictionary) }, null);
            //else if (type == typeof(IList))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IList) }, null);
            ////else if (type == typeof(List<object>))
            ////    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(object[]) }, null);
            //else if (type == typeof(IEnumerable))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IEnumerable) }, null);
            ////else if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            ////    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Nullable<>) }, null);
            //else
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { type }, null);

            //return brWrite;
            #endregion
        }

        internal virtual MethodInfo CreateWriteSizeMethod<T>(Type type)
        {
            string method = "Write";
            MethodInfo brWrite = null;

            //-----------------------------集合类型
            //char
            if (type == typeof(char[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<char>) }, null);
            else if (type == typeof(List<char>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<char>) }, null);
            else if (type == typeof(IList<char>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<char>) }, null);

            //bool
            else if (type == typeof(bool[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<bool>) }, null);
            else if (type == typeof(List<bool>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<bool>) }, null);
            else if (type == typeof(IList<bool>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<bool>) }, null);

            //byte
            else if (type == typeof(byte[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<byte>) }, null);
            else if (type == typeof(List<byte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<byte>) }, null);
            else if (type == typeof(IList<byte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<byte>) }, null);

            //sbyte
            else if (type == typeof(sbyte[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<sbyte>) }, null);
            else if (type == typeof(List<sbyte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<sbyte>) }, null);
            else if (type == typeof(IList<sbyte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<sbyte>) }, null);

            //short
            else if (type == typeof(short[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<short>) }, null);
            else if (type == typeof(List<short>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<short>) }, null);
            else if (type == typeof(IList<short>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<short>) }, null);

            //ushort
            else if (type == typeof(ushort[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ushort>) }, null);
            else if (type == typeof(List<ushort>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ushort>) }, null);
            else if (type == typeof(IList<ushort>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ushort>) }, null);

            //int
            else if (type == typeof(int[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<int>) }, null);
            else if (type == typeof(List<int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<int>) }, null);
            else if (type == typeof(IList<int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<int>) }, null);

            //uint
            else if (type == typeof(uint[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<uint>) }, null);
            else if (type == typeof(List<uint>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<uint>) }, null);
            else if (type == typeof(IList<uint>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<uint>) }, null);

            //long
            else if (type == typeof(long[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<long>) }, null);
            else if (type == typeof(List<long>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<long>) }, null);
            else if (type == typeof(IList<long>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<long>) }, null);

            //ulng
            else if (type == typeof(ulong[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ulong>) }, null);
            else if (type == typeof(List<ulong>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ulong>) }, null);
            else if (type == typeof(IList<ulong>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ulong>) }, null);

            //float
            else if (type == typeof(float[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<float>) }, null);
            else if (type == typeof(List<float>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<float>) }, null);
            else if (type == typeof(IList<float>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<float>) }, null);

            //double
            else if (type == typeof(double[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<double>) }, null);
            else if (type == typeof(List<double>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<double>) }, null);
            else if (type == typeof(IList<double>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<double>) }, null);

            //decimal
            else if (type == typeof(decimal[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<decimal>) }, null);
            else if (type == typeof(List<decimal>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<decimal>) }, null);
            else if (type == typeof(IList<decimal>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<decimal>) }, null);

            //string
            else if (type == typeof(string[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<string>) }, null);
            else if (type == typeof(List<string>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<string>) }, null);
            else if (type == typeof(IList<string>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<string>) }, null);

            //DateTime
            else if (type == typeof(DateTime[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTime>) }, null);
            else if (type == typeof(List<DateTime>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTime>) }, null);
            else if (type == typeof(IList<DateTime>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTime>) }, null);

            //DateTimeOffset
            else if (type == typeof(DateTimeOffset[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTimeOffset>) }, null);
            else if (type == typeof(List<DateTimeOffset>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTimeOffset>) }, null);
            else if (type == typeof(IList<DateTimeOffset>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTimeOffset>) }, null);

            //TimeSpan
            else if (type == typeof(TimeSpan[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<TimeSpan>) }, null);
            else if (type == typeof(List<TimeSpan>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<TimeSpan>) }, null);
            else if (type == typeof(IList<TimeSpan>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<TimeSpan>) }, null);

            //Uri
            else if (type == typeof(Uri[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Uri>) }, null);
            else if (type == typeof(List<Uri>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Uri>) }, null);
            else if (type == typeof(IList<Uri>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Uri>) }, null);

            //Guid
            else if (type == typeof(Guid[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Guid>) }, null);
            else if (type == typeof(List<Guid>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Guid>) }, null);
            else if (type == typeof(IList<Guid>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Guid>) }, null);

            //Enum
            else if (type == typeof(Enum[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Enum>) }, null);
            else if (type == typeof(List<Enum>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Enum>) }, null);
            else if (type == typeof(IList<Enum>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Enum>) }, null);


            //-----------------------------词典类型
            else if (type == typeof(Dictionary<int, int>) || type == typeof(IDictionary<int, int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<int, int>) }, null);

            else if (type == typeof(Dictionary<string, bool>) || type == typeof(IDictionary<string, bool>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, bool>) }, null);

            else if (type == typeof(Dictionary<string, byte>) || type == typeof(IDictionary<string, byte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, byte>) }, null);

            else if (type == typeof(Dictionary<string, sbyte>) || type == typeof(IDictionary<string, sbyte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, sbyte>) }, null);

            else if (type == typeof(Dictionary<string, char>) || type == typeof(IDictionary<string, char>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, char>) }, null);

            else if (type == typeof(Dictionary<string, short>) || type == typeof(IDictionary<string, short>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, short>) }, null);

            else if (type == typeof(Dictionary<string, ushort>) || type == typeof(IDictionary<string, ushort>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, ushort>) }, null);

            else if (type == typeof(Dictionary<string, int>) || type == typeof(IDictionary<string, int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, int>) }, null);

            else if (type == typeof(Dictionary<string, uint>) || type == typeof(IDictionary<string, uint>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, uint>) }, null);

            else if (type == typeof(Dictionary<string, long>) || type == typeof(IDictionary<string, long>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, long>) }, null);

            else if (type == typeof(Dictionary<string, ulong>) || type == typeof(IDictionary<string, ulong>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, ulong>) }, null);

            else if (type == typeof(Dictionary<string, float>) || type == typeof(IDictionary<string, float>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, float>) }, null);

            else if (type == typeof(Dictionary<string, double>) || type == typeof(IDictionary<string, double>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, double>) }, null);

            else if (type == typeof(Dictionary<string, decimal>) || type == typeof(IDictionary<string, decimal>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, decimal>) }, null);

            else if (type == typeof(Dictionary<string, string>) || type == typeof(IDictionary<string, string>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, string>) }, null);

            else if (type == typeof(Dictionary<string, object>) || type == typeof(IDictionary<string, object>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, object>) }, null);

                //-----------------------------其它类型
            //对于纯Object类型，使用该方法
            else if (type == typeof(object))
                brWrite = typeof(T).GetMethod(method + "Object", flag, null, new Type[] { typeof(object) }, null);
            //else if (type == typeof(List<object>))
            //    brWrite = typeof(TStream).GetMethod(method, BF, null, new Type[] { typeof(object[]) }, null);
            else if (type.IsEnum)
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(int) }, null);
            else
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { type }, null);

            return brWrite;
        }

        //internal virtual MethodInfo CreateWriterMethod<T>(Type type, string method)
        //{
        //    MethodInfo brWrite = null;

        //    if (type == typeof(IList<char>) || type == typeof(List<char>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<char>) }, null);

        //    else if (type == typeof(IList<bool>) || type == typeof(List<bool>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<bool>) }, null);

        //    if (type == typeof(IList<byte>) || type == typeof(List<byte>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<byte>) }, null);

        //    else if (type == typeof(IList<sbyte>) || type == typeof(List<sbyte>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<sbyte>) }, null);

        //    if (type == typeof(IList<short>) || type == typeof(List<short>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<short>) }, null);

        //    else if (type == typeof(IList<ushort>) || type == typeof(List<ushort>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ushort>) }, null);

        //    else if (type == typeof(IList<int>) || type == typeof(List<int>))
        //        //brWrite = typeof(TStream).GetMethod(method, BF, null, new Type[] { typeof(IList<int>) }, null);
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<int>) }, null);

        //    else if (type == typeof(IList<uint>) || type == typeof(List<uint>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<uint>) }, null);

        //    else if (type == typeof(IList<long>) || type == typeof(List<long>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<long>) }, null);

        //    else if (type == typeof(IList<ulong>) || type == typeof(List<ulong>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ulong>) }, null);

        //    else if (type == typeof(IList<float>) || type == typeof(List<float>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<float>) }, null);

        //    else if (type == typeof(IList<double>) || type == typeof(List<double>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<double>) }, null);

        //    else if (type == typeof(IList<decimal>) || type == typeof(List<decimal>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<decimal>) }, null);

        //    else if (type == typeof(IList<string>) || type == typeof(List<string>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<string>) }, null);

        //    else if (type == typeof(IList<DateTime>) || type == typeof(List<DateTime>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTime>) }, null);

        //    else if (type == typeof(IList<DateTimeOffset>) || type == typeof(List<DateTimeOffset>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTimeOffset>) }, null);

        //    else if (type == typeof(IList<TimeSpan>) || type == typeof(List<TimeSpan>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<TimeSpan>) }, null);

        //    else if (type == typeof(IList<Uri>) || type == typeof(List<Uri>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Uri>) }, null);

        //    else if (type == typeof(IList<Guid>) || type == typeof(List<Guid>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Guid>) }, null);

        //    else if (type == typeof(IList<Enum>) || type == typeof(List<Enum>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Enum>) }, null);



        //    else if (type == typeof(Dictionary<int, int>) || type == typeof(IDictionary<int, int>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<int, int>) }, null);



        //    else if (type == typeof(Dictionary<string, bool>) || type == typeof(IDictionary<string, bool>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, bool>) }, null);

        //    else if (type == typeof(Dictionary<string, byte>) || type == typeof(IDictionary<string, byte>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, byte>) }, null);

        //    else if (type == typeof(Dictionary<string, sbyte>) || type == typeof(IDictionary<string, sbyte>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, sbyte>) }, null);

        //    else if (type == typeof(Dictionary<string, char>) || type == typeof(IDictionary<string, char>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, char>) }, null);

        //    else if (type == typeof(Dictionary<string, short>) || type == typeof(IDictionary<string, short>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, short>) }, null);

        //    else if (type == typeof(Dictionary<string, ushort>) || type == typeof(IDictionary<string, ushort>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, ushort>) }, null);

        //    else if (type == typeof(Dictionary<string, int>) || type == typeof(IDictionary<string, int>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, int>) }, null);

        //    else if (type == typeof(Dictionary<string, uint>) || type == typeof(IDictionary<string, uint>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, uint>) }, null);

        //    else if (type == typeof(Dictionary<string, long>) || type == typeof(IDictionary<string, long>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, long>) }, null);

        //    else if (type == typeof(Dictionary<string, ulong>) || type == typeof(IDictionary<string, ulong>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, ulong>) }, null);

        //    else if (type == typeof(Dictionary<string, float>) || type == typeof(IDictionary<string, float>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, float>) }, null);

        //    else if (type == typeof(Dictionary<string, double>) || type == typeof(IDictionary<string, double>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, double>) }, null);

        //    else if (type == typeof(Dictionary<string, decimal>) || type == typeof(IDictionary<string, decimal>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, decimal>) }, null);

        //    else if (type == typeof(Dictionary<string, string>) || type == typeof(IDictionary<string, string>))
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, string>) }, null);

        //        //对于纯Object类型，使用该方法
        //    else if (type == typeof(object))
        //        brWrite = typeof(T).GetMethod(method + "Object", flag, null, new Type[] { typeof(object) }, null);
        //    //else if (type == typeof(List<object>))
        //    //    brWrite = typeof(TStream).GetMethod(method, BF, null, new Type[] { typeof(object[]) }, null);
        //    else if (type.IsEnum)
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(int) }, null);
        //    else
        //        brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { type }, null);

        //    return brWrite;
        //}

        #endregion

        #region 虚方法

        internal virtual Serialize<T> GenerateSerializationType<T>(Type type)
        {
            DynamicMethod dynamicGet = new DynamicMethod("Serialization_" + type.Name, typeof(void), new Type[] { typeof(T), typeof(object) }, typeof(object), true);
            ILGenerator mthdIL = dynamicGet.GetILGenerator();

            WriteFixPointer<T>(mthdIL);

            if (IsBaseType(type) == true)
            {
                SerializeBaseType<T>(type, mthdIL);
                CutTail<T>(mthdIL);
            }
            else if (type.IsClass)
            {
                LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
                mthdIL.Emit(OpCodes.Nop);
                mthdIL.Emit(OpCodes.Ldarg_1);//PU
                mthdIL.Emit(OpCodes.Castclass, type);//PU
                mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

                SerializeFields<T>(type, mthdIL, tpmEvent);
                SerializePropertys<T>(type, mthdIL, tpmEvent);
            }
            //目前还无法处理值类型
            else if (type.BaseType == typeof(ValueType))
            {
                LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
                mthdIL.Emit(OpCodes.Nop);
                mthdIL.Emit(OpCodes.Ldarg_1);//PU
                mthdIL.Emit(OpCodes.Castclass, type);//PU
                mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

                SerializeValueFields<T>(type, mthdIL, tpmEvent);
                SerializeValuePropertys<T>(type, mthdIL, tpmEvent);
            }
            else
            {
                SerializeBaseType<T>(type, mthdIL);
                CutTail<T>(mthdIL);
            }
            mthdIL.Emit(OpCodes.Ret);
            return (Serialize<T>)dynamicGet.CreateDelegate(typeof(Serialize<T>));

            #region old

            //if (type.IsClass)
            //{
            //    #region old
            //    //LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);

            //    //mthdIL.Emit(OpCodes.Nop);
            //    //mthdIL.Emit(OpCodes.Ldarg_2);//PU
            //    //mthdIL.Emit(OpCodes.Castclass, type);//PU
            //    //mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

            //    ////SerializeJsonFields<TStream>(type, mthdIL, tpmEvent);
            //    ////SerializeJsonPropertys<TStream>(type, mthdIL, tpmEvent);

            //    //foreach (FieldInfo fi in type.GetFields())
            //    //{
            //    //    MethodInfo brWrite = GetJsonWriterMethod<JsonStreamTest>(fi.FieldType);
            //    //    mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
            //    //    mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer

            //    //    mthdIL.Emit(OpCodes.Ldfld, fi);
            //    //    mthdIL.EmitCall(OpCodes.Call, brWrite, null);//PU

            //    //    mthdIL.Emit(OpCodes.Nop);



            //    //    //MethodInfo brWrite = GetJsonWriterMethod<JsonStreamTest>(fi.FieldType);
            //    //    //var lv = mthdIL.DeclareLocal(type);
            //    //    //mthdIL.Emit(OpCodes.Ldarg_0);
            //    //    //mthdIL.Emit(OpCodes.Unbox_Any, type);
            //    //    //mthdIL.Emit(OpCodes.Stloc_0);
            //    //    //mthdIL.Emit(OpCodes.Ldloca_S, lv);
            //    //    //mthdIL.Emit(OpCodes.Ldfld, fi);
            //    //    //if (fi.FieldType.IsValueType)
            //    //    //    mthdIL.Emit(OpCodes.Box, fi.FieldType);
            //    //    //mthdIL.Emit(OpCodes.Ldfld, fi);
            //    //    //mthdIL.EmitCall(OpCodes.Call, brWrite, null);//PU

            //    //    //MethodInfo brWrite = GetJsonWriterMethod<JsonStreamTest>(fi.FieldType);
            //    //    //var lv = mthdIL.DeclareLocal(type);
            //    //    //mthdIL.Emit(OpCodes.Ldarg_1);
            //    //    //mthdIL.Emit(OpCodes.Unbox_Any, type);
            //    //    //mthdIL.Emit(OpCodes.Stloc_1);
            //    //    //mthdIL.Emit(OpCodes.Ldloca_S, lv);
            //    //    //mthdIL.Emit(OpCodes.Ldfld, fi);
            //    //    //if (fi.FieldType.IsValueType)
            //    //    //    mthdIL.Emit(OpCodes.Box, fi.FieldType);
            //    //    //mthdIL.Emit(OpCodes.Ldfld, fi);
            //    //    //mthdIL.EmitCall(OpCodes.Call, brWrite, null);//PU

            //    //}
            //    #endregion

            //    if (type.GetInterface("IList") == typeof(IList) ||
            //        type.GetInterface("IDictionary") == typeof(IDictionary) ||
            //        type.GetInterface("ICollection") == typeof(ICollection) ||
            //        type.GetInterface("IEnumerable") == typeof(IEnumerable))
            //    {
            //        WriteUnFlag<TStream>(mthdIL);
            //        SerializeUnPackage<TStream>(type, mthdIL);
            //    }
            //    else
            //    {
            //        LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
            //        mthdIL.Emit(OpCodes.Nop);
            //        mthdIL.Emit(OpCodes.Ldarg_1);//PU
            //        mthdIL.Emit(OpCodes.Castclass, type);//PU
            //        mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

            //        SerializeFields<TStream>(type, mthdIL, tpmEvent);
            //        SerializePropertys<TStream>(type, mthdIL, tpmEvent);
            //    }
            //}
            //    //目前还无法处理值类型
            //else if (type.BaseType ==typeof(ValueType))
            //{
            //    if (type.IsPrimitive)
            //    {
            //        WriteUnFlag<TStream>(mthdIL);
            //        SerializeUnPackage<TStream>(type, mthdIL);
            //    }
            //    else
            //    {
            //        //LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
            //        //mthdIL.Emit(OpCodes.Nop);
            //        //mthdIL.Emit(OpCodes.Ldarg_1);//PU
            //        //mthdIL.Emit(OpCodes.Castclass, type);//PU
            //        //mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

            //        //SerializeFields<TStream>(type, mthdIL, tpmEvent);
            //        //SerializePropertys<TStream>(type, mthdIL, tpmEvent);


            //        LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
            //        mthdIL.Emit(OpCodes.Nop);
            //        mthdIL.Emit(OpCodes.Ldarg_1);//PU
            //        mthdIL.Emit(OpCodes.Castclass, type);//PU
            //        mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

            //        SerializeValueFields<TStream>(type, mthdIL, tpmEvent);
            //        SerializeValuePropertys<TStream>(type, mthdIL, tpmEvent);
            //    }
            //}
            //else
            //{
            //    WriteUnFlag<TStream>(mthdIL);
            //    SerializeUnPackage<TStream>(type, mthdIL);
            //}

            #endregion
        }

        internal virtual Estimate<T> GenerateSizeSerializationType<T>(Type type)
        {
            DynamicMethod dynamicGet = new DynamicMethod("SizeSerialization_" + type.Name, typeof(void), new Type[] { typeof(T), typeof(object) }, typeof(object), true);
            ILGenerator mthdIL = dynamicGet.GetILGenerator();

            if (IsBaseType(type) == true)
            {
                SerializeBaseType<T>(type, mthdIL);
                CutTail<T>(mthdIL);
            }
            else if (type.IsClass)
            {
                LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
                mthdIL.Emit(OpCodes.Nop);
                mthdIL.Emit(OpCodes.Ldarg_1);//PU
                mthdIL.Emit(OpCodes.Castclass, type);//PU
                mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

                SerializeSizeFields<T>(type, mthdIL, tpmEvent);
                SerializeSizePropertys<T>(type, mthdIL, tpmEvent);
            }
            //目前还无法处理值类型
            else if (type.BaseType == typeof(ValueType))
            {
                LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
                mthdIL.Emit(OpCodes.Nop);
                mthdIL.Emit(OpCodes.Ldarg_1);//PU
                mthdIL.Emit(OpCodes.Castclass, type);//PU
                mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

                SerializeValueFields<T>(type, mthdIL, tpmEvent);
                SerializeValuePropertys<T>(type, mthdIL, tpmEvent);
            }
            else
            {
                SerializeBaseType<T>(type, mthdIL);
                CutTail<T>(mthdIL);
            }
            mthdIL.Emit(OpCodes.Ret);

            return (Estimate<T>)dynamicGet.CreateDelegate(typeof(Estimate<T>));
        }

        internal virtual Deserialize<T> GenerateDeserializationType<T>(Type type)
        {
            DynamicMethod dynamicGet = new DynamicMethod("Deserialization_" + type.Name, type, new Type[] { typeof(T) }, typeof(object), true);
            ILGenerator deserializeIL = dynamicGet.GetILGenerator();

            if (IsBaseType(type) == true)
            {
                //LocalBuilder tpmRetEvent = deserializeIL.DeclareLocal(type);
                //LocalBuilder tpmRetEvent2 = deserializeIL.DeclareLocal(type);
                Label ret = deserializeIL.DefineLabel();
                DeserializeBaseType<T>(type, deserializeIL);
                deserializeIL.Emit(OpCodes.Br_S, ret);
                deserializeIL.MarkLabel(ret);
                //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent2);
                deserializeIL.Emit(OpCodes.Ret);
                return (Deserialize<T>)dynamicGet.CreateDelegate(typeof(Deserialize<T>));
            }
            else if (type.IsClass)
            {
                if (type.GetInterface("IList") == typeof(IList) ||
                    type.GetInterface("IDictionary") == typeof(IDictionary) ||
                    type.GetInterface("ICollection") == typeof(ICollection) ||
                    type.GetInterface("IEnumerable") == typeof(IEnumerable))
                {
                    //WriteUnFlag<TStream>(deserializeIL);
                    //SerializeJsonBaseType<TStream>(type, deserializeIL);
                }
                else
                {
                    ConstructorInfo ctorEvent = type.GetConstructor(new Type[0]);
                    //没有构造函数也能序列化，但是不能反序列化
                    if (ctorEvent == null)
                    {
                        //throw new Exception("The event class is missing a default constructor with 0 params");
                        return null;
                    }

                    LocalBuilder tpmRetEvent = deserializeIL.DeclareLocal(type);
                    LocalBuilder tpmRetEvent2 = deserializeIL.DeclareLocal(type);
                    Label ret = deserializeIL.DefineLabel();

                    deserializeIL.Emit(OpCodes.Newobj, ctorEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent);

                    DeserializeFields<T>(type, deserializeIL, tpmRetEvent, tpmRetEvent2);
                    DeserializePropertys<T>(type, deserializeIL, tpmRetEvent, tpmRetEvent2);

                    deserializeIL.Emit(OpCodes.Br_S, ret);
                    deserializeIL.MarkLabel(ret);
                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent2);
                    deserializeIL.Emit(OpCodes.Ret);
                }

                return (Deserialize<T>)dynamicGet.CreateDelegate(typeof(Deserialize<T>));
            }
            //目前还无法处理值类型
            else if (type.BaseType == typeof(ValueType))
            {
                //if (type.IsPrimitive)
                //{
                //    WriteUnFlag<TStream>(deserializeIL);
                //    SerializeUnPackage<TStream>(type, deserializeIL);
                //}
                //else
                //{
                //    //LocalBuilder tpmEvent = deserializeIL.DeclareLocal(type);
                //    //deserializeIL.Emit(OpCodes.Nop);
                //    //deserializeIL.Emit(OpCodes.Ldarg_1);//PU
                //    //deserializeIL.Emit(OpCodes.Castclass, type);//PU
                //    //deserializeIL.Emit(OpCodes.Stloc, tpmEvent);//PP

                //    //SerializeFields<TStream>(type, deserializeIL, tpmEvent);
                //    //SerializePropertys<TStream>(type, deserializeIL, tpmEvent);
                //}


                //deserializeIL.Emit(OpCodes.Ldobj);
                //deserializeIL.Emit(OpCodes.Stloc_0);
                //deserializeIL.Emit(OpCodes.Ldloc_0);
                //deserializeIL.Emit(OpCodes.Ret);
            }
            else
            {
                //WriteUnFlag<TStream>(deserializeIL);
                //SerializeUnPackage<TStream>(type, deserializeIL);
            }

            return null;
            //return (Deserialize<TStream>)dynamicGet.CreateDelegate(typeof(Deserialize<TStream>));

        }

        internal virtual SerializeWrite<T> GenerateSerializationWriteType<T>(Type[] types)
        {
            DynamicMethod dynamicGet = new DynamicMethod("SerializationWrite", typeof(void), new Type[] { typeof(T) }, typeof(object), true);
            ILGenerator mthdIL = dynamicGet.GetILGenerator();

            //LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
            mthdIL.Emit(OpCodes.Nop);
            //mthdIL.Emit(OpCodes.Ldarg_1);//PU
            //mthdIL.Emit(OpCodes.Castclass, type);//PU
            //mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

            mthdIL.Emit(OpCodes.Stloc_S,typeof(T));//PU
            //mthdIL.Emit(OpCodes.Ldloc_S, typeof(T));//PU
            for (int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
                MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(t), flag, null, new Type[] { }, null);

                //mthdIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                //mthdIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES
                mthdIL.Emit(OpCodes.Ldloc_S, typeof(T));//PU
                mthdIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
                mthdIL.Emit(OpCodes.Nop);
            }
            mthdIL.Emit(OpCodes.Ret);
            return (SerializeWrite<T>)dynamicGet.CreateDelegate(typeof(SerializeWrite<T>));
        }

        #endregion

        #region 序列化 

        internal virtual void SerializeValueFields<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            foreach (FieldInfo info in type.GetFields())
            {
                if (Utils.IsIgnoreAttribute(info))
                    continue;

                Type ctype = info.FieldType;
                MethodInfo brWrite = CreateWriterMethod<T>(ctype);
                if (brWrite == null)
                    brWrite = CreateWriteSizeMethod<T>(ctype);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer
                LocalBuilder dictionaryEntry = mthdIL.DeclareLocal(type);
                mthdIL.Emit(OpCodes.Unbox_Any, type);
                mthdIL.Emit(OpCodes.Stloc, dictionaryEntry);// save the DictionaryEntry

                mthdIL.Emit(OpCodes.Nop);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
                mthdIL.Emit(OpCodes.Ldfld, info);
                mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write
            }
        }

        internal virtual void SerializeValuePropertys<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            foreach (PropertyInfo info in type.GetProperties())
            {
                if (Utils.IsIgnoreAttribute(info))
                    continue;

                Type ctype = info.PropertyType;
                MethodInfo brWrite = CreateWriterMethod<T>(ctype);
                if (brWrite == null)
                    brWrite = CreateWriteSizeMethod<T>(ctype);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer
                MethodInfo get_Key = type.GetMethod("get_" + info.Name, new Type[0]);
                LocalBuilder dictionaryEntry = mthdIL.DeclareLocal(type);

                mthdIL.Emit(OpCodes.Unbox_Any, type);
                mthdIL.Emit(OpCodes.Stloc, dictionaryEntry);// save the DictionaryEntry
                mthdIL.Emit(OpCodes.Nop);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
                mthdIL.EmitCall(OpCodes.Call, get_Key, null);// call get_Key
                mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write
            }
        }


        internal virtual void SerializeFields<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            bool isize = typeof(T).GetInterface("ISize") == typeof(ISize) ? true : false;
            //字段
            foreach (FieldInfo info in type.GetFields())
            {
                if (Utils.IsIgnoreAttribute(info))
                    continue;

                Type ctype = info.FieldType;
                if (isize && IsBaseType(ctype) == true)
                    continue;
                MethodInfo brWrite = CreateWriterMethod<T>(ctype);
                if (brWrite == null)
                    brWrite = CreateWriteSizeMethod<T>(ctype);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer
                mthdIL.Emit(OpCodes.Ldfld, info);
                mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                mthdIL.Emit(OpCodes.Nop);
            }
        }

        internal virtual void SerializePropertys<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            bool isize = typeof(T).GetInterface("ISize") == typeof(ISize) ? true : false;
            //属性
            foreach (PropertyInfo info in type.GetProperties())
            {
                if (Utils.IsIgnoreAttribute(info))
                    continue;

                Type ctype = info.PropertyType;
                if (isize && IsBaseType(ctype) == true)
                    continue;

                MethodInfo brWrite = CreateWriterMethod<T>(ctype);
                if (brWrite == null)
                    brWrite = CreateWriteSizeMethod<T>(ctype);
                MethodInfo mi = type.GetMethod("get_" + info.Name);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
                mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU get the value of the proprty
                mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                mthdIL.Emit(OpCodes.Nop);
            }
        }


        internal virtual void SerializeSizeFields<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            //字段
            foreach (FieldInfo info in type.GetFields())
            {
                if (Utils.IsIgnoreAttribute(info))
                    continue;

                Type ctype = info.FieldType;
                if (IsFixedSizeType(ctype) == true)
                    continue;

                MethodInfo brWrite = GetWriter<T>(ctype);
                if (brWrite == null)
                    brWrite = CreateWriteSizeMethod<T>(ctype);

                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer
                mthdIL.Emit(OpCodes.Ldfld, info);
                mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                mthdIL.Emit(OpCodes.Nop);
            }
        }

        internal virtual void SerializeSizePropertys<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            bool isize = typeof(T).GetInterface("ISize") == typeof(ISize) ? true : false;
            //属性
            foreach (PropertyInfo info in type.GetProperties())
            {
                if (Utils.IsIgnoreAttribute(info))
                    continue;

                Type ctype = info.PropertyType;
                if (IsFixedSizeType(ctype) == true)
                    continue;

                MethodInfo brWrite = GetWriter<T>(ctype);
                if (brWrite == null)
                    brWrite = CreateWriteSizeMethod<T>(ctype);
                MethodInfo mi = type.GetMethod("get_" + info.Name);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
                mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU get the value of the proprty
                mthdIL.Emit(OpCodes.Nop);
            }
        }

        /// <summary>
        /// 序列化没有名称的类型，没有经过包装的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="mthdIL"></param>
        internal virtual void SerializeBaseType<T>(Type type, ILGenerator mthdIL)
        {
            MethodInfo brWrite = CreateWriterMethod<T>(type);
            mthdIL.Emit(OpCodes.Ldarg_0);
            mthdIL.Emit(OpCodes.Ldarg_1);
            if (type.IsValueType)
                mthdIL.Emit(OpCodes.Unbox_Any, type);
            mthdIL.EmitCall(OpCodes.Call, brWrite, null);
            mthdIL.Emit(OpCodes.Ret);
        }

        internal virtual void SerializeObjects<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            bool isize = typeof(T).GetInterface("ISize") == typeof(ISize) ? true : false;
            //字段
            foreach (FieldInfo info in type.GetFields())
            {
                if (Utils.IsIgnoreAttribute(info))
                    continue;

                Type ctype = info.FieldType;
                if (isize && IsBaseType(ctype) == true)
                    continue;
                MethodInfo brWrite = CreateWriterMethod<T>(ctype);
                if (brWrite == null)
                    brWrite = CreateWriteSizeMethod<T>(ctype);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer
                mthdIL.Emit(OpCodes.Ldfld, info);
                mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                mthdIL.Emit(OpCodes.Nop);
            }


            //ILGenerator deserializeIL = null;
            //foreach (FieldInfo fi in type.GetFields())
            //{
            //    if (fi.FieldType == typeof(string[]))
            //        continue;
            //    if (Utils.IsIgnoreAttribute(fi.GetCustomAttributes(true)))
            //        continue;

            //    LocalBuilder locTyp = deserializeIL.DeclareLocal(fi.FieldType);
            //    //MethodInfo brRead = typeof(TStream).GetMethod(GetJsonReaderMethod(fi.FieldType));
            //    MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(fi.FieldType), flag, null, new Type[] { }, null);

            //    //FieldInfo fld = fldMap[fi.Name];
            //    FieldInfo fld = fi;


            //    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
            //    deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES

            //    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
            //    deserializeIL.Emit(OpCodes.Stfld, fld);//PU
            //    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
            //    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);

            //}
        }

        #endregion

        #region 反序列化

        internal virtual void DeserializeFields<T>(Type type, ILGenerator deserializeIL, LocalBuilder tpmRetEvent, LocalBuilder tpmRetEvent2)
        {
            foreach (FieldInfo fi in type.GetFields())
            {
                if (fi.FieldType == typeof(string[]))
                    continue;
                if (Utils.IsIgnoreAttribute(fi.GetCustomAttributes(true)))
                    continue;

                //LocalBuilder locTyp = deserializeIL.DeclareLocal(fi.FieldType);
                //MethodInfo brRead = typeof(TStream).GetMethod(GetJsonReaderMethod(fi.FieldType));
                MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(fi.FieldType), flag, null, new Type[] { }, null);

                //FieldInfo fld = fldMap[fi.Name];
                FieldInfo fld = fi;


                #region OPCodes for DateTime DeSerialization

                //if (fi.FieldType == typeof(DateTime))
                //{
                //    ConstructorInfo ctorDtTime = typeof(DateTime).GetConstructor(new Type[] { typeof(Int64) });

                //    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                //    //deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                //    deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES

                //    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
                //    deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
                //    deserializeIL.Emit(OpCodes.Stfld, fld);//PU
                //    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                //    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);

                //}

                #endregion

                #region OPCodes for IDictionary DeSerialization

                if (fi.FieldType == typeof(IDictionary))
                {
                    Label loopLabelBegin = deserializeIL.DefineLabel();
                    Label loopLabelEnd = deserializeIL.DefineLabel();

                    LocalBuilder count = deserializeIL.DeclareLocal(typeof(Int32));
                    LocalBuilder key = deserializeIL.DeclareLocal(typeof(string));
                    LocalBuilder value = deserializeIL.DeclareLocal(typeof(string));
                    LocalBuilder boolVal = deserializeIL.DeclareLocal(typeof(bool));

                    MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
                    MethodInfo dicAdd = typeof(IDictionary).GetMethod("Add", new Type[] { typeof(object), typeof(object) });
                    MethodInfo brReadInt = typeof(T).GetMethod(CreateReaderMethod(typeof(int)));

                    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                    deserializeIL.EmitCall(OpCodes.Callvirt, brReadInt, null);//PU
                    deserializeIL.Emit(OpCodes.Stloc, count);

                    deserializeIL.Emit(OpCodes.Br, loopLabelEnd);

                    deserializeIL.MarkLabel(loopLabelBegin); //begin loop 
                    deserializeIL.Emit(OpCodes.Nop);
                    deserializeIL.Emit(OpCodes.Ldarg_1);
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
                    deserializeIL.Emit(OpCodes.Stloc, key);
                    deserializeIL.Emit(OpCodes.Ldarg_1);
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
                    deserializeIL.Emit(OpCodes.Stloc, value);

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    //deserializeIL.EmitCall(OpCodes.Callvirt, getProp, null);
                    deserializeIL.Emit(OpCodes.Ldfld, fld);
                    deserializeIL.Emit(OpCodes.Ldloc, key);
                    deserializeIL.Emit(OpCodes.Ldloc, value);
                    deserializeIL.EmitCall(OpCodes.Callvirt, dicAdd, null);//call add method
                    deserializeIL.Emit(OpCodes.Nop);

                    deserializeIL.Emit(OpCodes.Ldloc, count);
                    deserializeIL.Emit(OpCodes.Ldc_I4_1);
                    deserializeIL.Emit(OpCodes.Sub);
                    deserializeIL.Emit(OpCodes.Stloc, count);

                    deserializeIL.MarkLabel(loopLabelEnd); //end loop 
                    deserializeIL.Emit(OpCodes.Nop);
                    deserializeIL.Emit(OpCodes.Ldloc, count);
                    deserializeIL.Emit(OpCodes.Ldc_I4_0);
                    deserializeIL.Emit(OpCodes.Ceq);
                    deserializeIL.Emit(OpCodes.Ldc_I4_0);
                    deserializeIL.Emit(OpCodes.Ceq);
                    deserializeIL.Emit(OpCodes.Stloc_S, boolVal);
                    deserializeIL.Emit(OpCodes.Ldloc_S, boolVal);
                    deserializeIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
                }
                #endregion

                #region OPCodes for other types DeSerialization

                else
                {
                    //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    //deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES

                    //deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
                    //deserializeIL.Emit(OpCodes.Stfld, fld);//PU
                    //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    //deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);


                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES

                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
                    deserializeIL.Emit(OpCodes.Stfld, fld);//PU
                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
                }

                #endregion

            }
        }

        internal virtual void DeserializePropertys<T>(Type type, ILGenerator deserializeIL, LocalBuilder tpmRetEvent, LocalBuilder tpmRetEvent2)
        {
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.PropertyType == typeof(string[]))
                    continue;
                if (Utils.IsIgnoreAttribute(pi.GetCustomAttributes(true)))
                    continue;

                MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(pi.PropertyType), flag, null, new Type[] { }, null);
                MethodInfo setProp = type.GetMethod("set_" + pi.Name);
                MethodInfo getProp = type.GetMethod("get_" + pi.Name);

                //#region OPCodes for DateTime DeSerialization 使用这种方式分序列化会报错
                //if (pi.PropertyType == typeof(DateTime))
                //{
                //    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                //    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                //    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

                //    //似乎没用
                //    //FieldBuilder dateTimeFld = typeBuilder.DefineField("Ticks ", typeof(Int64), FieldAttributes.Public);
                //    ConstructorInfo ctorDtTime = typeof(DateTime).GetConstructor(new Type[] { typeof(Int64) });
                //    deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
                //    deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU

                //    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                //    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
                //}
                //#endregion

                #region OPCodes for IDictionary DeSerialization

                //else if (pi.PropertyType == typeof(IDictionary))
                if (pi.PropertyType == typeof(IDictionary))
                {
                    Label loopLabelBegin = deserializeIL.DefineLabel();
                    Label loopLabelEnd = deserializeIL.DefineLabel();

                    LocalBuilder count = deserializeIL.DeclareLocal(typeof(Int32));
                    LocalBuilder key = deserializeIL.DeclareLocal(typeof(string));
                    LocalBuilder value = deserializeIL.DeclareLocal(typeof(string));
                    LocalBuilder boolVal = deserializeIL.DeclareLocal(typeof(bool));

                    MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
                    MethodInfo dicAdd = typeof(IDictionary).GetMethod("Add", new Type[] { typeof(object), typeof(object) });
                    MethodInfo brReadInt = typeof(T).GetMethod(CreateReaderMethod(typeof(int)));

                    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                    deserializeIL.EmitCall(OpCodes.Callvirt, brReadInt, null);//PU
                    deserializeIL.Emit(OpCodes.Stloc, count);

                    deserializeIL.Emit(OpCodes.Br, loopLabelEnd);

                    deserializeIL.MarkLabel(loopLabelBegin); //begin loop 
                    deserializeIL.Emit(OpCodes.Nop);
                    deserializeIL.Emit(OpCodes.Ldarg_1);
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
                    deserializeIL.Emit(OpCodes.Stloc, key);
                    deserializeIL.Emit(OpCodes.Ldarg_1);
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
                    deserializeIL.Emit(OpCodes.Stloc, value);

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.EmitCall(OpCodes.Callvirt, getProp, null);
                    deserializeIL.Emit(OpCodes.Ldloc, key);
                    deserializeIL.Emit(OpCodes.Ldloc, value);
                    deserializeIL.EmitCall(OpCodes.Callvirt, dicAdd, null);//call add method
                    deserializeIL.Emit(OpCodes.Nop);

                    deserializeIL.Emit(OpCodes.Ldloc, count);
                    deserializeIL.Emit(OpCodes.Ldc_I4_1);
                    deserializeIL.Emit(OpCodes.Sub);
                    deserializeIL.Emit(OpCodes.Stloc, count);

                    deserializeIL.MarkLabel(loopLabelEnd); //end loop 
                    deserializeIL.Emit(OpCodes.Nop);
                    deserializeIL.Emit(OpCodes.Ldloc, count);
                    deserializeIL.Emit(OpCodes.Ldc_I4_0);
                    deserializeIL.Emit(OpCodes.Ceq);
                    deserializeIL.Emit(OpCodes.Ldc_I4_0);
                    deserializeIL.Emit(OpCodes.Ceq);
                    deserializeIL.Emit(OpCodes.Stloc_S, boolVal);
                    deserializeIL.Emit(OpCodes.Ldloc_S, boolVal);
                    deserializeIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);


                }
                #endregion

                #region List

                //else if (pi.PropertyType == typeof(List<int>))
                //{
                //    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                //    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                //    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

                //    //FieldBuilder dateTimeFld = typeBuilder.DefineField("Ticks ", typeof(Int64), FieldAttributes.Public);
                //    ConstructorInfo ctorDtTime = typeof(List<int>).GetConstructor(new Type[] { typeof(int[]) });
                //    deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
                //    deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU

                //    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                //    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
                //}
                //else if (pi.PropertyType == typeof(IList<int>))
                //{
                //    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                //    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                //    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

                //    //FieldBuilder dateTimeFld = typeBuilder.DefineField("Ticks ", typeof(Int64), FieldAttributes.Public);
                //    ConstructorInfo ctorDtTime = typeof(List<int>).GetConstructor(new Type[] { typeof(int[]) });
                //    deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
                //    deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU

                //    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                //    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
                //}

                #endregion

                #region OPCodes for other types DeSerialization
                else
                {
                    //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    //deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                    //deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

                    //deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU


                    //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    //deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);


                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

                    deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU


                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);

                }
                #endregion
            }
        }

        protected virtual void DeserializeBaseType<T>(Type type, ILGenerator deserializeIL)
        {
            MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(type), flag, null, new Type[] { }, null);
            if (type.IsPrimitive == false)
            {
                deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES
                deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
            }
            else
                throw new Exception("目前暂时不支持基元数据类型！");


            //MethodInfo brRead = typeof(TStream).GetMethod(GetJsonReaderMethod(type), BF, null, new Type[] { }, null);
            //deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES
            //deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
            //deserializeIL.Emit(OpCodes.Ret);




            //MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(type), flag, null, new Type[] { }, null);
            //ConstructorInfo ctorDtTime = null;
            //if (type == typeof(int))
            //{
            //    //ctorDtTime = type.GetConstructor(new Type[0]);

            //    deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES
            //    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
            //    //deserializeIL.Emit(OpCodes.Initobj);//PU
            //}
            //else if (type == typeof(int[]))
            //{
            //    //ctorDtTime = type.GetConstructor(new Type[0]);

            //    deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES
            //    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
            //    //deserializeIL.Emit(OpCodes.Initobj);//PU
            //}
            //else if (type == typeof(uint[]))
            //{
            //    //ctorDtTime = type.GetConstructor(new Type[0]);

            //    deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES
            //    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
            //    deserializeIL.Emit(OpCodes.Initobj);//PU
            //}





            //ConstructorInfo ctorDtTime = typeof(DateTime).GetConstructor(new Type[] { typeof(Int64) });

            //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
            //deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES

            //deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
            //deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
            //deserializeIL.Emit(OpCodes.Stfld, fld);//PU
            //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
            //deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
        }

        #endregion

        #region 其它

        /// <summary>
        /// 获取基本型数据字段的长度
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal abstract int GetSize(Type type);

        /// <summary>
        /// 是否是指定的序列化器的基础数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal virtual bool IsBaseType(Type type)
        {
            return type.IsPrimitive;
        }

        /// <summary>
        /// 该类型是否需要特殊计算size
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal virtual bool IsFixedSizeType(Type type)
        {
            return type.IsPrimitive;
        }

        private MethodInfo GetWriter<T>(Type ctype)
        {
            MethodInfo brWrite = null;
            bool isExplicit = false;
            //不是期待的类型
            if (
                IsExplicitList(ctype) == true ||
                IsExplicitEnumerable(ctype) == true ||
                IsExplicitDictionary(ctype) == true)
            {
                isExplicit = true;
            }
            if (isExplicit == false)
            {
                if (IsBaseType(ctype) == false)
                {
                    if (ctype.GetInterface("IList") == typeof(IList) || ctype == typeof(IList))
                        brWrite = CreateWriterMethod<T>(typeof(IList));
                    else if (ctype.GetInterface("IDictionary") == typeof(IDictionary) || ctype == typeof(IDictionary))
                        brWrite = CreateWriterMethod<T>(typeof(IDictionary));
                    else if (ctype.GetInterface("IEnumerable") == typeof(IEnumerable) || ctype == typeof(IEnumerable))
                        brWrite = CreateWriterMethod<T>(typeof(IEnumerable));
                }
            }
            return brWrite;
        }

        /// <summary>
        /// 可确定的List类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool IsExplicitList(Type t)
        {
            bool isExplicit = false;
            //可能是List及相关
            if (
                    t == typeof(List<byte>) || t == typeof(IList<byte>) || t == typeof(ArraySegment<byte>) ||
                    t == typeof(List<sbyte>) || t == typeof(IList<sbyte>) || t == typeof(ArraySegment<sbyte>) ||
                    t == typeof(List<short>) || t == typeof(IList<short>) || t == typeof(ArraySegment<short>) ||
                    t == typeof(List<ushort>) || t == typeof(IList<ushort>) || t == typeof(ArraySegment<ushort>) ||
                    t == typeof(List<int>) || t == typeof(IList<int>) || t == typeof(ArraySegment<int>) ||
                    t == typeof(List<uint>) || t == typeof(IList<uint>) || t == typeof(ArraySegment<uint>) ||
                    t == typeof(List<long>) || t == typeof(IList<long>) || t == typeof(ArraySegment<long>) ||
                    t == typeof(List<ulong>) || t == typeof(IList<ulong>) || t == typeof(ArraySegment<ulong>) ||
                    t == typeof(List<float>) || t == typeof(IList<float>) || t == typeof(ArraySegment<float>) ||
                    t == typeof(List<double>) || t == typeof(IList<double>) || t == typeof(ArraySegment<double>) ||
                    t == typeof(List<decimal>) || t == typeof(IList<decimal>) || t == typeof(ArraySegment<decimal>) ||
                    t == typeof(List<bool>) || t == typeof(IList<bool>) || t == typeof(ArraySegment<bool>) ||
                    t == typeof(List<DateTime>) || t == typeof(IList<DateTime>) || t == typeof(ArraySegment<DateTime>) ||
                //t == typeof(List<Enum>) || t == typeof(IList<Enum>) || t == typeof(ArraySegment<Enum>) ||
                    t == typeof(List<Guid>) || t == typeof(IList<Guid>) || t == typeof(ArraySegment<Guid>) ||
                    t == typeof(List<TimeSpan>) || t == typeof(IList<TimeSpan>) || t == typeof(ArraySegment<TimeSpan>) ||
                    t == typeof(List<DateTimeOffset>) || t == typeof(IList<DateTimeOffset>) || t == typeof(ArraySegment<DateTimeOffset>) ||
                    t == typeof(List<Uri>) || t == typeof(IList<Uri>) || t == typeof(ArraySegment<Uri>) ||
                    t == typeof(List<string>) || t == typeof(IList<string>) || t == typeof(ArraySegment<string>) ||

                    t == typeof(string) || t == typeof(string[]) ||
                    t == typeof(byte[]) ||
                    t == typeof(sbyte[]) ||
                    t == typeof(short[]) ||
                    t == typeof(ushort[]) ||
                    t == typeof(int[]) ||
                    t == typeof(uint[]) ||
                    t == typeof(long[]) ||
                    t == typeof(ulong[]) ||
                    t == typeof(float[]) ||
                    t == typeof(double[]) ||
                    t == typeof(decimal[]) ||
                    t == typeof(bool[]) ||
                    t == typeof(DateTime[]) ||
                    t == typeof(DateTimeOffset[]) ||
                    t == typeof(Guid[]) ||
                //t == typeof(Enum[]) ||
                    t == typeof(Uri[]) ||
                    t == typeof(TimeSpan[])
               )
            {
                isExplicit = true;
            }
            return isExplicit;

        }

        /// <summary>
        /// 可确定的List类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool IsExplicitDictionary(Type t)
        {
            bool isExplicit = false;
            //可能是IDictionary及相关
            if (
                 t == typeof(IDictionary<string, bool>) || t == typeof(Dictionary<string, bool>) ||
                 t == typeof(IDictionary<string, char>) || t == typeof(Dictionary<string, char>) ||
                 t == typeof(IDictionary<string, byte>) || t == typeof(Dictionary<string, byte>) ||
                 t == typeof(IDictionary<string, sbyte>) || t == typeof(Dictionary<string, sbyte>) ||
                 t == typeof(IDictionary<string, short>) || t == typeof(Dictionary<string, short>) ||
                 t == typeof(IDictionary<string, ushort>) || t == typeof(Dictionary<string, ushort>) ||
                 t == typeof(IDictionary<string, int>) || t == typeof(Dictionary<string, int>) ||
                 t == typeof(IDictionary<string, uint>) || t == typeof(Dictionary<string, uint>) ||
                 t == typeof(IDictionary<string, long>) || t == typeof(Dictionary<string, long>) ||
                 t == typeof(IDictionary<string, ulong>) || t == typeof(Dictionary<string, ulong>) ||
                 t == typeof(IDictionary<string, float>) || t == typeof(Dictionary<string, float>) ||
                 t == typeof(IDictionary<string, double>) || t == typeof(Dictionary<string, double>) ||
                 t == typeof(IDictionary<string, decimal>) || t == typeof(Dictionary<string, decimal>) ||
                 t == typeof(IDictionary<string, string>) || t == typeof(Dictionary<string, string>)
               )
            {
                isExplicit = true;
            }
            return isExplicit;
        }

        /// <summary>
        /// 可确定的枚举类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool IsExplicitEnumerable(Type t)
        {
            bool isExplicit = false;
            if (
                    t == typeof(string) ||
                    t == typeof(string[]) ||
                    t == typeof(byte[]) ||
                    t == typeof(sbyte[]) ||
                    t == typeof(short[]) ||
                    t == typeof(ushort[]) ||
                    t == typeof(int[]) ||
                    t == typeof(uint[]) ||
                    t == typeof(long[]) ||
                    t == typeof(ulong[]) ||
                    t == typeof(float[]) ||
                    t == typeof(double[]) ||
                    t == typeof(decimal[]) ||
                    t == typeof(bool[]) ||
                    t == typeof(DateTime[]) ||
                    t == typeof(DateTimeOffset[]) ||
                    t == typeof(Guid[]) ||
                    t == typeof(Enum[]) ||
                    t == typeof(Uri[]) ||
                    t == typeof(TimeSpan[])
                 )
            {
                isExplicit = true;
            }
            return isExplicit;
        }

        private void WriteFixPointer<T>(ILGenerator mthdIL)
        {
            //该写的值会被写入，但状态不会被记录(最终版)
            MethodInfo brWrite = typeof(T).GetMethod("FixPointer", flag, null, new Type[0], null);
            if (brWrite != null)
            {
                mthdIL.Emit(OpCodes.Ldarg_0);
                mthdIL.EmitCall(OpCodes.Call, brWrite, null);
            }
        }

        private void WriteUnFlag<T>(ILGenerator mthdIL)
        {
            //该写入的值不会被写入
            //MethodInfo brWrite = typeof(TStream).GetMethod("WriteUnFlag", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            //mthdIL.Emit(OpCodes.Ldarg_0);
            ////mthdIL.Emit(OpCodes.Ldarg_1);
            //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);
            //mthdIL.Emit(OpCodes.Ret);


            ////该写的值会被写入，但状态不会被记录
            //MethodInfo brWrite = typeof(TStream).GetMethod("WriteUnFlag", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            //mthdIL.Emit(OpCodes.Ldarg_0);
            ////mthdIL.Emit(OpCodes.Ldarg_1);
            //mthdIL.EmitCall(OpCodes.Call, brWrite, null);
            ////mthdIL.Emit(OpCodes.Ret);



            //该写的值会被写入，但状态不会被记录(最终版)
            MethodInfo brWrite = typeof(T).GetMethod("WriteUnFlag", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            mthdIL.Emit(OpCodes.Ldarg_0);
            mthdIL.EmitCall(OpCodes.Call, brWrite, null);
        }

        internal virtual void CutTail<T>(ILGenerator mthdIL)
        {
            ////该写的值会被写入，但状态不会被记录(最终版)
            //MethodInfo brWrite = typeof(TStream).GetMethod("CutTail", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            //mthdIL.Emit(OpCodes.Ldarg_0);
            //mthdIL.EmitCall(OpCodes.Call, brWrite, null);
        }

        #endregion

        #region 原始的,暂时不用了

        //internal Type GenerateJsonSerializationSurrogateType<T, TStream>(Type type)
        //{
        //    #region IL Initilization Code

        //    Type retType = null;

        //    try
        //    {
        //        AppDomain domain = Thread.GetDomain();
        //        AssemblyName asmName = new AssemblyName();
        //        asmName.Name = typeof(TStream).Name + "Surrogate";
        //        AssemblyBuilder asmBuilder = domain.DefineDynamicAssembly(
        //                                                           asmName,
        //                                                           AssemblyBuilderAccess.Run);

        //        ModuleBuilder surrogateModule = asmBuilder.DefineDynamicModule(typeof(TStream).Name + "SurrogateModule");
        //        //ModuleBuilder surrogateModule = myAsmBuilder.DefineDynamicModule( "SurrogateModule", "Surrogate.dll");

        //        TypeBuilder typeBuilder = surrogateModule.DefineType(typeof(TStream).Name + "_" + type.Name + "_EventSurrogate",
        //                                                            TypeAttributes.Public);
        //        typeBuilder.AddInterfaceImplementation(typeof(T));

        //        //TypeBuilder eventTypeBuilder = surrogateModule.DefineType(EventType.Name, TypeAttributes.Public);
        //        //TypeBuilder eventTypeBuilder = BuildTypeHierarchy(surrogateModule, EventType);

        //    #endregion

        //        #region Serialize Method Builder

        //        Type[] dpParams = new Type[] { typeof(TStream), typeof(object) };
        //        MethodBuilder serializeMethod = typeBuilder.DefineMethod(
        //                                               "Serialize",
        //                                                MethodAttributes.Public | MethodAttributes.Virtual,
        //                                                typeof(void),
        //                                                dpParams);

        //        ILGenerator mthdIL = serializeMethod.GetILGenerator();


        //        Label labelFinally = mthdIL.DefineLabel();
        //        if (type.IsClass)
        //        {
        //            LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);

        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Ldarg_2);//PU
        //            mthdIL.Emit(OpCodes.Castclass, type);//PU
        //            mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

        //            SerializeJsonFields<TStream>(type, mthdIL, tpmEvent);
        //            SerializeJsonPropertys<TStream>(type, mthdIL, tpmEvent);
        //        }
        //        else
        //        {
        //            MethodInfo brWrite = CreateWriterMethod<TStream>(type);
        //            mthdIL.Emit(OpCodes.Ldarg_0);
        //            mthdIL.Emit(OpCodes.Ldarg_1);
        //            mthdIL.Emit(OpCodes.Unbox_Any, type);
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);
        //            mthdIL.Emit(OpCodes.Ret);
        //        }


        //        mthdIL.MarkLabel(labelFinally);
        //        mthdIL.Emit(OpCodes.Ret);
        //        mthdIL = null;

        //        #endregion

        //        #region Deserialize Method Builder

        //        //dpParams = new Type[] { typeof(TStream) };
        //        //MethodBuilder deserializeMthd = typeBuilder.DefineMethod(
        //        //                                       "DeSerialize",
        //        //                                        MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.Final | MethodAttributes.NewSlot,
        //        //                                        typeof(object),
        //        //                                        dpParams);

        //        //ILGenerator deserializeIL = deserializeMthd.GetILGenerator();

        //        //if (type.IsClass == true)
        //        //{
        //        //    LocalBuilder tpmRetEvent = deserializeIL.DeclareLocal(type);
        //        //    LocalBuilder tpmRetEvent2 = deserializeIL.DeclareLocal(type);
        //        //    Label ret = deserializeIL.DefineLabel();

        //        //    ConstructorInfo ctorEvent = type.GetConstructor(new Type[0]);
        //        //    if (ctorEvent == null)
        //        //        throw new Exception("The event class is missing a default constructor with 0 params");

        //        //    deserializeIL.Emit(OpCodes.Newobj, ctorEvent);
        //        //    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent);


        //        //    DeserializeJsonFields<TStream>(type, deserializeIL, tpmRetEvent, tpmRetEvent2);
        //        //    DeserializeJsonPropertys<TStream>(type, deserializeIL, tpmRetEvent, tpmRetEvent2);

        //        //    deserializeIL.Emit(OpCodes.Br_S, ret);
        //        //    deserializeIL.MarkLabel(ret);
        //        //    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent2);
        //        //    deserializeIL.Emit(OpCodes.Ret);
        //        //}
        //        //else
        //        //{
        //        //    //ConstructorInfo ctorEvent = type.GetConstructor(new Type[0]);
        //        //    //if (ctorEvent == null)
        //        //    //    throw new Exception("The event class is missing a default constructor with 0 params");

        //        //    //deserializeIL.Emit(OpCodes.Newobj, ctorEvent);
        //        //    //deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent);

        //        //    //deserializeIL.Emit(OpCodes.Br_S, ret);
        //        //    //deserializeIL.MarkLabel(ret);
        //        //    //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent2);
        //        //    //deserializeIL.Emit(OpCodes.Ret);

        //        //    deserializeIL.Emit(OpCodes.Ldnull);
        //        //    deserializeIL.Emit(OpCodes.Stloc_0);
        //        //    deserializeIL.Emit(OpCodes.Ldloc_0);
        //        //    deserializeIL.Emit(OpCodes.Ret);
        //        //}

        //        #endregion

        //        retType = typeBuilder.CreateType();
        //    }
        //    catch (Exception x)
        //    {
        //        throw x;
        //    }
        //    return retType;
        //}

        //private void SerializeJsonFields<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        //{
        //    //Dictionary<string, FieldInfo> fldMap = new Dictionary<string, FieldInfo>();
        //    MethodInfo dateTimeTicks = typeof(DateTime).GetMethod("get_Ticks");
        //    MethodInfo objectArrayList = typeof(List<object>).GetMethod("ToArray");
        //    MethodInfo brWriteObjects = CreateWriterMethod<T>(typeof(object[]));

        //    foreach (FieldInfo fi in type.GetFields())
        //    {
        //        //if (fi.FieldType == typeof(string[]))
        //        //    continue;
        //        if (Utils.IsIgnoreAttribute(fi.GetCustomAttributes(true)))
        //            continue;

        //        MethodInfo brWrite = CreateWriterMethod<T>(fi.FieldType);

        //        //FieldBuilder fld = eventTypeBuilder.DefineField(fi.Name, fi.FieldType, fi.Attributes);
        //        //TypeBuilder bb = declaringTypeMap[fi.DeclaringType.FullName];
        //        //FieldInfo fld = null;
        //        FieldInfo fld = fi;// bb.GetField(fi.Name);


        //        //fldMap[fi.Name] = fld;

        //        mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //        mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer

        //        #region OPCodes for DateTime serialization

        //        if (fi.FieldType == typeof(DateTime))
        //        {
        //            mthdIL.Emit(OpCodes.Ldflda, fld);
        //            mthdIL.EmitCall(OpCodes.Call, dateTimeTicks, null);//PU
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        }

        //        #endregion


        //        #region  OPCodes for IDictionary serialization

        //        else if (fi.FieldType == typeof(IDictionary))
        //        {

        //            Label loopLabelBegin = mthdIL.DefineLabel();
        //            Label loopLabelEnd = mthdIL.DefineLabel();
        //            Label endFinally = mthdIL.DefineLabel();

        //            LocalBuilder dictionaryEntry = mthdIL.DeclareLocal(typeof(DictionaryEntry));
        //            LocalBuilder dicEnumerator = mthdIL.DeclareLocal(typeof(IDictionaryEnumerator));
        //            LocalBuilder comparsionResult = mthdIL.DeclareLocal(typeof(bool));
        //            LocalBuilder locIDisposable = mthdIL.DeclareLocal(typeof(IDisposable));

        //            MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
        //            MethodInfo getEnumerator = typeof(IDictionary).GetMethod("GetEnumerator", new Type[0]);
        //            MethodInfo moveNext = typeof(IEnumerator).GetMethod("MoveNext", new Type[0]);
        //            MethodInfo getCurrent = typeof(IEnumerator).GetMethod("get_Current", new Type[0]);
        //            MethodInfo dispose = typeof(IDisposable).GetMethod("Dispose", new Type[0]);
        //            MethodInfo get_Key = typeof(DictionaryEntry).GetMethod("get_Key", new Type[0]);
        //            MethodInfo get_Value = typeof(DictionaryEntry).GetMethod("get_Value", new Type[0]);
        //            MethodInfo get_Count = typeof(ICollection).GetMethod("get_Count");
        //            MethodInfo brWriteInt = CreateWriterMethod<T>(typeof(int));

        //            mthdIL.Emit(OpCodes.Ldfld, fld);
        //            mthdIL.EmitCall(OpCodes.Callvirt, get_Count, null);// get the array count
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWriteInt, null);// write the count
        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Nop);


        //            mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
        //            //mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU load  the  proprety again  into ES
        //            mthdIL.Emit(OpCodes.Ldfld, fld);
        //            mthdIL.EmitCall(OpCodes.Callvirt, getEnumerator, null);// get the enumerator
        //            mthdIL.Emit(OpCodes.Stloc, dicEnumerator);//save the enumerator

        //            mthdIL.BeginExceptionBlock();

        //            mthdIL.Emit(OpCodes.Br, loopLabelEnd);// start the loop

        //            mthdIL.MarkLabel(loopLabelBegin);//begin for each loop

        //            mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //            mthdIL.EmitCall(OpCodes.Callvirt, getCurrent, null);// call get_Current
        //            mthdIL.Emit(OpCodes.Unbox_Any, typeof(DictionaryEntry));
        //            mthdIL.Emit(OpCodes.Stloc, dictionaryEntry);// save the DictionaryEntry

        //            //get key
        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //            mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
        //            mthdIL.EmitCall(OpCodes.Call, get_Key, null);// call get_Key
        //            mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

        //            //get value
        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //            mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
        //            mthdIL.EmitCall(OpCodes.Call, get_Value, null);// call get_Value
        //            mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Nop);

        //            mthdIL.MarkLabel(loopLabelEnd);//end for each loop
        //            mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //            mthdIL.EmitCall(OpCodes.Callvirt, moveNext, null);// call move next
        //            mthdIL.Emit(OpCodes.Stloc, comparsionResult);//save the result
        //            mthdIL.Emit(OpCodes.Ldloc, comparsionResult);//load the result
        //            mthdIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);//loop if true
        //            //mthdIL.Emit(OpCodes.Leave_S, labelFinally);//leave if false

        //            mthdIL.BeginFinallyBlock();

        //            mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //            mthdIL.Emit(OpCodes.Isinst, typeof(System.IDisposable));
        //            mthdIL.Emit(OpCodes.Stloc_S, locIDisposable);
        //            mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);
        //            mthdIL.Emit(OpCodes.Ldnull);
        //            mthdIL.Emit(OpCodes.Ceq);
        //            mthdIL.Emit(OpCodes.Stloc, comparsionResult);
        //            mthdIL.Emit(OpCodes.Ldloc, comparsionResult);
        //            mthdIL.Emit(OpCodes.Brtrue_S, endFinally);
        //            mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);//load IDisposable
        //            mthdIL.EmitCall(OpCodes.Callvirt, dispose, null);// call IDisposable::Dispose
        //            mthdIL.Emit(OpCodes.Nop);

        //            mthdIL.MarkLabel(endFinally);
        //            mthdIL.EndExceptionBlock();

        //        }

        //        #endregion

        //        #region List

        //        //else if (fi.FieldType == typeof(int[]))
        //        //{
        //        //    LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(int[]));
        //        //    mthdIL.Emit(OpCodes.Stloc, tmpTicks);
        //        //    mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);
        //        //    mthdIL.EmitCall(OpCodes.Call, int32ArrayList, null);//PU
        //        //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //}
        //        else if (fi.FieldType.GetInterface("IEnumerable") == typeof(IEnumerable))
        //        {
        //            bool isExplicit = false;
        //            Type t = fi.FieldType;
        //            //可能是List及相关
        //            if (t.GetGenericArguments().Length == 1)
        //            {
        //                if (t == typeof(List<byte>) || t == typeof(IList<byte>) ||
        //                    t == typeof(List<sbyte>) || t == typeof(IList<sbyte>) ||
        //                    t == typeof(List<short>) || t == typeof(IList<short>) ||
        //                    t == typeof(List<ushort>) || t == typeof(IList<ushort>) ||
        //                    t == typeof(List<int>) || t == typeof(IList<int>) ||
        //                    t == typeof(List<uint>) || t == typeof(IList<uint>) ||
        //                    t == typeof(List<long>) || t == typeof(IList<long>) ||
        //                    t == typeof(List<ulong>) || t == typeof(IList<ulong>) ||
        //                    t == typeof(List<float>) || t == typeof(IList<float>) ||
        //                    t == typeof(List<double>) || t == typeof(IList<double>) ||
        //                    t == typeof(List<decimal>) || t == typeof(IList<decimal>) ||
        //                    t == typeof(List<string>) || t == typeof(IList<string>)
        //                    )
        //                {
        //                    isExplicit = true;
        //                }
        //            }
        //            //可能是Dictionary及相关
        //            else if (t.GetGenericArguments().Length == 2)
        //            {
        //                MethodInfo brWriteIEnumable = CreateWriterMethod<T>(typeof(IDictionary));
        //                mthdIL.Emit(OpCodes.Ldfld, fld);
        //                mthdIL.EmitCall(OpCodes.Callvirt, brWriteIEnumable, null);//PU
        //                continue;
        //            }
        //            else
        //            {
        //                if (t == typeof(string) || t == typeof(string[]) ||
        //                    t == typeof(byte[]) ||
        //                    t == typeof(sbyte[]) ||
        //                    t == typeof(short[]) ||
        //                    t == typeof(ushort[]) ||
        //                    t == typeof(int[]) ||
        //                    t == typeof(uint[]) ||
        //                    t == typeof(long[]) ||
        //                    t == typeof(ulong[]) ||
        //                    t == typeof(float[]) ||
        //                    t == typeof(double[]) ||
        //                    t == typeof(decimal[]) ||
        //                    t == typeof(bool[]) ||
        //                    t == typeof(DateTime[]) ||
        //                    t == typeof(DateTimeOffset[]) ||
        //                    t == typeof(Guid[]) ||
        //                    t == typeof(Enum[]) ||
        //                    t == typeof(Uri[]) ||
        //                    t == typeof(TimeSpan[])
        //                    )
        //                    isExplicit = true;
        //            }
        //            if (isExplicit == true)
        //            {
        //                mthdIL.Emit(OpCodes.Ldfld, fld);
        //                mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //                //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //            }
        //            //属性的Object写入还存在问题
        //            else
        //            {
        //                //if (fi.FieldType != typeof(IList<object>))
        //                //{
        //                MethodInfo brWriteIEnumable = CreateWriterMethod<T>(typeof(IEnumerable));
        //                mthdIL.Emit(OpCodes.Ldfld, fld);
        //                mthdIL.EmitCall(OpCodes.Callvirt, brWriteIEnumable, null);//PU
        //                //}
        //                //else
        //                //{
        //                //    MethodInfo brWriteIEnumable = typeof(TStream).GetMethod("Write", new Type[] { typeof(IEnumerable), typeof(bool) });
        //                //    mthdIL.Emit(OpCodes.Ldfld, fld);
        //                //    mthdIL.Emit(OpCodes.Ldfld, true);
        //                //    mthdIL.EmitCall(OpCodes.Callvirt, brWriteIEnumable,null);//PU
        //                //}

        //                //LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(object[]));
        //                //mthdIL.Emit(OpCodes.Ldnull, tmpArray);
        //                //mthdIL.Emit(OpCodes.Stloc, tmpArray);
        //                ////mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //                ////mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
        //                //mthdIL.EmitCall(OpCodes.Callvirt, objectArrayList, null);//PU
        //                ////mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //                ////mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //                ////mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //                //mthdIL.EmitCall(OpCodes.Callvirt, brWriteObjects, null);//PU
        //            }
        //        }

        //        #endregion

        //        #region OPCodes for other types serialization
        //        else
        //        {
        //            mthdIL.Emit(OpCodes.Ldfld, fld);
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        }
        //        #endregion

        //        mthdIL.Emit(OpCodes.Nop);
        //    }
        //}

        //private void SerializeJsonPropertys<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        //{
        //    MethodInfo dateTimeTicks = typeof(DateTime).GetMethod("get_Ticks");
        //    MethodInfo objectArrayList = typeof(List<object>).GetMethod("ToArray");
        //    MethodInfo brWriteObjects = CreateWriterMethod<T>(typeof(object[]));
        //    foreach (PropertyInfo pi in type.GetProperties())
        //    {
        //        if (pi.PropertyType == typeof(string[]))
        //            continue;
        //        if (Utils.IsIgnoreAttribute(pi.GetCustomAttributes(true)))
        //            continue;

        //        MethodInfo mi = type.GetMethod("get_" + pi.Name);
        //        MethodInfo brWrite = CreateWriterMethod<T>(pi.PropertyType);

        //        mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //        mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
        //        mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU get the value of the proprty

        //        #region OPCodes for DateTime serialization

        //        if (pi.PropertyType == typeof(DateTime))
        //        {
        //            LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(DateTime));
        //            mthdIL.Emit(OpCodes.Stloc, tmpTicks);
        //            mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);
        //            mthdIL.EmitCall(OpCodes.Call, dateTimeTicks, null);//PU
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        }
        //        #endregion

        //        #region OPCodes for IDictionary serialization

        //        else if (pi.PropertyType == typeof(IDictionary))
        //        {

        //            Label loopLabelBegin = mthdIL.DefineLabel();
        //            Label loopLabelEnd = mthdIL.DefineLabel();
        //            Label endFinally = mthdIL.DefineLabel();

        //            LocalBuilder dictionaryEntry = mthdIL.DeclareLocal(typeof(DictionaryEntry));
        //            LocalBuilder dicEnumerator = mthdIL.DeclareLocal(typeof(IDictionaryEnumerator));
        //            LocalBuilder comparsionResult = mthdIL.DeclareLocal(typeof(bool));
        //            LocalBuilder locIDisposable = mthdIL.DeclareLocal(typeof(IDisposable));

        //            MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
        //            MethodInfo getEnumerator = typeof(IDictionary).GetMethod("GetEnumerator", new Type[0]);
        //            MethodInfo moveNext = typeof(IEnumerator).GetMethod("MoveNext", new Type[0]);
        //            MethodInfo getCurrent = typeof(IEnumerator).GetMethod("get_Current", new Type[0]);
        //            MethodInfo dispose = typeof(IDisposable).GetMethod("Dispose", new Type[0]);
        //            MethodInfo get_Key = typeof(DictionaryEntry).GetMethod("get_Key", new Type[0]);
        //            MethodInfo get_Value = typeof(DictionaryEntry).GetMethod("get_Value", new Type[0]);
        //            MethodInfo get_Count = typeof(ICollection).GetMethod("get_Count");
        //            MethodInfo brWriteInt = CreateWriterMethod<T>(typeof(int));

        //            mthdIL.EmitCall(OpCodes.Callvirt, get_Count, null);// get the array count
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWriteInt, null);// write the count
        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Nop);


        //            mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
        //            mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU load  the  proprety again  into ES
        //            mthdIL.EmitCall(OpCodes.Callvirt, getEnumerator, null);// get the enumerator
        //            mthdIL.Emit(OpCodes.Stloc, dicEnumerator);//save the enumerator

        //            mthdIL.BeginExceptionBlock();

        //            mthdIL.Emit(OpCodes.Br, loopLabelEnd);// start the loop

        //            mthdIL.MarkLabel(loopLabelBegin);//begin for each loop

        //            mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //            mthdIL.EmitCall(OpCodes.Callvirt, getCurrent, null);// call get_Current
        //            mthdIL.Emit(OpCodes.Unbox_Any, typeof(DictionaryEntry));
        //            mthdIL.Emit(OpCodes.Stloc, dictionaryEntry);// save the DictionaryEntry

        //            //get key
        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //            mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
        //            mthdIL.EmitCall(OpCodes.Call, get_Key, null);// call get_Key
        //            mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

        //            //get value
        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //            mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
        //            mthdIL.EmitCall(OpCodes.Call, get_Value, null);// call get_Value
        //            mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Nop);

        //            mthdIL.MarkLabel(loopLabelEnd);//end for each loop
        //            mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //            mthdIL.EmitCall(OpCodes.Callvirt, moveNext, null);// call move next
        //            mthdIL.Emit(OpCodes.Stloc, comparsionResult);//save the result
        //            mthdIL.Emit(OpCodes.Ldloc, comparsionResult);//load the result
        //            mthdIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);//loop if true
        //            //mthdIL.Emit(OpCodes.Leave_S, labelFinally);//leave if false

        //            mthdIL.BeginFinallyBlock();

        //            mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //            mthdIL.Emit(OpCodes.Isinst, typeof(System.IDisposable));
        //            mthdIL.Emit(OpCodes.Stloc_S, locIDisposable);
        //            mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);
        //            mthdIL.Emit(OpCodes.Ldnull);
        //            mthdIL.Emit(OpCodes.Ceq);
        //            mthdIL.Emit(OpCodes.Stloc, comparsionResult);
        //            mthdIL.Emit(OpCodes.Ldloc, comparsionResult);
        //            mthdIL.Emit(OpCodes.Brtrue_S, endFinally);
        //            mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);//load IDisposable
        //            mthdIL.EmitCall(OpCodes.Callvirt, dispose, null);// call IDisposable::Dispose
        //            mthdIL.Emit(OpCodes.Nop);

        //            mthdIL.MarkLabel(endFinally);
        //            mthdIL.EndExceptionBlock();

        //        }

        //        #endregion

        //        #region List

        //        #region old
        //        //else if (pi.PropertyType == typeof(List<int>))
        //        //{
        //        //    //LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(List<int>));
        //        //    //mthdIL.Emit(OpCodes.Stloc, tmpTicks);
        //        //    //mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);
        //        //    //mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
        //        //    //mthdIL.Emit(OpCodes.Stloc_2, tmpTicks);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_0, tmpTicks);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_2, tmpTicks);
        //        //    //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU

        //        //    //LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(List<int>));
        //        //    //mthdIL.Emit(OpCodes.Stloc, tmpTicks);
        //        //    //mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);

        //        //    //mthdIL.Emit(OpCodes.Ldloc_1, tpmEvent);
        //        //    //mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
        //        //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //        //    //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU

        //        //    LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(int[]));
        //        //    mthdIL.Emit(OpCodes.Ldnull, tmpArray);
        //        //    mthdIL.Emit(OpCodes.Stloc, tmpArray);
        //        //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
        //        //    mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
        //        //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //        //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //}
        //        //else if (pi.PropertyType == typeof(IList<int>))
        //        //{
        //        //    LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(int[]));
        //        //    mthdIL.Emit(OpCodes.Ldnull, tmpArray);
        //        //    mthdIL.Emit(OpCodes.Stloc, tmpArray);
        //        //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
        //        //    mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
        //        //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //        //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //}
        //        ////else if (pi.PropertyType.GetGenericArguments().Length > 0)
        //        #endregion

        //        else if (pi.PropertyType.GetInterface("IEnumerable") == typeof(IEnumerable))
        //        {
        //            bool isExplicit = false;
        //            Type t = pi.PropertyType;
        //            //可能是List及相关
        //            if (pi.PropertyType.GetGenericArguments().Length == 1)
        //            {
        //                if (t == typeof(List<byte>) || t == typeof(IList<byte>) ||
        //                    t == typeof(List<sbyte>) || t == typeof(IList<sbyte>) ||
        //                    t == typeof(List<short>) || t == typeof(IList<short>) ||
        //                    t == typeof(List<ushort>) || t == typeof(IList<ushort>) ||
        //                    t == typeof(List<int>) || t == typeof(IList<int>) ||
        //                    t == typeof(List<uint>) || t == typeof(IList<uint>) ||
        //                    t == typeof(List<long>) || t == typeof(IList<long>) ||
        //                    t == typeof(List<ulong>) || t == typeof(IList<ulong>) ||
        //                    t == typeof(List<float>) || t == typeof(IList<float>) ||
        //                    t == typeof(List<double>) || t == typeof(IList<double>) ||
        //                    t == typeof(List<decimal>) || t == typeof(IList<decimal>)
        //                    )
        //                {
        //                    isExplicit = true;
        //                }
        //            }
        //            //可能是Dictionary及相关
        //            else if (pi.PropertyType.GetGenericArguments().Length == 2)
        //            {

        //            }
        //            else
        //            {
        //                if (t == typeof(string) || t == typeof(string[]) ||
        //                    t == typeof(byte[]) ||
        //                    t == typeof(sbyte[]) ||
        //                    t == typeof(short[]) ||
        //                    t == typeof(ushort[]) ||
        //                    t == typeof(int[]) ||
        //                    t == typeof(uint[]) ||
        //                    t == typeof(long[]) ||
        //                    t == typeof(ulong[]) ||
        //                    t == typeof(float[]) ||
        //                    t == typeof(double[]) ||
        //                    t == typeof(decimal[]) ||
        //                    t == typeof(bool[]) ||
        //                    t == typeof(DateTime[]) ||
        //                    t == typeof(DateTimeOffset[]) ||
        //                    t == typeof(Guid[]) ||
        //                    t == typeof(Enum[]) ||
        //                    t == typeof(Uri[]) ||
        //                    t == typeof(TimeSpan[])
        //                    )
        //                    isExplicit = true;
        //            }
        //            if (isExplicit == true)
        //                mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //            //对于纯Object类型可能特殊处理比较好
        //            else
        //            {
        //                LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(object[]));
        //                mthdIL.Emit(OpCodes.Ldnull, tmpArray);
        //                mthdIL.Emit(OpCodes.Stloc, tmpArray);
        //                //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //                //mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
        //                mthdIL.EmitCall(OpCodes.Callvirt, objectArrayList, null);//PU
        //                //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //                //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //                //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //                mthdIL.EmitCall(OpCodes.Callvirt, brWriteObjects, null);//PU
        //            }
        //        }

        //        #endregion

        //        #region OPCodes for all other type serialization

        //        else
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU

        //        #endregion

        //        mthdIL.Emit(OpCodes.Nop);
        //    }
        //}

        //private void DeserializeJsonFields<T>(Type type, ILGenerator deserializeIL, LocalBuilder tpmRetEvent, LocalBuilder tpmRetEvent2)
        //{
        //    foreach (FieldInfo fi in type.GetFields())
        //    {
        //        if (fi.FieldType == typeof(string[]))
        //            continue;
        //        if (Utils.IsIgnoreAttribute(fi.GetCustomAttributes(true)))
        //            continue;

        //        LocalBuilder locTyp = deserializeIL.DeclareLocal(fi.FieldType);
        //        //MethodInfo brRead = typeof(TStream).GetMethod(GetJsonReaderMethod(fi.FieldType));
        //        MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(fi.FieldType), flag, null, new Type[] { }, null);

        //        //FieldInfo fld = fldMap[fi.Name];
        //        FieldInfo fld = fi;


        //        #region OPCodes for DateTime DeSerialization

        //        if (fi.FieldType == typeof(DateTime))
        //        {
        //            ConstructorInfo ctorDtTime = typeof(DateTime).GetConstructor(new Type[] { typeof(Int64) });

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES

        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
        //            deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
        //            deserializeIL.Emit(OpCodes.Stfld, fld);//PU
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);

        //        }

        //        #endregion

        //        #region OPCodes for IDictionary DeSerialization

        //        else if (fi.FieldType == typeof(IDictionary))
        //        {
        //            Label loopLabelBegin = deserializeIL.DefineLabel();
        //            Label loopLabelEnd = deserializeIL.DefineLabel();

        //            LocalBuilder count = deserializeIL.DeclareLocal(typeof(Int32));
        //            LocalBuilder key = deserializeIL.DeclareLocal(typeof(string));
        //            LocalBuilder value = deserializeIL.DeclareLocal(typeof(string));
        //            LocalBuilder boolVal = deserializeIL.DeclareLocal(typeof(bool));

        //            MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
        //            MethodInfo dicAdd = typeof(IDictionary).GetMethod("Add", new Type[] { typeof(object), typeof(object) });
        //            MethodInfo brReadInt = typeof(T).GetMethod(CreateReaderMethod(typeof(int)));

        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brReadInt, null);//PU
        //            deserializeIL.Emit(OpCodes.Stloc, count);

        //            deserializeIL.Emit(OpCodes.Br, loopLabelEnd);

        //            deserializeIL.MarkLabel(loopLabelBegin); //begin loop 
        //            deserializeIL.Emit(OpCodes.Nop);
        //            deserializeIL.Emit(OpCodes.Ldarg_1);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
        //            deserializeIL.Emit(OpCodes.Stloc, key);
        //            deserializeIL.Emit(OpCodes.Ldarg_1);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
        //            deserializeIL.Emit(OpCodes.Stloc, value);

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            //deserializeIL.EmitCall(OpCodes.Callvirt, getProp, null);
        //            deserializeIL.Emit(OpCodes.Ldfld, fld);
        //            deserializeIL.Emit(OpCodes.Ldloc, key);
        //            deserializeIL.Emit(OpCodes.Ldloc, value);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, dicAdd, null);//call add method
        //            deserializeIL.Emit(OpCodes.Nop);

        //            deserializeIL.Emit(OpCodes.Ldloc, count);
        //            deserializeIL.Emit(OpCodes.Ldc_I4_1);
        //            deserializeIL.Emit(OpCodes.Sub);
        //            deserializeIL.Emit(OpCodes.Stloc, count);

        //            deserializeIL.MarkLabel(loopLabelEnd); //end loop 
        //            deserializeIL.Emit(OpCodes.Nop);
        //            deserializeIL.Emit(OpCodes.Ldloc, count);
        //            deserializeIL.Emit(OpCodes.Ldc_I4_0);
        //            deserializeIL.Emit(OpCodes.Ceq);
        //            deserializeIL.Emit(OpCodes.Ldc_I4_0);
        //            deserializeIL.Emit(OpCodes.Ceq);
        //            deserializeIL.Emit(OpCodes.Stloc_S, boolVal);
        //            deserializeIL.Emit(OpCodes.Ldloc_S, boolVal);
        //            deserializeIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
        //        }
        //        #endregion

        //        #region OPCodes for other types DeSerialization

        //        else
        //        {
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES

        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
        //            deserializeIL.Emit(OpCodes.Stfld, fld);//PU
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
        //        }

        //        #endregion

        //    }
        //}

        //private void DeserializeJsonPropertys<T>(Type type, ILGenerator deserializeIL, LocalBuilder tpmRetEvent, LocalBuilder tpmRetEvent2)
        //{
        //    foreach (PropertyInfo pi in type.GetProperties())
        //    {
        //        if (pi.PropertyType == typeof(string[]))
        //            continue;
        //        if (Utils.IsIgnoreAttribute(pi.GetCustomAttributes(true)))
        //            continue;

        //        MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(pi.PropertyType));
        //        MethodInfo setProp = type.GetMethod("set_" + pi.Name);
        //        MethodInfo getProp = type.GetMethod("get_" + pi.Name);

        //        #region OPCodes for DateTime DeSerialization
        //        if (pi.PropertyType == typeof(DateTime))
        //        {
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

        //            //似乎没用
        //            //FieldBuilder dateTimeFld = typeBuilder.DefineField("Ticks ", typeof(Int64), FieldAttributes.Public);
        //            ConstructorInfo ctorDtTime = typeof(DateTime).GetConstructor(new Type[] { typeof(Int64) });
        //            deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
        //            deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
        //        }
        //        #endregion

        //        #region OPCodes for IDictionary DeSerialization

        //        else if (pi.PropertyType == typeof(IDictionary))
        //        {
        //            Label loopLabelBegin = deserializeIL.DefineLabel();
        //            Label loopLabelEnd = deserializeIL.DefineLabel();

        //            LocalBuilder count = deserializeIL.DeclareLocal(typeof(Int32));
        //            LocalBuilder key = deserializeIL.DeclareLocal(typeof(string));
        //            LocalBuilder value = deserializeIL.DeclareLocal(typeof(string));
        //            LocalBuilder boolVal = deserializeIL.DeclareLocal(typeof(bool));

        //            MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
        //            MethodInfo dicAdd = typeof(IDictionary).GetMethod("Add", new Type[] { typeof(object), typeof(object) });
        //            MethodInfo brReadInt = typeof(T).GetMethod(CreateReaderMethod(typeof(int)));

        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brReadInt, null);//PU
        //            deserializeIL.Emit(OpCodes.Stloc, count);

        //            deserializeIL.Emit(OpCodes.Br, loopLabelEnd);

        //            deserializeIL.MarkLabel(loopLabelBegin); //begin loop 
        //            deserializeIL.Emit(OpCodes.Nop);
        //            deserializeIL.Emit(OpCodes.Ldarg_1);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
        //            deserializeIL.Emit(OpCodes.Stloc, key);
        //            deserializeIL.Emit(OpCodes.Ldarg_1);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
        //            deserializeIL.Emit(OpCodes.Stloc, value);

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, getProp, null);
        //            deserializeIL.Emit(OpCodes.Ldloc, key);
        //            deserializeIL.Emit(OpCodes.Ldloc, value);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, dicAdd, null);//call add method
        //            deserializeIL.Emit(OpCodes.Nop);

        //            deserializeIL.Emit(OpCodes.Ldloc, count);
        //            deserializeIL.Emit(OpCodes.Ldc_I4_1);
        //            deserializeIL.Emit(OpCodes.Sub);
        //            deserializeIL.Emit(OpCodes.Stloc, count);

        //            deserializeIL.MarkLabel(loopLabelEnd); //end loop 
        //            deserializeIL.Emit(OpCodes.Nop);
        //            deserializeIL.Emit(OpCodes.Ldloc, count);
        //            deserializeIL.Emit(OpCodes.Ldc_I4_0);
        //            deserializeIL.Emit(OpCodes.Ceq);
        //            deserializeIL.Emit(OpCodes.Ldc_I4_0);
        //            deserializeIL.Emit(OpCodes.Ceq);
        //            deserializeIL.Emit(OpCodes.Stloc_S, boolVal);
        //            deserializeIL.Emit(OpCodes.Ldloc_S, boolVal);
        //            deserializeIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);


        //        }
        //        #endregion

        //        #region List

        //        else if (pi.PropertyType == typeof(List<int>))
        //        {
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

        //            //FieldBuilder dateTimeFld = typeBuilder.DefineField("Ticks ", typeof(Int64), FieldAttributes.Public);
        //            ConstructorInfo ctorDtTime = typeof(List<int>).GetConstructor(new Type[] { typeof(int[]) });
        //            deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
        //            deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
        //        }
        //        else if (pi.PropertyType == typeof(IList<int>))
        //        {
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

        //            //FieldBuilder dateTimeFld = typeBuilder.DefineField("Ticks ", typeof(Int64), FieldAttributes.Public);
        //            ConstructorInfo ctorDtTime = typeof(List<int>).GetConstructor(new Type[] { typeof(int[]) });
        //            deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
        //            deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
        //        }

        //        #endregion

        //        #region OPCodes for other types DeSerialization
        //        else
        //        {
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

        //            deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU


        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);

        //        }
        //        #endregion
        //    }
        //}

        #endregion
    }
}
