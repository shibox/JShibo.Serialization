using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Transpose
{
    public unsafe class ColumnWriter: IWriterBaseType,IDisposable
    {
        #region 字段

        int count = 0;
        Type type = null;
        string name = string.Empty;
        bool[] boolValues = null;
        char[] charValues = null;
        sbyte[] sbyteValues = null;
        ushort[] ushortValues = null;
        float[] floatValues = null;
        double[] doubleValues = null;
        decimal[] decimalValues = null;
        string[] stringValues = null;
        DateTime[] datetimeValues = null;
        byte[] byteValues = null;
        short[] shortValues = null;
        int[] intValues = null;
        long[] longValues = null;
        object[] objectValues = null;
        DateTimeOffset[] dateTimeOffsetValues = null;
        uint[] uintValues = null;
        ulong[] ulongValues = null;
        TimeSpan[] timeSpanValues = null;
        Guid[] guidValues = null;

        #endregion

        #region 属性

        public int Count => count;

        public string Name => name;

        public Type Type => type;

        public object Value => GetValue();

        #endregion

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">类型名称</param>
        /// <param name="cap">初始化的容量</param>
        public ColumnWriter(Type type,string name, int cap)
        {
            this.type = type;
            this.name = name;
            if (type == typeof(bool))
                boolValues = new bool[cap];
            else if (type == typeof(char))
                charValues = new char[cap];
            else if (type == typeof(sbyte))
                sbyteValues = new sbyte[cap];
            else if (type == typeof(ushort))
                ushortValues = new ushort[cap];
            else if (type == typeof(float))
                floatValues = new float[cap];
            else if (type == typeof(double))
                doubleValues = new double[cap];
            else if (type == typeof(decimal))
                decimalValues = new decimal[cap];
            else if (type == typeof(string))
                stringValues = new string[cap];
            else if (type == typeof(DateTime))
                datetimeValues = new DateTime[cap];
            else if (type == typeof(byte))
            {
                //byteValues = new byte[cap];
                byteValues = GC.AllocateUninitializedArray<byte>(cap, true);
                //fixed (byte* ptr = byteValues)
                //    bytePtr = ptr;
            }
                
            else if (type == typeof(short))
                shortValues = new short[cap];
            else if (type == typeof(int))
                intValues = new int[cap];
            else if (type == typeof(long))
                longValues = new long[cap];
            else if (type == typeof(object))
                objectValues = new object[cap];
            else if (type == typeof(DateTimeOffset))
                dateTimeOffsetValues = new DateTimeOffset[cap];
            else if (type == typeof(uint))
                uintValues = new uint[cap];
            else if (type == typeof(ulong))
                ulongValues = new ulong[cap];
            else if (type == typeof(TimeSpan))
                timeSpanValues = new TimeSpan[cap];
            else if (type == typeof(Guid))
                guidValues = new Guid[cap];
            else
                throw new NotSupportedException("不支持该类型");
        }

        #endregion

        #region 方法

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(byte value)
        {
            //*bytePtr = value;
            //bytePtr++;
            byteValues[count] = value;
            count++;
        }

        public void Write(short value)
        {
            shortValues[count] = value;
            count++;
        }

        public void Write(int value)
        {
            intValues[count] = value;
            count++;
        }

        public void Write(long value)
        {
            longValues[count] = value;
            count++;
        }

        public void Write(object value)
        {
            objectValues[count] = value;
            count++;
        }

        public void Write(bool value)
        {
            boolValues[count] = value;
            count++;
        }

        public void Write(char value)
        {
            charValues[count] = value;
            count++;
        }

        public void Write(sbyte value)
        {
            sbyteValues[count] = value;
            count++;
        }

        public void Write(ushort value)
        {
            ushortValues[count] = value;
            count++;
        }

        public void Write(uint value)
        {
            uintValues[count] = value;
            count++;
        }

        public void Write(ulong value)
        {
            ulongValues[count] = value;
            count++;
        }

        public void Write(float value)
        {
            floatValues[count] = value;
            count++;
        }

        public void Write(double value)
        {
            doubleValues[count] = value;
            count++;
        }

        public void Write(decimal value)
        {
            decimalValues[count] = value;
            count++;
        }

        public void Write(DateTime value)
        {
            datetimeValues[count] = value;
            count++;
        }

        public void Write(string value)
        {
            stringValues[count] = value;
            count++;
        }

        public void Write(DateTimeOffset value)
        {
            dateTimeOffsetValues[count] = value;
            count++;
        }

        public void Write(TimeSpan value)
        {
            timeSpanValues[count] = value;
            count++;
        }

        public void Write(Guid value)
        {
            guidValues[count] = value;
            count++;
        }

        #endregion

        #region 公共

        /// <summary>
        /// 获取列中存储的值
        /// </summary>
        /// <returns>返回数据数组</returns>
        public Array GetValue()
        {
            if (type == typeof(bool))
                return boolValues;
            else if (type == typeof(char))
                return charValues ;
            else if (type == typeof(sbyte))
                return sbyteValues ;
            else if (type == typeof(ushort))
                return ushortValues ;
            else if (type == typeof(float))
                return floatValues ;
            else if (type == typeof(double))
                return doubleValues ;
            else if (type == typeof(decimal))
                return decimalValues ;
            else if (type == typeof(string))
                return stringValues ;
            else if (type == typeof(DateTime))
                return datetimeValues ;
            else if (type == typeof(byte))
                return byteValues ;
            else if (type == typeof(short))
                return shortValues ;
            else if (type == typeof(int))
                return intValues ;
            else if (type == typeof(long))
                return longValues ;
            else if (type == typeof(object))
                return objectValues;
            else if (type == typeof(DateTimeOffset))
                return dateTimeOffsetValues;
            else if (type == typeof(uint))
                return uintValues ;
            else if (type == typeof(ulong))
                return ulongValues ;
            else if (type == typeof(TimeSpan))
                return timeSpanValues;
            else if (type == typeof(Guid))
                return guidValues ;
            return null;
        }

        public void Dispose()
        {
            
        }

        #endregion
    }
}
