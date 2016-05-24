using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JShibo.Serialization.Common;
//using System.Threading.Tasks;

namespace JShibo.Serialization.Soc
{
    public class ObjectBufferSize : ISize
    {
        #region 字段属性构造函数

        internal Serialize<ObjectBufferSize>[] sers;
        int curObj = 0;
        int size = 0;
        /// <summary>
        /// 容量是否是精准预测的
        /// </summary>
        bool isExact = true;

        internal int Size
        {
            get { return size; }
        }

        /// <summary>
        /// 容量是否是精准预测的
        /// </summary>
        internal bool IsExact
        {
            get { return isExact; }
        }

        internal ObjectBufferSize()
        { }

        internal void SetInfo(ObjectBufferContext info)
        {
            sers = info.SizeSerializers;
        }

        #endregion

        #region Write ArraySegment

        public void Write(ArraySegment<bool> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<byte> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<sbyte> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<short> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 1) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<ushort> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 1) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<int> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 2) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<uint> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 2) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<long> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 3) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<ulong> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 3) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<float> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 2) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<double> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 3) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<decimal> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 4) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<char> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 1) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<string> value)
        {
            if (value == null)
                size++;
            else
            {
                size += Utils.GetSize(value.Count);
                foreach (string s in value)
                    Write(s);
            }
        }

        public void Write(ArraySegment<DateTime> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 3) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<DateTimeOffset> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 3) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<TimeSpan> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 3) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<Guid> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 4) + Utils.GetSize(value.Count);
        }

        public void Write(ArraySegment<Uri> value)
        {
            if (value == null)
                size++;
            else
            {
                size += Utils.GetSize(value.Count);
                foreach (Uri s in value)
                    Write(s.AbsoluteUri);
            }
        }

        #endregion

        #region Write Array

        public void Write(bool[] value)
        {
            if (value == null)
                size++;
            else
                size += value.Length + Utils.GetSize(value.Length);
        }

        public void Write(byte[] value)
        {
            if (value == null)
                size++;
            else
                size += value.Length + Utils.GetSize(value.Length);
        }

        public void Write(sbyte[] value)
        {
            if (value == null)
                size++;
            else
                size += value.Length + Utils.GetSize(value.Length);
        }

        public void Write(short[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 1) + Utils.GetSize(value.Length);
        }

        public void Write(ushort[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 1) + Utils.GetSize(value.Length);
        }

        public void Write(int[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 2) + Utils.GetSize(value.Length);
        }

        public void Write(uint[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 2) + Utils.GetSize(value.Length);
        }

        public void Write(long[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 3) + Utils.GetSize(value.Length);
        }

        public void Write(ulong[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 3) + Utils.GetSize(value.Length);
        }

        public void Write(float[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 2) + Utils.GetSize(value.Length);
        }

        public void Write(double[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 3) + Utils.GetSize(value.Length);
        }

        public void Write(decimal[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 4) + Utils.GetSize(value.Length);
        }

        public void Write(char[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 1) + Utils.GetSize(value.Length);
        }

        public void Write(string[] value)
        {
            if (value == null)
                size++;
            else
            {
                size += Utils.GetSize(value.Length);
                foreach (string s in value)
                    Write(s);
            }
        }

        public void Write(DateTime[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 3) + Utils.GetSize(value.Length);
        }

        public void Write(DateTimeOffset[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 3) + Utils.GetSize(value.Length);
        }

        public void Write(TimeSpan[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 3) + Utils.GetSize(value.Length);
        }

        public void Write(Guid[] value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 4) + Utils.GetSize(value.Length);
        }

        public void Write(Uri[] value)
        {
            if (value == null)
                size++;
            else
            {
                size += Utils.GetSize(value.Length);
                foreach (Uri s in value)
                    Write(s.AbsoluteUri);
            }
        }

        public void Write(byte[][] value)
        {
            WriteArray(value, 0);
        }

        #endregion

        #region Write IList

        public void Write(IList<bool> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count + Utils.GetSize(value.Count);
        }

        public void Write(IList<byte> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count + Utils.GetSize(value.Count);
        }

        public void Write(IList<sbyte> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count + Utils.GetSize(value.Count);
        }

        public void Write(IList<short> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 1) + Utils.GetSize(value.Count);
        }

        public void Write(IList<ushort> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 1) + Utils.GetSize(value.Count);
        }

        public void Write(IList<int> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 2) + Utils.GetSize(value.Count);
        }

        public void Write(IList<uint> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 2) + Utils.GetSize(value.Count);
        }

        public void Write(IList<long> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 3) + Utils.GetSize(value.Count);
        }

        public void Write(IList<ulong> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 3) + Utils.GetSize(value.Count);
        }

        public void Write(IList<float> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 2) + Utils.GetSize(value.Count);
        }

        public void Write(IList<double> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 3) + Utils.GetSize(value.Count);
        }

        public void Write(IList<decimal> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 4) + Utils.GetSize(value.Count);
        }

        public void Write(IList<char> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 1) + Utils.GetSize(value.Count);
        }

        public void Write(IList<string> value)
        {
            if (value == null)
                size++;
            else
            {
                size += Utils.GetSize(value.Count);
                foreach (string s in value)
                    Write(s);
            }
        }

        public void Write(IList<DateTime> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 3) + Utils.GetSize(value.Count);
        }

        public void Write(IList<DateTimeOffset> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 3) + Utils.GetSize(value.Count);
        }

        public void Write(IList<TimeSpan> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 3) + Utils.GetSize(value.Count);
        }

        public void Write(IList<Guid> value)
        {
            if (value == null)
                size++;
            else
                size += (value.Count << 37) + Utils.GetSize(value.Count);
        }

        public void Write(IList<Uri> value)
        {
            if (value == null)
                size++;
            else
            {
                size += Utils.GetSize(value.Count);
                foreach (Uri s in value)
                    Write(s.AbsoluteUri);
            }
        }

        public void Write(IList<byte[]> value)
        {
            WriteArray(value, 0);
        }

        #endregion

        #region Write IEnumerable

        public void Write(IEnumerable<bool> value)
        {
            size += value.Count() + 5;
        }

        public void Write(IEnumerable<byte> value)
        {
            size += value.Count() + 5;
        }

        public void Write(IEnumerable<sbyte> value)
        {
            size += value.Count() + 5;
        }

        public void Write(IEnumerable<short> value)
        {
            size += (value.Count() << 1) + 5;
        }

        public void Write(IEnumerable<ushort> value)
        {
            size += (value.Count() << 1) + 5;
        }

        public void Write(IEnumerable<int> value)
        {
            size += (value.Count() << 2) + 5;
        }

        public void Write(IEnumerable<uint> value)
        {
            size += (value.Count() << 2) + 5;
        }

        public void Write(IEnumerable<long> value)
        {
            size += (value.Count() << 3) + 5;
        }

        public void Write(IEnumerable<ulong> value)
        {
            size += (value.Count() << 3) + 5;
        }

        public void Write(IEnumerable<float> value)
        {
            size += (value.Count() << 2) + 5;
        }

        public void Write(IEnumerable<double> value)
        {
            size += (value.Count() << 3) + 5;
        }

        public void Write(IEnumerable<decimal> value)
        {
            size += (value.Count() << 4) + 5;
        }

        public void Write(IEnumerable<char> value)
        {
            size += (value.Count() << 1) + 5;
        }

        public void Write(IEnumerable<string> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<DateTime> value)
        {
            size += (value.Count() << 3) + 5;
        }

        public void Write(IEnumerable<DateTimeOffset> value)
        {
            size += (value.Count() << 3) + 5;
        }

        public void Write(IEnumerable<TimeSpan> value)
        {
            size += (value.Count() << 3) + 5;
        }

        public void Write(IEnumerable<Guid> value)
        {
            size += (value.Count() << 37) + 5;
        }

        public void Write(IEnumerable<Uri> value)
        {
            throw new NotImplementedException();
        }

        public void Write(IEnumerable<byte[]> value)
        {
            WriteArray(value, 0);
        }

        #endregion

        #region Write IDictionary

        public void Write(IDictionary<int, object> value)
        {
            throw new NotSupportedException("not supported object type!");
        }

        public void Write(IDictionary<int, bool> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 5 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, char> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 6 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, sbyte> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 5 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, byte> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 5 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, short> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 6 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, ushort> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 6 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, int> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 8 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, uint> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 8 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, long> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 12 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, ulong> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 12 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, float> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 8 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, double> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 12 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, decimal> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 20 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, DateTime> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 12 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<int, string> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 2 + 5;
                Write(value.Values);
            }
        }



        public void Write(IDictionary<string, object> value)
        {
            throw new NotSupportedException("not supported object type!");
        }

        public void Write(IDictionary<string, bool> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, char> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 1 + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, sbyte> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, byte> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, short> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 1 + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, ushort> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 1 + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, int> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 2 + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, uint> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 2 + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, long> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 3 + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, ulong> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 3 + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, float> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 2 + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, double> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 3 + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, decimal> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 4 + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, DateTime> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 3 + 5;
                Write(value.Keys);
            }
        }

        public void Write(IDictionary<string, string> value)
        {
            if (value == null)
                size++;
            else
            {
                size += 5;
                Write(value.Keys);
                Write(value.Values);
            }
        }



        public void Write(IDictionary<long, object> value)
        {
            throw new NotSupportedException("not supported object type!");
        }

        public void Write(IDictionary<long, bool> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 9 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, char> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 10 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, sbyte> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 9 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, byte> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 9 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, short> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 10 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, ushort> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 10 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, int> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 12 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, uint> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 12 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, long> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 16 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, ulong> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 16 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, float> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 12 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, double> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 16 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, decimal> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 24 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, DateTime> value)
        {
            if (value == null)
                size++;
            else
                size += value.Count * 16 + Utils.GetSize(value.Count);
        }

        public void Write(IDictionary<long, string> value)
        {
            if (value == null)
                size++;
            else
            {
                size += value.Count << 3 + 5;
                Write(value.Values);
            }
        }

        #endregion

        #region 其它

        public void Write(string value)
        {
            if (value == null)
                size++;
            else
                size += (value.Length << 1) + Utils.GetSize(value.Length << 1);
        }

        public void Write(Uri value)
        {
            if (value == null)
                size++;
            else
                size += value.AbsoluteUri.Length + Utils.GetSize(value.AbsoluteUri.Length);
        }

        public void Write(object value)
        {
            if (value == null)
                size++;
            else
            {
                sers[curObj++](this, value);
            }
        }

        private void Write(ICollection<string> keys)
        {
            foreach (string key in keys)
                size += key.Length << 2 + 4;
        }

        private void WriteArray(IEnumerable<byte[]> keys, int shift)
        {
            foreach (byte[] key in keys)
            {
                if (key == null)
                    size += 2;
                else
                    size += key.Length << shift + 4;
            }
        }

        //private void WriteArray<T>(ICollection<T> keys, int shift)
        //{
        //    foreach (T key in keys)
        //    {
        //        if (key == null)
        //            size += 2;
        //        else
        //        {
        //            Type type = typeof(T);
        //            if (type.IsArray)
        //            {
        //                if (type == typeof(byte[]))
        //                {
        //                    size += (key as byte[]).Length << shift + 4;
        //                }
        //            }
        //        }
        //    }
        //}

        #endregion
    }
}
