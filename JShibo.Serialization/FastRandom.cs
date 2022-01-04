//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JShibo.Serialization
//{
//    public sealed class FastRandom
//    {
//        #region 字段

//        /// <summary>
//        /// 字符
//        /// </summary>
//        public static string Letter = "abcdefghijklmnopqrstuvwxyz_ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";


//        const double REAL_UNIT_INT = 1.0 / ((double)int.MaxValue + 1.0);
//        const double REAL_UNIT_UINT = 1.0 / ((double)uint.MaxValue + 1.0);
//        const uint Y = 842502087, Z = 3579807591, W = 273326509;
//        static int rotateSeed = 0;
//        static FastRandom Instance = new FastRandom();
//        static object locker = new object();

//        uint x, y, z, w;
//        uint bitBuffer;
//        uint bitMask = 1;

//        #endregion

//        #region 构造函数

//        /// <summary>
//        /// 初始化种子为系统启动后经过的毫秒数。
//        /// </summary>
//        public FastRandom()
//            : this(Environment.TickCount)
//        {
//        }

//        /// <summary>
//        /// 随机数的种子
//        /// </summary>
//        /// <param name="seed"></param>
//        public FastRandom(int seed)
//        {
//            x = (uint)seed;
//            y = Y;
//            z = Z;
//            w = W;
//        }

//        #endregion

//        #region 内部用的

//        internal int InternalNext(int maxValue)
//        {
//            //if (maxValue < 0)
//            //    throw new ArgumentOutOfRangeException("maxValue", maxValue, "maxValue must be >=0");

//            uint t = (x ^ (x << 11));
//            x = y; y = z; z = w;
//            return (int)((REAL_UNIT_INT * (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))))) * maxValue);
//        }

//        internal int InternalNext(int minValue, int maxValue)
//        {
//            //if (minValue > maxValue)
//            //    throw new ArgumentOutOfRangeException("upperBound", maxValue, "upperBound must be >=lowerBound");

//            uint t = (x ^ (x << 11));
//            x = y; y = z; z = w;

//            // The explicit int cast before the first multiplication gives better performance.
//            // See comments in NextDouble.
//            int range = maxValue - minValue;
//            if (range < 0)
//            {	// If range is <0 then an overflow has occured and must resort to using long integer arithmetic instead (slower).
//                // We also must use all 32 bits of precision, instead of the normal 31, which again is slower.	
//                return minValue + (int)((REAL_UNIT_UINT * (double)(w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)))) * (double)((long)maxValue - (long)minValue));
//            }

//            // 31 bits of precision will suffice if range<=int.MaxValue. This allows us to cast to an int and gain
//            // a little more performance.
//            return minValue + (int)((REAL_UNIT_INT * (double)(int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))))) * (double)range);
//        }

//        /// <summary>
//        /// 用随机数填充指定字节数组的元素。
//        /// </summary>
//        /// <param name="buffer">包含随机数的字节数组。</param>
//        private void InternalNextBytes(byte[] buffer)
//        {
//            // Fill up the bulk of the buffer in chunks of 4 bytes at a time.
//            uint x = this.x, y = this.y, z = this.z, w = this.w;
//            int i = 0;
//            uint t;
//            for (int bound = buffer.Length - 3; i < bound;)
//            {
//                // Generate 4 bytes. 
//                // Increased performance is achieved by generating 4 random bytes per loop.
//                // Also note that no mask needs to be applied to zero out the higher order bytes before
//                // casting because the cast ignores thos bytes. Thanks to Stefan Trosch黷z for pointing this out.
//                t = (x ^ (x << 11));
//                x = y; y = z; z = w;
//                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                buffer[i++] = (byte)w;
//                buffer[i++] = (byte)(w >> 8);
//                buffer[i++] = (byte)(w >> 16);
//                buffer[i++] = (byte)(w >> 24);
//            }

