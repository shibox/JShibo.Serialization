using System;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Common
{
    /// <summary>
    /// 快速的将字符串字符串数据转换成值
    /// 数字最小值可能有风险吗
    /// </summary>
    public class FastToValue
    {
        #region 常量

        const byte BYTE_F = (byte)'-';
        const byte COMMA = (byte)',';
        const byte OBJECT_END = (byte)'}';

        #endregion

        #region 字段

        internal static bool[] ends = new bool[128];


        #endregion

        #region 构造函数

        static FastToValue()
        {
            ends[','] = true;
            ends[']'] = true;
        }

        #endregion

        #region byte

        public static float ToFloat(byte[] buffer, ref int pos)
        {
            return 0;
        }

        public static double ToDouble(byte[] buffer, ref int pos)
        {
            return 0;
        }

        public static decimal ToDecimal(byte[] buffer, ref int pos)
        {
            return 0;
        }

        public static DateTime ToDateTime(byte[] buffer, ref int pos)
        {
            return DateTime.Now;
        }

        public static DateTimeOffset ToDateTimeOffset(byte[] buffer, ref int pos)
        {
            return DateTime.Now;
        }

        public static Guid ToGuid(byte[] buffer, ref int pos)
        {
            return new Guid();
        }

        public static TimeSpan ToTimeSpan(byte[] buffer, ref int pos)
        {
            return new TimeSpan();
        }

        public static Uri ToUri(byte[] buffer, ref int pos)
        {
            return new Uri("");
        }

        #endregion

        #region char

        public static float ToFloat(char[] buffer, ref int pos)
        {
            return 0;
        }

        public static double ToDouble(char[] buffer, ref int pos)
        {
            return 0;
        }

        public static decimal ToDecimal(char[] buffer, ref int pos)
        {
            return 0;
        }

        public static DateTime ToDateTime(char[] buffer, ref int pos)
        {
            return DateTime.Now;
        }

        public static DateTimeOffset ToDateTimeOffset(char[] buffer, ref int pos)
        {
            return DateTime.Now;
        }

        public static Guid ToGuid(char[] buffer, ref int pos)
        {
            return new Guid();
        }

        public static TimeSpan ToTimeSpan(char[] buffer, ref int pos)
        {
            return new TimeSpan();
        }

        public unsafe static Uri ToUri(char[] buffer, ref int pos)
        {
            pos++;
            fixed (char* pd = &buffer[pos])
            {
                char* tpd = pd;
                int i = pos;
                for (; i < buffer.Length; i++)
                {
                    if (*tpd++ == '"')
                        break;
                }
                Uri uri = new Uri(new string(buffer, pos, i - pos));
                pos = i + 1;
                return uri;
            }
        }

        #endregion

        #region string

        public static float ToFloat(string buffer, ref int pos,char symbol)
        {
            int p = buffer.IndexOf(',', pos);
            if (p != -1)
            {
                float value = float.Parse(buffer.Substring(pos, pos - p));
                pos = p + 1;
                return value;
            }
            return 0;
        }

        public static double ToDouble(string buffer, ref int pos, char symbol)
        {
            int p = buffer.IndexOf(',', pos);
            if (p != -1)
            {
                double value = double.Parse(buffer.Substring(pos, pos - p));
                pos = p + 1;
                return value;
            }
            return 0;
        }

        public static decimal ToDecimal(string buffer, ref int pos, char symbol)
        {
            int p = buffer.IndexOf(',', pos);
            if (p != -1)
            {
                decimal value = decimal.Parse(buffer.Substring(pos, pos - p));
                pos = p + 1;
                return value;
            }
            return 0;
        }

        public static DateTime ToDateTime(string buffer, ref int pos, char symbol)
        {
            int p = buffer.IndexOf('"', pos);
            if (p != -1)
            {
                DateTime value = DateTime.Parse(buffer.Substring(pos, pos - p));
                pos = p + 2;
                return value;
            }
            return DateTime.Now;
        }

        public static DateTimeOffset ToDateTimeOffset(string buffer, ref int pos, char symbol)
        {
            int p = buffer.IndexOf('"', pos);
            if (p != -1)
            {
                DateTimeOffset value = DateTimeOffset.Parse(buffer.Substring(pos, pos - p));
                pos = p + 2;
                return value;
            }
            return DateTime.Now;
        }

        public static Guid ToGuid(string buffer, ref int pos, char symbol)
        {
            int p = buffer.IndexOf('"', pos);
            if (p != -1)
            {
                Guid value = Guid.Parse(buffer.Substring(pos, pos - p));
                pos = p + 2;
                return value;
            }
            return new Guid();
        }

        public static TimeSpan ToTimeSpan(string buffer, ref int pos, char symbol)
        {
            int p = buffer.IndexOf('"', pos);
            if (p != -1)
            {
                TimeSpan value = TimeSpan.Parse(buffer.Substring(pos, pos - p));
                pos = p + 2;
                return value;
            }
            return new TimeSpan();
        }

        public unsafe static Uri ToUri(string buffer, ref int pos, char symbol)
        {
            pos++;
            fixed (char* pd = buffer)
            {
                char* tpd = pd;
                tpd += pos;
                int i = pos;
                for (; i < buffer.Length; i++)
                {
                    if (*tpd++ == '"')
                        break;
                }
                Uri uri = new Uri(buffer.Substring(pos, i - pos));
                pos = i + 2;
                return uri;
            }
        }

        #endregion

        #region 直接转换

        public unsafe static bool TryToInt32(string buffer, ref int pos, char symbol,out int value)
        {
            value = 0;
            int start = pos;
            fixed (char* pd = buffer)
            {
                char* p = pd;
                p += pos;
                if (*p == '-')
                {
                    value += (*(p + 1) - 48);
                    value = -value;
                    p += 2;
                    start += 2;
                    for (; start < buffer.Length && *p != symbol && *p != ']'; start++)
                    {
                        value *= 10;
                        value -= (*p - 48);
                        p++;
                    }
                    if (*p == ']')
                        return true;
                }
                else
                {
                    for (; start < buffer.Length && *p != symbol && *p != ']'; start++)
                    {
                        value *= 10;
                        value += (*p - 48);
                        p++;
                    }
                    if (*p == ']')
                        return true;
                }
                pos = start;
                return false;
            }
        }

        public unsafe static bool TryToInt32B(string buffer, ref int pos, char symbol, out int value)
        {
            value = 0;
            int start = pos;
            fixed (char* pd = buffer)
            {
                char* p = pd;
                p += pos;
                if (*p == '-')
                {
                    value += (*(p + 1) - 48);
                    value = -value;
                    p += 2;
                    start += 2;
                    for (; start < buffer.Length && ends[*p] != true; start++)
                    {
                        value *= 10;
                        value -= (*p - 48);
                        p++;
                    }
                    if (*p == ']')
                        return true;
                }
                else
                {
                    for (; start < buffer.Length && ends[*p] != true; start++)
                    {
                        value *= 10;
                        value += (*p - 48);
                        p++;
                    }
                    if (*p == ']')
                        return true;
                }
                pos = start;
                return false;
            }
        }

        public unsafe static char* TryToInt32(char* p, ref int pos, out int value)
        {
            int position = pos;
            value = 0;
            if (*p == '-')
            {
                value = -(*(p + 1) - 48);
                p += 2;
                position += 2;
                for (; *p >= '0' && *p <= '9'; position++)
                    value = value * 10 - ((*p++) - 48);
            }
            else
            {
                for (; *p >= '0' && *p <= '9'; position++)
                    value = value * 10 + ((*p++) - 48);
            }
            pos = position;
            return p;
        }

        public unsafe static int ToInt32(string buffer, ref int pos, char symbol)
        {
            int value = 0, start = pos;
            fixed (char* pd = buffer)
            {
                char* p = pd;
                p += pos;
                if (*p == '-')
                {
                    value += (*(p+1) - 48);
                    value = -value;
                    p+=2;
                    start += 2;
                    for (; start < buffer.Length && *p != symbol; start++)
                    {
                        value *= 10;
                        value -= (*p - 48);
                        p++;
                    }
                }
                else
                {
                    for (; start < buffer.Length && *p != symbol; start++)
                    {
                        value *= 10;
                        value += (*p - 48);
                        p++;
                    }
                }
                pos = start;
                return value;
            }

            #region safe
            //int value = 0, start = pos;
            //if (buffer[start] == '-')
            //{
            //    value += (buffer[start + 1] - 48);
            //    value = -value;
            //    start += 2;
            //    for (; start < buffer.Length && buffer[start] != symbol; start++)
            //    {
            //        value *= 10;
            //        value -= (buffer[start] - 48);
            //    }
            //}
            //else
            //{
            //    for (; start < buffer.Length && buffer[start] != symbol; start++)
            //    {
            //        value *= 10;
            //        value += (buffer[start] - 48);
            //    }
            //}
            //pos = start;
            //return value;
            #endregion
        }

        public unsafe static long ToInt64(string buffer, ref int pos, char symbol)
        {
            long value = 0;
            int start = pos;
            fixed (char* pd = buffer)
            {
                char* p = pd;
                p += pos;
                if (*p == '-')
                {
                    value += (*(p + 1) - 48);
                    value = -value;
                    p += 2;
                    start += 2;
                    for (; start < buffer.Length && *p != symbol; start++)
                    {
                        value *= 10;
                        value -= (*p - 48);
                        p++;
                    }
                }
                else
                {
                    for (; start < buffer.Length && *p != symbol; start++)
                    {
                        value *= 10;
                        value += (*p - 48);
                        p++;
                    }
                }
                pos = start;
                return value;
            }
        }

        public unsafe static uint ToUInt32(string buffer, ref int pos, char symbol)
        {
            uint value = 0;
            int start = pos;
            for (; start < buffer.Length && buffer[start] != symbol; start++)
            {
                value *= 10;
                value += (uint)(buffer[start] - 48);
            }
            pos = start;
            return value;
        }

        public unsafe static ulong ToUInt64(string buffer, ref int pos, char symbol)
        {
            ulong value = 0;
            int start = pos;
            for (; start < buffer.Length && buffer[start] != ','; start++)
            {
                value *= 10;
                value += (ulong)(buffer[start] - 48);
            }
            pos = start;
            return value;
        }



        public unsafe static int ToInt32(byte[] buffer, ref int pos)
        {
            int value = 0, start = pos;
            if (buffer[start] == '-')
            {
                value = -value;
                start++;
            }
            for (; start < buffer.Length && buffer[start] != ','; start++)
            {
                value *= 10;
                value += (buffer[start] - 48);
            }
            pos = start;
            return value;
        }

        public unsafe static long ToInt64(byte[] buffer, ref int pos)
        {
            long value = 0;
            int start = pos;
            if (buffer[start] == '-')
            {
                value = -value;
                start++;
            }
            for (; start < buffer.Length && buffer[start] != ','; start++)
            {
                value *= 10;
                value += (buffer[start] - 48);
            }
            pos = start;
            return value;
        }

        public unsafe static uint ToUInt32(byte[] buffer, ref int pos)
        {
            uint value = 0;
            int start = pos;
            for (; start < buffer.Length && buffer[start] != ','; start++)
            {
                value *= 10;
                value += (uint)(buffer[start] - 48);
            }
            pos = start;
            return value;
        }

        public unsafe static ulong ToUInt64(byte[] buffer, ref int pos)
        {
            ulong value = 0;
            int start = pos;
            for (; start < buffer.Length && buffer[start] != ','; start++)
            {
                value *= 10;
                value += (ulong)(buffer[start] - 48);
            }
            pos = start;
            return value;
        }



        public unsafe static int ToInt32(char[] buffer, ref int pos)
        {
            int value = 0, start = pos;
            if (buffer[start] == '-')
            {
                value = -value;
                start++;
            }
            for (; start < buffer.Length && buffer[start] != ','; start++)
            {
                value *= 10;
                value += (buffer[start] - 48);
            }
            pos = start;
            return value;
        }

        public unsafe static long ToInt64(char[] buffer, ref int pos)
        {
            long value = 0;
            int start = pos;
            if (buffer[start] == '-')
            {
                value = -value;
                start++;
            }
            for (; start < buffer.Length && buffer[start] != ','; start++)
            {
                value *= 10;
                value += (buffer[start] - 48);
            }
            pos = start;
            return value;
        }

        public unsafe static uint ToUInt32(char[] buffer, ref int pos)
        {
            uint value = 0;
            int start = pos;
            for (; start < buffer.Length && buffer[start] != ','; start++)
            {
                value *= 10;
                value += (uint)(buffer[start] - 48);
            }
            pos = start;
            return value;
        }

        public unsafe static ulong ToUInt64(char[] buffer, ref int pos)
        {
            ulong value = 0;
            int start = pos;
            for (; start < buffer.Length && buffer[start] != ','; start++)
            {
                value *= 10;
                value += (ulong)(buffer[start] - 48);
            }
            pos = start;
            return value;
        }

        #endregion

    }
}
