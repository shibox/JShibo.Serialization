using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using JShibo.Serialization.Common;

namespace JShibo.Serialization.Json
{
    /// <summary>
    /// 固定长度的Json序列化
    /// </summary>
    internal class JsonStringFixed:JsonString
    {
        internal JsonStringFixed()
        { 
        
        }

        internal override unsafe void ResizeAndWriteName(int size)
        {
            if (names.Length > 0)
            {
                string name = names[current];
                if (_buffer.Length < position + size + name.Length)
                    Resize(size + name.Length + 4);
                fixed (char* psrc = name, pdst = &_buffer[position])
                {
                    char* tsrc = psrc, tdst = pdst;
                    *tdst++ = '"';
                    Utils.wstrcpy(tdst, tsrc, name.Length);
                    tdst += name.Length;
                    *tdst++ = '"';
                    *tdst++ = ':';
                    position += name.Length + 3;
                }
                current++;
            }
        }


    }
}
