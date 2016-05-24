using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
//using System.Linq;
using System.Text;

namespace JShibo.Serialization.Common
{
    /// <summary>
    /// 快速的将数字转换成字符串,相比直接ToString()性能提升很多，并且减少了很多不必要的转换逻辑
    /// long类型最大19个长度
    /// TimeSpan格式化最终需要特殊处理，速度比较慢
    /// 
    /// 格式化概述 http://msdn.microsoft.com/zh-cn/library/26etazsy%28v=vs.80%29.aspx
    /// 
    /// float格式化
    /// http://msdn.microsoft.com/en-us/library/aa280873
    /// http://blog.csdn.net/krocwang/article/details/4444491
    /// http://www.cnblogs.com/fromchaos/archive/2010/12/07/1898698.html
    /// </summary>
    public class FastToString
    {
        #region 常量

        const string Int32Min = "-2147483648";
        const string Int64Min = "-9223372036854775808";

        static readonly TwoDigits[] DigitPairs;
        static readonly char[] WriteGuidLookup = new char[] 
        { '0', '0', '0', '1', '0', '2', '0', '3', '0', '4', '0', '5', '0', '6', '0', '7', '0', '8', '0', '9', '0', 'a', '0', 'b', '0', 'c', '0', 'd', '0', 'e', '0', 'f', '1', '0', '1', '1', '1', '2', '1', '3', '1', '4', '1', '5', '1', '6', '1', '7', '1', '8', '1', '9', '1', 'a', '1', 'b', '1', 'c', '1', 'd', '1', 'e', '1', 'f', '2', '0', '2', '1', '2', '2', '2', '3', '2', '4', '2', '5', '2', '6', '2', '7', '2', '8', '2', '9', '2', 'a', '2', 'b', '2', 'c', '2', 'd', '2', 'e', '2', 'f', '3', '0', '3', '1', '3', '2', '3', '3', '3', '4', '3', '5', '3', '6', '3', '7', '3', '8', '3', '9', '3', 'a', '3', 'b', '3', 'c', '3', 'd', '3', 'e', '3', 'f', '4', '0', '4', '1', '4', '2', '4', '3', '4', '4', '4', '5', '4', '6', '4', '7', '4', '8', '4', '9', '4', 'a', '4', 'b', '4', 'c', '4', 'd', '4', 'e', '4', 'f', '5', '0', '5', '1', '5', '2', '5', '3', '5', '4', '5', '5', '5', '6', '5', '7', '5', '8', '5', '9', '5', 'a', '5', 'b', '5', 'c', '5', 'd', '5', 'e', '5', 'f', '6', '0', '6', '1', '6', '2', '6', '3', '6', '4', '6', '5', '6', '6', '6', '7', '6', '8', '6', '9', '6', 'a', '6', 'b', '6', 'c', '6', 'd', '6', 'e', '6', 'f', '7', '0', '7', '1', '7', '2', '7', '3', '7', '4', '7', '5', '7', '6', '7', '7', '7', '8', '7', '9', '7', 'a', '7', 'b', '7', 'c', '7', 'd', '7', 'e', '7', 'f', '8', '0', '8', '1', '8', '2', '8', '3', '8', '4', '8', '5', '8', '6', '8', '7', '8', '8', '8', '9', '8', 'a', '8', 'b', '8', 'c', '8', 'd', '8', 'e', '8', 'f', '9', '0', '9', '1', '9', '2', '9', '3', '9', '4', '9', '5', '9', '6', '9', '7', '9', '8', '9', '9', '9', 'a', '9', 'b', '9', 'c', '9', 'd', '9', 'e', '9', 'f', 'a', '0', 'a', '1', 'a', '2', 'a', '3', 'a', '4', 'a', '5', 'a', '6', 'a', '7', 'a', '8', 'a', '9', 'a', 'a', 'a', 'b', 'a', 'c', 'a', 'd', 'a', 'e', 'a', 'f', 'b', '0', 'b', '1', 'b', '2', 'b', '3', 'b', '4', 'b', '5', 'b', '6', 'b', '7', 'b', '8', 'b', '9', 'b', 'a', 'b', 'b', 'b', 'c', 'b', 'd', 'b', 'e', 'b', 'f', 'c', '0', 'c', '1', 'c', '2', 'c', '3', 'c', '4', 'c', '5', 'c', '6', 'c', '7', 'c', '8', 'c', '9', 'c', 'a', 'c', 'b', 'c', 'c', 'c', 'd', 'c', 'e', 'c', 'f', 'd', '0', 'd', '1', 'd', '2', 'd', '3', 'd', '4', 'd', '5', 'd', '6', 'd', '7', 'd', '8', 'd', '9', 'd', 'a', 'd', 'b', 'd', 'c', 'd', 'd', 'd', 'e', 'd', 'f', 'e', '0', 'e', '1', 'e', '2', 'e', '3', 'e', '4', 'e', '5', 'e', '6', 'e', '7', 'e', '8', 'e', '9', 'e', 'a', 'e', 'b', 'e', 'c', 'e', 'd', 'e', 'e', 'e', 'f', 'f', '0', 'f', '1', 'f', '2', 'f', '3', 'f', '4', 'f', '5', 'f', '6', 'f', '7', 'f', '8', 'f', '9', 'f', 'a', 'f', 'b', 'f', 'c', 'f', 'd', 'f', 'e', 'f', 'f' };
       
        #endregion

        #region 静态方法

        static FastToString()
        {
            DigitPairs = new TwoDigits[100];
            for (var i = 0; i < 100; ++i)
            {
                DigitPairs[i] = new TwoDigits((char)('0' + (i / 10)), (char)+('0' + (i % 10)));
            }
        }

        #endregion

        //#region 数字转换（字节）

        //internal static int ToString10(byte[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (byte)((value % 10) + 48);
        //    return 1;
        //}

        //internal static int ToString100(byte[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (byte)((value / 10) + 48);
        //    buffer[pos + 1] = (byte)((value % 10) + 48);
        //    return 2;
        //}

        //internal static int ToString1000(byte[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (byte)((value / 100) + 48);
        //    buffer[pos + 1] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 2] = (byte)((value % 10) + 48);
        //    return 3;
        //}

        //internal static int ToString10000(byte[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (byte)((value / 1000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 2] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 3] = (byte)((value % 10) + 48);
        //    return 4;
        //}

        //internal static int ToString100000(byte[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (byte)((value / 10000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 3] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 4] = (byte)((value % 10) + 48);
        //    return 5;
        //}

        //internal static int ToString1000000(byte[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (byte)((value / 100000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 4] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 5] = (byte)((value % 10) + 48);
        //    return 6;
        //}

        //internal static int ToString10000000(byte[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (byte)((value / 1000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 5] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 6] = (byte)((value % 10) + 48);
        //    return 7;
        //}

        //internal static int ToString100000000(byte[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (byte)((value / 10000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 6] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 7] = (byte)((value % 10) + 48);
        //    return 8;
        //}

        //internal static int ToString1000000000(byte[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (byte)((value / 100000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 7] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 8] = (byte)((value % 10) + 48);
        //    return 9;
        //}

        //internal static int ToString10000000000(byte[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (byte)((value / 1000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 8] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 9] = (byte)((value % 10) + 48);
        //    return 10;
        //}

        //internal static int ToString10000000000(byte[] buffer, int pos, uint value)
        //{
        //    buffer[pos] = (byte)((value / 1000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 8] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 9] = (byte)((value % 10) + 48);
        //    return 10;
        //}

        //internal static int ToString10000000000(byte[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (byte)((value / 1000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 8] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 9] = (byte)((value % 10) + 48);
        //    return 10;
        //}



        //internal static int ToString100000000000(byte[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (byte)((value / 10000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 8] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 9] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 10] = (byte)((value % 10) + 48);
        //    return 11;
        //}

        //internal static int ToString1000000000000(byte[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (byte)((value / 100000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 8] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 9] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 10] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 11] = (byte)((value % 10) + 48);
        //    return 12;
        //}

        //internal static int ToString10000000000000(byte[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (byte)((value / 1000000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 8] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 9] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 10] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 11] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 12] = (byte)((value % 10) + 48);
        //    return 13;
        //}

        //internal static int ToString100000000000000(byte[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (byte)((value / 10000000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 8] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 9] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 10] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 11] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 12] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 13] = (byte)((value % 10) + 48);
        //    return 14;
        //}

        //internal static int ToString1000000000000000(byte[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (byte)((value / 100000000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 100000000000000) / 10000000000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 8] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 9] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 10] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 11] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 12] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 13] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 14] = (byte)((value % 10) + 48);
        //    return 15;
        //}

        //internal static int ToString10000000000000000(byte[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (byte)((value / 1000000000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 1000000000000000) / 100000000000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 100000000000000) / 10000000000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 8] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 9] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 10] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 11] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 12] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 13] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 14] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 15] = (byte)((value % 10) + 48);
        //    return 16;
        //}

        //internal static int ToString100000000000000000(byte[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (byte)((value / 10000000000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 1000000000000000) / 100000000000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 100000000000000) / 10000000000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 8] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 9] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 10] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 11] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 12] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 13] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 14] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 15] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 16] = (byte)((value % 10) + 48);
        //    return 17;
        //}

        //internal static int ToString1000000000000000000(byte[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (byte)((value / 100000000000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 100000000000000000) / 10000000000000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 1000000000000000) / 100000000000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 100000000000000) / 10000000000000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 8] = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 9] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 10] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 11] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 12] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 13] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 14] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 15] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 16] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 17] = (byte)((value % 10) + 48);
        //    return 18;
        //}

        //internal static int ToString10000000000000000000(byte[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (byte)((value / 1000000000000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 1000000000000000000) / 100000000000000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 100000000000000000) / 10000000000000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 1000000000000000) / 100000000000000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 100000000000000) / 10000000000000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    buffer[pos + 8] = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 9] = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 10] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 11] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 12] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 13] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 14] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 15] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 16] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 17] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 18] = (byte)((value % 10) + 48);
        //    return 19;
        //}

        //internal static int ToString100000000000000000000(byte[] buffer, int pos, ulong value)
        //{
        //    buffer[pos] = (byte)((value / 10000000000000000000) + 48);
        //    buffer[pos + 1] = (byte)(((value % 10000000000000000000) / 1000000000000000000) + 48);
        //    buffer[pos + 2] = (byte)(((value % 1000000000000000000) / 100000000000000000) + 48);
        //    buffer[pos + 3] = (byte)(((value % 100000000000000000) / 10000000000000000) + 48);
        //    buffer[pos + 4] = (byte)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    buffer[pos + 5] = (byte)(((value % 1000000000000000) / 100000000000000) + 48);
        //    buffer[pos + 6] = (byte)(((value % 100000000000000) / 10000000000000) + 48);
        //    buffer[pos + 7] = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    buffer[pos + 8] = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    buffer[pos + 9] = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 10] = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 11] = (byte)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 12] = (byte)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 13] = (byte)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 14] = (byte)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 15] = (byte)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 16] = (byte)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 17] = (byte)(((value % 1000) / 100) + 48);
        //    buffer[pos + 18] = (byte)(((value % 100) / 10) + 48);
        //    buffer[pos + 19] = (byte)((value % 10) + 48);
        //    return 20;
        //}

        //#endregion

        //#region 数字转换 ref 指针

        //internal unsafe static byte* ToString10(byte* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos++;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString100(byte* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (byte)((value / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 2;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString1000(byte* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (byte)((value / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 3;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString10000(byte* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (byte)((value / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 4;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString100000(byte* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (byte)((value / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 5;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString1000000(byte* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (byte)((value / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 6;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString10000000(byte* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (byte)((value / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 7;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString100000000(byte* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (byte)((value / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 8;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString1000000000(byte* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (byte)((value / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 9;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString10000000000(byte* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (byte)((value / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 10;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString10000000000(byte* buffer, ref int pos, uint value)
        //{
        //    *buffer++ = (byte)((value / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 10;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString10000000000(byte* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (byte)((value / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 10;
        //    return buffer;
        //}



        //internal unsafe static byte* ToString100000000000(byte* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (byte)((value / 10000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 11;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString1000000000000(byte* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (byte)((value / 100000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 12;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString10000000000000(byte* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (byte)((value / 1000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 13;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString100000000000000(byte* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (byte)((value / 10000000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 14;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString1000000000000000(byte* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (byte)((value / 100000000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000000) / 10000000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 15;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString10000000000000000(byte* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (byte)((value / 1000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000000) / 100000000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000000) / 10000000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 16;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString100000000000000000(byte* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (byte)((value / 10000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000000) / 100000000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000000) / 10000000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 17;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString1000000000000000000(byte* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (byte)((value / 100000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000000000) / 10000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000000) / 100000000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000000) / 10000000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 18;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString10000000000000000000(byte* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (byte)((value / 1000000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000000000) / 100000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000000000) / 10000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000000) / 100000000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000000) / 10000000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 19;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString100000000000000000000(byte* buffer, ref int pos, ulong value)
        //{
        //    *buffer++ = (byte)((value / 10000000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000000000000) / 1000000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000000000) / 100000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000000000) / 10000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000000) / 100000000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000000) / 10000000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (byte)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (byte)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (byte)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (byte)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (byte)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (byte)(((value % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((value % 100) / 10) + 48);
        //    *buffer++ = (byte)((value % 10) + 48);
        //    pos += 20;
        //    return buffer;
        //}

        //#endregion

        //#region 转换

        ////internal unsafe static int ToString(byte* buffer, int pos, int value)
        ////{
        ////    if (value >= 0)
        ////    {
        ////        if (value < 100000)
        ////        {
        ////            if (value < 10)
        ////            {
        ////                return ToString10(buffer, pos, value);
        ////            }
        ////            else if (value < 100)
        ////            {
        ////                return ToString100(buffer, pos, value);
        ////            }
        ////            else if (value < 1000)
        ////            {
        ////                return ToString1000(buffer, pos, value);
        ////            }
        ////            else if (value < 10000)
        ////            {
        ////                return ToString10000(buffer, pos, value);
        ////            }
        ////            else
        ////            {
        ////                return ToString100000(buffer, pos, value);
        ////            }
        ////        }
        ////        else
        ////        {
        ////            if (value < 1000000)
        ////            {
        ////                return ToString1000000(buffer, pos, value);
        ////            }
        ////            else if (value < 10000000)
        ////            {
        ////                return ToString10000000(buffer, pos, value);
        ////            }
        ////            else if (value < 100000000)
        ////            {
        ////                return ToString100000000(buffer, pos, value);
        ////            }
        ////            else if (value < 1000000000)
        ////            {
        ////                return ToString1000000000(buffer, pos, value);
        ////            }
        ////            else
        ////            {
        ////                return ToString10000000000(buffer, pos, value);
        ////            }
        ////        }
        ////        //if (value < 10)
        ////        //{
        ////        //    return ToString10(buffer, pos, value);
        ////        //}
        ////        //else if (value < 100)
        ////        //{
        ////        //    return ToString100(buffer, pos, value);
        ////        //}
        ////        //else if (value < 1000)
        ////        //{
        ////        //    return ToString1000(buffer, pos, value);
        ////        //}
        ////        //else if (value < 10000)
        ////        //{
        ////        //    return ToString10000(buffer, pos, value);
        ////        //}
        ////        //else if (value < 100000)
        ////        //{
        ////        //    return ToString100000(buffer, pos, value);
        ////        //}
        ////        //else if (value < 1000000)
        ////        //{
        ////        //    return ToString1000000(buffer, pos, value);
        ////        //}
        ////        //else if (value < 10000000)
        ////        //{
        ////        //    return ToString10000000(buffer, pos, value);
        ////        //}
        ////        //else if (value < 100000000)
        ////        //{
        ////        //    return ToString100000000(buffer, pos, value);
        ////        //}
        ////        //else if (value < 1000000000)
        ////        //{
        ////        //    return ToString1000000000(buffer, pos, value);
        ////        //}
        ////        //else
        ////        //{
        ////        //    return ToString10000000000(buffer, pos, value);
        ////        //}
        ////    }
        ////    else
        ////    {
        ////        *buffer++ = (byte)'-';
        ////        value = -value;
        ////        return ToString(buffer, pos + 1, value) + 1;
        ////    }
        ////}

        ////internal unsafe static int ToString(byte* buffer, int pos, uint value)
        ////{
        ////    if (value < 10)
        ////    {
        ////        return ToString10(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 100)
        ////    {
        ////        return ToString100(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 1000)
        ////    {
        ////        return ToString1000(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 10000)
        ////    {
        ////        return ToString10000(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 100000)
        ////    {
        ////        return ToString100000(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 1000000)
        ////    {
        ////        return ToString1000000(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 10000000)
        ////    {
        ////        return ToString10000000(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 100000000)
        ////    {
        ////        return ToString100000000(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 1000000000)
        ////    {
        ////        return ToString1000000000(buffer, pos, (int)value);
        ////    }
        ////    else
        ////    {
        ////        return ToString10000000000(buffer, pos, value);
        ////    }
        ////}

        ////internal unsafe static int ToString(byte* buffer, int pos, long value)
        ////{
        ////    if (value >= 0)
        ////    {
        ////        if (value < 10)
        ////        {
        ////            return ToString10(buffer, pos, (int)value);
        ////        }
        ////        else if (value < 100)
        ////        {
        ////            return ToString100(buffer, pos, (int)value);
        ////        }
        ////        else if (value < 1000)
        ////        {
        ////            return ToString1000(buffer, pos, (int)value);
        ////        }
        ////        else if (value < 10000)
        ////        {
        ////            return ToString10000(buffer, pos, (int)value);
        ////        }
        ////        else if (value < 100000)
        ////        {
        ////            return ToString100000(buffer, pos, (int)value);
        ////        }
        ////        else if (value < 1000000)
        ////        {
        ////            return ToString1000000(buffer, pos, (int)value);
        ////        }
        ////        else if (value < 10000000)
        ////        {
        ////            return ToString10000000(buffer, pos, (int)value);
        ////        }
        ////        else if (value < 100000000)
        ////        {
        ////            return ToString100000000(buffer, pos, (int)value);
        ////        }
        ////        else if (value < 1000000000)
        ////        {
        ////            return ToString1000000000(buffer, pos, (int)value);
        ////        }


        ////        else if (value < 10000000000)
        ////        {
        ////            return ToString10000000000(buffer, pos, value);
        ////        }
        ////        else if (value < 100000000000)
        ////        {
        ////            return ToString100000000000(buffer, pos, value);
        ////        }
        ////        else if (value < 1000000000000)
        ////        {
        ////            return ToString1000000000000(buffer, pos, value);
        ////        }

        ////        else if (value < 10000000000000)
        ////        {
        ////            return ToString10000000000000(buffer, pos, value);
        ////        }
        ////        else if (value < 100000000000000)
        ////        {
        ////            return ToString100000000000000(buffer, pos, value);
        ////        }
        ////        else if (value < 1000000000000000)
        ////        {
        ////            return ToString1000000000000000(buffer, pos, value);
        ////        }
        ////        else if (value < 10000000000000000)
        ////        {
        ////            return ToString10000000000000000(buffer, pos, value);
        ////        }
        ////        else if (value < 100000000000000000)
        ////        {
        ////            return ToString100000000000000000(buffer, pos, value);
        ////        }
        ////        else if (value < 1000000000000000000)
        ////        {
        ////            return ToString1000000000000000000(buffer, pos, value);
        ////        }
        ////        else
        ////        {
        ////            return ToString10000000000000000000(buffer, pos, value);
        ////        }
        ////    }
        ////    else
        ////    {
        ////        buffer[pos] = (byte)'-';
        ////        value = -value;
        ////        return ToString(buffer, pos + 1, value) + 1;
        ////    }
        ////}

