using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using JShibo.Serialization.Common;

namespace JShibo.Serialization.Json
{
    public class JsonString :ICharBase
    {
        #region 常量

        const char OBJECT_START = '{';
        const char OBJECT_END = '}';
        const char ARRAY_START = '[';
        const char ARRAY_END = ']';
        const char COLON = ':';
        const char COMMA = ',';
        const char ZERO_FLAG = (char)0x02;

        #endregion

        #region 字段

        internal string json = string.Empty;
        internal Type[] types;
        internal Serialize<JsonString>[] sers;
        internal int[] typeCounts;
        internal int[] nameCounts;

        internal string[] names = new string[0];
        internal char[] _buffer = null;
        internal int position = 0;
        internal int current = 0;
        internal int curObj = 0;
        internal int maxDepth = 10;
        internal int curDepth = 0;
        internal bool isJsonBaseType = false;
        internal bool isRoot = true;
        /// <summary>
        /// 用于切换使用单引号还是双引号写入，该方式用于减少判断，而使用字段访问
        /// </summary>
        internal char Quote = '"';
        internal SerializerSettings sets = SerializerSettings.Default;

        #endregion

        #region 属性

        public int Position
        {
            get
            {
                return position;
            }
            set { position = value; }
        }

        public int MaxDepth
        {
            get { return maxDepth; }
            set { maxDepth = value; }
        }

        #endregion

        #region 构造函数

        public JsonString()
            : this(64)
        {
        }

        public JsonString(int capacity)
            : this(capacity, SerializerSettings.Default)
        {
        }

        public JsonString(char[] buffer)
            : this(buffer, SerializerSettings.Default)
        {
        }

        public JsonString(ref char[] buffer)
            : this(ref buffer, SerializerSettings.Default)
        {
        }

        public JsonString(char[] buffer, SerializerSettings set)
        {
            _buffer = buffer;
            sets = set;
        }

        public JsonString(ref char[] buffer, SerializerSettings set)
        {
            _buffer = buffer;
            sets = set;
        }

        public JsonString(int capacity, SerializerSettings set)
        {
            _buffer = new char[capacity];
            sets = set;
        }

        public JsonString(string json)
            : this(json, SerializerSettings.Default)
        {
        }

        public JsonString(string json, SerializerSettings set)
        {
            this.json = json;
            sets = set;
        }

        internal JsonString(JsonString stream)
        {
            this._buffer = stream._buffer;
            this.position = stream.position;
            this.sets = stream.sets;
        }


        #endregion

        #region 方法

        internal void Resize(int size)
        {
            if (_buffer.Length < position + size)
            {
                if (size > _buffer.Length)
                {
                    char[] temp = new char[_buffer.Length + size];
                    Buffer.BlockCopy(_buffer, 0, temp, 0, position * 2);
                    _buffer = temp;
                }
                else
                {
                    char[] temp = new char[_buffer.Length * 2];
                    Buffer.BlockCopy(_buffer, 0, temp, 0, position * 2);
                    _buffer = temp;
                }
            }
        }

        internal virtual unsafe void ResizeAndWriteName()
        {
            if (names.Length > 0)
            {
                string name = names[current];
                fixed (char* psrc = name, pdst = &_buffer[position])
                {
                    char* tsrc = psrc, tdst = pdst;
                    *tdst++ = Quote;
                    //FastWriteName.Write(name, _buffer, ref position);
                    //FastWriteName.Write(name, _buffer, position);
                    Utils.wstrcpy(tdst, tsrc, name.Length);
                    tdst += name.Length;
                    *tdst++ = Quote;
                    *tdst++ = ':';
                    position += name.Length + 3;
                }
                current++;
            }
        }

        internal virtual unsafe void ResizeAndWriteName(int size)
        {
            //if (names.Length > 0)
            if (isJsonBaseType == false)
            {
                string name = names[current];
                if (_buffer.Length < position + size + name.Length)
                    Resize(size + name.Length + 4);
                fixed (char* psrc = name, pdst = &_buffer[position])
                {
                    char* tsrc = psrc, tdst = pdst;
                    *tdst++ = Quote;
                    //FastWriteName.Write(name, _buffer, ref position);
                    //FastWriteName.Write(name, _buffer, position);
                    Buffer.MemoryCopy(tsrc, tdst, name.Length * 2,name.Length * 2);
                    //Utils.wstrcpy(tdst, tsrc, name.Length);
                    tdst += name.Length;
                    *tdst++ = Quote;
                    *tdst++ = ':';
                    position += name.Length + 3;
                }
                current++;
            }
            else
            {
                if (_buffer.Length < position + size)
                    Resize(size);
            }

            //string name = names[current];
            //if (_buffer.Length < position + size + name.Length)
            //    Resize(size + name.Length + 4);
            //fixed (char* psrc = name, pdst = &_buffer[position])
            //{
            //    char* tsrc = psrc, tdst = pdst;
            //    *tdst++ = Quotes;
            //    //FastWriteName.Write(name, _buffer, ref position);
            //    //FastWriteName.Write(name, _buffer, position);
            //    Utils.wstrcpy(tdst, tsrc, name.Length);
            //    tdst += name.Length;
            //    *tdst++ = Quotes;
            //    *tdst++ = ':';
            //    position += name.Length + 3;
            //}
            //current++;
        }

        internal virtual unsafe void ResizeAndWriteName(string name)
        {
            if (_buffer.Length < position + name.Length)
                Resize(name.Length + 4);
            fixed (char* psrc = name, pdst = &_buffer[position])
            {
                char* tsrc = psrc, tdst = pdst;
                *tdst++ = Quote;
                Utils.wstrcpy(tdst, tsrc, name.Length);
                tdst += name.Length;
                *tdst++ = Quote;
                *tdst++ = ':';
                position += name.Length + 3;
            }
            current++;
        }

        internal void SetInfo(JsonStringContext info)
        {
            if (info.Names.Length > 0)
            {
                if (sets.CamelCase)
                    names = info.GetNamesCamelCase();
                else
                    names = info.Names;
                if (sets.UseSingleQuotes)
                    Quote = '\'';

                sers = info.Serializers;
                types = info.Types;
                typeCounts = info.TypeCounts;
                nameCounts = info.NameCounts;
            }
            else
                isJsonBaseType = true;
        }

        #endregion

        #region 私有写入

        internal void WriteNull()
        {
            if (sets.NullValueIgnore)
                current++;
            else
            {
                ResizeAndWriteName(SizeConsts.NULL_SIZE);
                _buffer[position] = 'n';
                _buffer[position + 1] = 'u';
                _buffer[position + 2] = 'l';
                _buffer[position + 3] = 'l';
                _buffer[position + 4] = ',';
                position += 5;
            }
        }

        internal void WriteZeroArray()
        {
            ResizeAndWriteName(SizeConsts.ZERO_ARRAY_SIZE);
            _buffer[position] = '[';
            _buffer[position + 1] = ']';
            _buffer[position + 2] = ',';
            position += 3;
        }

        internal void WriteNullWithoutName()
        {
            Resize(5);
            _buffer[position] = 'n';
            _buffer[position + 1] = 'u';
            _buffer[position + 2] = 'l';
            _buffer[position + 3] = 'l';
            _buffer[position + 4] = ',';
            position += 5;
        }

        internal unsafe char* WriteNullWithNonName(char* buffer)
        {
            *buffer++ = 'n';
            *buffer++ = 'u';
            *buffer++ = 'l';
            *buffer++ = 'l';
            *buffer++ = ',';
            position += 5;
            return buffer;
        }

        internal void WriteZeroArrayWithoutName()
        {
            _buffer[position] = '[';
            _buffer[position + 1] = ']';
            _buffer[position + 2] = ',';
            position += 3;
        }

        internal void WriteZeroObject()
        {
            ResizeAndWriteName(10);
            _buffer[position] = '{';
            _buffer[position + 1] = '}';
            _buffer[position + 2] = ',';
            position += 3;
        }

        internal void WriteZeroObjectWithoutName()
        {
            Resize(10);
            _buffer[position] = '{';
            _buffer[position + 1] = '}';
            _buffer[position + 2] = ',';
            position += 3;
        }

        internal void WriteZeroString()
        {
            ResizeAndWriteName(10);
            _buffer[position] = Quote;
            _buffer[position + 1] = Quote;
            _buffer[position + 2] = ',';
            position += 3;
        }

        internal void WriteZeroStringWithoutName()
        {
            //ResizeAndWriteName(10);
            _buffer[position] = Quote;
            _buffer[position + 1] = Quote;
            _buffer[position + 2] = ',';
            position += 3;
        }

        internal void WriteTab()
        {
            _buffer[position] = '\r';
            _buffer[position + 1] = '\n';
            _buffer[position + 2] = ' ';
            _buffer[position + 3] = ' ';
            position += 4;
        }

        internal void CutTail()
        {
            position--;
        }

