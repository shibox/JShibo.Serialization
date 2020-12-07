
using System.Runtime.CompilerServices;

namespace JShibo.Serialization.Common
{
    public class FastWriteName
    {
        /// <summary>
        /// 没有发现明显的性能提升，似乎还下降了
        /// </summary>
        /// <param name="name"></param>
        /// <param name="buffer"></param>
        /// <param name="position"></param>
        public unsafe static void Write(string name, byte[] buffer, int position)
        {
            switch (name.Length)
            {
                case 1:
                    buffer[position] = (byte)name[0];
                    break;
                case 2:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    break;
                case 3:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    break;
                case 4:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    buffer[position + 3] = (byte)name[3];
                    break;
                case 5:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    buffer[position + 3] = (byte)name[3];
                    buffer[position + 4] = (byte)name[4];
                    break;
                case 6:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    buffer[position + 3] = (byte)name[3];
                    buffer[position + 4] = (byte)name[4];
                    buffer[position + 5] = (byte)name[5];
                    break;
                case 7:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    buffer[position + 3] = (byte)name[3];
                    buffer[position + 4] = (byte)name[4];
                    buffer[position + 5] = (byte)name[5];
                    buffer[position + 6] = (byte)name[6];
                    break;
                case 8:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    buffer[position + 3] = (byte)name[3];
                    buffer[position + 4] = (byte)name[4];
                    buffer[position + 5] = (byte)name[5];
                    buffer[position + 6] = (byte)name[6];
                    buffer[position + 7] = (byte)name[7];
                    break;
                case 9:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    buffer[position + 3] = (byte)name[3];
                    buffer[position + 4] = (byte)name[4];
                    buffer[position + 5] = (byte)name[5];
                    buffer[position + 6] = (byte)name[6];
                    buffer[position + 7] = (byte)name[7];
                    buffer[position + 8] = (byte)name[8];
                    break;
                case 10:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    buffer[position + 3] = (byte)name[3];
                    buffer[position + 4] = (byte)name[4];
                    buffer[position + 5] = (byte)name[5];
                    buffer[position + 6] = (byte)name[6];
                    buffer[position + 7] = (byte)name[7];
                    buffer[position + 8] = (byte)name[8];
                    buffer[position + 9] = (byte)name[9];
                    break;
                case 11:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    buffer[position + 3] = (byte)name[3];
                    buffer[position + 4] = (byte)name[4];
                    buffer[position + 5] = (byte)name[5];
                    buffer[position + 6] = (byte)name[6];
                    buffer[position + 7] = (byte)name[7];
                    buffer[position + 8] = (byte)name[8];
                    buffer[position + 9] = (byte)name[9];
                    buffer[position + 10] = (byte)name[10];
                    break;
                case 12:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    buffer[position + 3] = (byte)name[3];
                    buffer[position + 4] = (byte)name[4];
                    buffer[position + 5] = (byte)name[5];
                    buffer[position + 6] = (byte)name[6];
                    buffer[position + 7] = (byte)name[7];
                    buffer[position + 8] = (byte)name[8];
                    buffer[position + 9] = (byte)name[9];
                    buffer[position + 10] = (byte)name[10];
                    buffer[position + 11] = (byte)name[11];
                    break;
                case 13:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    buffer[position + 3] = (byte)name[3];
                    buffer[position + 4] = (byte)name[4];
                    buffer[position + 5] = (byte)name[5];
                    buffer[position + 6] = (byte)name[6];
                    buffer[position + 7] = (byte)name[7];
                    buffer[position + 8] = (byte)name[8];
                    buffer[position + 9] = (byte)name[9];
                    buffer[position + 10] = (byte)name[10];
                    buffer[position + 11] = (byte)name[11];
                    buffer[position + 12] = (byte)name[12];
                    break;
                case 14:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    buffer[position + 3] = (byte)name[3];
                    buffer[position + 4] = (byte)name[4];
                    buffer[position + 5] = (byte)name[5];
                    buffer[position + 6] = (byte)name[6];
                    buffer[position + 7] = (byte)name[7];
                    buffer[position + 8] = (byte)name[8];
                    buffer[position + 9] = (byte)name[9];
                    buffer[position + 10] = (byte)name[10];
                    buffer[position + 11] = (byte)name[11];
                    buffer[position + 12] = (byte)name[12];
                    buffer[position + 13] = (byte)name[13];
                    break;
                case 15:
                    buffer[position] = (byte)name[0];
                    buffer[position + 1] = (byte)name[1];
                    buffer[position + 2] = (byte)name[2];
                    buffer[position + 3] = (byte)name[3];
                    buffer[position + 4] = (byte)name[4];
                    buffer[position + 5] = (byte)name[5];
                    buffer[position + 6] = (byte)name[6];
                    buffer[position + 7] = (byte)name[7];
                    buffer[position + 8] = (byte)name[8];
                    buffer[position + 9] = (byte)name[9];
                    buffer[position + 10] = (byte)name[10];
                    buffer[position + 11] = (byte)name[11];
                    buffer[position + 12] = (byte)name[12];
                    buffer[position + 13] = (byte)name[13];
                    buffer[position + 14] = (byte)name[14];
                    break;
                default:
                    for (int i = 0; i < name.Length; i++)
                        buffer[position + i] = (byte)name[i];
                    break;
            }
        }