        ////internal unsafe static int ToString(byte* buffer, int pos, ulong value)
        ////{
        ////    if (value < 10)
        ////    {
        ////        return ToString10(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 100)
        ////    {
        ////        return ToString100(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 1000)
        ////    {
        ////        return ToString1000(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 10000)
        ////    {
        ////        return ToString10000(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 100000)
        ////    {
        ////        return ToString100000(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 1000000)
        ////    {
        ////        return ToString1000000(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 10000000)
        ////    {
        ////        return ToString10000000(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 100000000)
        ////    {
        ////        return ToString100000000(buffer, pos, (int)value);
        ////    }
        ////    else if (value < 1000000000)
        ////    {
        ////        return ToString1000000000(buffer, pos, (int)value);
        ////    }


        ////    else if (value < 10000000000)
        ////    {
        ////        return ToString10000000000(buffer, pos, (long)value);
        ////    }
        ////    else if (value < 100000000000)
        ////    {
        ////        return ToString100000000000(buffer, pos, (long)value);
        ////    }
        ////    else if (value < 1000000000000)
        ////    {
        ////        return ToString1000000000000(buffer, pos, (long)value);
        ////    }

        ////    else if (value < 10000000000000)
        ////    {
        ////        return ToString10000000000000(buffer, pos, (long)value);
        ////    }
        ////    else if (value < 100000000000000)
        ////    {
        ////        return ToString100000000000000(buffer, pos, (long)value);
        ////    }
        ////    else if (value < 1000000000000000)
        ////    {
        ////        return ToString1000000000000000(buffer, pos, (long)value);
        ////    }
        ////    else if (value < 10000000000000000)
        ////    {
        ////        return ToString10000000000000000(buffer, pos, (long)value);
        ////    }
        ////    else if (value < 100000000000000000)
        ////    {
        ////        return ToString100000000000000000(buffer, pos, (long)value);
        ////    }
        ////    else if (value < 1000000000000000000)
        ////    {
        ////        return ToString1000000000000000000(buffer, pos, (long)value);
        ////    }
        ////    else if (value < 10000000000000000000)
        ////    {
        ////        return ToString10000000000000000000(buffer, pos, (long)value);
        ////    }
        ////    else
        ////    {
        ////        return ToString100000000000000000000(buffer, pos, value);
        ////    }
        ////}

        ////internal unsafe static int ToString(byte* buffer, int pos, float value)
        ////{
        ////    if (float.IsNaN(value) || float.IsInfinity(value))
        ////    {
        ////        *buffer++ = (byte)'n';
        ////        *buffer++ = (byte)'u';
        ////        *buffer++ = (byte)'l';
        ////        *buffer = (byte)'l';
        ////        return 4;
        ////    }
        ////    else
        ////    {
        ////        string v = value.ToString();
        ////        for (int i = 0; i < v.Length; i++)
        ////            *buffer++ = (byte)v[i];
        ////        return v.Length;
        ////    }
        ////}

        ////internal unsafe static int ToString(byte* buffer, int pos, double value)
        ////{
        ////    string v = value.ToString();
        ////    for (int i = 0; i < v.Length; i++)
        ////        *buffer++ = (byte)v[i];
        ////    return v.Length;
        ////}

