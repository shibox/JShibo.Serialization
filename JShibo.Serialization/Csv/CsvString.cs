using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using JShibo.Serialization.Common;
using JShibo.Serialization.Json;

namespace JShibo.Serialization.Csv
{
    public class CsvString
    {
        #region 常量

        //const char OBJECT_START = '{';
        //const char OBJECT_END = '}';
        //const char ARRAY_START = '[';
        //const char ARRAY_END = ']';
        const char COLON = ':';
        const char COMMA = ',';
        const char Separator=',';

        #endregion

        #region 字段

        internal string json = string.Empty;
        internal Type[] types;
        internal Serialize<CsvString>[] sers;
        internal int[] typeCounts;
        internal int[] nameCounts;

        internal string[] names = new string[0];
        internal char[] _buffer = null;
        internal int position = 0;
        internal int current = 0;
        internal int currSer = 0;
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

        public CsvString()
            : this(64)
        {
        }

        public CsvString(int capacity)
            : this(capacity, SerializerSettings.Default)
        {
        }

        public CsvString(char[] buffer)
            : this(buffer, SerializerSettings.Default)
        {
        }

        public CsvString(ref char[] buffer)
            : this(ref buffer, SerializerSettings.Default)
        {
        }

        public CsvString(char[] buffer, SerializerSettings set)
        {
            _buffer = buffer;
            sets = set;
        }

        public CsvString(ref char[] buffer, SerializerSettings set)
        {
            _buffer = buffer;
            sets = set;
        }

        public CsvString(int capacity, SerializerSettings set)
        {
            _buffer = new char[capacity];
            sets = set;
        }

        public CsvString(string json)
            : this(json, SerializerSettings.Default)
        {
        }

        public CsvString(string json, SerializerSettings set)
        {
            this.json = json;
            sets = set;
        }

        internal CsvString(CsvString stream)
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
            if (_buffer.Length < position + size)
                Resize(size);
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

