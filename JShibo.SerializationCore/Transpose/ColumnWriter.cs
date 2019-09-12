using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Transpose
{
    public class ColumnWriter: IWriterBaseType
    {
        #region 字段

        int pos = 0;
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

        #region 构造函数

        public ColumnWriter(Type type,string name, int count)
        {
            this.type = type;
            this.name = name;
            if (type == typeof(bool))
                boolValues = new bool[count];
            else if (type == typeof(char))
                charValues = new char[count];
            else if (type == typeof(sbyte))
                sbyteValues = new sbyte[count];
            else if (type == typeof(ushort))
                ushortValues = new ushort[count];
            else if (type == typeof(float))
                floatValues = new float[count];
            else if (type == typeof(double))
                doubleValues = new double[count];
            else if (type == typeof(decimal))
                decimalValues = new decimal[count];
            else if (type == typeof(string))
                stringValues = new string[count];
            else if (type == typeof(DateTime))
                datetimeValues = new DateTime[count];
            else if (type == typeof(byte))
                byteValues = new byte[count];
            else if (type == typeof(short))
                shortValues = new short[count];
            else if (type == typeof(int))
                intValues = new int[count];
            else if (type == typeof(long))
                longValues = new long[count];
            else if (type == typeof(object))
                objectValues = new object[count];
            else if (type == typeof(DateTimeOffset))
                dateTimeOffsetValues = new DateTimeOffset[count];
            else if (type == typeof(uint))
                uintValues = new uint[count];
            else if (type == typeof(ulong))
                ulongValues = new ulong[count];
            else if (type == typeof(TimeSpan))
                timeSpanValues = new TimeSpan[count];
            else if (type == typeof(Guid))
                guidValues = new Guid[count];
            
        }

        #endregion

        #region 方法

        public void Write(byte value)
        {
            byteValues[pos] = value;
            pos++;
        }

        public void Write(short value)
        {
            shortValues[pos] = value;
            pos++;
        }

        public void Write(int value)
        {
            intValues[pos] = value;
            pos++;
        }

        public void Write(long value)
        {
            longValues[pos] = value;
            pos++;
        }

        public void Write(object value)
        {
            objectValues[pos] = value;
            pos++;
        }

        public void Write(bool value)
        {
            boolValues[pos] = value;
            pos++;
        }

        public void Write(char value)
        {
            charValues[pos] = value;
            pos++;
        }

        public void Write(sbyte value)
        {
            sbyteValues[pos] = value;
            pos++;
        }

        public void Write(ushort value)
        {
            ushortValues[pos] = value;
            pos++;
        }

        public void Write(uint value)
        {
            uintValues[pos] = value;
            pos++;
        }

        public void Write(ulong value)
        {
            ulongValues[pos] = value;
            pos++;
        }

        public void Write(float value)
        {
            floatValues[pos] = value;
            pos++;
        }

        public void Write(double value)
        {
            doubleValues[pos] = value;
            pos++;
        }

        public void Write(decimal value)
        {
            decimalValues[pos] = value;
            pos++;
        }

        public void Write(DateTime value)
        {
            datetimeValues[pos] = value;
            pos++;
        }

        public void Write(string value)
        {
            stringValues[pos] = value;
            pos++;
        }

        public void Write(DateTimeOffset value)
        {
            dateTimeOffsetValues[pos] = value;
            pos++;
        }

        public void Write(TimeSpan value)
        {
            timeSpanValues[pos] = value;
            pos++;
        }

        public void Write(Guid value)
        {
            guidValues[pos] = value;
            pos++;
        }

        #endregion

        #region 公共

        public object GetValue()
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

        #endregion
    }
}
