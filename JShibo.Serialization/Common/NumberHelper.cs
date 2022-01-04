using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Common
{
    public unsafe static class NumberHelper
    {
        [StructLayout(LayoutKind.Sequential, Size = sizeof(char) * 3)]
        internal struct ThreeChar
        {
            public char v1;
            public char v2;
            public char v3;
        }

        [StructLayout(LayoutKind.Sequential, Size = sizeof(byte) * 3)]
        internal struct ThreeByte
        {
            public byte v1;
            public byte v2;
            public byte v3;
        }

        [StructLayout(LayoutKind.Sequential, Size = sizeof(char) * 4)]
        internal struct FourChar
        {
            public char v1;
            public char v2;
            public char v3;
            public char v4;
        }

        private static readonly ThreeChar[] threeChar;
        private static readonly ThreeByte[] threeByte;
        private static readonly ThreeChar* threeChars;
        private static readonly ThreeByte* threeBytes;

        static NumberHelper()
        {
            var size = 1000;
            threeChars = (ThreeChar*)Marshal.AllocHGlobal(size * sizeof(ThreeChar));
            threeBytes = (ThreeByte*)Marshal.AllocHGlobal(size * sizeof(ThreeByte));
            for (uint i = 100; i < size; i++)
            {
                var chs = i.ToString();
                threeChars[i].v1 = chs[0];
                threeChars[i].v2 = chs[1];
                threeChars[i].v3 = chs[2];

                threeBytes[i].v1 = (byte)chs[0];
                threeBytes[i].v2 = (byte)chs[1];
                threeBytes[i].v3 = (byte)chs[2];
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
                var tens = (byte)((value * 205u) >> 11);
                *buffer = (byte)(tens + 48);
                *(buffer + 1) = (byte)(value - (tens * 10) + 48);
                return 2;
            }
            else
            {
                *(uint*)buffer = *(uint*)(threeBytes + value);
                return 3;
            }
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
                var tens = (char)((value * 205u) >> 11);
                *buffer = (char)(tens + '0');
                *(buffer + 1) = (char)(value - (tens * 10) + '0');
                return 2;
            }
            else
            {
                *(ulong*)buffer = *(ulong*)(threeChars + value);
                return 3;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static int ToString(int value, char* chars)
        {
            if (value >= 0)
                return ToDecimalString((uint)(value), chars);
            else
            {
                *chars = '-';
                return ToDecimalString((uint)(-value), chars + 1) + 1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToDecimalString(uint value, char* chars)
        {
            if (value >= 100000)
            {
                if (value >= 10000000)
                    if (value >= 1000000000) goto L10;
                    else if (value >= 100000000) goto L9;
                    else goto L8;
                else if (value >= 1000000) goto L7;
                else goto L6;
            }
            else if (value >= 100)
                if (value >= 10000) goto L5;
                else if (value >= 1000) goto L4;
                else goto L3;
            else if (value >= 10) goto L2;
            else goto L1;

        L10:
            uint s = value / 1000000000;
            DecimalAppendD1(chars, s);
            DecimalAppendD9(chars + 1, value - s * 1000000000);
            return 10;

        L9:
            DecimalAppendD9(chars, value);
            return 9;

        L8:
            s = value / 1000000;
            DecimalAppendD2(chars, s);
            DecimalAppendD6(chars + 2, value - s * 1000000);
            return 8;

        L7:
            s = value / 1000000;
            DecimalAppendD1(chars, s);
            DecimalAppendD6(chars + 1, value - s * 1000000);
            return 7;

        L6:
            DecimalAppendD6(chars, value);
            return 6;

        L5:
            s = value / 1000;
            DecimalAppendD2(chars, s);
            DecimalAppendD3(chars + 2, value - s * 1000);
            return 5;

        L4:
            s = value / 1000;
            DecimalAppendD1(chars, s);
            DecimalAppendD3(chars + 1, value - s * 1000);
            return 4;

        L3:
            DecimalAppendD3(chars, value);
            return 3;

        L2:
            DecimalAppendD2(chars, value);
            return 2;

        L1:
            DecimalAppendD1(chars, value);
            return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToDecimalString(ulong value, char* chars)
        {
            if (value >= 100000)
            {
                if (value >= 10000000000)
                    if (value >= 1000000000000000)
                        if (value >= 100000000000000000)
                            if (value >= 10000000000000000000) goto L20;
                            else if (value >= 1000000000000000000) goto L19;
                            else goto L18;
                        else if (value >= 10000000000000000) goto L17;
                        else goto L16;
                    else if (value >= 1000000000000)
                        if (value >= 100000000000000) goto L15;
                        else if (value >= 10000000000000) goto L14;
                        else goto L13;
                    else if (value >= 100000000000) goto L12;
                    else goto L11;
                else if (value >= 10000000)
                    if (value >= 1000000000) goto L10;
                    else if (value >= 100000000) goto L9;
                    else goto L8;
                else if (value >= 1000000) goto L7;
                else goto L6;
            }
            else if (value >= 100)
                if (value >= 10000) goto L5;
                else if (value >= 1000) goto L4;
                else goto L3;
            else if (value >= 10) goto L2;
            else goto L1;

            L20:
            ulong s = value / 1000000000000000000;
            DecimalAppendD2(chars, s);
            DecimalAppendD18(chars + 2, value - s * 1000000000000000000);
            return 20;

        L19:
            s = value / 1000000000000000000;
            DecimalAppendD1(chars, s);
            DecimalAppendD18(chars + 1, value - s * 1000000000000000000);
            return 19;

        L18:
            DecimalAppendD18(chars, value);
            return 18;

        L17:
            s = value / 1000000000000000;
            DecimalAppendD2(chars, s);
            DecimalAppendD15(chars + 2, value - s * 1000000000000000);
            return 17;

        L16:
            s = value / 1000000000000000;
            DecimalAppendD1(chars, s);
            DecimalAppendD15(chars + 1, value - s * 1000000000000000);
            return 16;

        L15:
            DecimalAppendD15(chars, value);
            return 15;

        L14:
            s = value / 1000000000000;
            DecimalAppendD2(chars, s);
            DecimalAppendD12(chars + 2, value - s * 1000000000000);
            return 14;

        L13:
            s = value / 1000000000000;
            DecimalAppendD1(chars, s);
            DecimalAppendD12(chars + 1, value - s * 1000000000000);
            return 13;

        L12:
            DecimalAppendD12(chars, value);
            return 12;

        L11:
            s = value / 1000000000;
            DecimalAppendD2(chars, s);
            DecimalAppendD9(chars + 2, value - s * 1000000000);
            return 11;

        L10:
            s = value / 1000000000;
            DecimalAppendD1(chars, s);
            DecimalAppendD9(chars + 1, value - s * 1000000000);
            return 10;

        L9:
            DecimalAppendD9(chars, value);
            return 9;

        L8:
            s = value / 1000000;
            DecimalAppendD2(chars, s);
            DecimalAppendD6(chars + 2, value - s * 1000000);
            return 8;

        L7:
            s = value / 1000000;
            DecimalAppendD1(chars, s);
            DecimalAppendD6(chars + 1, value - s * 1000000);
            return 7;

        L6:
            DecimalAppendD6(chars, value);
            return 6;

        L5:
            s = value / 1000;
            DecimalAppendD2(chars, s);
            DecimalAppendD3(chars + 2, value - s * 1000);
            return 5;

        L4:
            s = value / 1000;
            DecimalAppendD1(chars, s);
            DecimalAppendD3(chars + 1, value - s * 1000);
            return 4;

        L3:
            DecimalAppendD3(chars, value);
            return 3;

        L2:
            DecimalAppendD2(chars, value);
            return 2;

        L1:
            DecimalAppendD1(chars, value);
            return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void DecimalAppendD1(char* chars, ulong value)
        {
            *chars = (char)(((byte)value) + '0');
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void DecimalAppendD2(char* chars, ulong value)
        {
            chars[0] = (char)(((byte)value) / 10 + '0');
            chars[1] = (char)(((byte)value) % 10 + '0');
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void DecimalAppendD3(char* chars, ulong value)
        {
            var digitals = threeChars;

            *(long*)chars = *(long*)(digitals + value);
            // *(ThreeChar*)chars = digitals[value];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void DecimalAppendD6(char* chars, ulong value)
        {
            var digitals = threeChars;

            var v = value;

            var a = v / 1000;

            v -= a * 1000;

            *(long*)(chars + 0) = *(long*)(digitals + a);
            *(long*)(chars + 3) = *(long*)(digitals + v);
            //*((ThreeChar*)chars) = digitals[a];
            //*((ThreeChar*)(chars + 3)) = digitals[v - a * 1000];

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void DecimalAppendD9(char* chars, ulong value)
        {
            var digitals = threeChars;
            var v = value;
            var b = v / 1000;
            var a = b / 1000;
            v -= b * 1000;
            b -= a * 1000;
            *(long*)(chars + 0) = *(long*)(digitals + a);
            *(long*)(chars + 3) = *(long*)(digitals + b);
            *(long*)(chars + 6) = *(long*)(digitals + v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void DecimalAppendD12(char* chars, ulong value)
        {
            var digitals = threeChars;

            var v = value;

            var c = v / 1000;
            var b = c / 1000;
            var a = b / 1000;

            v -= c * 1000;
            c -= b * 1000;
            b -= a * 1000;

            *(long*)(chars + 0) = *(long*)(digitals + a);
            *(long*)(chars + 3) = *(long*)(digitals + b);
            *(long*)(chars + 6) = *(long*)(digitals + c);
            *(long*)(chars + 9) = *(long*)(digitals + v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void DecimalAppendD15(char* chars, ulong value)
        {
            var digitals = threeChars;

            var v = value;
            var d = v / 1000;
            var c = d / 1000;
            var b = c / 1000;
            var a = b / 1000;

            v -= d * 1000;
            d -= c * 1000;
            c -= b * 1000;
            b -= a * 1000;

            *(long*)(chars + 0) = *(long*)(digitals + a);
            *(long*)(chars + 3) = *(long*)(digitals + b);
            *(long*)(chars + 6) = *(long*)(digitals + c);
            *(long*)(chars + 9) = *(long*)(digitals + d);
            *(long*)(chars + 12) = *(long*)(digitals + v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void DecimalAppendD18(char* chars, ulong value)
        {
            var digitals = threeChars;

            var v = value;

            var e = v / 1000;
            var d = e / 1000;
            var c = d / 1000;
            var b = c / 1000;
            var a = b / 1000;

            v -= e * 1000;
            e -= d * 1000;
            d -= c * 1000;
            c -= b * 1000;
            b -= a * 1000;

            *(long*)(chars + 0) = *(long*)(digitals + a);
            *(long*)(chars + 3) = *(long*)(digitals + b);
            *(long*)(chars + 6) = *(long*)(digitals + c);
            *(long*)(chars + 9) = *(long*)(digitals + d);
            *(long*)(chars + 12) = *(long*)(digitals + e);
            *(long*)(chars + 15) = *(long*)(digitals + v);
        }

    }
}
