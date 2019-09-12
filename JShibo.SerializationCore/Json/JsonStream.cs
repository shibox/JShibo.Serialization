//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using System.Text;
//using JShibo.Serialization.Common;

//namespace JShibo.Serialization.Json
//{
//    /// <summary>
//    /// 使用流式模式序列化成Json，可以直接输出到前端的流，避免在输出到前端的时候，需要再次将
//    /// 字符串转换成流输出，降低性能。
//    /// sbyte / byte / short / ushort /int / uint / long / ulong
//    /// char / float / double / bool /decimal /object / string
//    /// </summary>
//    public class JsonStream
//    {
//        #region 常量

//        const byte OBJECT_START = (byte)'{';
//        const byte OBJECT_END = (byte)'}';
//        const byte ARRAY_START = (byte)'[';
//        const byte ARRAY_END = (byte)']';
//        const byte QUOTE = (byte)'"';
//        const byte COLON = (byte)':';
//        const byte COMMA = (byte)',';
//        const byte ZERO_FLAG = 0x02;


//        const byte BYTE_LOWER_N = (byte)'n';
//        const byte BYTE_LOWER_U = (byte)'u';
//        const byte BYTE_LOWER_L = (byte)'l';

//        const byte BYTE_UPPER_N = (byte)'N';
//        const byte BYTE_UPPER_U = (byte)'U';
//        const byte BYTE_UPPER_L = (byte)'L';

//        const byte BYTE_LOWER_T = (byte)'t';
//        const byte BYTE_LOWER_R = (byte)'r';
//        const byte BYTE_LOWER_E = (byte)'e';

//        const byte BYTE_LOWER_F = (byte)'f';
//        const byte BYTE_LOWER_A = (byte)'a';
//        const byte BYTE_LOWER_S = (byte)'s';

//        const byte BYTE_UPPER_T = (byte)'T';
//        const byte BYTE_UPPER_R = (byte)'R';
//        const byte BYTE_UPPER_E = (byte)'E';

//        const byte BYTE_UPPER_F = (byte)'F';
//        const byte BYTE_UPPER_A = (byte)'A';
//        const byte BYTE_UPPER_S = (byte)'S';
        
        


//        /// <summary>
//        /// 用于切换使用单引号还是双引号写入，该方式用于减少判断，而使用字段访问
//        /// </summary>

//        #endregion

//        #region 字段

//        internal Type[] types;
//        internal Serialize<JsonStream>[] sers;
//        //internal Deserialize<JsonStream>[] desers;
//        internal int[] typeCounts;
//        internal int[] nameCounts;

//        internal byte[] namesBytes;
//        //unsafe internal byte* namesBytesP;
//        internal int[] nameLens;
//        internal int cnamepos = 0;

//        internal string[] names;
//        internal byte[] _buffer = null;
//        internal int position = 0;
//        internal int current = 0;
//        internal int currSer = 0;
//        internal int maxDepth = 10;
//        internal int curDepth = 0;
//        //internal bool unFlag = false;
//        internal SerializerSettings sets = SerializerSettings.Default;

//        #endregion

//        #region 属性

//        public int Position
//        {
//            get 
//            {
//                return position;
//            }
//            set { position = value; }
//        }

//        public int MaxDepth
//        {
//            get { return maxDepth; }
//            set { maxDepth = value; }
//        }

//        internal unsafe  byte[] NamesBytes
//        {
//            set { namesBytes = value; }
//        }

//        #endregion

//        #region 构造函数

//        public JsonStream()
//            :this(64)
//        {
//        }

//        public JsonStream(int capacity)
//            :this(capacity,new SerializerSettings())
//        {
//        }

//        public JsonStream(byte[] buffer)
//            :this(buffer,new SerializerSettings())
//        {
//        }

//        public JsonStream(ref byte[] buffer)
//            :this(ref buffer,new SerializerSettings())
//        {
//        }

//        public JsonStream(byte[] buffer, SerializerSettings set)
//        {
//            _buffer = buffer;
//            sets = set;
//        }

//        public JsonStream(ref byte[] buffer, SerializerSettings set)
//        {
//            _buffer = buffer;
//            sets = set;
//        }

//        public JsonStream(int capacity,SerializerSettings set)
//        {
//            _buffer = new byte[capacity];
//            sets = set;
//        }

//        #endregion

//        #region 方法

//        private void Resize(int size)
//        {
//            if (_buffer.Length < position + size)
//            {
//                if (size > _buffer.Length)
//                {
//                    byte[] temp = new byte[_buffer.Length + size];
//                    Buffer.BlockCopy(_buffer, 0, temp, 0, position);
//                    _buffer = temp;
//                }
//                else
//                {
//                    byte[] temp = new byte[_buffer.Length * 2];
//                    Buffer.BlockCopy(_buffer, 0, temp, 0, position);
//                    _buffer = temp;
//                }
//            }
//        }

//        private void ResizeTo(int size)
//        {
//            if (size > _buffer.Length)
//            {
//                byte[] temp = new byte[_buffer.Length + size];
//                Buffer.BlockCopy(_buffer, 0, temp, 0, position);
//                _buffer = temp;
//            }
//            else
//            {
//                byte[] temp = new byte[_buffer.Length * 2];
//                Buffer.BlockCopy(_buffer, 0, temp, 0, position);
//                _buffer = temp;
//            }
//        }

//        private unsafe void ResizeAndWriteName(int size)
//        {
//            #region old

//            //string name = names[current];
//            //if (_buffer.Length < position + size + name.Length)
//            //{
//            //    if (size + name.Length > _buffer.Length)
//            //    {
//            //        byte[] temp = new byte[_buffer.Length + size];
//            //        Buffer.BlockCopy(_buffer, 0, temp, 0, position);
//            //        _buffer = temp;
//            //    }
//            //    else
//            //    {
//            //        byte[] temp = new byte[_buffer.Length * 2];
//            //        Buffer.BlockCopy(_buffer, 0, temp, 0, position);
//            //        _buffer = temp;
//            //    }
//            //}

//            //_buffer[position] = QUOTE;
//            //position++;
//            //for (int i = 0; i < name.Length; i++)
//            //    _buffer[position + i] = (byte)name[i];
//            //position += name.Length;
//            //_buffer[position] = QUOTE;
//            //position++;
//            //_buffer[position] = COLON;
//            //position++;
//            //current++;

//            #endregion

//            //if (unFlag == false)
//            //{
//            //    int len = nameLens[current];
//            //    if (_buffer.Length < position + size + len)
//            //        ResizeTo(size + len + 4);

//            //    _buffer[position] = QUOTE;
//            //    position++;
//            //    for (int i = 0; i < len; i++)
//            //        _buffer[position + i] = (byte)namesBytes[cnamepos + i];
//            //    cnamepos += len;
//            //    position += len;
//            //    _buffer[position] = QUOTE;
//            //    position++;
//            //    _buffer[position] = COLON;
//            //    position++;
//            //    current++;
//            //}
//            //else
//            //{
//            //    if (_buffer.Length < position + size)
//            //        ResizeTo(size);
//            //}




//            if (names.Length > 0)
//            {
//                string name = names[current];
//                if (_buffer.Length < position + size + name.Length)
//                    ResizeTo(size + name.Length + 4);

//                _buffer[position] = QUOTE;
//                position++;
//                if (sets.CamelCase && (name[0] > 96 && name[0] < 123))
//                {
//                    _buffer[position] = (byte)(name[0] - 32);
//                    for (int i = 1; i < name.Length; i++)
//                        _buffer[position + i] = (byte)name[i];
//                }
//                else
//                {
//                    //FastWriteName.Write(name, _buffer, ref position);
//                    //FastWriteName.Write(name, _buffer, position);
//                    for (int i = 0; i < name.Length; i++)
//                        _buffer[position + i] = (byte)name[i];
//                }
//                position += name.Length;

//                _buffer[position] = QUOTE;
//                position++;
//                _buffer[position] = COLON;
//                position++;
//                current++;
//            }
//            else
//            {
//                if (_buffer.Length < position + size)
//                    ResizeTo(size);
//            }




//            //if (unFlag == false)
//            //{
//            //    string name = names[current];
//            //    if (_buffer.Length < position + size + name.Length)
//            //        ResizeTo(size + name.Length + 4);

//            //    _buffer[position] = QUOTE;
//            //    position++;
//            //    for (int i = 0; i < name.Length; i++)
//            //        _buffer[position + i] = (byte)name[i];
//            //    position += name.Length;
//            //    _buffer[position] = QUOTE;
//            //    position++;
//            //    _buffer[position] = COLON;
//            //    position++;
//            //    current++;
//            //}
//            //else
//            //{
//            //    if (_buffer.Length < position + size)
//            //        ResizeTo(size);
//            //}