        public unsafe static void WriteUnsafe(string name, char[] buffer, int position)
        {
            fixed (char* src = name, dst = &buffer[position])
            {
                char* tsrc = src, tdst = dst;
                WriteUnsafe(tsrc, tdst, name.Length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static void WriteUnsafe(char* tsrc, char* tdst, int size)
        {
            switch (size)
            {
                case 1:
                    *tdst = *tsrc;
                    break;
                case 2:
                    *((uint*)tdst) = *((uint*)tsrc);
                    break;
                case 3:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *(tdst + 2) = *(tsrc + 2);
                    break;
                case 4:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    break;
                case 5:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *(tdst + 4) = *(tsrc + 4);
                    break;
                case 6:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    break;
                case 7:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *(tdst + 6) = *(tsrc + 6);
                    break;
                case 8:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    break;
                case 9:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *(tdst + 8) = *(tsrc + 8);
                    break;
                case 10:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    break;
                case 11:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *(tdst + 10) = *(tsrc + 10);
                    break;
                case 12:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    break;
                case 13:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *(tdst + 12) = *(tsrc + 12);
                    break;
                case 14:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    break;
                case 15:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *(tdst + 14) = *(tsrc + 14);
                    break;
                case 16:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    break;
                case 17:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *(tdst + 16) = *(tsrc + 16);
                    break;
                case 18:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    break;
                case 19:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *(tdst + 18) = *(tsrc + 18);
                    break;
                case 20:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    break;
                case 21:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *(tdst + 20) = *(tsrc + 20);
                    break;
                case 22:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    break;
                case 23:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *(tdst + 22) = *(tsrc + 22);
                    break;
                case 24:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    break;
                case 25:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    *(tdst + 24) = *(tsrc + 24);
                    break;
                case 26:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    *((uint*)(tdst + 24)) = *((uint*)(tsrc + 24));
                    break;
                case 27:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    *((uint*)(tdst + 24)) = *((uint*)(tsrc + 24));
                    *(tdst + 26) = *(tsrc + 26);
                    break;
                case 28:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    *((uint*)(tdst + 24)) = *((uint*)(tsrc + 24));
                    *((uint*)(tdst + 26)) = *((uint*)(tsrc + 26));
                    break;
                case 29:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    *((uint*)(tdst + 24)) = *((uint*)(tsrc + 24));
                    *((uint*)(tdst + 26)) = *((uint*)(tsrc + 26));
                    *(tdst + 28) = *(tsrc + 28);
                    break;
                case 30:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    *((uint*)(tdst + 24)) = *((uint*)(tsrc + 24));
                    *((uint*)(tdst + 26)) = *((uint*)(tsrc + 26));
                    *((uint*)(tdst + 28)) = *((uint*)(tsrc + 28));
                    break;
                case 31:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    *((uint*)(tdst + 24)) = *((uint*)(tsrc + 24));
                    *((uint*)(tdst + 26)) = *((uint*)(tsrc + 26));
                    *((uint*)(tdst + 28)) = *((uint*)(tsrc + 28));
                    *(tdst + 30) = *(tsrc + 30);
                    break;
                case 32:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 2)) = *((uint*)(tsrc + 2));
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 6)) = *((uint*)(tsrc + 6));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 10)) = *((uint*)(tsrc + 10));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    *((uint*)(tdst + 24)) = *((uint*)(tsrc + 24));
                    *((uint*)(tdst + 26)) = *((uint*)(tsrc + 26));
                    *((uint*)(tdst + 28)) = *((uint*)(tsrc + 28));
                    *((uint*)(tdst + 30)) = *((uint*)(tsrc + 30));
                    break;

                #region other










                #endregion

