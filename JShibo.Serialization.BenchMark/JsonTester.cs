using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using ProtoBuf;
using JShibo.Serialization;
using JShibo.Serialization.Json;
using JShibo.Serialization.BenchMark;
using JShibo.Serialization.Common;
using JShibo.Json.ClassGenerator;
using JShibo.Serialization.BenchMark.Entitiy;

namespace JShibo.Serialization.BenchMark
{
    public class JsonTester
    {
        #region 字段

        static int capacity = 1000000;
        static char[] buffer = new char[capacity];
        static bool isCapacity = true;
        static bool isBuffer = true;
        static int defaultSize = 500;
        static bool isPub = false;
        static bool isString = true;
        static int testCount = 100;
        static bool toString = false;
        static bool isInfo = false;
        static bool isConsole = true;
        static SerializerSettings sets = new SerializerSettings();

        #endregion

        #region 方法

        public static TestResult Test<T>() where T : new()
        {
            T graph = new T();
            return Test(graph);
        }

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

                DataContractJsonSerializer ser = new DataContractJsonSerializer(graph.GetType());
                BinaryFormatter binserializer = new BinaryFormatter();
                MemoryStream msBin = new MemoryStream();
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                #endregion

                #region 启动

                int size = 0;
                string s = string.Empty;
                JsonString os=null;
                if (isBuffer)
                    os = new JsonString(buffer);
                else
                    os = new JsonString();

                ShiboSerializer.Serialize(os, graph, sets);
                

                if (TestBaseConfig.Newtonsoft)
                    Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset);

                if (TestBaseConfig.ServiceStack)
                    ServiceStack.Text.JsonSerializer.SerializeToString(graph);

                if (TestBaseConfig.BinaryFormatter)
                    binserializer.Serialize(msBin, graph);

                if (TestBaseConfig.JavaScriptSerializer)
                    serializer.Serialize(graph);

                if (TestBaseConfig.DataContractJsonSerializer)
                    JsonSerializer(ser, graph);

                #endregion

                #region 检查结果

                s = os.ToString();

