using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JShibo.Serialization
{
    //Empty = 0,
    //Object = 1,
    //DBNull = 2,
    //Boolean = 3,
    //Char = 4,
    //SByte = 5,
    //Byte = 6,
    //Int16 = 7,
    //UInt16 = 8,
    //Int32 = 9,
    //UInt32 = 10,
    //Int64 = 11,
    //UInt64 = 12,
    //Single = 13,
    //Double = 14,
    //Decimal = 15,
    //DateTime = 16,
    //String = 18,
    //DateTimeOffset
    //TimeSpan
    //Guid
    //Stream
    //IList
    //IDictionary
    //IEnumerable


    interface IWriter
    {
        void Write(object value);
        void Write(bool value);
        void Write(char value);
        void Write(sbyte value);
        void Write(byte value);
        void Write(short value);
        void Write(ushort value);
        void Write(int value);
        void Write(uint value);
        void Write(long value);
        void Write(ulong value);
        void Write(float value);
        void Write(double value);
        void Write(decimal value);
        void Write(DateTime value);
        void Write(string value);
        void Write(DateTimeOffset value);
        void Write(TimeSpan value);
        void Write(Guid value);
        void Write(Stream value);
        void Write(IList value);
        void Write(IDictionary value);
        void Write(IEnumerable value);



        void Write(object[] value);
        void Write(List<object> value);
        void Write(IList<object> value);
        void Write(IEnumerable<object> value);

        void Write(bool[] value);
        void Write(List<bool> value);
        void Write(IList<bool> value);
        void Write(IEnumerable<bool> value);

        void Write(char[] value);
        void Write(List<char> value);
        void Write(IList<char> value);
        void Write(IEnumerable<char> value);

        void Write(sbyte[] value);
        void Write(List<sbyte> value);
        void Write(IList<sbyte> value);
        void Write(IEnumerable<sbyte> value);

        void Write(byte[] value);
        void Write(List<byte> value);
        void Write(IList<byte> value);
        void Write(IEnumerable<byte> value);

        void Write(short[] value);
        void Write(List<short> value);
        void Write(IList<short> value);
        void Write(IEnumerable<short> value);

        void Write(ushort[] value);
        void Write(List<ushort> value);
        void Write(IList<ushort> value);
        void Write(IEnumerable<ushort> value);

        void Write(int[] value);
        void Write(List<int> value);
        void Write(IList<int> value);
        void Write(IEnumerable<int> value);

        void Write(uint[] value);
        void Write(List<uint> value);
        void Write(IList<uint> value);
        void Write(IEnumerable<uint> value);

        void Write(long[] value);
        void Write(List<long> value);
        void Write(IList<long> value);
        void Write(IEnumerable<long> value);

        void Write(ulong[] value);
        void Write(List<ulong> value);
        void Write(IList<ulong> value);
        void Write(IEnumerable<ulong> value);

        void Write(float[] value);
        void Write(List<float> value);
        void Write(IList<float> value);
        void Write(IEnumerable<float> value);

        void Write(double[] value);
        void Write(List<double> value);
        void Write(IList<double> value);
        void Write(IEnumerable<double> value);

        void Write(decimal[] value);
        void Write(List<decimal> value);
        void Write(IList<decimal> value);
        void Write(IEnumerable<decimal> value);

        void Write(DateTime[] value);
        void Write(List<DateTime> value);
        void Write(IList<DateTime> value);
        void Write(IEnumerable<DateTime> value);

        void Write(string[] value);
        void Write(List<string> value);
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


        void Write(ArraySegment<bool> value);
        void Write(ArraySegment<char> value);
        void Write(ArraySegment<sbyte> value);
        void Write(ArraySegment<byte> value);
        void Write(ArraySegment<short> value);
        void Write(ArraySegment<ushort> value);
        void Write(ArraySegment<int> value);
        void Write(ArraySegment<uint> value);
        void Write(ArraySegment<long> value);
        void Write(ArraySegment<ulong> value);
        void Write(ArraySegment<float> value);
        void Write(ArraySegment<double> value);
        void Write(ArraySegment<decimal> value);
        void Write(ArraySegment<DateTime> value);
        void Write(ArraySegment<string> value);
    }




    //public abstract class IWriter
    //{
    //    internal abstract void Write(object value);
    //    internal abstract void Write(bool value);
    //    internal abstract void Write(char value);
    //    internal abstract void Write(sbyte value);
    //    internal abstract void Write(byte value);
    //    internal abstract void Write(short value);
    //    internal abstract void Write(ushort value);
    //    internal abstract void Write(int value);
    //    internal abstract void Write(uint value);
    //    internal abstract void Write(long value);
    //    internal abstract void Write(ulong value);
    //    internal abstract void Write(float value);
    //    internal abstract void Write(double value);
    //    internal abstract void Write(decimal value);
    //    internal abstract void Write(DateTime value);
    //    internal abstract void Write(string value);
    //    internal abstract void Write(DateTimeOffset value);
    //    internal abstract void Write(TimeSpan value);
    //    internal abstract void Write(Guid value);
    //    internal abstract void Write(Stream value);
    //    internal abstract void Write(IList value);
    //    internal abstract void Write(IDictionary value);
    //    internal abstract void Write(IEnumerable value);

    //    internal abstract void Write(object[] value);
    //    internal abstract void Write(List<object> value);
    //    internal abstract void Write(IList<object> value);
    //    internal abstract void Write(IEnumerable<object> value);

    //    internal abstract void Write(bool[] value);
    //    internal abstract void Write(List<bool> value);
    //    internal abstract void Write(IList<bool> value);
    //    internal abstract void Write(IEnumerable<bool> value);

    //    internal abstract void Write(char[] value);
    //    internal abstract void Write(List<char> value);
    //    internal abstract void Write(IList<char> value);
    //    internal abstract void Write(IEnumerable<char> value);

    //    internal abstract void Write(sbyte[] value);
    //    internal abstract void Write(List<sbyte> value);
    //    internal abstract void Write(IList<sbyte> value);
    //    internal abstract void Write(IEnumerable<sbyte> value);

    //    internal abstract void Write(byte[] value);
    //    internal abstract void Write(List<byte> value);
    //    internal abstract void Write(IList<byte> value);
    //    internal abstract void Write(IEnumerable<byte> value);

    //    internal abstract void Write(short[] value);
    //    internal abstract void Write(List<short> value);
    //    internal abstract void Write(IList<short> value);
    //    internal abstract void Write(IEnumerable<short> value);

    //    internal abstract void Write(ushort[] value);
    //    internal abstract void Write(List<ushort> value);
    //    internal abstract void Write(IList<ushort> value);
    //    internal abstract void Write(IEnumerable<ushort> value);

    //    internal abstract void Write(int[] value);
    //    internal abstract void Write(List<int> value);
    //    internal abstract void Write(IList<int> value);
    //    internal abstract void Write(IEnumerable<int> value);

    //    internal abstract void Write(uint[] value);
    //    internal abstract void Write(List<uint> value);
    //    internal abstract void Write(IList<uint> value);
    //    internal abstract void Write(IEnumerable<uint> value);

    //    internal abstract void Write(long[] value);
    //    internal abstract void Write(List<long> value);
    //    internal abstract void Write(IList<long> value);
    //    internal abstract void Write(IEnumerable<long> value);

    //    internal abstract void Write(ulong[] value);
    //    internal abstract void Write(List<ulong> value);
    //    internal abstract void Write(IList<ulong> value);
    //    internal abstract void Write(IEnumerable<ulong> value);

    //    internal abstract void Write(float[] value);
    //    internal abstract void Write(List<float> value);
    //    internal abstract void Write(IList<float> value);
    //    internal abstract void Write(IEnumerable<float> value);

    //    internal abstract void Write(double[] value);
    //    internal abstract void Write(List<double> value);
    //    internal abstract void Write(IList<double> value);
    //    internal abstract void Write(IEnumerable<double> value);

    //    internal abstract void Write(decimal[] value);
    //    internal abstract void Write(List<decimal> value);
    //    internal abstract void Write(IList<decimal> value);
    //    internal abstract void Write(IEnumerable<decimal> value);

    //    internal abstract void Write(DateTime[] value);
    //    internal abstract void Write(List<DateTime> value);
    //    internal abstract void Write(IList<DateTime> value);
    //    internal abstract void Write(IEnumerable<DateTime> value);

    //    internal abstract void Write(string[] value);
    //    internal abstract void Write(List<string> value);
    //    internal abstract void Write(IList<string> value);
    //    internal abstract void Write(IEnumerable<string> value);



    //    internal abstract void Write(IDictionary<int, object> value);
    //    internal abstract void Write(IDictionary<int, bool> value);
    //    internal abstract void Write(IDictionary<int, char> value);
    //    internal abstract void Write(IDictionary<int, sbyte> value);
    //    internal abstract void Write(IDictionary<int, byte> value);
    //    internal abstract void Write(IDictionary<int, short> value);
    //    internal abstract void Write(IDictionary<int, ushort> value);
    //    internal abstract void Write(IDictionary<int, int> value);
    //    internal abstract void Write(IDictionary<int, uint> value);
    //    internal abstract void Write(IDictionary<int, long> value);
    //    internal abstract void Write(IDictionary<int, ulong> value);
    //    internal abstract void Write(IDictionary<int, float> value);
    //    internal abstract void Write(IDictionary<int, double> value);
    //    internal abstract void Write(IDictionary<int, decimal> value);
    //    internal abstract void Write(IDictionary<int, DateTime> value);
    //    internal abstract void Write(IDictionary<int, string> value);



    //    internal abstract void Write(IDictionary<string, object> value);
    //    internal abstract void Write(IDictionary<string, bool> value);
    //    internal abstract void Write(IDictionary<string, char> value);
    //    internal abstract void Write(IDictionary<string, sbyte> value);
    //    internal abstract void Write(IDictionary<string, byte> value);
    //    internal abstract void Write(IDictionary<string, short> value);
    //    internal abstract void Write(IDictionary<string, ushort> value);
    //    internal abstract void Write(IDictionary<string, int> value);
    //    internal abstract void Write(IDictionary<string, uint> value);
    //    internal abstract void Write(IDictionary<string, long> value);
    //    internal abstract void Write(IDictionary<string, ulong> value);
    //    internal abstract void Write(IDictionary<string, float> value);
    //    internal abstract void Write(IDictionary<string, double> value);
    //    internal abstract void Write(IDictionary<string, decimal> value);
    //    internal abstract void Write(IDictionary<string, DateTime> value);
    //    internal abstract void Write(IDictionary<string, string> value);


    //    internal abstract void Write(IDictionary<long, object> value);
    //    internal abstract void Write(IDictionary<long, bool> value);
    //    internal abstract void Write(IDictionary<long, char> value);
    //    internal abstract void Write(IDictionary<long, sbyte> value);
    //    internal abstract void Write(IDictionary<long, byte> value);
    //    internal abstract void Write(IDictionary<long, short> value);
    //    internal abstract void Write(IDictionary<long, ushort> value);
    //    internal abstract void Write(IDictionary<long, int> value);
    //    internal abstract void Write(IDictionary<long, uint> value);
    //    internal abstract void Write(IDictionary<long, long> value);
    //    internal abstract void Write(IDictionary<long, ulong> value);
    //    internal abstract void Write(IDictionary<long, float> value);
    //    internal abstract void Write(IDictionary<long, double> value);
    //    internal abstract void Write(IDictionary<long, decimal> value);
    //    internal abstract void Write(IDictionary<long, DateTime> value);
    //    internal abstract void Write(IDictionary<long, string> value);


    //}



    //interface IWriter
    //{
    //    internal abstract void Write(object value);
    //    void Write(bool value);
    //    void Write(char value);
    //    void Write(sbyte value);
    //    void Write(byte value);
    //    void Write(short value);
    //    void Write(ushort value);
    //    void Write(int value);
    //    void Write(uint value);
    //    void Write(long value);
    //    void Write(ulong value);
    //    void Write(float value);
    //    void Write(double value);
    //    void Write(decimal value);
    //    void Write(DateTime value);
    //    void Write(string value);

    //    void Write(object[] value);
    //    void Write(List<object> value);
    //    void Write(IList<object> value);
    //    void Write(IEnumerable<object> value);

    //    void Write(bool[] value);
    //    void Write(List<bool> value);
    //    void Write(IList<bool> value);
    //    void Write(IEnumerable<bool> value);

    //    void Write(char[] value);
    //    void Write(List<char> value);
    //    void Write(IList<char> value);
    //    void Write(IEnumerable<char> value);

    //    void Write(sbyte[] value);
    //    void Write(List<sbyte> value);
    //    void Write(IList<sbyte> value);
    //    void Write(IEnumerable<sbyte> value);

    //    void Write(byte[] value);
    //    void Write(List<byte> value);
    //    void Write(IList<byte> value);
    //    void Write(IEnumerable<byte> value);

    //    void Write(short[] value);
    //    void Write(List<short> value);
    //    void Write(IList<short> value);
    //    void Write(IEnumerable<short> value);

    //    void Write(ushort[] value);
    //    void Write(List<ushort> value);
    //    void Write(IList<ushort> value);
    //    void Write(IEnumerable<ushort> value);

    //    void Write(int[] value);
    //    void Write(List<int> value);
    //    void Write(IList<int> value);
    //    void Write(IEnumerable<int> value);

    //    void Write(uint[] value);
    //    void Write(List<uint> value);
    //    void Write(IList<uint> value);
    //    void Write(IEnumerable<uint> value);

    //    void Write(long[] value);
    //    void Write(List<long> value);
    //    void Write(IList<long> value);
    //    void Write(IEnumerable<long> value);

    //    void Write(ulong[] value);
    //    void Write(List<ulong> value);
    //    void Write(IList<ulong> value);
    //    void Write(IEnumerable<ulong> value);

    //    void Write(float[] value);
    //    void Write(List<float> value);
    //    void Write(IList<float> value);
    //    void Write(IEnumerable<float> value);

    //    void Write(double[] value);
    //    void Write(List<double> value);
    //    void Write(IList<double> value);
    //    void Write(IEnumerable<double> value);

    //    void Write(decimal[] value);
    //    void Write(List<decimal> value);
    //    void Write(IList<decimal> value);
    //    void Write(IEnumerable<decimal> value);

    //    void Write(DateTime[] value);
    //    void Write(List<DateTime> value);
    //    void Write(IList<DateTime> value);
    //    void Write(IEnumerable<DateTime> value);

    //    void Write(string[] value);
    //    void Write(List<string> value);
    //    void Write(IList<string> value);
    //    void Write(IEnumerable<string> value);



    //}
}