                default:
                    Utils.wstrcpy(tdst, tsrc, size);
                    break;
            }
        }

        public unsafe static void WriteUnsafe(byte[] value, byte[] buffer, int position)
        {
            fixed (byte* src = value, dst = &buffer[position])
            {
                byte* tsrc = src, tdst = dst;
                WriteUnsafe(tsrc, tdst, value.Length);
            }
        }

        public unsafe static void WriteUnsafe(byte* tsrc, byte* tdst, int size)
        {
            switch (size)
            {
                case 1:
                    *tdst = *tsrc;
                    break;
                case 2:
                    *((ushort*)tdst) = *((ushort*)tsrc);
                    break;
                case 3:
                    *((ushort*)tdst) = *((ushort*)tsrc);
                    *(tdst + 2) = *(tsrc + 2);
                    break;
                case 4:
                    *((uint*)tdst) = *((uint*)tsrc);
                    break;
                case 5:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *(tdst + 4) = *(tsrc + 4);
                    break;
                case 6:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((ushort*)(tdst + 4)) = *((ushort*)(tsrc + 4));
                    break;
                case 7:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((ushort*)(tdst + 4)) = *((ushort*)(tsrc + 4));
                    *(tdst + 6) = *(tsrc + 6);
                    break;
                case 8:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    break;
                case 9:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *(tdst + 8) = *(tsrc + 8);
                    break;
                case 10:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((ushort*)(tdst + 8)) = *((ushort*)(tsrc + 8));
                    break;
                case 11:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((ushort*)(tdst + 8)) = *((ushort*)(tsrc + 8));
                    *(tdst + 10) = *(tsrc + 10);
                    break;
                case 12:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    break;
                case 13:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *(tdst + 12) = *(tsrc + 12);
                    break;
                case 14:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((ushort*)(tdst + 12)) = *((ushort*)(tsrc + 12));
                    break;
                case 15:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((ushort*)(tdst + 12)) = *((ushort*)(tsrc + 12));
                    *(tdst + 14) = *(tsrc + 14);
                    break;
                case 16:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    break;
                case 17:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *(tdst + 16) = *(tsrc + 16);
                    break;
                case 18:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((ushort*)(tdst + 16)) = *((ushort*)(tsrc + 16));
                    break;
                case 19:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((ushort*)(tdst + 16)) = *((ushort*)(tsrc + 16));
                    *(tdst + 18) = *(tsrc + 18);
                    break;
                case 20:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    break;
                case 21:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *(tdst + 20) = *(tsrc + 20);
                    break;
                case 22:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((ushort*)(tdst + 20)) = *((ushort*)(tsrc + 20));
                    break;
                case 23:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((ushort*)(tdst + 20)) = *((ushort*)(tsrc + 20));
                    *(tdst + 22) = *(tsrc + 22);
                    break;
                case 24:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    break;
                case 25:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *(tdst + 24) = *(tsrc + 24);
                    break;
                case 26:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    *((ushort*)(tdst + 24)) = *((ushort*)(tsrc + 24));
                    break;
                case 27:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *((ushort*)(tdst + 24)) = *((ushort*)(tsrc + 24));
                    *(tdst + 26) = *(tsrc + 26);
                    break;
                case 28:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    *((uint*)(tdst + 26)) = *((uint*)(tsrc + 26));
                    break;
                case 29:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *((uint*)(tdst + 24)) = *((uint*)(tsrc + 24));
                    *(tdst + 28) = *(tsrc + 28);
                    break;
                case 30:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    *((uint*)(tdst + 26)) = *((uint*)(tsrc + 26));
                    *((ushort*)(tdst + 28)) = *((ushort*)(tsrc + 28));
                    break;
                case 31:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 16)) = *((uint*)(tsrc + 16));
                    *((uint*)(tdst + 20)) = *((uint*)(tsrc + 20));
                    *((uint*)(tdst + 24)) = *((uint*)(tsrc + 24));
                    *((ushort*)(tdst + 28)) = *((ushort*)(tsrc + 28));
                    *(tdst + 30) = *(tsrc + 30);
                    break;
                case 32:
                    *((uint*)tdst) = *((uint*)tsrc);
                    *((uint*)(tdst + 4)) = *((uint*)(tsrc + 4));
                    *((uint*)(tdst + 8)) = *((uint*)(tsrc + 8));
                    *((uint*)(tdst + 12)) = *((uint*)(tsrc + 12));
                    *((uint*)(tdst + 14)) = *((uint*)(tsrc + 14));
                    *((uint*)(tdst + 18)) = *((uint*)(tsrc + 18));
                    *((uint*)(tdst + 22)) = *((uint*)(tsrc + 22));
                    *((uint*)(tdst + 26)) = *((uint*)(tsrc + 26));
                    *((uint*)(tdst + 30)) = *((uint*)(tsrc + 30));
                    break;
                default:
                    Utils.wstrcpy(tdst, tsrc, size);
                    break;
            }
        }



    }
}
