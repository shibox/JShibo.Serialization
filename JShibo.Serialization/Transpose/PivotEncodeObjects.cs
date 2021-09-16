using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Transpose
{
    public class PivotEncodeObjects
    {
        #region 字段

        internal SerializerSettings sets = SerializerSettings.Default;
        internal ColumnWriter[] writers = null;
        public object[] objs;
        internal int idx = 0;

        #endregion

        #region 构造函数

        public PivotEncodeObjects(Type[] type, int count)
        {
            writers = new ColumnWriter[type.Length];
            for (int i = 0; i < type.Length; i++)
                writers[i] = new ColumnWriter(type[i], type[i].Name, count);
        }

        public PivotEncodeObjects()
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

        public void ReadInt32()
        {
            writers[idx].Write((int)objs[idx]);
            idx++;
        }

        public void ReadUInt32()
        {
            writers[idx].Write((int)objs[idx]);
            idx++;
        }

        public void ReadUInt64()
        {
            writers[idx].Write((ulong)objs[idx]);
            idx++;
        }

        public void ReadInt64()
        {
            writers[idx].Write((long)objs[idx]);
            idx++;
        }

        public void ReadChar()
        {
            writers[idx].Write((char)objs[idx]);
            idx++;
        }

        public void ReadUInt16()
        {
            writers[idx].Write((ushort)objs[idx]);
            idx++;
        }

        public void ReadInt16()
        {
            writers[idx].Write((short)objs[idx]);
            idx++;
        }

        public void ReadString()
        {
            writers[idx].Write((string)objs[idx]);
            idx++;
        }

        public void ReadBoolean()
        {
            writers[idx].Write((bool)objs[idx]);
            idx++;
        }

        public void ReadByte()
        {
            writers[idx].Write((byte)objs[idx]);
            idx++;
        }

        public void ReadSByte()
        {
            writers[idx].Write((sbyte)objs[idx]);
            idx++;
        }

        public void ReadDecimal()
        {
            writers[idx].Write((decimal)objs[idx]);
            idx++;
        }

        public void ReadSingle()
        {
            writers[idx].Write((float)objs[idx]);
            idx++;
        }

        public void ReadDouble()
        {
            writers[idx].Write((double)objs[idx]);
            idx++;
        }

        public void ReadDateTime()
        {
            writers[idx].Write((DateTime)objs[idx]);
            idx++;
        }

        public void ReadDateTimeOffset()
        {
            writers[idx].Write((DateTimeOffset)objs[idx]);
            idx++;
        }

        public void ReadTimeSpan()
        {
            writers[idx].Write((TimeSpan)objs[idx]);
            idx++;
        }

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
            //if (value == null)
            //{
            //    WriteNull();
            //    //跳过中间的对象
            //    current += nameCounts[currSer];
            //    currSer += typeCounts[currSer] + 1;
            //}
            //else
            //{
            //    ResizeAndWriteName(10);

            //    //if (curDepth >= maxDepth)
            //    //    throw new Exception(string.Format(ExceptionConsts.MaxDepth, curDepth));
            //    curDepth++;
            //    //ShiboCsvStringSerializer.LoopSerialize(this, value);
            //}
        }

        #endregion

        #region 公共

        public DataColumn GetResult()
        {

            return null;
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

        

        #endregion
    }
}
