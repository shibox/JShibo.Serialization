using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization
{
    internal interface ISerializerArray
    {
        void Write(ArraySegment<bool> value);
        void Write(ArraySegment<byte> value);
        void Write(ArraySegment<sbyte> value);
        void Write(ArraySegment<short> value);
        void Write(ArraySegment<ushort> value);
        void Write(ArraySegment<int> value);
        void Write(ArraySegment<uint> value);
        void Write(ArraySegment<long> value);
        void Write(ArraySegment<ulong> value);
        void Write(ArraySegment<float> value);
        void Write(ArraySegment<double> value);
        void Write(ArraySegment<decimal> value);
        void Write(ArraySegment<char> value);
        void Write(ArraySegment<string> value);
        void Write(ArraySegment<DateTime> value);
        void Write(ArraySegment<DateTimeOffset> value);
        void Write(ArraySegment<TimeSpan> value);
        void Write(ArraySegment<Guid> value);
        void Write(ArraySegment<Uri> value);


        void Write(bool[] value);
        void Write(byte[] value);
        void Write(sbyte[] value);
        void Write(short[] value);
        void Write(ushort[] value);
        void Write(int[] value);
        void Write(uint[] value);
        void Write(long[] value);
        void Write(ulong[] value);
        void Write(float[] value);
        void Write(double[] value);
        void Write(decimal[] value);
        void Write(char[] value);
        void Write(string[] value);
        void Write(DateTime[] value);
        void Write(DateTimeOffset[] value);
        void Write(TimeSpan[] value);
        void Write(Guid[] value);
        void Write(Uri[] value);


        void Write(IList<bool> value);
        void Write(IList<byte> value);
        void Write(IList<sbyte> value);
        void Write(IList<short> value);
        void Write(IList<ushort> value);
        void Write(IList<int> value);
        void Write(IList<uint> value);
        void Write(IList<long> value);
        void Write(IList<ulong> value);
        void Write(IList<float> value);
        void Write(IList<double> value);
        void Write(IList<decimal> value);
        void Write(IList<char> value);
        void Write(IList<string> value);
        void Write(IList<DateTime> value);
        void Write(IList<DateTimeOffset> value);
        void Write(IList<TimeSpan> value);
        void Write(IList<Guid> value);
        void Write(IList<Uri> value);



        void Write(IEnumerable<bool> value);
        void Write(IEnumerable<byte> value);
        void Write(IEnumerable<sbyte> value);
        void Write(IEnumerable<short> value);
        void Write(IEnumerable<ushort> value);
        void Write(IEnumerable<int> value);
        void Write(IEnumerable<uint> value);
        void Write(IEnumerable<long> value);
        void Write(IEnumerable<ulong> value);
        void Write(IEnumerable<float> value);
        void Write(IEnumerable<double> value);
        void Write(IEnumerable<decimal> value);
        void Write(IEnumerable<char> value);
        void Write(IEnumerable<string> value);
        void Write(IEnumerable<DateTime> value);
        void Write(IEnumerable<DateTimeOffset> value);
        void Write(IEnumerable<TimeSpan> value);
        void Write(IEnumerable<Guid> value);
        void Write(IEnumerable<Uri> value);


        IList<bool> ReadBooleans();
        IList<byte> ReadBytes();
        IList<sbyte> ReadSBytes();
        IList<short> ReadShorts();
        IList<ushort> ReadUShorts();
        IList<int> ReadInts();
        IList<uint> ReadUInts();
        IList<long> ReadLongs();
        IList<ulong> ReadULongs();
        IList<float> ReadFloats();
        IList<double> ReadDoubles();
        IList<decimal> ReadDecimals();
        IList<char> ReadChars();
        IList<string> ReadStrings();
        IList<DateTime> ReadDateTimes();
        IList<DateTimeOffset> ReadDateTimeOffsets();
        IList<TimeSpan> ReadTimeSpans();
        IList<Guid> ReadGuids();
        IList<Uri> ReadUris();

    }
}
