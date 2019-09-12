
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using ProtoBuf;
using System.IO;
using System.Diagnostics;
using JShibo.Serialization.Soc;
using JShibo.Serialization.Json;
using JShibo.Serialization.BenchMark.Entitiy;
using MessagePack;

namespace JShibo.Serialization.BenchMark
{
    public class SocTester : TestConfig
    {
        #region 字段

        //static byte[] socBuffer = new byte[64];
        //static char[] cbuffer = new char[128];
        //static int capacity = 256;
        //static int defaultSize = 500;
        //static bool isPub = false;
        //static bool isString = true;
        //static int testCount = 1000000;
        //static bool toString = true;
        //static bool isInfo = false;
        //static bool isConsole = false;
        //static SerializerSettings sets = new SerializerSettings();

        #endregion

        public static TestResult Test(object graph)
        {
            TestResult result = new TestResult();
            while (true)
            {
                #region 参数

                int len = testCount;
                JsonStringContext info = ShiboSerializer.GetJsonStringTypeInfos(graph.GetType());

                JsonSerializerSettings nset = new JsonSerializerSettings();
                //nset.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;

                BinaryFormatter binserializer = new BinaryFormatter();
                //var serializer = MessagePackSerializer.Create(graph.GetType());
                MemoryStream msBin = new MemoryStream();
                MemoryStream socStream = new MemoryStream();
                MemoryStream msgStream = new MemoryStream();
                MemoryStream protopufStream = new MemoryStream();

                #endregion

                #region 启动

                int size = 0;
                string s = string.Empty;
                ObjectStream socOStream = new ObjectStream(socStream);

                //ShiboSerializer.BinSerialize(os, graph, sets);
                //ShiboSerializer.Serialize(graph);
                //ShiboSerializer.BinSerialize(graph);

                if (TestBaseConfig.Fastest)
                    Console.WriteLine(ShiboSerializer.Serialize(graph));

                if (TestBaseConfig.Soc)
                    ShiboSerializer.BinarySerialize(graph);

                //if (TestBaseConfig.MsgPack)
                //    serializer.PackSingleObject(graph);

                if (TestBaseConfig.SocStream)
                    ShiboSerializer.BinarySerialize(socOStream, graph);

                if (TestBaseConfig.Newtonsoft)
                    JsonConvert.SerializeObject(graph);

                if (TestBaseConfig.BinaryFormatter)
                    binserializer.Serialize(msBin, graph);

                if (TestBaseConfig.ServiceStack)
                    ServiceStack.Text.JsonSerializer.SerializeToString(graph);

                //if (TestBaseConfig.JavaScriptSerializer)
                //    serializer.Serialize(graph);

                //if (TestBaseConfig.DataContractJsonSerializer)
                //    JsonSerializer(ser, graph);

                #endregion

                #region 测试

                Stopwatch w = System.Diagnostics.Stopwatch.StartNew();
                if (TestBaseConfig.Soc)
                {
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        //byte[] bytes = ShiboSerializer.BinSerialize(graph);
                        //size += bytes.Length;

                        ObjectBuffer stream = new ObjectBuffer(socBuffer);
                        ShiboSerializer.BinarySerialize(stream, graph);
                        size += stream.Position;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time Soc Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }

                if (TestBaseConfig.MsgPack)
                {
                    size = 0;
                    w.Restart();
                    //for (int i = 0; i < len; i++)
                    //{
                    //    byte[] bytes = serializer.PackSingleObject(graph);
                    //    size += bytes.Length;
                    //}
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time MsgPack Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }

                if (TestBaseConfig.ProtoBuf)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        //protopufStream.Position = 0;
                        protopufStream = new MemoryStream(100);
                        Serializer.Serialize(protopufStream, graph);
                        size += (int)protopufStream.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time ProtoBuf Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }


                if (TestBaseConfig.Fastest)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        JsonString stream = new JsonString(jsonBuffer);
                        ShiboSerializer.Serialize(stream, graph);
                        size += stream.Position;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time Fastest Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }

