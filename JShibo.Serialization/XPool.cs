using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace JShibo.Serialization
{
    /// <summary>
    /// 高性能优化的内存池
    /// 性能比系统方案快一倍左右
    /// 注意：为了提升性能，去掉了一些安全性等判断，目前仅供内部使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XPool<T> where T : struct
    {
        /// <summary>
        /// 每个桶里数组的最大数量
        /// </summary>
        const int SlotCount = 8;
        const int BucketCount = 17;
        public static readonly XPool<T> Shared = new();
        readonly Bucket[] _buckets;

        public XPool()
        {
            _buckets = new Bucket[BucketCount];
            for (var i = 0; i < BucketCount; ++i)
            {
                var maxSlotSize = 16 << i;
                _buckets[i] = new Bucket();
                for (var j = 0; j < SlotCount; ++j)
                {
                    if (!_buckets[i].TryPush(new T[maxSlotSize]))
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }

        public static XPool<T> Create(int maxArrayLength, int maxArraysPerBucket)
        {
            return null;
        }

        /// <summary>
        /// 从内存缓冲区申请内存
        /// </summary>
        /// <param name="minimumLength">申请的长度</param>
        /// <returns>返回2的指数方的缓存长度</returns>
        public T[] Rent(int minimumLength)
        {
            Debug.WriteLineIf(minimumLength < 0, new ArgumentOutOfRangeException(nameof(minimumLength)).StackTrace);
            Debug.WriteLineIf(minimumLength == 0, "minimumLength can not be 0");
            //以下两种方案性能差不多
            //var idx = BitOperations.Log2((uint)minimumLength - 1 | 15) - 3;
            var idx = 28 - Lzcnt.LeadingZeroCount(((uint)minimumLength - 1) | 15);
            if(idx < 0)
                return new T[minimumLength];
            return _buckets[idx].TryPop(1 << (int)(idx + 4));
        }

        public void Return(T[] array, bool clearArray = false)
        {
            Debug.WriteLineIf(array == null, new ArgumentNullException(nameof(array)).StackTrace);
            Debug.WriteLineIf(array.Length == 0, "array length can not be 0");
            var idx = 28 - Lzcnt.LeadingZeroCount((uint)array.Length - 1);
            //内存池缓冲区归还的数组必须是指定长度的
            if (idx < 0 || (16 << ((int)idx)) != array.Length)
                return;
            _buckets[idx].TryPush(array);
        }

        private sealed class Bucket
        {
            private T[][] _items = new T[SlotCount][];
            private int _size;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool TryPush(T[] item)
            {
                if (_size >= SlotCount)
                    return false;
                _items[_size++] = item;
                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T[] TryPop(int length)
            {
                if (_size <= 0)
                    return new T[length];
                var arr = _items[--_size];
                _items[_size] = null;
                return arr;
            }

        }
    }

}