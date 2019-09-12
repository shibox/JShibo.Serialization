using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization
{
    /// <summary>
    /// 可变长度的写入接口
    /// </summary>
    interface IVWriter
    {
        void WriteVInt(int value);
        void WriteVInt(uint value);
        void WriteVLong(long value);
        void WriteVLong(ulong value);
    }
}
