using JShibo.Serialization.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization
{
    public class OValue : OBase, IReader
    {
        #region 字段

        internal Deserialize<OValue>[] desers;
        Random rd = new();

        #endregion

        #region 构造函数

        public OValue()
        {
            rd = new Random();
        }

        public OValue(int seed)
        {
            rd = new Random(seed);
        }

        #endregion

        #region 方法

        private unsafe void FixPointer()
        {
        }

        private int ReadSize()
        {
            return rd.Next(0,100);
        }

        private T[] Read<T>(int shift)
        {
            return Read<T>(shift, rd.Next(0, 100));
        }

        private T[] Read<T>(int shift,int size)
        {
            byte[] bytes = new byte[size << shift];
            rd.NextBytes(bytes);
            T[] value = new T[size];
            Buffer.BlockCopy(bytes, 0, value, 0, bytes.Length);
            return value;
        }

        private List<T> ReadList<T>(int shift)
        {
            return Read<T>(shift).ToList();
        }

        private ArraySegment<T> ReadArraySegment<T>(int shift)
        {
            T[] array = Read<T>(shift);
            return new ArraySegment<T>(array, 0, array.Length);
        }

        #endregion

        #region Read BaseType

        public int ReadVInt()
        {
            return rd.Next();
        }

        public uint ReadVUInt()
        {
            return (uint)rd.Next();
        }

        public long ReadVLong()
        {
            return rd.NextInt64();
        }

        public ulong ReadVULong()
        {
            return (ulong)rd.NextInt64();
        }

        public unsafe int ReadInt32()
        {
            return rd.Next();
        }

        public unsafe uint ReadUInt32()
        {
            return (uint)rd.Next();
        }

        public unsafe ulong ReadUInt64()
        {
            return (ulong)rd.NextInt64();
        }

        public unsafe long ReadInt64()
        {
            return rd.NextInt64();
        }

        public unsafe char ReadChar()
        {
            return (char)rd.Next();
        }

        public ushort ReadUInt16()
        {
            return (ushort)rd.Next();
        }

        public short ReadInt16()
        {
            return (short)rd.Next();
        }

        public string ReadString()
        {
            return rd.NextString();
        }

        public bool ReadBoolean()
        {
            return rd.Next(0,2) == 0;
        }

        public byte ReadByte()
        {
            return (byte)rd.Next();
        }

        public sbyte ReadSByte()
        {
            return (sbyte)rd.Next();
        }

        public unsafe decimal ReadDecimal()
        {
            return 0;
        }

        public unsafe float ReadSingle()
        {
            return rd.NextSingle();
        }

        public unsafe double ReadDouble()
        {
            return rd.NextDouble();
        }

        public unsafe DateTime ReadDateTime()
        {
            return DateTime.Now;
        }

        public unsafe DateTimeOffset ReadDateTimeOffset()
        {
            return DateTime.Now;
        }

        public unsafe TimeSpan ReadTimeSpan()
        {
            return TimeSpan.FromTicks( DateTime.Now.Ticks);
        }

        public unsafe Guid ReadGuid()
        {
            return new Guid();
        }

        public Stream ReadStream()
        {
            throw new NotImplementedException();
        }

        public IList ReadIList()
        {
             return null;
        }

        public IDictionary ReadIDictionary()
        {
            throw new NotImplementedException();
        }

        public IEnumerable ReadIEnumerable()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Read Array

        public bool[] ReadBools()
        {
            return Read<bool>(0);
        }

        public byte[] ReadBytes()
        {
            return Read<byte>(0);
        }

        public sbyte[] ReadSBytes()
        {
            return Read<sbyte>(0);
        }

        public short[] ReadShorts()
        {
            return Read<short>(1);
        }

        public ushort[] ReadUShorts()
        {
            return Read<ushort>(1);
        }

        public char[] ReadChars()
        {
            return Read<char>(1);
        }

        public int[] ReadInts()
        {
            return Read<int>(2);
        }

        public uint[] ReadUInts()
        {
            return Read<uint>(2);
        }

        public float[] ReadFloats()
        {
            return Read<float>(2);
        }

        public long[] ReadLongs()
        {
            return Read<long>(3);
        }

        public ulong[] ReadULongs()
        {
            return Read<ulong>(3);
        }

        public double[] ReadDoubles()
        {
            return Read<double>(3);
        }

        public string[] ReadStrings()
        {
            int count = rd.Next(0, 100);
            string[] value = new string[count];
            for (int i = 0; i < count; i++)
                value[i] = rd.NextString();
            return value;
        }

        public byte[][] ReadArrayBytes()
        {
            int count = rd.Next(0, 100);
            byte[][] value = new byte[count][];
            for (int i = 0; i < count; i++)
            {
                value[i] = new byte[rd.Next(1, 100)];
                rd.NextBytes(value[i]);
            }  
            return value;
        }

        #endregion

        #region Read List

        public List<bool> ReadBoolList()
        {
            return ReadList<bool>(0);
        }

        public List<byte> ReadByteList()
        {
            return ReadList<byte>(0);
        }

        public List<sbyte> ReadSByteList()
        {
            return ReadList<sbyte>(0);
        }

        public List<short> ReadShortList()
        {
            return ReadList<short>(1);
        }

        public List<ushort> ReadUShortList()
        {
            return ReadList<ushort>(1);
        }

        public List<char> ReadCharList()
        {
            return ReadList<char>(1);
        }

        public List<int> ReadIntList()
        {
            return ReadList<int>(2);
        }

        public List<uint> ReadUIntList()
        {
            return ReadList<uint>(2);
        }

        public List<float> ReadFloatList()
        {
            return ReadList<float>(2);
        }

        public List<long> ReadLongList()
        {
            return ReadList<long>(3);
        }

        public List<ulong> ReadULongList()
        {
            return ReadList<ulong>(3);
        }

        public List<double> ReadDoubleList()
        {
            return ReadList<double>(3);
        }

        public List<string> ReadStringList()
        {
            int count = rd.Next(0, 10000);
            List<string> value = new List<string>(count);
            for (int i = 0; i < count; i++)
                value.Add(rd.NextString());
            return value;
        }

        #endregion

        #region Read ArraySegment

        public ArraySegment<bool> ReadBoolArraySegment()
        {
            return ReadArraySegment<bool>(0);
        }

        public ArraySegment<byte> ReadByteArraySegment()
        {
            return ReadArraySegment<byte>(0);
        }

        public ArraySegment<sbyte> ReadSByteArraySegment()
        {
            return ReadArraySegment<sbyte>(0);
        }

        public ArraySegment<short> ReadShortArraySegment()
        {
            return ReadArraySegment<short>(1);
        }

        public ArraySegment<ushort> ReadUShortArraySegment()
        {
            return ReadArraySegment<ushort>(1);
        }

        public ArraySegment<char> ReadCharArraySegment()
        {
            return ReadArraySegment<char>(1);
        }

        public ArraySegment<int> ReadIntArraySegment()
        {
            return ReadArraySegment<int>(2);
        }

        public ArraySegment<uint> ReadUIntArraySegment()
        {
            return ReadArraySegment<uint>(2);
        }

        public ArraySegment<float> ReadFloatArraySegment()
        {
            return ReadArraySegment<float>(2);
        }

        public ArraySegment<long> ReadLongArraySegment()
        {
            return ReadArraySegment<long>(3);
        }

        public ArraySegment<ulong> ReadULongArraySegment()
        {
            return ReadArraySegment<ulong>(3);
        }

        public ArraySegment<double> ReadDoubleArraySegment()
        {
            return ReadArraySegment<double>(3);
        }

        public ArraySegment<string> ReadStringArraySegment()
        {
            int count = rd.Next(0, 100);
            string[] value = new string[count];
            for (int i = 0; i < count; i++)
                value[i] = rd.NextString();
            return new ArraySegment<string>( value,0,value.Length);
        }

        #endregion

        #region Read Dictionary

        public Dictionary<int, bool> ReadIntBoolDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            bool[] values = Read<bool>(0, size);
            Dictionary<int, bool> result = new Dictionary<int, bool>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, byte> ReadIntByteDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            byte[] values = Read<byte>(0, size);
            Dictionary<int, byte> result = new Dictionary<int, byte>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, sbyte> ReadIntSbyteDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            sbyte[] values = Read<sbyte>(0, size);
            Dictionary<int, sbyte> result = new Dictionary<int, sbyte>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, short> ReadIntShortDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            short[] values = Read<short>(1, size);
            Dictionary<int, short> result = new Dictionary<int, short>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, ushort> ReadIntUShortDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            ushort[] values = Read<ushort>(1, size);
            Dictionary<int, ushort> result = new Dictionary<int, ushort>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, char> ReadIntCharDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            char[] values = Read<char>(1, size);
            Dictionary<int, char> result = new Dictionary<int, char>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, int> ReadIntIntDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            int[] values = Read<int>(2, size);
            Dictionary<int, int> result = new Dictionary<int, int>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, uint> ReadIntUIntDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            uint[] values = Read<uint>(2, size);
            Dictionary<int, uint> result = new Dictionary<int, uint>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, float> ReadIntFloatDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            float[] values = Read<float>(2, size);
            Dictionary<int, float> result = new Dictionary<int, float>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, long> ReadIntLongDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            long[] values = Read<long>(3, size);
            Dictionary<int, long> result = new Dictionary<int, long>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, ulong> ReadIntULongDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            ulong[] values = Read<ulong>(3, size);
            Dictionary<int, ulong> result = new Dictionary<int, ulong>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, double> ReadIntDoubleDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            double[] values = Read<double>(3, size);
            Dictionary<int, double> result = new Dictionary<int, double>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, decimal> ReadIntDecimalDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            decimal[] values = Read<decimal>(4, size);
            Dictionary<int, decimal> result = new Dictionary<int, decimal>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, string> ReadIntStringDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            string[] values = new string[size];
            for (int i = 0; i < size; i++)
                values[i] = ReadString();
            Dictionary<int, string> result = new Dictionary<int, string>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<int, DateTime> ReadIntDateTimeDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            long[] values = Read<long>(3, size);
            Dictionary<int, DateTime> result = new Dictionary<int, DateTime>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], new DateTime(values[i]));
            return result;
        }

        public Dictionary<int, DateTimeOffset> ReadIntDateTimeOffsetDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            long[] values = Read<long>(3, size);
            Dictionary<int, DateTimeOffset> result = new Dictionary<int, DateTimeOffset>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], new DateTime(values[i]));
            return result;
        }

        public Dictionary<int, TimeSpan> ReadIntTimeSpanDictionary()
        {
            int size = ReadSize();
            int[] keys = Read<int>(2, size);
            long[] values = Read<long>(3, size);
            Dictionary<int, TimeSpan> result = new Dictionary<int, TimeSpan>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], new TimeSpan(values[i]));
            return result;
        }

        public Dictionary<int, Guid> ReadIntGuidDictionary()
        {
            throw new NotImplementedException();
            //int size = ReadSize();
            //int[] keys = Read<int>(2, size);
            //bool[] values = Read<bool>(0, size);
            //Dictionary<int, bool> result = new Dictionary<int, bool>(size);
            //for (int i = 0; i < size; i++)
            //    result.Add(keys[i], values[i]);
            //return result;
        }



        public Dictionary<long, bool> ReadLongBoolDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            bool[] values = Read<bool>(0, size);
            Dictionary<long, bool> result = new Dictionary<long, bool>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, byte> ReadLongByteDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            byte[] values = Read<byte>(0, size);
            Dictionary<long, byte> result = new Dictionary<long, byte>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, sbyte> ReadLongSbyteDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            sbyte[] values = Read<sbyte>(0, size);
            Dictionary<long, sbyte> result = new Dictionary<long, sbyte>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, short> ReadLongShortDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            short[] values = Read<short>(1, size);
            Dictionary<long, short> result = new Dictionary<long, short>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, ushort> ReadLongUShortDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            ushort[] values = Read<ushort>(1, size);
            Dictionary<long, ushort> result = new Dictionary<long, ushort>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, char> ReadLongCharDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            char[] values = Read<char>(1, size);
            Dictionary<long, char> result = new Dictionary<long, char>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, int> ReadLongIntDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            int[] values = Read<int>(2, size);
            Dictionary<long, int> result = new Dictionary<long, int>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, uint> ReadLongUIntDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            uint[] values = Read<uint>(2, size);
            Dictionary<long, uint> result = new Dictionary<long, uint>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, float> ReadLongFloatDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            float[] values = Read<float>(2, size);
            Dictionary<long, float> result = new Dictionary<long, float>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, long> ReadLongLongDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            long[] values = Read<long>(3, size);
            Dictionary<long, long> result = new Dictionary<long, long>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, ulong> ReadLongULongDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            ulong[] values = Read<ulong>(3, size);
            Dictionary<long, ulong> result = new Dictionary<long, ulong>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, double> ReadLongDoubleDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            double[] values = Read<double>(3, size);
            Dictionary<long, double> result = new Dictionary<long, double>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, decimal> ReadLongDecimalDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            decimal[] values = Read<decimal>(4, size);
            Dictionary<long, decimal> result = new Dictionary<long, decimal>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, string> ReadLongStringDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            string[] values = new string[size];
            for (int i = 0; i < size; i++)
                values[i] = ReadString();
            Dictionary<long, string> result = new Dictionary<long, string>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], values[i]);
            return result;
        }

        public Dictionary<long, DateTime> ReadLongDateTimeDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            long[] values = Read<long>(3, size);
            Dictionary<long, DateTime> result = new Dictionary<long, DateTime>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], new DateTime(values[i]));
            return result;
        }

        public Dictionary<long, DateTimeOffset> ReadLongDateTimeOffsetDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            long[] values = Read<long>(3, size);
            Dictionary<long, DateTimeOffset> result = new Dictionary<long, DateTimeOffset>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], new DateTime(values[i]));
            return result;
        }

        public Dictionary<long, TimeSpan> ReadLongTimeSpanDictionary()
        {
            int size = ReadSize();
            long[] keys = Read<long>(3, size);
            long[] values = Read<long>(3, size);
            Dictionary<long, TimeSpan> result = new Dictionary<long, TimeSpan>(size);
            for (int i = 0; i < size; i++)
                result.Add(keys[i], new TimeSpan(values[i]));
            return result;
        }

        public Dictionary<long, Guid> ReadLongGuidDictionary()
        {
            throw new NotImplementedException();
            //int size = ReadSize();
            //long[] keys = Read<long>(3, size);
            //bool[] values = Read<bool>(0, size);
            //Dictionary<int, bool> result = new Dictionary<int, bool>(size);
            //for (int i = 0; i < size; i++)
            //    result.Add(keys[i], values[i]);
            //return result;
        }

        #endregion

        #region Read Other

        public object ReadObject()
        {
            return desers[curObj++](this);
        }

        #endregion

    }
}
