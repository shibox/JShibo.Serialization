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
    public unsafe class PivotEncode:IDisposable
    {
        #region 字段

        internal SerializerSettings sets = SerializerSettings.Default;
        internal ColumnWriter[] writers = null;

        Type[] types = null;
        string[] names = null;
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
            var properties = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length > 0)
            {
                writers = new ColumnWriter[properties.Length];
                var cols = properties.Length;
                types = new Type[cols];
                names = new string[cols];
                boolValues = new bool[cols][];
                charValues = new char[cols][];
                sbyteValues = new sbyte[cols][];
                ushortValues = new ushort[cols][];
                floatValues = new float[cols][];
                doubleValues = new double[cols][];
                decimalValues = new decimal[cols][];
                stringValues = new string[cols][];
                datetimeValues = new DateTime[cols][];
                byteValues = new byte[cols][];
                shortValues = new short[cols][];
                intValues = new int[cols][];
                longValues = new long[cols][];
                objectValues = new object[cols][];
                dateTimeOffsetValues = new DateTimeOffset[cols][];
                uintValues = new uint[cols][];
                ulongValues = new ulong[cols][];
                timeSpanValues = new TimeSpan[cols][];
                guidValues = new Guid[cols][];
                for (int i = 0; i < properties.Length; i++)
                {
                    types[i]= properties[i].PropertyType;
                    names[i] = properties[i].Name;
                }

                for (int i = 0; i < properties.Length; i++)
                {
                    writers[i] = new ColumnWriter(properties[i].PropertyType, properties[i].Name, 0);
                    var cap = count;
                    var type = properties[i].PropertyType;
                    if (type == typeof(bool))
                        boolValues[i] = new bool[cap];
                    else if (type == typeof(char))
                        charValues[i] = new char[cap];
                    else if (type == typeof(sbyte))
                        sbyteValues[i] = new sbyte[cap];
                    else if (type == typeof(ushort))
                        ushortValues[i] = new ushort[cap];
                    else if (type == typeof(float))
                        floatValues[i] = new float[cap];
                    else if (type == typeof(double))
                        doubleValues[i] = new double[cap];
                    else if (type == typeof(decimal))
                        decimalValues[i] = new decimal[cap];
                    else if (type == typeof(string))
                        stringValues[i] = new string[cap];
                    else if (type == typeof(DateTime))
                        datetimeValues[i] = new DateTime[cap];
                    else if (type == typeof(byte))
                    {
                        //byteValues[i] = new byte[cap];
                        //byteValues[i] = ArrayPool<byte>.Shared.Rent(cap);
                        byteValues[i] = GC.AllocateUninitializedArray<byte>(cap, true);
                        //fixed (byte* ptr = byteValues)
                        //    bytePtr = ptr;
                    }

                    else if (type == typeof(short))
                        shortValues[i] = new short[cap];
                    else if (type == typeof(int))
                        intValues[i] = new int[cap];
                    else if (type == typeof(long))
                    {
                        //longValues[i] = new long[cap];
                        //longValues[i] = GC.AllocateUninitializedArray<long>(cap, true);
                        longValues[i] = useCache == true ? ArrayPool<long>.Shared.Rent(cap) : new long[cap];
                    }
                    else if (type == typeof(object))
                        objectValues[i] = new object[cap];
                    else if (type == typeof(DateTimeOffset))
                        dateTimeOffsetValues[i] = new DateTimeOffset[cap];
                    else if (type == typeof(uint))
                        uintValues[i] = new uint[cap];
                    else if (type == typeof(ulong))
                        ulongValues[i] = new ulong[cap];
                    else if (type == typeof(TimeSpan))
                        timeSpanValues[i] = new TimeSpan[cap];
                    else if (type == typeof(Guid))
                        guidValues[i] = new Guid[cap];
                    else
                        throw new NotSupportedException("不支持该类型");
                }
                //bytes = new byte[properties.Length][];
                //for (int i = 0; i < properties.Length; i++)
                //{
                //    //bytes[i] = new byte[count];
                //    bytes[i] = GC.AllocateUninitializedArray<byte>(count, true);
                //    //fixed (byte* ptr = bytes[i])
                //    //{
                //    //    //var v = (bytesPtr[i]);
                //    //    (bytesPtr[i]) = ptr;
                //    //}
                //}
            }
            else
            {
                //var fields = tp.GetFields(BindingFlags.Instance | BindingFlags.Public);
                //writers = new ColumnWriter[fields.Length];
                //for (int i = 0; i < fields.Length; i++)
                //    writers[i] = new ColumnWriter(fields[i].FieldType, fields[i].Name, count);
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

        internal void SetInfo(ConvertContext info)
        {
            
        }

        #endregion

        #region 私有写入

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Reset()
        {
            idx = 0;
            num++;
        }

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

        #region 公共

        public DataColumn[] GetResult()
        {
            return writers.Select((item, i) => new DataColumn()
            {
                Type = item.Type,
                Name = item.Name,
                Value = GetValue(item.Type, i),
            }).ToArray();
        }

        public Array GetValue(Type type,int i)
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

        public void Dispose()
        {
            //如果使用缓存，使用后要归还
            if (useCache)
            {
                foreach (var item in longValues)
                {
                    ArrayPool<long>.Shared.Return(item);
                }
            }
        }

        #endregion




    }
}
