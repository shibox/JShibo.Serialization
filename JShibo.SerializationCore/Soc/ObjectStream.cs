using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.Soc
{
    public class ObjectStream :IWriter, IByteBase
    {
        #region 字段

        internal Serialize<ObjectBuffer>[] sers;
        internal Stream _stream = null;
        internal byte[] _buffer = null;
        internal long startPosition = 0;

        #endregion

        #region 构造函数

        public ObjectStream(Stream stream)
        {
            this._stream = stream;
            this._buffer = new byte[1024];
            this.startPosition = stream.Position;
        }

        public ObjectStream(Stream stream,int size)
        {
            this._stream = stream;
            this._buffer = new byte[1024];
            this.startPosition = stream.Position;
        }

        #endregion

        #region 方法

        internal void SetInfo(ObjectStreamContext info)
        {
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
        }

        #endregion

        #region 公共

        public ArraySegment<byte> ToArraySegment()
        {
            throw new NotImplementedException();
        }

        public void WriteTo(Stream stream)
        {
            long length = _stream.Position;
            if (length > 1024 * 4)
            {
                _stream.Position = startPosition;
                byte[] temp = new byte[1024 * 4];
                long totalRead = 0;
                while(totalRead < length)
                {
                    int size = _stream.Read(temp, 0, temp.Length);
                    stream.Write(temp, 0, size);
                    totalRead += size;
                }
                stream.Flush();
            }
            else
            {
                byte[] temp = new byte[length];
                _stream.Position = startPosition;
                _stream.Read(temp, 0, temp.Length);
                stream.Write(temp, 0, temp.Length);
            }
        }

        public byte[] GetBuffer()
        {
            throw new NotImplementedException();
        }

        public byte[] ToArray()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Write BaseType

        public void Write(object value)
        {
            throw new NotImplementedException();
        }

        public void Write(bool value)
        {
            _stream.WriteByte(value == true ? (byte)1 : (byte)0);
        }

        public void Write(char value)
        {
            throw new NotImplementedException();
        }

        public void Write(sbyte value)
        {
            _stream.WriteByte((byte)value);
        }

        public void Write(byte value)
        {
            _stream.WriteByte(value);
        }

        public void Write(short value)
        {
            _stream.WriteByte((byte)value);
            _stream.WriteByte((byte)(value >> 8));
        }

        public void Write(ushort value)
        {
            _stream.WriteByte((byte)value);
            _stream.WriteByte((byte)(value >> 8));
        }

        public unsafe void Write(int value)
        {
            //_stream.WriteByte((byte)value);
            //_stream.WriteByte((byte)(value >> 8));
            //_stream.WriteByte((byte)(value >> 0x10));
            //_stream.WriteByte((byte)(value >> 0x18));

            fixed (byte* pd = &_buffer[0])
                *((int*)pd) = *((int*)&value);
            _stream.Write(_buffer, 0, 4);
        }

        public void Write(uint value)
        {
            throw new NotImplementedException();
        }

        public void Write(long value)
        {
            throw new NotImplementedException();
        }

        public void Write(ulong value)
        {
            throw new NotImplementedException();
        }

        public void Write(float value)
        {
            throw new NotImplementedException();
        }

        public void Write(double value)
        {
            throw new NotImplementedException();
        }

        public void Write(decimal value)
        {
            throw new NotImplementedException();
        }

        public void Write(DateTime value)
        {
            throw new NotImplementedException();
        }

        public void Write(string value)
        {
            throw new NotImplementedException();
        }

        public void Write(DateTimeOffset value)
        {
            throw new NotImplementedException();
        }

        public void Write(TimeSpan value)
        {
            throw new NotImplementedException();
        }

        public void Write(Guid value)
        {
            throw new NotImplementedException();
        }

        public void Write(System.IO.Stream value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Write IList

        public void Write(object[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<object> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<object> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<object> value)
        {
            throw new NotImplementedException();
        }

        public void Write(bool[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<bool> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<bool> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<bool> value)
        {
            throw new NotImplementedException();
        }

        public void Write(char[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<char> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<char> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<char> value)
        {
            throw new NotImplementedException();
        }

        public void Write(sbyte[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<sbyte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<sbyte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<sbyte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<byte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<byte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<byte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(short[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<short> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<short> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<short> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ushort[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<ushort> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<ushort> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<ushort> value)
        {
            throw new NotImplementedException();
        }

        public void Write(int[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<int> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<int> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<int> value)
        {
            throw new NotImplementedException();
        }

        public void Write(uint[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<uint> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<uint> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<uint> value)
        {
            throw new NotImplementedException();
        }

        public void Write(long[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<long> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<long> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<long> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ulong[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<ulong> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<ulong> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<ulong> value)
        {
            throw new NotImplementedException();
        }

        public void Write(float[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<float> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<float> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<float> value)
        {
            throw new NotImplementedException();
        }

        public void Write(double[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<double> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<double> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<double> value)
        {
            throw new NotImplementedException();
        }

        public void Write(decimal[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<decimal> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<decimal> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<decimal> value)
        {
            throw new NotImplementedException();
        }

        public void Write(DateTime[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<DateTime> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<DateTime> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<DateTime> value)
        {
            throw new NotImplementedException();
        }

        public void Write(string[] value)
        {
            throw new NotImplementedException();
        }

        public void Write(List<string> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IList<string> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<string> value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Write IDictionary

        public void Write(IDictionary<int, object> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, bool> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, char> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, sbyte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, byte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, short> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, ushort> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, int> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, uint> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, long> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, ulong> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, float> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, double> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, decimal> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, DateTime> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<int, string> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, object> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, bool> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, char> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, sbyte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, byte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, short> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, ushort> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, int> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, uint> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, long> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, ulong> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, float> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, double> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, decimal> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, DateTime> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<string, string> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, object> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, bool> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, char> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, sbyte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, byte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, short> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, ushort> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, int> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, uint> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, long> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, ulong> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, float> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, double> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, decimal> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, DateTime> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary<long, string> value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Write Other

        public void Write(IList value)
        {
            throw new NotImplementedException();
        }

        public void Write(IDictionary value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable value)
        {
            throw new NotImplementedException();
        }

        #endregion


        public void Write(ArraySegment<bool> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<char> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<sbyte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<byte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<short> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<ushort> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<int> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<uint> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<long> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<ulong> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<float> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<double> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<decimal> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<DateTime> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<string> value)
        {
            throw new NotImplementedException();
        }
    }
}