//            // Fill up any remaining bytes in the buffer.
//            if (i < buffer.Length)
//            {
//                // Generate 4 bytes.
//                t = (x ^ (x << 11));
//                x = y; y = z; z = w;
//                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                buffer[i++] = (byte)w;
//                if (i < buffer.Length)
//                {
//                    buffer[i++] = (byte)(w >> 8);
//                    if (i < buffer.Length)
//                    {
//                        buffer[i++] = (byte)(w >> 16);
//                        if (i < buffer.Length)
//                        {
//                            buffer[i] = (byte)(w >> 24);
//                        }
//                    }
//                }
//            }
//            this.x = x; this.y = y; this.z = z; this.w = w;
//        }

//        private void InternalNextBytes(byte[] buffer, int offset, int count)
//        {
//            // Fill up the bulk of the buffer in chunks of 4 bytes at a time.
//            uint x = this.x, y = this.y, z = this.z, w = this.w;
//            int i = offset;
//            uint t;
//            for (int bound = offset + count - 3; i < bound;)
//            {
//                // Generate 4 bytes. 
//                // Increased performance is achieved by generating 4 random bytes per loop.
//                // Also note that no mask needs to be applied to zero out the higher order bytes before
//                // casting because the cast ignores thos bytes. Thanks to Stefan Trosch黷z for pointing this out.
//                t = (x ^ (x << 11));
//                x = y; y = z; z = w;
//                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                buffer[i++] = (byte)w;
//                buffer[i++] = (byte)(w >> 8);
//                buffer[i++] = (byte)(w >> 16);
//                buffer[i++] = (byte)(w >> 24);
//            }

//            // Fill up any remaining bytes in the buffer.
//            if (i < offset + count)
//            {
//                // Generate 4 bytes.
//                t = (x ^ (x << 11));
//                x = y; y = z; z = w;
//                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                buffer[i++] = (byte)w;
//                if (i < buffer.Length)
//                {
//                    buffer[i++] = (byte)(w >> 8);
//                    if (i < buffer.Length)
//                    {
//                        buffer[i++] = (byte)(w >> 16);
//                        if (i < buffer.Length)
//                        {
//                            buffer[i] = (byte)(w >> 24);
//                        }
//                    }
//                }
//            }
//            this.x = x; this.y = y; this.z = z; this.w = w;
//        }

//        #endregion

//        #region 获得随机数 基本类型

//        /// <summary>
//        /// 返回非负随机数。
//        /// </summary>
//        /// <returns></returns>
//        public int Next()
//        {
//            uint t = (x ^ (x << 11));
//            x = y; y = z; z = w;
//            w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//            uint rtn = w & 0x7FFFFFFF;
//            if (rtn == 0x7FFFFFFF)
//                return Next();
//            return (int)rtn;
//        }

//        /// <summary>
//        ///  返回一个小于所指定最大值的非负随机数。
//        /// </summary>
//        /// <param name="maxValue">要生成的随机数的上限（随机数不能取该上限值）。maxValue 必须大于或等于零。</param>
//        /// <returns>大于等于零且小于 maxValue 的 32 位带符号整数，即：返回值的范围通常包括零但不包括 maxValue。不过，如果 maxValue 等于零，则返回maxValue。</returns>
//        public int Next(int maxValue)
//        {
//            if (maxValue < 0)
//                throw new ArgumentOutOfRangeException("maxValue", maxValue, "maxValue must be >=0");

//            uint t = (x ^ (x << 11));
//            x = y; y = z; z = w;
//            return (int)((REAL_UNIT_INT * (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))))) * maxValue);
//        }

//        /// <summary>
//        /// 返回一个指定范围内的随机数。
//        /// </summary>
//        /// <param name="minValue">返回的随机数的下界（随机数可取该下界值）。</param>
//        /// <param name="maxValue">返回的随机数的上界（随机数不能取该上界值）。maxValue 必须大于或等于 minValue。</param>
//        /// <returns>一个大于等于 minValue 且小于 maxValue 的 32 位带符号整数，即：返回的值范围包括 minValue 但不包括 maxValue。如果minValue 等于 maxValue，则返回 minValue。</returns>
//        public int Next(int minValue, int maxValue)
//        {
//            if (minValue > maxValue)
//                throw new ArgumentOutOfRangeException("upperBound", maxValue, "upperBound must be >=lowerBound");

//            uint t = (x ^ (x << 11));
//            x = y; y = z; z = w;

