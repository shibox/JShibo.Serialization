using JShibo.Serialization.Common;
using JShibo.Serialization.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Transpose
{
    public class PivotEncode
    {
        #region 字段

        internal SerializerSettings sets = SerializerSettings.Default;
        internal ColumnWriter[] writers = null;
        internal int idx = 0;

        #endregion
        
        #region 构造函数

        public PivotEncode(Type type,int count)
        {
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            writers = new ColumnWriter[properties.Length];
            for (int i = 0; i < properties.Length; i++)
                writers[i] = new ColumnWriter(properties[i].PropertyType,properties[i].Name, count);

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            writers = new ColumnWriter[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                writers[i] = new ColumnWriter(fields[i].FieldType, fields[i].Name, count);
        }

        public PivotEncode(Type[] type, int count)
        {
            writers = new ColumnWriter[type.Length];
            for (int i = 0; i < type.Length; i++)
                writers[i] = new ColumnWriter(type[i], type[i].Name, count);
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

        internal void Write(byte value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(sbyte value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(short value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(ushort value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(int value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(uint value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(long value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(ulong value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(float value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(double value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(decimal value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(string value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(bool value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(char value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal unsafe void Write(DateTime value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal unsafe void Write(DateTimeOffset value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal unsafe void Write(TimeSpan value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal unsafe void Write(Guid value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal unsafe void Write(Uri value)
        {
            writers[idx].Write(value);
            idx++;
        }

        internal void Write(object value)
        {
            writers[idx].Write(value);
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

        public ColumnsResult GetResult()
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

        //public char[] GetBuffer()
        //{
        //    return _buffer;
        //}

        //public char[] ToArray()
        //{
        //    char[] temp = new char[position];
        //    Buffer.BlockCopy(_buffer, 0, temp, 0, position << 1);
        //    return temp;
        //}

        #endregion

        

        
    }
}