//            //int len = nameLens[current];
//            //if (_buffer.Length < position + size + len)
//            //{
//            //    if (size + len > _buffer.Length)
//            //    {
//            //        byte[] temp = new byte[_buffer.Length + size];
//            //        Buffer.BlockCopy(_buffer, 0, temp, 0, position);
//            //        _buffer = temp;
//            //    }
//            //    else
//            //    {
//            //        byte[] temp = new byte[_buffer.Length * 2];
//            //        Buffer.BlockCopy(_buffer, 0, temp, 0, position);
//            //        _buffer = temp;
//            //    }
//            //}

//            //_buffer[position] = QUOTE;
//            //position++;
//            //for (int i = 0; i < len; i++)
//            //    _buffer[position + i] = (byte)*namesBytesP++;
//            ////cnamepos += len;
//            //position += len;
//            //_buffer[position] = QUOTE;
//            //position++;
//            //_buffer[position] = COLON;
//            //position++;
//            //current++;

//        }

//        private bool CheckAndSkipName()
//        {
//            if (names.Length > 0)
//            {
//                string name = names[current];
//                position++;
//                for (int i = 0; i < name.Length; i++)
//                {
//                    if (name[i] != _buffer[position + i])
//                        return false;
//                }
//                position += name.Length;
//                position+=2;
//                current++;
//                return true;
//            }
//            return false;
//        }

//        //internal void Serialize(object graph, JsonTypeInfo info)
//        //{
//        //    if (unFlag == false)
//        //        _buffer[position++] = OBJECT_START;

//        //    sers = info.Streams.ToArray();
//        //    types = info.Types.ToArray();
//        //    typeCounts = info.TypeCounts.ToArray();
//        //    nameCounts = info.NameCounts.ToArray();
//        //    names = info.Names.ToArray();

//        //    namesBytes = info.namesBytes;
//        //    nameLens = info.nameLens.ToArray();

//        //    info.Ser(this, graph);
//        //    if (unFlag == false)
//        //    {
//        //        if (_buffer[position - 1] == COMMA)
//        //        {
//        //            position--;
//        //            _buffer[position++] = OBJECT_END;
//        //        }
//        //        else
//        //            _buffer[position++] = OBJECT_END;
//        //    }
//        //}

//        //internal void SetPosition(ref int pos)
//        //{
//        //    this.position = pos;
//        //}

//        //internal void SetBuffer(ref byte[] buffer)
//        //{
//        //    this._buffer = buffer;
//        //}

//        #endregion

//        #region 私有写入

//        private void WriteNull()
//        {
//            if (sets.NullValueIgnore)
//                current++;
//            else
//            {
//                ResizeAndWriteName(SizeConsts.NULL_SIZE);
//                _buffer[position] = BYTE_LOWER_N;
//                _buffer[position + 1] = BYTE_LOWER_U;
//                _buffer[position + 2] = BYTE_LOWER_L;
//                _buffer[position + 3] = BYTE_LOWER_L;
//                _buffer[position + 4] = COMMA;
//                position += 5;
//            }
//        }

//        private void WriteZeroArray()
//        {
//            ResizeAndWriteName(SizeConsts.ZERO_ARRAY_SIZE);
//            _buffer[position] = ARRAY_START;
//            _buffer[position + 1] = ARRAY_END;
//            _buffer[position + 2] = COMMA;
//            position += 3;
//        }

//        internal void WriteNullWithoutName()
//        {
//            Resize(10);
//            _buffer[position] = BYTE_LOWER_N;
//            _buffer[position + 1] = BYTE_LOWER_U;
//            _buffer[position + 2] = BYTE_LOWER_L;
//            _buffer[position + 3] = BYTE_LOWER_L;
//            _buffer[position + 4] = COMMA;
//            position += 5;
//        }

//        private void WriteZeroArrayWithoutName()
//        {
//            Resize(10);
//            _buffer[position] = ARRAY_START;
//            _buffer[position + 1] = ARRAY_END;
//            position += 2;
//        }

//        private void WriteZeroObject()
//        {
//            ResizeAndWriteName(10);
//            _buffer[position] = OBJECT_START;
//            _buffer[position + 1] = OBJECT_END;
//            _buffer[position + 2] = COMMA;
//            position += 3;
//        }

//        private void WriteZeroObjectWithoutName()
//        {
//            Resize(10);
//            _buffer[position] = OBJECT_START;
//            _buffer[position + 1] = OBJECT_END;
//            position += 2;
//        }

//        private void WriteZeroString()
//        {
//            ResizeAndWriteName(10);
//            _buffer[position] = QUOTE;
//            _buffer[position + 1] = QUOTE;
//            _buffer[position + 2] = COMMA;
//            position += 3;
//        }

//        private void WriteZeroStringWithoutName()
//        {
//            Resize(10);
//            _buffer[position] = QUOTE;
//            _buffer[position + 1] = QUOTE;
//            _buffer[position + 2] = COMMA;
//            position += 3;
//        }

//        private void WriteTab()
//        {
//            _buffer[position] = (byte)'\r';
//            _buffer[position + 1] = (byte)'\n';
//            _buffer[position + 2] = (byte)' ';
//            _buffer[position + 3] = (byte)' ';
//            position += 4;
//        }



//        private void InternalWrite(byte value)
//        {
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        private void InternalWrite(sbyte value)
//        {
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        private void InternalWrite(short value)
//        {
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        private void InternalWrite(ushort value)
//        {
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        private void InternalWrite(int value)
//        {
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        private void InternalWrite(uint value)
//        {
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        private void InternalWrite(long value)
//        {
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        private void InternalWrite(ulong value)
//        {
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        private void InternalWrite(float value)
//        {
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        private void InternalWrite(double value)
//        {
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        private void InternalWrite(decimal value)
//        {
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        private void InternalWrite(string value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Length == 0)
//                WriteZeroStringWithoutName();
//            else
//            {
//                Resize((value.Length * 3) + 10);
//                _buffer[position] = QUOTE;
//                int len = sets.StringEncoding.GetBytes(value, 0, value.Length, _buffer, position + 1);
//                position += len + 1;
//                _buffer[position] = QUOTE;
//                _buffer[position + 1] = COMMA;
//                position += 2;
//            }
//        }

//        private void InternalWrite(bool value)
//        {
//            if (value)
//            {
//                _buffer[position] = BYTE_LOWER_T;
//                _buffer[position + 1] = BYTE_LOWER_R;
//                _buffer[position + 2] = BYTE_LOWER_U;
//                _buffer[position + 3] = BYTE_LOWER_E;
//                _buffer[position + 4] = COMMA;
//                position += 5;
//            }
//            else
//            {
//                _buffer[position] = BYTE_LOWER_F;
//                _buffer[position + 1] = BYTE_LOWER_A;
//                _buffer[position + 2] = BYTE_LOWER_L;
//                _buffer[position + 3] = BYTE_LOWER_S;
//                _buffer[position + 4] = BYTE_LOWER_E;
//                _buffer[position + 5] = COMMA;
//                position += 6;
//            }
//        }