        internal void WriteStartArray()
        {
            _buffer[position] = '[';
            position++;
        }

        internal void WriteStartObject()
        {
            _buffer[position] = '{';
            position++;
        }

        internal void WriteEndArray()
        {
            //直接覆盖掉最后一个“,”
            _buffer[position - 1] = ']';
            _buffer[position] = ',';
            position++;
        }

        internal void WriteEndObject()
        {
            _buffer[position - 1] = '}';
            _buffer[position] = ',';
            position++;
        }

        private void WriteList<T>(IList<T> value, int max, TypeCodes type)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * (max + 1) + SizeConsts.ARRAY_BASE_SIZE);
                switch (type)
                {
                    case TypeCodes.Boolean:
                        InternalWrite(((IList<bool>)value));
                        break;
                    case TypeCodes.Byte:
                        InternalWrite(((IList<byte>)value));
                        break;
                    case TypeCodes.Int32:
                        InternalWrite(((IList<int>)value));
                        break;
                    case TypeCodes.UInt32:
                        InternalWrite(((IList<uint>)value));
                        break;
                }
            }
        }

        private void WriteList<T>(IEnumerable<T> value, int max, TypeCodes type)
        {
            if (value == null)
                WriteNull();
            else
                WriteList<T>(value.ToArray(), max, type);
        }

        private void InternalWrite(object value)
        {
            if (value != null)
            {
                Type type = value.GetType();
                if (type == TypeConsts.String)
                    InternalWrite((string)value);
                else if (type == TypeConsts.Int32)
                {
                    Resize(SizeConsts.VALUETYPE_INT_MAX_LENGTH);
                    InternalWrite((int)value);
                }
                else if (type == TypeConsts.Int16)
                {
                    Resize(SizeConsts.VALUETYPE_SHORT_MAX_LENGTH);
                    InternalWrite((short)value);
                }
                else if (type == TypeConsts.Int64)
                {
                    Resize(SizeConsts.VALUETYPE_LONG_MAX_LENGTH);
                    InternalWrite((long)value);
                }
                else if (type == TypeConsts.Boolean)
                {
                    Resize(SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH);
                    InternalWrite((bool)value);
                }
                else if (type == TypeConsts.Byte)
                {
                    Resize(SizeConsts.VALUETYPE_BYTE_MAX_LENGTH);
                    InternalWrite((byte)value);
                }
                else if (type == TypeConsts.SByte)
                {
                    Resize(SizeConsts.VALUETYPE_SBYTE_MAX_LENGTH);
                    InternalWrite((sbyte)value);
                }
                else if (type == TypeConsts.UInt16)
                {
                    Resize(SizeConsts.VALUETYPE_USHORT_MAX_LENGTH);
                    InternalWrite((ushort)value);
                }
                else if (type == TypeConsts.UInt32)
                {
                    Resize(SizeConsts.VALUETYPE_UINT_MAX_LENGTH);
                    InternalWrite((uint)value);
                }
                else if (type == TypeConsts.UInt64)
                {
                    Resize(SizeConsts.VALUETYPE_ULONG_MAX_LENGTH);
                    InternalWrite((ulong)value);
                }
                else if (type == TypeConsts.Char)
                {
                    Resize(SizeConsts.VALUETYPE_CHAR_MAX_LENGTH);
                    InternalWrite((char)value);
                }
                else if (type == TypeConsts.Single)
                {
                    Resize(SizeConsts.VALUETYPE_FLOAT_MAX_LENGTH);
                    InternalWrite((float)value);
                }
                else if (type == TypeConsts.Double)
                {
                    Resize(SizeConsts.VALUETYPE_DOUBLE_MAX_LENGTH);
                    InternalWrite((double)value);
                }
                else if (type == TypeConsts.Decimal)
                {
                    Resize(SizeConsts.VALUETYPE_DECIMAL_MAX_LENGTH);
                    InternalWrite((decimal)value);
                }
                else if (type == TypeConsts.DateTime)
                {
                    Resize(SizeConsts.VALUETYPE_DATETIME_MAX_LENGTH);
                    InternalWrite((DateTime)value);
                }
                else if (type == TypeConsts.TimeSpan)
                {
                    Resize(SizeConsts.VALUETYPE_TIMESPAN_MAX_LENGTH);
                    InternalWrite((TimeSpan)value);
                }
                else if (type == TypeConsts.DateTimeOffset)
                {
                    Resize(SizeConsts.VALUETYPE_DATETIMEOFFSET_MAX_LENGTH);
                    InternalWrite((DateTimeOffset)value);
                }
                else if (type == TypeConsts.Guid)
                {
                    Resize(SizeConsts.VALUETYPE_GUID_MAX_LENGTH);
                    InternalWrite((Guid)value);
                }
                else if (type == TypeConsts.Uri)
                {
                    InternalWrite((Uri)value);
                }
                //else if (type == TypeConsts.Enum)
                //    InternalWrite((Enum)value);
                //else if (type == TypeConsts.DataTable)
                //    InternalWrite((DataTable)value);
                else
                    WriteObject(value);
            }
            else
                WriteNull();
        }

        private void InternalWrite(byte value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        private void InternalWrite(sbyte value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        private void InternalWrite(short value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        private void InternalWrite(ushort value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        private void InternalWrite(int value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        private void InternalWrite(uint value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        private void InternalWrite(long value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        private void InternalWrite(ulong value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        private void InternalWrite(float value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        private void InternalWrite(double value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        private void InternalWrite(decimal value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        private void InternalWrite(string value)
        {
            if (value == null)
            {
                Resize(10);
                WriteNullWithoutName();
            }
            else if (value.Length == 0)
            {
                Resize(10);
                WriteZeroStringWithoutName();
            }
            else
            {
                Resize((value.Length * 2) + 10);
                WriteString(value);
            }
        }

        private void InternalWrite(bool value)
        {
            if (value)
            {
                _buffer[position] = 't';
                _buffer[position + 1] = 'r';
                _buffer[position + 2] = 'u';
                _buffer[position + 3] = 'e';
                _buffer[position + 4] = ',';
                position += 5;
            }
            else
            {
                _buffer[position] = 'f';
                _buffer[position + 1] = 'a';
                _buffer[position + 2] = 'l';
                _buffer[position + 3] = 's';
                _buffer[position + 4] = 'e';
                _buffer[position + 5] = ',';
                position += 6;
            }
        }

        private void InternalWrite(char value)
        {
            _buffer[position] = Quote;
            _buffer[position + 1] = value;
            _buffer[position + 2] = Quote;
            this._buffer[position + 3] = ',';
            position += 4;
        }

        private unsafe void InternalWrite(DateTime value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = Quote;
                p = FastToString.ToString(p, ref position, value);
                *p++ = Quote;
                *p++ = ',';
                position += 3;
            }
        }

        private unsafe void InternalWrite(DateTimeOffset value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = Quote;
                p = FastToString.ToString(p, ref position, value);
                *p++ = Quote;
                *p++ = ',';
                position += 3;
            }
        }

        private unsafe void InternalWrite(TimeSpan value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = Quote;
                p = FastToString.ToString(p, ref position, value);
                *p++ = Quote;
                *p++ = ',';
                position += 3;
            }
        }

        private unsafe void InternalWrite(Guid value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = Quote;
                p = FastToString.ToString(p, ref position, value);
                *p++ = Quote;
                *p++ = ',';
                position += 3;
            }
        }

        private unsafe void InternalWrite(Uri value)
        {
            Resize((value.AbsoluteUri.Length) + 10);
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = Quote;
                p = FastToString.ToString(p, ref position, value);
                *p++ = Quote;
                *p++ = ',';
                position += 3;
            }
        }

        private unsafe char* InternalWrite(char* buffer, string value)
        {
            if (value == null)
            {
                *buffer++ = 'n';
                *buffer++ = 'u';
                *buffer++ = 'l';
                *buffer++ = 'l';
                *buffer++ = ',';
                position += 5;
            }
            else if (value.Length == 0)
            {
                *buffer++ = Quote;
                *buffer++ = Quote;
                *buffer++ = ',';
                position += 3;
            }
            else
            {
                int escCount = 0;
                *buffer++ = Quote;
                fixed (char* chs = value)
                {
                    if (sets.Escape == StringEscape.Default)
                    {
                        if (sets.CheckJsonEscape == CheckEscape.None) //&& Utils.FastFindEscape(chs,value.Length)==true) //&& value.IndexOf('"') != -1)
                        {
                            Utils.wstrcpy(buffer, chs, value.Length);
                            buffer += value.Length;
                        }
                        //else if (sets.CheckJsonEscape == CheckEscape.May)
                        //{
                        //    if (value.IndexOf('"') != -1)
                        //    //if (Utils.FastFindEscape(chs, value.Length) == true)
                        //    {
                        //        escCount = Utils.CheckThenCopy(buffer, chs, value.Length);
                        //        buffer += value.Length + escCount;
                        //    }
                        //    else
                        //    {
                        //        Utils.wstrcpy(buffer, chs, value.Length);
                        //        buffer += value.Length;
                        //    }
                        //}
                        else if (sets.CheckJsonEscape == CheckEscape.Must)
                        {
                            //escCount = Utils.WriteUsAct(buffer, chs, value.Length);
                            //escCount = Utils.CheckThenCopy(buffer, chs, value.Length);
                            escCount = Utils.CheckFullThenCopy(buffer, chs, value.Length);
                            buffer += value.Length + escCount;
                        }
                        else if (sets.CheckJsonEscape == CheckEscape.OnlyCheckQuote)
                        {
                            escCount = Utils.CheckThenCopy(buffer, chs, value.Length);
                            buffer += value.Length + escCount;
                        }
                        position += value.Length + 3 + escCount;
                    }
                    //else if (sets.Escape == StringEscape.EscapeHtml)
                    //{
                    //    Utils.wstrcpy(buffer, chs, value.Length);
                    //    buffer += value.Length;
                    //    position += value.Length + 3 + escCount;
                    //}
                    else if (sets.Escape == StringEscape.EscapeNonAscii)
                    {
                        int ecount = Utils.ToCharAsUnicode(buffer, chs, value.Length);
                        buffer += ecount * 6 + value.Length - ecount;
                        position += ecount * 6 + value.Length - ecount + 3;
                    }
                }
                *buffer++ = Quote;
                *buffer++ = ',';
            }
            return buffer;
        }

        private unsafe char* InternalWriteNonCheck(char* buffer, string value)
        {
            if (value == null)
                return WriteNullWithNonName(buffer);
            else if (value.Length == 0)
            {
                *buffer++ = Quote;
                *buffer++ = Quote;
                *buffer++ = ',';
                position += 3;
            }
            else
            {
                *buffer++ = Quote;
                fixed (char* chs = value)
                {
                    Utils.wstrcpy(buffer, chs, value.Length);
                    buffer += value.Length;
                }
                *buffer++ = Quote;
                *buffer++ = ',';
                position += value.Length + 3;
            }
            return buffer;
        }

        private unsafe char* InternalWriteFullCheck(char* buffer, string value)
        {
            if (value == null)
                return WriteNullWithNonName(buffer);
            else if (value.Length == 0)
            {
                *buffer++ = Quote;
                *buffer++ = Quote;
                *buffer++ = ',';
                position += 3;
            }
            else
            {
                *buffer++ = Quote;
                int escCount = 0;
                fixed (char* chs = value)
                {
                    escCount = Utils.CheckFullThenCopy(buffer, chs, value.Length);
                    buffer += value.Length + escCount;
                }
                *buffer++ = Quote;
                *buffer++ = ',';
                position += value.Length + 3;
            }
            return buffer;
        }

        private unsafe char* InternalWriteEscapeNonAscii(char* buffer, string value)
        {
            if (value == null)
                return WriteNullWithNonName(buffer);
            else if (value.Length == 0)
            {
                *buffer++ = Quote;
                *buffer++ = Quote;
                *buffer++ = ',';
                position += 3;
            }
            else
            {
                *buffer++ = Quote;
                fixed (char* chs = value)
                {
                    int ecount = Utils.ToCharAsUnicode(buffer, chs, value.Length);
                    buffer += ecount * 6 + value.Length - ecount;
                    position += ecount * 6 + value.Length - ecount + 3;
                }
                *buffer++ = Quote;
                *buffer++ = ',';
                position += value.Length + 3;
            }
            return buffer;
        }

        private unsafe void InternalWrite(bool[] value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Length - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(char[] value)
        {
            fixed (char* psrc = value, pdst = &_buffer[position])
            {
                char* tsrc = psrc, tdst = pdst;
                *tdst++ = Quote;
                Utils.wstrcpy(tdst, tsrc, value.Length);
                tdst += value.Length;
                *tdst++ = Quote;
                *tdst++ = ',';
                position += value.Length + 3;
            }
        }

        private unsafe void InternalWrite(short[] value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Length - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(ushort[] value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Length - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(int[] value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Length - 1;

                #region old
                //if (sets.NumericCheck == NumericCheckType.Middle)
                //{
                //    for (int i = 0; i < len; i++)
                //    {
                //        p = FastToString.ToString(p, ref position, value[i]);
                //        *p++ = ',';
                //        position++;
                //    }
                //}
                //else if (sets.NumericCheck == NumericCheckType.Max)
                //{
                //    for (int i = 0; i < len; i++)
                //    {
                //        p = FastToString.ToStringMax(p, ref position, value[i]);
                //        *p++ = ',';
                //        position++;
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < len; i++)
                //    {
                //        p = FastToString.ToStringMin(p, ref position, value[i]);
                //        *p++ = ',';
                //        position++;
                //    }
                //}
                #endregion

                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(uint[] value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Length - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(long[] value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Length - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(ulong[] value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Length - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(float[] value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Length - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(double[] value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Length - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(decimal[] value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Length - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(string[] value)
        {
            int size = GetSize(value, sets.Escape);
            Resize(size);
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                for (int i = 0; i < value.Length; i++)
                    p = InternalWrite(p, value[i]);
                p--;
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(DateTime[] value)
        {
            ResizeAndWriteName(value.Length * 10 + SizeConsts.VALUETYPE_LEN);
            _buffer[position] = '[';
            position++;
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                for (int i = 0; i < value.Length; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
            }
            _buffer[position - 1] = ']';
            _buffer[position] = ',';
            position++;
        }

        private unsafe void InternalWrite(TimeSpan[] value)
        {
            ResizeAndWriteName(value.Length * 10 + SizeConsts.VALUETYPE_LEN);
            _buffer[position] = '[';
            position++;
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                int len = 0;
                for (int i = 0; i < value.Length; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
            }
            _buffer[position - 1] = ']';
            _buffer[position] = ',';
            position++;
        }



        private unsafe void InternalWrite(IList<bool> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Count - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(IList<short> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Count - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(IList<ushort> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Count - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(IList<int> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Count - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(IList<uint> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Count - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(IList<long> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Count - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(IList<ulong> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Count - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(IList<float> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Count - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(IList<double> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Count - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(IList<decimal> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                int len = value.Count - 1;
                for (int i = 0; i < len; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
                p = FastToString.ToString(p, ref position, value[len]);
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }

        private unsafe void InternalWrite(IList<DateTime> value)
        {
            _buffer[position] = '[';
            position++;
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                int count = value.Count;
                for (int i = 0; i < count; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
            }
            _buffer[position - 1] = ']';
            _buffer[position] = ',';
            position++;
        }

        private unsafe void InternalWrite(IList<TimeSpan> value)
        {
            _buffer[position] = '[';
            position++;
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                int count = value.Count;
                for (int i = 0; i < count; i++)
                {
                    p = FastToString.ToString(p, ref position, value[i]);
                    *p++ = ',';
                    position++;
                }
            }
            _buffer[position - 1] = ']';
            _buffer[position] = ',';
            position++;
        }

        private unsafe void InternalWrite(IList<string> value)
        {
            //int size = GetSize(value, sets.Escape);
            //Resize(size);
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                if (sets.Escape == StringEscape.Default)
                {
                    if (sets.CheckJsonEscape == CheckEscape.None)
                    {
                        for (int i = 0; i < value.Count; i++)
                            p = InternalWriteNonCheck(p, value[i]);
                    }
                    else if (sets.CheckJsonEscape == CheckEscape.Must)
                    {
                        for (int i = 0; i < value.Count; i++)
                            p = InternalWriteFullCheck(p, value[i]);
                    }
                    else if (sets.CheckJsonEscape == CheckEscape.OnlyCheckQuote)
                    {
                        //escCount = Utils.CheckThenCopy(tdst, tsrc, value.Length);
                        //tdst += value.Length + escCount;
                    }
                    //position += value.Length + 3 + escCount;
                    else if (sets.Escape == StringEscape.EscapeNonAscii)
                    {
                        for (int i = 0; i < value.Count; i++)
                            p = InternalWriteEscapeNonAscii(p, value[i]);
                    }
                }
                p--;
                *p++ = ']';
                *p = ',';
                position += 2;
            }
        }


        private unsafe void InternalWrite(IEnumerable<bool> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (bool v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<char> value)
        {
            _buffer[position] = '[';
            position++;
            foreach (char v in value)
                InternalWrite(v);
            _buffer[position - 1] = ']';
            _buffer[position] = ',';
            position++;
        }

        private unsafe void InternalWrite(IEnumerable<byte> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (byte v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<sbyte> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (sbyte v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<short> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (short v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<ushort> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (ushort v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<int> value)
        {
            //_buffer[position] = '[';
            //position++;
            //foreach (int v in value)
            //    InternalWrite(v);
            //_buffer[position - 1] = ']';
            //_buffer[position] = ',';
            //position++;



            //_buffer[position] = '[';
            //position++;
            //fixed (char* pd = &_buffer[position])
            //{
            //    char* p = pd;
            //    int len = 0;
            //    foreach (int v in value)
            //    {
            //        len = FastToString.ToString(p, position, v);
            //        position += len + 1;
            //        p += len;
            //        *p++ = ',';
            //    }
            //}
            //_buffer[position - 1] = ']';
            //_buffer[position] = ',';
            //position++;


            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (int v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<uint> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (uint v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<long> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (long v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<ulong> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (ulong v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<float> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (float v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<double> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (double v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<decimal> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (decimal v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<string> value)
        {
            _buffer[position] = '[';
            position++;
            foreach (string v in value)
                InternalWrite(v);

            _buffer[position - 1] = ']';
            _buffer[position] = ',';
            position++;
        }

        private unsafe void InternalWrite(IEnumerable<DateTime> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (DateTime v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<TimeSpan> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (TimeSpan v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<DateTimeOffset> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (DateTimeOffset v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<Guid> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (Guid v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<Enum> value)
        {
            //需要确认长度
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = '[';
                position++;
                foreach (Enum v in value)
                {
                    p = FastToString.ToString(p, ref position, v);
                    *p++ = ',';
                    position++;
                }
                if (*(p - 1) == ',')
                    p--;
                *p++ = ']';
                *p = ',';
                position++;
            }
        }

        private unsafe void InternalWrite(IEnumerable<Uri> value)
        {
            //需要确认长度
            _buffer[position] = '[';
            position++;
            foreach (Uri v in value)
                InternalWrite(v);

            _buffer[position - 1] = ']';
            _buffer[position] = ',';
            position++;
        }





        private unsafe void InternalWriteKeyName(string value)
        {
            if (value.Length == 0)
            {
                Resize(10);
                _buffer[position] = Quote;
                _buffer[position + 1] = Quote;
                _buffer[position + 2] = ':';
                position += 3;
            }
            else
            {
                Resize((value.Length) + 10);
                WriteString(value,':');
            }
        }

        private void WriteObjectEnumerable(IEnumerable value)
        {
            ResizeAndWriteName(10);
            WriteStartArray();
            foreach (object o in value)
            {
                if (o != null)
                {
                    if (o is ValueType)
                    {
                        //进行值类型判断
                    }
                    else
                        ShiboJsonStringSerializer.LoopSerialize(this, o);
                }
                else
                    WriteNullWithoutName();
            }
            WriteEndArray();
        }

        private void WriteTEnumerable(IEnumerable value)
        {
            ResizeAndWriteName(10);
            ShiboJsonStringSerializer.LoopSerializeEnumerable(this, value);

            #region old
            //IEnumerator it = value.GetEnumerator();
            //if (it.MoveNext())
            //{
            //    _buffer[position] = '[';
            //    position++;

            //    Type type = it.Current.GetType();
            //    Serialize<JsonString> ser = ShiboJsonStringSerializer.GenerateJsonSerializeSurrogate(type);
            //    string[] names = ShiboJsonStringSerializer.GetSerializeNames(type);
            //    ShiboJsonStringSerializer.LoopSerialize(this, it.Current, ser, names);
            //    while (it.MoveNext())
            //    {
            //        ShiboJsonStringSerializer.LoopSerialize(this, it.Current, ser, names);
            //    }
            //    //直接覆盖掉最后一个“,”
            //    _buffer[position - 1] = ']';
            //    _buffer[position] = ',';
            //    position++;
            //}
            //else
            //    WriteZeroArrayWithoutName();
            #endregion
        }

        private void WriteObjectList(IList value)
        {
            ResizeAndWriteName(10);
            if (value.Count > 0)
                ShiboJsonStringSerializer.LoopSerializeObjectList(this, value);
            else
                WriteZeroArrayWithoutName();
        }

        private void WriteTList(IList value)
        {
            ResizeAndWriteName(10);
            if (value.Count > 0)
            {
                #region old

                //_buffer[position] = '[';
                //position++;

                //Type type = value[0].GetType();
                //IJsonStreamSerialize ser = ShiboJsonStreamSerializer.GetJsonSurrogateFromType(type);
                //string[] names = ShiboJsonStreamSerializer.GetSerializeNames(type);
                //ShiboJsonStreamSerializer.LoopSerialize(this, value[0], ser, names);
                //for (int i = 1; i < count; i++)
                //{
                //    ShiboJsonStreamSerializer.LoopSerialize(this, value[i], ser, names);
                //}
                ////直接覆盖掉最后一个“,”
                //_buffer[position - 1] = ']';
                //_buffer[position] = ',';
                //position++;


                //_buffer[position] = '[';
                //position++;

                //Type type = value[0].GetType();
                //Serialize<JsonStream> ser = ShiboJsonStreamSerializer.GetJsonSurrogateFromType(type);
                //string[] names = ShiboJsonStreamSerializer.GetSerializeNames(type);
                //ShiboJsonStreamSerializer.LoopSerialize(this, value[0], ser, names);
                //for (int i = 1; i < count; i++)
                //{
                //    ShiboJsonStreamSerializer.LoopSerialize(this, value[i], ser, names);
                //}
                ////直接覆盖掉最后一个“,”
                //_buffer[position - 1] = ']';
                //_buffer[position] = ',';
                //position++;




                //_buffer[position] = '[';
                //position++;

                //Type type = value[0].GetType();
                //JsonTypeInfo info = ShiboJsonStreamSerializer.GetJsonTypes(type);
                //ShiboJsonStreamSerializer.LoopSerialize(this, value[0], info);
                //for (int i = 1; i < count; i++)
                //{
                //    //ShiboJsonStreamSerializer.LoopSerialize(this, value[i], info);
                //    ShiboJsonStreamSerializer.LoopSerializeList(this, value[i], info.Ser);
                //}
                ////直接覆盖掉最后一个“,”
                //_buffer[position - 1] = ']';
                //_buffer[position] = ',';
                //position++;

                #endregion

                ShiboJsonStringSerializer.LoopSerializeList(this, value);
            }
            else
                WriteZeroArrayWithoutName();
        }

        private void WriteStringKeyDictionary(Type type1, IDictionary value)
        {
            if (type1 == TypeConsts.Int32)
                InternalWrite((Dictionary<string, int>)value);
            else if (type1 == typeof(char))
                InternalWrite((Dictionary<string, byte>)value);
            else if (type1 == typeof(string))
                InternalWrite((Dictionary<string, string>)value);
            else if (type1 == typeof(sbyte))
                InternalWrite((Dictionary<string, sbyte>)value);
            else if (type1 == typeof(short))
                InternalWrite((Dictionary<string, short>)value);
            else if (type1 == typeof(uint))
                InternalWrite((Dictionary<string, uint>)value);
            else if (type1 == typeof(long))
                InternalWrite((Dictionary<string, long>)value);
            else if (type1 == typeof(ulong))
                InternalWrite((Dictionary<string, ulong>)value);
            else if (type1 == typeof(float))
                InternalWrite((Dictionary<string, float>)value);
            else if (type1 == typeof(double))
                InternalWrite((Dictionary<string, double>)value);
            else if (type1 == typeof(decimal))
                InternalWrite((Dictionary<string, decimal>)value);
            else
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName((string)v.Key);
                    InternalWrite(v.Value.ToString());
                }
        }

        private void WriteObjectKeyDictionary(Type type1, IDictionary value)
        {
            _buffer[position++] = '{';
            if (type1 == typeof(int))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((int)v.Value);
                }
            else if (type1 == typeof(char))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((char)v.Value);
                }
            else if (type1 == typeof(sbyte))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((sbyte)v.Value);
                }
            else if (type1 == typeof(short))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((short)v.Value);
                }
            else if (type1 == typeof(ushort))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((ushort)v.Value);
                }
            else if (type1 == typeof(uint))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((uint)v.Value);
                }
            else if (type1 == typeof(long))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((long)v.Value);
                }
            else if (type1 == typeof(ulong))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((ulong)v.Value);
                }
            else if (type1 == typeof(float))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((float)v.Value);
                }
            else if (type1 == typeof(double))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((double)v.Value);
                }
            else if (type1 == typeof(decimal))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((decimal)v.Value);
                }
            else if (type1 == typeof(char))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((char)v.Value);
                }
            else if (type1 == typeof(string))
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite((string)v.Value);
                }
            else
                foreach (DictionaryEntry v in value)
                {
                    InternalWriteKeyName(v.Key.ToString());
                    InternalWrite(v.Value.ToString());
                }
            _buffer[position - 1] = '}';
            _buffer[position++] = ',';
        }





        internal unsafe void InternalWrite(IDictionary<int, int> value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                foreach (KeyValuePair<int, int> v in value)
                {
                    *p++ = Quote;
                    position++;
                    p = FastToString.ToString(p, ref position, v.Key);
                    *p++ = Quote;
                    *p++ = ':';
                    position += 2;
                    p = FastToString.ToString(p, ref position, v.Value);
                    *p++ = ',';
                    position++;
                }
            }
        }


        internal unsafe void InternalWrite(IDictionary<string, char> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, char> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, bool> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, bool> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, byte> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, byte> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, sbyte> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, sbyte> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, short> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, short> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, ushort> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, ushort> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, int> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, int> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';


            //fixed (char* pd = &_buffer[position])
            //{
            //    char* p = pd;
            //    *p++ = '{';
            //    position++;
            //    foreach (KeyValuePair<string, int> v in value)
            //    {
            //        //*p++ = Quotes;
            //        //position++;
            //        //p = FastToString.ToString(p, ref position, v.Key);
            //        //*p++ = Quotes;
            //        //*p++ = ':';
            //        //position += 2;
            //        //p = FastToString.ToString(p, ref position, v.Value);
            //        //*p++ = ',';
            //        //position++;
            //        InternalWriteKeyName(v.Key);
            //        InternalWrite(v.Value);
            //    }
            //    if (value.Count > 0)
            //        p--;
            //    *p++ = '}';
            //    *p++ = ',';
            //    position++;
            //}


            //_buffer[position++] = '{';
            //foreach (KeyValuePair<int, int> v in value)
            //{
            //    _buffer[position] = Quotes;
            //    position += FastToString.ToString(_buffer, position + 1, v.Key);
            //    _buffer[position + 1] = Quotes;
            //    _buffer[position + 2] = ':';
            //    position += 3;
            //    InternalWrite(v.Value);
            //}
            //if (value.Count > 0)
            //    position--;
            //_buffer[position++] = '}';
        }

        internal unsafe void InternalWrite(IDictionary<string, uint> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, uint> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, long> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, long> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, ulong> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, ulong> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, float> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, float> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, double> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, double> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, decimal> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, decimal> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, string> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, string> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<string, object> value)
        {
            //_buffer[position++] = '{';
            foreach (KeyValuePair<string, object> item in value)
            {
                InternalWriteKeyName(item.Key);
                InternalWrite(item.Value);
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        internal unsafe void InternalWrite(IDictionary<object, object> value)
        {
            //无法计算出容量，无法通过指针一次性优化
            //_buffer[position++] = '{';
            foreach (KeyValuePair<object, object> item in value)
            {
                InternalWriteKeyName(item.Key.ToString());
                InternalWrite(item.Value.ToString());
            }
            //_buffer[position - 1] = '}';
            //_buffer[position++] = ',';
        }

        #endregion

        #region Write BaseType

        internal void Write(bool value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(char value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_CHAR_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(byte value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_BYTE_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(sbyte value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_SBYTE_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(short value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_SHORT_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(ushort value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_USHORT_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(int value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_INT_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(uint value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_UINT_MAX_LENGTH);
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        internal void Write(long value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_LONG_MAX_LENGTH);

            //if (sets.NumericCheck == NumericCheckType.Middle)
            //    position += FastToString.ToString(_buffer, position, value);
            //else if (sets.NumericCheck == NumericCheckType.Max)
            //    position += FastToString.ToStringMax(_buffer, position, value);
            //else
            //    position += FastToString.ToStringMin(_buffer, position, value);

            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        internal void Write(ulong value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_ULONG_MAX_LENGTH);

            //if (sets.NumericCheck == NumericCheckType.Middle)
            //    position += FastToString.ToString(_buffer, position, value);
            //else if (sets.NumericCheck == NumericCheckType.Max)
            //    position += FastToString.ToStringMax(_buffer, position, value);
            //else
            //    position += FastToString.ToStringMin(_buffer, position, value);

            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = ',';
            position++;
        }

        internal void Write(float value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_FLOAT_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(double value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_DOUBLE_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(decimal value)
        {
            ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_DECIMAL_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(string value)
        {
            if (value == null)
                WriteNull();
            else if (value.Length == 0)
                WriteZeroString();
            else
            {
                ResizeAndWriteName((value.Length * 2) + SizeConsts.CLASSTYPE_LEN);
                WriteString(value);
            }
        }

        internal void Write(DateTime value)
        {
            ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + SizeConsts.VALUETYPE_DATETIME_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(TimeSpan value)
        {
            ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + SizeConsts.VALUETYPE_TIMESPAN_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(DateTimeOffset value)
        {
            ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + SizeConsts.VALUETYPE_DATETIMEOFFSET_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(Guid value)
        {
            ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + SizeConsts.VALUETYPE_GUID_MAX_LENGTH);
            InternalWrite(value);
        }

        internal void Write(Uri value)
        {
            if (value == null)
                WriteNull();
            else
            {
                ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + value.AbsoluteUri.Length);
                InternalWrite(value);
            }
        }

        internal void Write(DBNull value)
        {
            WriteNull();
        }

        internal void Write(DataTable value)
        {
            if (value == null)
                WriteNull();
            else if (value.Rows.Count == 0)
                WriteZeroObject();
            else
            {
                TypeCode[] codes = new TypeCode[value.Columns.Count];
                string[] colNames = new string[value.Columns.Count];
                for (int i = 0; i < codes.Length; i++)
                {
                    if (value.Columns[i].DataType == typeof(Boolean))
                        codes[i] = TypeCode.Boolean;
                    else if (value.Columns[i].DataType == typeof(Char))
                        codes[i] = TypeCode.Char;
                    else if (value.Columns[i].DataType == typeof(SByte))
                        codes[i] = TypeCode.SByte;
                    else if (value.Columns[i].DataType == typeof(Byte))
                        codes[i] = TypeCode.Byte;
                    else if (value.Columns[i].DataType == typeof(Int16))
                        codes[i] = TypeCode.Int16;
                    else if (value.Columns[i].DataType == typeof(UInt16))
                        codes[i] = TypeCode.UInt16;
                    else if (value.Columns[i].DataType == typeof(Int32))
                        codes[i] = TypeCode.Int32;
                    else if (value.Columns[i].DataType == typeof(UInt32))
                        codes[i] = TypeCode.UInt32;
                    else if (value.Columns[i].DataType == typeof(Int64))
                        codes[i] = TypeCode.Int64;
                    else if (value.Columns[i].DataType == typeof(UInt64))
                        codes[i] = TypeCode.UInt64;
                    else if (value.Columns[i].DataType == typeof(Single))
                        codes[i] = TypeCode.Single;
                    else if (value.Columns[i].DataType == typeof(Double))
                        codes[i] = TypeCode.Double;
                    else if (value.Columns[i].DataType == typeof(Decimal))
                        codes[i] = TypeCode.Decimal;
                    else if (value.Columns[i].DataType == typeof(DateTime))
                        codes[i] = TypeCode.DateTime;
                    else if (value.Columns[i].DataType == typeof(String))
                        codes[i] = TypeCode.String;
                    else
                        codes[i] = TypeCode.String;
                    colNames[i] = value.Columns[i].ColumnName;
                }

                WriteStartArray();
                for (int j = 0; j < value.Rows.Count; j++)
                {
                    WriteStartObject();
                    object[] objs = value.Rows[j].ItemArray;
                    for (int i = 0; i < codes.Length; i++)
                    {
                        ResizeAndWriteName(colNames[i]);
                        switch (codes[i])
                        {
                            case TypeCode.Boolean:
                                Write((Boolean)objs[i]);
                                break;
                            case TypeCode.Char:
                                Write((Char)objs[i]);
                                break;
                            case TypeCode.SByte:
                                Write((SByte)objs[i]);
                                break;
                            case TypeCode.Byte:
                                Write((Byte)objs[i]);
                                break;
                            case TypeCode.Int16:
                                Write((Int16)objs[i]);
                                break;
                            case TypeCode.UInt16:
                                Write((UInt16)objs[i]);
                                break;
                            case TypeCode.Int32:
                                Write((Int32)objs[i]);
                                break;
                            case TypeCode.UInt32:
                                Write((UInt32)objs[i]);
                                break;
                            case TypeCode.Int64:
                                Write((Int64)objs[i]);
                                break;
                            case TypeCode.UInt64:
                                Write((UInt64)objs[i]);
                                break;
                            case TypeCode.Single:
                                Write((Single)objs[i]);
                                break;
                            case TypeCode.Double:
                                Write((Double)objs[i]);
                                break;
                            case TypeCode.Decimal:
                                Write((Decimal)objs[i]);
                                break;
                            case TypeCode.DateTime:
                                Write((DateTime)objs[i]);
                                break;
                            case TypeCode.String:
                                Write((String)objs[i]);
                                break;
                            default:
                                Write(objs[i].ToString());
                                break;
                        }


                        //switch (codes[i])
                        //{
                        //    case TypeCode.Boolean:
                        //        Write((Boolean)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.Char:
                        //        Write((Char)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.SByte:
                        //        Write((SByte)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.Byte:
                        //        Write((Byte)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.Int16:
                        //        Write((Int16)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.UInt16:
                        //        Write((UInt16)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.Int32:
                        //        Write((Int32)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.UInt32:
                        //        Write((UInt32)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.Int64:
                        //        Write((Int64)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.UInt64:
                        //        Write((UInt64)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.Single:
                        //        Write((Single)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.Double:
                        //        Write((Double)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.Decimal:
                        //        Write((Decimal)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.DateTime:
                        //        Write((DateTime)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    case TypeCode.String:
                        //        Write((String)value.Rows[j].ItemArray[i]);
                        //        break;
                        //    default:
                        //        Write(value.Rows[j].ItemArray[i].ToString());
                        //        break;
                        //}

                        //switch (codes[i])
                        //{
                        //    case TypeCode.Boolean:
                        //        Write((Boolean)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.Char:
                        //        Write((Char)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.SByte:
                        //        Write((SByte)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.Byte:
                        //        Write((Byte)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.Int16:
                        //        Write((Int16)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.UInt16:
                        //        Write((UInt16)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.Int32:
                        //        Write((Int32)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.UInt32:
                        //        Write((UInt32)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.Int64:
                        //        Write((Int64)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.UInt64:
                        //        Write((UInt64)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.Single:
                        //        Write((Single)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.Double:
                        //        Write((Double)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.Decimal:
                        //        Write((Decimal)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.DateTime:
                        //        Write((DateTime)value.Rows[j][i]);
                        //        break;
                        //    case TypeCode.String:
                        //        Write((String)value.Rows[j][i]);
                        //        break;
                        //    default:
                        //        Write(value.Rows[j][i].ToString());
                        //        break;
                        //}
                    }
                    WriteEndObject();
                }
                //position--;
                WriteEndArray();


            }
        }

        internal void Write(DataSet value)
        {
            if (value == null)
                WriteNull();
            else if (value.Tables.Count == 0)
                WriteZeroObject();
            else
            {
                foreach (DataTable table in value.Tables)
                {
                    WriteStartObject();
                    Write(value);
                    WriteEndObject();
                }
            }
        }

        #endregion

        #region Write ArraySegment

        //internal void Write(ArraySegment<bool> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<char> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_CHAR_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<byte> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_BYTE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<sbyte> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_SBYTE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<short> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_SHORT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<ushort> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_USHORT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<int> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_INT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<uint> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_UINT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<long> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_LONG_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<ulong> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_ULONG_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<float> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_FLOAT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<double> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DOUBLE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<decimal> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DECIMAL_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<DateTime> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DATETIME_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<TimeSpan> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_TIMESPAN_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<DateTimeOffset> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DATETIMEOFFSET_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<Guid> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_GUID_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<Uri> value)
        //{
        //    if (value.Count == 0)
        //        WriteZeroArray();
        //    else
        //    {
        //        ResizeAndWriteName(value.Count * SizeConsts.ARRAY_BASE_SIZE + SizeConsts.ARRAY_BASE_SIZE);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(ArraySegment<string> value)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region Write Nullable

        internal void Write(Nullable<bool> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<char> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<byte> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<sbyte> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<short> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<ushort> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<int> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<uint> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<long> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<ulong> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<float> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<double> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<decimal> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<DateTime> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<TimeSpan> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<DateTimeOffset> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        internal void Write(Nullable<Guid> value)
        {
            if (value == null)
                WriteNull();
            else
                Write(value.Value);
        }

        #endregion

        #region Write IList

        internal void Write(bool[] value)
        {
            WriteList<bool>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Boolean);
        }

        internal void Write(byte[] value)
        {
            WriteList<byte>(value, SizeConsts.VALUETYPE_BYTE_MAX_LENGTH, TypeCodes.Byte);
        }

        internal void Write(sbyte[] value)
        {
            WriteList<sbyte>(value, SizeConsts.VALUETYPE_SBYTE_MAX_LENGTH, TypeCodes.SByte);
        }

        internal void Write(char[] value)
        {
            WriteList<char>(value, SizeConsts.VALUETYPE_CHAR_MAX_LENGTH, TypeCodes.Char);
        }

        internal void Write(short[] value)
        {
            WriteList<short>(value, SizeConsts.VALUETYPE_SHORT_MAX_LENGTH, TypeCodes.Int16);
        }

        internal void Write(ushort[] value)
        {
            WriteList<ushort>(value, SizeConsts.VALUETYPE_USHORT_MAX_LENGTH, TypeCodes.UInt16);
        }

        internal void Write(int[] value)
        {
            //if (value == null)
            //    WriteNull();
            //else if (value.Length == 0)
            //    WriteZeroArray();
            //else
            //{
            //    ResizeAndWriteName(value.Length * SizeConsts.VALUETYPE_INT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
            //    InternalWrite(value);
            //}
            WriteList<int>(value, SizeConsts.VALUETYPE_INT_MAX_LENGTH, TypeCodes.Int32);
        }

        internal void Write(uint[] value)
        {
            WriteList<uint>(value, SizeConsts.VALUETYPE_UINT_MAX_LENGTH, TypeCodes.UInt32);
        }

        internal void Write(long[] value)
        {
            WriteList<long>(value, SizeConsts.VALUETYPE_LONG_MAX_LENGTH, TypeCodes.Int64);
        }

        internal void Write(ulong[] value)
        {
            WriteList<ulong>(value, SizeConsts.VALUETYPE_ULONG_MAX_LENGTH, TypeCodes.UInt64);
        }

        internal void Write(float[] value)
        {
            WriteList<float>(value, SizeConsts.VALUETYPE_FLOAT_MAX_LENGTH, TypeCodes.Single);
        }

        internal void Write(double[] value)
        {
            WriteList<double>(value, SizeConsts.VALUETYPE_DOUBLE_MAX_LENGTH, TypeCodes.Double);
        }

        internal void Write(decimal[] value)
        {
            WriteList<decimal>(value, SizeConsts.VALUETYPE_DECIMAL_MAX_LENGTH, TypeCodes.Decimal);
        }

        internal void Write(string[] value)
        {
            WriteList<string>(value, SizeConsts.VALUETYPE_STRING_MAX_LENGTH, TypeCodes.String);
        }

        internal void Write(DateTime[] value)
        {
            WriteList<DateTime>(value, SizeConsts.VALUETYPE_DATETIME_MAX_LENGTH, TypeCodes.DateTime);
        }

        internal void Write(DateTimeOffset[] value)
        {
            WriteList<DateTimeOffset>(value, SizeConsts.VALUETYPE_DATETIMEOFFSET_MAX_LENGTH, TypeCodes.DateTimeOffset);
        }

        internal void Write(TimeSpan[] value)
        {
            WriteList<TimeSpan>(value, SizeConsts.VALUETYPE_TIMESPAN_MAX_LENGTH, TypeCodes.TimeSpan);
        }

        internal void Write(Guid[] value)
        {
            WriteList<Guid>(value, SizeConsts.VALUETYPE_GUID_MAX_LENGTH, TypeCodes.Guid);
        }

        internal void Write(Enum[] value)
        {
            WriteList<Enum>(value, SizeConsts.VALUETYPE_ENUM_MAX_LENGTH, TypeCodes.Enum);
        }

        internal void Write(Uri[] value)
        {
            WriteList<Uri>(value, SizeConsts.VALUETYPE_URI_MAX_LENGTH, TypeCodes.Uri);
        }



        internal void Write(List<bool> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<char> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_CHAR_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<byte> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_BYTE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<sbyte> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_SBYTE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<short> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_SHORT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<ushort> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_USHORT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<int> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_INT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<uint> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_UINT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<long> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_LONG_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<ulong> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_ULONG_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<float> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_FLOAT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<double> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DOUBLE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<decimal> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DECIMAL_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<string> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<DateTime> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DATETIME_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<DateTimeOffset> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DATETIMEOFFSET_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<TimeSpan> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_TIMESPAN_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<Guid> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_GUID_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<Enum> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(List<Uri> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }




        internal void Write(IList<bool> value)
        {
            WriteList<bool>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Boolean);
        }

        internal void Write(IList<char> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_CHAR_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<byte> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_BYTE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<sbyte> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_SBYTE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<short> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_SHORT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<ushort> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_USHORT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<int> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_INT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<uint> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_UINT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<long> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_LONG_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<ulong> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_ULONG_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<float> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_FLOAT_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<double> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DOUBLE_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<decimal> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DECIMAL_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<string> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<DateTime> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DATETIME_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<DateTimeOffset> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_DATETIMEOFFSET_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<TimeSpan> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_TIMESPAN_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<Guid> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(value.Count * SizeConsts.VALUETYPE_GUID_MAX_LENGTH + SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<Enum> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        internal void Write(IList<Uri> value)
        {
            if (value == null)
                WriteNull();
            else if (value.Count == 0)
                WriteZeroArray();
            else
            {
                ResizeAndWriteName(SizeConsts.ARRAY_BASE_SIZE);
                InternalWrite(value);
            }
        }

        #endregion

        #region Write IDictionary

        internal void Write(IDictionary<int, int> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, bool> value)
        {
            if (names.Length > 0 && isRoot == false)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, char> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, byte> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, sbyte> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, short> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, ushort> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, int> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, uint> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, long> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, ulong> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, float> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, double> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, decimal> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, string> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        internal void Write(IDictionary<string, object> value)
        {
            if (names.Length > 0)
            {
                if (value == null)
                    WriteNull();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    _buffer[position++] = '{';
                    InternalWrite(value);
                    _buffer[position - 1] = '}';
                    _buffer[position++] = ',';
                }
            }
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObjectWithoutName();
                else
                {
                    if (isRoot == true)
                    {
                        _buffer[position++] = '{';
                        Resize(10);
                        InternalWrite(value);
                        _buffer[position - 1] = '}';
                        _buffer[position++] = ',';
                    }
                    else
                    {
                        Resize(10);
                        InternalWrite(value);
                    }
                }
            }
        }

        #endregion

        #region Write IEnumerable

        internal void Write(IEnumerable<bool> value)
        {
            WriteList<bool>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Boolean);
        }

        internal void Write(IEnumerable<byte> value)
        {
            WriteList<byte>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Byte);
        }

        internal void Write(IEnumerable<sbyte> value)
        {
            WriteList<sbyte>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.SByte);
        }

        internal void Write(IEnumerable<char> value)
        {
            WriteList<char>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Char);
        }

        internal void Write(IEnumerable<short> value)
        {
            WriteList<short>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Int16);
        }

        internal void Write(IEnumerable<ushort> value)
        {
            WriteList<ushort>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.UInt16);
        }

        internal void Write(IEnumerable<int> value)
        {
            WriteList<int>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Int32);
        }

        internal void Write(IEnumerable<uint> value)
        {
            WriteList<uint>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.UInt32);
        }

        internal void Write(IEnumerable<long> value)
        {
            WriteList<long>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Int64);
        }

        internal void Write(IEnumerable<ulong> value)
        {
            WriteList<ulong>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.UInt64);
        }

        internal void Write(IEnumerable<float> value)
        {
            WriteList<float>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Single);
        }

        internal void Write(IEnumerable<double> value)
        {
            WriteList<double>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Double);
        }

        internal void Write(IEnumerable<decimal> value)
        {
            WriteList<decimal>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Decimal);
        }

        internal void Write(IEnumerable<string> value)
        {
            WriteList<string>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.String);
        }

        internal void Write(IEnumerable<DateTime> value)
        {
            WriteList<DateTime>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.DateTime);
        }

        internal void Write(IEnumerable<DateTimeOffset> value)
        {
            WriteList<DateTimeOffset>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.DateTimeOffset);
        }

        internal void Write(IEnumerable<TimeSpan> value)
        {
            WriteList<TimeSpan>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.TimeSpan);
        }

        internal void Write(IEnumerable<Guid> value)
        {
            WriteList<Guid>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Guid);
        }

        internal void Write(IEnumerable<Enum> value)
        {
            WriteList<Enum>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Enum);
        }

        internal void Write(IEnumerable<Uri> value)
        {
            WriteList<Uri>(value, SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH, TypeCodes.Uri);
        }

        #endregion

        #region Write Other

        internal void Write(object value)
        {
            if (value == null)
            {
                WriteNull();
                //跳过中间的对象
                current += nameCounts[curObj];
                curObj += typeCounts[curObj] + 1;
            }
            else
            {
                ResizeAndWriteName(10);

                //if (curDepth >= maxDepth)
                //    throw new Exception(string.Format(ExceptionConsts.MaxDepth, curDepth));
                curDepth++;
                ShiboJsonStringSerializer.Serialize(this, value, sers[curObj++]);
                _buffer[position] = ',';
                position++;
            }
        }

        internal void WriteObject(object value)
        {
            if (value == null)
            {
                WriteNull();
                //跳过中间的对象
                current += nameCounts[curObj];
                curObj += typeCounts[curObj] + 1;
            }
            else
            {
                ResizeAndWriteName(10);

                //if (curDepth >= maxDepth)
                //    throw new Exception(string.Format(ExceptionConsts.MaxDepth, curDepth));
                curDepth++;
                ShiboJsonStringSerializer.LoopSerialize(this, value);
            }
        }

        internal void Write(object[] value)
        {
            if (value == null)
                WriteNull();
            else if (value.Length == 0)
                WriteZeroArray();
            else
            {
                //ResizeAndWriteName(value.Length * 10 +SizeConsts.VALUETYPE_LEN );
                //Write((IEnumerable<int>)value);

                ResizeAndWriteName(10);
                _buffer[position] = '[';
                position++;
                foreach (object o in value)
                {
                    ShiboJsonStringSerializer.LoopSerialize(this, o);
                }
                //直接覆盖掉最后一个“,”
                _buffer[position - 1] = ']';
                _buffer[position] = ',';
                position++;
            }
        }

        internal void Write(IList value)
        {
            if (value == null)
                WriteNull();
            else
            {
                Type type = value.GetType();
                if (type.GetGenericArguments().Length > 0)
                {
                    Type argType = type.GetGenericArguments()[0];
                    if (argType == typeof(object))
                        WriteObjectEnumerable((IEnumerable)value);
                    else if (argType.IsPrimitive)
                    {
                        if (value is short[])
                            Write((short[])value);
                        else if (value is ushort[])
                            Write((ushort[])value);
                        else if (value is int[])
                            Write((int[])value);
                        else if (value is uint[])
                            Write((uint[])value);
                        else if (value is long[])
                            Write((long[])value);
                        else if (value is ulong[])
                            Write((ulong[])value);
                        else if (value is float[])
                            Write((float[])value);
                        else if (value is double[])
                            Write((double[])value);
                        else if (value is decimal[])
                            Write((decimal[])value);
                        else if (value is bool[])
                            Write((bool[])value);
                        else if (value is sbyte[])
                            Write((sbyte[])value);
                    }
                    else
                        WriteTList(value);
                }
                else if (type.IsArray)
                {
                    if (type != typeof(object[]))
                        WriteTList(value);
                    else
                        WriteObjectEnumerable((IEnumerable)value);
                }
                else
                {
                    if (value is short[])
                        Write((short[])value);
                    else if (value is ushort[])
                        Write((ushort[])value);
                    else if (value is int[])
                        Write((int[])value);
                    else if (value is uint[])
                        Write((uint[])value);
                    else if (value is long[])
                        Write((long[])value);
                    else if (value is ulong[])
                        Write((ulong[])value);
                    else if (value is float[])
                        Write((float[])value);
                    else if (value is double[])
                        Write((double[])value);
                    else if (value is decimal[])
                        Write((decimal[])value);
                    else if (value is bool[])
                        Write((bool[])value);
                    else if (value is sbyte[])
                        Write((sbyte[])value);
                    else if (value is DateTime[])
                        Write((DateTime[])value);
                    else if (value is TimeSpan[])
                        Write((TimeSpan[])value);
                    else if (value is Guid[])
                        Write((Guid[])value);
                    else if (value is DateTimeOffset[])
                        Write((DateTimeOffset[])value);
                    else if (value is Uri[])
                        Write((Uri[])value);
                    else
                        WriteObjectEnumerable(value);
                }
            }
        }

        internal void Write(IDictionary value)
        {
            if (value == null)
                WriteNull();
            else
            {
                if (value == null)
                    WriteNullWithoutName();
                else if (value.Count == 0)
                    WriteZeroObject();
                else
                {
                    ResizeAndWriteName(10);
                    //_buffer[position] = '{';
                    //position++;
                    Type type = value.GetType();
                    Type[] args = type.GetGenericArguments();
                    if (args.Length == 2)
                    {
                        //对于Json而言，map的key都将变成string
                        if (args[0] == TypeConsts.String)
                            WriteStringKeyDictionary(args[1], value);
                        else
                            WriteObjectKeyDictionary(args[1], value);
                    }
                    else
                    {

                    }
                    //if (value.Count > 0)
                    //    position--;
                    //_buffer[position] = '}';
                    //_buffer[position + 1] = ',';
                    //position += 2;
                }
            }
        }

        internal void Write(IEnumerable value)
        {
            if (value == null)
                WriteNull();
            else
            {
                if (value is IList)
                    Write((IList)value);
                else if (value is IDictionary)
                    Write((IDictionary)value);
                else
                    WriteObjectEnumerable(value);
            }
        }

        #endregion

        #region 公共

        public override string ToString()
        {
            //if (_buffer[position - 1] == ',')
            //    position--;
            return new string(_buffer, 0, position);
        }

        public ArraySegment<char> ToArraySegment()
        {
            if (_buffer[position - 1] == ',')
                position--;
            return new ArraySegment<char>(_buffer, 0, position);
        }

        public void WriteTo(Stream stream)
        {
            //if (_buffer[position - 1] == ',')
            //    position--;

            //if (unFlag == true)
            //{
            //    if (stream is MemoryStream)
            //    {
            //        stream.Write(_buffer, 1, position - 1);
            //    }
            //    else if (stream is FileStream)
            //    {
            //        stream.Write(_buffer, 1, position - 1);
            //    }
            //    else
            //    {
            //        stream.Write(_buffer, 1, position - 1);
            //    }
            //}
            //else
            //{
            //    if (stream is MemoryStream)
            //    {
            //        stream.Write(_buffer, 0, position);
            //    }
            //    else if (stream is FileStream)
            //    {
            //        stream.Write(_buffer, 0, position);
            //    }
            //    else
            //    {
            //        stream.Write(_buffer, 0, position);
            //    }
            //}
        }

        public char[] GetBuffer()
        {
            return _buffer;
        }

        public char[] ToArray()
        {
            char[] temp = new char[position];
            Buffer.BlockCopy(_buffer, 0, temp, 0, position << 1);
            return temp;
        }

        #endregion

        #region 独立私有

        private static int GetSize(string[] value, StringEscape escape)
        {
            int size = 0;
            if (escape == StringEscape.Default)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] != null)
                        size += value[i].Length;
                    else
                        size += 4;
                }
            }
            //else if(escape == StringEscape.EscapeHtml)
            //{
            //    for (int i = 0; i < value.Length; i++)
            //    {
            //        if (value[i] != null)
            //            size += value[i].Length;
            //        else
            //            size += 4;
            //    }
            //}
            else if (escape == StringEscape.EscapeNonAscii)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] != null)
                        size += value[i].Length * 6;
                    else
                        size += 4;
                }
            }
            return size + (value.Length * 3) + 2;
        }

        private static int GetSize(IList<string> value, StringEscape escape)
        {
            int size = 0;
            if (escape == StringEscape.Default)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i] != null)
                        size += value[i].Length;
                    else
                        size += 4;
                }
            }
            //else if(escape == StringEscape.EscapeHtml)
            //{
            //    for (int i = 0; i < value.Length; i++)
            //    {
            //        if (value[i] != null)
            //            size += value[i].Length;
            //        else
            //            size += 4;
            //    }
            //}
            else if (escape == StringEscape.EscapeNonAscii)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i] != null)
                        size += value[i].Length * 6;
                    else
                        size += 4;
                }
            }
            return size + (value.Count * 3) + 2;
        }

        private unsafe void WriteString(string value)
        {
            fixed (char* psrc = value, pdst = &_buffer[position])
            {
                char* tsrc = psrc, tdst = pdst;
                *tdst++ = Quote;
                int escCount = 0;
                if (sets.Escape == StringEscape.Default)
                {
                    if (sets.CheckJsonEscape == CheckEscape.None)
                    {
                        Utils.wstrcpy(tdst, tsrc, value.Length);
                        tdst += value.Length;
                    }
                    //else if (sets.CheckJsonEscape == CheckEscape.May)
                    //{
                    //    if (value.IndexOf('"') != -1)
                    //    {
                    //        escCount = Utils.CheckThenCopy(tdst, tsrc, value.Length);
                    //        tdst += value.Length + escCount;
                    //    }
                    //    else
                    //    {
                    //        Utils.wstrcpy(tdst, tsrc, value.Length);
                    //        tdst += value.Length;
                    //    }
                    //}
                    else if (sets.CheckJsonEscape == CheckEscape.Must)
                    {
                        escCount = Utils.CheckFullThenCopy(tdst, tsrc, value.Length);
                        tdst += value.Length + escCount;
                    }
                    else if (sets.CheckJsonEscape == CheckEscape.OnlyCheckQuote)
                    {
                        escCount = Utils.CheckThenCopy(tdst, tsrc, value.Length);
                        tdst += value.Length + escCount;
                    }
                    position += value.Length + 3 + escCount;
                }
                else if (sets.Escape == StringEscape.EscapeHtml)
                {
                    //Utils.wstrcpy(tdst, tsrc, value.Length);
                    //tdst += value.Length + escCount;
                    //position += value.Length + 3 + escCount;
                }
                else if (sets.Escape == StringEscape.EscapeNonAscii)
                {
                    int ecount = Utils.ToCharAsUnicode(tdst, tsrc, value.Length);
                    tdst += ecount * 6 + value.Length - ecount;
                    position += ecount * 6 + value.Length - ecount + 3;
                }
                *tdst++ = Quote;
                *tdst++ = ',';
            }
        }

        private unsafe void WriteString(string value,char next)
        {
            #region old
            //fixed (char* psrc = value, pdst = &_buffer[position])
            //{
            //    char* tsrc = psrc, tdst = pdst;
            //    *tdst++ = Quote;
            //    Utils.wstrcpy(tdst, tsrc, value.Length);
            //    tdst += value.Length;
            //    *tdst++ = Quote;
            //    *tdst++ = ',';
            //    position += value.Length + 3;
            //}
            #endregion

            fixed (char* psrc = value, pdst = &_buffer[position])
            {
                char* tsrc = psrc, tdst = pdst;
                *tdst++ = Quote;
                int escCount = 0;
                if (sets.Escape == StringEscape.Default)
                {
                    if (sets.CheckJsonEscape == CheckEscape.None)
                    {
                        Utils.wstrcpy(tdst, tsrc, value.Length);
                        tdst += value.Length;
                    }
                    //else if (sets.CheckJsonEscape == CheckEscape.May)
                    //{
                    //    if (value.IndexOf('"') != -1)
                    //    {
                    //        escCount = Utils.CheckThenCopy(tdst, tsrc, value.Length);
                    //        tdst += value.Length + escCount;
                    //    }
                    //    else
                    //    {
                    //        Utils.wstrcpy(tdst, tsrc, value.Length);
                    //        tdst += value.Length;
                    //    }
                    //}
                    else if (sets.CheckJsonEscape == CheckEscape.Must)
                    {
                        escCount = Utils.CheckFullThenCopy(tdst, tsrc, value.Length);
                        tdst += value.Length + escCount;
                    }
                    else if (sets.CheckJsonEscape == CheckEscape.OnlyCheckQuote)
                    {
                        escCount = Utils.CheckThenCopy(tdst, tsrc, value.Length);
                        tdst += value.Length + escCount;
                    }
                    position += value.Length + 3 + escCount;
                }
                //else if (sets.Escape == StringEscape.EscapeHtml)
                //{
                //    Utils.wstrcpy(buffer, chs, value.Length);
                //    buffer += value.Length;
                //    position += value.Length + 3 + escCount;
                //}
                else if (sets.Escape == StringEscape.EscapeNonAscii)
                {
                    int ecount = Utils.ToCharAsUnicode(tdst, tsrc, value.Length);
                    tdst += ecount * 6 + value.Length - ecount;
                    position += ecount * 6 + value.Length - ecount + 3;
                }
                *tdst++ = Quote;
                *tdst++ = next;
            }
        }

        #endregion

        #region 接口

        //void Write(bool value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(byte value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(sbyte value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(short value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ushort value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(int value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(uint value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(long value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ulong value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(float value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(double value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(decimal value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(char value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(string value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(DateTime value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(DateTimeOffset value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(TimeSpan value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(Guid value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(Uri value)
        //{
        //    throw new NotImplementedException();
        //}




        //void Write(ArraySegment<bool> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<byte> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<sbyte> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<short> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<ushort> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<int> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<uint> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<long> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<ulong> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<float> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<double> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<decimal> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<char> value)
        //{
        //    throw new NotImplementedException();
        //}



        //void Write(ArraySegment<DateTime> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<DateTimeOffset> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<TimeSpan> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<Guid> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ArraySegment<Uri> value)
        //{
        //    throw new NotImplementedException();
        //}




        //void Write(bool[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(byte[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(sbyte[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(short[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ushort[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(int[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(uint[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(long[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(ulong[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(float[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(double[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(decimal[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(char[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(string[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(DateTime[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(DateTimeOffset[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(TimeSpan[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(Guid[] value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(Uri[] value)
        //{
        //    throw new NotImplementedException();
        //}





        //void Write(IList<bool> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<byte> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<sbyte> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<short> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<ushort> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<int> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<uint> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<long> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<ulong> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<float> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<double> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<decimal> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<char> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<string> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<DateTime> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<DateTimeOffset> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<TimeSpan> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<Guid> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IList<Uri> value)
        //{
        //    throw new NotImplementedException();
        //}





        //void Write(IEnumerable<bool> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<byte> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<sbyte> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<short> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<ushort> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<int> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<uint> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<long> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<ulong> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<float> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<double> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<decimal> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<char> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<string> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<DateTime> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<DateTimeOffset> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<TimeSpan> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<Guid> value)
        //{
        //    throw new NotImplementedException();
        //}

        //void Write(IEnumerable<Uri> value)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

    }
}