//            // The explicit int cast before the first multiplication gives better performance.
//            // See comments in NextDouble.
//            int range = maxValue - minValue;
//            if (range < 0)
//            {	// If range is <0 then an overflow has occured and must resort to using long integer arithmetic instead (slower).
//                // We also must use all 32 bits of precision, instead of the normal 31, which again is slower.	
//                return minValue + (int)((REAL_UNIT_UINT * (double)(w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)))) * (double)((long)maxValue - (long)minValue));
//            }

//            // 31 bits of precision will suffice if range<=int.MaxValue. This allows us to cast to an int and gain
//            // a little more performance.
//            return minValue + (int)((REAL_UNIT_INT * (double)(int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))))) * (double)range);
//        }

//        public long NextLong()
//        {
//            return (((long)Next()) << 32) | Next();
//        }

//        public ulong NextULong()
//        {
//            return (((ulong)NextUInt()) << 32) | NextUInt();
//        }

//        public float NextFloat()
//        {
//            return (float)Next();
//        }

//        /// <summary>
//        /// 返回一个介于 0.0 和 1.0 之间的随机数。
//        /// </summary>
//        /// <returns>大于等于 0.0 并且小于 1.0 的双精度浮点数。</returns>
//        public double NextDouble()
//        {
//            uint t = (x ^ (x << 11));
//            x = y; y = z; z = w;

//            // Here we can gain a 2x speed improvement by generating a value that can be cast to 
//            // an int instead of the more easily available uint. If we then explicitly cast to an 
//            // int the compiler will then cast the int to a double to perform the multiplication, 
//            // this final cast is a lot faster than casting from a uint to a double. The extra cast
//            // to an int is very fast (the allocated bits remain the same) and so the overall effect 
//            // of the extra cast is a significant performance improvement.
//            //
//            // Also note that the loss of one bit of precision is equivalent to what occurs within 
//            // System.Random.
//            return (REAL_UNIT_INT * (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)))));
//        }

//        public string NextString()
//        {
//            int v = Next(1, 200);
//            int[] ints = new int[v];
//            NextInts(ints, 0, ints.Length);
//            char[] chars = new char[ints.Length];
//            for (int i = 0; i < ints.Length; i++)
//                chars[i] = Letter[(ints[i] % Letter.Length)];
//            return new string(chars);
//        }

//        public string NextAsciiString()
//        {
//            return NextString();
//        }

//        /// <summary>
//        /// 全中文的字符串
//        /// </summary>
//        /// <returns></returns>
//        public string NextChineseString()
//        {
//            return NextString();
//        }

//        /// <summary>
//        /// 随机字符串中只包含数字和字母随机数
//        /// </summary>
//        /// <returns></returns>
//        public string NextNumberLetterString()
//        {
//            return NextString();
//        }

//        /// <summary>
//        /// 返回非负随机数。
//        /// 如果转换成int，产生的随机数既包含正数，也包含负数，性能比Next方法提高了20%
//        /// </summary>
//        /// <returns>大于等于零且小于 System.UInt32.MaxValue 的 32 位带符号整数。</returns>
//        public uint NextUInt()
//        {
//            uint t = (x ^ (x << 11));
//            x = y; y = z; z = w;
//            return (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)));
//        }

//        /// <summary>
//        /// Generates a random int over the range 0 to int.MaxValue, inclusive. 
//        /// This method differs from Next() only in that the range is 0 to int.MaxValue
//        /// and not 0 to int.MaxValue-1.
//        /// 
//        /// The slight difference in range means this method is slightly faster than Next()
//        /// but is not functionally equivalent to System.Random.Next().
//        /// </summary>
//        /// <returns></returns>
//        public int NextInt()
//        {
//            uint t = (x ^ (x << 11));
//            x = y; y = z; z = w;
//            return (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))));
//        }

//        /// <summary>
//        /// Generates a single random bit.
//        /// This method's performance is improved by generating 32 bits in one operation and storing them
//        /// ready for future calls.
//        /// </summary>
//        /// <returns></returns>
//        public bool NextBool()
//        {
//            if (bitMask == 1)
//            {
//                // Generate 32 more bits.
//                uint t = (x ^ (x << 11));
//                x = y; y = z; z = w;
//                bitBuffer = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                // Reset the bitMask that tells us which bit to read next.
//                bitMask = 0x80000000;
//                return (bitBuffer & bitMask) == 0;
//            }

