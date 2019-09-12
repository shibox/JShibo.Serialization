using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.Soc
{
    public class ObjectStreamSize
    {
        #region 字段属性构造函数

        int size = 0;

        internal int Size
        {
            get { return size; }
        }

        internal ObjectStreamSize()
        { }

        #endregion
    }
}
