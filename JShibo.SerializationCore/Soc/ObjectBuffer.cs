using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JShibo.Serialization.Common;

namespace JShibo.Serialization.Soc
{
    /// <summary>
    /// 序列化和反序列化使用的内存流
    /// 
    /// 二进制序列化格式参考
    /// http://www.cnblogs.com/lxinxuan/archive/2006/09/06/496340.html
    /// http://blog.chinaunix.net/uid-27105712-id-3266286.html
    /// http://www.blogjava.net/kelly859/archive/2012/08/14/385478.html
    /// http://www.cnblogs.com/zhuweisky/archive/2011/04/17/2017260.html
    /// 
    /// http://www.open-open.com/home/space.php?uid=37924&do=blog&id=5873
    /// https://developers.google.com/protocol-buffers/docs/encoding?csw=1
    /// https://developers.google.com/protocol-buffers/docs/encoding?hl=zh
    /// 
    /// Soc全称：Shibo Object Component
    /// </summary>
    public class ObjectBuffer :OBase, IWriter,IVWriter, IByteBase
    {
        #region 字段

        internal Serialize<ObjectBuffer>[] sers;
        internal byte[] _buffer = null;
        unsafe internal byte* bp = null;

        #endregion

        #region 构造函数

        public ObjectBuffer()
            : this(64)
        {
        }

        public ObjectBuffer(int cap)
        {
            _buffer = new byte[cap];
        }

        public ObjectBuffer(byte[] buffer)
        {
            _buffer = buffer;
        }

        #endregion

        #region 方法

        internal void SetInfo(ObjectBufferContext info)
        {
            #region old
            //if (info.Names.Length > 0)
            //{
            //    if (sets.CamelCase)
            //        names = info.GetNamesCamelCase();
            //    else
            //        names = info.Names;
            //    if (sets.UseSingleQuotes)
            //        Quote = '\'';

            //    sers = info.SerializeStreams;
            //    types = info.Types;
            //    typeCounts = info.TypeCounts;
            //    nameCounts = info.NameCounts;
            //    nameLens = info.NameLens;
            //}
            //else
            //    isJsonBaseType = true;
            #endregion
            
            sers = info.Serializers;
            types = info.Types;
            typeCounts = info.TypeCounts;
            isHaveObjectType = info.IsHaveObjectType;
        }

        private unsafe void FixPointer()
        {
            fixed(byte* pd=&_buffer[position])
            {
                bp = pd;
            }
        }

        public unsafe void Reset()
        {
            position = 0;
        }

        private void Resize(int size)
        {
            if (size > _buffer.Length)
            {
                byte[] temp = new byte[_buffer.Length + size];
                Buffer.BlockCopy(_buffer, 0, temp, 0, position);
                _buffer = temp;
            }
            else
            {
                byte[] temp = new byte[_buffer.Length * 2];
                Buffer.BlockCopy(_buffer, 0, temp, 0, position);
                _buffer = temp;
            }
        }

        /// <summary>
        /// 写入类型数据信息，用于直接实现反序列化
        /// </summary>
        /// <param name="type"></param>
        internal void WriteType(Type type)
        {
            if (sets.WriteType)
            {
                _buffer[position++] = VersionWriteType;
                Write(type.Assembly.FullName);
                Write(type.FullName);
            }
            else
                _buffer[position++] = Version;
        }

        private void WriteNull()
        {
            if (_buffer.Length < position + 1)
                Resize(1);
            this._buffer[position] = NULL_FLAG;
            position++;
        }

        private void WriteZero()
        {
            if (_buffer.Length < position + 1)
                Resize(1);
            this._buffer[position] = ZERO_FLAG;
            position++;
        }

        private bool WriteNullOrEmpty<TKey, TValue>(IDictionary<TKey, TValue> value)
        {
            bool isWriter = false;
            if (value == null)
            {
                WriteNull();
                isWriter = true;
            }
            else if (value.Count == 0)
            {
                WriteZero();
                isWriter = true;
            }
            return isWriter;
        }

        private bool WriteNullOrEmpty(IDictionary value)
        {
            bool isWriter = false;
            if (value == null)
            {
                WriteNull();
                isWriter = true;
            }
            else if (value.Count == 0)
            {
                WriteZero();
                isWriter = true;
            }
            return isWriter;
        }

        private int ReadSize()
        {
            return Utils.ReadSize(_buffer, ref position);
        }

        /// <summary>
        /// 写入非值类型长度固定的数据，使用可变长度写入，同时对于写入的长度不区分类型
        /// 如int数组，10个写入的长度是40，而不是10，方便在某些数据处理时在不知类型的情况下进行跳跃
        /// 
        /// </summary>
        /// <param name="size"></param>
        private void WriteSize(int size)
        {
            Utils.WriteSize(_buffer, size, ref position);
        }

        private void Write<T>(ArraySegment<T> value, int shift)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                int size = value.Count << shift;
                if (_buffer.Length < position + size + 5)
                    Resize(size + 5);
                WriteSize(value.Count);
                Buffer.BlockCopy(value.Array, value.Offset << shift, _buffer, position, size);
                position += size;
            }
        }

        private void Write<T>(T[] value, int shift)
        {
            if (value == null)
                WriteNull();
            else if (value.Length == 0)
                WriteZero();
            else
            {
                int size = value.Length << shift;
                int capacity = size + Utils.GetSize(size);
                if (_buffer.Length < position + capacity)
                    Resize(capacity);
                WriteSize(value.Length);
                Buffer.BlockCopy(value, 0, _buffer, position, size);
                position += size;
            }
        }

        private void WriteArray<T>(T[] value, int shift)
        {
            if (value == null)
                WriteNull();
            else if (value.Length == 0)
                WriteZero();
            else
            {
                int size = value.Length << shift;
                int capacity = size + Utils.GetSize(size);
                if (_buffer.Length < position + capacity)
                    Resize(capacity);
                WriteSize(value.Length);
                if (typeof(T) == typeof(byte[]))
                {
                    byte[][] v = value as byte[][];
                    foreach (byte[] key in v)
                        Write(key);
                }
                if (typeof(T) == typeof(int[]))
                {
                    int[][] v = value as int[][];
                    foreach (int[] key in v)
                        Write(key);
                }
            }
        }

        private void WriteKeyValue<T>(T[] value, int shift)
        {
            int size = value.Length << shift;
            Buffer.BlockCopy(value, 0, _buffer, position, size);
            position += size;
        }

        private void Write<T>(List<T> value, int shift)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
                Write<T>(value.ToArray(), shift);

            ////需要linq的支持
#if NET20
                InternalWrite(IEnumerable<int>(value));
