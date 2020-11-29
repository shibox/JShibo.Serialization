using JShibo.Serialization.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace JShibo.SerializationCore.BenchMark.Tester
{
    public class ToStringBenckmark
    {
        /// <summary>
        /// 234  三个长度测试
        /// 测试A：407ms
        /// 测试B：273ms
        /// </summary>
        public unsafe static void Run()
        {
            var buffer = stackalloc byte[100];
            var value = (byte)45;
            var count = 100000000;
            var w = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                FastToString.ToString(buffer, value);
            Console.WriteLine($"A first cost:{w.ElapsedMilliseconds}");

            w = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                FastToString.ToStringNumber(buffer, value);
            Console.WriteLine($"B first cost:{w.ElapsedMilliseconds}");

            w = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                FastToString.ToString(buffer, value);
            Console.WriteLine($"A second cost:{w.ElapsedMilliseconds}");

            w = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                FastToString.ToStringNumber(buffer, value);
            Console.WriteLine($"B second cost:{w.ElapsedMilliseconds}");

        }

    }
}
