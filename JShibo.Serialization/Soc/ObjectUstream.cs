using JShibo.Serialization.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.Soc
{
    public class ObjectUstream : OBase,IReader
    {
        #region 字段

        internal Deserialize<ObjectUstream>[] desers;
        internal Stream _stream = null;
        internal byte[] _buffer = null;
        internal long startPosition = 0;

        #endregion

        #region 构造函数

        public ObjectUstream(Stream stream)
        {
            _buffer = new byte[1024] ;
        }

        #endregion

        #region 方法

        private int ReadSize()
        {
            return Utils.ReadSize(_buffer, ref position);
        }

        #endregion

        public object ReadObject()
        {
            throw new NotImplementedException();
        }

        public bool ReadBoolean()
        {
            throw new NotImplementedException();
        }

        public char ReadChar()
        {
            throw new NotImplementedException();
        }

        public sbyte ReadSByte()
        {
            throw new NotImplementedException();
        }

        public byte ReadByte()
        {
            throw new NotImplementedException();
        }

        public short ReadInt16()
        {
            throw new NotImplementedException();
        }

        public ushort ReadUInt16()
        {
            throw new NotImplementedException();
        }

        public int ReadInt32()
        {
            throw new NotImplementedException();
        }

        public uint ReadUInt32()
        {
            throw new NotImplementedException();
        }

        public long ReadInt64()
        {
            throw new NotImplementedException();
        }

        public ulong ReadUInt64()
        {
            throw new NotImplementedException();
        }

        public float ReadSingle()
        {
            throw new NotImplementedException();
        }

        public double ReadDouble()
        {
            throw new NotImplementedException();
        }

        public decimal ReadDecimal()
        {
            throw new NotImplementedException();
        }

        public DateTime ReadDateTime()
        {
            throw new NotImplementedException();
        }

        public string ReadString()
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset ReadDateTimeOffset()
        {
            throw new NotImplementedException();
        }

        public TimeSpan ReadTimeSpan()
        {
            throw new NotImplementedException();
        }

        public Guid ReadGuid()
        {
            throw new NotImplementedException();
        }

        public Stream ReadStream()
        {
            throw new NotImplementedException();
        }

        public System.Collections.IList ReadIList()
        {
            throw new NotImplementedException();
        }

        public System.Collections.IDictionary ReadIDictionary()
        {
            throw new NotImplementedException();
        }

        public System.Collections.IEnumerable ReadIEnumerable()
        {
            throw new NotImplementedException();
        }
    }
}
