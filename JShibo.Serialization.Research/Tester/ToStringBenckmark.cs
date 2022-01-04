using JShibo.Serialization.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace JShibo.Serialization.BenchMark.Tester
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
            TestDig();
            return;

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
                length = value.ToString().Length;
            Console.WriteLine($"D first cost:{w.ElapsedMilliseconds}");

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

            w = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                length = value.ToString().Length;
            Console.WriteLine($"D second cost:{w.ElapsedMilliseconds}");

            Console.WriteLine(*(buffer + 0));
            Console.WriteLine(*(buffer + 1));
            Console.WriteLine(*(buffer + 2));

            //Console.WriteLine(value.ToString() == new string(buffer, 0, length));
        }

        public unsafe static void TestDig()
        {
            var json = "{\"V0\":1528956021,\"V1\":1441128801,\"V2\":2091397125,\"V3\":967487988,\"V4\":1918901016,\"V5\":150209244,\"V6\":903947231,\"V7\":1504182662,\"V8\":736262512,\"V9\":52984882}";
            var o = JsonConvert.DeserializeObject<RootClass>(json);
            var w = Stopwatch.StartNew();
            var buffer = stackalloc char[300];
            int len = 0;
            var ch = "-V0-:,".ToCharArray();
            fixed (char* p = &ch[0])
            {
                for (int i = 0; i < 100000; i++)
                {
                    var l = NumberHelper.ToString(o.V0, buffer);
                    //FastWriteName.WriteUnsafe(p, buffer, 6);
                    len += l + 6;
                    buffer += l + 6;
                    l = NumberHelper.ToString(o.V1, buffer);
                    //FastWriteName.WriteUnsafe(p, buffer, 6);
                    len += l + 6;
                    buffer += l + 6;
                    l = NumberHelper.ToString(o.V2, buffer);
                    //FastWriteName.WriteUnsafe(p, buffer, 6);
                    len += l + 6;
                    buffer += l + 6;
                    l = NumberHelper.ToString(o.V3, buffer);
                    //FastWriteName.WriteUnsafe(p, buffer, 6);
                    len += l + 6;
                    buffer += l + 6;
                    l = NumberHelper.ToString(o.V4, buffer);
                    //FastWriteName.WriteUnsafe(p, buffer, 6);
                    len += l + 6;
                    buffer += l + 6;
                    l = NumberHelper.ToString(o.V5, buffer);
                    //FastWriteName.WriteUnsafe(p, buffer, 6);
                    len += l + 6;
                    buffer += l + 6;
                    l = NumberHelper.ToString(o.V6, buffer);
                    //FastWriteName.WriteUnsafe(p, buffer, 6);
                    len += l + 6;
                    buffer += l + 6;
                    l = NumberHelper.ToString(o.V7, buffer);
                    //FastWriteName.WriteUnsafe(p, buffer, 6);
                    len += l + 6;
                    buffer += l + 6;
                    l = NumberHelper.ToString(o.V8, buffer);
                    //FastWriteName.WriteUnsafe(p, buffer, 6);
                    len += l + 6;
                    buffer += l + 6;
                    l = NumberHelper.ToString(o.V9, buffer);
                    //FastWriteName.WriteUnsafe(p, buffer, 6);
                    len += l + 6;
                    buffer += l + 6;

                    //var l = NumberHelper.ToString(o.V0, buffer);
                    //FastWriteName.WriteUnsafe(p, buffer, 6);
                    //len += l;
                    //buffer += l;
                    //l = NumberHelper.ToString(o.V1, buffer);
                    //len += l;
                    //buffer += l;
                    //l = NumberHelper.ToString(o.V2, buffer);
                    //len += l;
                    //buffer += l;
                    //l = NumberHelper.ToString(o.V3, buffer);
                    //len += l;
                    //buffer += l;
                    //l = NumberHelper.ToString(o.V4, buffer);
                    //len += l;
                    //buffer += l;
                    //l = NumberHelper.ToString(o.V5, buffer);
                    //len += l;
                    //buffer += l;
                    //l = NumberHelper.ToString(o.V6, buffer);
                    //len += l;
                    //buffer += l;
                    //l = NumberHelper.ToString(o.V7, buffer);
                    //len += l;
                    //buffer += l;
                    //l = NumberHelper.ToString(o.V8, buffer);
                    //len += l;
                    //buffer += l;
                    //l = NumberHelper.ToString(o.V9, buffer);
                    //len += l;
                    //buffer += l;
                    
                    buffer -= len;
                    Console.WriteLine(new string(buffer, 0, len));
                    len = 0;
                    //buffer += NumberHelper.ToString(o.V0, buffer);
                    //buffer += NumberHelper.ToString(o.V1, buffer);
                    //buffer += NumberHelper.ToString(o.V2, buffer);
                    //buffer += NumberHelper.ToString(o.V3, buffer);
                    //buffer += NumberHelper.ToString(o.V4, buffer);
                    //buffer += NumberHelper.ToString(o.V5, buffer);
                    //buffer += NumberHelper.ToString(o.V6, buffer);
                    //buffer += NumberHelper.ToString(o.V7, buffer);
                    //buffer += NumberHelper.ToString(o.V8, buffer);
                    //buffer += NumberHelper.ToString(o.V9, buffer);

                }
            }
            
            Console.WriteLine($"cost:{w.ElapsedMilliseconds}");
        }

        public class RootClass
        {
            public int V0 { get; set; }
            public int V1 { get; set; }
            public int V2 { get; set; }
            public int V3 { get; set; }
            public int V4 { get; set; }
            public int V5 { get; set; }
            public int V6 { get; set; }
            public int V7 { get; set; }
            public int V8 { get; set; }
            public int V9 { get; set; }
        }

    }
}
