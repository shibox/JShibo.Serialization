using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JShibo.Serialization
{
    /// <summary>
    /// 比较节约内存的内存池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XPoolSave<T> where T : struct
    {
        #region 字段

        static int MinSize = 32;
        static int MaxSize = 1024 * 1024 * 16;
        static int MaxIndex = 0;
        static int Right = 0;
        static int Middle = 0;

        static int[] indexs = null;
        static List<Stack<T[]>> buffers = null;
        static T[] tempBuffer;
        static int tempIndex = -1;

        #endregion

        #region 构造函数

        static XPoolSave()
        {
            buffers = new List<Stack<T[]>>();
            MaxIndex = (int)Math.Log(MaxSize / MinSize, 1.1) + 1;
            for (int i = 0; i < MaxIndex; i++)
                buffers.Add(new Stack<T[]>());
            indexs = new int[MaxIndex];
            for (int i = 0; i < MaxIndex; i++)
                indexs[i] = (int)(MinSize * Math.Pow(1.1, i));
            Right = MaxIndex;
            Middle = MaxIndex >> 1;
        }

        #endregion

        #region 方法

        public static T[] Rent(int size)
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
            return new T[size];
        }

        public static void Return(T[] value)
        {
            int index = BinarySearch(value.Length);
            if (index != tempIndex && index < MaxIndex)
                buffers[index].Push(value);
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
