using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace JShibo.Serialization
{
    /// <summary>
    /// 字符缓存管理区
    /// </summary>
    public class CharsBufferManager
    {
        #region 字段

        static int MinSize = 32;
        static int MaxSize = 1024 * 1024 * 16;
        static int MaxIndex = 0;
        static int Right = 0;
        static int Middle = 0;

        static int[] indexs = null;
        static List<Stack<char[]>> buffers = null;
        static char[] tempBuffer;
        static int tempIndex = -1;

        #endregion

        #region 构造函数

        static CharsBufferManager()
        {
            buffers = new List<Stack<char[]>>();
            MaxIndex = (int)Math.Log(MaxSize / MinSize, 1.1) + 1;
            for (int i = 0; i < MaxIndex; i++)
                buffers.Add(new Stack<char[]>());
            indexs = new int[MaxIndex];
            for (int i = 0; i < MaxIndex; i++)
                indexs[i] = (int)(MinSize * Math.Pow(1.1, i));
            Right = MaxIndex;
            Middle = MaxIndex >> 1;
        }

        #endregion
        
        #region 方法

        public static char[] GetBuffer(int size)
        {
            int index = BinarySearch(size);
            if (tempIndex == index)
                return tempBuffer;
            if (index < MaxIndex && buffers[index].Count > 0)
            {
                tempBuffer = buffers[index].Pop();
                tempIndex = index;
                return tempBuffer;
            }
            return new char[size];
        }

        public static void SetBuffer(char[] value)
        {
            int index = BinarySearch(value.Length);
            if (index != tempIndex && index < MaxIndex)
                buffers[index].Push(value);
        }

        public static char[] GetBufferSync(int size)
        {
            int index = BinarySearch(size);
            char[] result = null;
            if (index < MaxIndex)
            {
                Stack<char[]> buffer = buffers[index];
                if (buffer.Count > 0)
                {
                    try
                    {
                        Monitor.Enter(buffer);
                        if (buffer.Count > 0)
                            result = buffer.Pop();
                    }
                    finally
                    {
                        Monitor.Exit(buffer);
                    }
                }
            }
            result = new char[size];
            return result;
        }

        public static void SetBufferSync(char[] value)
        {
            int index = BinarySearch(value.Length);
            if (index != tempIndex && index < MaxIndex)
                buffers[index].Push(value);

            //int index = (int)Math.Log(value.Length / MinSize, 1.1);
            //if (index < MaxIndex)
            //{
            //    Stack<char[]> buffer = buffers[index];
            //    //lock (buffer)
            //    //{
            //    buffer.Push(value);
            //    //}
            //}
        }

        private static int BinarySearch(int value)
        {
            int left = 0;
            int right = Right;
            int middle = Middle;
            while (left <= right)
            {
                if (value > indexs[middle])
                    left = middle + 1;
                else if (value < indexs[middle])
                    right = middle - 1;
                else
                    break;
                middle = (right + left) >> 1;
            }
            return middle + 1;
        }

        #endregion

    }

}

