using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization
{
    interface IWriterBaseType
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
    }
}