        ////internal unsafe static int ToString(byte* buffer, int pos, decimal value)
        ////{
        ////    string v = value.ToString();
        ////    for (int i = 0; i < v.Length; i++)
        ////        *buffer++ = (byte)v[i];
        ////    return v.Length;
        ////}

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, byte value)
        //{
        //    if (value < 10)
        //        return ToString10(buffer, ref pos, value);
        //    else if (value < 100)
        //        return ToString100(buffer, ref pos, value);
        //    else
        //        return ToString1000(buffer, ref pos, value);
        //}

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, sbyte value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //            return ToString10(buffer, ref pos, value);
        //        else if (value < 100)
        //            return ToString100(buffer, ref pos, value);
        //        else
        //            return ToString1000(buffer, ref pos, value);
        //    }
        //    else
        //    {
        //        if (value == sbyte.MinValue)
        //        {
        //            *buffer++ = (byte)'-';
        //            *buffer++ = (byte)'1';
        //            *buffer++ = (byte)'2';
        //            *buffer++ = (byte)'8';
        //            pos += 4;
        //            return buffer;
        //        }
        //        else
        //        {
        //            *buffer++ = (byte)'-';
        //            value = (sbyte)-value;
        //            pos++;
        //            if (value < 10)
        //                return ToString10(buffer, ref pos, value);
        //            else if (value < 100)
        //                return ToString100(buffer, ref pos, value);
        //            else
        //                return ToString1000(buffer, ref pos, value);
        //        }
        //    }
        //}

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, ushort value)
        //{
        //    if (value < 10)
        //        return ToString10(buffer, ref pos, value);
        //    else if (value < 100)
        //        return ToString100(buffer, ref pos, value);
        //    else if (value < 1000)
        //        return ToString1000(buffer, ref pos, value);
        //    else if (value < 10000)
        //        return ToString10000(buffer, ref pos, value);
        //    else
        //        return ToString100000(buffer, ref pos, value);
        //}

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, short value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //            return ToString10(buffer, ref pos, value);
        //        else if (value < 100)
        //            return ToString100(buffer, ref pos, value);
        //        else if (value < 1000)
        //            return ToString1000(buffer, ref pos, value);
        //        else if (value < 10000)
        //            return ToString10000(buffer, ref pos, value);
        //        else
        //            return ToString100000(buffer, ref pos, value);
        //    }
        //    else
        //    {
        //        if (value == short.MinValue)
        //        {
        //            *buffer++ = (byte)'-';
        //            *buffer++ = (byte)'3';
        //            *buffer++ = (byte)'2';
        //            *buffer++ = (byte)'7';
        //            *buffer++ = (byte)'6';
        //            *buffer++ = (byte)'8';
        //            pos += 6;
        //            return buffer;
        //        }
        //        else
        //        {
        //            *buffer++ = (byte)'-';
        //            pos++;
        //            value = (short)-value;
        //            if (value < 10)
        //                return ToString10(buffer, ref pos, value);
        //            else if (value < 100)
        //                return ToString100(buffer, ref pos, value);
        //            else if (value < 1000)
        //                return ToString1000(buffer, ref pos, value);
        //            else if (value < 10000)
        //                return ToString10000(buffer, ref pos, value);
        //            else
        //                return ToString100000(buffer, ref pos, value);
        //        }
        //    }
        //}

        //internal unsafe static byte* ToString(byte* buffer,ref int pos, int value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 100000)
        //        {
        //            if (value < 10)
        //            {
        //                return ToString10(buffer, ref  pos, value);
        //            }
        //            else if (value < 100)
        //            {
        //                return ToString100(buffer, ref  pos, value);
        //            }
        //            else if (value < 1000)
        //            {
        //                return ToString1000(buffer, ref  pos, value);
        //            }
        //            else if (value < 10000)
        //            {
        //                return ToString10000(buffer, ref  pos, value);
        //            }
        //            else
        //            {
        //                return ToString100000(buffer, ref pos, value);
        //            }
        //        }
        //        else
        //        {
        //            if (value < 1000000)
        //            {
        //                return ToString1000000(buffer, ref  pos, value);
        //            }
        //            else if (value < 10000000)
        //            {
        //                return ToString10000000(buffer, ref  pos, value);
        //            }
        //            else if (value < 100000000)
        //            {
        //                return ToString100000000(buffer, ref  pos, value);
        //            }
        //            else if (value < 1000000000)
        //            {
        //                return ToString1000000000(buffer, ref  pos, value);
        //            }
        //            else
        //            {
        //                return ToString10000000000(buffer, ref  pos, value);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (value == int.MinValue)
        //        {
        //            //-2147483648
        //            *buffer++ = (byte)'-';
        //            *buffer++ = (byte)'2';
        //            *buffer++ = (byte)'1';
        //            *buffer++ = (byte)'4';
        //            *buffer++ = (byte)'7';
        //            *buffer++ = (byte)'4';
        //            *buffer++ = (byte)'8';
        //            *buffer++ = (byte)'3';
        //            *buffer++ = (byte)'6';
        //            *buffer++ = (byte)'4';
        //            *buffer++ = (byte)'8';
        //            pos += 11;
        //            return buffer;
        //        }
        //        else
        //        {
        //            *buffer++ = (byte)'-';
        //            value = -value;
        //            pos++;
        //            return ToString(buffer, ref pos, value);
        //        }
        //    }
        //}

        //internal unsafe static byte* ToString(byte* buffer,ref int pos, uint value)
        //{
        //    if (value < 10)
        //    {
        //        return ToString10(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 100)
        //    {
        //        return ToString100(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 1000)
        //    {
        //        return ToString1000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 10000)
        //    {
        //        return ToString10000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 100000)
        //    {
        //        return ToString100000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 1000000)
        //    {
        //        return ToString1000000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 10000000)
        //    {
        //        return ToString10000000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 100000000)
        //    {
        //        return ToString100000000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 1000000000)
        //    {
        //        return ToString1000000000(buffer, ref  pos, (int)value);
        //    }
        //    else
        //    {
        //        return ToString10000000000(buffer, ref  pos, value);
        //    }
        //}

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, long value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //        {
        //            return ToString10(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 100)
        //        {
        //            return ToString100(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 1000)
        //        {
        //            return ToString1000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 10000)
        //        {
        //            return ToString10000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 100000)
        //        {
        //            return ToString100000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 1000000)
        //        {
        //            return ToString1000000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 10000000)
        //        {
        //            return ToString10000000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 100000000)
        //        {
        //            return ToString100000000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 1000000000)
        //        {
        //            return ToString1000000000(buffer, ref pos, (int)value);
        //        }


        //        else if (value < 10000000000)
        //        {
        //            return ToString10000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 100000000000)
        //        {
        //            return ToString100000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 1000000000000)
        //        {
        //            return ToString1000000000000(buffer, ref pos, value);
        //        }

        //        else if (value < 10000000000000)
        //        {
        //            return ToString10000000000000(buffer, ref  pos, value);
        //        }
        //        else if (value < 100000000000000)
        //        {
        //            return ToString100000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 1000000000000000)
        //        {
        //            return ToString1000000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 10000000000000000)
        //        {
        //            return ToString10000000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 100000000000000000)
        //        {
        //            return ToString100000000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 1000000000000000000)
        //        {
        //            return ToString1000000000000000000(buffer, ref pos, value);
        //        }
        //        else
        //        {
        //            return ToString10000000000000000000(buffer, ref pos, value);
        //        }
        //    }
        //    else
        //    {
        //        if (value == long.MinValue)
        //        {
        //            //-92233 72036 85477 5808
        //            *buffer++ = (byte)'-';
        //            *buffer++ = (byte)'9';
        //            *buffer++ = (byte)'2';
        //            *buffer++ = (byte)'2';
        //            *buffer++ = (byte)'3';
        //            *buffer++ = (byte)'3';
        //            *buffer++ = (byte)'7';
        //            *buffer++ = (byte)'2';
        //            *buffer++ = (byte)'0';
        //            *buffer++ = (byte)'3';
        //            *buffer++ = (byte)'6';
        //            *buffer++ = (byte)'8';
        //            *buffer++ = (byte)'5';
        //            *buffer++ = (byte)'4';
        //            *buffer++ = (byte)'7';
        //            *buffer++ = (byte)'7';
        //            *buffer++ = (byte)'5';
        //            *buffer++ = (byte)'8';
        //            *buffer++ = (byte)'0';
        //            *buffer++ = (byte)'8';
        //            pos += 20;
        //            return buffer;
        //        }
        //        else
        //        {
        //            *buffer++ = (byte)'-';
        //            value = -value;
        //            pos++;
        //            return ToString(buffer, ref  pos, value);
        //        }
        //    }
        //}

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, ulong value)
        //{
        //    if (value < 10)
        //    {
        //        return ToString10(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 100)
        //    {
        //        return ToString100(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 1000)
        //    {
        //        return ToString1000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 10000)
        //    {
        //        return ToString10000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 100000)
        //    {
        //        return ToString100000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 1000000)
        //    {
        //        return ToString1000000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 10000000)
        //    {
        //        return ToString10000000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 100000000)
        //    {
        //        return ToString100000000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 1000000000)
        //    {
        //        return ToString1000000000(buffer, ref  pos, (int)value);
        //    }


        //    else if (value < 10000000000)
        //    {
        //        return ToString10000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 100000000000)
        //    {
        //        return ToString100000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 1000000000000)
        //    {
        //        return ToString1000000000000(buffer, ref  pos, (long)value);
        //    }

        //    else if (value < 10000000000000)
        //    {
        //        return ToString10000000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 100000000000000)
        //    {
        //        return ToString100000000000000(buffer,ref   pos, (long)value);
        //    }
        //    else if (value < 1000000000000000)
        //    {
        //        return ToString1000000000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 10000000000000000)
        //    {
        //        return ToString10000000000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 100000000000000000)
        //    {
        //        return ToString100000000000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 1000000000000000000)
        //    {
        //        return ToString1000000000000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 10000000000000000000)
        //    {
        //        return ToString10000000000000000000(buffer, ref  pos, (long)value);
        //    }
        //    else
        //    {
        //        return ToString100000000000000000000(buffer, ref  pos, value);
        //    }
        //}

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, float value)
        //{
        //    if (float.IsNaN(value) || float.IsInfinity(value))
        //    {
        //        *buffer++ = (byte)'n';
        //        *buffer++ = (byte)'u';
        //        *buffer++ = (byte)'l';
        //        *buffer++ = (byte)'l';
        //        pos += 4;
        //        return buffer;
        //    }
        //    else
        //    {
        //        string v = value.ToString();
        //        for (int i = 0; i < v.Length; i++)
        //            *buffer++ = (byte)v[i];
        //        pos += v.Length;
        //        return buffer;
        //    }
        //}

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, double value)
        //{
        //    if (double.IsNaN(value) || double.IsInfinity(value))
        //    {
        //        *buffer++ = (byte)'n';
        //        *buffer++ = (byte)'u';
        //        *buffer++ = (byte)'l';
        //        *buffer++ = (byte)'l';
        //        pos += 4;
        //        return buffer;
        //    }
        //    else
        //    {
        //        string v = value.ToString();
        //        for (int i = 0; i < v.Length; i++)
        //            *buffer++ = (byte)v[i];
        //        pos += v.Length;
        //        return buffer;
        //    }
        //}

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, decimal value)
        //{
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        *buffer++ = (byte)v[i];
        //    pos += v.Length;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, bool value)
        //{
        //    if (value)
        //    {
        //        *buffer++ = (byte)'t';
        //        *buffer++ = (byte)'r';
        //        *buffer++ = (byte)'u';
        //        *buffer++ = (byte)'e';
        //        *buffer++ = (byte)',';
        //        pos += 5;
        //    }
        //    else
        //    {
        //        *buffer++ = (byte)'f';
        //        *buffer++ = (byte)'a';
        //        *buffer++ = (byte)'l';
        //        *buffer++ = (byte)'s';
        //        *buffer++ = (byte)'e';
        //        *buffer++ = (byte)',';
        //        pos += 6;
        //    }
        //    return buffer;
        //}



        //internal static int ToString(byte[] buffer, int pos, byte value)
        //{
        //    if (value < 10)
        //        return ToString10(buffer, pos, value);
        //    else if (value < 100)
        //        return ToString100(buffer, pos, value);
        //    else
        //        return ToString1000(buffer, pos, value);
        //}

        //internal static int ToString(byte[] buffer, int pos, sbyte value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //            return ToString10(buffer, pos, value);
        //        else if (value < 100)
        //            return ToString100(buffer, pos, value);
        //        else
        //            return ToString1000(buffer, pos, value);
        //    }
        //    else
        //    {
        //        if (value == sbyte.MinValue)
        //        {
        //            buffer[pos] = (byte)'-';
        //            buffer[pos + 1] = (byte)'1';
        //            buffer[pos + 2] = (byte)'2';
        //            buffer[pos + 3] = (byte)'8';
        //            return 4;
        //        }
        //        else
        //        {
        //            buffer[pos] = (byte)'-';
        //            value = (sbyte)-value;
        //            if (value < 10)
        //                return ToString10(buffer, pos + 1, value) + 1;
        //            else if (value < 100)
        //                return ToString100(buffer, pos + 1, value) + 1;
        //            else
        //                return ToString1000(buffer, pos + 1, value) + 1;
        //        }
        //    }
        //}

        //internal static int ToString(byte[] buffer, int pos, ushort value)
        //{
        //    if (value < 10)
        //        return ToString10(buffer, pos, value);
        //    else if (value < 100)
        //        return ToString100(buffer, pos, value);
        //    else if (value < 1000)
        //        return ToString1000(buffer, pos, value);
        //    else if (value < 10000)
        //        return ToString10000(buffer, pos, value);
        //    else
        //        return ToString100000(buffer, pos, value);
        //}

        //internal static int ToString(byte[] buffer, int pos, short value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //            return ToString10(buffer, pos, value);
        //        else if (value < 100)
        //            return ToString100(buffer, pos, value);
        //        else if (value < 1000)
        //            return ToString1000(buffer, pos, value);
        //        else if (value < 10000)
        //            return ToString10000(buffer, pos, value);
        //        else
        //            return ToString100000(buffer, pos, value);
        //    }
        //    else
        //    {
        //        if (value == short.MinValue)
        //        {
        //            buffer[pos] = (byte)'-';
        //            buffer[pos + 1] = (byte)'3';
        //            buffer[pos + 2] = (byte)'2';
        //            buffer[pos + 3] = (byte)'7';
        //            buffer[pos + 3] = (byte)'6';
        //            buffer[pos + 3] = (byte)'8';
        //            return 6;
        //        }
        //        else
        //        {
        //            buffer[pos] = (byte)'-';
        //            value = (short)-value;
        //            if (value < 10)
        //                return ToString10(buffer, pos + 1, value) + 1;
        //            else if (value < 100)
        //                return ToString100(buffer, pos + 1, value) + 1;
        //            else if (value < 1000)
        //                return ToString1000(buffer, pos + 1, value) + 1;
        //            else if (value < 10000)
        //                return ToString10000(buffer, pos + 1, value) + 1;
        //            else
        //                return ToString100000(buffer, pos + 1, value) + 1;
        //        }
        //    }
        //}

        //internal static int ToString(byte[] buffer, int pos, int value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 100000)
        //        {
        //            if (value < 10)
        //            {
        //                return ToString10(buffer, pos, value);
        //            }
        //            else if (value < 100)
        //            {
        //                return ToString100(buffer, pos, value);
        //            }
        //            else if (value < 1000)
        //            {
        //                return ToString1000(buffer, pos, value);
        //            }
        //            else if (value < 10000)
        //            {
        //                return ToString10000(buffer, pos, value);
        //            }
        //            else
        //            {
        //                return ToString100000(buffer, pos, value);
        //            }
        //        }
        //        else
        //        {
        //            if (value < 1000000)
        //            {
        //                return ToString1000000(buffer, pos, value);
        //            }
        //            else if (value < 10000000)
        //            {
        //                return ToString10000000(buffer, pos, value);
        //            }
        //            else if (value < 100000000)
        //            {
        //                return ToString100000000(buffer, pos, value);
        //            }
        //            else if (value < 1000000000)
        //            {
        //                return ToString1000000000(buffer, pos, value);
        //            }
        //            else
        //            {
        //                return ToString10000000000(buffer, pos, value);
        //            }
        //        }


        //        //if (value < 10)
        //        //{
        //        //    return ToString10(buffer, pos, value);
        //        //}
        //        //else if (value < 100)
        //        //{
        //        //    return ToString100(buffer, pos, value);
        //        //}
        //        //else if (value < 1000)
        //        //{
        //        //    return ToString1000(buffer, pos, value);
        //        //}
        //        //else if (value < 10000)
        //        //{
        //        //    return ToString10000(buffer, pos, value);
        //        //}
        //        //else if (value < 100000)
        //        //{
        //        //    return ToString100000(buffer, pos, value);
        //        //}
        //        //else if (value < 1000000)
        //        //{
        //        //    return ToString1000000(buffer, pos, value);
        //        //}
        //        //else if (value < 10000000)
        //        //{
        //        //    return ToString10000000(buffer, pos, value);
        //        //}
        //        //else if (value < 100000000)
        //        //{
        //        //    return ToString100000000(buffer, pos, value);
        //        //}
        //        //else if (value < 1000000000)
        //        //{
        //        //    return ToString1000000000(buffer, pos, value);
        //        //}
        //        //else
        //        //{
        //        //    return ToString10000000000(buffer, pos, value);
        //        //}
        //    }
        //    else
        //    {
        //        buffer[pos] = (byte)'-';
        //        value = -value;
        //        return ToString(buffer, pos + 1, value) + 1;
        //    }
        //}

        //internal static int ToString(byte[] buffer, int pos, uint value)
        //{
        //    if (value < 10)
        //    {
        //        return ToString10(buffer, pos, (int)value);
        //    }
        //    else if (value < 100)
        //    {
        //        return ToString100(buffer, pos, (int)value);
        //    }
        //    else if (value < 1000)
        //    {
        //        return ToString1000(buffer, pos, (int)value);
        //    }
        //    else if (value < 10000)
        //    {
        //        return ToString10000(buffer, pos, (int)value);
        //    }
        //    else if (value < 100000)
        //    {
        //        return ToString100000(buffer, pos, (int)value);
        //    }
        //    else if (value < 1000000)
        //    {
        //        return ToString1000000(buffer, pos, (int)value);
        //    }
        //    else if (value < 10000000)
        //    {
        //        return ToString10000000(buffer, pos, (int)value);
        //    }
        //    else if (value < 100000000)
        //    {
        //        return ToString100000000(buffer, pos, (int)value);
        //    }
        //    else if (value < 1000000000)
        //    {
        //        return ToString1000000000(buffer, pos, (int)value);
        //    }
        //    else
        //    {
        //        return ToString10000000000(buffer, pos, value);
        //    }
        //}

        //internal static int ToString(byte[] buffer, int pos, long value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //        {
        //            return ToString10(buffer, pos, (int)value);
        //        }
        //        else if (value < 100)
        //        {
        //            return ToString100(buffer, pos, (int)value);
        //        }
        //        else if (value < 1000)
        //        {
        //            return ToString1000(buffer, pos, (int)value);
        //        }
        //        else if (value < 10000)
        //        {
        //            return ToString10000(buffer, pos, (int)value);
        //        }
        //        else if (value < 100000)
        //        {
        //            return ToString100000(buffer, pos, (int)value);
        //        }
        //        else if (value < 1000000)
        //        {
        //            return ToString1000000(buffer, pos, (int)value);
        //        }
        //        else if (value < 10000000)
        //        {
        //            return ToString10000000(buffer, pos, (int)value);
        //        }
        //        else if (value < 100000000)
        //        {
        //            return ToString100000000(buffer, pos, (int)value);
        //        }
        //        else if (value < 1000000000)
        //        {
        //            return ToString1000000000(buffer, pos, (int)value);
        //        }


        //        else if (value < 10000000000)
        //        {
        //            return ToString10000000000(buffer, pos, value);
        //        }
        //        else if (value < 100000000000)
        //        {
        //            return ToString100000000000(buffer, pos, value);
        //        }
        //        else if (value < 1000000000000)
        //        {
        //            return ToString1000000000000(buffer, pos, value);
        //        }

        //        else if (value < 10000000000000)
        //        {
        //            return ToString10000000000000(buffer, pos, value);
        //        }
        //        else if (value < 100000000000000)
        //        {
        //            return ToString100000000000000(buffer, pos, value);
        //        }
        //        else if (value < 1000000000000000)
        //        {
        //            return ToString1000000000000000(buffer, pos, value);
        //        }
        //        else if (value < 10000000000000000)
        //        {
        //            return ToString10000000000000000(buffer, pos, value);
        //        }
        //        else if (value < 100000000000000000)
        //        {
        //            return ToString100000000000000000(buffer, pos, value);
        //        }
        //        else if (value < 1000000000000000000)
        //        {
        //            return ToString1000000000000000000(buffer, pos, value);
        //        }
        //        else
        //        {
        //            return ToString10000000000000000000(buffer, pos, value);
        //        }
        //    }
        //    else
        //    {
        //        if (value == long.MinValue)
        //        {
        //            //-92233 72036 85477 5808
        //            buffer[pos] = (byte)'-';
        //            buffer[pos + 1] = (byte)'9';
        //            buffer[pos + 2] = (byte)'2';
        //            buffer[pos + 3] = (byte)'2';
        //            buffer[pos + 4] = (byte)'3';
        //            buffer[pos + 5] = (byte)'3';
        //            buffer[pos + 6] = (byte)'7';
        //            buffer[pos + 7] = (byte)'2';
        //            buffer[pos + 8] = (byte)'0';
        //            buffer[pos + 9] = (byte)'3';
        //            buffer[pos + 10] = (byte)'6';
        //            buffer[pos + 11] = (byte)'8';
        //            buffer[pos + 12] = (byte)'5';
        //            buffer[pos + 13] = (byte)'4';
        //            buffer[pos + 14] = (byte)'7';
        //            buffer[pos + 15] = (byte)'7';
        //            buffer[pos + 16] = (byte)'5';
        //            buffer[pos + 17] = (byte)'8';
        //            buffer[pos + 18] = (byte)'0';
        //            buffer[pos + 19] = (byte)'8';
        //            return 20;
        //        }
        //        else
        //        {
        //            buffer[pos] = (byte)'-';
        //            value = -value;
        //            return ToString(buffer, pos + 1, value) + 1;
        //        }
        //    }
        //}

        //internal static int ToString(byte[] buffer, int pos, ulong value)
        //{
        //    if (value < 10)
        //    {
        //        return ToString10(buffer, pos, (int)value);
        //    }
        //    else if (value < 100)
        //    {
        //        return ToString100(buffer, pos, (int)value);
        //    }
        //    else if (value < 1000)
        //    {
        //        return ToString1000(buffer, pos, (int)value);
        //    }
        //    else if (value < 10000)
        //    {
        //        return ToString10000(buffer, pos, (int)value);
        //    }
        //    else if (value < 100000)
        //    {
        //        return ToString100000(buffer, pos, (int)value);
        //    }
        //    else if (value < 1000000)
        //    {
        //        return ToString1000000(buffer, pos, (int)value);
        //    }
        //    else if (value < 10000000)
        //    {
        //        return ToString10000000(buffer, pos, (int)value);
        //    }
        //    else if (value < 100000000)
        //    {
        //        return ToString100000000(buffer, pos, (int)value);
        //    }
        //    else if (value < 1000000000)
        //    {
        //        return ToString1000000000(buffer, pos, (int)value);
        //    }


        //    else if (value < 10000000000)
        //    {
        //        return ToString10000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 100000000000)
        //    {
        //        return ToString100000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 1000000000000)
        //    {
        //        return ToString1000000000000(buffer, pos, (long)value);
        //    }

        //    else if (value < 10000000000000)
        //    {
        //        return ToString10000000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 100000000000000)
        //    {
        //        return ToString100000000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 1000000000000000)
        //    {
        //        return ToString1000000000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 10000000000000000)
        //    {
        //        return ToString10000000000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 100000000000000000)
        //    {
        //        return ToString100000000000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 1000000000000000000)
        //    {
        //        return ToString1000000000000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 10000000000000000000)
        //    {
        //        return ToString10000000000000000000(buffer, pos, (long)value);
        //    }
        //    else
        //    {
        //        return ToString100000000000000000000(buffer, pos, value);
        //    }
        //}

        //internal static int ToString(byte[] buffer, int pos, float value)
        //{
        //    if (float.IsNaN(value) || float.IsInfinity(value))
        //    {
        //        buffer[pos] = (byte)'n';
        //        buffer[pos + 1] = (byte)'u';
        //        buffer[pos + 2] = (byte)'l';
        //        buffer[pos + 3] = (byte)'l';
        //        return 4;
        //    }
        //    else
        //    {
        //        string v = value.ToString();
        //        for (int i = 0; i < v.Length; i++)
        //            buffer[pos + i] = (byte)v[i];
        //        return v.Length;
        //    }
        //}

        //internal static int ToString(byte[] buffer, int pos, double value)
        //{
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        buffer[pos + i] = (byte)v[i];
        //    return v.Length;
        //}

        //internal static int ToString(byte[] buffer, int pos, decimal value)
        //{
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        buffer[pos + i] = (byte)v[i];
        //    return v.Length;
        //}

        //internal static int ToString(byte[] buffer, int pos, bool value)
        //{
        //    if (value)
        //    {
        //        buffer[pos] = (byte)'t';
        //        buffer[pos + 1] = (byte)'r';
        //        buffer[pos + 2] = (byte)'u';
        //        buffer[pos + 3] = (byte)'e';
        //        return 4;
        //    }
        //    else
        //    {
        //        buffer[pos] = (byte)'f';
        //        buffer[pos + 1] = (byte)'a';
        //        buffer[pos + 2] = (byte)'l';
        //        buffer[pos + 3] = (byte)'s';
        //        buffer[pos + 4] = (byte)'e';
        //        return 5;
        //    }
        //}

        //#endregion

        //#region 时间

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, DateTime value)
        //{
        //    //string s = value.ToString("o");
        //    //2013-07-09T17:13:22.5532438+08:00
        //    //TimeZoneInfo.Local.BaseUtcOffset
        //    int v = value.Year;
        //    *buffer++ = (byte)((v / 1000) + 48);
        //    *buffer++ = (byte)(((v % 1000) / 100) + 48);
        //    *buffer++ = (byte)(((v % 100) / 10) + 48);
        //    *buffer++ = (byte)((v % 10) + 48);

        //    v = value.Month;
        //    *buffer++ = (byte)('-');
        //    *buffer++ = (byte)(((v % 100) / 10) + 48);
        //    *buffer++ = (byte)((v % 10) + 48);

        //    v = value.Day;
        //    *buffer++ = (byte)('-');
        //    *buffer++ = (byte)(((v % 100) / 10) + 48);
        //    *buffer++ = (byte)((v % 10) + 48);

        //    v = value.Hour;
        //    *buffer++ = (byte)('T');
        //    *buffer++ = (byte)(((v % 100) / 10) + 48);
        //    *buffer++ = (byte)((v % 10) + 48);

        //    v = value.Minute;
        //    *buffer++ = (byte)(':');
        //    *buffer++ = (byte)(((v % 100) / 10) + 48);
        //    *buffer++ = (byte)((v % 10) + 48);

        //    v = value.Second;
        //    *buffer++ = (byte)(':');
        //    *buffer++ = (byte)(((v % 100) / 10) + 48);
        //    *buffer++ = (byte)((v % 10) + 48);

        //    pos += 19;
        //    return buffer;

        //    //v = value.Millisecond;
        //    //*buffer++ = (byte)('.');
        //    //*buffer++ = (byte)((v / 100) + 48);
        //    //*buffer++ = (byte)(((v % 100) / 10) + 48);
        //    //*buffer++ = (byte)((v % 10) + 48);

        //    //*buffer++ = (byte)('0');
        //    //*buffer++ = (byte)('0');
        //    //*buffer++ = (byte)('0');
        //    //*buffer++ = (byte)('0');

        //    //TimeSpan span = TimeZoneInfo.Local.BaseUtcOffset;
        //    //v = span.Hours;
        //    //*buffer++ = (byte)('+');
        //    //*buffer++ = (byte)(((v % 100) / 10) + 48);
        //    //*buffer++ = (byte)((v % 10) + 48);

        //    //v = span.Minutes;
        //    //*buffer++ = (byte)(':');
        //    //*buffer++ = (byte)(((v % 100) / 10) + 48);
        //    //*buffer++ = (byte)((v % 10) + 48);

        //    //pos += 33;
        //    //return buffer;
        //}

        //internal unsafe static byte* ToString(byte* buffer,ref int pos, TimeSpan value)
        //{
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        *buffer++ = (byte)v[i];
        //    pos += v.Length;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString(byte* buffer,ref int pos, DateTimeOffset value)
        //{
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        *buffer++ = (byte)v[i];
        //    pos += v.Length;
        //    return buffer;
        //}

        ///// <summary>
        ///// ISO 8601日期格式，去掉异常判断，提升性能
        ///// 时间转换暂时不能获取到微秒部分的数据
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <param name="pos"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //internal static int ToString(byte[] buffer, int pos, DateTime value)
        //{
        //    //string s = value.ToString("o");
        //    //2013-07-09T17:13:22.5532438+08:00
        //    //TimeZoneInfo.Local.BaseUtcOffset
        //    int v = value.Year;
        //    buffer[pos] = (byte)((v / 1000) + 48);
        //    buffer[pos + 1] = (byte)(((v % 1000) / 100) + 48);
        //    buffer[pos + 2] = (byte)(((v % 100) / 10) + 48);
        //    buffer[pos + 3] = (byte)((v % 10) + 48);

        //    v = value.Month;
        //    buffer[pos + 4] = (byte)('-');
        //    buffer[pos + 5] = (byte)(((v % 100) / 10) + 48);
        //    buffer[pos + 6] = (byte)((v % 10) + 48);

        //    v = value.Day;
        //    buffer[pos + 7] = (byte)('-');
        //    buffer[pos + 8] = (byte)(((v % 100) / 10) + 48);
        //    buffer[pos + 9] = (byte)((v % 10) + 48);

        //    v = value.Hour;
        //    buffer[pos + 10] = (byte)('T');
        //    buffer[pos + 11] = (byte)(((v % 100) / 10) + 48);
        //    buffer[pos + 12] = (byte)((v % 10) + 48);

        //    v = value.Minute;
        //    buffer[pos + 13] = (byte)(':');
        //    buffer[pos + 14] = (byte)(((v % 100) / 10) + 48);
        //    buffer[pos + 15] = (byte)((v % 10) + 48);

        //    v = value.Second;
        //    buffer[pos + 16] = (byte)(':');
        //    buffer[pos + 17] = (byte)(((v % 100) / 10) + 48);
        //    buffer[pos + 18] = (byte)((v % 10) + 48);

        //    return 19;

        //    //v = value.Millisecond;
        //    //buffer[pos + 19] = (byte)('.');
        //    //buffer[pos + 20] = (byte)((v / 100) + 48);
        //    //buffer[pos + 21] = (byte)(((v % 100) / 10) + 48);
        //    //buffer[pos + 22] = (byte)((v % 10) + 48);

        //    //buffer[pos + 23] = (byte)('0');
        //    //buffer[pos + 24] = (byte)('0');
        //    //buffer[pos + 25] = (byte)('0');
        //    //buffer[pos + 26] = (byte)('0');

        //    //TimeSpan span = TimeZoneInfo.Local.BaseUtcOffset;
        //    //v = span.Hours;
        //    //buffer[pos + 27] = (byte)('+');
        //    //buffer[pos + 28] = (byte)(((v % 100) / 10) + 48);
        //    //buffer[pos + 29] = (byte)((v % 10) + 48);

        //    //v = span.Minutes;
        //    //buffer[pos + 30] = (byte)(':');
        //    //buffer[pos + 31] = (byte)(((v % 100) / 10) + 48);
        //    //buffer[pos + 32] = (byte)((v % 10) + 48);

        //    //return 33;
        //}

        //internal static int ToString(byte[] buffer, int pos, TimeSpan value)
        //{
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        buffer[pos + i] = (byte)v[i];
        //    return v.Length;
        //}

        //internal static int ToString(byte[] buffer, int pos, DateTimeOffset value)
        //{
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        buffer[pos + i] = (byte)v[i];
        //    return v.Length;
        //}

        //#endregion

        //#region 其它基础类型

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, Guid value)
        //{
        //    //Guid sp = new Guid("337c7f2b-7a34-4f50-9141-bab9e6478cc8");
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        *buffer++ = (byte)v[i];
        //    pos += v.Length;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, Uri value)
        //{
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        *buffer++ = (byte)v[i];
        //    pos += v.Length;
        //    return buffer;
        //}

        //internal unsafe static byte* ToString(byte* buffer, ref int pos, Enum value)
        //{
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        *buffer++ = (byte)v[i];
        //    pos += v.Length;
        //    return buffer;
        //}



        //internal static int ToString(byte[] buffer, int pos, Guid value)
        //{
        //    //Guid sp = new Guid("337c7f2b-7a34-4f50-9141-bab9e6478cc8");
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        buffer[pos + i] = (byte)v[i];
        //    return v.Length;
        //}

        //internal static int ToString(byte[] buffer, int pos, Uri value)
        //{
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        buffer[pos + i] = (byte)v[i];
        //    return v.Length;
        //}

        //internal static int ToString(byte[] buffer, int pos, Enum value)
        //{
        //    string v = value.ToString();
        //    for (int i = 0; i < v.Length; i++)
        //        buffer[pos + i] = (byte)v[i];
        //    return v.Length;
        //}

        //internal unsafe static int ToString(byte[] buffer, int pos, byte[] value)
        //{
        //    //Convert.ToBase64String(value, 0, value.Length, Base64FormattingOptions.None);
        //    fixed (byte* psrc = value, pdst = &buffer[pos])
        //    {
        //        Utils.ConvertToBase64Array(pdst, psrc, 0, value.Length, false);
        //    }
        //    return (value.Length << 1);
        //}

        //#endregion

        

        




















        #region 数字转换（字符）

        //internal static int ToString10(char[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (char)((value % 10) + 48);
        //    return 1;
        //}

        //internal static int ToString100(char[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (char)((value / 10) + 48);
        //    buffer[pos + 1] = (char)((value % 10) + 48);
        //    return 2;
        //}

        //internal static int ToString1000(char[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (char)((value / 100) + 48);
        //    buffer[pos + 1] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 2] = (char)((value % 10) + 48);
        //    return 3;
        //}

        //internal static int ToString10000(char[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (char)((value / 1000) + 48);
        //    buffer[pos + 1] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 2] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 3] = (char)((value % 10) + 48);
        //    return 4;
        //}

        //internal static int ToString100000(char[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (char)((value / 10000) + 48);
        //    buffer[pos + 1] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 2] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 3] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 4] = (char)((value % 10) + 48);
        //    return 5;
        //}

        //internal static int ToString1000000(char[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (char)((value / 100000) + 48);
        //    buffer[pos + 1] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 2] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 3] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 4] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 5] = (char)((value % 10) + 48);
        //    return 6;
        //}

        //internal static int ToString10000000(char[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (char)((value / 1000000) + 48);
        //    buffer[pos + 1] = (char)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 2] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 3] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 4] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 5] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 6] = (char)((value % 10) + 48);
        //    return 7;
        //}

        //internal static int ToString100000000(char[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (char)((value / 10000000) + 48);
        //    buffer[pos + 1] = (char)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 2] = (char)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 3] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 4] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 5] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 6] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 7] = (char)((value % 10) + 48);
        //    return 8;
        //}

        //internal static int ToString1000000000(char[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (char)((value / 100000000) + 48);
        //    buffer[pos + 1] = (char)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 2] = (char)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 3] = (char)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 4] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 5] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 6] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 7] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 8] = (char)((value % 10) + 48);
        //    return 9;
        //}

        //internal static int ToString10000000000(char[] buffer, int pos, int value)
        //{
        //    buffer[pos] = (char)((value / 1000000000) + 48);
        //    buffer[pos + 1] = (char)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 2] = (char)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 3] = (char)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 4] = (char)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 5] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 6] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 7] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 8] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 9] = (char)((value % 10) + 48);
        //    return 10;
        //}

        //internal static int ToString10000000000(char[] buffer, int pos, uint value)
        //{
        //    buffer[pos] = (char)((value / 1000000000) + 48);
        //    buffer[pos + 1] = (char)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 2] = (char)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 3] = (char)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 4] = (char)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 5] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 6] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 7] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 8] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 9] = (char)((value % 10) + 48);
        //    return 10;
        //}

        //internal static int ToString10000000000(char[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (char)((value / 1000000000) + 48);
        //    buffer[pos + 1] = (char)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 2] = (char)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 3] = (char)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 4] = (char)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 5] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 6] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 7] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 8] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 9] = (char)((value % 10) + 48);
        //    return 10;
        //}



        //internal static int ToString100000000000(char[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (char)((value / 10000000000) + 48);
        //    buffer[pos + 1] = (char)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 2] = (char)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 3] = (char)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 4] = (char)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 5] = (char)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 6] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 7] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 8] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 9] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 10] = (char)((value % 10) + 48);
        //    return 11;
        //}

        //internal static int ToString1000000000000(char[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (char)((value / 100000000000) + 48);
        //    buffer[pos + 1] = (char)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 2] = (char)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 3] = (char)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 4] = (char)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 5] = (char)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 6] = (char)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 7] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 8] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 9] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 10] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 11] = (char)((value % 10) + 48);
        //    return 12;
        //}

        //internal static int ToString10000000000000(char[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (char)((value / 1000000000000) + 48);
        //    buffer[pos + 1] = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    buffer[pos + 2] = (char)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 3] = (char)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 4] = (char)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 5] = (char)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 6] = (char)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 7] = (char)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 8] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 9] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 10] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 11] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 12] = (char)((value % 10) + 48);
        //    return 13;
        //}

        //internal static int ToString100000000000000(char[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (char)((value / 10000000000000) + 48);
        //    buffer[pos + 1] = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    buffer[pos + 2] = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    buffer[pos + 3] = (char)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 4] = (char)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 5] = (char)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 6] = (char)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 7] = (char)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 8] = (char)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 9] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 10] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 11] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 12] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 13] = (char)((value % 10) + 48);
        //    return 14;
        //}

        //internal static int ToString1000000000000000(char[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (char)((value / 100000000000000) + 48);
        //    buffer[pos + 1] = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //    buffer[pos + 2] = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    buffer[pos + 3] = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    buffer[pos + 4] = (char)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 5] = (char)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 6] = (char)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 7] = (char)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 8] = (char)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 9] = (char)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 10] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 11] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 12] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 13] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 14] = (char)((value % 10) + 48);
        //    return 15;
        //}

        //internal static int ToString10000000000000000(char[] buffer, int pos, long value)
        //{
        //    buffer[pos] = (char)((value / 1000000000000000) + 48);
        //    buffer[pos + 1] = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //    buffer[pos + 2] = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //    buffer[pos + 3] = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    buffer[pos + 4] = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    buffer[pos + 5] = (char)(((value % 100000000000) / 10000000000) + 48);
        //    buffer[pos + 6] = (char)(((value % 10000000000) / 1000000000) + 48);
        //    buffer[pos + 7] = (char)(((value % 1000000000) / 100000000) + 48);
        //    buffer[pos + 8] = (char)(((value % 100000000) / 10000000) + 48);
        //    buffer[pos + 9] = (char)(((value % 10000000) / 1000000) + 48);
        //    buffer[pos + 10] = (char)(((value % 1000000) / 100000) + 48);
        //    buffer[pos + 11] = (char)(((value % 100000) / 10000) + 48);
        //    buffer[pos + 12] = (char)(((value % 10000) / 1000) + 48);
        //    buffer[pos + 13] = (char)(((value % 1000) / 100) + 48);
        //    buffer[pos + 14] = (char)(((value % 100) / 10) + 48);
        //    buffer[pos + 15] = (char)((value % 10) + 48);
        //    return 16;
        //}

        //internal unsafe static int ToString100000000000000000(char[] buffer, int pos, long value)
        //{
        //    //buffer[pos] = (char)((value / 10000000000000000) + 48);
        //    //buffer[pos + 1] = (char)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    //buffer[pos + 2] = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //    //buffer[pos + 3] = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //    //buffer[pos + 4] = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    //buffer[pos + 5] = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    //buffer[pos + 6] = (char)(((value % 100000000000) / 10000000000) + 48);
        //    //buffer[pos + 7] = (char)(((value % 10000000000) / 1000000000) + 48);
        //    //buffer[pos + 8] = (char)(((value % 1000000000) / 100000000) + 48);
        //    //buffer[pos + 9] = (char)(((value % 100000000) / 10000000) + 48);
        //    //buffer[pos + 10] = (char)(((value % 10000000) / 1000000) + 48);
        //    //buffer[pos + 11] = (char)(((value % 1000000) / 100000) + 48);
        //    //buffer[pos + 12] = (char)(((value % 100000) / 10000) + 48);
        //    //buffer[pos + 13] = (char)(((value % 10000) / 1000) + 48);
        //    //buffer[pos + 14] = (char)(((value % 1000) / 100) + 48);
        //    //buffer[pos + 15] = (char)(((value % 100) / 10) + 48);
        //    //buffer[pos + 16] = (char)((value % 10) + 48);
        //    //return 17;

        //    fixed (char* pd = &buffer[pos])
        //    {
        //        char* tpd = pd;
        //        *tpd++ = (char)((value / 10000000000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000000000) / 1000000000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //        *tpd++ = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000000) / 100000000000) + 48);
        //        *tpd++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000) / 100000000) + 48);
        //        *tpd++ = (char)(((value % 100000000) / 10000000) + 48);
        //        *tpd++ = (char)(((value % 10000000) / 1000000) + 48);
        //        *tpd++ = (char)(((value % 1000000) / 100000) + 48);
        //        *tpd++ = (char)(((value % 100000) / 10000) + 48);
        //        *tpd++ = (char)(((value % 10000) / 1000) + 48);
        //        *tpd++ = (char)(((value % 1000) / 100) + 48);
        //        *tpd++ = (char)(((value % 100) / 10) + 48);
        //        *tpd++ = (char)((value % 10) + 48);
        //    }
        //    return 17;
        //}

        //internal unsafe static int ToString1000000000000000000(char[] buffer, int pos, long value)
        //{
        //    //buffer[pos] = (char)((value / 100000000000000000) + 48);
        //    //buffer[pos + 1] = (char)(((value % 100000000000000000) / 10000000000000000) + 48);
        //    //buffer[pos + 2] = (char)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    //buffer[pos + 3] = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //    //buffer[pos + 4] = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //    //buffer[pos + 5] = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    //buffer[pos + 6] = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    //buffer[pos + 7] = (char)(((value % 100000000000) / 10000000000) + 48);
        //    //buffer[pos + 8] = (char)(((value % 10000000000) / 1000000000) + 48);
        //    //buffer[pos + 9] = (char)(((value % 1000000000) / 100000000) + 48);
        //    //buffer[pos + 10] = (char)(((value % 100000000) / 10000000) + 48);
        //    //buffer[pos + 11] = (char)(((value % 10000000) / 1000000) + 48);
        //    //buffer[pos + 12] = (char)(((value % 1000000) / 100000) + 48);
        //    //buffer[pos + 13] = (char)(((value % 100000) / 10000) + 48);
        //    //buffer[pos + 14] = (char)(((value % 10000) / 1000) + 48);
        //    //buffer[pos + 15] = (char)(((value % 1000) / 100) + 48);
        //    //buffer[pos + 16] = (char)(((value % 100) / 10) + 48);
        //    //buffer[pos + 17] = (char)((value % 10) + 48);
        //    //return 18;

        //    fixed (char* pd = &buffer[pos])
        //    {
        //        char* tpd = pd;
        //        *tpd++ = (char)((value / 100000000000000000) + 48);
        //        *tpd++ = (char)(((value % 100000000000000000) / 10000000000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000000000) / 1000000000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //        *tpd++ = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000000) / 100000000000) + 48);
        //        *tpd++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000) / 100000000) + 48);
        //        *tpd++ = (char)(((value % 100000000) / 10000000) + 48);
        //        *tpd++ = (char)(((value % 10000000) / 1000000) + 48);
        //        *tpd++ = (char)(((value % 1000000) / 100000) + 48);
        //        *tpd++ = (char)(((value % 100000) / 10000) + 48);
        //        *tpd++ = (char)(((value % 10000) / 1000) + 48);
        //        *tpd++ = (char)(((value % 1000) / 100) + 48);
        //        *tpd++ = (char)(((value % 100) / 10) + 48);
        //        *tpd++ = (char)((value % 10) + 48);
        //    }
        //    return 18;
        //}

        //internal unsafe static int ToString10000000000000000000(char[] buffer, int pos, long value)
        //{
        //    //buffer[pos] = (char)((value / 1000000000000000000) + 48);
        //    //buffer[pos + 1] = (char)(((value % 1000000000000000000) / 100000000000000000) + 48);
        //    //buffer[pos + 2] = (char)(((value % 100000000000000000) / 10000000000000000) + 48);
        //    //buffer[pos + 3] = (char)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    //buffer[pos + 4] = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //    //buffer[pos + 5] = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //    //buffer[pos + 6] = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    //buffer[pos + 7] = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    //buffer[pos + 8] = (char)(((value % 100000000000) / 10000000000) + 48);
        //    //buffer[pos + 9] = (char)(((value % 10000000000) / 1000000000) + 48);
        //    //buffer[pos + 10] = (char)(((value % 1000000000) / 100000000) + 48);
        //    //buffer[pos + 11] = (char)(((value % 100000000) / 10000000) + 48);
        //    //buffer[pos + 12] = (char)(((value % 10000000) / 1000000) + 48);
        //    //buffer[pos + 13] = (char)(((value % 1000000) / 100000) + 48);
        //    //buffer[pos + 14] = (char)(((value % 100000) / 10000) + 48);
        //    //buffer[pos + 15] = (char)(((value % 10000) / 1000) + 48);
        //    //buffer[pos + 16] = (char)(((value % 1000) / 100) + 48);
        //    //buffer[pos + 17] = (char)(((value % 100) / 10) + 48);
        //    //buffer[pos + 18] = (char)((value % 10) + 48);
        //    //return 19;

        //    fixed (char* pd = &buffer[pos])
        //    {
        //        char* tpd = pd;

        //        //long v = value;
        //        //*tpd++ = (char)((v / 1000000000000000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 1000000000000000000)) / 100000000000000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 100000000000000000)) / 10000000000000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 10000000000000000)) / 1000000000000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 1000000000000000)) / 100000000000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 100000000000000)) / 10000000000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 10000000000000)) / 1000000000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 1000000000000)) / 100000000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 100000000000)) / 10000000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 10000000000)) / 1000000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 1000000000)) / 100000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 100000000)) / 10000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 10000000)) / 1000000) + 48);
        //        //*tpd++ = (char)(((v = (v % 1000000)) / 100000) + 48);
        //        //*tpd++ = (char)(((v = (v % 100000)) / 10000) + 48);
        //        //*tpd++ = (char)(((v = (v % 10000)) / 1000) + 48);
        //        //*tpd++ = (char)(((v = (v % 1000)) / 100) + 48);
        //        //*tpd++ = (char)(((v = (v % 100)) / 10) + 48);
        //        //*tpd++ = (char)((v = (v % 10)) + 48);

        //        *tpd++ = (char)((value / 1000000000000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000000000000) / 100000000000000000) + 48);
        //        *tpd++ = (char)(((value % 100000000000000000) / 10000000000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000000000) / 1000000000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //        *tpd++ = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000000) / 100000000000) + 48);
        //        *tpd++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000) / 100000000) + 48);
        //        *tpd++ = (char)(((value % 100000000) / 10000000) + 48);
        //        *tpd++ = (char)(((value % 10000000) / 1000000) + 48);
        //        *tpd++ = (char)(((value % 1000000) / 100000) + 48);
        //        *tpd++ = (char)(((value % 100000) / 10000) + 48);
        //        *tpd++ = (char)(((value % 10000) / 1000) + 48);
        //        *tpd++ = (char)(((value % 1000) / 100) + 48);
        //        *tpd++ = (char)(((value % 100) / 10) + 48);
        //        *tpd++ = (char)((value % 10) + 48);
        //    }
        //    return 19;
        //}

        //internal unsafe static int ToString100000000000000000000(char[] buffer, int pos, ulong value)
        //{
        //    //buffer[pos] = (char)((value / 10000000000000000000) + 48);
        //    //buffer[pos + 1] = (char)(((value % 10000000000000000000) / 1000000000000000000) + 48);
        //    //buffer[pos + 2] = (char)(((value % 1000000000000000000) / 100000000000000000) + 48);
        //    //buffer[pos + 3] = (char)(((value % 100000000000000000) / 10000000000000000) + 48);
        //    //buffer[pos + 4] = (char)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    //buffer[pos + 5] = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //    //buffer[pos + 6] = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //    //buffer[pos + 7] = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    //buffer[pos + 8] = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    //buffer[pos + 9] = (char)(((value % 100000000000) / 10000000000) + 48);
        //    //buffer[pos + 10] = (char)(((value % 10000000000) / 1000000000) + 48);
        //    //buffer[pos + 11] = (char)(((value % 1000000000) / 100000000) + 48);
        //    //buffer[pos + 12] = (char)(((value % 100000000) / 10000000) + 48);
        //    //buffer[pos + 13] = (char)(((value % 10000000) / 1000000) + 48);
        //    //buffer[pos + 14] = (char)(((value % 1000000) / 100000) + 48);
        //    //buffer[pos + 15] = (char)(((value % 100000) / 10000) + 48);
        //    //buffer[pos + 16] = (char)(((value % 10000) / 1000) + 48);
        //    //buffer[pos + 17] = (char)(((value % 1000) / 100) + 48);
        //    //buffer[pos + 18] = (char)(((value % 100) / 10) + 48);
        //    //buffer[pos + 19] = (char)((value % 10) + 48);
        //    //return 20;

        //    fixed (char* pd = &buffer[pos])
        //    {
        //        char* tpd = pd;
        //        *tpd++ = (char)((value / 10000000000000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000000000000) / 1000000000000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000000000000) / 100000000000000000) + 48);
        //        *tpd++ = (char)(((value % 100000000000000000) / 10000000000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000000000) / 1000000000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //        *tpd++ = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000000) / 100000000000) + 48);
        //        *tpd++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //        *tpd++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //        *tpd++ = (char)(((value % 1000000000) / 100000000) + 48);
        //        *tpd++ = (char)(((value % 100000000) / 10000000) + 48);
        //        *tpd++ = (char)(((value % 10000000) / 1000000) + 48);
        //        *tpd++ = (char)(((value % 1000000) / 100000) + 48);
        //        *tpd++ = (char)(((value % 100000) / 10000) + 48);
        //        *tpd++ = (char)(((value % 10000) / 1000) + 48);
        //        *tpd++ = (char)(((value % 1000) / 100) + 48);
        //        *tpd++ = (char)(((value % 100) / 10) + 48);
        //        *tpd++ = (char)((value % 10) + 48);
        //    }
        //    return 20;
        //}

        #endregion

        #region 数字转换 ref 指针

        //internal unsafe static char* ToString10(char* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos++;
        //    return buffer;
        //}

        //internal unsafe static char* ToString100(char* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (char)((value / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 2;
        //    return buffer;
        //}

        //internal unsafe static char* ToString1000(char* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (char)((value / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 3;
        //    return buffer;
        //}

        //internal unsafe static char* ToString10000(char* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (char)((value / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 4;
        //    return buffer;
        //}

        //internal unsafe static char* ToString100000(char* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (char)((value / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 5;
        //    return buffer;
        //}

        //internal unsafe static char* ToString1000000(char* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (char)((value / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 6;
        //    return buffer;
        //}

        //internal unsafe static char* ToString10000000(char* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (char)((value / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 7;
        //    return buffer;
        //}

        //internal unsafe static char* ToString100000000(char* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (char)((value / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 8;
        //    return buffer;
        //}

        //internal unsafe static char* ToString1000000000(char* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (char)((value / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 9;
        //    return buffer;
        //}

        //internal unsafe static char* ToString10000000000(char* buffer, ref int pos, int value)
        //{
        //    *buffer++ = (char)((value / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 10;
        //    return buffer;
        //}

        //internal unsafe static char* ToString10000000000(char* buffer, ref int pos, uint value)
        //{
        //    *buffer++ = (char)((value / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 10;
        //    return buffer;
        //}

        //internal unsafe static char* ToString10000000000(char* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (char)((value / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 10;
        //    return buffer;
        //}



        //internal unsafe static char* ToString100000000000(char* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (char)((value / 10000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 11;
        //    return buffer;
        //}

        //internal unsafe static char* ToString1000000000000(char* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (char)((value / 100000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 12;
        //    return buffer;
        //}

        //internal unsafe static char* ToString10000000000000(char* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (char)((value / 1000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 13;
        //    return buffer;
        //}

        //internal unsafe static char* ToString100000000000000(char* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (char)((value / 10000000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 14;
        //    return buffer;
        //}

        //internal unsafe static char* ToString1000000000000000(char* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (char)((value / 100000000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 15;
        //    return buffer;
        //}

        //internal unsafe static char* ToString10000000000000000(char* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (char)((value / 1000000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 16;
        //    return buffer;
        //}

        //internal unsafe static char* ToString100000000000000000(char* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (char)((value / 10000000000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 17;
        //    return buffer;
        //}

        //internal unsafe static char* ToString1000000000000000000(char* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (char)((value / 100000000000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000000000) / 10000000000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 18;
        //    return buffer;
        //}

        //internal unsafe static char* ToString10000000000000000000(char* buffer, ref int pos, long value)
        //{
        //    *buffer++ = (char)((value / 1000000000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000000000) / 100000000000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000000000) / 10000000000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 19;
        //    return buffer;
        //}

        //internal unsafe static char* ToString100000000000000000000(char* buffer, ref int pos, ulong value)
        //{
        //    *buffer++ = (char)((value / 10000000000000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000000000000) / 1000000000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000000000) / 100000000000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000000000) / 10000000000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000000000) / 1000000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000000) / 100000000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000000) / 10000000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000000) / 1000000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000000) / 100000000000) + 48);
        //    *buffer++ = (char)(((value % 100000000000) / 10000000000) + 48);
        //    *buffer++ = (char)(((value % 10000000000) / 1000000000) + 48);
        //    *buffer++ = (char)(((value % 1000000000) / 100000000) + 48);
        //    *buffer++ = (char)(((value % 100000000) / 10000000) + 48);
        //    *buffer++ = (char)(((value % 10000000) / 1000000) + 48);
        //    *buffer++ = (char)(((value % 1000000) / 100000) + 48);
        //    *buffer++ = (char)(((value % 100000) / 10000) + 48);
        //    *buffer++ = (char)(((value % 10000) / 1000) + 48);
        //    *buffer++ = (char)(((value % 1000) / 100) + 48);
        //    *buffer++ = (char)(((value % 100) / 10) + 48);
        //    *buffer++ = (char)((value % 10) + 48);
        //    pos += 20;
        //    return buffer;
        //}

        #endregion

        #region 转换

        //internal unsafe static char* ToString(char* buffer, ref int pos, byte value)
        //{
        //    if (value < 10)
        //        return ToString10(buffer, ref pos, value);
        //    else if (value < 100)
        //        return ToString100(buffer, ref pos, value);
        //    else
        //        return ToString1000(buffer, ref pos, value);
        //}

        //internal unsafe static char* ToString(char* buffer, ref int pos, sbyte value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //            return ToString10(buffer, ref pos, value);
        //        else if (value < 100)
        //            return ToString100(buffer, ref pos, value);
        //        else
        //            return ToString1000(buffer, ref pos, value);
        //    }
        //    else
        //    {
        //        if (value == sbyte.MinValue)
        //        {
        //            *buffer++ = '-';
        //            *buffer++ = '1';
        //            *buffer++ = '2';
        //            *buffer++ = '8';
        //            pos += 4;
        //            return buffer;
        //        }
        //        else
        //        {
        //            *buffer++ = '-';
        //            value = (sbyte)-value;
        //            pos++;
        //            if (value < 10)
        //                return ToString10(buffer, ref pos, value);
        //            else if (value < 100)
        //                return ToString100(buffer, ref pos, value);
        //            else
        //                return ToString1000(buffer, ref pos, value);
        //        }
        //    }
        //}

        //internal unsafe static char* ToString(char* buffer, ref int pos, ushort value)
        //{
        //    if (value < 10)
        //        return ToString10(buffer, ref pos, value);
        //    else if (value < 100)
        //        return ToString100(buffer, ref pos, value);
        //    else if (value < 1000)
        //        return ToString1000(buffer, ref pos, value);
        //    else if (value < 10000)
        //        return ToString10000(buffer, ref pos, value);
        //    else
        //        return ToString100000(buffer, ref pos, value);
        //}

        //internal unsafe static char* ToString(char* buffer, ref int pos, short value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //            return ToString10(buffer, ref pos, value);
        //        else if (value < 100)
        //            return ToString100(buffer, ref pos, value);
        //        else if (value < 1000)
        //            return ToString1000(buffer, ref pos, value);
        //        else if (value < 10000)
        //            return ToString10000(buffer, ref pos, value);
        //        else
        //            return ToString100000(buffer, ref pos, value);
        //    }
        //    else
        //    {
        //        if (value == short.MinValue)
        //        {
        //            *buffer++ = '-';
        //            *buffer++ = '3';
        //            *buffer++ = '2';
        //            *buffer++ = '7';
        //            *buffer++ = '6';
        //            *buffer++ = '8';
        //            pos += 6;
        //            return buffer;
        //        }
        //        else
        //        {
        //            *buffer++ = '-';
        //            pos++;
        //            value = (short)-value;
        //            if (value < 10)
        //                return ToString10(buffer, ref pos, value);
        //            else if (value < 100)
        //                return ToString100(buffer, ref pos, value);
        //            else if (value < 1000)
        //                return ToString1000(buffer, ref pos, value);
        //            else if (value < 10000)
        //                return ToString10000(buffer, ref pos, value);
        //            else
        //                return ToString100000(buffer, ref pos, value);
        //        }
        //    }
        //}

        //internal unsafe static char* ToString(char* buffer, ref int pos, int value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 100000)
        //        {
        //            if (value < 10)
        //            {
        //                return ToString10(buffer, ref  pos, value);
        //            }
        //            else if (value < 100)
        //            {
        //                return ToString100(buffer, ref  pos, value);
        //            }
        //            else if (value < 1000)
        //            {
        //                return ToString1000(buffer, ref  pos, value);
        //            }
        //            else if (value < 10000)
        //            {
        //                return ToString10000(buffer, ref  pos, value);
        //            }
        //            else
        //            {
        //                return ToString100000(buffer, ref pos, value);
        //            }
        //        }
        //        else
        //        {
        //            if (value < 1000000)
        //            {
        //                return ToString1000000(buffer, ref  pos, value);
        //            }
        //            else if (value < 10000000)
        //            {
        //                return ToString10000000(buffer, ref  pos, value);
        //            }
        //            else if (value < 100000000)
        //            {
        //                return ToString100000000(buffer, ref  pos, value);
        //            }
        //            else if (value < 1000000000)
        //            {
        //                return ToString1000000000(buffer, ref  pos, value);
        //            }
        //            else
        //            {
        //                return ToString10000000000(buffer, ref  pos, value);
        //            }
        //        }
        //    }
        //    else
        //        return ToStringNegative(buffer, ref pos, value);
        //}

        //internal unsafe static char* ToStringMin(char* buffer, ref int pos, int value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //            return ToString10(buffer, ref pos, value);
        //        else if (value < 100)
        //            return ToString100(buffer, ref pos, value);
        //        else if (value < 1000)
        //            return ToString1000(buffer, ref pos, value);
        //        else if (value < 10000)
        //            return ToString10000(buffer, ref pos, value);
        //        else if (value < 100000)
        //            return ToString100000(buffer, ref pos, value);
        //        else if (value < 1000000)
        //            return ToString1000000(buffer, ref pos, value);
        //        else if (value < 10000000)
        //            return ToString10000000(buffer, ref pos, value);
        //        else if (value < 100000000)
        //            return ToString100000000(buffer, ref pos, value);
        //        else if (value < 1000000000)
        //            return ToString1000000000(buffer, ref pos, value);
        //        else
        //            return ToString10000000000(buffer, ref pos, value);
        //    }
        //    else
        //        return ToStringNegative(buffer, ref pos, value);
        //}

        //internal unsafe static char* ToStringMax(char* buffer, ref int pos, int value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value > 1000000000)
        //            return ToString10000000000(buffer, ref pos, value);
        //        else if (value > 100000000)
        //            return ToString1000000000(buffer, ref pos, value);
        //        else if (value > 10000000)
        //            return ToString100000000(buffer, ref pos, value);
        //        else if (value > 1000000)
        //            return ToString10000000(buffer, ref pos, value);
        //        else if (value > 100000)
        //            return ToString1000000(buffer, ref pos, value);
        //        else if (value > 10000)
        //            return ToString100000(buffer, ref pos, value);
        //        else if (value > 1000)
        //            return ToString10000(buffer, ref pos, value);
        //        else if (value > 100)
        //            return ToString1000(buffer, ref pos, value);
        //        else if (value > 10)
        //            return ToString100(buffer, ref pos, value);
        //        else
        //            return ToString10(buffer, ref pos, value);
        //    }
        //    else
        //        return ToStringNegative(buffer, ref pos, value);
        //}

        //internal unsafe static char* ToString(char* buffer, ref int pos, uint value)
        //{
        //    if (value < 10)
        //    {
        //        return ToString10(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 100)
        //    {
        //        return ToString100(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 1000)
        //    {
        //        return ToString1000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 10000)
        //    {
        //        return ToString10000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 100000)
        //    {
        //        return ToString100000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 1000000)
        //    {
        //        return ToString1000000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 10000000)
        //    {
        //        return ToString10000000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 100000000)
        //    {
        //        return ToString100000000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 1000000000)
        //    {
        //        return ToString1000000000(buffer, ref  pos, (int)value);
        //    }
        //    else
        //    {
        //        return ToString10000000000(buffer, ref  pos, value);
        //    }
        //}

        //internal unsafe static char* ToString(char* buffer, ref int pos, long value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //        {
        //            return ToString10(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 100)
        //        {
        //            return ToString100(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 1000)
        //        {
        //            return ToString1000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 10000)
        //        {
        //            return ToString10000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 100000)
        //        {
        //            return ToString100000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 1000000)
        //        {
        //            return ToString1000000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 10000000)
        //        {
        //            return ToString10000000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 100000000)
        //        {
        //            return ToString100000000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 1000000000)
        //        {
        //            return ToString1000000000(buffer, ref pos, (int)value);
        //        }


        //        else if (value < 10000000000)
        //        {
        //            return ToString10000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 100000000000)
        //        {
        //            return ToString100000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 1000000000000)
        //        {
        //            return ToString1000000000000(buffer, ref pos, value);
        //        }

        //        else if (value < 10000000000000)
        //        {
        //            return ToString10000000000000(buffer, ref  pos, value);
        //        }
        //        else if (value < 100000000000000)
        //        {
        //            return ToString100000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 1000000000000000)
        //        {
        //            return ToString1000000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 10000000000000000)
        //        {
        //            return ToString10000000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 100000000000000000)
        //        {
        //            return ToString100000000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 1000000000000000000)
        //        {
        //            return ToString1000000000000000000(buffer, ref pos, value);
        //        }
        //        else
        //        {
        //            return ToString10000000000000000000(buffer, ref pos, value);
        //        }
        //    }
        //    else
        //    {
        //        if (value == long.MinValue)
        //        {
        //            //-92233 72036 85477 5808
        //            *buffer++ = '-';
        //            *buffer++ = '9';
        //            *buffer++ = '2';
        //            *buffer++ = '2';
        //            *buffer++ = '3';
        //            *buffer++ = '3';
        //            *buffer++ = '7';
        //            *buffer++ = '2';
        //            *buffer++ = '0';
        //            *buffer++ = '3';
        //            *buffer++ = '6';
        //            *buffer++ = '8';
        //            *buffer++ = '5';
        //            *buffer++ = '4';
        //            *buffer++ = '7';
        //            *buffer++ = '7';
        //            *buffer++ = '5';
        //            *buffer++ = '8';
        //            *buffer++ = '0';
        //            *buffer++ = '8';
        //            pos += 20;
        //            return buffer;
        //        }
        //        else
        //        {
        //            *buffer++ = '-';
        //            value = -value;
        //            pos++;
        //            return ToString(buffer, ref  pos, value);
        //        }
        //    }
        //}

        //internal unsafe static char* ToStringMin(char* buffer, ref int pos, long value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //        {
        //            return ToString10(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 100)
        //        {
        //            return ToString100(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 1000)
        //        {
        //            return ToString1000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 10000)
        //        {
        //            return ToString10000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 100000)
        //        {
        //            return ToString100000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 1000000)
        //        {
        //            return ToString1000000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 10000000)
        //        {
        //            return ToString10000000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 100000000)
        //        {
        //            return ToString100000000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 1000000000)
        //        {
        //            return ToString1000000000(buffer, ref pos, (int)value);
        //        }


        //        else if (value < 10000000000)
        //        {
        //            return ToString10000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 100000000000)
        //        {
        //            return ToString100000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 1000000000000)
        //        {
        //            return ToString1000000000000(buffer, ref pos, value);
        //        }

        //        else if (value < 10000000000000)
        //        {
        //            return ToString10000000000000(buffer, ref  pos, value);
        //        }
        //        else if (value < 100000000000000)
        //        {
        //            return ToString100000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 1000000000000000)
        //        {
        //            return ToString1000000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 10000000000000000)
        //        {
        //            return ToString10000000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 100000000000000000)
        //        {
        //            return ToString100000000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 1000000000000000000)
        //        {
        //            return ToString1000000000000000000(buffer, ref pos, value);
        //        }
        //        else
        //        {
        //            return ToString10000000000000000000(buffer, ref pos, value);
        //        }
        //    }
        //    else
        //        return ToStringNegative(buffer, ref pos, value);
        //}

        //internal unsafe static char* ToStringMax(char* buffer, ref int pos, long value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //        {
        //            return ToString10(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 100)
        //        {
        //            return ToString100(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 1000)
        //        {
        //            return ToString1000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 10000)
        //        {
        //            return ToString10000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 100000)
        //        {
        //            return ToString100000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 1000000)
        //        {
        //            return ToString1000000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 10000000)
        //        {
        //            return ToString10000000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 100000000)
        //        {
        //            return ToString100000000(buffer, ref pos, (int)value);
        //        }
        //        else if (value < 1000000000)
        //        {
        //            return ToString1000000000(buffer, ref pos, (int)value);
        //        }


        //        else if (value < 10000000000)
        //        {
        //            return ToString10000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 100000000000)
        //        {
        //            return ToString100000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 1000000000000)
        //        {
        //            return ToString1000000000000(buffer, ref pos, value);
        //        }

        //        else if (value < 10000000000000)
        //        {
        //            return ToString10000000000000(buffer, ref  pos, value);
        //        }
        //        else if (value < 100000000000000)
        //        {
        //            return ToString100000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 1000000000000000)
        //        {
        //            return ToString1000000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 10000000000000000)
        //        {
        //            return ToString10000000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 100000000000000000)
        //        {
        //            return ToString100000000000000000(buffer, ref pos, value);
        //        }
        //        else if (value < 1000000000000000000)
        //        {
        //            return ToString1000000000000000000(buffer, ref pos, value);
        //        }
        //        else
        //        {
        //            return ToString10000000000000000000(buffer, ref pos, value);
        //        }
        //    }
        //    else
        //        return ToStringNegative(buffer, ref pos, value);
        //}

        //internal unsafe static char* ToString(char* buffer, ref int pos, ulong value)
        //{
        //    if (value < 10)
        //    {
        //        return ToString10(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 100)
        //    {
        //        return ToString100(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 1000)
        //    {
        //        return ToString1000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 10000)
        //    {
        //        return ToString10000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 100000)
        //    {
        //        return ToString100000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 1000000)
        //    {
        //        return ToString1000000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 10000000)
        //    {
        //        return ToString10000000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 100000000)
        //    {
        //        return ToString100000000(buffer, ref  pos, (int)value);
        //    }
        //    else if (value < 1000000000)
        //    {
        //        return ToString1000000000(buffer, ref  pos, (int)value);
        //    }


        //    else if (value < 10000000000)
        //    {
        //        return ToString10000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 100000000000)
        //    {
        //        return ToString100000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 1000000000000)
        //    {
        //        return ToString1000000000000(buffer, ref  pos, (long)value);
        //    }

        //    else if (value < 10000000000000)
        //    {
        //        return ToString10000000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 100000000000000)
        //    {
        //        return ToString100000000000000(buffer, ref   pos, (long)value);
        //    }
        //    else if (value < 1000000000000000)
        //    {
        //        return ToString1000000000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 10000000000000000)
        //    {
        //        return ToString10000000000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 100000000000000000)
        //    {
        //        return ToString100000000000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 1000000000000000000)
        //    {
        //        return ToString1000000000000000000(buffer, ref  pos, (long)value);
        //    }
        //    else if (value < 10000000000000000000)
        //    {
        //        return ToString10000000000000000000(buffer, ref  pos, (long)value);
        //    }
        //    else
        //    {
        //        return ToString100000000000000000000(buffer, ref  pos, value);
        //    }
        //}



        internal unsafe static char* ToString(char* buffer, ref int pos, float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                *buffer++ = 'n';
                *buffer++ = 'u';
                *buffer++ = 'l';
                *buffer++ = 'l';
                pos += 4;
                return buffer;
            }
            else
            {
                string v = value.ToString();
                for (int i = 0; i < v.Length; i++)
                    *buffer++ = v[i];
                pos += v.Length;
                return buffer;
            }
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                *buffer++ = 'n';
                *buffer++ = 'u';
                *buffer++ = 'l';
                *buffer++ = 'l';
                pos += 4;
                return buffer;
            }
            else
            {
                string v = value.ToString();
                for (int i = 0; i < v.Length; i++)
                    *buffer++ = v[i];
                pos += v.Length;
                return buffer;
            }
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, decimal value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                *buffer++ = v[i];
            pos += v.Length;
            return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, bool value)
        {
            if (value)
            {
                *buffer++ = 't';
                *buffer++ = 'r';
                *buffer++ = 'u';
                *buffer++ = 'e';
                //*buffer++ = ',';
                pos += 4;
            }
            else
            {
                *buffer++ = 'f';
                *buffer++ = 'a';
                *buffer++ = 'l';
                *buffer++ = 's';
                *buffer++ = 'e';
                //*buffer++ = ',';
                pos += 5;
            }
            return buffer;
        }



        //internal static int ToString(char[] buffer, int pos, byte value)
        //{
        //    if (value < 10)
        //        return ToString10(buffer, pos, value);
        //    else if (value < 100)
        //        return ToString100(buffer, pos, value);
        //    else
        //        return ToString1000(buffer, pos, value);
        //}

        //internal static int ToString(char[] buffer, int pos, sbyte value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //            return ToString10(buffer, pos, value);
        //        else if (value < 100)
        //            return ToString100(buffer, pos, value);
        //        else
        //            return ToString1000(buffer, pos, value);
        //    }
        //    else
        //    {
        //        if (value == sbyte.MinValue)
        //        {
        //            buffer[pos] = '-';
        //            buffer[pos + 1] = '1';
        //            buffer[pos + 2] = '2';
        //            buffer[pos + 3] = '8';
        //            return 4;
        //        }
        //        else
        //        {
        //            buffer[pos] = '-';
        //            value = (sbyte)-value;
        //            if (value < 10)
        //                return ToString10(buffer, pos + 1, value) + 1;
        //            else if (value < 100)
        //                return ToString100(buffer, pos + 1, value) + 1;
        //            else
        //                return ToString1000(buffer, pos + 1, value) + 1;
        //        }
        //    }
        //}

        //internal static int ToString(char[] buffer, int pos, ushort value)
        //{
        //    if (value < 10)
        //        return ToString10(buffer, pos, value);
        //    else if (value < 100)
        //        return ToString100(buffer, pos, value);
        //    else if (value < 1000)
        //        return ToString1000(buffer, pos, value);
        //    else if (value < 10000)
        //        return ToString10000(buffer, pos, value);
        //    else
        //        return ToString100000(buffer, pos, value);
        //}

        //internal static int ToString(char[] buffer, int pos, short value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //            return ToString10(buffer, pos, value);
        //        else if (value < 100)
        //            return ToString100(buffer, pos, value);
        //        else if (value < 1000)
        //            return ToString1000(buffer, pos, value);
        //        else if (value < 10000)
        //            return ToString10000(buffer, pos, value);
        //        else
        //            return ToString100000(buffer, pos, value);
        //    }
        //    else
        //    {
        //        if (value == short.MinValue)
        //        {
        //            buffer[pos] = '-';
        //            buffer[pos + 1] = '3';
        //            buffer[pos + 2] = '2';
        //            buffer[pos + 3] = '7';
        //            buffer[pos + 3] = '6';
        //            buffer[pos + 3] = '8';
        //            return 6;
        //        }
        //        else
        //        {
        //            buffer[pos] = '-';
        //            value = (short)-value;
        //            if (value < 10)
        //                return ToString10(buffer, pos + 1, value) + 1;
        //            else if (value < 100)
        //                return ToString100(buffer, pos + 1, value) + 1;
        //            else if (value < 1000)
        //                return ToString1000(buffer, pos + 1, value) + 1;
        //            else if (value < 10000)
        //                return ToString10000(buffer, pos + 1, value) + 1;
        //            else
        //                return ToString100000(buffer, pos + 1, value) + 1;
        //        }
        //    }
        //}

        //public static int ToString(char[] buffer, int pos, int value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 100000)
        //        {
        //            if (value < 10)
        //                return ToString10(buffer, pos, value);
        //            else if (value < 100)
        //                return ToString100(buffer, pos, value);
        //            else if (value < 1000)
        //                return ToString1000(buffer, pos, value);
        //            else if (value < 10000)
        //                return ToString10000(buffer, pos, value);
        //            else
        //                return ToString100000(buffer, pos, value);
        //        }
        //        else
        //        {
        //            if (value < 1000000)
        //                return ToString1000000(buffer, pos, value);
        //            else if (value < 10000000)
        //                return ToString10000000(buffer, pos, value);
        //            else if (value < 100000000)
        //                return ToString100000000(buffer, pos, value);
        //            else if (value < 1000000000)
        //                return ToString1000000000(buffer, pos, value);
        //            else
        //                return ToString10000000000(buffer, pos, value);
        //        }
        //    }
        //    else
        //        return ToStringNegative(buffer, pos, value);
        //}

        //internal static int ToStringMin(char[] buffer, int pos, int value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //            return ToString10(buffer, pos, value);
        //        else if (value < 100)
        //            return ToString100(buffer, pos, value);
        //        else if (value < 1000)
        //            return ToString1000(buffer, pos, value);
        //        else if (value < 10000)
        //            return ToString10000(buffer, pos, value);
        //        else if (value < 100000)
        //            return ToString100000(buffer, pos, value);
        //        else if (value < 1000000)
        //            return ToString1000000(buffer, pos, value);
        //        else if (value < 10000000)
        //            return ToString10000000(buffer, pos, value);
        //        else if (value < 100000000)
        //            return ToString100000000(buffer, pos, value);
        //        else if (value < 1000000000)
        //            return ToString1000000000(buffer, pos, value);
        //        else
        //            return ToString10000000000(buffer, pos, value);
        //    }
        //    else
        //        return ToStringNegative(buffer, pos, value);
        //}

        //internal static int ToStringMax(char[] buffer, int pos, int value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value > 1000000000)
        //            return ToString10000000000(buffer, pos, value);
        //        else if (value > 100000000)
        //            return ToString1000000000(buffer, pos, value);
        //        else if (value > 10000000)
        //            return ToString100000000(buffer, pos, value);
        //        else if (value > 1000000)
        //            return ToString10000000(buffer, pos, value);
        //        else if (value > 100000)
        //            return ToString1000000(buffer, pos, value);
        //        else if (value > 10000)
        //            return ToString100000(buffer, pos, value);
        //        else if (value > 1000)
        //            return ToString10000(buffer, pos, value);
        //        else if (value > 100)
        //            return ToString1000(buffer, pos, value);
        //        else if (value > 10)
        //            return ToString100(buffer, pos, value);
        //        else
        //            return ToString10(buffer, pos, value);
        //    }
        //    else
        //        return ToStringNegative(buffer, pos, value);
        //}

        //public static int ToString(char[] buffer, int pos, uint value)
        //{
        //    if (value < 100000)
        //    {
        //        if (value < 10)
        //            return ToString10(buffer, pos, (int)value);
        //        else if (value < 100)
        //            return ToString100(buffer, pos, (int)value);
        //        else if (value < 1000)
        //            return ToString1000(buffer, pos, (int)value);
        //        else if (value < 10000)
        //            return ToString10000(buffer, pos, (int)value);
        //        else
        //            return ToString100000(buffer, pos, (int)value);
        //    }
        //    else
        //    {
        //        if (value < 1000000)
        //            return ToString1000000(buffer, pos, (int)value);
        //        else if (value < 10000000)
        //            return ToString10000000(buffer, pos, (int)value);
        //        else if (value < 100000000)
        //            return ToString100000000(buffer, pos, (int)value);
        //        else if (value < 1000000000)
        //            return ToString1000000000(buffer, pos, (int)value);
        //        else
        //            return ToString10000000000(buffer, pos, value);
        //    }
        //}

        //internal static int ToStringMin(char[] buffer, int pos, uint value)
        //{
        //    if (value < 10)
        //        return ToString10(buffer, pos, (int)value);
        //    else if (value < 100)
        //        return ToString100(buffer, pos, (int)value);
        //    else if (value < 1000)
        //        return ToString1000(buffer, pos, (int)value);
        //    else if (value < 10000)
        //        return ToString10000(buffer, pos, (int)value);
        //    else if (value < 100000)
        //        return ToString100000(buffer, pos, (int)value);
        //    else if (value < 1000000)
        //        return ToString1000000(buffer, pos, (int)value);
        //    else if (value < 10000000)
        //        return ToString10000000(buffer, pos, (int)value);
        //    else if (value < 100000000)
        //        return ToString100000000(buffer, pos, (int)value);
        //    else if (value < 1000000000)
        //        return ToString1000000000(buffer, pos, (int)value);
        //    else
        //        return ToString10000000000(buffer, pos, value);
        //}

        //internal static int ToStringMax(char[] buffer, int pos, uint value)
        //{
        //    if (value > 1000000000)
        //        return ToString10000000000(buffer, pos, value);
        //    else if (value > 100000000)
        //        return ToString1000000000(buffer, pos, (int)value);
        //    else if (value > 10000000)
        //        return ToString100000000(buffer, pos, (int)value);
        //    else if (value > 1000000)
        //        return ToString10000000(buffer, pos, (int)value);
        //    else if (value > 100000)
        //        return ToString1000000(buffer, pos, (int)value);
        //    else if (value > 10000)
        //        return ToString100000(buffer, pos, (int)value);
        //    else if (value > 1000)
        //        return ToString10000(buffer, pos, (int)value);
        //    else if (value > 100)
        //        return ToString1000(buffer, pos, (int)value);
        //    else if (value > 10)
        //        return ToString100(buffer, pos, (int)value);
        //    else
        //        return ToString10(buffer, pos, (int)value);
        //}

        //internal static int ToString(char[] buffer, int pos, long value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 1000000000)
        //        {
        //            if (value < 10)
        //                return ToString10(buffer, pos, (int)value);
        //            else if (value < 100)
        //                return ToString100(buffer, pos, (int)value);
        //            else if (value < 1000)
        //                return ToString1000(buffer, pos, (int)value);
        //            else if (value < 10000)
        //                return ToString10000(buffer, pos, (int)value);
        //            else if (value < 100000)
        //                return ToString100000(buffer, pos, (int)value);
        //            else if (value < 1000000)
        //                return ToString1000000(buffer, pos, (int)value);
        //            else if (value < 10000000)
        //                return ToString10000000(buffer, pos, (int)value);
        //            else if (value < 100000000)
        //                return ToString100000000(buffer, pos, (int)value);
        //            else
        //                return ToString1000000000(buffer, pos, (int)value);
        //        }
        //        else
        //        {                
        //             if (value < 10000000000)
        //                 return ToString10000000000(buffer, pos, value);
        //            else if (value < 100000000000)
        //                 return ToString100000000000(buffer, pos, value);
        //            else if (value < 1000000000000)
        //                 return ToString1000000000000(buffer, pos, value);
        //            else if (value < 10000000000000)
        //                 return ToString10000000000000(buffer, pos, value);
        //            else if (value < 100000000000000)
        //                 return ToString100000000000000(buffer, pos, value);
        //            else if (value < 1000000000000000)
        //                 return ToString1000000000000000(buffer, pos, value);
        //            else if (value < 10000000000000000)
        //                 return ToString10000000000000000(buffer, pos, value);
        //            else if (value < 100000000000000000)
        //                 return ToString100000000000000000(buffer, pos, value);
        //            else if (value < 1000000000000000000)
        //                 return ToString1000000000000000000(buffer, pos, value);
        //            else
        //                 return ToString10000000000000000000(buffer, pos, value);
        //        }
        //    }
        //    else
        //        return ToStringNegative(buffer, pos, value);
        //}

        //internal static int ToStringMin(char[] buffer, int pos, long value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value < 10)
        //                return ToString10(buffer, pos, (int)value);
        //            else if (value < 100)
        //                return ToString100(buffer, pos, (int)value);
        //            else if (value < 1000)
        //                return ToString1000(buffer, pos, (int)value);
        //            else if (value < 10000)
        //                return ToString10000(buffer, pos, (int)value);
        //            else if (value < 100000)
        //                return ToString100000(buffer, pos, (int)value);
        //            else if (value < 1000000)
        //                return ToString1000000(buffer, pos, (int)value);
        //            else if (value < 10000000)
        //                return ToString10000000(buffer, pos, (int)value);
        //            else if (value < 100000000)
        //                return ToString100000000(buffer, pos, (int)value);
        //            else if (value < 1000000000)
        //                return ToString1000000000(buffer, pos, (int)value);
                
        //            else if (value < 10000000000)
        //                 return ToString10000000000(buffer, pos, value);
        //            else if (value < 100000000000)
        //                 return ToString100000000000(buffer, pos, value);
        //            else if (value < 1000000000000)
        //                 return ToString1000000000000(buffer, pos, value);
        //            else if (value < 10000000000000)
        //                 return ToString10000000000000(buffer, pos, value);
        //            else if (value < 100000000000000)
        //                 return ToString100000000000000(buffer, pos, value);
        //            else if (value < 1000000000000000)
        //                 return ToString1000000000000000(buffer, pos, value);
        //            else if (value < 10000000000000000)
        //                 return ToString10000000000000000(buffer, pos, value);
        //            else if (value < 100000000000000000)
        //                 return ToString100000000000000000(buffer, pos, value);
        //            else if (value < 1000000000000000000)
        //                 return ToString1000000000000000000(buffer, pos, value);
        //            else
        //                 return ToString10000000000000000000(buffer, pos, value);
        //    }
        //    else
        //        return ToStringNegativeMin(buffer, pos, value);
        //}

        //internal static int ToStringMax(char[] buffer, int pos, long value)
        //{
        //    if (value >= 0)
        //    {
        //        if (value > 1000000000000000000)
        //            return ToString10000000000000000000(buffer, pos, value);
        //        else if (value > 100000000000000000)
        //            return ToString1000000000000000000(buffer, pos, value);
        //        else if (value > 10000000000000000)
        //            return ToString100000000000000000(buffer, pos, value);
        //        else if (value > 1000000000000000)
        //            return ToString10000000000000000(buffer, pos, value);
        //        else if (value > 100000000000000)
        //            return ToString1000000000000000(buffer, pos, value);
        //        else if (value > 10000000000000)
        //            return ToString100000000000000(buffer, pos, value);
        //        else if (value > 1000000000000)
        //            return ToString10000000000000(buffer, pos, value);
        //        else if (value > 100000000000)
        //            return ToString1000000000000(buffer, pos, value);
        //        else if (value > 10000000000)
        //            return ToString100000000000(buffer, pos, value);
        //        else if (value > 1000000000)
        //            return ToString10000000000(buffer, pos, value);
        //        else if (value > 100000000)
        //            return ToString1000000000(buffer, pos, (int)value);
        //        else if (value > 10000000)
        //            return ToString100000000(buffer, pos, (int)value);
        //        else if (value > 1000000)
        //            return ToString10000000(buffer, pos, (int)value);
        //        else if (value > 100000)
        //            return ToString1000000(buffer, pos, (int)value);
        //        else if (value > 10000)
        //            return ToString100000(buffer, pos, (int)value);
        //        else if (value > 1000)
        //            return ToString10000(buffer, pos, (int)value);
        //        else if (value > 100)
        //            return ToString1000(buffer, pos, (int)value);
        //        else if (value > 10)
        //            return ToString100(buffer, pos, (int)value);
        //        else
        //            return ToString10(buffer, pos, (int)value);
        //    }
        //    else
        //        return ToStringNegativeMax(buffer, pos, value);
        //}

        //internal static int ToString(char[] buffer, int pos, ulong value)
        //{
        //    if (value < 10)
        //    {
        //        return ToString10(buffer, pos, (int)value);
        //    }
        //    else if (value < 100)
        //    {
        //        return ToString100(buffer, pos, (int)value);
        //    }
        //    else if (value < 1000)
        //    {
        //        return ToString1000(buffer, pos, (int)value);
        //    }
        //    else if (value < 10000)
        //    {
        //        return ToString10000(buffer, pos, (int)value);
        //    }
        //    else if (value < 100000)
        //    {
        //        return ToString100000(buffer, pos, (int)value);
        //    }
        //    else if (value < 1000000)
        //    {
        //        return ToString1000000(buffer, pos, (int)value);
        //    }
        //    else if (value < 10000000)
        //    {
        //        return ToString10000000(buffer, pos, (int)value);
        //    }
        //    else if (value < 100000000)
        //    {
        //        return ToString100000000(buffer, pos, (int)value);
        //    }
        //    else if (value < 1000000000)
        //    {
        //        return ToString1000000000(buffer, pos, (int)value);
        //    }


        //    else if (value < 10000000000)
        //    {
        //        return ToString10000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 100000000000)
        //    {
        //        return ToString100000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 1000000000000)
        //    {
        //        return ToString1000000000000(buffer, pos, (long)value);
        //    }

        //    else if (value < 10000000000000)
        //    {
        //        return ToString10000000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 100000000000000)
        //    {
        //        return ToString100000000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 1000000000000000)
        //    {
        //        return ToString1000000000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 10000000000000000)
        //    {
        //        return ToString10000000000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 100000000000000000)
        //    {
        //        return ToString100000000000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 1000000000000000000)
        //    {
        //        return ToString1000000000000000000(buffer, pos, (long)value);
        //    }
        //    else if (value < 10000000000000000000)
        //    {
        //        return ToString10000000000000000000(buffer, pos, (long)value);
        //    }
        //    else
        //    {
        //        return ToString100000000000000000000(buffer, pos, value);
        //    }
        //}

        //internal static int ToStringMin(char[] buffer, int pos, ulong value)
        //{
        //    if (value < 10)
        //        return ToString10(buffer, pos, (int)value);
        //    else if (value < 100)
        //        return ToString100(buffer, pos, (int)value);
        //    else if (value < 1000)
        //        return ToString1000(buffer, pos, (int)value);
        //    else if (value < 10000)
        //        return ToString10000(buffer, pos, (int)value);
        //    else if (value < 100000)
        //        return ToString100000(buffer, pos, (int)value);
        //    else if (value < 1000000)
        //        return ToString1000000(buffer, pos, (int)value);
        //    else if (value < 10000000)
        //        return ToString10000000(buffer, pos, (int)value);
        //    else if (value < 100000000)
        //        return ToString100000000(buffer, pos, (int)value);
        //    else if (value < 1000000000)
        //        return ToString1000000000(buffer, pos, (int)value);

        //    else if (value < 10000000000)
        //        return ToString10000000000(buffer, pos, (long)value);
        //    else if (value < 100000000000)
        //        return ToString100000000000(buffer, pos, (long)value);
        //    else if (value < 1000000000000)
        //        return ToString1000000000000(buffer, pos, (long)value);
        //    else if (value < 10000000000000)
        //        return ToString10000000000000(buffer, pos, (long)value);
        //    else if (value < 100000000000000)
        //        return ToString100000000000000(buffer, pos, (long)value);
        //    else if (value < 1000000000000000)
        //        return ToString1000000000000000(buffer, pos, (long)value);
        //    else if (value < 10000000000000000)
        //        return ToString10000000000000000(buffer, pos, (long)value);
        //    else if (value < 100000000000000000)
        //        return ToString100000000000000000(buffer, pos, (long)value);
        //    else if (value < 1000000000000000000)
        //        return ToString1000000000000000000(buffer, pos, (long)value);
        //    else if (value < 10000000000000000000)
        //        return ToString10000000000000000000(buffer, pos, (long)value);
        //    else
        //        return ToString100000000000000000000(buffer, pos, value);
        //}

        //internal static int ToStringMax(char[] buffer, int pos, ulong value)
        //{
        //    if (value > 10000000000000000000)
        //        return ToString100000000000000000000(buffer, pos, value);
        //    else if (value > 1000000000000000000)
        //        return ToString10000000000000000000(buffer, pos, (long)value);
        //    else if (value > 100000000000000000)
        //        return ToString1000000000000000000(buffer, pos, (long)value);
        //    else if (value > 10000000000000000)
        //        return ToString100000000000000000(buffer, pos, (long)value);
        //    else if (value > 1000000000000000)
        //        return ToString10000000000000000(buffer, pos, (long)value);
        //    else if (value > 100000000000000)
        //        return ToString1000000000000000(buffer, pos, (long)value);
        //    else if (value > 10000000000000)
        //        return ToString100000000000000(buffer, pos, (long)value);
        //    else if (value > 1000000000000)
        //        return ToString10000000000000(buffer, pos, (long)value);
        //    else if (value > 100000000000)
        //        return ToString1000000000000(buffer, pos, (long)value);
        //    else if (value > 10000000000)
        //        return ToString100000000000(buffer, pos, (long)value);
        //    else if (value > 1000000000)
        //        return ToString10000000000(buffer, pos, (long)value);
        //    else if (value > 100000000)
        //        return ToString1000000000(buffer, pos, (int)value);
        //    else if (value > 10000000)
        //        return ToString100000000(buffer, pos, (int)value);
        //    else if (value > 1000000)
        //        return ToString10000000(buffer, pos, (int)value);
        //    else if (value > 100000)
        //        return ToString1000000(buffer, pos, (int)value);
        //    else if (value > 10000)
        //        return ToString100000(buffer, pos, (int)value);
        //    else if (value > 1000)
        //        return ToString10000(buffer, pos, (int)value);
        //    else if (value > 100)
        //        return ToString1000(buffer, pos, (int)value);
        //    else if (value > 10)
        //        return ToString100(buffer, pos, (int)value);
        //    else
        //        return ToString10(buffer, pos, (int)value);
        //}


        internal static int ToString(char[] buffer, int pos, float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                buffer[pos] = 'n';
                buffer[pos + 1] = 'u';
                buffer[pos + 2] = 'l';
                buffer[pos + 3] = 'l';
                return 4;
            }
            else
            {
                string v = value.ToString();
                for (int i = 0; i < v.Length; i++)
                    buffer[pos + i] = v[i];
                return v.Length;
            }
        }

        internal static int ToString(char[] buffer, int pos, double value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                buffer[pos + i] = v[i];
            return v.Length;
        }

        internal static int ToString(char[] buffer, int pos, decimal value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                buffer[pos + i] = v[i];
            return v.Length;
        }

        internal static int ToString(char[] buffer, int pos, bool value)
        {
            if (value)
            {
                buffer[pos] = 't';
                buffer[pos + 1] = 'r';
                buffer[pos + 2] = 'u';
                buffer[pos + 3] = 'e';
                return 4;
            }
            else
            {
                buffer[pos] = 'f';
                buffer[pos + 1] = 'a';
                buffer[pos + 2] = 'l';
                buffer[pos + 3] = 's';
                buffer[pos + 4] = 'e';
                return 5;
            }
        }





        internal unsafe static char* ToString(char* buffer, ref int pos, int value)
        {
            bool isMinus = false;
            if (value < 0)
            {
                if (value > -10)
                {
                    *buffer++ = '-';
                    *buffer++ = (char)((-value % 10) + 48);
                    pos += 2;
                    return buffer;
                }
                else if (value > -100)
                {
                    *buffer++ = '-';
                    *buffer++ = (char)((-value / 10) + 48);
                    *buffer++ = (char)((-value % 10) + 48);
                    pos += 3;
                    return buffer;
                }
                if (value == int.MinValue)
                {
                    fixed (char* smem = Int32Min)
                    {
                        *((uint*)buffer) = *((uint*)smem);
                        *((uint*)(buffer + 2)) = *((uint*)(smem + 2));
                        *((uint*)(buffer + 4)) = *((uint*)(smem + 4));
                        *((uint*)(buffer + 6)) = *((uint*)(smem + 6));
                        *((uint*)(buffer + 8)) = *((uint*)(smem + 8));
                        *(buffer + 10) = *(smem + 10);
                        pos += 11;
                        return buffer;
                    }
                }
                isMinus = true;
                value = -value;
            }
            else if (value < 10)
            {
                *buffer++ = (char)((value % 10) + 48);
                pos += 1;
                return buffer;
            }
            else if (value < 100)
            {
                *buffer++ = (char)((value / 10) + 48);
                *buffer++ = (char)((value % 10) + 48);
                pos += 2;
                return buffer;
            }

            int size = 0;
            buffer += 11;
            do
            {
                var ix = value % 10;
                value /= 10;
                *buffer-- = (char)('0' + ix);
                size++;
            } while (value != 0);
            if (isMinus == true)
            {
                size++;
                *buffer-- = '-';
            }
            int skip = 11 - size;
            buffer -= skip;
            for (int i = 0; i < size; i++)
                *buffer++ = *(buffer + skip);
            pos += size;
            return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, long value)
        {
            return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, uint value)
        {
            int size = 0;
            char* d = buffer + 11;
            do
            {
                var ix = value % 10;
                value /= 10;
                *d-- = (char)('0' + ix);
                size++;
            } while (value != 0);
            int skip = 11 - size;
            d -= skip;
            for (int i = 0; i < size; i++)
                *d++ = *(d + skip);
            pos += size;
            return d;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, ulong value)
        {
            return buffer;
        }


        internal static int ToString(char[] buffer, int pos, int value)
        {
            return ToStringFast(buffer, pos, value);
        }

        internal unsafe static int ToString(char[] buffer, int pos, long value)
        {
            bool isMinus = false;
            if (value < 0)
            {
                if (value > -10)
                {
                    buffer[pos] = '-';
                    buffer[pos + 1] = (char)((-value % 10) + 48);
                    return 2;
                }
                else if (value > -100)
                {
                    buffer[pos] = '-';
                    buffer[pos + 1] = (char)((-value / 10) + 48);
                    buffer[pos + 2] = (char)((-value % 10) + 48);
                    return 3;
                }
                if (value == long.MinValue)
                {
                    fixed (char* smem = Int64Min, pd = &buffer[pos])
                    {
                        char* d = pd;
                        *((uint*)d) = *((uint*)smem);
                        *((uint*)(d + 2)) = *((uint*)(smem + 2));
                        *((uint*)(d + 4)) = *((uint*)(smem + 4));
                        *((uint*)(d + 6)) = *((uint*)(smem + 6));
                        *((uint*)(d + 8)) = *((uint*)(smem + 8));

                        *((uint*)(d + 10)) = *((uint*)(smem + 10));
                        *((uint*)(d + 12)) = *((uint*)(smem + 12));
                        *((uint*)(d + 14)) = *((uint*)(smem + 14));
                        *((uint*)(d + 16)) = *((uint*)(smem + 16));
                        *((uint*)(d + 18)) = *((uint*)(smem + 18));
                        *(d + 20) = *(smem + 20);
                        return 21;
                    }
                }
                isMinus = true;
                value = -value;
            }
            else if (value < 10)
            {
                buffer[pos] = (char)((value % 10) + 48);
                return 1;
            }
            else if (value < 100)
            {
                buffer[pos] = (char)((value / 10) + 48);
                buffer[pos + 1] = (char)((value % 10) + 48);
                return 2;
            }
            int size = 0;
            fixed (char* pd = &buffer[21 + pos])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    size++;
                } while (value != 0);
                if (isMinus == true)
                {
                    size++;
                    *d-- = '-';
                }
                int skip = 21 - size;
                d -= skip;
                for (int i = 0; i < size; i++)
                    *d++ = *(d + skip);
                return size;
            }
        }

        internal unsafe static int ToString(char[] buffer, int pos, uint value)
        {
            int size = 0;
            fixed (char* pd = &buffer[10 + pos])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    size++;
                } while (value != 0);
                int skip = 10 - size;
                d -= skip;
                for (int i = 0; i < size; i++)
                    *d++ = *(d + skip);
                return size;
            }
        }

        internal unsafe static int ToString(char[] buffer, int pos, ulong value)
        {
            int size = 0;
            fixed (char* pd = &buffer[21 + pos])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    size++;
                } while (value != 0);
                int skip = 21 - size;
                d -= skip;
                for (int i = 0; i < size; i++)
                    *d++ = *(d + skip);
                return size;
            }
        }



        public unsafe static int ToStringFast(char[] buffer, int pos, int value)
        {
            bool isMinus = false;
            if (value < 0)
            {
                if (value > -10)
                {
                    buffer[pos] = '-';
                    buffer[pos+1] = (char)((-value % 10) + 48);
                    return 2;
                }
                else if (value > -100)
                {
                    buffer[pos] = '-';
                    buffer[pos + 1] = (char)((-value / 10) + 48);
                    buffer[pos + 2] = (char)((-value % 10) + 48);
                    return 3;
                }
                if (value == int.MinValue)
                {
                    fixed (char* smem = Int32Min, pd = &buffer[pos])
                    {
                        char* d = pd;
                        *((uint*)d) = *((uint*)smem);
                        *((uint*)(d + 2)) = *((uint*)(smem + 2));
                        *((uint*)(d + 4)) = *((uint*)(smem + 4));
                        *((uint*)(d + 6)) = *((uint*)(smem + 6));
                        *((uint*)(d + 8)) = *((uint*)(smem + 8));
                        *(d + 10) = *(smem + 10);
                        return 11;
                    }
                }
                isMinus = true;
                value = -value;
            }
            else if (value < 10)
            {
                buffer[pos] = (char)((value % 10) + 48);
                return 1;
            }
            else if (value < 100)
            {
                buffer[pos] = (char)((value / 10) + 48);
                buffer[pos+1] = (char)((value % 10) + 48);
                return 2;
            }
            int size = 0;
            fixed (char* pd = &buffer[11 + pos])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    size++;
                } while (value != 0);
                if (isMinus == true)
                {
                    size++;
                    *d-- = '-';
                }
                int skip = 11 - size;
                d -= skip;
                for (int i = 0; i < size; i++)
                    *d++ = *(d + skip);
                return size;
            }

            #region old
            //fixed (char* pd = &buffer[ptr+pos])
            //{
            //    char* d = pd;
            //    do
            //    {
            //        var ix = value % 10;
            //        value /= 10;
            //        *d-- = (char)('0' + ix);
            //        ptr--;
            //    } while (value != 0);
            //    if (isMinus == true)
            //    {
            //        ptr--;
            //        *d-- = '-';
            //    }
            //    int count = 11 - ptr;
            //    d -= ptr;
            //    ptr++;
            //    for (int i = 0; i < count; i++)
            //        *(d + pos + i) = *(d + ptr + i);
            //    return count;
            //}
            #endregion
        }
        
        /// <summary>
        /// 将数组写入到字符缓冲区中，目前性能最好的
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public unsafe static int ToStringFast(char[] buffer, int pos, uint value)
        {
            #region old 1
            //int ptr = 11;
            //fixed (char* pd = &buffer[ptr + pos])
            //{
            //    char* d = pd;
            //    do
            //    {
            //        var ix = value % 10;
            //        value /= 10;
            //        *d-- = (char)('0' + ix);
            //        ptr--;
            //    } while (value != 0);
            //    int count = 11 - ptr;
            //    d -= ptr;
            //    ptr++;
            //    for (int i = 0; i < count; i++)
            //        *(d + pos + i) = *(d + ptr + i);
            //    pos += count;
            //}
            #endregion

            #region old 2
            //int size = 0;
            //fixed (char* pd = &buffer[11 + pos])
            //{
            //    char* d = pd;
            //    do
            //    {
            //        var ix = value % 10;
            //        value /= 10;
            //        *d-- = (char)('0' + ix);
            //        size++;
            //    } while (value != 0);
            //    int skip = 11 - size;
            //    d -= skip;
            //    for (int i = 0; i < size; i++)
            //        *d++ = *(d + skip);
            //    pos += size;
            //}
            #endregion
            
            int size = 0;
            fixed (char* pd = &buffer[11 + pos])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    size++;
                } while (value != 0);
                int skip = 11 - size;
                d -= skip;
                for (int i = 0; i < size; i++)
                    *d++ = *(d + skip);
                return size;
            }
        }

        public unsafe static int ToStringFast(char* buffer,int pos, uint value)
        {
            int size = 0;
            buffer += 11;
            do
            {
                var ix = value % 10;
                value /= 10;
                *buffer-- = (char)('0' + ix);
                size++;
            } while (value != 0);
            int skip = 11 - size;
            buffer -= skip;
            for (int i = 0; i < size; i++)
                *buffer++ = *(buffer + skip);
            return size;
            //pos += size;
            //return buffer;
        }

        public unsafe static int ToStringFast(char[] buffer, int pos, long value)
        {
            int ptr = 21;
            bool isMinus = false;
            if (value < 0)
            {
                if (value > -10)
                {
                    buffer[pos] = '-';
                    buffer[pos + 1] = (char)((-value % 10) + 48);
                    return 2;
                }
                else if (value > -100)
                {
                    buffer[pos] = '-';
                    buffer[pos + 1] = (char)((-value / 10) + 48);
                    buffer[pos + 2] = (char)((-value % 10) + 48);
                    return 3;
                }
                if (value == int.MinValue)
                {
                    fixed (char* smem = Int64Min, pd = &buffer[pos])
                    {
                        char* d = pd;
                        *((uint*)d) = *((uint*)smem);
                        *((uint*)(d + 2)) = *((uint*)(smem + 2));
                        *((uint*)(d + 4)) = *((uint*)(smem + 4));
                        *((uint*)(d + 6)) = *((uint*)(smem + 6));
                        *((uint*)(d + 8)) = *((uint*)(smem + 8));
                        *((uint*)(d + 10)) = *((uint*)(smem + 10));
                        *((uint*)(d + 12)) = *((uint*)(smem + 12));
                        *((uint*)(d + 14)) = *((uint*)(smem + 14));
                        *((uint*)(d + 16)) = *((uint*)(smem + 16));
                        *((uint*)(d + 18)) = *((uint*)(smem + 18));
                        *(d + 20) = *(smem + 20);
                        return 21;
                    }
                }
                isMinus = true;
                value = -value;
            }
            else if (value < 10)
            {
                buffer[pos] = (char)((value % 10) + 48);
                return 1;
            }
            else if (value < 100)
            {
                buffer[pos] = (char)((value / 10) + 48);
                buffer[pos + 1] = (char)((value % 10) + 48);
                return 2;
            }
            fixed (char* pd = &buffer[ptr])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    ptr--;
                } while (value != 0);
                if (isMinus == true)
                {
                    ptr--;
                    *d-- = '-';
                }
                int count = 21 - ptr;
                d -= ptr;
                ptr++;
                for (int i = 0; i < count; i++)
                    *(d + pos + i) = *(d + ptr + i);
                return count;
            }
        }

        public unsafe static int ToStringFast(char[] buffer, int pos, ulong value)
        {
            return 0;
            //int ptr = 21;
            //fixed (char* pd = &buffer[ptr])
            //{
            //    char* d = pd;
            //    do
            //    {
            //        var ix = value % 10;
            //        value /= 10;
            //        *d-- = (char)('0' + ix);
            //        ptr--;
            //    } while (value != 0);
            //    int count = 21 - ptr;
            //    d -= ptr;
            //    ptr++;
            //    for (int i = 0; i < count; i++)
            //        *(d + pos + i) = *(d + ptr + i);
            //    return count;
            //}
        }

        #endregion

        #region 时间

        internal unsafe static char* ToString(char* buffer, ref int pos, DateTime value)
        {
            //string s = value.ToString("o");
            //2013-07-09T17:13:22.5532438+08:00
            //TimeZoneInfo.Local.BaseUtcOffset
            int v = value.Year;
            *buffer++ = (char)((v / 1000) + 48);
            *buffer++ = (char)(((v % 1000) / 100) + 48);
            *buffer++ = (char)(((v % 100) / 10) + 48);
            *buffer++ = (char)((v % 10) + 48);

            v = value.Month;
            *buffer++ = '-';
            *buffer++ = (char)(((v % 100) / 10) + 48);
            *buffer++ = (char)((v % 10) + 48);

            v = value.Day;
            *buffer++ = '-';
            *buffer++ = (char)(((v % 100) / 10) + 48);
            *buffer++ = (char)((v % 10) + 48);

            v = value.Hour;
            *buffer++ = 'T';
            *buffer++ = (char)(((v % 100) / 10) + 48);
            *buffer++ = (char)((v % 10) + 48);

            v = value.Minute;
            *buffer++ = ':';
            *buffer++ = (char)(((v % 100) / 10) + 48);
            *buffer++ = (char)((v % 10) + 48);

            v = value.Second;
            *buffer++ = ':';
            *buffer++ = (char)(((v % 100) / 10) + 48);
            *buffer++ = (char)((v % 10) + 48);

            pos += 19;
            return buffer;

            //v = value.Millisecond;
            //*buffer++ = (char)('.');
            //*buffer++ = (char)((v / 100) + 48);
            //*buffer++ = (char)(((v % 100) / 10) + 48);
            //*buffer++ = (char)((v % 10) + 48);

            //*buffer++ = (char)('0');
            //*buffer++ = (char)('0');
            //*buffer++ = (char)('0');
            //*buffer++ = (char)('0');

            //TimeSpan span = TimeZoneInfo.Local.BaseUtcOffset;
            //v = span.Hours;
            //*buffer++ = (char)('+');
            //*buffer++ = (char)(((v % 100) / 10) + 48);
            //*buffer++ = (char)((v % 10) + 48);

            //v = span.Minutes;
            //*buffer++ = (char)(':');
            //*buffer++ = (char)(((v % 100) / 10) + 48);
            //*buffer++ = (char)((v % 10) + 48);

            //pos += 33;
            //return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, TimeSpan value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                *buffer++ = (char)v[i];
            pos += v.Length;
            return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, DateTimeOffset value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                *buffer++ = (char)v[i];
            pos += v.Length;
            return buffer;
        }

        /// <summary>
        /// ISO 8601日期格式，去掉异常判断，提升性能
        /// 时间转换暂时不能获取到微秒部分的数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static int ToString(char[] buffer, int pos, DateTime value)
        {
            //string s = value.ToString("o");
            //2013-07-09T17:13:22.5532438+08:00
            //TimeZoneInfo.Local.BaseUtcOffset
            int v = value.Year;
            buffer[pos] = (char)((v / 1000) + 48);
            buffer[pos + 1] = (char)(((v % 1000) / 100) + 48);
            buffer[pos + 2] = (char)(((v % 100) / 10) + 48);
            buffer[pos + 3] = (char)((v % 10) + 48);

            v = value.Month;
            buffer[pos + 4] = '-';
            buffer[pos + 5] = (char)(((v % 100) / 10) + 48);
            buffer[pos + 6] = (char)((v % 10) + 48);

            v = value.Day;
            buffer[pos + 7] = '-';
            buffer[pos + 8] = (char)(((v % 100) / 10) + 48);
            buffer[pos + 9] = (char)((v % 10) + 48);

            v = value.Hour;
            buffer[pos + 10] = 'T';
            buffer[pos + 11] = (char)(((v % 100) / 10) + 48);
            buffer[pos + 12] = (char)((v % 10) + 48);

            v = value.Minute;
            buffer[pos + 13] = ':';
            buffer[pos + 14] = (char)(((v % 100) / 10) + 48);
            buffer[pos + 15] = (char)((v % 10) + 48);

            v = value.Second;
            buffer[pos + 16] = (char)(':');
            buffer[pos + 17] = (char)(((v % 100) / 10) + 48);
            buffer[pos + 18] = (char)((v % 10) + 48);

            return 19;

            //v = value.Millisecond;
            //buffer[pos + 19] = (char)('.');
            //buffer[pos + 20] = (char)((v / 100) + 48);
            //buffer[pos + 21] = (char)(((v % 100) / 10) + 48);
            //buffer[pos + 22] = (char)((v % 10) + 48);

            //buffer[pos + 23] = (char)('0');
            //buffer[pos + 24] = (char)('0');
            //buffer[pos + 25] = (char)('0');
            //buffer[pos + 26] = (char)('0');

            //TimeSpan span = TimeZoneInfo.Local.BaseUtcOffset;
            //v = span.Hours;
            //buffer[pos + 27] = (char)('+');
            //buffer[pos + 28] = (char)(((v % 100) / 10) + 48);
            //buffer[pos + 29] = (char)((v % 10) + 48);

            //v = span.Minutes;
            //buffer[pos + 30] = (char)(':');
            //buffer[pos + 31] = (char)(((v % 100) / 10) + 48);
            //buffer[pos + 32] = (char)((v % 10) + 48);

            //return 33;
        }

        internal static int ToString(char[] buffer, int pos, TimeSpan value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                buffer[pos + i] = (char)v[i];
            return v.Length;
        }

        internal static int ToString(char[] buffer, int pos, DateTimeOffset value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                buffer[pos + i] = (char)v[i];
            return v.Length;
        }

        #endregion

        #region 其它基础类型

        internal unsafe static char* ToString(char* buffer, ref int pos, Guid value)
        {
            //Guid sp = new Guid("337c7f2b-7a34-4f50-9141-bab9e6478cc8");
            string v = value.ToString();
            fixed (char* pd = v)
            {
                Utils.FastCopyGuid(buffer, pd);
                buffer += 36;
            }
            pos += v.Length;
            return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, Uri value)
        {
            string v = value.AbsoluteUri;
            fixed (char* pd = v)
            {
                Utils.wstrcpy(buffer, pd, v.Length);
            }
            pos += v.Length;
            return buffer;
        }

        internal unsafe static char* ToString(char* buffer, ref int pos, Enum value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                *buffer++ = (char)v[i];
            pos += v.Length;
            return buffer;
        }

        public unsafe static void WriteGuid(Guid guid, char[] buffer)
        {
            // 1314FAD4-7505-439D-ABD2-DBD89242928C
            // 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
            //
            // Guid is guaranteed to be a 36 character string

            // get all the dashes in place
            buffer[8] = '-';
            buffer[13] = '-';
            buffer[18] = '-';
            buffer[23] = '-';

            // Bytes are in a different order than you might expect
            // For: 35 91 8b c9 - 19 6d - 40 ea  - 97 79  - 88 9d 79 b7 53 f0 
            // Get: C9 8B 91 35   6D 19   EA 40    97 79    88 9D 79 B7 53 F0 
            // Ix:   0  1  2  3    4  5    6  7     8  9    10 11 12 13 14 15
            //
            // And we have to account for dashes
            //
            // So the map is like so:
            // bytes[0]  -> chars[3]  -> buffer[ 6, 7]
            // bytes[1]  -> chars[2]  -> buffer[ 4, 5]
            // bytes[2]  -> chars[1]  -> buffer[ 2, 3]
            // bytes[3]  -> chars[0]  -> buffer[ 0, 1]
            // bytes[4]  -> chars[5]  -> buffer[11,12]
            // bytes[5]  -> chars[4]  -> buffer[ 9,10]
            // bytes[6]  -> chars[7]  -> buffer[16,17]
            // bytes[7]  -> chars[6]  -> buffer[14,15]
            // bytes[8]  -> chars[8]  -> buffer[19,20]
            // bytes[9]  -> chars[9]  -> buffer[21,22]
            // bytes[10] -> chars[10] -> buffer[24,25]
            // bytes[11] -> chars[11] -> buffer[26,27]
            // bytes[12] -> chars[12] -> buffer[28,29]
            // bytes[13] -> chars[13] -> buffer[30,31]
            // bytes[14] -> chars[14] -> buffer[32,33]
            // bytes[15] -> chars[15] -> buffer[34,35]
            var visibleMembers = new GuidStruct(guid);

            // bytes[0]
            var b = visibleMembers.B00 * 2;
            buffer[6] = WriteGuidLookup[b];
            buffer[7] = WriteGuidLookup[b + 1];

            // bytes[1]
            b = visibleMembers.B01 * 2;
            buffer[4] = WriteGuidLookup[b];
            buffer[5] = WriteGuidLookup[b + 1];

            // bytes[2]
            b = visibleMembers.B02 * 2;
            buffer[2] = WriteGuidLookup[b];
            buffer[3] = WriteGuidLookup[b + 1];

            // bytes[3]
            b = visibleMembers.B03 * 2;
            buffer[0] = WriteGuidLookup[b];
            buffer[1] = WriteGuidLookup[b + 1];

            // bytes[4]
            b = visibleMembers.B04 * 2;
            buffer[11] = WriteGuidLookup[b];
            buffer[12] = WriteGuidLookup[b + 1];

            // bytes[5]
            b = visibleMembers.B05 * 2;
            buffer[9] = WriteGuidLookup[b];
            buffer[10] = WriteGuidLookup[b + 1];

            // bytes[6]
            b = visibleMembers.B06 * 2;
            buffer[16] = WriteGuidLookup[b];
            buffer[17] = WriteGuidLookup[b + 1];

            // bytes[7]
            b = visibleMembers.B07 * 2;
            buffer[14] = WriteGuidLookup[b];
            buffer[15] = WriteGuidLookup[b + 1];

            // bytes[8]
            b = visibleMembers.B08 * 2;
            buffer[19] = WriteGuidLookup[b];
            buffer[20] = WriteGuidLookup[b + 1];

            // bytes[9]
            b = visibleMembers.B09 * 2;
            buffer[21] = WriteGuidLookup[b];
            buffer[22] = WriteGuidLookup[b + 1];

            // bytes[10]
            b = visibleMembers.B10 * 2;
            buffer[24] = WriteGuidLookup[b];
            buffer[25] = WriteGuidLookup[b + 1];

            // bytes[11]
            b = visibleMembers.B11 * 2;
            buffer[26] = WriteGuidLookup[b];
            buffer[27] = WriteGuidLookup[b + 1];

            // bytes[12]
            b = visibleMembers.B12 * 2;
            buffer[28] = WriteGuidLookup[b];
            buffer[29] = WriteGuidLookup[b + 1];

            // bytes[13]
            b = visibleMembers.B13 * 2;
            buffer[30] = WriteGuidLookup[b];
            buffer[31] = WriteGuidLookup[b + 1];

            // bytes[14]
            b = visibleMembers.B14 * 2;
            buffer[32] = WriteGuidLookup[b];
            buffer[33] = WriteGuidLookup[b + 1];

            // bytes[15]
            b = visibleMembers.B15 * 2;
            buffer[34] = WriteGuidLookup[b];
            buffer[35] = WriteGuidLookup[b + 1];

            //writer.Write(buffer, 0, 36);
        }

        public unsafe static void ToString(char[] buffer, int pos, Guid value)
        {
            fixed (char* ds = &buffer[pos])
            {
                char* pd = ds;
                var visibleMembers = new GuidStruct(value);

                // bytes[3]
                var b = visibleMembers.B03 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[2]
                b = visibleMembers.B02 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[1]
                b = visibleMembers.B01 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[0]
                b = visibleMembers.B00 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                *pd++ = '-';

                // bytes[5]
                b = visibleMembers.B05 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[4]
                b = visibleMembers.B04 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                *pd++ = '-';

                // bytes[7]
                b = visibleMembers.B07 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[6]
                b = visibleMembers.B06 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                *pd++ = '-';

                // bytes[8]
                b = visibleMembers.B08 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[9]
                b = visibleMembers.B09 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                *pd++ = '-';

                // bytes[10]
                b = visibleMembers.B10 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[11]
                b = visibleMembers.B11 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[12]
                b = visibleMembers.B12 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[13]
                b = visibleMembers.B13 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[14]
                b = visibleMembers.B14 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];

                // bytes[15]
                b = visibleMembers.B15 * 2;
                *pd++ = WriteGuidLookup[b];
                *pd++ = WriteGuidLookup[b + 1];
            }


            //fixed (char* ds = &buffer[pos])
            //{
            //    char* pd = ds;
            //    var visibleMembers = new GuidStruct(value);

            //    // bytes[3]
            //    var b = visibleMembers.B03 * 2;
            //    buffer[0] = WriteGuidLookup[b];
            //    buffer[1] = WriteGuidLookup[b + 1];

            //    // bytes[2]
            //    b = visibleMembers.B02 * 2;
            //    buffer[2] = WriteGuidLookup[b];
            //    buffer[3] = WriteGuidLookup[b + 1];

            //    // bytes[1]
            //    b = visibleMembers.B01 * 2;
            //    buffer[4] = WriteGuidLookup[b];
            //    buffer[5] = WriteGuidLookup[b + 1];

            //    // bytes[0]
            //    b = visibleMembers.B00 * 2;
            //    buffer[6] = WriteGuidLookup[b];
            //    buffer[7] = WriteGuidLookup[b + 1];

            //    buffer[8] = '-';

            //    // bytes[5]
            //    b = visibleMembers.B05 * 2;
            //    buffer[9] = WriteGuidLookup[b];
            //    buffer[10] = WriteGuidLookup[b + 1];

            //    // bytes[4]
            //    b = visibleMembers.B04 * 2;
            //    buffer[11] = WriteGuidLookup[b];
            //    buffer[12] = WriteGuidLookup[b + 1];

            //    buffer[13] = '-';

            //    // bytes[7]
            //    b = visibleMembers.B07 * 2;
            //    buffer[14] = WriteGuidLookup[b];
            //    buffer[15] = WriteGuidLookup[b + 1];

            //    // bytes[6]
            //    b = visibleMembers.B06 * 2;
            //    buffer[16] = WriteGuidLookup[b];
            //    buffer[17] = WriteGuidLookup[b + 1];

            //    buffer[18] = '-';

            //    // bytes[8]
            //    b = visibleMembers.B08 * 2;
            //    buffer[19] = WriteGuidLookup[b];
            //    buffer[20] = WriteGuidLookup[b + 1];

            //    // bytes[9]
            //    b = visibleMembers.B09 * 2;
            //    buffer[21] = WriteGuidLookup[b];
            //    buffer[22] = WriteGuidLookup[b + 1];

            //    buffer[23] = '-';

            //    // bytes[10]
            //    b = visibleMembers.B10 * 2;
            //    buffer[24] = WriteGuidLookup[b];
            //    buffer[25] = WriteGuidLookup[b + 1];

            //    // bytes[11]
            //    b = visibleMembers.B11 * 2;
            //    buffer[26] = WriteGuidLookup[b];
            //    buffer[27] = WriteGuidLookup[b + 1];

            //    // bytes[12]
            //    b = visibleMembers.B12 * 2;
            //    buffer[28] = WriteGuidLookup[b];
            //    buffer[29] = WriteGuidLookup[b + 1];

            //    // bytes[13]
            //    b = visibleMembers.B13 * 2;
            //    buffer[30] = WriteGuidLookup[b];
            //    buffer[31] = WriteGuidLookup[b + 1];

            //    // bytes[14]
            //    b = visibleMembers.B14 * 2;
            //    buffer[32] = WriteGuidLookup[b];
            //    buffer[33] = WriteGuidLookup[b + 1];

            //    // bytes[15]
            //    b = visibleMembers.B15 * 2;
            //    buffer[34] = WriteGuidLookup[b];
            //    buffer[35] = WriteGuidLookup[b + 1];
            //}
        }

        internal unsafe static int ToString(char[] buffer, int pos, Uri value)
        {
            string v = value.AbsoluteUri;
            fixed (char* pd = v,dst=&buffer[pos])
            {
                Utils.wstrcpy(dst, pd, v.Length);
            }
            return v.Length;
        }

        internal static int ToString(char[] buffer, int pos, Enum value)
        {
            string v = value.ToString();
            for (int i = 0; i < v.Length; i++)
                buffer[pos + i] = (char)v[i];
            return v.Length;
        }

        internal unsafe static int ToString(char[] buffer, int pos, byte[] value)
        {
            fixed (char* pdst = &buffer[pos])
            {
                fixed (byte* psrc = value)
                {
                    Utils.ConvertToBase64Array(pdst, psrc, 0, value.Length, false);
                }
            }
            return (value.Length << 1);
        }

        #endregion

        #region 其它

        //private unsafe static char* ToStringNegative(char* buffer, ref int pos, int value)
        //{
        //    if (value == int.MinValue)
        //    {
        //        //-2147483648
        //        //*buffer++ = '-';
        //        //*buffer++ = '2';
        //        //*buffer++ = '1';
        //        //*buffer++ = '4';
        //        //*buffer++ = '7';
        //        //*buffer++ = '4';
        //        //*buffer++ = '8';
        //        //*buffer++ = '3';
        //        //*buffer++ = '6';
        //        //*buffer++ = '4';
        //        //*buffer++ = '8';
        //        //pos += 11;
        //        //return buffer;

        //        fixed (char* smem = Int32Min)
        //        {
        //            *((uint*)buffer) = *((uint*)smem);
        //            *((uint*)(buffer + 2)) = *((uint*)(smem + 2));
        //            *((uint*)(buffer + 4)) = *((uint*)(smem + 4));
        //            *((uint*)(buffer + 6)) = *((uint*)(smem + 6));
        //            *((uint*)(buffer + 8)) = *((uint*)(smem + 8));
        //            *(buffer + 10) = *(smem + 10);
        //            pos += 11;
        //            buffer += 11;
        //            return buffer;
        //        }
        //    }
        //    else
        //    {
        //        *buffer++ = '-';
        //        value = -value;
        //        pos++;
        //        return ToString(buffer, ref pos, value);
        //    }
        //}

        //private unsafe static int ToStringNegative(char[] buffer, int pos, int value)
        //{
        //    if (value == int.MinValue)
        //    {
        //        //-2147483648
        //        buffer[pos] = '-';
        //        buffer[pos + 1] = '2';
        //        buffer[pos + 2] = '1';
        //        buffer[pos + 3] = '4';
        //        buffer[pos + 4] = '7';
        //        buffer[pos + 5] = '4';
        //        buffer[pos + 6] = '8';
        //        buffer[pos + 7] = '3';
        //        buffer[pos + 8] = '6';
        //        buffer[pos + 9] = '4';
        //        buffer[pos + 10] = '8';
        //        return 11;
        //    }
        //    else
        //    {
        //        buffer[pos] = '-';
        //        value = -value;
        //        return ToString(buffer, pos + 1, value) + 1;
        //    }
        //}


        //private unsafe static char* ToStringNegative(char* buffer, ref int pos, long value)
        //{
        //    if (value == long.MinValue)
        //        return WriteLongMin(buffer, ref pos);
        //    else
        //    {
        //        *buffer++ = '-';
        //        value = -value;
        //        pos++;
        //        return ToString(buffer, ref pos, value);
        //    }
        //}

        //private unsafe static char* ToStringNegativeMin(char* buffer, ref int pos, long value)
        //{
        //    if (value == long.MinValue)
        //        return WriteLongMin(buffer, ref pos);
        //    else
        //    {
        //        *buffer++ = '-';
        //        value = -value;
        //        pos++;
        //        return ToStringMin(buffer, ref pos, value);
        //    }
        //}

        //private unsafe static char* ToStringNegativeMax(char* buffer, ref int pos, long value)
        //{
        //    if (value == long.MinValue)
        //        return WriteLongMin(buffer, ref pos);
        //    else
        //    {
        //        *buffer++ = '-';
        //        value = -value;
        //        pos++;
        //        return ToStringMax(buffer, ref pos, value);
        //    }
        //}


        //private unsafe static int ToStringNegative(char[] buffer, int pos, long value)
        //{
        //    if (value == long.MinValue)
        //    {
        //        WriteLongMin(buffer, pos);
        //        return 20;
        //    }
        //    else
        //    {
        //        buffer[pos] = '-';
        //        value = -value;
        //        return ToString(buffer, pos + 1, value) + 1;
        //    }
        //}

        //private unsafe static int ToStringNegativeMin(char[] buffer, int pos, long value)
        //{
        //    if (value == long.MinValue)
        //    {
        //        WriteLongMin(buffer, pos);
        //        return 20;
        //    }
        //    else
        //    {
        //        buffer[pos] = '-';
        //        value = -value;
        //        return ToStringMin(buffer, pos + 1, value) + 1;
        //    }
        //}

        //private unsafe static int ToStringNegativeMax(char[] buffer, int pos, long value)
        //{
        //    if (value == long.MinValue)
        //    {
        //        WriteLongMin(buffer, pos);
        //        return 20;
        //    }
        //    else
        //    {
        //        buffer[pos] = '-';
        //        value = -value;
        //        return ToStringMax(buffer, pos + 1, value) + 1;
        //    }
        //}


        //private unsafe static char* WriteLongMin(char* buffer, ref int pos)
        //{
        //    fixed (char* smem = Int64Min)
        //    {
        //        ////-92233 72036 85477 5808
        //        //*buffer++ = '-';
        //        //*buffer++ = '9';
        //        //*buffer++ = '2';
        //        //*buffer++ = '2';
        //        //*buffer++ = '3';
        //        //*buffer++ = '3';
        //        //*buffer++ = '7';
        //        //*buffer++ = '2';
        //        //*buffer++ = '0';
        //        //*buffer++ = '3';
        //        //*buffer++ = '6';
        //        //*buffer++ = '8';
        //        //*buffer++ = '5';
        //        //*buffer++ = '4';
        //        //*buffer++ = '7';
        //        //*buffer++ = '7';
        //        //*buffer++ = '5';
        //        //*buffer++ = '8';
        //        //*buffer++ = '0';
        //        //*buffer++ = '8';
        //        //pos += 20;
        //        //return buffer;

        //        *((uint*)buffer) = *((uint*)smem);
        //        *((uint*)(buffer + 2)) = *((uint*)(smem + 2));
        //        *((uint*)(buffer + 4)) = *((uint*)(smem + 4));
        //        *((uint*)(buffer + 6)) = *((uint*)(smem + 6));
        //        *((uint*)(buffer + 8)) = *((uint*)(smem + 8));
        //        *((uint*)(buffer + 10)) = *((uint*)(smem + 10));
        //        *((uint*)(buffer + 12)) = *((uint*)(smem + 12));
        //        *((uint*)(buffer + 14)) = *((uint*)(smem + 14));
        //        *((uint*)(buffer + 16)) = *((uint*)(smem + 16));
        //        *((uint*)(buffer + 18)) = *((uint*)(smem + 18));
        //        pos += 20;
        //        buffer += 20;
        //        return buffer;
        //    }
        //}

        //private unsafe static void WriteLongMin(char[] buffer, int pos)
        //{
        //    fixed (char* pd = &buffer[pos], smem = Int64Min)
        //    {
        //        //-92233 72036 85477 5808
        //        //buffer[pos] = '-';
        //        //buffer[pos + 1] = '9';
        //        //buffer[pos + 2] = '2';
        //        //buffer[pos + 3] = '2';
        //        //buffer[pos + 4] = '3';
        //        //buffer[pos + 5] = '3';
        //        //buffer[pos + 6] = '7';
        //        //buffer[pos + 7] = '2';
        //        //buffer[pos + 8] = '0';
        //        //buffer[pos + 9] = '3';
        //        //buffer[pos + 10] = '6';
        //        //buffer[pos + 11] = '8';
        //        //buffer[pos + 12] = '5';
        //        //buffer[pos + 13] = '4';
        //        //buffer[pos + 14] = '7';
        //        //buffer[pos + 15] = '7';
        //        //buffer[pos + 16] = '5';
        //        //buffer[pos + 17] = '8';
        //        //buffer[pos + 18] = '0';
        //        //buffer[pos + 19] = '8';
        //        //return 20;

        //        char* tpd = pd;
        //        *((uint*)tpd) = *((uint*)smem);
        //        *((uint*)(tpd + 2)) = *((uint*)(smem + 2));
        //        *((uint*)(tpd + 4)) = *((uint*)(smem + 4));
        //        *((uint*)(tpd + 6)) = *((uint*)(smem + 6));
        //        *((uint*)(tpd + 8)) = *((uint*)(smem + 8));
        //        *((uint*)(tpd + 10)) = *((uint*)(smem + 10));
        //        *((uint*)(tpd + 12)) = *((uint*)(smem + 12));
        //        *((uint*)(tpd + 14)) = *((uint*)(smem + 14));
        //        *((uint*)(tpd + 16)) = *((uint*)(smem + 16));
        //        *((uint*)(tpd + 18)) = *((uint*)(smem + 18));
        //    }
        //}

        #endregion

        #region 纯ToString

        public unsafe static string ToString(uint value)
        {
            int ptr = 11;
            char[] buffer = new char[12];
            fixed (char* pd = &buffer[ptr])
            {
                char* d = pd;
                do
                {
                    var ix = value % 10;
                    value /= 10;
                    *d-- = (char)('0' + ix);
                    ptr--;
                } while (value != 0);
                return new string(buffer, ptr + 1, 11 - ptr);
            }
        }

        public unsafe static int ToStringSimple(char[] buffer, int pos, uint value)
        {
            int ptr = 11;
            fixed (char* pd = &buffer[ptr])
            {
                char* d = pd;
                do
                {
                    byte ix = (byte)(value % 100);
                    value /= 100;
                    var chars = DigitPairs[ix];
                    *d-- = chars.Second;
                    *d-- = chars.First;
                    ptr -= 2;
                } while (value != 0);
                if (*(d + 1) == '0')
                    ptr++;
                int count = 11 - ptr;
                d -= (ptr - 1);
                ptr++;
                for (int i = 0; i < count; i++)
                    *(d + pos + i) = *(d + ptr + i);
                return count;
            }

            #region safe

            //int ptr = 20;
            //var copy = value;
            //do
            //{
            //    byte ix = (byte)(copy % 100);
            //    copy /= 100;

            //    var chars = DigitPairs[ix];
            //    buffer[ptr--] = chars.Second;
            //    buffer[ptr--] = chars.First;
            //} while (copy != 0);

            //if (buffer[ptr + 1] == '0')
            //    ptr++;
            //int count = 20 - ptr;
            //ptr++;
            //for (int i = 0; i < count; i++)
            //    buffer[pos + i] = buffer[ptr + i];
            //return count;

            #endregion
        }

        public unsafe static string ToString(Guid value)
        {
            char[] buffer = new char[36];
            ToString(buffer, 0, value);
            return new string(buffer);
        }

        #endregion
    }

    

}
