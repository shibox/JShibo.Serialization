using JShibo.Serialization.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.Json
{
    public class JsonUstring
    {
        #region 字段

        internal Type[] types;
        internal Deserialize<JsonUstring>[] desers;
        internal int[] typeCounts;
        internal int[] nameCounts;
        internal CheckAttribute[] checkAttributes;

        internal string[] names = new string[0];
        internal string _buffer = null;
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
        internal int currentLast = 0;

        #endregion

        #region 构造函数

        public JsonUstring(string json)
        {
            this._buffer = json;
        }

        #endregion

        #region 方法

        private bool CheckAndSkipName()
        {
            if (names.Length > 0)
            {
                string name = names[current];
                position++;
                if (Utils.LowerChars[name[0]] !=Utils.LowerChars[_buffer[position]])
                    return false;
                for (int i = 1; i < name.Length; i++)
                {
                    if (name[i] != _buffer[position + i])
                        return false;
                }
                position += name.Length;
                //跳过"和:，下面直接是数据区
                position += 2;
                current++;
            }
            return true;
        }

        private bool CheckAndSkipNameNonNullArray<T>(ref T[] values)
        {
            if (names.Length > 0)
            {
                string name = names[current];
                position++;
                if (Utils.LowerChars[name[0]] != Utils.LowerChars[_buffer[position]])
                    return false;
                for (int i = 1; i < name.Length; i++)
                {
                    if (name[i] != _buffer[position + i])
                        return false;
                }
                position += name.Length;
                //跳过"和:，下面直接是数据区
                position += 2;
                current++;
                if (_buffer[position] == 'n')
                {
                    position += 5;
                    values = null;
                    return false;
                }
                else if (_buffer[position] == '[' && _buffer[position + 1] == ']')
                {
                    position += 3;
                    values = new T[] { };
                    return false;
                }
                else
                    values = new T[4];
            }
            return true;
        }

        private bool CheckAndSkipNameNonNullList<T>(ref List<T> values)
        {
            if (names.Length > 0)
            {
                string name = names[current];
                position++;
                if (Utils.LowerChars[name[0]] != Utils.LowerChars[_buffer[position]])
                    return false;
                for (int i = 1; i < name.Length; i++)
                {
                    if (name[i] != _buffer[position + i])
                        return false;
                }
                position += name.Length;
                //跳过"和:，下面直接是数据区
                position += 2;
                current++;
                if (_buffer[position] == 'n')
                {
                    position += 5;
                    values = null;
                    return false;
                }
                else if (_buffer[position] == '[' && _buffer[position + 1] == ']')
                {
                    position += 3;
                    values = new List<T>();
                    return false;
                }
                else
                    values = new List<T>();
            }
            return true;
        }

        private unsafe bool CheckAndSkipName(char* pd)
        {
            if (names.Length > 0)
            {
                string name = names[current];
                position++;
                for (int i = 0; i < name.Length; i++)
                {
                    if (name[i] != *pd++)
                        return false;
                }
                position += name.Length;
                position += 2;
                current++;
            }
            return true;
        }

        private bool IsNull()
        {
            return false;
        }

        #endregion

        #region Read BaseType

        internal int ReadInt32()
        {
            int value = 0;
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = FastToValue.ToInt32(_buffer, ref position, ',');
                else
                {
                    value = FastToValue.ToInt32(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal uint ReadUInt32()
        {
            uint value = 0;
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = FastToValue.ToUInt32(_buffer, ref position, ',');
                else
                {
                    value = FastToValue.ToUInt32(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal ulong ReadUInt64()
        {
            ulong value = 0;
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = FastToValue.ToUInt64(_buffer, ref position, ',');
                else
                {
                    value = FastToValue.ToUInt64(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal long ReadInt64()
        {
            long value = 0;
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = FastToValue.ToInt64(_buffer, ref position, ',');
                else
                {
                    value = FastToValue.ToInt64(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal char ReadChar()
        {
            if (CheckAndSkipName())
            {
                position++;
                char value = _buffer[position];
                position += 2;
                return value;
            }
            else
                throw new Exception();
        }

        internal ushort ReadUInt16()
        {
            ushort value = 0;
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = (ushort)FastToValue.ToUInt32(_buffer, ref position, ',');
                else
                {
                    value = (ushort)FastToValue.ToUInt32(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal short ReadInt16()
        {
            short value = 0;
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = (short)FastToValue.ToInt32(_buffer, ref position, ',');
                else
                {
                    value = (short)FastToValue.ToInt32(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal string ReadString()
        {
            if (CheckAndSkipName())
            {
                if (IsNull())
                    return null;
                else
                {
                    position++;
                    if (checkAttributes[current - 1].Check == CheckEscape.None)
                    {
                        //position++;
                        int pos = _buffer.IndexOf('"', position);
                        if (pos != -1)
                        {
                            string value = _buffer.Substring(position, pos - position);
                            position = pos + 2;
                            return value;
                        }
                    }
                    else
                    {
                        int pos = _buffer.IndexOf('"', position);
                        if (pos != -1)
                        {
                            string value = _buffer.Substring(position, pos - position);
                            position = pos + 1;
                            return value;
                        }
                    }
                }
            }
            return string.Empty;
        }

        internal bool ReadBoolean()
        {
            if (CheckAndSkipName())
            {
                if (
                    (this._buffer[position] == 't' || this._buffer[position] == 'T') &&
                    (this._buffer[position + 1] == 'r' || this._buffer[position + 1] == 'R') &&
                    (this._buffer[position + 2] == 'u' || this._buffer[position + 2] == 'U') &&
                    (this._buffer[position + 3] == 'e' || this._buffer[position + 3] == 'E')
                   )
                {
                    position += 5;
                    return true;
                }
                else if
                    (
                    (this._buffer[position] == 'f' || this._buffer[position] == 'F') &&
                    (this._buffer[position] == 'a' || this._buffer[position] == 'A') &&
                    (this._buffer[position] == 'l' || this._buffer[position] == 'L') &&
                    (this._buffer[position] == 's' || this._buffer[position] == 'S') &&
                    (this._buffer[position] == 'e' || this._buffer[position] == 'E')
                    )
                {
                    position += 6;
                    return false;
                }
                else
                {
                    throw new Exception("不正确的二进制数据类型！");
                }
            }
            else
                return false;
        }

        internal byte ReadByte()
        {
            byte value = 0;
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = (byte)FastToValue.ToInt32(_buffer, ref position, ',');
                else
                {
                    value = (byte)FastToValue.ToInt32(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal sbyte ReadSByte()
        {
            sbyte value = 0;
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = (sbyte)FastToValue.ToUInt32(_buffer, ref position, ',');
                else
                {
                    value = (sbyte)FastToValue.ToUInt32(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal decimal ReadDecimal()
        {
            decimal value = 0;
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = FastToValue.ToDecimal(_buffer, ref position, ',');
                else
                {
                    value = FastToValue.ToDecimal(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal unsafe float ReadSingle()
        {
            float value = 0;
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = FastToValue.ToFloat(_buffer, ref position, ',');
                else
                {
                    value = FastToValue.ToFloat(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal unsafe double ReadDouble()
        {
            double value = 0;
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = FastToValue.ToDouble(_buffer, ref position, ',');
                else
                {
                    value = FastToValue.ToDouble(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal DateTime ReadDateTime()
        {
            DateTime value = default(DateTime);
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = FastToValue.ToDateTime(_buffer, ref position, ',');
                else
                {
                    value = FastToValue.ToDateTime(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal DateTimeOffset ReadDateTimeOffset()
        {
            DateTimeOffset value = default(DateTimeOffset);
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = FastToValue.ToDateTimeOffset(_buffer, ref position, ',');
                else
                {
                    value = FastToValue.ToDateTimeOffset(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal TimeSpan ReadTimeSpan()
        {
            TimeSpan value = default(TimeSpan);
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = FastToValue.ToTimeSpan(_buffer, ref position, ',');
                else
                {
                    value = FastToValue.ToTimeSpan(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal Guid ReadGuid()
        {
            Guid value = default(Guid);
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = FastToValue.ToGuid(_buffer, ref position, ',');
                else
                {
                    value = FastToValue.ToGuid(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal Uri ReadUri()
        {
            Uri value = null;
            if (CheckAndSkipName())
            {
                if (current != currentLast)
                    value = FastToValue.ToUri(_buffer, ref position, ',');
                else
                {
                    value = FastToValue.ToUri(_buffer, ref position, '}');
                    position++;
                }
                position++;
            }
            return value;
        }

        internal object ReadObject()
        {
            object value = 0;
            if (CheckAndSkipName())
            {
                currentLast = current + nameCounts[currSer];
                position++;
                value = ShiboJsonStringSerializer.Deserialize(this, desers[currSer++]);
                currentLast = names.Length;
                return value;
            }
            return null;
        }

        #endregion

        #region Read List

        internal bool[] ReadBools()
        {
            throw new NotImplementedException();
        }

        internal List<bool> ReadBoolList()
        {
            throw new NotImplementedException();
        }

        internal ArraySegment<bool> ReadBoolArraySegment()
        {
            throw new NotImplementedException();
        }


        internal byte[] ReadBytes()
        {
            ArraySegment<uint> array = ReadUIntArraySegment();
            return Utils.ToArrayByte(array.Array, 0, array.Count);
        }

        internal List<byte> ReadByteList()
        {
            ArraySegment<uint> array = ReadUIntArraySegment();
            return Utils.ToListByte(array.Array, 0, array.Count);
        }

        internal ArraySegment<byte> ReadByteArraySegment()
        {
            ArraySegment<uint> array = ReadUIntArraySegment();
            return new ArraySegment<byte>(Utils.ToArrayByte(array.Array, 0, array.Count));
        }


        internal sbyte[] ReadSBytes()
        {
            ArraySegment<int> array = ReadIntArraySegment();
            return Utils.ToArraySByte(array.Array, 0, array.Count);
        }

        internal List<sbyte> ReadSByteList()
        {
            ArraySegment<int> array = ReadIntArraySegment();
            return Utils.ToListSByte(array.Array, 0, array.Count);
        }

        internal ArraySegment<sbyte> ReadSByteArraySegment()
        {
            ArraySegment<int> array = ReadIntArraySegment();
            return new ArraySegment<sbyte>(Utils.ToArraySByte(array.Array, 0, array.Count));
        }


        internal short[] ReadShorts()
        {
            ArraySegment<int> array = ReadIntArraySegment();
            return Utils.ToArrayShort(array.Array, 0, array.Count);
        }

        internal List<short> ReadShortList()
        {
            ArraySegment<int> array = ReadIntArraySegment();
            return Utils.ToListShort(array.Array, 0, array.Count);
        }

        internal ArraySegment<short> ReadShortArraySegment()
        {
            ArraySegment<int> array = ReadIntArraySegment();
            return new ArraySegment<short>(Utils.ToArrayShort(array.Array, 0, array.Count));
        }


        internal ushort[] ReadUShorts()
        {
            ArraySegment<uint> array = ReadUIntArraySegment();
            return Utils.ToArrayUShort(array.Array, 0, array.Count);
        }

        internal List<ushort> ReadUShortList()
        {
            ArraySegment<uint> array = ReadUIntArraySegment();
            return Utils.ToListUShort(array.Array, 0, array.Count);
        }

        internal ArraySegment<ushort> ReadUShortArraySegment()
        {
            ArraySegment<uint> array = ReadUIntArraySegment();
            return new ArraySegment<ushort>(Utils.ToArrayUShort(array.Array, 0, array.Count));
        }



        internal unsafe int[] ReadInts()
        {
            int[] values = null;
            if (CheckAndSkipNameNonNullArray(ref values))
            {
                int used = 0;
                fixed (char* pd = _buffer)
                {
                    char* p = pd;
                    p += ++position;
                    while (true)
                    {
                        if (used == values.Length)
                            values = Utils.Resize(values, 2);
                        int value = 0;
                        if (*p == '-')
                        {
                            value = -(*(p + 1) - 48);
                            p += 2;
                            position += 2;
                            for (; *p >= '0' && *p <= '9'; position++)
                                value = value * 10 - ((*p++) - 48);
                        }
                        else
                        {
                            for (; *p >= '0' && *p <= '9'; position++)
                                value = value * 10 + ((*p++) - 48);
                        }
                        values[used++] = value;
                        if (*p == ']')
                            break;
                        p++;
                        position++;
                    }
                    position += 2;
                }
                return Utils.ToArray(values, 0, used, 2);
            }
            return values;

            #region old
            //int[] values = new int[4];
            //int used = 0;
            //if (CheckAndSkipName())
            //{
            //    fixed (char* pd = _buffer)
            //    {
            //        char* p = pd;
            //        p += position;
            //        if (*p == 'n')
            //        {
            //            position += 5;
            //            return null;
            //        }
            //        else
            //        {
            //            p++;
            //            position++;
            //            while (true)
            //            {
            //                if (used == values.Length)
            //                    values = Utils.Resize(values, 2);
            //                int value = 0;
            //                if (*p == '-')
            //                {
            //                    value = -(*(p + 1) - 48);
            //                    p += 2;
            //                    position += 2;
            //                    for (; *p >= '0' && *p <= '9'; position++)
            //                        value = value * 10 - ((*p++) - 48);
            //                }
            //                else
            //                {
            //                    for (; *p >= '0' && *p <= '9'; position++)
            //                        value = value * 10 + ((*p++) - 48);
            //                }
            //                values[used++] = value;
            //                if (*p == ']')
            //                    break;
            //                p++;
            //                position++;
            //            }
            //            position += 2;
            //        }
            //    }

            //}
            //return Utils.ToArray(values, 0, used, 2);
            #endregion
        }

        internal unsafe List<int> ReadIntList()
        {
            List<int> values = null;
            if (CheckAndSkipNameNonNullList(ref values))
            {
                fixed (char* pd = _buffer)
                {
                    char* p = pd;
                    p += ++position;
                    while (true)
                    {
                        int value = 0;
                        if (*p == '-')
                        {
                            value = -(*(p + 1) - 48);
                            p += 2;
                            position += 2;
                            for (; *p >= '0' && *p <= '9'; position++)
                                value = value * 10 - ((*p++) - 48);
                        }
                        else
                        {
                            for (; *p >= '0' && *p <= '9'; position++)
                                value = value * 10 + ((*p++) - 48);
                        }
                        values.Add(value);
                        if (*p == ']')
                            break;
                        p++;
                        position++;
                    }
                    position += 2;
                }
                return values;
            }
            return values;
        }

        internal unsafe ArraySegment<int> ReadIntArraySegment()
        {
            int[] values = null;
            if (CheckAndSkipNameNonNullArray(ref values))
            {
                int used = 0;
                fixed (char* pd = _buffer)
                {
                    char* p = pd;
                    p += ++position;
                    while (true)
                    {
                        if (used == values.Length)
                            values = Utils.Resize(values, 2);
                        int value = 0;
                        if (*p == '-')
                        {
                            value = -(*(p + 1) - 48);
                            p += 2;
                            position += 2;
                            for (; *p >= '0' && *p <= '9'; position++)
                                value = value * 10 - ((*p++) - 48);
                        }
                        else
                        {
                            for (; *p >= '0' && *p <= '9'; position++)
                                value = value * 10 + ((*p++) - 48);
                        }
                        values[used++] = value;
                        if (*p == ']')
                            break;
                        p++;
                        position++;
                    }
                    position += 2;
                }
                return new ArraySegment<int>(values, 0, used);
            }
            return new ArraySegment<int>();
        }


        internal unsafe uint[] ReadUInts()
        {
            uint[] values = null;
            if (CheckAndSkipNameNonNullArray(ref values))
            {
                int used = 0;
                fixed (char* pd = _buffer)
                {
                    char* p = pd;
                    p += ++position;
                    while (true)
                    {
                        if (used == values.Length)
                            values = Utils.Resize(values, 2);
                        uint value = 0;
                        for (; *p >= '0' && *p <= '9'; position++)
                            value = value * 10 + (uint)((*p++) - 48);
                        values[used++] = value;
                        if (*p == ']')
                            break;
                        p++;
                        position++;
                    }
                    position += 2;
                }
                return Utils.ToArray(values, 0, used, 2);
            }
            return values;
        }

        internal unsafe List<uint> ReadUIntList()
        {
            List<uint> values = null;
            if (CheckAndSkipNameNonNullList(ref values))
            {
                fixed (char* pd = _buffer)
                {
                    char* p = pd;
                    p += ++position;
                    while (true)
                    {
                        uint value = 0;
                        for (; *p >= '0' && *p <= '9'; position++)
                            value = value * 10 + (uint)((*p++) - 48);
                        values.Add(value);
                        if (*p == ']')
                            break;
                        p++;
                        position++;
                    }
                    position += 2;
                }
                return values;
            }
            return values;
        }

        internal unsafe ArraySegment<uint> ReadUIntArraySegment()
        {
            uint[] values = null;
            if (CheckAndSkipNameNonNullArray(ref values))
            {
                int used = 0;
                fixed (char* pd = _buffer)
                {
                    char* p = pd;
                    p += ++position;
                    while (true)
                    {
                        if (used == values.Length)
                            values = Utils.Resize(values, 2);
                        uint value = 0;
                        for (; *p >= '0' && *p <= '9'; position++)
                            value = value * 10 + (uint)((*p++) - 48);
                        values[used++] = value;
                        if (*p == ']')
                            break;
                        p++;
                        position++;
                    }
                    position += 2;
                }
                return new ArraySegment<uint>(values, 0, used);
            }
            return new ArraySegment<uint>();
        }


        internal unsafe long[] ReadLongs()
        {
            long[] values = null;
            if (CheckAndSkipNameNonNullArray(ref values))
            {
                int used = 0;
                fixed (char* pd = _buffer)
                {
                    char* p = pd;
                    p += ++position;
                    while (true)
                    {
                        if (used == values.Length)
                            values = Utils.Resize(values, 2);
                        long value = 0;
                        if (*p == '-')
                        {
                            value = -(*(p + 1) - 48);
                            p += 2;
                            position += 2;
                            for (; *p >= '0' && *p <= '9'; position++)
                                value = value * 10 - ((*p++) - 48);
                        }
                        else
                        {
                            for (; *p >= '0' && *p <= '9'; position++)
                                value = value * 10 + ((*p++) - 48);
                        }
                        values[used++] = value;
                        if (*p == ']')
                            break;
                        p++;
                        position++;
                    }
                    position += 2;
                }
                return Utils.ToArray(values, 0, used, 2);
            }
            return values;

            #region old
            //int[] values = new int[4];
            //int used = 0;
            //if (CheckAndSkipName())
            //{
            //    fixed (char* pd = _buffer)
            //    {
            //        char* p = pd;
            //        p += position;
            //        if (*p == 'n')
            //        {
            //            position += 5;
            //            return null;
            //        }
            //        else
            //        {
            //            p++;
            //            position++;
            //            while (true)
            //            {
            //                if (used == values.Length)
            //                    values = Utils.Resize(values, 2);
            //                int value = 0;
            //                if (*p == '-')
            //                {
            //                    value = -(*(p + 1) - 48);
            //                    p += 2;
            //                    position += 2;
            //                    for (; *p >= '0' && *p <= '9'; position++)
            //                        value = value * 10 - ((*p++) - 48);
            //                }
            //                else
            //                {
            //                    for (; *p >= '0' && *p <= '9'; position++)
            //                        value = value * 10 + ((*p++) - 48);
            //                }
            //                values[used++] = value;
            //                if (*p == ']')
            //                    break;
            //                p++;
            //                position++;
            //            }
            //            position += 2;
            //        }
            //    }

            //}
            //return Utils.ToArray(values, 0, used, 2);
            #endregion
        }

        internal unsafe List<long> ReadLongList()
        {
            List<long> values = null;
            if (CheckAndSkipNameNonNullList(ref values))
            {
                fixed (char* pd = _buffer)
                {
                    char* p = pd;
                    p += ++position;
                    while (true)
                    {
                        long value = 0;
                        if (*p == '-')
                        {
                            value = -(*(p + 1) - 48);
                            p += 2;
                            position += 2;
                            for (; *p >= '0' && *p <= '9'; position++)
                                value = value * 10 - ((*p++) - 48);
                        }
                        else
                        {
                            for (; *p >= '0' && *p <= '9'; position++)
                                value = value * 10 + ((*p++) - 48);
                        }
                        values.Add(value);
                        if (*p == ']')
                            break;
                        p++;
                        position++;
                    }
                    position += 2;
                }
                return values;
            }
            return values;
        }

        internal unsafe ArraySegment<long> ReadLongArraySegment()
        {
            long[] values = null;
            if (CheckAndSkipNameNonNullArray(ref values))
            {
                int used = 0;
                fixed (char* pd = _buffer)
                {
                    char* p = pd;
                    p += ++position;
                    while (true)
                    {
                        if (used == values.Length)
                            values = Utils.Resize(values, 2);
                        long value = 0;
                        if (*p == '-')
                        {
                            value = -(*(p + 1) - 48);
                            p += 2;
                            position += 2;
                            for (; *p >= '0' && *p <= '9'; position++)
                                value = value * 10 - ((*p++) - 48);
                        }
                        else
                        {
                            for (; *p >= '0' && *p <= '9'; position++)
                                value = value * 10 + ((*p++) - 48);
                        }
                        values[used++] = value;
                        if (*p == ']')
                            break;
                        p++;
                        position++;
                    }
                    position += 2;
                }
                return new ArraySegment<long>(values, 0, used);
            }
            return new ArraySegment<long>();
        }


        internal unsafe ulong[] ReadULongs()
        {
            ulong[] values = null;
            if (CheckAndSkipNameNonNullArray(ref values))
            {
                int used = 0;
                fixed (char* pd = _buffer)
                {
                    char* p = pd;
                    p += ++position;
                    while (true)
                    {
                        if (used == values.Length)
                            values = Utils.Resize(values, 2);
                        ulong value = 0;
                        for (; *p >= '0' && *p <= '9'; position++)
                            value = value * 10 + (uint)((*p++) - 48);
                        values[used++] = value;
                        if (*p == ']')
                            break;
                        p++;
                        position++;
                    }
                    position += 2;
                }
                return Utils.ToArray(values, 0, used, 2);
            }
            return values;
        }

        internal unsafe List<ulong> ReadULongList()
        {
            List<ulong> values = null;
            if (CheckAndSkipNameNonNullList(ref values))
            {
                fixed (char* pd = _buffer)
                {
                    char* p = pd;
                    p += ++position;
                    while (true)
                    {
                        ulong value = 0;
                        for (; *p >= '0' && *p <= '9'; position++)
                            value = value * 10 + (uint)((*p++) - 48);
                        values.Add(value);
                        if (*p == ']')
                            break;
                        p++;
                        position++;
                    }
                    position += 2;
                }
                return values;
            }
            return values;
        }

        internal unsafe ArraySegment<ulong> ReadULongArraySegment()
        {
            ulong[] values = null;
            if (CheckAndSkipNameNonNullArray(ref values))
            {
                int used = 0;
                fixed (char* pd = _buffer)
                {
                    char* p = pd;
                    p += ++position;
                    while (true)
                    {
                        if (used == values.Length)
                            values = Utils.Resize(values, 2);
                        ulong value = 0;
                        for (; *p >= '0' && *p <= '9'; position++)
                            value = value * 10 + (uint)((*p++) - 48);
                        values[used++] = value;
                        if (*p == ']')
                            break;
                        p++;
                        position++;
                    }
                    position += 2;
                }
                return new ArraySegment<ulong>(values, 0, used);
            }
            return new ArraySegment<ulong>();
        }


        internal List<float> ReadFloats()
        {
            throw new NotImplementedException();
        }


        internal List<double> ReadDoubles()
        {
            throw new NotImplementedException();
        }


        internal List<decimal> ReadDecimals()
        {
            throw new NotImplementedException();
        }


        internal List<char> ReadChars()
        {
            throw new NotImplementedException();
        }


        internal string[] ReadStrings()
        {
            List<string> result = ReadStringList();
            if (result == null)
                return null;
            else
                return result.ToArray();
        }

        internal unsafe List<string> ReadStringList()
        {
            List<string> values = null;
            if (CheckAndSkipNameNonNullList(ref values))
            {
                position++;
                if (checkAttributes[current - 1].Check == CheckEscape.None)
                {
                    #region old

                    //int pos = 0;
                    //fixed (char* pd = _buffer)
                    //{
                    //    char* p = pd;
                    //    p += position;
                    //    while (true)
                    //    {
                    //        pos = position;
                    //        for (; *p != '"'; position++)
                    //            p++;
                    //        if (*p == '"')
                    //        {
                    //            string value = _buffer.Substring(pos, position - pos);
                    //            position++;
                    //            p++;
                    //            values.Add(value);
                    //            if (*p == ',')
                    //            {
                    //                position += 2;
                    //                p += 2;
                    //            }
                    //            if (*p == ']')
                    //                break;
                    //        }
                    //        else
                    //            break;
                    //    }
                    //}

                    #endregion
                    
                    while (true)
                    {
                        if (_buffer[position] == 'n')
                        {
                            position += 4;
                            if (_buffer[position] == ',')
                                position++;
                            else if (_buffer[position] == ']')
                                break;
                        }
                        else
                        {
                            position++;
                            int pos = _buffer.IndexOf('"', position);
                            if (pos != -1)
                            {
                                string value = _buffer.Substring(position, pos - position);
                                position = pos + 1;
                                values.Add(value);
                                if (_buffer[position] == ',')
                                    position++;
                                else if (_buffer[position] == ']')
                                    break;
                            }
                            else
                                break;
                        }
                    }
                }
                else
                {
                    while (true)
                    {
                        int pos = _buffer.IndexOf('"', position);
                        if (pos != -1)
                        {
                            string value = _buffer.Substring(position, pos - position);
                            position = pos + 1;
                            values.Add(value);
                        }
                        else
                            break;
                    }
                }
                position += 2;
            }
            return values;
        }

        internal ArraySegment<string> ReadStringArraySegment()
        {
            List<string> result = ReadStringList();
            if (result == null)
                return new ArraySegment<string>();
            else
                return new ArraySegment<string>( result.ToArray());
        }


        internal List<DateTime> ReadDateTimes()
        {
            throw new NotImplementedException();
        }

        internal List<DateTimeOffset> ReadDateTimeOffsets()
        {
            throw new NotImplementedException();
        }

        internal List<TimeSpan> ReadTimeSpans()
        {
            throw new NotImplementedException();
        }

        internal List<Guid> ReadGuids()
        {
            throw new NotImplementedException();
        }

        internal List<Uri> ReadUris()
        {
            throw new NotImplementedException();
        }

        internal object[] ReadObjects()
        {
            return null;
        }

        #endregion

        #region Read Dictionary



        #endregion

    }
}
