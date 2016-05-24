
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using ProtoBuf;
using System.IO;
using System.Diagnostics;
using JShibo.Serialization;
using JShibo.Serialization.Soc;
using JShibo.Serialization.Json;
using MsgPack.Serialization;
using MsgPack;
using JShibo.Serialization.BenchMark.Entitiy;

namespace JShibo.Serialization.BenchMark
{
    public class SocTester
    {
        #region 字段

        static byte[] buffer = new byte[64];
        static char[] cbuffer = new char[128];
        static int capacity = 256;
        static int defaultSize = 500;
        static bool isPub = false;
        static bool isString = true;
        static int testCount = 1000000;
        static bool toString = true;
        static bool isInfo = false;
        static bool isConsole = false;
        static SerializerSettings sets = new SerializerSettings();

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
                var serializer = MessagePackSerializer.Create(graph.GetType());
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
                    ShiboSerializer.BinSerialize(graph);

                if(TestBaseConfig.MsgPack)
                    serializer.PackSingleObject(graph);

                if (TestBaseConfig.SocStream)
                    ShiboSerializer.BinSerialize(socOStream, graph);

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

                        ObjectBuffer stream = new ObjectBuffer(buffer);
                        ShiboSerializer.BinSerialize(stream, graph);
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
                    for (int i = 0; i < len; i++)
                    {
                        byte[] bytes = serializer.PackSingleObject(graph);
                        size += bytes.Length;
                    }
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
                        JsonString stream = new JsonString(cbuffer);
                        ShiboSerializer.Serialize( stream,graph);
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

        public static void Test()
        {
            //Guid guid = new Guid("e92b8e30-a6e5-41f6-a6b9-188230a23dd2");
            //byte[] buffer = guid.ToByteArray();
            //Console.WriteLine(BitConverter.ToString(buffer));
            Dictionary<int, string> v = new Dictionary<int, string>();
            v.Add(1, null);

            TestBaseConfig.Seed = 1;
            Int32Class a = Int32Class.Init();
            string s1 = ShiboSerializer.Serialize(a);
            string s2 = null;
            byte[] buffer = ShiboSerializer.BinSerialize(a);
            a = ShiboSerializer.BinDeserialize<Int32Class>(buffer);
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
    }
}
