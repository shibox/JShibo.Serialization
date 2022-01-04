using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace JShibo.Serialization
{
    /// <summary>
    /// �������Ż����ڴ��
    /// ���ܱ�ϵͳ������һ������
    /// ע�⣺Ϊ���������ܣ�ȥ����һЩ��ȫ�Ե��жϣ�Ŀǰ�����ڲ�ʹ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XPool<T> where T : struct
    {
        /// <summary>
        /// ÿ��Ͱ��������������
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
        /// ���ڴ滺���������ڴ�
        /// </summary>
        /// <param name="minimumLength">����ĳ���</param>
        /// <returns>����2��ָ�����Ļ��泤��</returns>
        public T[] Rent(int minimumLength)
        {
            Debug.WriteLineIf(minimumLength < 0, new ArgumentOutOfRangeException(nameof(minimumLength)).StackTrace);
            Debug.WriteLineIf(minimumLength == 0, "minimumLength can not be 0");
            //�������ַ������ܲ��
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
            //�ڴ�ػ������黹�����������ָ�����ȵ�
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