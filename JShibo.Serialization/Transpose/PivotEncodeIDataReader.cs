using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Transpose
{
    public class PivotEncodeIDataReader
    {
        #region 字段

        internal SerializerSettings sets = SerializerSettings.Default;
        internal ColumnWriter[] writers = null;
        internal object[] values;
        internal int idx = 0;

        #endregion

        #region 构造函数

        public PivotEncodeIDataReader(Type[] type, int count)
        {
            writers = new ColumnWriter[type.Length];
            for (int i = 0; i < type.Length; i++)
                writers[i] = new ColumnWriter(type[i], type[i].Name, count);
            values = new object[type.Length];
        }

        public PivotEncodeIDataReader()
        {
            
        }

        #endregion

        #region 方法

        internal void SetInfo(ConvertContext info)
        {

        }

        #endregion

        #region 私有写入

        internal void Reset()
        {
            idx = 0;
        }

        #endregion

        #region Write BaseType

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadInt32()
        {
            writers[idx].Write((int)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadUInt32()
        {
            writers[idx].Write((int)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadUInt64()
        {
            writers[idx].Write((ulong)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadInt64()
        {
            writers[idx].Write((long)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadChar()
        {
            writers[idx].Write((char)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadUInt16()
        {
            writers[idx].Write((ushort)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadInt16()
        {
            writers[idx].Write((short)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadString()
        {
            writers[idx].Write((string)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBoolean()
        {
            writers[idx].Write((bool)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadByte()
        {
            writers[idx].Write((byte)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadSByte()
        {
            writers[idx].Write((sbyte)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadDecimal()
        {
            writers[idx].Write((decimal)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadSingle()
        {
            writers[idx].Write((float)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadDouble()
        {
            writers[idx].Write((double)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadDateTime()
        {
            writers[idx].Write((DateTime)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadDateTimeOffset()
        {
            writers[idx].Write((DateTimeOffset)values[idx]);
            idx++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadTimeSpan()
        {
            writers[idx].Write((TimeSpan)values[idx]);
            idx++;
        }

        #endregion

        #region 公共

        public DataColumn GetResult()
        {

            return null;
        }

        #endregion
    }
}
