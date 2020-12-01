using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Common
{
    /// <summary>
    /// 快速的将数字转换成字符串,相比直接ToString()性能提升很多，并且减少了很多不必要的转换逻辑
    /// long类型最大19个长度
    /// TimeSpan格式化最终需要特殊处理，速度比较慢
    /// 
    /// 格式化概述 http://msdn.microsoft.com/zh-cn/library/26etazsy%28v=vs.80%29.aspx
    /// 
    /// float格式化
    /// http://msdn.microsoft.com/en-us/library/aa280873
    /// http://blog.csdn.net/krocwang/article/details/4444491
    /// http://www.cnblogs.com/fromchaos/archive/2010/12/07/1898698.html
    /// </summary>
    public unsafe static class FastToString
    {
        #region 常量

        const string Int32Min = "2147483648";
        const string Int32Max = "2147483647";
        const string Int64Min = "-9223372036854775808";
        const string Int64Max = "9223372036854775807";
        const string UInt32Max = "4294967295";
        const string UInt64Max = "18446744073709551615";

        static readonly TwoDigits[] DigitPairs;
        static readonly char[] WriteGuidLookup = new char[] 
        { '0', '0', '0', '1', '0', '2', '0', '3', '0', '4', '0', '5', '0', '6', '0', '7', '0', '8', '0', '9', '0', 'a', '0', 'b', '0', 'c', '0', 'd', '0', 'e', '0', 'f', '1', '0', '1', '1', '1', '2', '1', '3', '1', '4', '1', '5', '1', '6', '1', '7', '1', '8', '1', '9', '1', 'a', '1', 'b', '1', 'c', '1', 'd', '1', 'e', '1', 'f', '2', '0', '2', '1', '2', '2', '2', '3', '2', '4', '2', '5', '2', '6', '2', '7', '2', '8', '2', '9', '2', 'a', '2', 'b', '2', 'c', '2', 'd', '2', 'e', '2', 'f', '3', '0', '3', '1', '3', '2', '3', '3', '3', '4', '3', '5', '3', '6', '3', '7', '3', '8', '3', '9', '3', 'a', '3', 'b', '3', 'c', '3', 'd', '3', 'e', '3', 'f', '4', '0', '4', '1', '4', '2', '4', '3', '4', '4', '4', '5', '4', '6', '4', '7', '4', '8', '4', '9', '4', 'a', '4', 'b', '4', 'c', '4', 'd', '4', 'e', '4', 'f', '5', '0', '5', '1', '5', '2', '5', '3', '5', '4', '5', '5', '5', '6', '5', '7', '5', '8', '5', '9', '5', 'a', '5', 'b', '5', 'c', '5', 'd', '5', 'e', '5', 'f', '6', '0', '6', '1', '6', '2', '6', '3', '6', '4', '6', '5', '6', '6', '6', '7', '6', '8', '6', '9', '6', 'a', '6', 'b', '6', 'c', '6', 'd', '6', 'e', '6', 'f', '7', '0', '7', '1', '7', '2', '7', '3', '7', '4', '7', '5', '7', '6', '7', '7', '7', '8', '7', '9', '7', 'a', '7', 'b', '7', 'c', '7', 'd', '7', 'e', '7', 'f', '8', '0', '8', '1', '8', '2', '8', '3', '8', '4', '8', '5', '8', '6', '8', '7', '8', '8', '8', '9', '8', 'a', '8', 'b', '8', 'c', '8', 'd', '8', 'e', '8', 'f', '9', '0', '9', '1', '9', '2', '9', '3', '9', '4', '9', '5', '9', '6', '9', '7', '9', '8', '9', '9', '9', 'a', '9', 'b', '9', 'c', '9', 'd', '9', 'e', '9', 'f', 'a', '0', 'a', '1', 'a', '2', 'a', '3', 'a', '4', 'a', '5', 'a', '6', 'a', '7', 'a', '8', 'a', '9', 'a', 'a', 'a', 'b', 'a', 'c', 'a', 'd', 'a', 'e', 'a', 'f', 'b', '0', 'b', '1', 'b', '2', 'b', '3', 'b', '4', 'b', '5', 'b', '6', 'b', '7', 'b', '8', 'b', '9', 'b', 'a', 'b', 'b', 'b', 'c', 'b', 'd', 'b', 'e', 'b', 'f', 'c', '0', 'c', '1', 'c', '2', 'c', '3', 'c', '4', 'c', '5', 'c', '6', 'c', '7', 'c', '8', 'c', '9', 'c', 'a', 'c', 'b', 'c', 'c', 'c', 'd', 'c', 'e', 'c', 'f', 'd', '0', 'd', '1', 'd', '2', 'd', '3', 'd', '4', 'd', '5', 'd', '6', 'd', '7', 'd', '8', 'd', '9', 'd', 'a', 'd', 'b', 'd', 'c', 'd', 'd', 'd', 'e', 'd', 'f', 'e', '0', 'e', '1', 'e', '2', 'e', '3', 'e', '4', 'e', '5', 'e', '6', 'e', '7', 'e', '8', 'e', '9', 'e', 'a', 'e', 'b', 'e', 'c', 'e', 'd', 'e', 'e', 'e', 'f', 'f', '0', 'f', '1', 'f', '2', 'f', '3', 'f', '4', 'f', '5', 'f', '6', 'f', '7', 'f', '8', 'f', '9', 'f', 'a', 'f', 'b', 'f', 'c', 'f', 'd', 'f', 'e', 'f', 'f' };

        static readonly char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f',
            'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        static readonly char[] DigitTens = { '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '1', '1', '1', '1',
            '1', '1', '1', '1', '2', '2', '2', '2', '2', '2', '2', '2', '2', '2', '3', '3', '3', '3', '3', '3', '3',
            '3', '3', '3', '4', '4', '4', '4', '4', '4', '4', '4', '4', '4', '5', '5', '5', '5', '5', '5', '5', '5',
            '5', '5', '6', '6', '6', '6', '6', '6', '6', '6', '6', '6', '7', '7', '7', '7', '7', '7', '7', '7', '7',
            '7', '8', '8', '8', '8', '8', '8', '8', '8', '8', '8', '9', '9', '9', '9', '9', '9', '9', '9', '9', '9', };

        static readonly char[] DigitOnes = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5',
            '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6',
            '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8',
            '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', };

        static readonly int[] intSizeTable = { 9, 99, 999, 9999, 99999, 999999, 9999999, 99999999, 999999999, int.MaxValue };
        static readonly uint[] uintSizeTable = { 9, 99, 999, 9999, 99999, 999999, 9999999, 99999999, 999999999, uint.MaxValue };

        #endregion

        #region 静态方法

        static FastToString()
        {
            DigitPairs = new TwoDigits[100];
            for (var i = 0; i < 100; ++i)
            {
                DigitPairs[i] = new TwoDigits((char)('0' + (i / 10)), (char)+('0' + (i % 10)));
            }
        }

        #endregion

        #region 转换

        internal unsafe static char* ToString(char* buffer, ref int pos, float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                *buffer++ = 'n';
                *buffer++ = 'u';
                *buffer++ = 'l';
                *buffer++ = 'l';
                pos += 4;
                return buffer;
            }
            else
            {
                string v = value.ToString();
                for (int i = 0; i < v.Length; i++)
                    *buffer++ = v[i];
                pos += v.Length;
                return buffer;
            }
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                *buffer++ = 'n';
                *buffer++ = 'u';
                *buffer++ = 'l';
                *buffer++ = 'l';
                pos += 4;
                return buffer;
            }
            else
            {
                string v = value.ToString();
                for (int i = 0; i < v.Length; i++)
                    *buffer++ = v[i];
                pos += v.Length;
                return buffer;
            }
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, decimal value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                *buffer++ = v[i];
            pos += v.Length;
            return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, bool value)
        {
            if (value)
            {
                *buffer++ = 't';
                *buffer++ = 'r';
                *buffer++ = 'u';
                *buffer++ = 'e';
                //*buffer++ = ',';
                pos += 4;
            }
            else
            {
                *buffer++ = 'f';
                *buffer++ = 'a';
                *buffer++ = 'l';
                *buffer++ = 's';
                *buffer++ = 'e';
                //*buffer++ = ',';
                pos += 5;
            }
            return buffer;
        }

        internal static int ToString(char[] buffer, int pos, float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                buffer[pos] = 'n';
                buffer[pos + 1] = 'u';
                buffer[pos + 2] = 'l';
                buffer[pos + 3] = 'l';
                return 4;
            }
            else
            {
                string v = value.ToString();
                for (int i = 0; i < v.Length; i++)
                    buffer[pos + i] = v[i];
                return v.Length;
            }
        }

        internal static int ToString(char[] buffer, int pos, double value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                buffer[pos + i] = v[i];
            return v.Length;
        }

        internal static int ToString(char[] buffer, int pos, decimal value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                buffer[pos + i] = v[i];
            return v.Length;
        }

        internal static int ToString(char[] buffer, int pos, bool value)
        {
            if (value)
            {
                buffer[pos] = 't';
                buffer[pos + 1] = 'r';
                buffer[pos + 2] = 'u';
                buffer[pos + 3] = 'e';
                return 4;
            }
            else
            {
                buffer[pos] = 'f';
                buffer[pos + 1] = 'a';
                buffer[pos + 2] = 'l';
                buffer[pos + 3] = 's';
                buffer[pos + 4] = 'e';
                return 5;
            }
        }



        /// <summary>
        /// 正负数处理逻辑、整体块复制还可以优化
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public unsafe static char* ToString(char* buffer, ref int pos, int value)
        {
            bool isMinus = false;
            if (value < 0)
            {
                if (value > -10)
                {
                    *buffer++ = '-';
                    *buffer++ = (char)((-value % 10) + 48);
                    pos += 2;
                    return buffer;
                }
                else if (value > -100)
                {
                    *buffer++ = '-';
                    *buffer++ = (char)((-value / 10) + 48);
                    *buffer++ = (char)((-value % 10) + 48);
                    pos += 3;
                    return buffer;
                }
                if (value == int.MinValue)
                {
                    fixed (char* smem = Int32Min)
                    {
                        *((uint*)buffer) = *((uint*)smem);
                        *((uint*)(buffer + 2)) = *((uint*)(smem + 2));
                        *((uint*)(buffer + 4)) = *((uint*)(smem + 4));
                        *((uint*)(buffer + 6)) = *((uint*)(smem + 6));
                        *((uint*)(buffer + 8)) = *((uint*)(smem + 8));
                        *(buffer + 10) = *(smem + 10);
                        pos += 11;
                        return buffer;
                    }
                }
                isMinus = true;
                value = -value;
            }
            else if (value < 10)
            {
                *buffer++ = (char)((value % 10) + 48);
                pos += 1;
                return buffer;
            }
            else if (value < 100)
            {
                *buffer++ = (char)((value / 10) + 48);
                *buffer++ = (char)((value % 10) + 48);
                pos += 2;
                return buffer;
            }

            int size = 0;
            buffer += 11;
            do
            {
                var ix = value % 10;
                value /= 10;
                *buffer-- = (char)('0' + ix);
                size++;
            } while (value != 0);
            if (isMinus == true)
            {
                size++;
                *buffer-- = '-';
            }
            int skip = 11 - size;
            buffer -= skip;
            for (int i = 0; i < size; i++)
                *buffer++ = *(buffer + skip);
            pos += size;
            return buffer;
        }

        public unsafe static char* ToString1(char* buffer, ref int pos, int value)
        {
            if (value < 0)
            {
                *buffer++ = '-';
                //if (value > -10)
                //{
                //    *buffer++ = (char)(-value + 48);
                //    pos += 2;
                //    return buffer;
                //}
                //else if (value > -100)
                //{
                //    *buffer++ = (char)((-value / 10) + 48);
                //    *buffer++ = (char)((-value % 10) + 48);
                //    pos += 3;
                //    return buffer;
                //}
                if (value == int.MinValue)
                {
                    fixed (char* smem = Int32Min)
                    {
                        *((uint*)buffer) = *((uint*)smem);
                        *((uint*)(buffer + 2)) = *((uint*)(smem + 2));
                        *((uint*)(buffer + 4)) = *((uint*)(smem + 4));
                        *((uint*)(buffer + 6)) = *((uint*)(smem + 6));
                        *((uint*)(buffer + 8)) = *((uint*)(smem + 8));
                        pos += 11;
                        return buffer;
                    }
                }
                value = -value;
                pos++;
            }
            else if (value < 10)
            {
                *buffer++ = (char)(value + 48);
                pos += 1;
                return buffer;
            }
            else if (value < 100)
            {
                *buffer++ = (char)((value / 10) + 48);
                *buffer++ = (char)((value % 10) + 48);
                pos += 2;
                return buffer;
            }

            int size = 0;
            buffer += 9;
            do
            {
                int ix = value % 10;
                value /= 10;
                *buffer-- = (char)('0' + ix);
                size++;
            } while (value != 0);
            pos += size;
            if (size == 10)
                return buffer;
            int skip = 9 - size;
            buffer -= skip;
            for (int i = 0; i < size; i++)
                *buffer++ = *(buffer + skip);

            //int skip = 10 - size;
            //buffer -= skip - 1;
            //for (int i = 0; i < size; i++)
            //{
            //    *buffer = *(buffer + skip);
            //    buffer++;
            //}

            return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, long value)
        {
            return buffer;
        }

        public unsafe static char* ToString(char* buffer, ref int pos, uint value)
        {
            //buffer

            int size = 0;
            buffer += 11;
            do
            {
                uint ix = value % 10;
                value /= 10;
                *buffer-- = (char)('0' + ix);
                size++;
            } while (value != 0);
            int skip = 11 - size;
            buffer -= skip;
            for (int i = 0; i < size; i++)
                *buffer++ = *(buffer + skip);
            pos += size;
            return buffer;

            //int size = 0;
            //char* d = buffer + 11;
            //do
            //{
            //    var ix = value % 10;
            //    value /= 10;
            //    *d-- = (char)('0' + ix);
            //    size++;
            //} while (value != 0);
            //int skip = 11 - size;
            //d -= skip;
            //for (int i = 0; i < size; i++)
            //    *d++ = *(d + skip);
            //pos += size;
            //return d;
        }

        

        public unsafe static int ToString(char* buffer, uint value)
        {
            if (value < 10)
            {
                *buffer = (char)(value + 48);
                return 1;
            }
            else if (value < 100)
            {
                *buffer = (char)((value / 10) + 48);
                *(buffer + 1) = (char)((value % 10) + 48);
                return 2;
            }
            else if (value < 1000)
            {
                *buffer = (char)((value / 100) + 48);
                *(buffer + 1) = (char)(((value % 100) / 10) + 48);
                *(buffer + 2) = (char)((value % 10) + 48);
                return 3;
            }
            if (value <= int.MaxValue)
                return WriteCharsUInt((int)value, buffer);

            int v = (int)(value / 10);
            buffer += 9;
            *buffer-- = (char)('0' + (value % 10));
            do
            {
                int ix = v % 10;
                v /= 10;
                *buffer-- = (char)('0' + ix);
            } while (v != 0);
            return 10;
        }

        public unsafe static int ToString(char* buffer, byte value)
        {
            if (value < 10)
            {
                *buffer = (char)(value + 48);
                return 1;
            }
            else if (value < 100)
            {
                *buffer = (char)((value / 10) + 48);
                *(buffer+1) = (char)((value % 10) + 48);
                return 2;
            }
            else
            {
                *buffer = (char)((value / 100) + 48);
                *(buffer + 1) = (char)(((value % 100) / 10) + 48);
                *(buffer + 2) = (char)((value % 10) + 48);
                return 3;
            }
        }

        public unsafe static int ToString(byte* buffer, byte value)
        {
            if (value < 10)
            {
                *buffer = (byte)(value + 48);
                return 1;
            }
            else if (value < 100)
            {
                *buffer = (byte)((value / 10) + 48);
                *(buffer + 1) = (byte)((value % 10) + 48);
                return 2;
            }
            else
            {
                *buffer = (byte)((value / 100) + 48);
                *(buffer + 1) = (byte)(((value % 100) / 10) + 48);
                *(buffer + 2) = (byte)((value % 10) + 48);
                return 3;
            }
        }

        public unsafe static int ToStringNumber(byte* buffer,byte value)
        {
            const byte AsciiDigitStart = (byte)'0';
            if (value < 10)
            {
                *buffer = (byte)(value + 48);
                return 1;
            }
            else if (value < 100)
            {
                var tens = (byte)((value * 205u) >> 11); // div10, valid to 1028
                *buffer = (byte)(tens + AsciiDigitStart);
                *(buffer + 1) = (byte)(value - (tens * 10) + AsciiDigitStart);
                return 2;
            }
            else
            {
                var digit0 = (byte)((value * 41u) >> 12); // div100, valid to 1098
                var digits01 = (byte)((value * 205u) >> 11); // div10, valid to 1028
                *buffer = (byte)(digit0 + AsciiDigitStart);
                *(buffer + 1) = (byte)(digits01 - (digit0 * 10) + AsciiDigitStart);
                *(buffer + 2) = (byte)(value - (digits01 * 10) + AsciiDigitStart);
                return 3;
            }
        }

        public unsafe static int ToString(char[] buffer,int pos, byte value)
        {
            if (value < 10)
            {
                buffer[pos] = (char)((value % 10) + 48);
                return 1;
            }
            else if (value < 100)
            {
                buffer[pos] = (char)((value / 10) + 48);
                buffer[pos + 1] = (char)((value % 10) + 48);
                return 2;
            }
            else
            {
                buffer[pos] = (char)((value / 10) + 48);
                buffer[pos + 1] = (char)((value % 10) + 48);
                buffer[pos + 2] = (char)((value % 100) + 48);
                return 3;
            }
        }

        public unsafe static int ToString(byte[] buffer, int pos, byte value)
        {
            if (value < 10)
            {
                buffer[pos + 0] = (byte)((value % 10) + 48);
                return 1;
            }
            else if (value < 100)
            {
                buffer[pos + 0] = (byte)((value / 10) + 48);
                buffer[pos + 1] = (byte)((value % 10) + 48);
                return 2;
            }
            else
            {
                buffer[pos + 0] = (byte)((value / 10) + 48);
                buffer[pos + 1] = (byte)((value % 10) + 48);
                buffer[pos + 2] = (byte)((value % 100) + 48);
                return 3;
            }
        }

        public unsafe static int WriteCharsUInt(int value, char* buffer)
        {
            int q, r, size = 0;
            buffer += 9;
            while (value >= 65536)
            {
                q = value / 100;
                r = value - ((q << 6) + (q << 5) + (q << 2));
                value = q;
                *buffer-- = DigitOnes[r];
                *buffer-- = DigitTens[r];
                size += 2;
            }
            for (;;)
            {
                q = (int)((uint)(value * 52429) >> (16 + 3));
                r = value - ((q << 3) + (q << 1));
                *buffer-- = digits[r];
                value = q;
                size++;
                if (value == 0) break;
            }
            if (size < 10)
            {
                int skip = 9 - size;
                buffer -= skip;
                for (int n = 0; n < size; n++)
                    *buffer++ = *(buffer + skip);
            }
            return size;
        }

        public unsafe static int WriteChars(int value, char* buffer)
        {
            if (value < 0)
            {

            }
            else
            {

            }
            int q, r, size = 0;
            buffer += 9;
            while (value >= 65536)
            {
                q = value / 100;
                r = value - ((q << 6) + (q << 5) + (q << 2));
                value = q;
                *buffer-- = DigitOnes[r];
                *buffer-- = DigitTens[r];
                size += 2;
            }
            for (;;)
            {
                q = (int)((uint)(value * 52429) >> (16 + 3));
                r = value - ((q << 3) + (q << 1));
                *buffer-- = digits[r];
                value = q;
                size++;
                if (value == 0) break;
            }
            if (size < 10)
            {
                int skip = 9 - size;
                buffer -= skip;
                for (int n = 0; n < size; n++)
                    *buffer++ = *(buffer + skip);
            }
            return size;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, ulong value)
        {
            return buffer;
        }


        internal static int ToString(char[] buffer, int pos, int value)
        {
            return ToStringFast(buffer, pos, value);
        }

        internal unsafe static int ToString(char[] buffer, int pos, long value)
        {
            bool isMinus = false;
            if (value < 0)
            {
                if (value > -10)
                {
                    buffer[pos] = '-';
                    buffer[pos + 1] = (char)((-value % 10) + 48);
                    return 2;
                }
                else if (value > -100)
                {
                    buffer[pos] = '-';
                    buffer[pos + 1] = (char)((-value / 10) + 48);
                    buffer[pos + 2] = (char)((-value % 10) + 48);
                    return 3;
                }
                if (value == long.MinValue)
                {
                    fixed (char* smem = Int64Min, pd = &buffer[pos])
                    {
                        char* d = pd;
                        *((uint*)d) = *((uint*)smem);
                        *((uint*)(d + 2)) = *((uint*)(smem + 2));
                        *((uint*)(d + 4)) = *((uint*)(smem + 4));
                        *((uint*)(d + 6)) = *((uint*)(smem + 6));
                        *((uint*)(d + 8)) = *((uint*)(smem + 8));

                        *((uint*)(d + 10)) = *((uint*)(smem + 10));
                        *((uint*)(d + 12)) = *((uint*)(smem + 12));
                        *((uint*)(d + 14)) = *((uint*)(smem + 14));
                        *((uint*)(d + 16)) = *((uint*)(smem + 16));
                        *((uint*)(d + 18)) = *((uint*)(smem + 18));
                        *(d + 20) = *(smem + 20);
                        return 21;
                    }
                }
                isMinus = true;
                value = -value;
            }
            else if (value < 10)
            {
                buffer[pos] = (char)((value % 10) + 48);
                return 1;
            }
            else if (value < 100)
            {
                buffer[pos] = (char)((value / 10) + 48);
                buffer[pos + 1] = (char)((value % 10) + 48);
                return 2;
            }
            int size = 0;
            fixed (char* pd = &buffer[21 + pos])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    size++;
                } while (value != 0);
                if (isMinus == true)
                {
                    size++;
                    *d-- = '-';
                }
                int skip = 21 - size;
                d -= skip;
                for (int i = 0; i < size; i++)
                    *d++ = *(d + skip);
                return size;
            }
        }

        internal unsafe static int ToString(char[] buffer, int pos, uint value)
        {
            int size = 0;
            fixed (char* pd = &buffer[10 + pos])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    size++;
                } while (value != 0);
                int skip = 10 - size;
                d -= skip;
                for (int i = 0; i < size; i++)
                    *d++ = *(d + skip);
                return size;
            }
        }

        internal unsafe static int ToString(char[] buffer, int pos, ulong value)
        {
            int size = 0;
            fixed (char* pd = &buffer[21 + pos])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    size++;
                } while (value != 0);
                int skip = 21 - size;
                d -= skip;
                for (int i = 0; i < size; i++)
                    *d++ = *(d + skip);
                return size;
            }
        }

        
        
        
        public unsafe static int ToStringFast(char[] buffer, int pos, int value)
        {
            bool isMinus = false;
            if (value < 0)
            {
                if (value > -10)
                {
                    buffer[pos] = '-';
                    buffer[pos+1] = (char)((-value % 10) + 48);
                    return 2;
                }
                else if (value > -100)
                {
                    buffer[pos] = '-';
                    buffer[pos + 1] = (char)((-value / 10) + 48);
                    buffer[pos + 2] = (char)((-value % 10) + 48);
                    return 3;
                }
                if (value == int.MinValue)
                {
                    fixed (char* smem = Int32Min, pd = &buffer[pos])
                    {
                        char* d = pd;
                        *((uint*)d) = *((uint*)smem);
                        *((uint*)(d + 2)) = *((uint*)(smem + 2));
                        *((uint*)(d + 4)) = *((uint*)(smem + 4));
                        *((uint*)(d + 6)) = *((uint*)(smem + 6));
                        *((uint*)(d + 8)) = *((uint*)(smem + 8));
                        *(d + 10) = *(smem + 10);
                        return 11;
                    }
                }
                isMinus = true;
                value = -value;
            }
            else if (value < 10)
            {
                buffer[pos] = (char)((value % 10) + 48);
                return 1;
            }
            else if (value < 100)
            {
                buffer[pos] = (char)((value / 10) + 48);
                buffer[pos+1] = (char)((value % 10) + 48);
                return 2;
            }
            int size = 0;
            fixed (char* pd = &buffer[11 + pos])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    size++;
                } while (value != 0);
                if (isMinus == true)
                {
                    size++;
                    *d-- = '-';
                }
                int skip = 11 - size;
                d -= skip;
                for (int i = 0; i < size; i++)
                    *d++ = *(d + skip);
                return size;
            }

            #region old
            //fixed (char* pd = &buffer[ptr+pos])
            //{
            //    char* d = pd;
            //    do
            //    {
            //        var ix = value % 10;
            //        value /= 10;
            //        *d-- = (char)('0' + ix);
            //        ptr--;
            //    } while (value != 0);
            //    if (isMinus == true)
            //    {
            //        ptr--;
            //        *d-- = '-';
            //    }
            //    int count = 11 - ptr;
            //    d -= ptr;
            //    ptr++;
            //    for (int i = 0; i < count; i++)
            //        *(d + pos + i) = *(d + ptr + i);
            //    return count;
            //}
            #endregion
        }
        
        /// <summary>
        /// 将数组写入到字符缓冲区中，目前性能最好的
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public unsafe static int ToStringFast(char[] buffer, int pos, uint value)
        {
            #region old 1
            //int ptr = 11;
            //fixed (char* pd = &buffer[ptr + pos])
            //{
            //    char* d = pd;
            //    do
            //    {
            //        var ix = value % 10;
            //        value /= 10;
            //        *d-- = (char)('0' + ix);
            //        ptr--;
            //    } while (value != 0);
            //    int count = 11 - ptr;
            //    d -= ptr;
            //    ptr++;
            //    for (int i = 0; i < count; i++)
            //        *(d + pos + i) = *(d + ptr + i);
            //    pos += count;
            //}
            #endregion

            #region old 2
            //int size = 0;
            //fixed (char* pd = &buffer[11 + pos])
            //{
            //    char* d = pd;
            //    do
            //    {
            //        var ix = value % 10;
            //        value /= 10;
            //        *d-- = (char)('0' + ix);
            //        size++;
            //    } while (value != 0);
            //    int skip = 11 - size;
            //    d -= skip;
            //    for (int i = 0; i < size; i++)
            //        *d++ = *(d + skip);
            //    pos += size;
            //}
            #endregion
            
            int size = 0;
            fixed (char* pd = &buffer[11 + pos])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    size++;
                } while (value != 0);
                int skip = 11 - size;
                d -= skip;
                for (int i = 0; i < size; i++)
                    *d++ = *(d + skip);
                return size;
            }
        }

        public unsafe static int ToStringFast(char* buffer,int pos, uint value)
        {
            int size = 0;
            buffer += 11;
            do
            {
                var ix = value % 10;
                value /= 10;
                *buffer-- = (char)('0' + ix);
                size++;
            } while (value != 0);
            int skip = 11 - size;
            buffer -= skip;
            for (int i = 0; i < size; i++)
                *buffer++ = *(buffer + skip);
            return size;
            //pos += size;
            //return buffer;
        }

        public unsafe static int ToStringFast(char[] buffer, int pos, long value)
        {
            int ptr = 21;
            bool isMinus = false;
            if (value < 0)
            {
                if (value > -10)
                {
                    buffer[pos] = '-';
                    buffer[pos + 1] = (char)((-value % 10) + 48);
                    return 2;
                }
                else if (value > -100)
                {
                    buffer[pos] = '-';
                    buffer[pos + 1] = (char)((-value / 10) + 48);
                    buffer[pos + 2] = (char)((-value % 10) + 48);
                    return 3;
                }
                if (value == int.MinValue)
                {
                    fixed (char* smem = Int64Min, pd = &buffer[pos])
                    {
                        char* d = pd;
                        *((uint*)d) = *((uint*)smem);
                        *((uint*)(d + 2)) = *((uint*)(smem + 2));
                        *((uint*)(d + 4)) = *((uint*)(smem + 4));
                        *((uint*)(d + 6)) = *((uint*)(smem + 6));
                        *((uint*)(d + 8)) = *((uint*)(smem + 8));
                        *((uint*)(d + 10)) = *((uint*)(smem + 10));
                        *((uint*)(d + 12)) = *((uint*)(smem + 12));
                        *((uint*)(d + 14)) = *((uint*)(smem + 14));
                        *((uint*)(d + 16)) = *((uint*)(smem + 16));
                        *((uint*)(d + 18)) = *((uint*)(smem + 18));
                        *(d + 20) = *(smem + 20);
                        return 21;
                    }
                }
                isMinus = true;
                value = -value;
            }
            else if (value < 10)
            {
                buffer[pos] = (char)((value % 10) + 48);
                return 1;
            }
            else if (value < 100)
            {
                buffer[pos] = (char)((value / 10) + 48);
                buffer[pos + 1] = (char)((value % 10) + 48);
                return 2;
            }
            fixed (char* pd = &buffer[ptr])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    ptr--;
                } while (value != 0);
                if (isMinus == true)
                {
                    ptr--;
                    *d-- = '-';
                }
                int count = 21 - ptr;
                d -= ptr;
                ptr++;
                for (int i = 0; i < count; i++)
                    *(d + pos + i) = *(d + ptr + i);
                return count;
            }
        }

        public unsafe static int ToStringFast(char[] buffer, int pos, ulong value)
        {
            return 0;
            //int ptr = 21;
            //fixed (char* pd = &buffer[ptr])
            //{
            //    char* d = pd;
            //    do
            //    {
            //        var ix = value % 10;
            //        value /= 10;
            //        *d-- = (char)('0' + ix);
            //        ptr--;
            //    } while (value != 0);
            //    int count = 21 - ptr;
            //    d -= ptr;
            //    ptr++;
            //    for (int i = 0; i < count; i++)
            //        *(d + pos + i) = *(d + ptr + i);
            //    return count;
            //}
        }

        #endregion

        #region 时间

        internal unsafe static char* ToString(char* buffer, ref int pos, DateTime value)
        {
            //string s = value.ToString("o");
            //2013-07-09T17:13:22.5532438+08:00
            //TimeZoneInfo.Local.BaseUtcOffset
            int v = value.Year;
            *buffer++ = (char)((v / 1000) + 48);
            *buffer++ = (char)(((v % 1000) / 100) + 48);
            *buffer++ = (char)(((v % 100) / 10) + 48);
            *buffer++ = (char)((v % 10) + 48);

            v = value.Month;
            *buffer++ = '-';
            *buffer++ = (char)(((v % 100) / 10) + 48);
            *buffer++ = (char)((v % 10) + 48);

            v = value.Day;
            *buffer++ = '-';
            *buffer++ = (char)(((v % 100) / 10) + 48);
            *buffer++ = (char)((v % 10) + 48);

            v = value.Hour;
            *buffer++ = 'T';
            *buffer++ = (char)(((v % 100) / 10) + 48);
            *buffer++ = (char)((v % 10) + 48);

            v = value.Minute;
            *buffer++ = ':';
            *buffer++ = (char)(((v % 100) / 10) + 48);
            *buffer++ = (char)((v % 10) + 48);

            v = value.Second;
            *buffer++ = ':';
            *buffer++ = (char)(((v % 100) / 10) + 48);
            *buffer++ = (char)((v % 10) + 48);

            pos += 19;
            return buffer;

            //v = value.Millisecond;
            //*buffer++ = (char)('.');
            //*buffer++ = (char)((v / 100) + 48);
            //*buffer++ = (char)(((v % 100) / 10) + 48);
            //*buffer++ = (char)((v % 10) + 48);

            //*buffer++ = (char)('0');
            //*buffer++ = (char)('0');
            //*buffer++ = (char)('0');
            //*buffer++ = (char)('0');

            //TimeSpan span = TimeZoneInfo.Local.BaseUtcOffset;
            //v = span.Hours;
            //*buffer++ = (char)('+');
            //*buffer++ = (char)(((v % 100) / 10) + 48);
            //*buffer++ = (char)((v % 10) + 48);

            //v = span.Minutes;
            //*buffer++ = (char)(':');
            //*buffer++ = (char)(((v % 100) / 10) + 48);
            //*buffer++ = (char)((v % 10) + 48);

            //pos += 33;
            //return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, TimeSpan value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                *buffer++ = (char)v[i];
            pos += v.Length;
            return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, DateTimeOffset value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                *buffer++ = (char)v[i];
            pos += v.Length;
            return buffer;
        }

        /// <summary>
        /// ISO 8601日期格式，去掉异常判断，提升性能
        /// 时间转换暂时不能获取到微秒部分的数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static int ToString(char[] buffer, int pos, DateTime value)
        {
            //string s = value.ToString("o");
            //2013-07-09T17:13:22.5532438+08:00
            //TimeZoneInfo.Local.BaseUtcOffset
            int v = value.Year;
            buffer[pos] = (char)((v / 1000) + 48);
            buffer[pos + 1] = (char)(((v % 1000) / 100) + 48);
            buffer[pos + 2] = (char)(((v % 100) / 10) + 48);
            buffer[pos + 3] = (char)((v % 10) + 48);

            v = value.Month;
            buffer[pos + 4] = '-';
            buffer[pos + 5] = (char)(((v % 100) / 10) + 48);
            buffer[pos + 6] = (char)((v % 10) + 48);

            v = value.Day;
            buffer[pos + 7] = '-';
            buffer[pos + 8] = (char)(((v % 100) / 10) + 48);
            buffer[pos + 9] = (char)((v % 10) + 48);

            v = value.Hour;
            buffer[pos + 10] = 'T';
            buffer[pos + 11] = (char)(((v % 100) / 10) + 48);
            buffer[pos + 12] = (char)((v % 10) + 48);

            v = value.Minute;
            buffer[pos + 13] = ':';
            buffer[pos + 14] = (char)(((v % 100) / 10) + 48);
            buffer[pos + 15] = (char)((v % 10) + 48);

            v = value.Second;
            buffer[pos + 16] = (char)(':');
            buffer[pos + 17] = (char)(((v % 100) / 10) + 48);
            buffer[pos + 18] = (char)((v % 10) + 48);

            return 19;

            //v = value.Millisecond;
            //buffer[pos + 19] = (char)('.');
            //buffer[pos + 20] = (char)((v / 100) + 48);
            //buffer[pos + 21] = (char)(((v % 100) / 10) + 48);
            //buffer[pos + 22] = (char)((v % 10) + 48);

            //buffer[pos + 23] = (char)('0');
            //buffer[pos + 24] = (char)('0');
            //buffer[pos + 25] = (char)('0');
            //buffer[pos + 26] = (char)('0');

            //TimeSpan span = TimeZoneInfo.Local.BaseUtcOffset;
            //v = span.Hours;
            //buffer[pos + 27] = (char)('+');
            //buffer[pos + 28] = (char)(((v % 100) / 10) + 48);
            //buffer[pos + 29] = (char)((v % 10) + 48);

            //v = span.Minutes;
            //buffer[pos + 30] = (char)(':');
            //buffer[pos + 31] = (char)(((v % 100) / 10) + 48);
            //buffer[pos + 32] = (char)((v % 10) + 48);

            //return 33;
        }

        internal static int ToString(char[] buffer, int pos, TimeSpan value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                buffer[pos + i] = (char)v[i];
            return v.Length;
        }

        internal static int ToString(char[] buffer, int pos, DateTimeOffset value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                buffer[pos + i] = (char)v[i];
            return v.Length;
        }

        #endregion

        #region 其它基础类型

        internal unsafe static char* ToString(char* buffer, ref int pos, Guid value)
        {
            //Guid sp = new Guid("337c7f2b-7a34-4f50-9141-bab9e6478cc8");
            string v = value.ToString();
            fixed (char* pd = v)
            {
                Utils.FastCopyGuid(buffer, pd);
                buffer += 36;
            }
            pos += v.Length;
            return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, Uri value)
        {
            string v = value.AbsoluteUri;
            fixed (char* pd = v)
            {
                Utils.wstrcpy(buffer, pd, v.Length);
            }
            pos += v.Length;
            return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, Enum value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                *buffer++ = (char)v[i];
            pos += v.Length;
            return buffer;
        }

        public unsafe static void WriteGuid(Guid guid, char[] buffer)
        {
            // 1314FAD4-7505-439D-ABD2-DBD89242928C
            // 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
            //
            // Guid is guaranteed to be a 36 character string

            // get all the dashes in place
            buffer[8] = '-';
            buffer[13] = '-';
            buffer[18] = '-';
            buffer[23] = '-';

            // Bytes are in a different order than you might expect
            // For: 35 91 8b c9 - 19 6d - 40 ea  - 97 79  - 88 9d 79 b7 53 f0 
            // Get: C9 8B 91 35   6D 19   EA 40    97 79    88 9D 79 B7 53 F0 
            // Ix:   0  1  2  3    4  5    6  7     8  9    10 11 12 13 14 15
            //
            // And we have to account for dashes
            //
            // So the map is like so:
            // bytes[0]  -> chars[3]  -> buffer[ 6, 7]
            // bytes[1]  -> chars[2]  -> buffer[ 4, 5]
            // bytes[2]  -> chars[1]  -> buffer[ 2, 3]
            // bytes[3]  -> chars[0]  -> buffer[ 0, 1]
            // bytes[4]  -> chars[5]  -> buffer[11,12]
            // bytes[5]  -> chars[4]  -> buffer[ 9,10]
            // bytes[6]  -> chars[7]  -> buffer[16,17]
            // bytes[7]  -> chars[6]  -> buffer[14,15]
            // bytes[8]  -> chars[8]  -> buffer[19,20]
            // bytes[9]  -> chars[9]  -> buffer[21,22]
            // bytes[10] -> chars[10] -> buffer[24,25]
            // bytes[11] -> chars[11] -> buffer[26,27]
            // bytes[12] -> chars[12] -> buffer[28,29]
            // bytes[13] -> chars[13] -> buffer[30,31]
            // bytes[14] -> chars[14] -> buffer[32,33]
            // bytes[15] -> chars[15] -> buffer[34,35]
            var visibleMembers = new GuidStruct(guid);

            // bytes[0]
            var b = visibleMembers.B00 * 2;
            buffer[6] = WriteGuidLookup[b];
            buffer[7] = WriteGuidLookup[b + 1];

            // bytes[1]
            b = visibleMembers.B01 * 2;
            buffer[4] = WriteGuidLookup[b];
            buffer[5] = WriteGuidLookup[b + 1];

            // bytes[2]
            b = visibleMembers.B02 * 2;
            buffer[2] = WriteGuidLookup[b];
            buffer[3] = WriteGuidLookup[b + 1];

            // bytes[3]
            b = visibleMembers.B03 * 2;
            buffer[0] = WriteGuidLookup[b];
            buffer[1] = WriteGuidLookup[b + 1];

            // bytes[4]
            b = visibleMembers.B04 * 2;
            buffer[11] = WriteGuidLookup[b];
            buffer[12] = WriteGuidLookup[b + 1];

            // bytes[5]
            b = visibleMembers.B05 * 2;
            buffer[9] = WriteGuidLookup[b];
            buffer[10] = WriteGuidLookup[b + 1];

            // bytes[6]
            b = visibleMembers.B06 * 2;
            buffer[16] = WriteGuidLookup[b];
            buffer[17] = WriteGuidLookup[b + 1];

            // bytes[7]
            b = visibleMembers.B07 * 2;
            buffer[14] = WriteGuidLookup[b];
            buffer[15] = WriteGuidLookup[b + 1];

            // bytes[8]
            b = visibleMembers.B08 * 2;
            buffer[19] = WriteGuidLookup[b];
            buffer[20] = WriteGuidLookup[b + 1];

            // bytes[9]
            b = visibleMembers.B09 * 2;
            buffer[21] = WriteGuidLookup[b];
            buffer[22] = WriteGuidLookup[b + 1];

            // bytes[10]
            b = visibleMembers.B10 * 2;
            buffer[24] = WriteGuidLookup[b];
            buffer[25] = WriteGuidLookup[b + 1];

            // bytes[11]
            b = visibleMembers.B11 * 2;
            buffer[26] = WriteGuidLookup[b];
            buffer[27] = WriteGuidLookup[b + 1];

            // bytes[12]
            b = visibleMembers.B12 * 2;
            buffer[28] = WriteGuidLookup[b];
            buffer[29] = WriteGuidLookup[b + 1];

            // bytes[13]
            b = visibleMembers.B13 * 2;
            buffer[30] = WriteGuidLookup[b];
            buffer[31] = WriteGuidLookup[b + 1];

            // bytes[14]
            b = visibleMembers.B14 * 2;
            buffer[32] = WriteGuidLookup[b];
            buffer[33] = WriteGuidLookup[b + 1];

            // bytes[15]
            b = visibleMembers.B15 * 2;
            buffer[34] = WriteGuidLookup[b];
            buffer[35] = WriteGuidLookup[b + 1];

            //writer.Write(buffer, 0, 36);
        }

        public unsafe static void ToString(char[] buffer, int pos, Guid value)
        {
            fixed (char* ds = &buffer[pos])
            {
                char* pd = ds;
                var visibleMembers = new GuidStruct(value);

                // bytes[3]
                var b = visibleMembers.B03 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[2]
                b = visibleMembers.B02 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[1]
                b = visibleMembers.B01 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[0]
                b = visibleMembers.B00 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                *pd++ = '-';

                // bytes[5]
                b = visibleMembers.B05 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[4]
                b = visibleMembers.B04 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                *pd++ = '-';

                // bytes[7]
                b = visibleMembers.B07 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[6]
                b = visibleMembers.B06 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                *pd++ = '-';

                // bytes[8]
                b = visibleMembers.B08 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[9]
                b = visibleMembers.B09 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                *pd++ = '-';

                // bytes[10]
                b = visibleMembers.B10 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[11]
                b = visibleMembers.B11 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[12]
                b = visibleMembers.B12 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[13]
                b = visibleMembers.B13 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[14]
                b = visibleMembers.B14 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[15]
                b = visibleMembers.B15 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];
            }


            //fixed (char* ds = &buffer[pos])
            //{
            //    char* pd = ds;
            //    var visibleMembers = new GuidStruct(value);

            //    // bytes[3]
            //    var b = visibleMembers.B03 * 2;
            //    buffer[0] = WriteGuidLookup[b];
            //    buffer[1] = WriteGuidLookup[b + 1];

            //    // bytes[2]
            //    b = visibleMembers.B02 * 2;
            //    buffer[2] = WriteGuidLookup[b];
            //    buffer[3] = WriteGuidLookup[b + 1];

            //    // bytes[1]
            //    b = visibleMembers.B01 * 2;
            //    buffer[4] = WriteGuidLookup[b];
            //    buffer[5] = WriteGuidLookup[b + 1];

            //    // bytes[0]
            //    b = visibleMembers.B00 * 2;
            //    buffer[6] = WriteGuidLookup[b];
            //    buffer[7] = WriteGuidLookup[b + 1];

            //    buffer[8] = '-';

            //    // bytes[5]
            //    b = visibleMembers.B05 * 2;
            //    buffer[9] = WriteGuidLookup[b];
            //    buffer[10] = WriteGuidLookup[b + 1];

            //    // bytes[4]
            //    b = visibleMembers.B04 * 2;
            //    buffer[11] = WriteGuidLookup[b];
            //    buffer[12] = WriteGuidLookup[b + 1];

            //    buffer[13] = '-';

            //    // bytes[7]
            //    b = visibleMembers.B07 * 2;
            //    buffer[14] = WriteGuidLookup[b];
            //    buffer[15] = WriteGuidLookup[b + 1];

            //    // bytes[6]
            //    b = visibleMembers.B06 * 2;
            //    buffer[16] = WriteGuidLookup[b];
            //    buffer[17] = WriteGuidLookup[b + 1];

            //    buffer[18] = '-';

            //    // bytes[8]
            //    b = visibleMembers.B08 * 2;
            //    buffer[19] = WriteGuidLookup[b];
            //    buffer[20] = WriteGuidLookup[b + 1];

            //    // bytes[9]
            //    b = visibleMembers.B09 * 2;
            //    buffer[21] = WriteGuidLookup[b];
            //    buffer[22] = WriteGuidLookup[b + 1];

            //    buffer[23] = '-';

            //    // bytes[10]
            //    b = visibleMembers.B10 * 2;
            //    buffer[24] = WriteGuidLookup[b];
            //    buffer[25] = WriteGuidLookup[b + 1];

            //    // bytes[11]
            //    b = visibleMembers.B11 * 2;
            //    buffer[26] = WriteGuidLookup[b];
            //    buffer[27] = WriteGuidLookup[b + 1];

            //    // bytes[12]
            //    b = visibleMembers.B12 * 2;
            //    buffer[28] = WriteGuidLookup[b];
            //    buffer[29] = WriteGuidLookup[b + 1];

            //    // bytes[13]
            //    b = visibleMembers.B13 * 2;
            //    buffer[30] = WriteGuidLookup[b];
            //    buffer[31] = WriteGuidLookup[b + 1];

            //    // bytes[14]
            //    b = visibleMembers.B14 * 2;
            //    buffer[32] = WriteGuidLookup[b];
            //    buffer[33] = WriteGuidLookup[b + 1];

            //    // bytes[15]
            //    b = visibleMembers.B15 * 2;
            //    buffer[34] = WriteGuidLookup[b];
            //    buffer[35] = WriteGuidLookup[b + 1];
            //}
        }

        internal unsafe static int ToString(char[] buffer, int pos, Uri value)
        {
            string v = value.AbsoluteUri;
            fixed (char* pd = v,dst=&buffer[pos])
            {
                Utils.wstrcpy(dst, pd, v.Length);
            }
            return v.Length;
        }

        internal static int ToString(char[] buffer, int pos, Enum value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                buffer[pos + i] = (char)v[i];
            return v.Length;
        }

        internal unsafe static int ToString(char[] buffer, int pos, byte[] value)
        {
            fixed (char* pdst = &buffer[pos])
            {
                fixed (byte* psrc = value)
                {
                    Utils.ConvertToBase64Array(pdst, psrc, 0, value.Length, false);
                }
            }
            return (value.Length << 1);
        }

        #endregion

        #region 纯ToString

        public unsafe static int ToStringSimple(char[] buffer, int pos, uint value)
        {
            int ptr = 11;
            fixed (char* pd = &buffer[ptr])
            {
                char* d = pd;
                do
                {
                    byte ix = (byte)(value % 100);
                    value /= 100;
                    var chars = DigitPairs[ix];
                    *d-- = chars.Second;
                    *d-- = chars.First;
                    ptr -= 2;
                } while (value != 0);
                if (*(d + 1) == '0')
                    ptr++;
                int count = 11 - ptr;
                d -= (ptr - 1);
                ptr++;
                for (int i = 0; i < count; i++)
                    *(d + pos + i) = *(d + ptr + i);
                return count;
            }
        }

        public unsafe static string ToString(Guid value)
        {
            char[] buffer = new char[36];
            ToString(buffer, 0, value);
            return new string(buffer);
        }

        #endregion

        #region 其它方法

        // Requires positive x
        public static int StringSize(int value)
        {
            int n = 1;
            if (value < 0)
            {
                value = -value;
                n += 1;
            }
            for (int i = 0; ; i++)
                if (value <= intSizeTable[i])
                    return i + n;
        }

        public static int StringSize(uint x)
        {
            for (int i = 0; ; i++)
            {
                if (x <= uintSizeTable[i])
                    return i + 1;
            }
        }

        #endregion
    }

    

}
