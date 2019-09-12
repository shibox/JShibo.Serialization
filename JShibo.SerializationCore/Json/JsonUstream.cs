using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.Json
{
    public class JsonUstream:OBase
    {
        #region 字段

        internal Type[] types;
        internal Deserialize<JsonUstream>[] desers;
        internal int[] typeCounts;
        internal int[] nameCounts;

        internal byte[] namesBytes;
        unsafe internal byte* namesBytesP;
        internal int[] nameLens;
        internal int cnamepos = 0;

        internal string[] names;
        internal byte[] _buffer = null;
        internal int position = 0;
        internal int current = 0;
        internal int currSer = 0;
        internal int maxDepth = 10;
        internal int curDepth = 0;
        //internal bool unFlag = false;
        internal SerializerSettings sets = SerializerSettings.Default;

        #endregion
    }
}
