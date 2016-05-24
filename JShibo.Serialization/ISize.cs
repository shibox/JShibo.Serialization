using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization
{
    interface ISize
    {
        void Write(ArraySegment<bool> value);
        void Write(IList<bool> value);
        void Write(IEnumerable<bool> value);

        void Write(ArraySegment<char> value);
        void Write(IList<char> value);
        void Write(IEnumerable<char> value);

        void Write(ArraySegment<sbyte> value);
        void Write(IList<sbyte> value);
        void Write(IEnumerable<sbyte> value);

        void Write(ArraySegment<byte> value);
        void Write(IList<byte> value);
        void Write(IEnumerable<byte> value);

        void Write(ArraySegment<short> value);
        void Write(IList<short> value);
        void Write(IEnumerable<short> value);

        void Write(ArraySegment<ushort> value);
        void Write(IList<ushort> value);
        void Write(IEnumerable<ushort> value);

        void Write(ArraySegment<int> value);
        void Write(IList<int> value);
        void Write(IEnumerable<int> value);

        void Write(ArraySegment<uint> value);
        void Write(IList<uint> value);
        void Write(IEnumerable<uint> value);

        void Write(ArraySegment<long> value);
        void Write(IList<long> value);
        void Write(IEnumerable<long> value);

        void Write(ArraySegment<ulong> value);
        void Write(IList<ulong> value);
        void Write(IEnumerable<ulong> value);

        void Write(float[] value);
        void Write(ArraySegment<float> value);
        void Write(IList<float> value);
        void Write(IEnumerable<float> value);

        void Write(ArraySegment<double> value);
        void Write(IList<double> value);
        void Write(IEnumerable<double> value);

        void Write(ArraySegment<decimal> value);
        void Write(IList<decimal> value);
        void Write(IEnumerable<decimal> value);

        void Write(ArraySegment<DateTime> value);
        void Write(IList<DateTime> value);
        void Write(IEnumerable<DateTime> value);

        void Write(ArraySegment<string> value);
        void Write(IList<string> value);
        void Write(IEnumerable<string> value);


        void Write(IDictionary<int, object> value);
        void Write(IDictionary<int, bool> value);
        void Write(IDictionary<int, char> value);
        void Write(IDictionary<int, sbyte> value);
        void Write(IDictionary<int, byte> value);
        void Write(IDictionary<int, short> value);
        void Write(IDictionary<int, ushort> value);
        void Write(IDictionary<int, int> value);
        void Write(IDictionary<int, uint> value);
        void Write(IDictionary<int, long> value);
        void Write(IDictionary<int, ulong> value);
        void Write(IDictionary<int, float> value);
        void Write(IDictionary<int, double> value);
        void Write(IDictionary<int, decimal> value);
        void Write(IDictionary<int, DateTime> value);
        void Write(IDictionary<int, string> value);



        void Write(IDictionary<string, object> value);
        void Write(IDictionary<string, bool> value);
        void Write(IDictionary<string, char> value);
        void Write(IDictionary<string, sbyte> value);
        void Write(IDictionary<string, byte> value);
        void Write(IDictionary<string, short> value);
        void Write(IDictionary<string, ushort> value);
        void Write(IDictionary<string, int> value);
        void Write(IDictionary<string, uint> value);
        void Write(IDictionary<string, long> value);
        void Write(IDictionary<string, ulong> value);
        void Write(IDictionary<string, float> value);
        void Write(IDictionary<string, double> value);
        void Write(IDictionary<string, decimal> value);
        void Write(IDictionary<string, DateTime> value);
        void Write(IDictionary<string, string> value);


        void Write(IDictionary<long, object> value);
        void Write(IDictionary<long, bool> value);
        void Write(IDictionary<long, char> value);
        void Write(IDictionary<long, sbyte> value);
        void Write(IDictionary<long, byte> value);
        void Write(IDictionary<long, short> value);
        void Write(IDictionary<long, ushort> value);
        void Write(IDictionary<long, int> value);
        void Write(IDictionary<long, uint> value);
        void Write(IDictionary<long, long> value);
        void Write(IDictionary<long, ulong> value);
        void Write(IDictionary<long, float> value);
        void Write(IDictionary<long, double> value);
        void Write(IDictionary<long, decimal> value);
        void Write(IDictionary<long, DateTime> value);
        void Write(IDictionary<long, string> value);
    }
}
