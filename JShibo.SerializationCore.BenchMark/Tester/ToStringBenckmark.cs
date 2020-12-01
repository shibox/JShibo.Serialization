using JShibo.Serialization.Common;
using JShibo.SerializationCore.Common;
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
            var value = (byte)234;
            var count = 100000000;
            var length = 0;
            var w = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                length = FastToString.ToString(buffer, value);
            Console.WriteLine($"A first cost:{w.ElapsedMilliseconds}");

            w = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                length = FastToString.ToStringNumber(buffer, value);
            Console.WriteLine($"B first cost:{w.ElapsedMilliseconds}");

            w = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                length = NumberHelper.ToString(buffer, value);
            Console.WriteLine($"C first cost:{w.ElapsedMilliseconds}");

            w = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                length = FastToString.ToString(buffer, value);
            Console.WriteLine($"A second cost:{w.ElapsedMilliseconds}");

            w = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                length = FastToString.ToStringNumber(buffer, value);
            Console.WriteLine($"B second cost:{w.ElapsedMilliseconds}");

            w = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                length = NumberHelper.ToString(buffer, value);
            Console.WriteLine($"C second cost:{w.ElapsedMilliseconds}");

            Console.WriteLine(*(buffer + 0));
            Console.WriteLine(*(buffer + 1));
            Console.WriteLine(*(buffer + 2));

            //Console.WriteLine(value.ToString() == new string(buffer, 0, length));
        }

    }
}