//        private void InternalWrite(char value)
//        {
//            _buffer[position] = QUOTE;
//            int len = sets.StringEncoding.GetBytes(new char[] { value }, 0, 1, _buffer, position + 1);
//            _buffer[position + len + 1] = QUOTE;
//            position += len + 2;
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        private unsafe void InternalWrite(DateTime value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = QUOTE;
//                p = FastToString.ToString(p, ref position, value);
//                *p++ = QUOTE;
//                *p++ = COMMA;
//                position += 3;
//            }
//        }

//        private void InternalWrite(Guid value)
//        {
//            string v = value.ToString();
//            this._buffer[position] = QUOTE;
//            position++;
//            for (int i = 0; i < v.Length; i++)
//                _buffer[position + i] = (byte)v[i];
//            position += v.Length;
//            this._buffer[position] = QUOTE;
//            this._buffer[position + 1] = COMMA;
//            position += 2;
//        }

//        private unsafe void InternalWrite(Uri value)
//        {
//            string v = value.AbsoluteUri;
//            this._buffer[position] = QUOTE;
//            position++;
//            for (int i = 0; i < v.Length; i++)
//                _buffer[position + i] = (byte)v[i];
//            position += v.Length;
//            this._buffer[position] = QUOTE;
//            this._buffer[position + 1] = COMMA;
//            position += 2;
//        }

//        private unsafe byte* InternalWrite(byte* buffer, ref int pos, string value)
//        {
//            if (value == null)
//            {
//                Resize(10);
//                *buffer++ = BYTE_LOWER_N;
//                *buffer++ = BYTE_LOWER_U;
//                *buffer++ = BYTE_LOWER_L;
//                *buffer++ = BYTE_LOWER_L;
//                *buffer++ = COMMA;
//                pos += 5;
//                return buffer;
//            }
//            else if (value.Length == 0)
//            {
//                *buffer++ = QUOTE;
//                *buffer++ = QUOTE;
//                *buffer++ = COMMA;
//                position += 3;
//            }
//            else
//            {
//                *buffer++ = QUOTE;
//                fixed (char* chs = value)
//                {
//                    int len = sets.StringEncoding.GetBytes(chs, value.Length, buffer, _buffer.Length);
//                    pos += len + 3;
//                }
//                *buffer++ = QUOTE;
//                *buffer++ = COMMA;
//            }
//            return buffer;
//        }



//        private unsafe void InternalWrite(bool[] value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Length - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        //private unsafe void InternalWrite(char[] value)
//        //{
//        //    fixed (byte* pd = &_buffer[position])
//        //    {
//        //        byte* p = pd;
//        //        *p++ = ARRAY_START;
//        //        position++;
//        //        int len = value.Length - 1;
//        //        for (int i = 0; i < len; i++)
//        //        {
//        //            p = FastToString.ToString(p, ref position, value[i]);
//        //            *p++ = COMMA;
//        //            position++;
//        //        }
//        //        p = FastToString.ToString(p, ref position, value[len]);
//        //        *p++ = ARRAY_END;
//        //        *p = COMMA;
//        //        position += 2;
//        //    }
//        //}

//        private unsafe void InternalWrite(short[] value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Length - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(ushort[] value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Length - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(int[] value)
//        {
//            //fixed (byte* pd = &_buffer[position])
//            //{
//            //    fixed (int* pdint = &value[0])
//            //    {
//            //        byte* p = pd;
//            //        int* pint=pdint;
//            //        *p++ = ARRAY_START;
//            //        position++;
//            //        int len = value.Length - 1;
//            //        for (int i = 0; i < len; i++)
//            //        {
//            //            p = FastToString.ToString(p, ref position, *pint++);
//            //            *p++ = COMMA;
//            //            position++;
//            //        }
//            //        p = FastToString.ToString(p, ref position, value[len]);
//            //        *p++ = ARRAY_END;
//            //        *p = COMMA;
//            //        position += 2;
//            //    }

//            //    //byte* p = pd;
//            //    //*p++ = ARRAY_START;
//            //    //position++;
//            //    //int len = value.Length - 1;
//            //    //for (int i = 0; i < len; i++)
//            //    //{
//            //    //    p = FastToString.ToString(p, ref position, value[i]);
//            //    //    *p++ = COMMA;
//            //    //    position++;
//            //    //}
//            //    //p = FastToString.ToString(p, ref position, value[len]);
//            //    //*p++ = ARRAY_END;
//            //    //*p = COMMA;
//            //    //position += 2;
//            //}


//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Length - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }


//            //ResizeAndWriteName(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//            //_buffer[position] = ARRAY_START;
//            //position++;
//            //fixed (byte* pd = &_buffer[position])
//            //{
//            //    byte* p = pd;
//            //    int len = 0;
//            //    for (int i = 0; i < value.Length; i++)
//            //    {
//            //        len = FastToString.ToString(p, position, value[i]);
//            //        position += len + 1;
//            //        p += len;
//            //        *p++ = COMMA;
//            //    }
//            //}
//            //_buffer[position - 1] = ARRAY_END;
//            //_buffer[position] = COMMA;
//            //position++;
//        }

//        private unsafe void InternalWrite(uint[] value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Length - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(long[] value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Length - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(ulong[] value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Length - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(float[] value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Length - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(double[] value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Length - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(decimal[] value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Length - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(string[] value)
//        {
//            //fixed (byte* pd = &_buffer[position])
//            //{
//            //    byte* p = pd;
//            //    *p++ = ARRAY_START;
//            //    position++;
//            //    int len = value.Length - 1;
//            //    for (int i = 0; i < len; i++)
//            //    {
//            //        p = FastToString.ToString(p, ref position, value[i]);
//            //        *p++ = COMMA;
//            //        position++;
//            //    }
//            //    p = FastToString.ToString(p, ref position, value[len]);
//            //    *p++ = ARRAY_END;
//            //    *p = COMMA;
//            //    position += 2;
//            //}

//            _buffer[position] = ARRAY_START;
//            position++;
//            foreach (string v in value)
//            {
//                Resize(value.Length * 3 + SizeConsts.STRING_BASE_SIZE);
//                InternalWrite(v);
//            }
//            _buffer[position - 1] = ARRAY_END;
//            _buffer[position] = COMMA;
//            position++;
//        }

//        private unsafe void InternalWrite(DateTime[] value)
//        {
//            ResizeAndWriteName(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//            _buffer[position] = ARRAY_START;
//            position++;
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                for (int i = 0; i < value.Length; i++)
//                {
//                    p = FastToString.ToString(p,ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//            }
//            _buffer[position - 1] = ARRAY_END;
//            _buffer[position] = COMMA;
//            position++;
//        }

//        private unsafe void InternalWrite(TimeSpan[] value)
//        {
//            ResizeAndWriteName(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//            _buffer[position] = ARRAY_START;
//            position++;
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                int len = 0;
//                for (int i = 0; i < value.Length; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//            }
//            _buffer[position - 1] = ARRAY_END;
//            _buffer[position] = COMMA;
//            position++;
//        }



//        private unsafe void InternalWrite(IList<bool> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Count - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(IList<short> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Count - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(IList<ushort> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Count - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(IList<int> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Count - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(IList<uint> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Count - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(IList<long> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Count - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(IList<ulong> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Count - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(IList<float> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Count - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(IList<double> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Count - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(IList<decimal> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                int len = value.Count - 1;
//                for (int i = 0; i < len; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//                p = FastToString.ToString(p, ref position, value[len]);
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position += 2;
//            }
//        }

//        private unsafe void InternalWrite(IList<DateTime> value)
//        {
//            _buffer[position] = ARRAY_START;
//            position++;
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                int count = value.Count;
//                for (int i = 0; i < count; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//            }
//            _buffer[position - 1] = ARRAY_END;
//            _buffer[position] = COMMA;
//            position++;
//        }

//        private unsafe void InternalWrite(IList<TimeSpan> value)
//        {
//            _buffer[position] = ARRAY_START;
//            position++;
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                int count = value.Count;
//                for (int i = 0; i < count; i++)
//                {
//                    p = FastToString.ToString(p, ref position, value[i]);
//                    *p++ = COMMA;
//                    position++;
//                }
//            }
//            _buffer[position - 1] = ARRAY_END;
//            _buffer[position] = COMMA;
//            position++;
//        }




//        private unsafe void InternalWrite(IEnumerable<bool> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (bool v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<char> value)
//        {
//            _buffer[position] = ARRAY_START;
//            position++;
//            foreach (char v in value)
//                InternalWrite(v);
//            _buffer[position - 1] = ARRAY_END;
//            _buffer[position] = COMMA;
//            position++;
//        }

//        private unsafe void InternalWrite(IEnumerable<byte> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (byte v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<sbyte> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (sbyte v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<short> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (short v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<ushort> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (ushort v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<int> value)
//        {
//            //_buffer[position] = ARRAY_START;
//            //position++;
//            //foreach (int v in value)
//            //    InternalWrite(v);
//            //_buffer[position - 1] = ARRAY_END;
//            //_buffer[position] = COMMA;
//            //position++;



//            //_buffer[position] = ARRAY_START;
//            //position++;
//            //fixed (byte* pd = &_buffer[position])
//            //{
//            //    byte* p = pd;
//            //    int len = 0;
//            //    foreach (int v in value)
//            //    {
//            //        len = FastToString.ToString(p, position, v);
//            //        position += len + 1;
//            //        p += len;
//            //        *p++ = COMMA;
//            //    }
//            //}
//            //_buffer[position - 1] = ARRAY_END;
//            //_buffer[position] = COMMA;
//            //position++;


//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (int v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<uint> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (uint v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<long> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (long v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<ulong> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (ulong v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<float> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (float v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<double> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (double v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<decimal> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (decimal v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<string> value)
//        {
//            _buffer[position] = ARRAY_START;
//            position++;
//            foreach (string v in value)
//                InternalWrite(v);

//            _buffer[position - 1] = ARRAY_END;
//            _buffer[position] = COMMA;
//            position++;
//        }

//        private unsafe void InternalWrite(IEnumerable<DateTime> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (DateTime v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<TimeSpan> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (TimeSpan v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<DateTimeOffset> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (DateTimeOffset v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<Guid> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (Guid v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<Enum> value)
//        {
//            //需要确认长度
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = ARRAY_START;
//                position++;
//                foreach (Enum v in value)
//                {
//                    p = FastToString.ToString(p, ref position, v);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (*(p - 1) == ',')
//                    p--;
//                *p++ = ARRAY_END;
//                *p = COMMA;
//                position++;
//            }
//        }

//        private unsafe void InternalWrite(IEnumerable<Uri> value)
//        {
//            //需要确认长度
//            _buffer[position] = ARRAY_START;
//            position++;
//            foreach (Uri v in value)
//                InternalWrite(v);

//            _buffer[position - 1] = ARRAY_END;
//            _buffer[position] = COMMA;
//            position++;
//        }





//        private void InternalWriteKeyName(string value)
//        {
//            if (value.Length == 0)
//            {
//                Resize(10);
//                _buffer[position] = QUOTE;
//                _buffer[position + 1] = QUOTE;
//                _buffer[position + 2] = COLON;
//                position += 3;
//            }
//            else
//            {
//                Resize((value.Length * 3) + 10);
//                _buffer[position] = QUOTE;
//                int len = sets.StringEncoding.GetBytes(value, 0, value.Length, _buffer, position + 1);
//                position += len + 1;
//                _buffer[position] = QUOTE;
//                _buffer[position + 1] = COLON;
//                position += 2;
//            }
//        }

//        private void WriteObjectEnumerable(IEnumerable value)
//        {
//            ResizeAndWriteName(10);
//            _buffer[position] = ARRAY_START;
//            position++;
//            foreach (object o in value)
//            {
//                if (o != null)
//                {
//                    if (o is ValueType)
//                    {
//                        //进行值类型判断
//                    }
//                    else
//                        ShiboJsonStreamSerializer.LoopSerialize(this, o);
//                }
//                else
//                {
//                    WriteNullWithoutName();
//                }
//            }
//            //直接覆盖掉最后一个“,”
//            if (_buffer[position - 1] == ',')
//                position--;
//            _buffer[position] = ARRAY_END;
//            _buffer[position + 1] = COMMA;
//            position += 2;
//        }

//        private void WriteTEnumerable(IEnumerable value)
//        {
//            ResizeAndWriteName(10);
//            //类型可确定，是一个固定的类型，减少类型
//            IEnumerator it = value.GetEnumerator();
//            if (it.MoveNext())
//            {
//                //_buffer[position] = ARRAY_START;
//                //position++;

//                //Type type = it.Current.GetType();
//                //IJsonStreamSerialize ser = ShiboJsonStreamSerializer.GetJsonSurrogateFromType(type);
//                //string[] names = ShiboJsonStreamSerializer.GetSerializeNames(type);
//                //ShiboJsonStreamSerializer.LoopSerialize(this, it.Current, ser, names);
//                //while (it.MoveNext())
//                //{
//                //    ShiboJsonStreamSerializer.LoopSerialize(this, it.Current, ser, names);
//                //}
//                ////直接覆盖掉最后一个“,”
//                //_buffer[position - 1] = ARRAY_END;
//                //_buffer[position] = COMMA;
//                //position++;

//                _buffer[position] = ARRAY_START;
//                position++;

//                Type type = it.Current.GetType();
//                Serialize<JsonStream> ser = ShiboJsonStreamSerializer.GenerateDataSerializeSurrogate(type);
//                string[] names = ShiboJsonStreamSerializer.GetSerializeNames(type);
//                ShiboJsonStreamSerializer.LoopSerialize(this, it.Current, ser, names);
//                while (it.MoveNext())
//                {
//                    ShiboJsonStreamSerializer.LoopSerialize(this, it.Current, ser, names);
//                }
//                //直接覆盖掉最后一个“,”
//                _buffer[position - 1] = ARRAY_END;
//                _buffer[position] = COMMA;
//                position++;
//            }
//        }

//        private void WriteObjectList(IList value)
//        {
//            ResizeAndWriteName(10);
//            if (value.Count > 0)
//            {
//                ShiboJsonStreamSerializer.LoopSerializeObjectList(this, value);
//            }
//            else
//            {
//                _buffer[position] = ARRAY_START;
//                _buffer[position + 1] = ARRAY_END;
//                _buffer[position + 2] = COMMA;
//                position += 3;
//            }



//            //ResizeAndWriteName(10);
//            //_buffer[position] = ARRAY_START;
//            //position++;
//            //foreach (object o in value)
//            //{
//            //    if (o != null)
//            //        ShiboJsonStreamSerializer.LoopSerialize(this, o);
//            //    else
//            //    {
//            //        Resize(6);
//            //        _buffer[position] = (byte)'n';
//            //        _buffer[position + 1] = (byte)'u';
//            //        _buffer[position + 2] = (byte)'l';
//            //        _buffer[position + 3] = (byte)'l';
//            //        _buffer[position + 4] = COMMA;
//            //        position += 5;
//            //    }
//            //}
//            ////直接覆盖掉最后一个“,”
//            //if (_buffer[position - 1] == ',')
//            //    position--;
//            //_buffer[position] = ARRAY_END;
//            //_buffer[position + 1] = COMMA;
//            //position += 2;
//        }

//        private void WriteTList(IList value)
//        {
//            ResizeAndWriteName(10);
//            if (value.Count > 0)
//            {
//                //_buffer[position] = ARRAY_START;
//                //position++;

//                //Type type = value[0].GetType();
//                //IJsonStreamSerialize ser = ShiboJsonStreamSerializer.GetJsonSurrogateFromType(type);
//                //string[] names = ShiboJsonStreamSerializer.GetSerializeNames(type);
//                //ShiboJsonStreamSerializer.LoopSerialize(this, value[0], ser, names);
//                //for (int i = 1; i < count; i++)
//                //{
//                //    ShiboJsonStreamSerializer.LoopSerialize(this, value[i], ser, names);
//                //}
//                ////直接覆盖掉最后一个“,”
//                //_buffer[position - 1] = ARRAY_END;
//                //_buffer[position] = COMMA;
//                //position++;


//                //_buffer[position] = ARRAY_START;
//                //position++;

//                //Type type = value[0].GetType();
//                //Serialize<JsonStream> ser = ShiboJsonStreamSerializer.GetJsonSurrogateFromType(type);
//                //string[] names = ShiboJsonStreamSerializer.GetSerializeNames(type);
//                //ShiboJsonStreamSerializer.LoopSerialize(this, value[0], ser, names);
//                //for (int i = 1; i < count; i++)
//                //{
//                //    ShiboJsonStreamSerializer.LoopSerialize(this, value[i], ser, names);
//                //}
//                ////直接覆盖掉最后一个“,”
//                //_buffer[position - 1] = ARRAY_END;
//                //_buffer[position] = COMMA;
//                //position++;




//                //_buffer[position] = ARRAY_START;
//                //position++;

//                //Type type = value[0].GetType();
//                //JsonTypeInfo info = ShiboJsonStreamSerializer.GetJsonTypes(type);
//                //ShiboJsonStreamSerializer.LoopSerialize(this, value[0], info);
//                //for (int i = 1; i < count; i++)
//                //{
//                //    //ShiboJsonStreamSerializer.LoopSerialize(this, value[i], info);
//                //    ShiboJsonStreamSerializer.LoopSerializeList(this, value[i], info.Ser);
//                //}
//                ////直接覆盖掉最后一个“,”
//                //_buffer[position - 1] = ARRAY_END;
//                //_buffer[position] = COMMA;
//                //position++;

//                ShiboJsonStreamSerializer.LoopSerializeList(this, value);
//            }
//            else
//            {
//                _buffer[position] = ARRAY_START;
//                _buffer[position + 1] = ARRAY_END;
//                _buffer[position + 2] = COMMA;
//                position += 3;
//            }
//        }

//        private void WriteStringKeyDictionary(Type type1,IDictionary value)
//        {
//            if (type1 == TypeConsts.Int32)
//                InternalWrite((Dictionary<string, int>)value);
//            else if (type1 == typeof(byte))
//                InternalWrite((Dictionary<string, byte>)value);
//            else if (type1 == typeof(string))
//                InternalWrite((Dictionary<string, string>)value);
//            else if (type1 == typeof(sbyte))
//                InternalWrite((Dictionary<string, sbyte>)value);
//            else if (type1 == typeof(short))
//                InternalWrite((Dictionary<string, short>)value);
//            else if (type1 == typeof(uint))
//                InternalWrite((Dictionary<string, uint>)value);
//            else if (type1 == typeof(long))
//                InternalWrite((Dictionary<string, long>)value);
//            else if (type1 == typeof(ulong))
//                InternalWrite((Dictionary<string, ulong>)value);
//            else if (type1 == typeof(float))
//                InternalWrite((Dictionary<string, float>)value);
//            else if (type1 == typeof(double))
//                InternalWrite((Dictionary<string, double>)value);
//            else if (type1 == typeof(decimal))
//                InternalWrite((Dictionary<string, decimal>)value);
//            else
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName((string)v.Key);
//                    InternalWrite(v.Value.ToString());
//                }
//        }

//        private void WriteObjectKeyDictionary(Type type1, IDictionary value)
//        {
//            _buffer[position++] = OBJECT_START;
//            if (type1 == typeof(int))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((int)v.Value);
//                }
//            else if (type1 == typeof(byte))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((byte)v.Value);
//                }
//            else if (type1 == typeof(sbyte))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((sbyte)v.Value);
//                }
//            else if (type1 == typeof(short))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((short)v.Value);
//                }
//            else if (type1 == typeof(ushort))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((ushort)v.Value);
//                }
//            else if (type1 == typeof(uint))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((uint)v.Value);
//                }
//            else if (type1 == typeof(long))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((long)v.Value);
//                }
//            else if (type1 == typeof(ulong))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((ulong)v.Value);
//                }
//            else if (type1 == typeof(float))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((float)v.Value);
//                }
//            else if (type1 == typeof(double))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((double)v.Value);
//                }
//            else if (type1 == typeof(decimal))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((decimal)v.Value);
//                }
//            else if (type1 == typeof(char))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((char)v.Value);
//                }
//            else if (type1 == typeof(string))
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite((string)v.Value);
//                }
//            else
//                foreach (DictionaryEntry v in value)
//                {
//                    InternalWriteKeyName(v.Key.ToString());
//                    InternalWrite(v.Value.ToString());
//                }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }





//        internal unsafe void InternalWrite(IDictionary<int, int> value)
//        {
//            fixed (byte* pd = &_buffer[position])
//            {
//                byte* p = pd;
//                *p++ = OBJECT_START;
//                position++;
//                foreach (KeyValuePair<int, int> v in value)
//                {
//                    *p++ = QUOTE;
//                    position++;
//                    p = FastToString.ToString(p, ref position, v.Key);
//                    *p++ = QUOTE;
//                    *p++ = COLON;
//                    position += 2;
//                    p = FastToString.ToString(p, ref position, v.Value);
//                    *p++ = COMMA;
//                    position++;
//                }
//                if (value.Count > 0)
//                    p--;
//                *p++ = OBJECT_END;
//                *p++ = COMMA;
//                position++;
//            }
            

//            //_buffer[position++] = OBJECT_START;
//            //foreach (KeyValuePair<int, int> v in value)
//            //{
//            //    _buffer[position] = QUOTE;
//            //    position += FastToString.ToString(_buffer, position + 1, v.Key);
//            //    _buffer[position + 1] = QUOTE;
//            //    _buffer[position + 2] = COLON;
//            //    position += 3;
//            //    InternalWrite(v.Value);
//            //}
//            //if (value.Count > 0)
//            //    position--;
//            //_buffer[position++] = OBJECT_END;
//        }


//        internal unsafe void InternalWrite(IDictionary<string, char> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, char> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, bool> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, bool> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, byte> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, byte> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, sbyte> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, sbyte> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, short> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, short> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, ushort> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, ushort> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, int> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, int> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position-1] = OBJECT_END;
//            _buffer[position++] = COMMA;


//            //fixed (byte* pd = &_buffer[position])
//            //{
//            //    byte* p = pd;
//            //    *p++ = OBJECT_START;
//            //    position++;
//            //    foreach (KeyValuePair<string, int> v in value)
//            //    {
//            //        //*p++ = QUOTE;
//            //        //position++;
//            //        //p = FastToString.ToString(p, ref position, v.Key);
//            //        //*p++ = QUOTE;
//            //        //*p++ = COLON;
//            //        //position += 2;
//            //        //p = FastToString.ToString(p, ref position, v.Value);
//            //        //*p++ = COMMA;
//            //        //position++;
//            //        InternalWriteKeyName(v.Key);
//            //        InternalWrite(v.Value);
//            //    }
//            //    if (value.Count > 0)
//            //        p--;
//            //    *p++ = OBJECT_END;
//            //    *p++ = COMMA;
//            //    position++;
//            //}


//            //_buffer[position++] = OBJECT_START;
//            //foreach (KeyValuePair<int, int> v in value)
//            //{
//            //    _buffer[position] = QUOTE;
//            //    position += FastToString.ToString(_buffer, position + 1, v.Key);
//            //    _buffer[position + 1] = QUOTE;
//            //    _buffer[position + 2] = COLON;
//            //    position += 3;
//            //    InternalWrite(v.Value);
//            //}
//            //if (value.Count > 0)
//            //    position--;
//            //_buffer[position++] = OBJECT_END;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, uint> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, uint> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, long> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, long> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, ulong> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, ulong> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, float> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, float> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, double> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, double> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, decimal> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, decimal> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        internal unsafe void InternalWrite(IDictionary<string, string> value)
//        {
//            _buffer[position++] = OBJECT_START;
//            foreach (KeyValuePair<string, string> item in value)
//            {
//                InternalWriteKeyName(item.Key);
//                InternalWrite(item.Value);
//            }
//            _buffer[position - 1] = OBJECT_END;
//            _buffer[position++] = COMMA;
//        }

//        #endregion

//        #region Write Object

        

//        internal void Write(bool value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH);
//            InternalWrite(value);
//        }

//        internal void Write(char value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_CHAR_MAX_LENGTH);
//            InternalWrite(value);
//        }

//        internal void Write(byte value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_BYTE_MAX_LENGTH);
//            InternalWrite(value);
//        }

//        internal void Write(sbyte value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_SBYTE_MAX_LENGTH);
//            InternalWrite(value);
//        }

//        internal void Write(short value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_SHORT_MAX_LENGTH);
//            InternalWrite(value);
//        }

//        internal void Write(ushort value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_USHORT_MAX_LENGTH);
//            InternalWrite(value);
//        }

//        internal void Write(int value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_INT_MAX_LENGTH);
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        internal void Write(uint value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_UINT_MAX_LENGTH);
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        internal void Write(long value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_LONG_MAX_LENGTH);
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        internal void Write(ulong value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_ULONG_MAX_LENGTH);
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = COMMA;
//            position++;
//        }

//        internal void Write(float value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_FLOAT_MAX_LENGTH);
//            InternalWrite(value);
//        }

//        internal void Write(double value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_DOUBLE_MAX_LENGTH);
//            InternalWrite(value);
//        }

//        internal void Write(decimal value)
//        {
//            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_DECIMAL_MAX_LENGTH);
//            InternalWrite(value);
//        }

//        internal void Write(string value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroString();
//            else
//            {
//                ResizeAndWriteName((value.Length * 3) + SizeConsts.CLASSTYPE_LEN);
//                _buffer[position] = QUOTE;
//                int len = sets.StringEncoding.GetBytes(value, 0, value.Length, _buffer, position + 1);
//                position += len + 1;
//                _buffer[position] = QUOTE;
//                _buffer[position+1] = COMMA;
//                position += 2;
//            }
//        }

//        internal void Write(DateTime value)
//        {
//            ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + SizeConsts.VALUETYPE_DATETIME_MAX_LENGTH);
//            InternalWrite(value);
//        }

//        internal void Write(TimeSpan value)
//        {
//            ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + SizeConsts.VALUETYPE_TIMESPAN_MAX_LENGTH);
//            this._buffer[position] = QUOTE;
//            position++;
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = QUOTE;
//            this._buffer[position + 1] = COMMA;
//            position += 2;
//        }

//        internal void Write(DateTimeOffset value)
//        {
//            ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + SizeConsts.VALUETYPE_DATETIMEOFFSET_MAX_LENGTH);
//            this._buffer[position] = QUOTE;
//            position++;
//            position += FastToString.ToString(_buffer, position, value);
//            this._buffer[position] = QUOTE;
//            this._buffer[position + 1] = COMMA;
//            position += 2;
//        }

//        internal void Write(Enum value)
//        {
//            //if (value == null)
//            //{
//            //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + 5);
//            //    this._buffer[position] = (byte)'0';
//            //    this._buffer[position + 1] = COMMA;
//            //    position += 2;
//            //}
//            //else
//            //{
//            //    string v = value.ToString();
//            //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + v.Length);
//            //    this._buffer[position] = QUOTE;
//            //    position++;
//            //    for (int i = 0; i < v.Length; i++)
//            //        _buffer[position + i] = (byte)v[i];
//            //    position += v.Length;
//            //    this._buffer[position] = QUOTE;
//            //    this._buffer[position + 1] = COMMA;
//            //    position += 2;
//            //}
//        }

//        internal void Write(Guid value)
//        {
//            ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + SizeConsts.VALUETYPE_GUID_MAX_LENGTH);
//            InternalWrite(value);
//        }

//        internal void Write(Uri value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + value.AbsoluteUri.Length);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(DBNull value)
//        {
//            WriteNull();
//        }

//        internal void Write(object value)
//        {
//            if (value == null)
//            {
//                WriteNull();
//                //跳过中间的对象
//                current += nameCounts[currSer];
//                currSer += typeCounts[currSer] + 1;
//            }
//            else
//            {
//                ResizeAndWriteName(10);

//                if (curDepth >= maxDepth)
//                    throw new Exception(string.Format(ExceptionConsts.MaxDepth, curDepth));
//                curDepth++;
//                ShiboJsonStreamSerializer.Serialize(this, value, sers[currSer++]);
//                _buffer[position] = COMMA;
//                position++;
//            }
//        }

//        internal void WriteObject(object value)
//        {
//            if (value == null)
//            {
//                WriteNull();
//                //跳过中间的对象
//                current += nameCounts[currSer];
//                currSer += typeCounts[currSer] + 1;
//            }
//            else
//            {
//                ResizeAndWriteName(10);

//                if (curDepth >= maxDepth)
//                    throw new Exception(string.Format(ExceptionConsts.MaxDepth, curDepth));
//                curDepth++;
//                ShiboJsonStreamSerializer.LoopSerialize(this, value);
//            }
//        }

//        //internal void Write(ValueType value)
//        //{
//        //    foreach (FieldInfo field in value.GetType().GetFields())
//        //    {
//        //        Write(field.GetValue(value));
//        //    }
//        //}



//        internal void Write(Nullable<bool> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<char> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<byte> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<sbyte> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<short> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<ushort> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<int> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<uint> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<long> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<ulong> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<float> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<double> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<decimal> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<DateTime> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<TimeSpan> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<DateTimeOffset> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }

//        internal void Write(Nullable<Guid> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//                Write(value.Value);
//        }








//        internal void Write(bool[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(byte[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * 2 + SizeConsts.CLASSTYPE_LEN);
//                _buffer[position] = QUOTE;
//                position++;
//                int len = FastToString.ToString(_buffer, position, value);
//                position += len;
//                _buffer[position] = QUOTE;
//                _buffer[position + 1] = COMMA;
//                position += 2;
//            }
//        }

//        internal void Write(sbyte[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_SBYTE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(char[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(short[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_SBYTE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(ushort[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_USHORT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(int[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                //ResizeAndWriteName(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//                //_buffer[position] = ARRAY_START;
//                //position++;
//                //fixed (byte* pd = &_buffer[position])
//                //{
//                //    byte* p = pd;
//                //    int len = 0, v = 0;
//                //    for (int i = 0; i < value.Length; i++)
//                //    {
//                //        len = FastToString.ToString(p, position, value[i]);
//                //        position += len + 1;
//                //        p += len;
//                //        *p++ = COMMA;

//                //        //v = value[i];
//                //        //if (v < 0)
//                //        //{
//                //        //    *p++ = (byte)'-';
//                //        //    position++;
//                //        //    v = -v;
//                //        //}
//                //        //len = FastToString.ToString(p, position, v);
//                //        //position += len + 1;
//                //        //p += len;
//                //        //*p++ = COMMA;
//                //    }
//                //}
//                //_buffer[position - 1] = ARRAY_END;
//                //_buffer[position] = COMMA;
//                //position++;


//                //ResizeAndWriteName(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//                //fixed (byte* pd = &_buffer[position])
//                //{
//                //    byte* p = pd;
//                //    *p++ = ARRAY_START;
//                //    position++;
//                //    for (int i = 0; i < value.Length - 1; i++)
//                //    {
//                //        FastToString.ToString(ref p, ref position, value[i]);
//                //        *p++ = COMMA;
//                //        position++;
//                //    }
//                //    FastToString.ToString(ref p, ref position, value[value.Length - 1]);
//                //    *p++ = ARRAY_END;
//                //    *p++ = COMMA;
//                //    position += 2;
//                //}

//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_INT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(uint[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_UINT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(long[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_LONG_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(ulong[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_ULONG_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(float[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_FLOAT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(double[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_DOUBLE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(decimal[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_DECIMAL_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(string[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.ARRAY_BASE_SIZE );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(DateTime[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_DATETIME_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(DateTimeOffset[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_DATETIMEOFFSET_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(TimeSpan[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_TIMESPAN_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(Guid[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_GUID_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(Enum[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(Uri[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }




//        internal void Write(IList<bool> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<char> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_CHAR_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<byte> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_BYTE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<sbyte> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_SBYTE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<short> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_SHORT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<ushort> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_USHORT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<int> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_INT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<uint> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_UINT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<long> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_LONG_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<ulong> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_ULONG_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<float> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_FLOAT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<double> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DOUBLE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<decimal> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DECIMAL_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<string> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<DateTime> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DATETIME_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<DateTimeOffset> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DATETIMEOFFSET_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<TimeSpan> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_TIMESPAN_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<Guid> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_GUID_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<Enum> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IList<Uri> value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Count == 0)
//                WriteZeroArray();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.ARRAY_BASE_SIZE);
//                InternalWrite(value);
//            }
//        }




//        //internal void Write(ICollection<int> value)
//        //{
//        //    if (value == null)
//        //        WriteNull();
//        //    else if (value.Count == 0)
//        //        WriteZeroArray();
//        //    else
//        //    {
//        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_INT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
//        //        InternalWrite(value);
//        //    }
//        //}




//        internal void Write(IEnumerable<bool> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<byte> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<sbyte> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<char> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<short> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<ushort> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<int> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<uint> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<long> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<ulong> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<float> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<double> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<decimal> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<string> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<DateTime> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<DateTimeOffset> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<TimeSpan> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<Guid> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<Enum> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IEnumerable<Uri> value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                ResizeAndWriteName(SizeConsts.VALUETYPE_LEN );
//                InternalWrite(value);
//            }
//        }





//        internal void Write(IDictionary<int, int> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }



//        internal void Write(IDictionary<string, bool> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, char> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, byte> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, sbyte> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, short> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, ushort> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, int> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, uint> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, long> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, ulong> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, float> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, double> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, decimal> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }

//        internal void Write(IDictionary<string, string> value)
//        {
//            if (value == null)
//                WriteNullWithoutName();
//            else if (value.Count == 0)
//                WriteZeroObjectWithoutName();
//            else
//            {
//                ResizeAndWriteName(10);
//                InternalWrite(value);
//            }
//        }





        

//        internal void Write(object[] value)
//        {
//            if (value == null)
//                WriteNull();
//            else if (value.Length == 0)
//                WriteZeroArray();
//            else
//            {
//                //ResizeAndWriteName(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//                //Write((IEnumerable<int>)value);

//                ResizeAndWriteName(10);
//                _buffer[position] = ARRAY_START;
//                position++;
//                foreach (object o in value)
//                {
//                    ShiboJsonStreamSerializer.LoopSerialize(this, o);
//                }
//                //直接覆盖掉最后一个“,”
//                _buffer[position - 1] = ARRAY_END;
//                _buffer[position] = COMMA;
//                position++;
//            }
//        }

//        internal void Write(IList value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                Type type = value.GetType();
//                if (type.GetGenericArguments().Length > 0)
//                {
//                    Type argType = type.GetGenericArguments()[0];
//                    if (argType == typeof(object))
//                        WriteObjectEnumerable((IEnumerable)value);
//                    else if (argType.IsPrimitive)
//                    {
//                        if (value is short[])
//                            Write((short[])value);
//                        else if (value is ushort[])
//                            Write((ushort[])value);
//                        else if (value is int[])
//                            Write((int[])value);
//                        else if (value is uint[])
//                            Write((uint[])value);
//                        else if (value is long[])
//                            Write((long[])value);
//                        else if (value is ulong[])
//                            Write((ulong[])value);
//                        else if (value is float[])
//                            Write((float[])value);
//                        else if (value is double[])
//                            Write((double[])value);
//                        else if (value is decimal[])
//                            Write((decimal[])value);
//                        else if (value is bool[])
//                            Write((bool[])value);
//                        else if (value is sbyte[])
//                            Write((sbyte[])value);
//                    }
//                    else
//                        WriteTList(value);
//                }
//                else
//                {
//                    if (value is short[])
//                        Write((short[])value);
//                    else if (value is ushort[])
//                        Write((ushort[])value);
//                    else if (value is int[])
//                        Write((int[])value);
//                    else if (value is uint[])
//                        Write((uint[])value);
//                    else if (value is long[])
//                        Write((long[])value);
//                    else if (value is ulong[])
//                        Write((ulong[])value);
//                    else if (value is float[])
//                        Write((float[])value);
//                    else if (value is double[])
//                        Write((double[])value);
//                    else if (value is decimal[])
//                        Write((decimal[])value);
//                    else if (value is bool[])
//                        Write((bool[])value);
//                    else if (value is sbyte[])
//                        Write((sbyte[])value);
//                    else if (value is DateTime[])
//                        Write((DateTime[])value);
//                    else if (value is TimeSpan[])
//                        Write((TimeSpan[])value);
//                    else if (value is Guid[])
//                        Write((Guid[])value);
//                    else if (value is DateTimeOffset[])
//                        Write((DateTimeOffset[])value);
//                    else if (value is Uri[])
//                        Write((Uri[])value);
//                    else
//                        WriteObjectEnumerable(value);
//                }
//            }
//        }

//        internal void Write(IDictionary value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                if (value == null)
//                    WriteNullWithoutName();
//                else if (value.Count == 0)
//                    WriteZeroObject();
//                else
//                {
//                    ResizeAndWriteName(10);
//                    //_buffer[position] = OBJECT_START;
//                    //position++;
//                    Type type = value.GetType();
//                    Type[] args = type.GetGenericArguments();
//                    if (args.Length == 2)
//                    {
//                        //对于Json而言，map的key都将变成string
//                        if (args[0] == TypeConsts.String)
//                            WriteStringKeyDictionary(args[1], value);
//                        else
//                            WriteObjectKeyDictionary(args[1], value);
//                    }
//                    else
//                    {

//                    }
//                    //if (value.Count > 0)
//                    //    position--;
//                    //_buffer[position] = OBJECT_END;
//                    //_buffer[position + 1] = COMMA;
//                    //position += 2;
//                }
//            }
//        }

//        internal void Write(IEnumerable value)
//        {
//            if (value == null)
//                WriteNull();
//            else
//            {
//                if (value is IList)
//                    Write((IList)value);
//                else if (value is IDictionary)
//                    Write((IDictionary)value);
//                else
//                    WriteObjectEnumerable(value);
//            }
//        }

//        #endregion

//        #region Write UnPackage

//        internal void WriteUnFlag()
//        {
//            //unFlag = true;
//        }


//        //internal void WriteUn(bool value)
//        //{
//        //    Resize(15);
//        //    position += FastToString.ToString(_buffer, position, value);
//        //}

//        //internal void WriteUn(byte value)
//        //{
//        //    Resize(15);
//        //    position += FastToString.ToString(_buffer, position, value);
//        //}

//        //internal void WriteUn(sbyte value)
//        //{
//        //    Resize(15);
//        //    position += FastToString.ToString(_buffer, position, value);
//        //}

//        //internal void WriteUn(short value)
//        //{
//        //    Resize(15);
//        //    position += FastToString.ToString(_buffer, position, value);
//        //}

//        //internal void WriteUn(ushort value)
//        //{
//        //    Resize(15);
//        //    position += FastToString.ToString(_buffer, position, value);
//        //}

//        //internal void WriteUn(int value)
//        //{
//        //    Resize(15);
//        //    position += FastToString.ToString(_buffer, position, value);
//        //}

//        //internal void WriteUn(uint value)
//        //{
//        //    Resize(15);
//        //    position += FastToString.ToString(_buffer, position, value);
//        //}

//        //internal void WriteUn(long value)
//        //{
//        //    Resize(15);
//        //    position += FastToString.ToString(_buffer, position, value);
//        //}

//        //internal void WriteUn(ulong value)
//        //{
//        //    Resize(15);
//        //    position += FastToString.ToString(_buffer, position, value);
//        //}

//        //internal void WriteUn(float value)
//        //{
//        //    Resize(15);
//        //    position += FastToString.ToString(_buffer, position, value);
//        //}

//        //internal void WriteUn(double value)
//        //{
//        //    Resize(15);
//        //    position += FastToString.ToString(_buffer, position, value);
//        //}

//        //internal void WriteUn(decimal value)
//        //{
//        //    Resize(15);
//        //    position += FastToString.ToString(_buffer, position, value);
//        //}

//        //internal void WriteUn(DBNull value)
//        //{
//        //    WriteNull();
//        //    position--;
//        //}

//        //internal unsafe void WriteUn(DateTime value)
//        //{
//        //    ResizeAndWriteName(40);
//        //    fixed (byte* pd = &_buffer[position])
//        //    {
//        //        byte* p = pd;
//        //        *p++ = QUOTE;
//        //        int len = FastToString.ToString(p, position, value);
//        //        p += len;
//        //        *p++ = QUOTE;
//        //        position += len + 2;
//        //    }
//        //}

//        //internal void WriteUn(TimeSpan value)
//        //{
//        //    ResizeAndWriteName(40);
//        //    this._buffer[position] = QUOTE;
//        //    position++;
//        //    position += FastToString.ToString(_buffer, position, value);
//        //    this._buffer[position] = QUOTE;
//        //    position++;
//        //}

//        //internal void WriteUn(DateTimeOffset value)
//        //{
//        //    ResizeAndWriteName(40);
//        //    this._buffer[position] = QUOTE;
//        //    position++;
//        //    position += FastToString.ToString(_buffer, position, value);
//        //    this._buffer[position] = QUOTE;
//        //    position++;
//        //}

//        //internal void WriteUn(Enum value)
//        //{
//        //    string v = value.ToString();
//        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + v.Length);
//        //    this._buffer[position] = QUOTE;
//        //    position++;
//        //    for (int i = 0; i < v.Length; i++)
//        //        _buffer[position + i] = (byte)v[i];
//        //    position += v.Length;
//        //    this._buffer[position] = QUOTE;
//        //    position++;
//        //}

//        //internal void WriteUn(Guid value)
//        //{
//        //    string v = value.ToString();
//        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + 28);
//        //    this._buffer[position] = QUOTE;
//        //    position++;
//        //    for (int i = 0; i < v.Length; i++)
//        //        _buffer[position + i] = (byte)v[i];
//        //    position += v.Length;
//        //    this._buffer[position] = QUOTE;
//        //    position++;
//        //}

//        //internal void WriteUn(Uri value)
//        //{
//        //    string v = value.ToString();
//        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + 28);
//        //    this._buffer[position] = QUOTE;
//        //    position++;
//        //    for (int i = 0; i < v.Length; i++)
//        //        _buffer[position + i] = (byte)v[i];
//        //    position += v.Length;
//        //    this._buffer[position] = QUOTE;
//        //    position++;
//        //}




//        //internal void WriteUn(short[] value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Length == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(ushort[] value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Length == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(int[] value)
//        //{
//        //    //if (value == null)
//        //    //    WriteNullWithoutName();
//        //    //else if (value.Length == 0)
//        //    //    WriteZeroArrayWithoutName();
//        //    //else
//        //    //{
//        //    //    Resize(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//        //    //    InternalWrite(value);
//        //    //    position--;
//        //    //}
//        //    Write(value);
//        //}

//        //internal void WriteUn(uint[] value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Length == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(long[] value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Length == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(ulong[] value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Length == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(float[] value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Length == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(double[] value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Length == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(decimal[] value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Length == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(bool[] value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Length == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(DateTime[] value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Length == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

        


//        //internal void WriteUn(IList<short> value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Count == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Count * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(IList<ushort> value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Count == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Count * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(IList<int> value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Count == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Count * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(IList<uint> value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Count == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Count * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(IList<long> value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Count == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Count * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(IList<ulong> value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Count == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Count * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(IList<float> value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Count == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Count * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(IList<double> value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Count == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Count * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(IList<decimal> value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Count == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Count * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(IList<bool> value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Count == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Count * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(IList<sbyte> value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else if (value.Count == 0)
//        //        WriteZeroArrayWithoutName();
//        //    else
//        //    {
//        //        Resize(value.Count * 10 +SizeConsts.VALUETYPE_LEN );
//        //        InternalWrite(value);
//        //        position--;
//        //    }
//        //}
        




//        //internal void WriteUn(IList value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else
//        //    {
//        //        Type type = value.GetType();
//        //        Type[] args = type.GetGenericArguments();
//        //        if (args.Length == 1)
//        //        {
//        //            if (args[0] == TypeConsts.Object)
//        //                WriteObjectList(value);
//        //            else
//        //                WriteTList(value);
//        //        }
//        //        else
//        //        {
//        //            throw new Exception("无参数的暂时还未处理！");
//        //        }
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(IEnumerable value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else
//        //    {
//        //        if (value is IList)
//        //            Write((IList)value);
//        //        else if (value is ISet<int>)
//        //            InternalWrite((IEnumerable<int>)value);
//        //        else
//        //            WriteObjectEnumerable(value);
//        //        position--;
//        //    }
//        //}

//        //internal void WriteUn(IDictionary value)
//        //{
//        //    if (value == null)
//        //        WriteNullWithoutName();
//        //    else
//        //    {
//        //        ResizeAndWriteName(10);
//        //        _buffer[position] = OBJECT_START;
//        //        position++;
//        //        Type type = value.GetType();
//        //        Type[] args = type.GetGenericArguments();
//        //        if (args.Length == 2)
//        //        {
//        //            //对于Json而言，map的key都将变成string
//        //            if (args[0] == TypeConsts.String)
//        //                WriteStringKeyDictionary(args[1], value);
//        //            else
//        //                WriteObjectKeyDictionary(args[1], value);
//        //        }
//        //        else
//        //        { 
                
//        //        }
//        //        if (value.Count > 0)
//        //            position--;
//        //        _buffer[position] = OBJECT_END;
//        //        position++;
//        //    }
//        //}

//        //internal void WriteUn(object value)
//        //{
//        //    if (value == null)
//        //    {
//        //        WriteNull();
//        //        //跳过中间的对象
//        //        current += nameCounts[currSer];
//        //        currSer += typeCounts[currSer] + 1;
//        //    }
//        //    else
//        //    {
//        //        ResizeAndWriteName(10);

//        //        if (curDepth >= maxDepth)
//        //            throw new Exception("序列化超出最大序列化深度！");
//        //        curDepth++;
//        //        //ShiboJsonStreamSerializer.Serialize(this, value,sers[currSer++]);
//        //        //ShiboJsonStreamSerializer.Serialize(this, value, sers[currSer++]);
//        //        _buffer[position] = COMMA;
//        //        position++;
//        //    }
//        //}


//        #endregion

//        #region Read

//        internal int ReadInt32()
//        {
//            if (CheckAndSkipName())
//            {
//                int value = FastToValue.ToInt32(_buffer,ref position);
//                return value;
//            }
//            else 
//                return 0;

//            //position += 4;
//            //return (((this._buffer[position - 4] | (this._buffer[position - 3] << 8)) | (this._buffer[position - 2] << 0x10)) | (this._buffer[position - 1] << 0x18));
//        }

//        internal uint ReadUInt32()
//        {
//            position += 4;
//            return (uint)(((this._buffer[position - 4] | (this._buffer[position - 3] << 8)) | (this._buffer[position - 2] << 0x10)) | (this._buffer[position - 1] << 0x18));
//            //return (uint)(((this.m_buffer[0] | (this.m_buffer[1] << 8)) | (this.m_buffer[2] << 0x10)) | (this.m_buffer[3] << 0x18));
//        }

//        internal ulong ReadUInt64()
//        {
//            position += 8;
//            uint num = (uint)(((this._buffer[position - 8] | (this._buffer[position - 7] << 8)) | (this._buffer[position - 6] << 0x10)) | (this._buffer[position - 5] << 0x18));
//            uint num2 = (uint)(((this._buffer[position - 4] | (this._buffer[position - 3] << 8)) | (this._buffer[position - 2] << 0x10)) | (this._buffer[position - 1] << 0x18));
//            return (ulong)(((ulong)num2 << 0x20) | num);
//        }

//        internal long ReadInt64()
//        {
//            position += 8;
//            uint num = (uint)(((this._buffer[position-8] | (this._buffer[position-7] << 8)) | (this._buffer[position-6] << 0x10)) | (this._buffer[position-5] << 0x18));
//            uint num2 = (uint)(((this._buffer[position - 4] | (this._buffer[position - 3] << 8)) | (this._buffer[position - 2] << 0x10)) | (this._buffer[position - 1] << 0x18));
//            return (long)(((long)num2 << 0x20) | num);
//        }

//        internal int ReadChar()
//        {
//            return 0;
//        }

//        internal int ReadChars()
//        {
//            return 0;
//        }

//        internal int ReadUInt16()
//        {
//            position += 2;
//            return (ushort)(this._buffer[position - 2] | (this._buffer[position - 1] << 8));
//        }

//        internal int ReadInt16()
//        {
//            position += 2;
//            return (short)(this._buffer[position - 2] | (this._buffer[position - 1] << 8));
//        }

//        internal string ReadString()
//        {
//            if (this._buffer[position] == 0)
//            {
//                position++;
//                int len = (((this._buffer[position] | (this._buffer[position + 1] << 8)) | (this._buffer[position + 2] << 0x10)) | (this._buffer[position + 3] << 0x18));
//                string value = sets.StringEncoding.GetString(this._buffer, position + 4, len);
//                position += len + 4;
//                return value;
//            }
//            else if (this._buffer[position] == ZERO_FLAG)
//            {
//                position++;
//                return string.Empty;
//            }
//            else
//            {
//                position++;
//                return null;
//            }
//        }

//        internal bool ReadBoolean()
//        {
//            if (CheckAndSkipName())
//            {
//                if (
//                    (this._buffer[position] == BYTE_LOWER_T || this._buffer[position] == BYTE_UPPER_T) &&
//                    (this._buffer[position + 1] == BYTE_LOWER_R || this._buffer[position + 1] == BYTE_UPPER_R) &&
//                    (this._buffer[position + 2] == BYTE_LOWER_U || this._buffer[position + 2] == BYTE_UPPER_U) &&
//                    (this._buffer[position + 3] == BYTE_LOWER_E || this._buffer[position + 3] == BYTE_UPPER_E)
//                   )
//                {
//                    position += 5;
//                    return true;
//                }
//                else if
//                    (
//                    (this._buffer[position] == BYTE_LOWER_F || this._buffer[position] == BYTE_UPPER_F) &&
//                    (this._buffer[position + 1] == BYTE_LOWER_A || this._buffer[position + 1] == BYTE_UPPER_A) &&
//                    (this._buffer[position + 2] == BYTE_LOWER_L || this._buffer[position + 2] == BYTE_UPPER_L) &&
//                    (this._buffer[position + 3] == BYTE_LOWER_S || this._buffer[position + 3] == BYTE_UPPER_S) &&
//                    (this._buffer[position + 4] == BYTE_LOWER_E || this._buffer[position + 4] == BYTE_UPPER_E)
//                    )
//                {
//                    position += 6;
//                    return false;
//                }
//                else
//                {
//                    throw new Exception("不正确的二进制数据类型！");
//                }

//                //if (
//                //    (this._buffer[position] == (byte)'t' || this._buffer[position] == (byte)'T') &&
//                //    (this._buffer[position+1] == (byte)'r' || this._buffer[position+1] == (byte)'R') &&
//                //    (this._buffer[position+2] == (byte)'u' || this._buffer[position+2] == (byte)'U') &&
//                //    (this._buffer[position+3] == (byte)'e' || this._buffer[position+3] == (byte)'E')
//                //   )
//                //{
//                //    position += 5;
//                //    return true;
//                //}
//                //else if
//                //    (
//                //    (this._buffer[position] == (byte)'f' || this._buffer[position] == (byte)'F') &&
//                //    (this._buffer[position] == (byte)'a' || this._buffer[position] == (byte)'A') &&
//                //    (this._buffer[position] == (byte)'l' || this._buffer[position] == (byte)'L') &&
//                //    (this._buffer[position] == (byte)'s' || this._buffer[position] == (byte)'S') &&
//                //    (this._buffer[position] == (byte)'e' || this._buffer[position] == (byte)'E')
//                //    )
//                //{
//                //    position += 6;
//                //    return false;
//                //}
//                //else
//                //{
//                //    throw new Exception("不正确的二进制数据类型！");
//                //}
//            }
//            else
//                return false;
//        }

//        internal byte ReadByte()
//        {
//            position++;
//            return (byte)this._buffer[position - 1];
//        }

//        internal sbyte ReadSByte()
//        {
//            position++;
//            return (sbyte)this._buffer[position - 1];
//        }

//        internal int ReadBytes()
//        {
//            return 0;
//        }

//        internal decimal ReadDecimal()
//        {
//            //return new decimal();
//            //return decimal.ToDecimal(this.m_buffer);
//            position += 16;
//            return 0;
//        }

//        internal unsafe float ReadSingle()
//        {
//            position += 4;
//            uint num = (uint)(((this._buffer[position - 4] | (this._buffer[position - 3] << 8)) | (this._buffer[position - 2] << 0x10)) | (this._buffer[position - 1] << 0x18));
//            return *(((float*)&num));
//        }

//        internal unsafe double ReadDouble()
//        {
//            position += 8;
//            uint num = (uint)(((this._buffer[position - 8] | (this._buffer[position - 7] << 8)) | (this._buffer[position - 6] << 0x10)) | (this._buffer[position - 5] << 0x18));
//            uint num2 = (uint)(((this._buffer[position - 4] | (this._buffer[position - 3] << 8)) | (this._buffer[position - 2] << 0x10)) | (this._buffer[position - 1] << 0x18));
//            ulong num3 = (ulong)(((ulong)num2 << 0x20) | num);
//            return *(((double*)&num3));
//        }

//        internal int[] ReadInt32Array()
//        {
//            if (this._buffer[position] == 0)
//            {
//                position++;
//                int len = (((this._buffer[position] | (this._buffer[position + 1] << 8)) | (this._buffer[position + 2] << 0x10)) | (this._buffer[position + 3] << 0x18));
//                int[] value = new int[len];
//                Buffer.BlockCopy(_buffer, position + 4, value, 0, len << 2);
//                position += (len << 2) + 4;
//                return value;
//            }
//            else if (this._buffer[position] == ZERO_FLAG)
//            {
//                position++;
//                return new int[] { };
//            }
//            else
//            {
//                position++;
//                return null;
//            }
//        }

//        internal object ReadObject()
//        {
//            //if (this._buffer[position] == 0)
//            //{
//            //    position++;
//            //    Type type = types[current++];
//            //    return ShiboSerializer.LoopDeserialize(this, type);
//            //}
//            //else
//            //{
//            //    position++;
//            //    return null;
//            //}

//            //Type type = types[current++];
//            //return ShiboSerializer.LoopDeserialize(this, type);

//            return null;
//        }

//        internal object[] ReadObjects()
//        {
//            return null;
//        }

//        #endregion

//        #region 公共

//        public override string ToString()
//        {
//            if (_buffer[position - 1] == COMMA)
//                position--;
//            return sets.StringEncoding.GetString(_buffer, 0, position);
//        }

//        public ArraySegment<byte> GetArray()
//        {
//            if (_buffer[position - 1] == COMMA)
//                position--;

//            return new ArraySegment<byte>(_buffer, 0, position);
//        }

//        public void WriteTo(Stream stream)
//        {
//            if (_buffer[position - 1] == COMMA)
//                position--;

//            if (stream is MemoryStream)
//            {
//                stream.Write(_buffer, 0, position);
//            }
//            else if (stream is FileStream)
//            {
//                stream.Write(_buffer, 0, position);
//            }
//            else
//            {
//                stream.Write(_buffer, 0, position);
//            }
//        }

//        public byte[] GetBuffer()
//        {
//            return _buffer;
//        }

//        public byte[] ToArray()
//        {
//            byte[] temp = new byte[position];
//            Buffer.BlockCopy(_buffer, 0, temp, 0, position);
//            return temp;
//        }

//        #endregion
//    }
//}
