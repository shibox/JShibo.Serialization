using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Json
{
    /// <summary>
    /// 用于计算字符串需要的长度信息
    /// </summary>
    public class JsonStringSize : ISize
    {
        #region 字段

        int size = 0;

        internal Type[] types;
        internal Serialize<JsonStringSize>[] sers;
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

        internal int Size
        {
            get { return size; }
        }

        #endregion

        #region 构造函数

        internal void SetInfo(JsonStringContext info)
        {
            sers = info.SizeSerializers;

            //if (info.Names.Length > 0)
            //{
            //    if (sets.CamelCase)
            //        names = info.GetNamesCamelCase();
            //    else
            //        names = info.Names;
            //    if (sets.UseSingleQuotes)
            //        Quote = '\'';

            //    sers = info.Serializes;
            //    types = info.Types;
            //    typeCounts = info.TypeCounts;
            //    nameCounts = info.NameCounts;
            //}
            //else
            //    isJsonBaseType = true;
        }

        #endregion

        #region 方法

        public void Write(string value)
        {
            if (value != null)
                size += value.Length + 3;
            else
                size += 7;
        }

        public void Write(string[] value)
        {
            if (value != null)
                size += value.Length + 3;
            else
                size += 7;
        }

        public void Write(Uri value)
        {
            if (value != null)
                size += value.AbsoluteUri.Length + 3;
            else
                size += 7;
        }

        public void Write(IList value)
        {

        }



        public void Write(int[] value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Length * 12;
        }

        public void Write(uint[] value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Length * 12;
        }



        public void Write(IList<bool> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 8;
        }

        public void Write(IList<char> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 4;
        }

        public void Write(IList<byte> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 4;
        }

        public void Write(IList<sbyte> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 5;
        }

        public void Write(IList<short> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 6;
        }

        public void Write(IList<ushort> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 7;
        }

        public void Write(IList<int> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 12;
        }

        public void Write(IList<uint> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 11;
        }

        public void Write(IList<long> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 21;
        }

        public void Write(IList<ulong> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 20;
        }

        public void Write(IList<float> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 11;
        }

        public void Write(IList<double> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 20;
        }

        public void Write(IList<decimal> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 20;
        }

        public void Write(IList<string> value)
        {
            if (value == null)
                size += 7;
            else
            {
                foreach (string s in value)
                {
                    if (s != null)
                        size += s.Length + 3;
                    else
                        size += 7;
                }
            }
        }

        public void Write(IList<DateTime> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 36;
        }

        public void Write(IList<DateTimeOffset> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 20;
        }

        public void Write(IList<TimeSpan> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 20;
        }

        public void Write(IList<Guid> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 39;
        }

        public void Write(IList<Enum> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 10;
        }

        public void Write(IList<Uri> value)
        {
            if (value == null)
                size += 7;
            else
            {
                foreach (Uri s in value)
                {
                    if (s != null)
                        size += s.AbsoluteUri.Length + 3;
                    else
                        size += 7;
                }
            }
        }



        #endregion


        public void Write(ArraySegment<bool> value)
        {
            if (value == null)
                size += 7;
            else
                size += value.Count * 8;
        }

        

        public void Write(IEnumerable<bool> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<char> value)
        {
            throw new NotImplementedException();
        }

       

        public void Write(IEnumerable<char> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<sbyte> value)
        {
            throw new NotImplementedException();
        }

       

        public void Write(IEnumerable<sbyte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<byte> value)
        {
            throw new NotImplementedException();
        }

        

        public void Write(IEnumerable<byte> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<short> value)
        {
            throw new NotImplementedException();
        }

        

        public void Write(IEnumerable<short> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<ushort> value)
        {
            throw new NotImplementedException();
        }

        

        public void Write(IEnumerable<ushort> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<int> value)
        {
            throw new NotImplementedException();
        }

        

        public void Write(IEnumerable<int> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<uint> value)
        {
            throw new NotImplementedException();
        }

        

        public void Write(IEnumerable<uint> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<long> value)
        {
            throw new NotImplementedException();
        }

        

        public void Write(IEnumerable<long> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<ulong> value)
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

        public void Write(ArraySegment<float> value)
        {
            throw new NotImplementedException();
        }

        

        public void Write(IEnumerable<float> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<double> value)
        {
            throw new NotImplementedException();
        }

        

        public void Write(IEnumerable<double> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<decimal> value)
        {
            throw new NotImplementedException();
        }

       

        public void Write(IEnumerable<decimal> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<DateTime> value)
        {
            throw new NotImplementedException();
        }

       

        public void Write(IEnumerable<DateTime> value)
        {
            throw new NotImplementedException();
        }

        public void Write(ArraySegment<string> value)
        {
            throw new NotImplementedException();
        }

        

        public void Write(IEnumerable<string> value)
        {
            throw new NotImplementedException();
        }

        #region IDictionary Int

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

        #endregion

        #region IDictionary String

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

        #endregion

        #region IDictionary Long

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

        public void Write(object value)
        {
            size += 5;

            ShiboJsonStringSerializer.Serialize(this, value, sers[curObj++]);
        }
        
    }
}
