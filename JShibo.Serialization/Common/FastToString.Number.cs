using JShibo.Serialization.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Common
{
    public unsafe static partial class FastToString
    {
        
        #region 数字字符串转换

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
                *(buffer + 1) = (char)((value % 10) + 48);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int ToStringNumber(byte* buffer, byte value)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int ToString(char[] buffer, int pos, byte value)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void ToStringSign(char* buffer, int value, ref int pos)
        {
            if (value < 10)
            {
                *buffer = (char)(value + (char)'0');
                *(buffer + 1) = ',';
                pos += 2;
            }
            else if (value < 100)
            {
                var tens = (char)((value * 205u) >> 11); // div10, valid to 1028
                *buffer = (char)(tens + (char)'0');
                *(buffer + 1) = (char)(value - (tens * 10) + (char)'0');
                *(buffer + 2) = ',';
                pos += 3;
            }
            else
            {
                var digit0 = (char)((value * 41u) >> 12); // div100, valid to 1098
                var digits01 = (char)((value * 205u) >> 11); // div10, valid to 1028
                *buffer = (char)(digit0 + (char)'0');
                *(buffer + 1) = (char)(digits01 - (digit0 * 10) + (char)'0');
                *(buffer + 2) = (char)(value - (digits01 * 10) + (char)'0');
                *(buffer + 3) = ',';
                pos += 4;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void ToStringSign(byte* buffer, int value, ref int pos)
        {
            if (value < 10)
            {
                *buffer = (byte)(value + '0');
                *(buffer + 1) = (byte)',';
                pos += 2;
            }
            else if (value < 100)
            {
                var tens = (byte)((value * 205u) >> 11); // div10, valid to 1028
                *buffer = (byte)(tens + '0');
                *(buffer + 1) = (byte)(value - (tens * 10) + '0');
                *(buffer + 2) = (byte)',';
                pos += 3;
            }
            else
            {
                var digit0 = (char)((value * 41u) >> 12); // div100, valid to 1098
                var digits01 = (char)((value * 205u) >> 11); // div10, valid to 1028
                *buffer = (byte)(digit0 + '0');
                *(buffer + 1) = (byte)(digits01 - (digit0 * 10) + '0');
                *(buffer + 2) = (byte)(value - (digits01 * 10) + '0');
                *(buffer + 3) = (byte)',';
                pos += 4;
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
            for (; ; )
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            for (; ; )
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe static char* ToString(char* buffer, ref int pos, ulong value)
        {
            return buffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int ToString(char[] buffer, int pos, int value)
        {
            return ToStringFast(buffer, pos, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int ToString(byte[] buffer, int pos, int value)
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



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int ToStringFast(char[] buffer, int pos, int value)
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
                buffer[pos + 1] = (char)((value % 10) + 48);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int ToStringFast(byte[] buffer, int pos, int value)
        {
            bool isMinus = false;
            if (value < 0)
            {
                if (value > -10)
                {
                    buffer[pos] = (byte)'-';
                    buffer[pos + 1] = (byte)((-value % 10) + 48);
                    return 2;
                }
                else if (value > -100)
                {
                    buffer[pos] = (byte)'-';
                    buffer[pos + 1] = (byte)((-value / 10) + 48);
                    buffer[pos + 2] = (byte)((-value % 10) + 48);
                    return 3;
                }
                if (value == int.MinValue)
                {
                    fixed (byte* smem = Int32MinBytes, pd = &buffer[pos])
                    {
                        byte* d = pd;
                        *((ulong*)d) = *((ulong*)smem);
                        *((ushort*)(d + 8)) = *((ushort*)(smem + 8));
                        *(d + 10) = *(smem + 10);
                        return 11;
                    }
                }
                isMinus = true;
                value = -value;
            }
            else if (value < 10)
            {
                buffer[pos] = (byte)((value % 10) + 48);
                return 1;
            }
            else if (value < 100)
            {
                buffer[pos] = (byte)((value / 10) + 48);
                buffer[pos + 1] = (byte)((value % 10) + 48);
                return 2;
            }
            int size = 0;
            fixed (byte* pd = &buffer[11 + pos])
            {
                byte* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (byte)('0' + ix);
                    size++;
                } while (value != 0);
                if (isMinus == true)
                {
                    size++;
                    *d-- = (byte)'-';
                }
                int skip = 11 - size;
                d -= skip;
                for (int i = 0; i < size; i++)
                    *d++ = *(d + skip);
                return size;
            }
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

        public unsafe static int ToStringFast(char* buffer, int pos, uint value)
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
    }
}
