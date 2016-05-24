using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JShibo.Serialization
{
    interface IByteBase
    {
        ArraySegment<byte> ToArraySegment();
        void WriteTo(Stream stream);
        byte[] GetBuffer();
        byte[] ToArray();
    }

    interface ICharBase
    {
        ArraySegment<char> ToArraySegment();
        void WriteTo(Stream stream);
        char[] GetBuffer();
        char[] ToArray();
    }
}