                if (TestBaseConfig.Newtonsoft)
                {
                    Console.WriteLine();
                    if (s != Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset))
                        Console.WriteLine("may error  --Json.Net ");
                    else
                        Console.WriteLine("ok   --Json.Net");
                }

                if (TestBaseConfig.ServiceStack)
                {
                    Console.WriteLine();
                    if (s != ServiceStack.Text.JsonSerializer.SerializeToString(graph))
                        Console.WriteLine("may error   ----ServiceStack.Text");
                    else
                        Console.WriteLine("ok   --ServiceStack.Text");
                }

                if (TestBaseConfig.Fastjson)
                {
                    Console.WriteLine();
                    if (s != fastJSON.JSON.ToJSON(graph))
                        Console.WriteLine("may error   ----fastJSON");
                    else
                        Console.WriteLine("ok   --fastJSON");
                }
                

                #endregion

                #region 测试
                JsonStringContext context = ShiboSerializer.GetJsonStringTypeInfos(graph.GetType());
                Stopwatch w = System.Diagnostics.Stopwatch.StartNew();
                w.Start();
                for (int i = 0; i < len; i++)
                {
                    if (isBuffer)
                        os = new JsonString(buffer);
                    else
                        os = new JsonString();
                    if (isInfo)
                        ShiboSerializer.Serialize(os, graph, context, sets);
                    else
                        ShiboSerializer.Serialize(os, graph, sets);

                    //os = ShiboSerializer.SerializeToBuffer(graph);
                    if (toString)
                        size += os.ToString().Length;
                    else
                        size += os.Position;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time Fastest.Json Serializer= " + w.ElapsedMilliseconds + "    v:" + size);

                if (TestBaseConfig.Newtonsoft)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        string st = Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset);
                        size += st.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time JsonConvert Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }
                

                if (TestBaseConfig.ServiceStack)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        string st = ServiceStack.Text.JsonSerializer.SerializeToString(graph);
                        size += st.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time ServiceStack Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }

                if (TestBaseConfig.Fastjson)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        string st = fastJSON.JSON.ToJSON(graph);
                        size += st.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time fastJSON Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }
                


                //v = 0;
                //w.Restart();
                //for (int i = 0; i < len; i++)
                //{
                //    v += graph.ToString().Length;
                //}
                //w.Stop();
                //Console.WriteLine();
                //Console.WriteLine("Time StringBuilder Serializer= " + w.ElapsedMilliseconds + "    v:" + v);

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
                


                if (TestBaseConfig.JavaScriptSerializer)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        string st = serializer.Serialize(graph);
                        size += st.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time JavaScriptSerializer Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }
               

                if (TestBaseConfig.DataContractJsonSerializer)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        string st = JsonSerializer(ser, graph);
                        size += st.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time DataContractJsonSerializer Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }


                #endregion

                break;
            }
            return result;
        }

        public static TestResult TestFromFile(string file)
        {
            return Test(File.ReadAllText(file));
        }

        public static TestResult Test(string json)
        {
            TestResult result = new TestResult();
            result.Code = JsonClassGenerator.GenerateString(json, CodeLanguage.CSharp);
            Type type = TestUtils.Compile(result.Code,json,ref result);
            object graph = TestUtils.GetGraph(type, json);

            Took socTook = new Took();
            Took fastestJsonTook = new Took();
            Took newtonsoftTook = new Took();
            Took serviceStackTook = new Took();
            Took fastJsonTook = new Took();
            Took javaScriptTook = new Took();
            Took dataContractTook = new Took();
            Took binTook = new Took();
            Took protobufTook = new Took();
            Took msgPackTook = new Took();

            while (true)
            {
                #region 参数

                int len = testCount;
                JsonStringContext info = ShiboSerializer.GetJsonStringTypeInfos(graph.GetType());

                JsonSerializerSettings nset = new JsonSerializerSettings();
                //nset.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;

                DataContractJsonSerializer ser = new DataContractJsonSerializer(graph.GetType());
                BinaryFormatter binserializer = new BinaryFormatter();
                MemoryStream msBin = new MemoryStream();
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                #endregion

                #region 启动

                int size = 0;
                string s = string.Empty;
                JsonString os = null;
                if (isBuffer)
                    os = new JsonString(buffer);
                else
                    os = new JsonString();

                if (TestBaseConfig.Fastest)
                {
                    ShiboSerializer.Serialize(os, graph, sets);
                    fastestJsonTook.Json = os.ToString();
                }

                if (TestBaseConfig.Newtonsoft)
                    newtonsoftTook.Json = Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset);

                if (TestBaseConfig.ServiceStack)
                    serviceStackTook.Json = ServiceStack.Text.JsonSerializer.SerializeToString(graph);

                if (TestBaseConfig.BinaryFormatter)
                    binserializer.Serialize(msBin, graph);

                if (TestBaseConfig.JavaScriptSerializer)
                    javaScriptTook.Json = serializer.Serialize(graph);

                if (TestBaseConfig.DataContractJsonSerializer)
                    dataContractTook.Json = JsonSerializer(ser, graph);

                if (TestBaseConfig.Fastjson)
                    fastJsonTook.Json = fastJSON.JSON.ToJSON(graph);

                #endregion

                #region 检查结果

                s = os.ToString();

                if (TestBaseConfig.Newtonsoft)
                {
                    Console.WriteLine();
                    if (s != Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset))
                        Console.WriteLine("may error  --Json.Net ");
                    else
                        Console.WriteLine("ok   --Json.Net");
                }

                if (TestBaseConfig.ServiceStack)
                {
                    Console.WriteLine();
                    if (s != ServiceStack.Text.JsonSerializer.SerializeToString(graph))
                        Console.WriteLine("may error   ----ServiceStack.Text");
                    else
                        Console.WriteLine("ok   --ServiceStack.Text");
                }

                if (TestBaseConfig.Fastjson)
                {
                    Console.WriteLine();
                    if (s != fastJSON.JSON.ToJSON(graph))
                        Console.WriteLine("may error   ----fastJSON");
                    else
                        Console.WriteLine("ok   --fastJSON");
                }


                #endregion

                #region 测试
                JsonStringContext context = ShiboSerializer.GetJsonStringTypeInfos(graph.GetType());
                Stopwatch w = System.Diagnostics.Stopwatch.StartNew();
                w.Start();
                for (int i = 0; i < len; i++)
                {
                    if (isBuffer)
                        os = new JsonString(buffer);
                    else
                        os = new JsonString();
                    if (isInfo)
                        ShiboSerializer.Serialize(os, graph, context, sets);
                    else
                        ShiboSerializer.Serialize(os, graph, sets);

                    //os = ShiboSerializer.SerializeToBuffer(graph);
                    if (toString)
                        size += os.ToString().Length;
                    else
                        size += os.Position;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time Fastest.Json Serializer= " + w.ElapsedMilliseconds + "    v:" + size);

                if (TestBaseConfig.Newtonsoft)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        string st = Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset);
                        size += st.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time JsonConvert Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }


                if (TestBaseConfig.ServiceStack)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        string st = ServiceStack.Text.JsonSerializer.SerializeToString(graph);
                        size += st.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time ServiceStack Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }

                if (TestBaseConfig.Fastjson)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        string st = fastJSON.JSON.ToJSON(graph);
                        size += st.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time fastJSON Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }



                //v = 0;
                //w.Restart();
                //for (int i = 0; i < len; i++)
                //{
                //    v += graph.ToString().Length;
                //}
                //w.Stop();
                //Console.WriteLine();
                //Console.WriteLine("Time StringBuilder Serializer= " + w.ElapsedMilliseconds + "    v:" + v);

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



                if (TestBaseConfig.JavaScriptSerializer)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        string st = serializer.Serialize(graph);
                        size += st.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time JavaScriptSerializer Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }


                if (TestBaseConfig.DataContractJsonSerializer)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        string st = JsonSerializer(ser, graph);
                        size += st.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time DataContractJsonSerializer Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }


                #endregion

                break;
            }
            return result;
        }

        public static TestResult TestWithProto(object graph)
        {
            TestResult result = new TestResult();
            while (true)
            {
                #region 参数

                int len = testCount;
                JsonStringContext info = ShiboSerializer.GetJsonStringTypeInfos(graph.GetType());

                JsonSerializerSettings nset = new JsonSerializerSettings();
                //nset.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;

                DataContractJsonSerializer ser = new DataContractJsonSerializer(graph.GetType());
                BinaryFormatter binserializer = new BinaryFormatter();
                MemoryStream msBin = new MemoryStream();
                binserializer.Serialize(msBin, graph);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                #endregion

                #region 启动

                int v = 0;
                string s = string.Empty;
                JsonString os = new JsonString(capacity);
                ShiboSerializer.Serialize(os, graph, sets);
                s = os.ToString();

                MemoryStream msPub = new MemoryStream();

                Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset);
                if (isPub == true)
                    Serializer.Serialize(msPub, graph);
                ServiceStack.Text.JsonSerializer.SerializeToString(graph);
                serializer.Serialize(graph);
                JsonSerializer(ser, graph);

                #endregion

                #region 检查结果

                Console.WriteLine();
                if (s != Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset))
                    Console.WriteLine("may error  --Json.Net ");
                else
                    Console.WriteLine("ok   --Json.Net");

                Console.WriteLine();
                if (s != ServiceStack.Text.JsonSerializer.SerializeToString(graph))
                    Console.WriteLine("may error   ----ServiceStack.Text");
                else
                    Console.WriteLine("ok   --ServiceStack.Text");

                #endregion

                #region 测试

                Stopwatch w = System.Diagnostics.Stopwatch.StartNew();
                w.Start();
                for (int i = 0; i < len; i++)
                {
                    os = new JsonString(capacity);
                    ShiboSerializer.Serialize(os, graph, sets);
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time Fastest.Json Serializer= " + w.ElapsedMilliseconds + "    v:" + v);

                v = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    string st = Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset);
                    v += st.Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time JsonConvert Serializer= " + w.ElapsedMilliseconds + "    v:" + v);

                v = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    string st = ServiceStack.Text.JsonSerializer.SerializeToString(graph);
                    v += st.Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time ServiceStack Serializer= " + w.ElapsedMilliseconds + "    v:" + v);


                //v = 0;
                //w.Restart();
                //for (int i = 0; i < len; i++)
                //{
                //    v += graph.ToString().Length;
                //}
                //w.Stop();
                //Console.WriteLine();
                //Console.WriteLine("Time StringBuilder Serializer= " + w.ElapsedMilliseconds + "    v:" + v);

                v = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    MemoryStream stream = new MemoryStream();
                    binserializer.Serialize(stream, graph);
                    v += (int)stream.Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time BinaryFormatter Serializer= " + w.ElapsedMilliseconds + "    v:" + v);

                v = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    string st = serializer.Serialize(graph);
                    v += st.Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time JavaScriptSerializer Serializer= " + w.ElapsedMilliseconds + "    v:" + v);

                v = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    string st = JsonSerializer(ser, graph);
                    v += st.Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time DataContractJsonSerializer Serializer= " + w.ElapsedMilliseconds + "    v:" + v);


                #endregion

                break;
            }
            return result;
        }

        public static TestResult AdvancedTest(object graph)
        {
            while (true)
            {
                int len = testCount;
                JsonStringContext info = ShiboSerializer.GetJsonStringTypeInfos(graph.GetType());

                JsonSerializerSettings nset = new JsonSerializerSettings();
                //nset.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;

                DataContractJsonSerializer ser = new DataContractJsonSerializer(graph.GetType());
                BinaryFormatter binserializer = new BinaryFormatter();
                MemoryStream msBin = new MemoryStream();
                binserializer.Serialize(msBin, graph);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                int size = 0;
                string s = string.Empty;
                if (isString)
                {
                    JsonString os = new JsonString(capacity);
                    if (isInfo)
                        ShiboSerializer.Serialize(os, graph, info, sets);
                    else
                        ShiboSerializer.Serialize(os, graph, sets);
                    s = os.ToString();
                }
                else
                {
                    //JsonStream os = new JsonStream();
                    //ShiboSerializer.Serialize(os, graph, sets);
                    //s = os.ToString();
                }

                //fastJSON.JSONParameters fp = new fastJSON.JSONParameters() { UsingGlobalTypes = false, UseExtensions = false };
                MemoryStream msPub = new MemoryStream();


                if (isConsole)
                {
                    Console.WriteLine(s);
                    Console.WriteLine();

                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset));
                    Console.WriteLine();

                    if (isPub == true)
                    {
                        Serializer.Serialize(msPub, graph);
                        Console.WriteLine("PB:" + msPub.Length.ToString());
                        Console.WriteLine();
                    }

                    Console.WriteLine(ServiceStack.Text.JsonSerializer.SerializeToString(graph));
                    Console.WriteLine();

                    //Console.WriteLine(fastJSON.JSON.Instance.ToJSON(graph, fp));
                    //Console.WriteLine();

                    Console.WriteLine(serializer.Serialize(graph));
                    Console.WriteLine();

                    Console.WriteLine(JsonSerializer(ser, graph));
                    Console.WriteLine();
                }
                else
                {
                    Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset);

                    if (isPub == true)
                    {
                        Serializer.Serialize(msPub, graph);
                    }
                    ServiceStack.Text.JsonSerializer.SerializeToString(graph);
                    //fastJSON.JSON.Instance.ToJSON(graph, fp);

                    serializer.Serialize(graph);

                    JsonSerializer(ser, graph);
                }

                Console.WriteLine();
                if (s != Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset))
                    Console.WriteLine("may error  --Json.Net ");
                else
                    Console.WriteLine("ok   --Json.Net");

                Console.WriteLine();
                if (s != ServiceStack.Text.JsonSerializer.SerializeToString(graph))
                    Console.WriteLine("may error   ----ServiceStack.Text");
                else
                    Console.WriteLine("ok   --ServiceStack.Text");

                Stopwatch w = Stopwatch.StartNew();
                w.Start();

                w = System.Diagnostics.Stopwatch.StartNew();
                w.Start();
                if (isString)
                {
                    if (isInfo)
                    {
                        for (int i = 0; i < len; i++)
                        {
                            JsonString os = new JsonString(capacity);
                            ShiboSerializer.Serialize(os, graph, info, sets);
                            if (toString == false)
                                size += os.Position;
                            else
                                size += os.ToString().Length;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < len; i++)
                        {
                            JsonString os = new JsonString(capacity);
                            ShiboSerializer.Serialize(os, graph, sets);
                            if (toString == false)
                                size += os.Position;
                            else
                                size += os.ToString().Length;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < len; i++)
                    {
                        //JsonStream os = new JsonStream(capacity);
                        //ShiboSerializer.Serialize(os, graph, sets);
                        //if (toString == false)
                        //    size += os.Position;
                        //else
                        //    size += os.ToString().Length;
                    }
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time Fastest.Json Serializer= " + w.ElapsedMilliseconds + "    v:" + size);

                size = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    string st = Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset);
                    size += st.Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time JsonConvert Serializer= " + w.ElapsedMilliseconds + "    v:" + size);

                if (isPub)
                {
                    size = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        msPub = new MemoryStream();
                        Serializer.Serialize(msPub, graph);
                        size += (int)msPub.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time Protocol Buffer Serializer= " + w.ElapsedMilliseconds + "    v:" + size);
                }

                size = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    string st = ServiceStack.Text.JsonSerializer.SerializeToString(graph);
                    size += st.Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time ServiceStack Serializer= " + w.ElapsedMilliseconds + "    v:" + size);


                size = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    size += graph.ToString().Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time StringBuilder Serializer= " + w.ElapsedMilliseconds + "    v:" + size);

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

                size = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    string st = serializer.Serialize(graph);
                    size += st.Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time JavaScriptSerializer Serializer= " + w.ElapsedMilliseconds + "    v:" + size);

                size = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    string st = JsonSerializer(ser, graph);
                    size += st.Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time DataContractJsonSerializer Serializer= " + w.ElapsedMilliseconds + "    v:" + size);

                break;
            }

            return new TestResult();
        }

        private static string JsonSerializer(DataContractJsonSerializer ser, object graph)
        {
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, graph);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        #endregion

        #region 加载或生成数据

        public static void TestCase1()
        {
            string json = File.ReadAllText("bigjson.txt");
            ClubJsonCase o = JsonConvert.DeserializeObject<ClubJsonCase>(json);
            //HashSet<int> list = new HashSet<int>();
            //foreach (string s in o.Result.PicItems)
            //{
            //    if (list.Contains(s.Length) == false)
            //        list.Add(s.Length);
            //}

            Test(o);
        }

        #endregion

        #region 基础测试

        public unsafe static void TestCopy()
        {
            string s = "album/userphotos/2014/2/15/7a7f_71d8bd20_71d8bd20.jpghrfh";
            char[] buffer = new char[1000000];
            Stopwatch w = Stopwatch.StartNew();
            for (int j = 0; j < 1000; j++)
            {
                fixed (char* pb = &buffer[0])
                {
                    char* a = pb;
                    for (int i = 0; i < 10000; i++)
                    {
                        fixed (char* pd = s)
                        {
                            FastWriteName.WriteUnsafe(a, pd, s.Length);
                            //Utils.wstrcpy( a,pd, s.Length);
                            a += s.Length;
                        }
                    }
                }
            }
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds);
            
        }

        #endregion
    }
}