#endif
        }

        private void Write<T>(IList<T> value, int shift)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
                Write<T>(value.ToArray(), shift);
        }

        private void Write<T>(IEnumerable<T> value, int shift)
        {
            if (value == null)
                WriteNull();
            else
                Write<T>(value.ToArray(), shift);

#if NET20
            List<int> array = new List<int>();
            foreach (int v in value)
                array.Add(v);
            InternalWrite(array.ToArray());              
#endif
        }

        private void WriteArray<T>(List<T> value, int shift)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
                WriteArray<T>(value.ToArray(), shift);
        }

        private void WriteArray<T>(IList<T> value, int shift)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
                WriteArray<T>(value.ToArray(), shift);
        }

        private void WriteArray<T>(IEnumerable<T> value, int shift)
        {
            if (value == null)
                WriteNull();
            else
                WriteArray<T>(value.ToArray(), shift);
        }

        public void Write<TKey, TValue>(IDictionary<TKey, TValue> value)
            //where TKey : struct
            //where TValue : struct
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 20 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<TKey, TValue> item in value)
                {
                    Write<TKey>(item.Key);
                    Write<TValue>(item.Value);
                }
            }
        }

        private unsafe void Write<T>(T value)// where T : struct
        {
            //if (value is int)
            //{
            //    int v = (int)value;// Convert.ToInt32(value);
            //    fixed (byte* pd = &_buffer[position])
            //        *((int*)pd) = *((int*)&(v));
            //    position += 4;
            //}
            
        }

        private void InternalWrite(object[] value)
        {
            int size = value.Length << 2;
            if (_buffer.Length < position + size + 5)
                Resize(size + 5);
            WriteSize(size);
            if (size < 10)
            {
                for (int i = 0; i < size; i++)
                {
                    //int v = value[i];
                    //this._buffer[position] = (byte)v;
                    //this._buffer[position + 1] = (byte)(v >> 8);
                    //this._buffer[position + 2] = (byte)(v >> 0x10);
                    //this._buffer[position + 3] = (byte)(v >> 0x18);
                    //position += 4;
                }
            }
            else
            {
                //Buffer.BlockCopy(value, 0, _buffer, position, size);
                //position += size;
            }
        }

        private void InternalWrite(IList<object> value)
        {
            ////需要linq的支持
#if NET20
                InternalWrite(IEnumerable<int>(value));
#endif
            InternalWrite(value.ToArray());
        }

        private void InternalWrite(IEnumerable<object> value)
        {
#if NET20
            List<int> array = new List<int>();
            foreach (int v in value)
                array.Add(v);
            InternalWrite(array.ToArray());              
#endif
            InternalWrite(value.ToArray());
        }


        private void InternalWrite(DateTime[] value, int offset, int count)
        {
            int size = count << 3;
            if (_buffer.Length < position + size + 5)
                Resize(size + 5);
            WriteSize(count);
            if (size < 10)
            {
                Buffer.BlockCopy(value, offset << 3, _buffer, position, size);
                position += size;
            }
            else
            {
                Buffer.BlockCopy(value, offset << 3, _buffer, position, size);
                position += size;
            }
        }

        private void InternalWrite(IList<DateTime> value)
        {
            ////需要linq的支持
#if NET20
                InternalWrite(IEnumerable<int>(value));
#endif
            InternalWrite(value.ToArray(), 0, value.Count);
        }

        private void InternalWrite(IEnumerable<DateTime> value)
        {
#if NET20
            List<int> array = new List<int>();
            foreach (int v in value)
                array.Add(v);
            InternalWrite(array.ToArray());              
#endif
            DateTime[] array = value.ToArray();
            InternalWrite(array,0,array.Length);
        }




        private void InternalWrite(string[] value, int offset, int size)
        {
            WriteSize(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                Write(value[i]);
            }
        }

        private void InternalWrite(IList<string> value)
        {
            ////需要linq的支持
#if NET20
                InternalWrite(IEnumerable<int>(value));
#endif
            InternalWrite(value.ToArray(),0,value.Count);
        }

        private void InternalWrite(IEnumerable<string> value)
        {
#if NET20
            List<int> array = new List<int>();
            foreach (int v in value)
                array.Add(v);
            InternalWrite(array.ToArray());              
#endif
            string[] array = value.ToArray();
            InternalWrite(array,0,array.Length);
        }


        #endregion

        //#region Write BaseType

        //unsafe public void Write(int value)
        //{
        //    if (_buffer.Length < position + 4)
        //        Resize(4);
            
        //    fixed (byte* pd = &_buffer[position])
        //        *((int*)pd) = *((int*)&value);
        //    position += 4;
        //}

        //public void Write(bool value)
        //{
        //    if (_buffer.Length < position + 1)
        //        Resize(1);

        //    this._buffer[position++] = value ? ((byte)1) : ((byte)0);
        //}

        //public void Write(byte value)
        //{
        //    if (_buffer.Length < position + 1)
        //        Resize(1);

        //    this._buffer[position++] = value;
        //}

        //public void Write(sbyte value)
        //{
        //    if (_buffer.Length < position + 1)
        //        Resize(1);

        //    this._buffer[position++] = (byte)value;
        //}

        //unsafe public void Write(decimal value)
        //{
        //    if (_buffer.Length < position + 16)
        //        Resize(16);

        //    DecimalStruct v = new DecimalStruct(value);
        //    fixed (byte* pd = &_buffer[position])
        //    {
        //        *((int*)pd) = v.lo;
        //        *((int*)(pd + 4)) = v.mid;
        //        *((int*)(pd + 8)) = v.hi;
        //        *((int*)(pd + 12)) = v.flags;
        //    }
        //    position += 16;
        //}

        //unsafe public void Write(double value)
        //{
        //    if (_buffer.Length < position + 8)
        //        Resize(8);

        //    fixed (byte* pd = &_buffer[position])
        //        *((ulong*)pd) = *((ulong*)&value);

        //    position += 8;
        //}

        //public void Write(short value)
        //{
        //    if (_buffer.Length < position + 2)
        //        Resize(2);

        //    this._buffer[position] = (byte)value;
        //    this._buffer[position + 1] = (byte)(value >> 8);
        //    position += 2;
        //}

        //public void Write(ushort value)
        //{
        //    if (_buffer.Length < position + 2)
        //        Resize(2);

        //    this._buffer[position] = (byte)value;
        //    this._buffer[position + 1] = (byte)(value >> 8);
        //    position += 2;
        //}

        //unsafe public void Write(uint value)
        //{
        //    if (_buffer.Length < position + 4)
        //        Resize(4);

        //    fixed (byte* pd = &_buffer[position])
        //        *((int*)pd) = *((int*)&value);

        //    position += 4;
        //}

        //unsafe public void Write(long value)
        //{
        //    if (_buffer.Length < position + 8)
        //        Resize(8);
        //    fixed (byte* pd = &_buffer[position])
        //        *((long*)pd) = *((long*)&value);

        //    position += 8;
        //}

        //unsafe public void Write(ulong value)
        //{
        //    if (_buffer.Length < position + 8)
        //        Resize(8);

        //    fixed (byte* pd = &_buffer[position])
        //        *((ulong*)pd) = *((ulong*)&value);

        //    position += 8;
        //}

        //unsafe public void Write(float value)
        //{
        //    if (_buffer.Length < position + 4)
        //        Resize(4);

        //    fixed (byte* pd = &_buffer[position])
        //        *((uint*)pd) = *((uint*)&value);

        //    position += 4;
        //}

        //public void Write(char value)
        //{
        //    if (_buffer.Length < position + 2)
        //        Resize(2);

        //    this._buffer[position] = (byte)value;
        //    this._buffer[position + 1] = (byte)(value >> 8);
        //    position += 2;
        //}

        ///// <summary>
        ///// 字符串的长度以固定四个字节写入
        ///// </summary>
        ///// <param name="value"></param>
        //public void Write(string value)
        //{
        //    if (value == null)
        //        WriteNull();
        //    else if (value.Length == 0)
        //        WriteZero();
        //    else
        //    {
        //        if (sets.StringEncoding == Encoding.Unicode)
        //        {
        //            int size = (value.Length << 1);
        //            if (_buffer.Length < position + size + 6)
        //                Resize(size + 6);
        //            Utils.WriteSize(_buffer, size, ref position);
        //            Utils.StringAsUnicode(_buffer, position, value);
        //            position += size;
        //        }
        //        else
        //        {
        //            byte[] bytes = sets.StringEncoding.GetBytes(value);
        //            if (_buffer.Length < position + bytes.Length + 6)
        //                Resize(bytes.Length + 6);
        //            Utils.WriteSize(_buffer, bytes.Length, ref position);
        //            Buffer.BlockCopy(bytes, 0, _buffer, position, bytes.Length);
        //            position += bytes.Length;
        //        }
        //    }
        //}

        //public void Write(Uri value)
        //{
        //    if (value == null)
        //        WriteNull();
        //    else if (value.AbsoluteUri.Length == 0)
        //        WriteZero();
        //    else
        //    {
        //        string uri = value.AbsoluteUri;
        //        if (_buffer.Length < position + uri.Length + 4)
        //            Resize(uri.Length + 4);
        //        Utils.WriteSize(_buffer, uri.Length, ref position);
        //        Utils.StringAsAscii(_buffer, position, uri);
        //        position += uri.Length;
        //    }
        //}

        //public void Write(object value)
        //{
        //    if (value == null)
        //    {
        //        if (_buffer.Length < position + 1)
        //            Resize(1);
        //        this._buffer[position] = NULL_FLAG;
        //        position++;
        //    }
        //    else
        //    {
        //        if (curDepth >= maxDepth)
        //            throw new Exception("序列化超出最大序列化深度！");
        //        curDepth++;
        //        sers[curObj++](this, value);
        //    }
        //}

        //public void Write(DateTime value)
        //{
        //    Write(value.Ticks);
        //}

        //public void Write(DateTimeOffset value)
        //{
        //    Write(value.Ticks);
        //}

        //public void Write(TimeSpan value)
        //{
        //    Write(value.Ticks);
        //}

        ///// <summary>
        ///// 实际写入16个字节长度的数据，需要一定的转换
        ///// </summary>
        ///// <param name="value"></param>
        //public unsafe void Write(Guid value)
        //{
        //    fixed (byte* ds = &_buffer[position])
        //    {
        //        byte* pd = ds;
        //        GuidStructSoc guid = new GuidStructSoc(value);
        //        int a = guid._a;
        //        short b = guid._b, c = guid._c;
        //        *((int*)pd) = *((int*)&a);
        //        *((short*)(pd + 4)) = *((short*)&b);
        //        *((short*)(pd + 6)) = *((short*)&c);
        //        pd += 8;

        //        *pd++ = guid._d;
        //        *pd++ = guid._e;
        //        *pd++ = guid._f;
        //        *pd++ = guid._g;
        //        *pd++ = guid._h;
        //        *pd++ = guid._i;
        //        *pd++ = guid._j;
        //        *pd++ = guid._k;
        //    }
        //    position += 16;
        //}

        //public void Write(Stream value)
        //{
        //    long len = value.Length;

        //}

        //public void WriteVInt(int value)
        //{
        //    uint v = (uint)((value << 1) ^ (value >> 31));
        //    WriteVInt(v);
        //}

        //public void WriteVInt(uint value)
        //{
        //    Utils.WriteVLong1(_buffer, value, ref position);
        //}

        //public void WriteVLong(long value)
        //{
        //    ulong v = (ulong)((value << 1) ^ (value >> 63));
        //    WriteVLong(v);
        //}

        //public void WriteVLong(ulong value)
        //{

        //}

        //public void WriteObject(object value)
        //{
        //    WriteNotSupported();
        //}

        //#endregion

        #region MyRegion

        //#region Write BaseType

        //unsafe public void Write(int value)
        //{
        //    if (_buffer.Length < position + 4)
        //        Resize(4);

        //    //this._buffer[position] = (byte)value;
        //    //this._buffer[position + 1] = (byte)(value >> 8);
        //    //this._buffer[position + 2] = (byte)(value >> 0x10);
        //    //this._buffer[position + 3] = (byte)(value >> 0x18);

        //    fixed (byte* pd = &_buffer[position])
        //        *((int*)pd) = *((int*)&value);
        //    position += 4;
        //}

        //public void Write(bool value)
        //{
        //    if (_buffer.Length < position + 1)
        //        Resize(1);

        //    this._buffer[position++] = value ? ((byte)1) : ((byte)0);
        //}

        //public void Write(byte value)
        //{
        //    if (_buffer.Length < position + 1)
        //        Resize(1);

        //    this._buffer[position++] = value;
        //}

        //public void Write(sbyte value)
        //{
        //    if (_buffer.Length < position + 1)
        //        Resize(1);

        //    this._buffer[position++] = (byte)value;
        //}

        //unsafe public void Write(decimal value)
        //{
        //    if (_buffer.Length < position + 16)
        //        Resize(16);

        //    DecimalStruct v = new DecimalStruct(value);
        //    fixed (byte* pd = &_buffer[position])
        //    {
        //        *((int*)pd) = v.lo;
        //        *((int*)(pd + 4)) = v.mid;
        //        *((int*)(pd + 8)) = v.hi;
        //        *((int*)(pd + 12)) = v.flags;
        //    }
        //    position += 16;

        //    //int[] v = decimal.GetBits(value);

        //    //this._buffer[position] = (byte)v[0];
        //    //this._buffer[position + 1] = (byte)(v[0] >> 8);
        //    //this._buffer[position + 2] = (byte)(v[0] >> 0x10);
        //    //this._buffer[position + 3] = (byte)(v[0] >> 0x18);

        //    //this._buffer[position + 4] = (byte)v[1];
        //    //this._buffer[position + 5] = (byte)(v[1] >> 8);
        //    //this._buffer[position + 6] = (byte)(v[1] >> 0x10);
        //    //this._buffer[position + 7] = (byte)(v[1] >> 0x18);

        //    //this._buffer[position + 8] = (byte)v[2];
        //    //this._buffer[position + 9] = (byte)(v[2] >> 8);
        //    //this._buffer[position + 10] = (byte)(v[2] >> 0x10);
        //    //this._buffer[position + 11] = (byte)(v[2] >> 0x18);

        //    //this._buffer[position + 12] = (byte)v[3];
        //    //this._buffer[position + 13] = (byte)(v[3] >> 8);
        //    //this._buffer[position + 14] = (byte)(v[3] >> 0x10);
        //    //this._buffer[position + 15] = (byte)(v[3] >> 0x18);
        //    //position += 16;
        //}

        //unsafe public void Write(double value)
        //{
        //    if (_buffer.Length < position + 8)
        //        Resize(8);

        //    //ulong num = *((ulong*)&value);

        //    fixed (byte* pd = &_buffer[position])
        //        *((ulong*)pd) = *((ulong*)&value);

        //    //this._buffer[position] = (byte)num;
        //    //this._buffer[position + 1] = (byte)(num >> 8);
        //    //this._buffer[position + 2] = (byte)(num >> 0x10);
        //    //this._buffer[position + 3] = (byte)(num >> 0x18);
        //    //this._buffer[position + 4] = (byte)(num >> 0x20);
        //    //this._buffer[position + 5] = (byte)(num >> 40);
        //    //this._buffer[position + 6] = (byte)(num >> 0x30);
        //    //this._buffer[position + 7] = (byte)(num >> 0x38);
        //    position += 8;
        //}

        //public void Write(short value)
        //{
        //    if (_buffer.Length < position + 2)
        //        Resize(2);

        //    this._buffer[position] = (byte)value;
        //    this._buffer[position + 1] = (byte)(value >> 8);
        //    position += 2;
        //}

        //public void Write(ushort value)
        //{
        //    if (_buffer.Length < position + 2)
        //        Resize(2);

        //    this._buffer[position] = (byte)value;
        //    this._buffer[position + 1] = (byte)(value >> 8);
        //    position += 2;
        //}

        //unsafe public void Write(uint value)
        //{
        //    if (_buffer.Length < position + 4)
        //        Resize(4);

        //    fixed (byte* pd = &_buffer[position])
        //        *((int*)pd) = *((int*)&value);

        //    //this._buffer[position] = (byte)value;
        //    //this._buffer[position + 1] = (byte)(value >> 8);
        //    //this._buffer[position + 2] = (byte)(value >> 0x10);
        //    //this._buffer[position + 3] = (byte)(value >> 0x18);
        //    position += 4;
        //}

        //unsafe public void Write(long value)
        //{
        //    if (_buffer.Length < position + 8)
        //        Resize(8);
        //    fixed (byte* pd = &_buffer[position])
        //        *((long*)pd) = *((long*)&value);

        //    //this._buffer[position] = (byte)value;
        //    //this._buffer[position + 1] = (byte)(value >> 8);
        //    //this._buffer[position + 2] = (byte)(value >> 0x10);
        //    //this._buffer[position + 3] = (byte)(value >> 0x18);
        //    //this._buffer[position + 4] = (byte)(value >> 0x20);
        //    //this._buffer[position + 5] = (byte)(value >> 40);
        //    //this._buffer[position + 6] = (byte)(value >> 0x30);
        //    //this._buffer[position + 7] = (byte)(value >> 0x38);
        //    position += 8;
        //}

        //unsafe public void Write(ulong value)
        //{
        //    if (_buffer.Length < position + 8)
        //        Resize(8);

        //    fixed (byte* pd = &_buffer[position])
        //        *((ulong*)pd) = *((ulong*)&value);

        //    //this._buffer[position] = (byte)value;
        //    //this._buffer[position + 1] = (byte)(value >> 8);
        //    //this._buffer[position + 2] = (byte)(value >> 0x10);
        //    //this._buffer[position + 3] = (byte)(value >> 0x18);
        //    //this._buffer[position + 4] = (byte)(value >> 0x20);
        //    //this._buffer[position + 5] = (byte)(value >> 40);
        //    //this._buffer[position + 6] = (byte)(value >> 0x30);
        //    //this._buffer[position + 7] = (byte)(value >> 0x38);
        //    position += 8;
        //}

        //unsafe public void Write(float value)
        //{
        //    if (_buffer.Length < position + 4)
        //        Resize(4);

        //    //uint num = *((uint*)&value);
        //    fixed (byte* pd = &_buffer[position])
        //        *((uint*)pd) = *((uint*)&value);

        //    //this._buffer[position] = (byte)num;
        //    //this._buffer[position + 1] = (byte)(num >> 8);
        //    //this._buffer[position + 2] = (byte)(num >> 0x10);
        //    //this._buffer[position + 3] = (byte)(num >> 0x18);
        //    position += 4;
        //}

        //public void Write(char value)
        //{
        //    if (_buffer.Length < position + 2)
        //        Resize(2);

        //    this._buffer[position] = (byte)value;
        //    this._buffer[position + 1] = (byte)(value >> 8);
        //    position += 2;
        //}

        ///// <summary>
        ///// 字符串的长度以固定四个字节写入
        ///// </summary>
        ///// <param name="value"></param>
        //public void Write(string value)
        //{
        //    if (value == null)
        //        WriteNull();
        //    else if (value.Length == 0)
        //        WriteZero();
        //    else
        //    {
        //        if (sets.StringEncoding == Encoding.Unicode)
        //        {
        //            int size = (value.Length << 1);
        //            if (_buffer.Length < position + size + 6)
        //                Resize(size + 6);
        //            Utils.WriteSize(_buffer, size, ref position);
        //            Utils.StringAsUnicode(_buffer, position, value);
        //            position += size;
        //        }
        //        else
        //        {
        //            byte[] bytes = sets.StringEncoding.GetBytes(value);
        //            if (_buffer.Length < position + bytes.Length + 6)
        //                Resize(bytes.Length + 6);
        //            Utils.WriteSize(_buffer, bytes.Length, ref position);
        //            Buffer.BlockCopy(bytes, 0, _buffer, position, bytes.Length);
        //            position += bytes.Length;
        //        }
        //    }
        //}

        //public void Write(Uri value)
        //{
        //    if (value == null)
        //        WriteNull();
        //    else if (value.AbsoluteUri.Length == 0)
        //        WriteZero();
        //    else
        //    {
        //        string uri = value.AbsoluteUri;
        //        if (_buffer.Length < position + uri.Length + 4)
        //            Resize(uri.Length + 4);
        //        Utils.WriteSize(_buffer, uri.Length, ref position);
        //        Utils.StringAsAscii(_buffer, position, uri);
        //        position += uri.Length;
        //    }
        //}

        //public void Write(object value)
        //{
        //    if (value == null)
        //    {
        //        if (_buffer.Length < position + 1)
        //            Resize(1);
        //        this._buffer[position] = NULL_FLAG;
        //        position++;
        //    }
        //    else
        //    {
        //        if (curDepth >= maxDepth)
        //            throw new Exception("序列化超出最大序列化深度！");
        //        curDepth++;
        //        sers[curObj++](this, value);
        //    }
        //}

        //public void Write(DateTime value)
        //{
        //    Write(value.Ticks);
        //}

        //public void Write(DateTimeOffset value)
        //{
        //    Write(value.Ticks);
        //}

        //public void Write(TimeSpan value)
        //{
        //    Write(value.Ticks);
        //}

        ///// <summary>
        ///// 实际写入16个字节长度的数据，需要一定的转换
        ///// </summary>
        ///// <param name="value"></param>
        //public unsafe void Write(Guid value)
        //{
        //    fixed (byte* ds = &_buffer[position])
        //    {
        //        byte* pd = ds;
        //        GuidStructSoc guid = new GuidStructSoc(value);
        //        int a = guid._a;
        //        short b = guid._b, c = guid._c;
        //        *((int*)pd) = *((int*)&a);
        //        *((short*)(pd + 4)) = *((short*)&b);
        //        *((short*)(pd + 6)) = *((short*)&c);
        //        pd += 8;

        //        *pd++ = guid._d;
        //        *pd++ = guid._e;
        //        *pd++ = guid._f;
        //        *pd++ = guid._g;
        //        *pd++ = guid._h;
        //        *pd++ = guid._i;
        //        *pd++ = guid._j;
        //        *pd++ = guid._k;
        //    }
        //    position += 16;
        //}

        //public void Write(Stream value)
        //{
        //    long len = value.Length;

        //}



        //public void WriteVInt(int value)
        //{
        //    uint v = (uint)((value << 1) ^ (value >> 31));
        //    WriteVInt(v);
        //}

        //public void WriteVInt(uint value)
        //{
        //    Utils.WriteVLong1(_buffer, value, ref position);
        //}

        //public void WriteVLong(long value)
        //{
        //    ulong v = (ulong)((value << 1) ^ (value >> 63));
        //    WriteVLong(v);
        //}

        //public void WriteVLong(ulong value)
        //{

        //}

        //public void WriteObject(object value)
        //{
        //    WriteNotSupported();
        //}

        //#endregion

        #endregion

        #region Write BaseType 无注释检查

        #region Write BaseType

        unsafe public void Write(int value)
        {
            //fixed (byte* pd = &_buffer[position])
            //    *((int*)pd) = *((int*)&value);
            //position += 4;

            *((int*)bp) = *((int*)&value);
            position += 4;
            bp += 4;
        }

        public void Write(bool value)
        {
            this._buffer[position++] = value ? ((byte)1) : ((byte)0);
        }

        public void Write(byte value)
        {
            this._buffer[position++] = value;
        }

        public void Write(sbyte value)
        {
            this._buffer[position++] = (byte)value;
        }

        unsafe public void Write(decimal value)
        {
            DecimalStruct v = new DecimalStruct(value);
            fixed (byte* pd = &_buffer[position])
            {
                *((int*)pd) = v.lo;
                *((int*)(pd + 4)) = v.mid;
                *((int*)(pd + 8)) = v.hi;
                *((int*)(pd + 12)) = v.flags;
            }
            position += 16;
        }

        unsafe public void Write(double value)
        {
            fixed (byte* pd = &_buffer[position])
                *((ulong*)pd) = *((ulong*)&value);

            position += 8;
        }

        public void Write(short value)
        {
            this._buffer[position] = (byte)value;
            this._buffer[position + 1] = (byte)(value >> 8);
            position += 2;
        }

        public void Write(ushort value)
        {
            this._buffer[position] = (byte)value;
            this._buffer[position + 1] = (byte)(value >> 8);
            position += 2;
        }

        unsafe public void Write(uint value)
        {
            fixed (byte* pd = &_buffer[position])
                *((int*)pd) = *((int*)&value);
            position += 4;
        }

        unsafe public void Write(long value)
        {
            fixed (byte* pd = &_buffer[position])
                *((long*)pd) = *((long*)&value);
            position += 8;
        }

        unsafe public void Write(ulong value)
        {
            fixed (byte* pd = &_buffer[position])
                *((ulong*)pd) = *((ulong*)&value);
            position += 8;
        }

        unsafe public void Write(float value)
        {
            fixed (byte* pd = &_buffer[position])
                *((uint*)pd) = *((uint*)&value);
            position += 4;
        }

        public void Write(char value)
        {
            this._buffer[position] = (byte)value;
            this._buffer[position + 1] = (byte)(value >> 8);
            position += 2;
        }

        /// <summary>
        /// 字符串的长度以固定四个字节写入
        /// </summary>
        /// <param name="value"></param>
        public void Write(string value)
        {
            if (value == null)
                WriteNull();
            else if (value.Length == 0)
                WriteZero();
            else
            {
                if (sets.StringEncoding == Encoding.Unicode)
                {
                    int size = (value.Length << 1);
                    if (_buffer.Length < position + size + 6)
                        Resize(size + 6);
                    Utils.WriteSize(_buffer, size, ref position);
                    Utils.StringAsUnicode(_buffer, position, value);
                    position += size;
                }
                else
                {
                    byte[] bytes = sets.StringEncoding.GetBytes(value);
                    if (_buffer.Length < position + bytes.Length + 6)
                        Resize(bytes.Length + 6);
                    Utils.WriteSize(_buffer, bytes.Length, ref position);
                    Buffer.BlockCopy(bytes, 0, _buffer, position, bytes.Length);
                    position += bytes.Length;
                }
            }
        }

        public void Write(Uri value)
        {
            if (value == null)
                WriteNull();
            else if (value.AbsoluteUri.Length == 0)
                WriteZero();
            else
            {
                string uri = value.AbsoluteUri;
                if (_buffer.Length < position + uri.Length + 4)
                    Resize(uri.Length + 4);
                Utils.WriteSize(_buffer, uri.Length, ref position);
                Utils.StringAsAscii(_buffer, position, uri);
                position += uri.Length;
            }
        }

        public void Write(object value)
        {
            if (value == null)
            {
                if (_buffer.Length < position + 1)
                    Resize(1);
                this._buffer[position] = NULL_FLAG;
                position++;
            }
            else
            {
                if (curDepth >= maxDepth)
                    throw new Exception("序列化超出最大序列化深度！");
                curDepth++;
                sers[curObj++](this, value);
            }
        }

        public void Write(DateTime value)
        {
            Write(value.Ticks);
        }

        public void Write(DateTimeOffset value)
        {
            Write(value.Ticks);
        }

        public void Write(TimeSpan value)
        {
            Write(value.Ticks);
        }

        /// <summary>
        /// 实际写入16个字节长度的数据，需要一定的转换
        /// </summary>
        /// <param name="value"></param>
        public unsafe void Write(Guid value)
        {
            fixed (byte* ds = &_buffer[position])
            {
                byte* pd = ds;
                GuidStructSoc guid = new GuidStructSoc(value);
                int a = guid._a;
                short b = guid._b, c = guid._c;
                *((int*)pd) = *((int*)&a);
                *((short*)(pd + 4)) = *((short*)&b);
                *((short*)(pd + 6)) = *((short*)&c);
                pd += 8;

                *pd++ = guid._d;
                *pd++ = guid._e;
                *pd++ = guid._f;
                *pd++ = guid._g;
                *pd++ = guid._h;
                *pd++ = guid._i;
                *pd++ = guid._j;
                *pd++ = guid._k;
            }
            position += 16;
        }

        public void Write(Stream value)
        {
            long len = value.Length;

        }

        public void WriteVInt(int value)
        {
            uint v = (uint)((value << 1) ^ (value >> 31));
            WriteVInt(v);
        }

        public void WriteVInt(uint value)
        {
            Utils.WriteVLong1(_buffer, value, ref position);
        }

        public void WriteVLong(long value)
        {
            ulong v = (ulong)((value << 1) ^ (value >> 63));
            WriteVLong(v);
        }

        public void WriteVLong(ulong value)
        {

        }

        public void WriteObject(object value)
        {
            WriteNotSupported();
        }

        #endregion

        #endregion

        #region Write IList

        public void Write(object[] value)
        {
            WriteNotSupported();
        }

        public void Write(List<object> value)
        {
            WriteNotSupported();
        }

        public void Write(IList<object> value)
        {
            WriteNotSupported();
        }

        public void Write(IEnumerable<object> value)
        {
            WriteNotSupported();
        }



        public void Write(bool[] value)
        {
            Write<bool>(value, 0);
        }

        public void Write(List<bool> value)
        {
            Write<bool>(value, 0);
        }

        public void Write(IList<bool> value)
        {
            Write<bool>(value, 0);
        }

        public void Write(IEnumerable<bool> value)
        {
            Write<bool>(value, 0);
        }



        public void Write(char[] value)
        {
            Write<char>(value, 1);
        }

        public void Write(List<char> value)
        {
            Write<char>(value, 1);
        }

        public void Write(IList<char> value)
        {
            Write<char>(value, 1);
        }

        public void Write(IEnumerable<char> value)
        {
            Write<char>(value, 1);
        }


        public void Write(sbyte[] value)
        {
            Write<sbyte>(value, 0);
        }

        public void Write(List<sbyte> value)
        {
            Write<sbyte>(value, 0);
        }

        public void Write(IList<sbyte> value)
        {
            Write<sbyte>(value, 0);
        }

        public void Write(IEnumerable<sbyte> value)
        {
            Write<sbyte>(value, 0);
        }



        public void Write(byte[] value)
        {
            Write<byte>(value, 0);
        }

        public void Write(List<byte> value)
        {
            Write<byte>(value, 0);
        }

        public void Write(IList<byte> value)
        {
            Write<byte>(value, 0);
        }

        public void Write(IEnumerable<byte> value)
        {
            Write<byte>(value, 0);
        }



        public void Write(short[] value)
        {
            Write<short>(value, 1);
        }

        public void Write(List<short> value)
        {
            Write<short>(value, 1);
        }

        public void Write(IList<short> value)
        {
            Write<short>(value, 1);
        }

        public void Write(IEnumerable<short> value)
        {
            Write<short>(value, 1);
        }



        public void Write(ushort[] value)
        {
            Write<ushort>(value, 1);
        }

        public void Write(List<ushort> value)
        {
            Write<ushort>(value, 1);
        }

        public void Write(IList<ushort> value)
        {
            Write<ushort>(value, 1);
        }

        public void Write(IEnumerable<ushort> value)
        {
            Write<ushort>(value, 1);
        }



        public void Write(int[] value)
        {
            Write<int>(value, 2);
        }

        public void Write(List<int> value)
        {
            Write<int>(value, 2);
        }

        public void Write(IList<int> value)
        {
            Write<int>(value, 2);
        }

        public void Write(IEnumerable<int> value)
        {
            Write<int>(value, 2);
        }



        public void Write(uint[] value)
        {
            Write<uint>(value, 2);
        }

        public void Write(List<uint> value)
        {
            Write<uint>(value, 2);
        }

        public void Write(IList<uint> value)
        {
            Write<uint>(value, 2);
        }

        public void Write(IEnumerable<uint> value)
        {
            Write<uint>(value, 2);
        }



        public void Write(long[] value)
        {
            Write<long>(value, 3);
        }

        public void Write(List<long> value)
        {
            Write<long>(value, 3);
        }

        public void Write(IList<long> value)
        {
            Write<long>(value, 3);
        }

        public void Write(IEnumerable<long> value)
        {
            Write<long>(value, 3);
        }



        public void Write(ulong[] value)
        {
            Write<ulong>(value, 3);
        }

        public void Write(List<ulong> value)
        {
            Write<ulong>(value, 3);
        }

        public void Write(IList<ulong> value)
        {
            Write<ulong>(value, 3);
        }

        public void Write(IEnumerable<ulong> value)
        {
            Write<ulong>(value, 3);
        }



        public void Write(float[] value)
        {
            Write<float>(value, 2);
        }

        public void Write(List<float> value)
        {
            Write<float>(value, 2);
        }

        public void Write(IList<float> value)
        {
            Write<float>(value, 2);
        }

        public void Write(IEnumerable<float> value)
        {
            Write<float>(value, 2);
        }



        public void Write(double[] value)
        {
            Write<double>(value, 3);
        }

        public void Write(List<double> value)
        {
            Write<double>(value, 3);
        }

        public void Write(IList<double> value)
        {
            Write<double>(value, 3);
        }

        public void Write(IEnumerable<double> value)
        {
            Write<double>(value, 3);
        }



        public void Write(decimal[] value)
        {
            Write<decimal>(value, 4);
        }

        public void Write(List<decimal> value)
        {
            Write<decimal>(value, 4);
        }

        public void Write(IList<decimal> value)
        {
            Write<decimal>(value, 4);
        }

        public void Write(IEnumerable<decimal> value)
        {
            Write<decimal>(value, 4);
        }


        public void Write(DateTime[] value)
        {
            if (value == null)
                WriteNull();
            else if (value.Length == 0)
                WriteZero();
            else
                InternalWrite(value, 0, value.Length);
        }

        public void Write(List<DateTime> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
                InternalWrite(value.ToArray(), 0, value.Count);
        }

        public void Write(IList<DateTime> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
                InternalWrite(value);
        }

        public void Write(IEnumerable<DateTime> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count() == 0)
                WriteZero();
            else
                InternalWrite(value);
        }




        public void Write(string[] value)
        {
            if (value == null)
                WriteNull();
            else if (value.Length == 0)
                WriteZero();
            else
                InternalWrite(value,0,value.Length);
        }

        public void Write(List<string> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
                InternalWrite(value.ToArray(), 0, value.Count);
        }

        public void Write(IList<string> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
                InternalWrite(value);
        }

        public void Write(IEnumerable<string> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count() == 0)
                WriteZero();
            else
                InternalWrite(value);
        }


        public void Write(Guid[] value)
        {
            Write<Guid>(value, 4);
        }

        public void Write(List<Guid> value)
        {
            Write<Guid>(value, 4);
        }

        public void Write(IList<Guid> value)
        {
            Write<Guid>(value, 4);
        }

        public void Write(IEnumerable<Guid> value)
        {
            Write<Guid>(value, 4);
        }

        public void Write(byte[][] value)
        {
            WriteArray<byte[]>(value, 0);
        }

        public void Write(List<byte[]> value)
        {
            WriteArray<byte[]>(value, 0);
        }

        public void Write(IList<byte[]> value)
        {
            WriteArray<byte[]>(value, 0);
        }

        public void Write(IEnumerable<byte[]> value)
        {
            WriteArray<byte[]>(value, 0);
        }

        #endregion

        #region Write IDictionary

        public void Write(IDictionary<int, object> value)
        {
            WriteNotSupported();
        }

        public void Write(IDictionary<int, bool> value)
        {
            if (!WriteNullOrEmpty<int, bool>(value))
            {
                Resize(value.Count * 5 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<bool>(value.Values.ToArray(), 0);

                //Resize(value.Count * 5 + 4);
                //WriteSize(value.Count);
                //foreach (KeyValuePair<int, bool> item in value)
                //{
                //    Write(item.Key);
                //    Write(item.Value);
                //}
            }
        }

        public void Write(IDictionary<int, char> value)
        {
            if (!WriteNullOrEmpty<int, char>(value))
            {
                Resize(value.Count * 6 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<char>(value.Values.ToArray(), 1);
            }
        }

        public void Write(IDictionary<int, sbyte> value)
        {
            if (!WriteNullOrEmpty<int, sbyte>(value))
            {
                Resize(value.Count * 5 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<sbyte>(value.Values.ToArray(), 0);
            }
        }

        public void Write(IDictionary<int, byte> value)
        {
            if (!WriteNullOrEmpty<int, byte>(value))
            {
                Resize(value.Count * 5 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<byte>(value.Values.ToArray(), 0);
            }
        }

        public void Write(IDictionary<int, short> value)
        {
            if (!WriteNullOrEmpty<int, short>(value))
            {
                Resize(value.Count * 6 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<short>(value.Values.ToArray(), 1);
            }
        }

        public void Write(IDictionary<int, ushort> value)
        {
            if (!WriteNullOrEmpty<int, ushort>(value))
            {
                Resize(value.Count * 6 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<ushort>(value.Values.ToArray(), 1);
            }
        }

        public void Write(IDictionary<int, int> value)
        {
            if (!WriteNullOrEmpty<int, int>(value))
            {
                Resize(value.Count * 8 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<int>(value.Values.ToArray(), 2);
            }
        }

        public void Write(IDictionary<int, uint> value)
        {
            if (!WriteNullOrEmpty<int, uint>(value))
            {
                Resize(value.Count * 8 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<uint>(value.Values.ToArray(), 2);
            }
        }

        public void Write(IDictionary<int, long> value)
        {
            if (!WriteNullOrEmpty<int, long>(value))
            {
                Resize(value.Count * 12 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<long>(value.Values.ToArray(), 3);
            }
        }

        public void Write(IDictionary<int, ulong> value)
        {
            if (!WriteNullOrEmpty<int, ulong>(value))
            {
                Resize(value.Count * 12 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<ulong>(value.Values.ToArray(), 3);
            }
        }

        public void Write(IDictionary<int, float> value)
        {
            if (!WriteNullOrEmpty<int, float>(value))
            {
                Resize(value.Count * 8 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<float>(value.Values.ToArray(), 2);
            }
        }

        public void Write(IDictionary<int, double> value)
        {
            if (!WriteNullOrEmpty<int, double>(value))
            {
                Resize(value.Count * 12 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<double>(value.Values.ToArray(), 3);
            }
        }

        public void Write(IDictionary<int, decimal> value)
        {
            if (!WriteNullOrEmpty<int, decimal>(value))
            {
                Resize(value.Count * 20 + 4);
                WriteSize(value.Count);
                WriteKeyValue<int>(value.Keys.ToArray(), 2);
                WriteKeyValue<decimal>(value.Values.ToArray(), 4);
            }
        }

        public void Write(IDictionary<int, DateTime> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 12 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<int, DateTime> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<int, string> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 4 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<int, string> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }




        public void Write(IDictionary<string, object> value)
        {
            WriteNotSupported();
        }

        public void Write(IDictionary<string, bool> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 2 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, bool> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, char> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 3 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, char> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, sbyte> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 2 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, sbyte> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, byte> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 2 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, byte> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, short> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 3 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, short> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, ushort> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 3 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, ushort> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, int> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 5 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, int> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, uint> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 5 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, uint> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, long> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 9 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, long> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, ulong> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 9 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, ulong> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, float> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 5 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, float> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, double> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 9 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, double> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, decimal> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 17 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, decimal> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, DateTime> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 9 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, DateTime> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<string, string> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 2 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<string, string> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }




        public void Write(IDictionary<long, object> value)
        {
            WriteNotSupported();
        }

        public void Write(IDictionary<long, bool> value)
        {
            if (!WriteNullOrEmpty<long, bool>(value))
            {
                Resize(value.Count * 9 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<bool>(value.Values.ToArray(), 0);
            }
        }

        public void Write(IDictionary<long, char> value)
        {
            if (!WriteNullOrEmpty<long, char>(value))
            {
                Resize(value.Count * 10 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<char>(value.Values.ToArray(), 1);
            }
        }

        public void Write(IDictionary<long, sbyte> value)
        {
            if (!WriteNullOrEmpty<long, sbyte>(value))
            {
                Resize(value.Count * 9 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<sbyte>(value.Values.ToArray(), 0);
            }
        }

        public void Write(IDictionary<long, byte> value)
        {
            if (!WriteNullOrEmpty<long, byte>(value))
            {
                Resize(value.Count * 9 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<byte>(value.Values.ToArray(), 0);
            }
        }

        public void Write(IDictionary<long, short> value)
        {
            if (!WriteNullOrEmpty<long, short>(value))
            {
                Resize(value.Count * 10 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<short>(value.Values.ToArray(), 1);
            }
        }

        public void Write(IDictionary<long, ushort> value)
        {
            if (!WriteNullOrEmpty<long, ushort>(value))
            {
                Resize(value.Count * 10 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<ushort>(value.Values.ToArray(), 1);
            }
        }

        public void Write(IDictionary<long, int> value)
        {
            if (!WriteNullOrEmpty<long, int>(value))
            {
                Resize(value.Count * 12 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<int>(value.Values.ToArray(), 2);
            }
        }

        public void Write(IDictionary<long, uint> value)
        {
            if (!WriteNullOrEmpty<long, uint>(value))
            {
                Resize(value.Count * 12 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<uint>(value.Values.ToArray(), 2);
            }
        }

        public void Write(IDictionary<long, long> value)
        {
            if (!WriteNullOrEmpty<long, long>(value))
            {
                Resize(value.Count * 16 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<long>(value.Values.ToArray(), 3);
            }
        }

        public void Write(IDictionary<long, ulong> value)
        {
            if (!WriteNullOrEmpty<long, ulong>(value))
            {
                Resize(value.Count * 16 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<ulong>(value.Values.ToArray(), 3);
            }
        }

        public void Write(IDictionary<long, float> value)
        {
            if (!WriteNullOrEmpty<long, float>(value))
            {
                Resize(value.Count * 12 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<float>(value.Values.ToArray(), 2);
            }
        }

        public void Write(IDictionary<long, double> value)
        {
            if (!WriteNullOrEmpty<long, double>(value))
            {
                Resize(value.Count * 16 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<double>(value.Values.ToArray(), 3);
            }
        }

        public void Write(IDictionary<long, decimal> value)
        {
            if (!WriteNullOrEmpty<long, decimal>(value))
            {
                Resize(value.Count * 24 + 4);
                WriteSize(value.Count);
                WriteKeyValue<long>(value.Keys.ToArray(), 3);
                WriteKeyValue<decimal>(value.Values.ToArray(), 4);
            }
        }

        public void Write(IDictionary<long, DateTime> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 16 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<long, DateTime> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        public void Write(IDictionary<long, string> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZero();
            else
            {
                Resize(value.Count * 8 + 4);
                WriteSize(value.Count);
                foreach (KeyValuePair<long, string> item in value)
                {
                    Write(item.Key);
                    Write(item.Value);
                }
            }
        }

        #endregion

        #region ArraySegment

        public void Write(ArraySegment<bool> value)
        {
            Write<bool>(value, 0);
        }

        public void Write(ArraySegment<char> value)
        {
            Write<char>(value, 1);
        }

        public void Write(ArraySegment<sbyte> value)
        {
            Write<sbyte>(value, 0);
        }

        public void Write(ArraySegment<byte> value)
        {
            Write<byte>(value, 0);
        }

        public void Write(ArraySegment<short> value)
        {
            Write<short>(value, 1);
        }

        public void Write(ArraySegment<ushort> value)
        {
            Write<ushort>(value, 1);
        }

        public void Write(ArraySegment<int> value)
        {
            Write<int>(value, 2);
        }

        public void Write(ArraySegment<uint> value)
        {
            Write<uint>(value, 2);
        }

        public void Write(ArraySegment<long> value)
        {
            Write<long>(value, 3);
        }

        public void Write(ArraySegment<ulong> value)
        {
            Write<ulong>(value, 3);
        }

        public void Write(ArraySegment<float> value)
        {
            Write<float>(value, 2);
        }

        public void Write(ArraySegment<double> value)
        {
            Write<double>(value, 3);
        }

        public void Write(ArraySegment<decimal> value)
        {
            Write<decimal>(value, 4);
        }

        public void Write(ArraySegment<DateTime> value)
        {
            if (value.Count == 0)
                WriteZero();
            else
                InternalWrite(value.Array, value.Offset, value.Count);
        }

        public void Write(ArraySegment<string> value)
        {
            if (value.Count == 0)
                WriteZero();
            else
                InternalWrite(value.Array, value.Offset, value.Count);
        }

        #endregion

        #region Write Other

        public void Write(IList value)
        {
            
        }

        public void Write(IDictionary value)
        {
            
        }

        public void Write(IEnumerable value)
        {
            
        }

        internal void WriteNotSupported()
        {
            throw new NotSupportedException("not supported object type!");
        }

        #endregion

        #region 公共

        public ArraySegment<byte> ToArraySegment()
        {
            return new ArraySegment<byte>(_buffer, 0, position);
        }

        public void WriteTo(Stream stream)
        {
            stream.Write(_buffer, 0, position);
        }

        public byte[] GetBuffer()
        {
            return _buffer;
        }

        public byte[] ToArray()
        {
            //如果数据的长度和实际长度一样，直接返回
            if (_buffer.Length == position)
                return _buffer;
            byte[] temp = new byte[position];
            Buffer.BlockCopy(_buffer, 0, temp, 0, position);
            return temp;
        }

        #endregion
        
    }
}
