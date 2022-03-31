using JShibo.Serialization.Common;
using JShibo.Serialization.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Transpose
{
    /// <summary>
    /// 行列转换最好的是使用代码生成模式，性能最好
    /// </summary>
    public unsafe class PivotEncode:IDisposable
    {
        #region 字段

        (Type Type,string Name)[] typename = null;
        List<object> used = new();
        bool[][] boolValues = null;
        char[][] charValues = null;
        sbyte[][] sbyteValues = null;
        ushort[][] ushortValues = null;
        float[][] floatValues = null;
        double[][] doubleValues = null;
        decimal[][] decimalValues = null;
        string[][] stringValues = null;
        DateTime[][] datetimeValues = null;
        byte[][] byteValues = null;
        short[][] shortValues = null;
        int[][] intValues = null;
        long[][] longValues = null;
        object[][] objectValues = null;
        DateTimeOffset[][] dateTimeOffsetValues = null;
        uint[][] uintValues = null;
        ulong[][] ulongValues = null;
        TimeSpan[][] timeSpanValues = null;
        Guid[][] guidValues = null;
        Uri[][] uriValues = null;
        bool useCache;
        bool dispose = false;

        /// <summary>
        /// 数组中第几个元素
        /// </summary>
        internal int num = 0;
        /// <summary>
        /// 当前写的是哪个数组
        /// </summary>
        internal int idx = 0;
        

        #endregion
        
        #region 构造函数

        public PivotEncode(Type tp,int count,bool useCache = true)
        {
            this.useCache = useCache;
            var vs = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (vs.Length > 0)
            {
                var cols = vs.Length;
                var types = vs.Select(x => x.PropertyType).Distinct();
                foreach (var type in types)
                {
                    if (type == typeof(bool))
                    {
                        boolValues = new bool[cols][];
                        used.Add(boolValues);
                    }
                    else if (type == typeof(char))
                    {
                        charValues = new char[cols][];
                        used.Add(charValues);
                    }
                    else if (type == typeof(sbyte))
                    {
                        sbyteValues = new sbyte[cols][];
                        used.Add(sbyteValues);
                    }
                    else if (type == typeof(ushort))
                    {
                        ushortValues = new ushort[cols][];
                        used.Add(ushortValues);
                    }
                    else if (type == typeof(float))
                    {
                        floatValues = new float[cols][];
                        used.Add(floatValues);
                    }
                    else if (type == typeof(double))
                    {
                        doubleValues = new double[cols][];
                        used.Add(doubleValues);
                    }
                    else if (type == typeof(decimal))
                    {
                        decimalValues = new decimal[cols][];
                        used.Add(decimalValues);
                    }
                    else if (type == typeof(string))
                    {
                        stringValues = new string[cols][];
                        used.Add(stringValues);
                    }
                    else if (type == typeof(DateTime))
                    {
                        datetimeValues = new DateTime[cols][];
                        used.Add(datetimeValues);
                    }
                    else if (type == typeof(byte))
                    {
                        byteValues = new byte[cols][];
                        used.Add(byteValues);
                    }
                    else if (type == typeof(short))
                    {
                        shortValues = new short[cols][];
                        used.Add(shortValues);
                    }
                    else if (type == typeof(int))
                    {
                        intValues = new int[cols][];
                        used.Add(intValues);
                    }
                    else if (type == typeof(long))
                    {
                        longValues = new long[cols][];
                        used.Add(longValues);
                    }
                    else if (type == typeof(object))
                    {
                        objectValues = new object[cols][];
                        used.Add(objectValues);
                    }
                    else if (type == typeof(DateTimeOffset))
                    {
                        dateTimeOffsetValues = new DateTimeOffset[cols][];
                        used.Add(dateTimeOffsetValues);
                    }
                    else if (type == typeof(uint))
                    {
                        uintValues = new uint[cols][];
                        used.Add(uintValues);
                    }
                    else if (type == typeof(ulong))
                    {
                        ulongValues = new ulong[cols][];
                        used.Add(ulongValues);
                    }
                    else if (type == typeof(TimeSpan))
                    {
                        timeSpanValues = new TimeSpan[cols][];
                        used.Add(timeSpanValues);
                    }
                    else if (type == typeof(Guid))
                    {
                        guidValues = new Guid[cols][];
                        used.Add(guidValues);
                    }
                }

                //boolValues = new bool[cols][];
                //charValues = new char[cols][];
                //sbyteValues = new sbyte[cols][];
                //ushortValues = new ushort[cols][];
                //floatValues = new float[cols][];
                //doubleValues = new double[cols][];
                //decimalValues = new decimal[cols][];
                //stringValues = new string[cols][];
                //datetimeValues = new DateTime[cols][];
                //byteValues = new byte[cols][];
                //shortValues = new short[cols][];
                //intValues = new int[cols][];
                //longValues = new long[cols][];
                //objectValues = new object[cols][];
                //dateTimeOffsetValues = new DateTimeOffset[cols][];
                //uintValues = new uint[cols][];
                //ulongValues = new ulong[cols][];
                //timeSpanValues = new TimeSpan[cols][];
                //guidValues = new Guid[cols][];
                typename = new (Type type, string name)[vs.Length];

                for (int i = 0; i < vs.Length; i++)
                {
                    typename[i]= (vs[i].PropertyType, vs[i].Name);
                    var cap = count;
                    var type = vs[i].PropertyType;
                    if (type == typeof(bool))
                        boolValues[i] = useCache == true ? ArrayPool<bool>.Shared.Rent(cap) : new bool[cap];
                    else if (type == typeof(char))
                        charValues[i] = useCache == true ? ArrayPool<char>.Shared.Rent(cap) : new char[cap];
                    else if (type == typeof(sbyte))
                        sbyteValues[i] = useCache == true ? ArrayPool<sbyte>.Shared.Rent(cap) : new sbyte[cap];
                    else if (type == typeof(ushort))
                        ushortValues[i] = useCache == true ? ArrayPool<ushort>.Shared.Rent(cap) : new ushort[cap];
                    else if (type == typeof(float))
                        floatValues[i] = useCache == true ? ArrayPool<float>.Shared.Rent(cap) : new float[cap];
                    else if (type == typeof(double))
                        doubleValues[i] = useCache == true ? ArrayPool<double>.Shared.Rent(cap) : new double[cap];
                    else if (type == typeof(decimal))
                        decimalValues[i] = useCache == true ? ArrayPool<decimal>.Shared.Rent(cap) : new decimal[cap];
                    else if (type == typeof(string))
                        stringValues[i] = new string[cap];
                    else if (type == typeof(DateTime))
                        datetimeValues[i] = useCache == true ? ArrayPool<DateTime>.Shared.Rent(cap) : new DateTime[cap];
                    else if (type == typeof(byte))
                    {
                        //byteValues[i] = new byte[cap];
                        byteValues[i] = useCache == true ? ArrayPool<byte>.Shared.Rent(cap) : new byte[cap];
                        //byteValues[i] = GC.AllocateUninitializedArray<byte>(cap, true);
                        //fixed (byte* ptr = byteValues)
                        //    bytePtr = ptr;
                    }
                    else if (type == typeof(short))
                        shortValues[i] = useCache == true ? ArrayPool<short>.Shared.Rent(cap) : new short[cap];
                    else if (type == typeof(int))
                        intValues[i] = useCache == true ? ArrayPool<int>.Shared.Rent(cap) : new int[cap];
                    else if (type == typeof(long))
                        longValues[i] = useCache == true ? ArrayPool<long>.Shared.Rent(cap) : new long[cap];
                    else if (type == typeof(object))
                        objectValues[i] = new object[cap];
                    else if (type == typeof(DateTimeOffset))
                        dateTimeOffsetValues[i] = useCache == true ? ArrayPool<DateTimeOffset>.Shared.Rent(cap) : new DateTimeOffset[cap];
                    else if (type == typeof(uint))
                        uintValues[i] = useCache == true ? ArrayPool<uint>.Shared.Rent(cap) : new uint[cap];
                    else if (type == typeof(ulong))
                        ulongValues[i] = useCache == true ? ArrayPool<ulong>.Shared.Rent(cap) : new ulong[cap];
                    else if (type == typeof(TimeSpan))
                        timeSpanValues[i] = useCache == true ? ArrayPool<TimeSpan>.Shared.Rent(cap) : new TimeSpan[cap];
                    else if (type == typeof(Guid))
                        guidValues[i] = useCache == true ? ArrayPool<Guid>.Shared.Rent(cap) : new Guid[cap];
                    else
                        throw new NotSupportedException("不支持该类型");
                }
            }
            else
            {
                //按照字段类型处理
                //var fields = tp.GetFields(BindingFlags.Instance | BindingFlags.Public);
            }
        }

        public PivotEncode(Type[] type, int count)
        {
            //writers = new ColumnWriter[type.Length];
            //for (int i = 0; i < type.Length; i++)
            //    writers[i] = new ColumnWriter(type[i], type[i].Name, count);
        }

        #endregion

        #region 方法

        #endregion

        #region Write BaseType

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(byte value)
        {
            //writers[idx].Write(value);
            //idx++;
            //bytes[idx][num] = value;
            //idx++;
            //*bytesPtr[idx] = value;
            //(*bytesPtr[idx])++;
            byteValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(sbyte value)
        {
            sbyteValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(short value)
        {
            shortValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(ushort value)
        {
            ushortValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(int value)
        {
            intValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(uint value)
        {
            uintValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(long value)
        {
            longValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(ulong value)
        {
            ulongValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(float value)
        {
            floatValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(double value)
        {
            doubleValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(decimal value)
        {
            decimalValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(string value)
        {
            stringValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(bool value)
        {
            boolValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(char value)
        {
            charValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void Write(DateTime value)
        {
            datetimeValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void Write(DateTimeOffset value)
        {
            dateTimeOffsetValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void Write(TimeSpan value)
        {
            timeSpanValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void Write(Guid value)
        {
            guidValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe void Write(Uri value)
        {
            uriValues[idx][num] = value;
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Write(object value)
        {
            objectValues[idx][num] = value;
            idx++;
        }

        #endregion

        /// <summary>
        /// 获得行转列后的结果
        /// </summary>
        /// <returns></returns>
        public DataColumn[] GetResult()
        {
            if (dispose == false)
            {
                return typename.Select((item, i) => new DataColumn()
                {
                    Type = item.Type,
                    Name = item.Name,
                    Value = GetValue(item.Type, i),
                }).ToArray();
            }
            return null;
        }

        public void Dispose()
        {
            if (dispose == false)
            {//如果使用缓存，使用后要归还
                if (useCache)
                {
                    var i = 0;
                    foreach (var item in typename)
                    {
                        if (item.Type == typeof(bool))
                            ArrayPool<bool>.Shared.Return(boolValues[i]);
                        else if (item.Type == typeof(char))
                            ArrayPool<char>.Shared.Return(charValues[i]);
                        else if (item.Type == typeof(sbyte))
                            ArrayPool<sbyte>.Shared.Return(sbyteValues[i]);
                        else if (item.Type == typeof(ushort))
                            ArrayPool<ushort>.Shared.Return(ushortValues[i]);
                        else if (item.Type == typeof(float))
                            ArrayPool<float>.Shared.Return(floatValues[i]);
                        else if (item.Type == typeof(double))
                            ArrayPool<double>.Shared.Return(doubleValues[i]);
                        else if (item.Type == typeof(decimal))
                            ArrayPool<decimal>.Shared.Return(decimalValues[i]);
                        //else if (item.Type == typeof(string))
                        //    ArrayPool<string>.Shared.Return(stringValues[i]);
                        else if (item.Type == typeof(DateTime))
                            ArrayPool<DateTime>.Shared.Return(datetimeValues[i]);
                        else if (item.Type == typeof(byte))
                            ArrayPool<byte>.Shared.Return(byteValues[i]);
                        else if (item.Type == typeof(short))
                            ArrayPool<short>.Shared.Return(shortValues[i]);
                        else if (item.Type == typeof(int))
                            ArrayPool<int>.Shared.Return(intValues[i]);
                        else if (item.Type == typeof(long))
                            ArrayPool<long>.Shared.Return(longValues[i]);
                        //else if (item.Type == typeof(object))
                        //    ArrayPool<bool>.Shared.Return(objectValues[i]);
                        else if (item.Type == typeof(DateTimeOffset))
                            ArrayPool<DateTimeOffset>.Shared.Return(dateTimeOffsetValues[i]);
                        else if (item.Type == typeof(uint))
                            ArrayPool<uint>.Shared.Return(uintValues[i]);
                        else if (item.Type == typeof(ulong))
                            ArrayPool<ulong>.Shared.Return(ulongValues[i]);
                        else if (item.Type == typeof(TimeSpan))
                            ArrayPool<TimeSpan>.Shared.Return(timeSpanValues[i]);
                        else if (item.Type == typeof(Guid))
                            ArrayPool<Guid>.Shared.Return(guidValues[i]);
                        idx++;
                    }
                };
                used.ForEach(item => item = null);
                used = null;
                dispose = true;
            }
        }

        private Array GetValue(Type type, int i)
        {
            if (type == typeof(bool))
                return boolValues[i];
            else if (type == typeof(char))
                return charValues[i];
            else if (type == typeof(sbyte))
                return sbyteValues[i];
            else if (type == typeof(ushort))
                return ushortValues[i];
            else if (type == typeof(float))
                return floatValues[i];
            else if (type == typeof(double))
                return doubleValues[i];
            else if (type == typeof(decimal))
                return decimalValues[i];
            else if (type == typeof(string))
                return stringValues[i];
            else if (type == typeof(DateTime))
                return datetimeValues[i];
            else if (type == typeof(byte))
                return byteValues[i];
            else if (type == typeof(short))
                return shortValues[i];
            else if (type == typeof(int))
                return intValues[i];
            else if (type == typeof(long))
                return longValues[i];
            else if (type == typeof(object))
                return objectValues[i];
            else if (type == typeof(DateTimeOffset))
                return dateTimeOffsetValues[i];
            else if (type == typeof(uint))
                return uintValues[i];
            else if (type == typeof(ulong))
                return ulongValues[i];
            else if (type == typeof(TimeSpan))
                return timeSpanValues[i];
            else if (type == typeof(Guid))
                return guidValues[i];
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Reset()
        {
            idx = 0;
            num++;
        }

    }
}