        internal void SetInfo(CsvStringContext info)
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
                _buffer[position + 4] = Separator;
                position += 5;
            }
        }

        internal void WriteZeroArray()
        {
            ResizeAndWriteName(SizeConsts.ZERO_ARRAY_SIZE);
            _buffer[position] = '[';
            _buffer[position + 1] = ']';
            _buffer[position + 2] = Separator;
            position += 3;
        }

        internal void WriteNullWithoutName()
        {
            Resize(5);
            _buffer[position] = 'n';
            _buffer[position + 1] = 'u';
            _buffer[position + 2] = 'l';
            _buffer[position + 3] = 'l';
            _buffer[position + 4] = Separator;
            position += 5;
        }

        internal void WriteZeroArrayWithoutName()
        {
            _buffer[position] = '[';
            _buffer[position + 1] = ']';
            _buffer[position + 2] = Separator;
            position += 3;
        }

        internal void WriteZeroObject()
        {
            ResizeAndWriteName(10);
            _buffer[position] = '{';
            _buffer[position + 1] = '}';
            _buffer[position + 2] = Separator;
            position += 3;
        }

        internal void WriteZeroObjectWithoutName()
        {
            Resize(10);
            _buffer[position] = '{';
            _buffer[position + 1] = '}';
            _buffer[position + 2] = Separator;
            position += 3;
        }

        internal void WriteZeroString()
        {
            ResizeAndWriteName(10);
            _buffer[position] = Quote;
            _buffer[position + 1] = Quote;
            _buffer[position + 2] = Separator;
            position += 3;
        }

        internal void WriteZeroStringWithoutName()
        {
            //ResizeAndWriteName(10);
            _buffer[position] = Quote;
            _buffer[position + 1] = Quote;
            _buffer[position + 2] = Separator;
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
            _buffer[position] = Separator;
            position++;
        }

        internal void WriteEndObject()
        {
            _buffer[position - 1] = '}';
            _buffer[position] = Separator;
            position++;
        }

        private unsafe char* InternalWrite(char* buffer, string value)
        {
            if (value == null)
            {
                *buffer++ = 'n';
                *buffer++ = 'u';
                *buffer++ = 'l';
                *buffer++ = 'l';
                *buffer++ = Separator;
                position += 5;
                return buffer;
            }
            else if (value.Length == 0)
            {
                *buffer++ = Quote;
                *buffer++ = Quote;
                *buffer++ = Separator;
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
                *buffer++ = Separator;
            }
            return buffer;
        }

        internal unsafe void WriteHeader(string[] headers)
        {
            fixed (char* pdst = &_buffer[position])
            {
                char* tdst = pdst;
                for (int i = 0; i < headers.Length; i++)
                {
                    int len = headers[i].Length;
                    fixed (char* psrc=headers[i])
                    {
                        Utils.wstrcpy(tdst, psrc, len);
                        tdst += len;
                        *tdst++ = ' ';
                        position += len + 1;
                    }
                }
                *tdst++ = '\r';
                *tdst++ = '\n';
                position += 2;
            }
        }

        internal void WriteNewLine()
        {
            this._buffer[position-1] = '\r';
            this._buffer[position] = '\n';
            position++;
        }

        #endregion

        #region base type

        internal void Write(object value)
        {
            if (value != null)
            {
                Type type = value.GetType();
                if (type == TypeConsts.String)
                    Write((string)value);
                else if (type == TypeConsts.Int32)
                {
                    Resize(SizeConsts.VALUETYPE_INT_MAX_LENGTH);
                    Write((int)value);
                }
                else if (type == TypeConsts.Int16)
                {
                    Resize(SizeConsts.VALUETYPE_SHORT_MAX_LENGTH);
                    Write((short)value);
                }
                else if (type == TypeConsts.Int64)
                {
                    Resize(SizeConsts.VALUETYPE_LONG_MAX_LENGTH);
                    Write((long)value);
                }
                else if (type == TypeConsts.Boolean)
                {
                    Resize(SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH);
                    Write((bool)value);
                }
                else if (type == TypeConsts.Byte)
                {
                    Resize(SizeConsts.VALUETYPE_BYTE_MAX_LENGTH);
                    Write((byte)value);
                }
                else if (type == TypeConsts.SByte)
                {
                    Resize(SizeConsts.VALUETYPE_SBYTE_MAX_LENGTH);
                    Write((sbyte)value);
                }
                else if (type == TypeConsts.UInt16)
                {
                    Resize(SizeConsts.VALUETYPE_USHORT_MAX_LENGTH);
                    Write((ushort)value);
                }
                else if (type == TypeConsts.UInt32)
                {
                    Resize(SizeConsts.VALUETYPE_UINT_MAX_LENGTH);
                    Write((uint)value);
                }
                else if (type == TypeConsts.UInt64)
                {
                    Resize(SizeConsts.VALUETYPE_ULONG_MAX_LENGTH);
                    Write((ulong)value);
                }
                else if (type == TypeConsts.Char)
                {
                    Resize(SizeConsts.VALUETYPE_CHAR_MAX_LENGTH);
                    Write((char)value);
                }
                else if (type == TypeConsts.Single)
                {
                    Resize(SizeConsts.VALUETYPE_FLOAT_MAX_LENGTH);
                    Write((float)value);
                }
                else if (type == TypeConsts.Double)
                {
                    Resize(SizeConsts.VALUETYPE_DOUBLE_MAX_LENGTH);
                    Write((double)value);
                }
                else if (type == TypeConsts.Decimal)
                {
                    Resize(SizeConsts.VALUETYPE_DECIMAL_MAX_LENGTH);
                    Write((decimal)value);
                }
                else if (type == TypeConsts.DateTime)
                {
                    Resize(SizeConsts.VALUETYPE_DATETIME_MAX_LENGTH);
                    Write((DateTime)value);
                }
                else if (type == TypeConsts.TimeSpan)
                {
                    Resize(SizeConsts.VALUETYPE_TIMESPAN_MAX_LENGTH);
                    Write((TimeSpan)value);
                }
                else if (type == TypeConsts.DateTimeOffset)
                {
                    Resize(SizeConsts.VALUETYPE_DATETIMEOFFSET_MAX_LENGTH);
                    Write((DateTimeOffset)value);
                }
                else if (type == TypeConsts.Guid)
                {
                    Resize(SizeConsts.VALUETYPE_GUID_MAX_LENGTH);
                    Write((Guid)value);
                }
                else if (type == TypeConsts.Uri)
                {
                    Write((Uri)value);
                }
                //else if (type == TypeConsts.Enum)
                //    Write((Enum)value);
                //else if (type == TypeConsts.DataTable)
                //    Write((DataTable)value);
                else
                    WriteObject(value);
            }
            else
                WriteNull();
        }

        internal void Write(byte value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = Separator;
            position++;
        }

        internal void Write(sbyte value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = Separator;
            position++;
        }

        internal void Write(short value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = Separator;
            position++;
        }

        internal void Write(ushort value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = Separator;
            position++;
        }

        internal void Write(int value)
        {
            //if (sets.NumericCheck == NumericCheckType.Middle)
            //    position += FastToString.ToString(_buffer, position, value);
            //else if (sets.NumericCheck == NumericCheckType.Max)
            //    position += FastToString.ToStringMax(_buffer, position, value);
            //else
            //    position += FastToString.ToStringMin(_buffer, position, value);

            //this._buffer[position] = Separator;
            //position++;

            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = Separator;
            position++;
        }

        internal void Write(uint value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = Separator;
            position++;
        }

        internal void Write(long value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = Separator;
            position++;
        }

        internal void Write(ulong value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = Separator;
            position++;
        }

        internal void Write(float value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = Separator;
            position++;
        }

        internal void Write(double value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = Separator;
            position++;
        }

        internal void Write(decimal value)
        {
            position += FastToString.ToString(_buffer, position, value);
            this._buffer[position] = Separator;
            position++;
        }

        internal void Write(string value)
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

        internal void Write(bool value)
        {
            if (value)
            {
                _buffer[position] = 't';
                _buffer[position + 1] = 'r';
                _buffer[position + 2] = 'u';
                _buffer[position + 3] = 'e';
                _buffer[position + 4] = Separator;
                position += 5;
            }
            else
            {
                _buffer[position] = 'f';
                _buffer[position + 1] = 'a';
                _buffer[position + 2] = 'l';
                _buffer[position + 3] = 's';
                _buffer[position + 4] = 'e';
                _buffer[position + 5] = Separator;
                position += 6;
            }
        }

        internal void Write(char value)
        {
            _buffer[position] = Quote;
            _buffer[position + 1] = value;
            _buffer[position + 2] = Quote;
            this._buffer[position + 3] = Separator;
            position += 4;
        }

        internal unsafe void Write(DateTime value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = Quote;
                p = FastToString.ToString(p, ref position, value);
                *p++ = Quote;
                *p++ = Separator;
                position += 3;
            }
        }

        internal unsafe void Write(DateTimeOffset value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = Quote;
                p = FastToString.ToString(p, ref position, value);
                *p++ = Quote;
                *p++ = Separator;
                position += 3;
            }
        }

        internal unsafe void Write(TimeSpan value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = Quote;
                p = FastToString.ToString(p, ref position, value);
                *p++ = Quote;
                *p++ = Separator;
                position += 3;
            }
        }

        internal unsafe void Write(Guid value)
        {
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                *p++ = Quote;
                p = FastToString.ToString(p, ref position, value);
                *p++ = Quote;
                *p++ = Separator;
                position += 3;
            }
        }

        internal unsafe void Write(Uri value)
        {
            Resize((value.AbsoluteUri.Length) + 10);
            fixed (char* pd = &_buffer[position])
            {
                char* p = pd;
                p = FastToString.ToString(p, ref position, value);
                *p++ = '\r';
                *p++ = '\n';
                position += 2;
            }
        }

        #endregion

        #region Write BaseType

        //internal void Write(bool value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_BOOLEAN_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(char value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_CHAR_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(byte value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_BYTE_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(sbyte value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_SBYTE_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(short value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_SHORT_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(ushort value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_USHORT_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(int value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_INT_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(uint value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_UINT_MAX_LENGTH);
        //    position += FastToString.ToString(_buffer, position, value);
        //    this._buffer[position] = Separator;
        //    position++;
        //}

        //internal void Write(long value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_LONG_MAX_LENGTH);

        //    //if (sets.NumericCheck == NumericCheckType.Middle)
        //    //    position += FastToString.ToString(_buffer, position, value);
        //    //else if (sets.NumericCheck == NumericCheckType.Max)
        //    //    position += FastToString.ToStringMax(_buffer, position, value);
        //    //else
        //    //    position += FastToString.ToStringMin(_buffer, position, value);

        //    position += FastToString.ToString(_buffer, position, value);
        //    this._buffer[position] = Separator;
        //    position++;
        //}

        //internal void Write(ulong value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_ULONG_MAX_LENGTH);

        //    //if (sets.NumericCheck == NumericCheckType.Middle)
        //    //    position += FastToString.ToString(_buffer, position, value);
        //    //else if (sets.NumericCheck == NumericCheckType.Max)
        //    //    position += FastToString.ToStringMax(_buffer, position, value);
        //    //else
        //    //    position += FastToString.ToStringMin(_buffer, position, value);

        //    position += FastToString.ToString(_buffer, position, value);
        //    this._buffer[position] = Separator;
        //    position++;
        //}

        //internal void Write(float value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_FLOAT_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(double value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_DOUBLE_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(decimal value)
        //{
        //    ResizeAndWriteName(SizeConsts.VALUETYPE_LEN + SizeConsts.VALUETYPE_DECIMAL_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(string value)
        //{
        //    if (value == null)
        //        WriteNull();
        //    else if (value.Length == 0)
        //        WriteZeroString();
        //    else
        //    {
        //        ResizeAndWriteName((value.Length * 2) + SizeConsts.CLASSTYPE_LEN);
        //        WriteString(value);
        //    }
        //}

        //internal void Write(DateTime value)
        //{
        //    ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + SizeConsts.VALUETYPE_DATETIME_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(TimeSpan value)
        //{
        //    ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + SizeConsts.VALUETYPE_TIMESPAN_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(DateTimeOffset value)
        //{
        //    ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + SizeConsts.VALUETYPE_DATETIMEOFFSET_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(Guid value)
        //{
        //    ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + SizeConsts.VALUETYPE_GUID_MAX_LENGTH);
        //    InternalWrite(value);
        //}

        //internal void Write(Uri value)
        //{
        //    if (value == null)
        //        WriteNull();
        //    else
        //    {
        //        ResizeAndWriteName(SizeConsts.CLASSTYPE_LEN + value.AbsoluteUri.Length);
        //        InternalWrite(value);
        //    }
        //}

        //internal void Write(DBNull value)
        //{
        //    WriteNull();
        //}

        //internal void Write(DataTable value)
        //{
        //    if (value == null)
        //        WriteNull();
        //    else if (value.Rows.Count == 0)
        //        WriteZeroObject();
        //    else
        //    {
        //        TypeCode[] codes = new TypeCode[value.Columns.Count];
        //        string[] colNames = new string[value.Columns.Count];
        //        for (int i = 0; i < codes.Length; i++)
        //        {
        //            if (value.Columns[i].DataType == typeof(Boolean))
        //                codes[i] = TypeCode.Boolean;
        //            else if (value.Columns[i].DataType == typeof(Char))
        //                codes[i] = TypeCode.Char;
        //            else if (value.Columns[i].DataType == typeof(SByte))
        //                codes[i] = TypeCode.SByte;
        //            else if (value.Columns[i].DataType == typeof(Byte))
        //                codes[i] = TypeCode.Byte;
        //            else if (value.Columns[i].DataType == typeof(Int16))
        //                codes[i] = TypeCode.Int16;
        //            else if (value.Columns[i].DataType == typeof(UInt16))
        //                codes[i] = TypeCode.UInt16;
        //            else if (value.Columns[i].DataType == typeof(Int32))
        //                codes[i] = TypeCode.Int32;
        //            else if (value.Columns[i].DataType == typeof(UInt32))
        //                codes[i] = TypeCode.UInt32;
        //            else if (value.Columns[i].DataType == typeof(Int64))
        //                codes[i] = TypeCode.Int64;
        //            else if (value.Columns[i].DataType == typeof(UInt64))
        //                codes[i] = TypeCode.UInt64;
        //            else if (value.Columns[i].DataType == typeof(Single))
        //                codes[i] = TypeCode.Single;
        //            else if (value.Columns[i].DataType == typeof(Double))
        //                codes[i] = TypeCode.Double;
        //            else if (value.Columns[i].DataType == typeof(Decimal))
        //                codes[i] = TypeCode.Decimal;
        //            else if (value.Columns[i].DataType == typeof(DateTime))
        //                codes[i] = TypeCode.DateTime;
        //            else if (value.Columns[i].DataType == typeof(String))
        //                codes[i] = TypeCode.String;
        //            else
        //                codes[i] = TypeCode.String;
        //            colNames[i] = value.Columns[i].ColumnName;
        //        }

        //        WriteStartArray();
        //        for (int j = 0; j < value.Rows.Count; j++)
        //        {
        //            WriteStartObject();
        //            object[] objs = value.Rows[j].ItemArray;
        //            for (int i = 0; i < codes.Length; i++)
        //            {
        //                ResizeAndWriteName(colNames[i]);
        //                switch (codes[i])
        //                {
        //                    case TypeCode.Boolean:
        //                        Write((Boolean)objs[i]);
        //                        break;
        //                    case TypeCode.Char:
        //                        Write((Char)objs[i]);
        //                        break;
        //                    case TypeCode.SByte:
        //                        Write((SByte)objs[i]);
        //                        break;
        //                    case TypeCode.Byte:
        //                        Write((Byte)objs[i]);
        //                        break;
        //                    case TypeCode.Int16:
        //                        Write((Int16)objs[i]);
        //                        break;
        //                    case TypeCode.UInt16:
        //                        Write((UInt16)objs[i]);
        //                        break;
        //                    case TypeCode.Int32:
        //                        Write((Int32)objs[i]);
        //                        break;
        //                    case TypeCode.UInt32:
        //                        Write((UInt32)objs[i]);
        //                        break;
        //                    case TypeCode.Int64:
        //                        Write((Int64)objs[i]);
        //                        break;
        //                    case TypeCode.UInt64:
        //                        Write((UInt64)objs[i]);
        //                        break;
        //                    case TypeCode.Single:
        //                        Write((Single)objs[i]);
        //                        break;
        //                    case TypeCode.Double:
        //                        Write((Double)objs[i]);
        //                        break;
        //                    case TypeCode.Decimal:
        //                        Write((Decimal)objs[i]);
        //                        break;
        //                    case TypeCode.DateTime:
        //                        Write((DateTime)objs[i]);
        //                        break;
        //                    case TypeCode.String:
        //                        Write((String)objs[i]);
        //                        break;
        //                    default:
        //                        Write(objs[i].ToString());
        //                        break;
        //                }


        //                //switch (codes[i])
        //                //{
        //                //    case TypeCode.Boolean:
        //                //        Write((Boolean)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.Char:
        //                //        Write((Char)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.SByte:
        //                //        Write((SByte)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.Byte:
        //                //        Write((Byte)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.Int16:
        //                //        Write((Int16)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.UInt16:
        //                //        Write((UInt16)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.Int32:
        //                //        Write((Int32)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.UInt32:
        //                //        Write((UInt32)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.Int64:
        //                //        Write((Int64)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.UInt64:
        //                //        Write((UInt64)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.Single:
        //                //        Write((Single)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.Double:
        //                //        Write((Double)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.Decimal:
        //                //        Write((Decimal)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.DateTime:
        //                //        Write((DateTime)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    case TypeCode.String:
        //                //        Write((String)value.Rows[j].ItemArray[i]);
        //                //        break;
        //                //    default:
        //                //        Write(value.Rows[j].ItemArray[i].ToString());
        //                //        break;
        //                //}

        //                //switch (codes[i])
        //                //{
        //                //    case TypeCode.Boolean:
        //                //        Write((Boolean)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.Char:
        //                //        Write((Char)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.SByte:
        //                //        Write((SByte)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.Byte:
        //                //        Write((Byte)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.Int16:
        //                //        Write((Int16)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.UInt16:
        //                //        Write((UInt16)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.Int32:
        //                //        Write((Int32)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.UInt32:
        //                //        Write((UInt32)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.Int64:
        //                //        Write((Int64)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.UInt64:
        //                //        Write((UInt64)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.Single:
        //                //        Write((Single)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.Double:
        //                //        Write((Double)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.Decimal:
        //                //        Write((Decimal)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.DateTime:
        //                //        Write((DateTime)value.Rows[j][i]);
        //                //        break;
        //                //    case TypeCode.String:
        //                //        Write((String)value.Rows[j][i]);
        //                //        break;
        //                //    default:
        //                //        Write(value.Rows[j][i].ToString());
        //                //        break;
        //                //}
        //            }
        //            WriteEndObject();
        //        }
        //        //position--;
        //        WriteEndArray();


        //    }
        //}

        //internal void Write(DataSet value)
        //{
        //    if (value == null)
        //        WriteNull();
        //    else if (value.Tables.Count == 0)
        //        WriteZeroObject();
        //    else
        //    {
        //        foreach (DataTable table in value.Tables)
        //        {
        //            WriteStartObject();
        //            Write(value);
        //            WriteEndObject();
        //        }
        //    }
        //}

        #endregion

        #region Write Other

        //internal void Write(object value)
        //{
        //    //if (value == null)
        //    //{
        //    //    WriteNull();
        //    //    //跳过中间的对象
        //    //    current += nameCounts[currSer];
        //    //    currSer += typeCounts[currSer] + 1;
        //    //}
        //    //else
        //    //{
        //    //    ResizeAndWriteName(10);

        //    //    //if (curDepth >= maxDepth)
        //    //    //    throw new Exception(string.Format(ExceptionConsts.MaxDepth, curDepth));
        //    //    curDepth++;
        //    //    ShiboCsvStringSerializer.Serialize(this, value, sers[currSer++]);
        //    //    _buffer[position] = Separator;
        //    //    position++;
        //    //}
        //}

        internal void WriteObject(object value)
        {
            if (value == null)
            {
                WriteNull();
                //跳过中间的对象
                current += nameCounts[currSer];
                currSer += typeCounts[currSer] + 1;
            }
            else
            {
                ResizeAndWriteName(10);

                //if (curDepth >= maxDepth)
                //    throw new Exception(string.Format(ExceptionConsts.MaxDepth, curDepth));
                curDepth++;
                //ShiboCsvStringSerializer.LoopSerialize(this, value);
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
                Utils.wstrcpy(tdst, tsrc, value.Length);
                tdst += value.Length;
                *tdst++ = Separator;
                position += value.Length + 1;
            }

            //fixed (char* psrc = value, pdst = &_buffer[position])
            //{
            //    char* tsrc = psrc, tdst = pdst;
            //    *tdst++ = Quote;
            //    int escCount = 0;
            //    if (sets.Escape == StringEscape.Default)
            //    {
            //        if (sets.CheckJsonEscape == CheckEscape.None)
            //        {
            //            Utils.wstrcpy(tdst, tsrc, value.Length);
            //            tdst += value.Length;
            //        }
            //        //else if (sets.CheckJsonEscape == CheckEscape.May)
            //        //{
            //        //    if (value.IndexOf('"') != -1)
            //        //    {
            //        //        escCount = Utils.CheckThenCopy(tdst, tsrc, value.Length);
            //        //        tdst += value.Length + escCount;
            //        //    }
            //        //    else
            //        //    {
            //        //        Utils.wstrcpy(tdst, tsrc, value.Length);
            //        //        tdst += value.Length;
            //        //    }
            //        //}
            //        else if (sets.CheckJsonEscape == CheckEscape.Must)
            //        {
            //            escCount = Utils.CheckFullThenCopy(tdst, tsrc, value.Length);
            //            tdst += value.Length + escCount;
            //        }
            //        else if (sets.CheckJsonEscape == CheckEscape.OnlyCheckQuote)
            //        {
            //            escCount = Utils.CheckThenCopy(tdst, tsrc, value.Length);
            //            tdst += value.Length + escCount;
            //        }
            //        position += value.Length + 3 + escCount;
            //    }
            //    else if (sets.Escape == StringEscape.EscapeHtml)
            //    {
            //        //Utils.wstrcpy(tdst, tsrc, value.Length);
            //        //tdst += value.Length + escCount;
            //        //position += value.Length + 3 + escCount;
            //    }
            //    else if (sets.Escape == StringEscape.EscapeNonAscii)
            //    {
            //        int ecount = Utils.ToCharAsUnicode(tdst, tsrc, value.Length);
            //        tdst += ecount * 6 + value.Length - ecount;
            //        position += ecount * 6 + value.Length - ecount + 3;
            //    }
            //    *tdst++ = '\r';
            //    *tdst++ = '\n';
            //}
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
            //    *tdst++ = Separator;
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
