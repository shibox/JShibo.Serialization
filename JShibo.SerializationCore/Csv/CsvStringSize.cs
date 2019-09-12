using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.Csv
{
    public class CsvStringSize
    {
        #region 字段

        int size = 0;

        internal int Size
        {
            get { return size; }
        }

        #endregion

        #region 方法

        internal void Write(string value)
        {
            if (value != null)
                size += value.Length + 3;
            else
                size += 7;
        }

        internal void Write(Uri value)
        {
            if (value != null)
                size += value.AbsoluteUri.Length + 3;
            else
                size += 7;
        }

        #endregion
    }
}
