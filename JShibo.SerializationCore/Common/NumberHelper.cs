using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.SerializationCore.Common
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



    }
}
