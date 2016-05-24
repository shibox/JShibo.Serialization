using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization
{
    public class OBase
    {
        #region 字段

        /// <summary>
        /// 版本，用于定义写入数据的格式
        /// </summary>
        internal const int Version = 1;
        internal const int VersionWriteType = 2;
        internal const byte HAVE_TYPE_FLAG = 0x01;
        internal const byte NULL_FLAG = 0x80;
        internal const byte ZERO_FLAG = 0x00;
        //internal const byte VALUE_FLAG = 0x40;

        internal int[] typeCounts;
        internal string[] names = new string[0];
        internal Encoding[] encodings = new Encoding[0];

        internal bool isHaveObjectType = false;

        internal Type[] types = null;
        internal int position = 0;
        internal int current = 0;
        internal int curObj = 0;
        internal int maxDepth = 10;
        internal int curDepth = 0;
        internal SerializerSettings sets = SerializerSettings.Default;

        #endregion

        #region 属性

        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        public int MaxDepth
        {
            get { return maxDepth; }
            set { maxDepth = value; }
        }

        #endregion
    }
}
