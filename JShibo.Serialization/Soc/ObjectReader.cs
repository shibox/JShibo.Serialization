using JShibo.Serialization.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace JShibo.Serialization.Soc
{
    public class ObjectReader : OBase, IReader
    {
        #region 字段

        internal Deserialize<ObjectReader>[] desers;
        internal byte[] _buffer = null;

        #endregion

        #region 构造函数

        public ObjectReader(byte[] buffer)
        {
            _buffer = buffer;
            position = 1;
        }

        #endregion

        #region 方法

        private int ReadSize()
        {
            return Utils.ReadSize(_buffer, ref position);
        }

        private T[] Read<T>()
        {
            if (this._buffer[position] == NULL_FLAG)
            {
                position++;
                return null;
            }
            else if (this._buffer[position] == ZERO_FLAG)
            {
                position++;
                return new T[0];
            }
            else
            {
                int size = ReadSize();
                T[] value = new T[size];
                Buffer.BlockCopy(_buffer, position, value, 0, size);
                position += size;
                return value;
            }
        }

        private T[] Read<T>(int shift)
        {
            if (this._buffer[position] == NULL_FLAG)
            {
                position++;
                return null;
            }
            else if (this._buffer[position] == ZERO_FLAG)
            {
                position++;
                return new T[0];
            }
            else
            {
                int size = ReadSize();
                T[] value = new T[size];
                size = size << shift;
                Buffer.BlockCopy(_buffer, position, value, 0, size);
                position += size;
                return value;
            }
        }

        private List<T> ReadList<T>(int shift)
        {
            if (this._buffer[position] == NULL_FLAG)
            {
                position++;
                return null;
            }
            else if (this._buffer[position] == ZERO_FLAG)
            {
                position++;
                return new List<T>();
            }
            else
            {
                int size = ReadSize();
                T[] value = new T[size];
                size = size << shift;
                Buffer.BlockCopy(_buffer, position, value, 0, size);
                position += size;
                return value.ToList();
            }
        }

        private T[] Read<T>(int shift,int size)
        {
            T[] value = new T[size];
            size = size << shift;
            Buffer.BlockCopy(_buffer, position, value, 0, size);
            position += size;
            return value;
        }

        #endregion

        #region Read BaseType

        public int ReadVInt()
        {
            uint n = ReadVUInt();
            return (-((int)n & 1) ^ (((int)n >> 1) & 0x7fffffff));

            //int value = (int)n;
            //value = (-(value & 1) ^ ((value >> 1) & 0x7fffffff));
            //return value;
        }
        
        public uint ReadVUInt()
        {
            //int n = 2;
            ////int value = (int)ziggedValue;
            //n = (-(n & 1) ^ ((n >> 1) & 0x7fffffff));
            //return 0;
            return Utils.ReadVLong1UInt(_buffer, ref position);
        }
        
        public long ReadVLong()
        {
            ulong n = ReadVUInt();
            return (-((long)n & 1) ^ (((long)n >> 1) & 0x7fffffffffffffff));

            //int n = 2;
            ////int value = (int)ziggedValue;
            //n = (-(n & 1) ^ ((n >> 1) & 0x7fffffff));
            //return 0;
        }
        
        public ulong ReadVULong()
        {
            //int n = 2;
            ////int value = (int)ziggedValue;
            //n = (-(n & 1) ^ ((n >> 1) & 0x7fffffff));
            return 0;
        }

        public unsafe int ReadInt32()
        {
            //position += 4;
            //return (((this._buffer[position - 4] | (this._buffer[position - 3] << 8)) | (this._buffer[position - 2] << 0x10)) | (this._buffer[position - 1] << 0x18));

            int value = 0;
            fixed (byte* pd = &_buffer[position])
                value = *((int*)pd);
            position += 4;
            return value;
        }
        
        public unsafe uint ReadUInt32()
        {
            //position += 4;
            //return (uint)(((this._buffer[position - 4] | (this._buffer[position - 3] << 8)) | (this._buffer[position - 2] << 0x10)) | (this._buffer[position - 1] << 0x18));
            //return (uint)(((this.m_buffer[0] | (this.m_buffer[1] << 8)) | (this.m_buffer[2] << 0x10)) | (this.m_buffer[3] << 0x18));

            uint value = 0;
            fixed (byte* pd = &_buffer[position])
                value = *((uint*)pd);
            position += 4;
            return value;
        }
        
        public unsafe ulong ReadUInt64()
        {
            //position += 8;
            //uint num = (uint)(((this._buffer[position - 8] | (this._buffer[position - 7] << 8)) | (this._buffer[position - 6] << 0x10)) | (this._buffer[position - 5] << 0x18));
            //uint num2 = (uint)(((this._buffer[position - 4] | (this._buffer[position - 3] << 8)) | (this._buffer[position - 2] << 0x10)) | (this._buffer[position - 1] << 0x18));
            //return (ulong)(((ulong)num2 << 0x20) | num);
            ulong value = 0;
            fixed (byte* pd = &_buffer[position])
                value = *((ulong*)pd);
            position += 8;
            return value;
        }
        
        public unsafe long ReadInt64()
        {
            //position += 8;
            //uint num = (uint)(((this._buffer[position - 8] | (this._buffer[position - 7] << 8)) | (this._buffer[position - 6] << 0x10)) | (this._buffer[position - 5] << 0x18));
            //uint num2 = (uint)(((this._buffer[position - 4] | (this._buffer[position - 3] << 8)) | (this._buffer[position - 2] << 0x10)) | (this._buffer[position - 1] << 0x18));
            //return (long)(((long)num2 << 0x20) | num);

            long value = 0;
            fixed (byte* pd = &_buffer[position])
                value = *((long*)pd);
            position += 8;
            return value;
        }
        
        public unsafe char ReadChar()
        {
            char value = '\0';
            fixed (byte* pd = &_buffer[position])
                value = *((char*)pd);
            position += 2;
            return value;
        }
        
        public ushort ReadUInt16()
        {
            position += 2;
            return (ushort)(this._buffer[position - 2] | (this._buffer[position - 1] << 8));
        }
        
        public short ReadInt16()
        {
            position += 2;
            return (short)(this._buffer[position - 2] | (this._buffer[position - 1] << 8));
        }
        
        public string ReadString()
        {
            if (this._buffer[position] == NULL_FLAG)
            {
                position++;
                return null;
            }
            else
            {
                int len = Utils.ReadSize(_buffer, ref position);
                if (len == 0)
                    return string.Empty;
                else
                {
                    string value = sets.StringEncoding.GetString(_buffer, position, len);
                    position += len;
                    return value;
                }
            }
        }
        
        public bool ReadBoolean()
        {
            position++;
            return (this._buffer[position - 1] != 0);
        }
        
        public byte ReadByte()
        {
            position++;
            return (byte)this._buffer[position - 1];
        }
        
        public sbyte ReadSByte()
        {
            position++;
            return (sbyte)this._buffer[position - 1];
        }
        
        public unsafe decimal ReadDecimal()
        {
            decimal value = 0;
            fixed (byte* pd = &_buffer[position])
                value = *((decimal*)pd);
            position += 16;
            return value;
        }
        
        public unsafe float ReadSingle()
        {
            //position += 4;
            //uint num = (uint)(((this._buffer[position - 4] | (this._buffer[position - 3] << 8)) | (this._buffer[position - 2] << 0x10)) | (this._buffer[position - 1] << 0x18));
            //return *(((float*)&num));

            float value = 0;
            fixed (byte* pd = &_buffer[position])
                value = *((float*)pd);
            position += 4;
            return value;
        }
        
        public unsafe double ReadDouble()
        {
            //position += 8;
            //uint num = (uint)(((this._buffer[position - 8] | (this._buffer[position - 7] << 8)) | (this._buffer[position - 6] << 0x10)) | (this._buffer[position - 5] << 0x18));
            //uint num2 = (uint)(((this._buffer[position - 4] | (this._buffer[position - 3] << 8)) | (this._buffer[position - 2] << 0x10)) | (this._buffer[position - 1] << 0x18));
            //ulong num3 = (ulong)(((ulong)num2 << 0x20) | num);
            //return *(((double*)&num3));

            double value = 0;
            fixed (byte* pd = &_buffer[position])
                value = *((double*)pd);
            position += 8;
            return value;
        }

        public unsafe DateTime ReadDateTime()
        {
            long ticks = ReadInt64();
            return new DateTime(ticks);
            //long value = 0;
            //fixed (byte* pd = &_buffer[position])
            //    value = *((long*)pd);
            //position += 8;
            //return new DateTime(value);
            //return DateTime.FromBinary(value);
        }

        public unsafe DateTimeOffset ReadDateTimeOffset()
        {
            return new DateTime(ReadInt64());
        }

        public unsafe TimeSpan ReadTimeSpan()
        {
            return new TimeSpan(ReadInt64());
        }

        public unsafe Guid ReadGuid()
        {
            int a;
            short b, c;
            
            fixed (byte* pd = &_buffer[position])
            {
                byte* p = pd;
                a = *((int*)p);
                b = *((short*)(p + 4));
                c = *((short*)(p + 6));
                p += 8;

                //byte d, e, f, g, h, i, j, k;
                //d = *p++;
                //e = *p++;
                //f = *p++;
                //g = *p++;
                //h = *p++;
                //i = *p++;
                //j = *p++;
                //k = *p++;
                //Guid value = new Guid(a,b,c,d,e,f,g,h,i,j,k);

                Guid value = new Guid(a, b, c, *p++, *p++, *p++, *p++, *p++, *p++, *p++, *p++);
                position += 16;
                return value;
            }
        }

        public Stream ReadStream()
        {
            int size = ReadSize();
            MemoryStream stream = new MemoryStream();
            stream.Write(_buffer, position, size);
            return stream;
        }

        public IList ReadIList()
        {
            throw new NotImplementedException();
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

        public decimal[] ReadDecimals()
        {
            return Read<decimal>(4);
        }

        public string[] ReadStrings()
        {
            if (_buffer[position] == NULL_FLAG)
            {
                position++;
                return null;
            }
            else if (_buffer[position] == ZERO_FLAG)
            {
                position++;
                return ConstsEmpty.StringEmpty;
            }
            else
            {
                int size = ReadSize();
                string[] value = new string[size];
                for (int i = 0; i < size; i++)
                {
                    value[i] = ReadString();
                }
                return value;
            }
        }

        public DateTime[] ReadDateTimes()
        {
            int size = ReadSize();
            DateTime[] values = new DateTime[size];
            for (int i = 0; i < size; i++)
                values[i] = ReadDateTime();
            return values;
        }

        public DateTimeOffset[] ReadDateTimeOffsets()
        {
            int size = ReadSize();
            DateTimeOffset[] values = new DateTimeOffset[size];
            for (int i = 0; i < size; i++)
                values[i] = ReadDateTimeOffset();
            return values;
        }

        public TimeSpan[] ReadTimeSpans()
        {
            int size = ReadSize();
            TimeSpan[] values = new TimeSpan[size];
            for (int i = 0; i < size; i++)
                values[i] = ReadTimeSpan();
            return values;
        }

        public Guid[] ReadGuids()
        {
            int size = ReadSize();
            Guid[] values = new Guid[size];
            for (int i = 0; i < size; i++)
                values[i] = ReadGuid();
            return values;
        }

        public byte[][] ReadArrayBytes()
        {
            return null;
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

        public List<decimal> ReadDecimalList()
        {
            return ReadList<decimal>(4);
        }

        public List<string> ReadStringList()
        {
            if (_buffer[position] == NULL_FLAG)
            {
                position++;
                return null;
            }
            else if (_buffer[position] == ZERO_FLAG)
            {
                position++;
                return new List<string>();
            }
            else
            {
                int size = ReadSize();
                List<string> value = new List<string>(size);
                for (int i = 0; i < size; i++)
                    value.Add(ReadString());
                return value;
            }
        }

        public List<DateTime> ReadDateTimeList()
        {
            int size = ReadSize();
            List<DateTime> values = new List<DateTime>(size);
            for (int i = 0; i < size; i++)
                values.Add( ReadDateTime());
            return values;
        }

        public List<DateTimeOffset> ReadDateTimeOffsetList()
        {
            int size = ReadSize();
            List<DateTimeOffset> values = new List<DateTimeOffset>(size);
            for (int i = 0; i < size; i++)
                values.Add(ReadDateTimeOffset());
            return values;
        }

        public List<TimeSpan> ReadTimeSpanList()
        {
            int size = ReadSize();
            List<TimeSpan> values = new List<TimeSpan>(size);
            for (int i = 0; i < size; i++)
                values[i].Add(ReadTimeSpan());
            return values;
        }

        public List<Guid> ReadGuidList()
        {
            int size = ReadSize();
            List<Guid> values = new List<Guid>(size);
            for (int i = 0; i < size; i++)
                values.Add(ReadGuid());
            return values;
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

            //int size = ReadSize();
            //Dictionary<int, bool> result = new Dictionary<int, bool>(size);
            //for (int i = 0; i < size; i++)
            //    result.Add(ReadInt32(), ReadBoolean());
            //return result;
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
                result.Add(keys[i], new DateTime( values[i]));
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
            //return desers[curObj++](this);
            object value = ObjectBufferSerializer.Deserialize(this, desers[curObj++]);
            return value;
        }

        #endregion
        
    }
}