                if (TestBaseConfig.Newtonsoft)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        string json = JsonConvert.SerializeObject(graph);
                        size += json.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time Newtonsoft Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }

                if (TestBaseConfig.ServiceStack)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        string json = ServiceStack.Text.JsonSerializer.SerializeToString(graph);
                        size += json.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time ServiceStack Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }


                if (TestBaseConfig.BinaryFormatter)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        MemoryStream stream = new MemoryStream();
                        binserializer.Serialize(stream, graph);
                        size += (int)stream.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time BinaryFormatter Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }

                #endregion

                break;
            }
            return result;
        }

        public static void TestCase1()
        {
            //Guid guid = new Guid("e92b8e30-a6e5-41f6-a6b9-188230a23dd2");
            //byte[] buffer = guid.ToByteArray();
            //Console.WriteLine(BitConverter.ToString(buffer));
            Dictionary<int, string> v = new Dictionary<int, string>();
            v.Add(1, null);

            TestBaseConfig.Seed = 1;
            Int32Class a = ShiboSerializer.Initialize<Int32Class>();// Int32Class.Init();
            string s1 = ShiboSerializer.Serialize(a);
            string s2 = null;
            byte[] buffer = ShiboSerializer.BinarySerialize(a);
            a = ShiboSerializer.BinaryDeserialize<Int32Class>(buffer);
            s2 = ShiboSerializer.Serialize(a);
            Console.WriteLine(BitConverter.ToString(buffer));
            Console.WriteLine(s1 == s2);
            //Console.ReadLine();
            Test(a);

            //Int32FieldClass a = Int32FieldClass.Init();
            //byte[] buffer = ShiboSerializer.BinSerialize(a);
            ////byte[] b = new byte[400];
            ////Buffer.BlockCopy(buffer, 0, b, 0, 4);
            //a = ShiboSerializer.BinDeserialize<Int32FieldClass>(buffer);
            //Console.WriteLine(BitConverter.ToString(buffer));
            //Console.WriteLine(ShiboSerializer.Serialize(a));
            Console.ReadLine();

        }

        public static void TestCase2()
        {
            TestBaseConfig.Seed = 1;
            Int32Class a = ShiboSerializer.Initialize<Int32Class>(); //Int32Class.Init();
            byte[] buffer = ShiboSerializer.BinarySerialize(a);
            Int32Class b = ShiboSerializer.BinaryDeserialize<Int32Class>(buffer);
            Stopwatch w = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                buffer = ShiboSerializer.BinarySerialize(a);
                //b = ShiboSerializer.BinDeserialize<Int32Class>(buffer);
            }
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds);
            Console.WriteLine(JsonConvert.SerializeObject(a) == JsonConvert.SerializeObject(b));
        }

        public static void TestCase3()
        {
            TestBaseConfig.Seed = 1;
            MixClass a = MixClass.Init();
            byte[] buffer = ShiboSerializer.BinarySerialize(a);
            MixClass b = ShiboSerializer.BinaryDeserialize<MixClass>(buffer);
            Stopwatch w = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                //buffer = ShiboSerializer.BinSerialize(a);
                b = ShiboSerializer.BinaryDeserialize<MixClass>(buffer);
            }
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds);
            Console.WriteLine(JsonConvert.SerializeObject(a) == JsonConvert.SerializeObject(b));
        }

        public static void TestCase4()
        {
            TestBaseConfig.Seed = 1;
            MixClass a = MixClass.Init();
            byte[] buffer = ShiboSerializer.BinarySerialize(a);
            MixClass b = ShiboSerializer.BinaryDeserialize<MixClass>(buffer);
            Stopwatch w = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                buffer = ShiboSerializer.BinarySerialize(a);
                //b = ShiboSerializer.BinDeserialize<MixClass>(buffer);
            }
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds);
            Console.WriteLine(JsonConvert.SerializeObject(a) == JsonConvert.SerializeObject(b));
        }

        public static void TestCase5()
        {
            TestBaseConfig.Seed = 1;
            MixClass a = MixClass.Init();
            //MixClass a = new MixClass();
            byte[] buffer = ShiboSerializer.BinarySerialize(a);
            MixClass b = ShiboSerializer.BinaryDeserialize<MixClass>(buffer);
            Stopwatch w = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                //buffer = ShiboSerializer.BinSerialize(a);
                a = ShiboSerializer.BinaryDeserialize<MixClass>(buffer);
            }
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds);
            Console.WriteLine(JsonConvert.SerializeObject(a));
            //Console.WriteLine(JsonConvert.SerializeObject(a) == JsonConvert.SerializeObject(b));
        }

        public static void TestCase6()
        {
            TestBaseConfig.Seed = 1;
            ArraySegmentClass a = new ArraySegmentClass();
            byte[] buffer = ShiboSerializer.BinarySerialize(a);
            Stopwatch w = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                buffer = ShiboSerializer.BinarySerialize(a);
            }
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds);
            Console.WriteLine(buffer);
            Console.WriteLine(JsonConvert.SerializeObject(a));
        }

        /// <summary>
        /// 对象数据初始化测试
        /// </summary>
        public static void TestCase7()
        {
            TestBaseConfig.Seed = 1;
            MixClass a = ShiboSerializer.Initialize<MixClass>();            
            Stopwatch w = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                a = ShiboSerializer.Initialize<MixClass>();
            }
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds);
            Console.WriteLine(JsonConvert.SerializeObject(a));
        }

        public static void TestCase8()
        {
            TestBaseConfig.Seed = 1;
            MixClass a = ShiboSerializer.Initialize<MixClass>(TestBaseConfig.Seed);
            //a.V8 = "sdfdfg";
            //a.V5.V3 = 1;
            MixClass b = ShiboSerializer.Initialize<MixClass>(TestBaseConfig.Seed);
            //b.V8 = "sdfdfg";
            //Assert.AreEqual(
            //Console.WriteLine(ShiboComparer.Compare(a, b));
        }

        public static void TestCase9()
        {
            TestBaseConfig.Seed = 1;
            int[] ints = new int[1000];
            new FastRandom().NextInts(ints, 0, ints.Length);
            byte[] bytes = ShiboSerializer.BinarySerialize(ints);
            int[] b = ShiboSerializer.BinaryDeserialize<int[]>(bytes);

            //int ints = 45456;
            //byte[] bytes = ShiboSerializer.BinarySerialize(ints);
            //int b = ShiboSerializer.BinaryDeserialize<int>(bytes);

            Stopwatch w = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                bytes = ShiboSerializer.BinarySerialize(ints);
            }
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds);
            Console.WriteLine(JsonConvert.SerializeObject(b));
        }

        /// <summary>
        /// 
        /// </summary>
        public static void TestCase10()
        {
            TestBaseConfig.Seed = 1;
            Dictionary<int, bool> a = ShiboSerializer.Initialize<Dictionary<int, bool>>(53456158);
            byte[] bytes = ShiboSerializer.BinarySerialize(a);
            Dictionary<int, bool> b = ShiboSerializer.BinaryDeserialize<Dictionary<int, bool>>(bytes);

            Stopwatch w = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                //bytes = ShiboSerializer.BinarySerialize(a);
                b = ShiboSerializer.BinaryDeserialize<Dictionary<int, bool>>(bytes);
            }
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds);
            Console.WriteLine(JsonConvert.SerializeObject(b.Count));
        }

        public static void TestCase11()
        {
            TestBaseConfig.Seed = 1;
            List<string> a = ShiboSerializer.Initialize<List<string>>(53456158);
            byte[] bytes = ShiboSerializer.BinarySerialize(a);
            List<string> b = ShiboSerializer.BinaryDeserialize<List<string>>(bytes);
            //Console.WriteLine(ShiboComparer.Compare<List<string>>(a, b));
            Console.WriteLine(JsonConvert.SerializeObject(a) == JsonConvert.SerializeObject(b));

            //Stopwatch w = Stopwatch.StartNew();
            //for (int i = 0; i < 10000; i++)
            //{
            //    //bytes = ShiboSerializer.BinarySerialize(a);
            //    b = ShiboSerializer.BinaryDeserialize<List<string>>(bytes);
            //}
            //w.Stop();
            //Console.WriteLine(w.ElapsedMilliseconds);
            //Console.WriteLine(JsonConvert.SerializeObject(b.Count));
        }

        public static void TestCase12()
        {
            TestBaseConfig.Seed = 1;
            IList<string> a = null;
            byte[] bytes = ShiboSerializer.BinarySerialize(a);
            //bytes = ShiboCompression.Compress(bytes);
            //ShiboDecompress de = new ShiboDecompress();
            //bytes = de.Decompress(bytes,0);
            //string[] b = ShiboSerializer.BinaryDeserialize<string[]>(bytes);
            ////Console.WriteLine(ShiboComparer.Compare<List<string>>(a, b));
            //Console.WriteLine(JsonConvert.SerializeObject(a) == JsonConvert.SerializeObject(b));

            //Stopwatch w = Stopwatch.StartNew();
            //for (int i = 0; i < 10000; i++)
            //{
            //    //bytes = ShiboSerializer.BinarySerialize(a);
            //    b = ShiboSerializer.BinaryDeserialize<List<string>>(bytes);
            //}
            //w.Stop();
            //Console.WriteLine(w.ElapsedMilliseconds);
            //Console.WriteLine(JsonConvert.SerializeObject(b.Count));
        }

        public static void TestCase13()
        {
            TestBaseConfig.Seed = 1;
            byte[][] a = ShiboSerializer.Initialize<byte[][]>(53456158);
            byte[] bytes = ShiboSerializer.BinarySerialize(a);
            byte[][] b = ShiboSerializer.BinaryDeserialize<byte[][]>(bytes);
            //Console.WriteLine(ShiboComparer.Compare<List<string>>(a, b));
            Console.WriteLine(JsonConvert.SerializeObject(a) == JsonConvert.SerializeObject(b));

            //Stopwatch w = Stopwatch.StartNew();
            //for (int i = 0; i < 10000; i++)
            //{
            //    //bytes = ShiboSerializer.BinarySerialize(a);
            //    b = ShiboSerializer.BinaryDeserialize<List<string>>(bytes);
            //}
            //w.Stop();
            //Console.WriteLine(w.ElapsedMilliseconds);
            //Console.WriteLine(JsonConvert.SerializeObject(b.Count));
        }

        public static void TestCase14()
        {
            TestBaseConfig.Seed = 1;
            Int32Class a = ShiboSerializer.Initialize<Int32Class>(53456158);
            byte[] bytes = ShiboSerializer.BinarySerialize(a);
            Int32Class b = ShiboSerializer.BinaryDeserialize<Int32Class>(bytes);
            //Console.WriteLine(ShiboComparer.Compare<List<string>>(a, b));
            Console.WriteLine(JsonConvert.SerializeObject(a) == JsonConvert.SerializeObject(b));

            ObjectBuffer bf = new ObjectBuffer(50);
            Stopwatch w = Stopwatch.StartNew();
            for (int i = 0; i < 1000000; i++)
            {
                //bytes = ShiboSerializer.BinarySerialize(a);
                //b = ShiboSerializer.BinaryDeserialize<List<string>>(bytes);
                bf.Reset();
                ShiboSerializer.BinarySerialize(bf, a);
            }
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds);
            //Console.WriteLine(JsonConvert.SerializeObject(b.Count));
        }

        public static void A()
        {
            ObjectBufferContext context = SocSerializer.Create<ClubJsonCase>();
            context.Serialize("");
        }

        public static void Run()
        {
            //TestCase1();
            //TestCase2();
            //TestCase3();
            //TestCase4();
            //TestCase5();
            //TestCase6();
            //TestCase7();
            //TestCase8();
            //TestCase9();
            //TestCase10();
            //TestCase11();
            //TestCase12();
            //TestCase13();
            TestCase14();
        }



        
    }
}