//            return (bitBuffer & (bitMask >>= 1)) == 0;
//        }

//        #endregion

//        #region 基本类型 数组

//        public unsafe byte[] NextBytes(int size)
//        {
//            byte[] buffer = new byte[size];
//            NextBytes(buffer, 0, size);
//            return buffer;
//        }

//        public unsafe void NextBytes(byte[] buffer)
//        {
//            NextBytes(buffer, 0, buffer.Length);
//        }

//        /// <summary>
//        /// 用随机数填充指定字节数组的元素。
//        /// 使用非安全代码获得随机数据
//        /// 通过一个测试获得比安全代码高一倍的性能，通过每次移动四个字节，这个可能在不同的CPU上有显著的差别
//        /// 通过采用每次移动填充四个数据，性能大约提升15%
//        /// </summary>
//        /// <param name="buffer">包含随机数的字节数组。</param>
//        public unsafe void NextBytes(byte[] buffer, int offset, int count)
//        {
//            uint x = this.x, y = this.y, z = this.z, w = this.w;

//            fixed (byte* pd = &buffer[offset])
//            {
//                byte* tpd = pd;
//                uint* pint = (uint*)pd;
//                int i = 0, len = count >> 4;
//                for (; i < len; i++)
//                {
//                    uint t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                    t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                    t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                    t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
//                }

//                int length = count - (i << 4);
//                tpd += (i << 4);
//                if ((length & 8) != 0)
//                {
//                    uint t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                    t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                    tpd += 8;
//                }
//                if ((length & 4) != 0)
//                {
//                    uint t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                    tpd += 4;
//                }
//                if ((length & 2) != 0)
//                {
//                    uint t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
//                    *((short*)tpd) = (short)w;
//                    tpd += 2;
//                }
//                if ((length & 1) != 0)
//                {
//                    uint t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
//                    *tpd++ = (byte)w;
//                }
//            }
//            this.x = x; this.y = y; this.z = z; this.w = w;
//        }

//        public unsafe void NextShorts(short[] buffer, int offset, int count)
//        {

//        }

//        public unsafe void NextUShorts(ushort[] buffer, int offset, int count)
//        {

//        }

//        public unsafe void NextInts(int[] buffer, int offset, int count)
//        {
//            uint x = this.x, y = this.y, z = this.z, w = this.w;
//            fixed (int* pd = &buffer[offset])
//            {
//                int* pint = pd;
//                int i = 0, len = count >> 2;
//                for (; i < len; i++)
//                {
//                    uint t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))));

//                    t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))));

//                    t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))));

//                    t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))));
//                }
//                int leave = count - (len << 2);
//                for (i = 0; i < leave; i++)
//                {
//                    uint t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))));
//                }
//            }
//            this.x = x; this.y = y; this.z = z; this.w = w;
//        }

//        public unsafe void NextUInts(uint[] buffer, int offset, int count)
//        {
//            uint x = this.x, y = this.y, z = this.z, w = this.w;
//            fixed (uint* pd = &buffer[offset])
//            {
//                uint* pint = pd;
//                int i = 0, len = count >> 2;
//                for (; i < len; i++)
//                {
//                    uint t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                    t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                    t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

//                    t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
//                }
//                int leave = count - (len << 2);
//                for (i = 0; i < leave; i++)
//                {
//                    uint t = (x ^ (x << 11));
//                    x = y; y = z; z = w;
//                    *pint++ = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
//                }
//            }
//            this.x = x; this.y = y; this.z = z; this.w = w;
//        }

//        public unsafe void NextLongs(long[] buffer, int offset, int count)
//        {

//        }

//        public unsafe void NextULongs(ulong[] buffer, int offset, int count)
//        {

//        }

//        public unsafe void NextFloats(float[] buffer, int offset, int count)
//        {

//        }

//        public unsafe void NextDoubles(double[] buffer, int offset, int count)
//        {

//        }

//        #endregion
//    }
//}
