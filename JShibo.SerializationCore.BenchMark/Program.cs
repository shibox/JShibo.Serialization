using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using ProtoBuf;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using JShibo.Serialization.Json;
using JShibo.Serialization.BenchMark.Entitiy;

namespace JShibo.Serialization.BenchMark
{
    class Program
    {
        static int capacity = 65536;
        static char[] buffer = new char[capacity];
        static int defaultSize = 500;
        static bool isPub = false;
        static bool isString = true;
        static int testCount = 100;
        static bool toString = true;
        static bool isInfo = false;
        static bool isConsole = false;
        static string conn = null;
        //static Northwind northwind = new Northwind(conn);
        static SerializerSettings sets = new SerializerSettings();

        static void Main(string[] args)
        {


            //TopQueueTester.TestCase1();
            //Heap.Run();
            //BaseTester.Run();
            JsonTester.Run();
            //SocTester.Run();
            //WebServerTester.Run();
            //RestRerviceTester.Run();
            //CsvTester.Test(null);
            //TransposeTester.Test(null);
            //TypeTester.Run();
            //FastToStringTests.Run();
            //RazorTester.Run();
            //ColumnConvertTest.Test1();
            //object o = NestTester.Search2("bmw");
            //Console.WriteLine(JsonConvert.SerializeObject(o, Formatting.Indented));


            //CsvTester.Test(null);
            //JsonTester.TestCase1();
            //JsonTester.TestCase2();
            //JsonTester.TestFrom("bigjson.txt");
            //JsonTester.TestCopy();
            //JsonString s = ShiboSerializer.SerializeToBuffer(StringClass.Init());
            //s = ShiboSerializer.SerializeToBuffer(StringClass.Init());
            //Console.WriteLine(s.ToString());
            //JsonTestNorthwind();
            //Console.ReadLine();

            //Console.WriteLine(ShiboSerializer.Deserialize<TestClass7>("{\"boolValue\":true}"));
            //Console.ReadLine();

            //Console.WriteLine(ShiboSerializer.Deserialize<int>("123"));
            //Console.ReadLine();

            //Console.WriteLine(ShiboSerializer.Deserialize<TestClass7>("{\"int32Value\":123}"));
            //Console.ReadLine();

            //int p = 0;
            //long v = FastToValue.ToInt64((long.MinValue.ToString()+"}").ToCharArray(), ref p);
            //Console.WriteLine(v);
            //Console.ReadLine();

            //IList<int> k = new int[] { 567, 678 };
            //Console.WriteLine(JsonConvert.SerializeObject(k));
            //k = JsonConvert.DeserializeObject<List<int>>(JsonConvert.SerializeObject(k));
            ////MessageBox.Show(ServiceStack.Text.JsonSerializer.SerializeToString(k));
            //Console.WriteLine(Fastest.Json.ShiboSerializer.Serialize(k));
            //Console.ReadLine();


            //sets.CamelCase = true;
            //sets.Escape = StringEscape.EscapeNonAscii;
            //TestClassAllType all = new TestClassAllType();

            //IDictionary<int, int> sp = new Dictionary<int, int>();
            //sp.Add(90, 78);

            //IDictionary<string, int> sp = new Dictionary<string, int>();
            //sp.Add("90", 78);
            //sp.Add("94", 78);

            //IDictionary<string, string> sp = new Dictionary<string, string>();
            //sp.Add("90", "gh");
            //sp.Add("94", "25");
            //object objectValue = new TestClass3();
            //ISet<int> s = new HashSet<int>();
            //s.Add(456);

            //ICollection<int> ss = new List<int>();
            //ss.Add(96);

            //IList ilistInt32 = new int[] { 457, 568 };

            //IList<int?> sp = new int?[] { 567, 678 };

            //JsonTest<TestClassAllType>();
            //JsonTest<TestClassAllValueType>();
            //JsonTest<int>();
            //JsonTest(45);
            //JsonTest(all.IListInt32);
            //JsonTest(all.ListInt32);
            //JsonTest(all.ListTClass);
            //JsonTest(all.ListObjectClass);
            //JsonTest(null);
            //JsonTest<TestClass6>();
            //JsonTest<TestClass4>();
            //JsonTest<TestClass3>();
            //JsonTest<OrdersQry>();
            //JsonTest(sp);
            //JsonTest(ss);
            //JsonTest(ilistInt32);
            //JsonTest(s);
            //JsonTest(objectValue);
            //JsonTest(all.Int32Array.ToList<int>());
            //JsonTest(all.Int32Array);
            //JsonTest((IEnumerable<int>)all.Int32Array);
            //JsonTest(new VVAA(123, 156));
            //JsonTest<colclass>();
            //JsonTest<TestClass7>();
            //IList<int> list = null;
            //JsonTest(list);
            //JsonTest(all.stringArray);
            //JsonTest(all.timeSpanValue);
            //JsonTest(all.uriValue);
            //JsonTest(int.MinValue+2);
            //JsonTest(long.MinValue+2);
            //JsonTest(all.byteArrayValue);

            //JsonTester.Test<ArraySegmentClass>();
            //JsonTester.Test(new ArraySegmentClass().V);
            //JsonTester.Test(new ValueTypeTestA(123,456));
            //JsonTester.Test(new ValueTypeTestB(new int[] { 45,78},6));
            //JsonTester.Test(new ValueTypeTestC(14, "abc"));
            //JsonTester.Test(new ValueTypeTestD(147894, 5));
            //JsonTester.Test(new ValueTypeTestE(new int[] { 45,78}));
            //JsonTester.Test(new ValueTypeTestF("abc"));
            //JsonTester.Test(new ValueTypeTestG("abc","def"));
            //JsonTester.Test(new ValueTypeTestH(12, "def"));

            //JsonTest(Int8Class.Init());
            //JsonTest(Int16Class.Init());
            //JsonTest(Int32Class.Init());
            //JsonTest(FloatClass.Init());
            //JsonTest(GuidClass.Init());
            //JsonTest(DateTimeClass.Init());
            //JsonTester.Test(StringClass.Init());

            //SocTester.TestCase1();

            //ShiboSerializer.BinSerialize(Int32Class.Init());
            //BinTester.Test(Int32Class.Init());
            //JsonTester.Test(Int32Class.Init());
            //JsonTester.Test(Int64Class.Init());
            //Fastest.Json.
            //Console.WriteLine(AutohomeContent.Test());
            //JsonTester.Test(Get(File.ReadAllText("json_format.txt")));

            //Console.WriteLine(AutohomeContent.Test());
            //JsonTester.Test(Get(File.ReadAllText("json_format.txt")));

            //DataTable table = new DataTable();
            //SqlConnection conn = null;
            //conn.Open();
            //SqlCommand cmd = new SqlCommand("select top 10 [id],[url],[referrer], body from [Autohome_ContentDb0]", conn);
            ////SqlCommand cmd = new SqlCommand("select top 10 [id],[url],[referrer], body from [Autohome_ContentDb0]", conn);
            ////SqlCommand cmd = new SqlCommand("select top 10 [url],[referrer], body from [Autohome_ContentDb0]", conn);
            ////SqlCommand cmd = new SqlCommand("select top 10 [id] from [Autohome_ContentDb0]", conn);
            //SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //sda.Fill(table);
            //conn.Close();

            //List<string> list = new List<string>();
            //foreach (DataRow row in table.Rows)
            //    list.Add(row["body"].ToString());
            //JsonTester.Test(list.ToArray());
            //JsonTester.Test(table);
            //rtb_Response.Text = JsonConvert.SerializeObject(table);

            //TTTT();

            //VVAA? va = null;
            //JsonTest(va);
            //JsonStream stream = new JsonStream();
            //VVAA va = new VVAA(1, 1);
            //W(stream, va.A);
            //W(stream, va.B);
            //foreach (FieldInfo field in va.GetType().GetFields())
            //{
            //    object v = field.GetValue(va);
            //    W(stream, (int)v);
            //}



            Console.ReadLine();
        }

        public static void JsonTest(object graph)
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
                //JavaScriptSerializer serializer = new JavaScriptSerializer();

                int v = 0;
                string s = string.Empty;
                if (isString)
                {
                    JsonString os = new JsonString(buffer);
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

                    //Console.WriteLine(serializer.Serialize(graph));
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

                    //serializer.Serialize(graph);

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

                //Console.WriteLine();
                //if (s != fastJSON.JSON.Instance.ToJSON(graph, fp))
                //    Console.WriteLine("结果不一样，有错误！   ----fastJSON");
                //else
                //    Console.WriteLine("和 fastJSON 正确");

                //if (s.Length != fastJSON.JSON.Instance.ToJSON(graph, fp).Length)
                //    Console.WriteLine("长度不一样，有错误！   ----fastJSON");
                //else
                //    Console.WriteLine("和 fastJSON 长度正确");

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
                            JsonString os = new JsonString(buffer);
                            ShiboSerializer.Serialize(os, graph, info, sets);
                            if (toString == false)
                                v += os.Position;
                            else
                                v += os.ToString().Length;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < len; i++)
                        {
                            JsonString os = new JsonString(buffer);
                            ShiboSerializer.Serialize(os, graph, sets);
                            if (toString == false)
                                v += os.Position;
                            else
                                v += os.ToString().Length;
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
                        //    v += os.Position;
                        //else
                        //    v += os.ToString().Length;
                    }
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time Fastest.Json serializer= " + w.ElapsedMilliseconds + "    v:" + v);

                v = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    string st = Newtonsoft.Json.JsonConvert.SerializeObject(graph, nset);
                    v += st.Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time JsonConvert serializer= " + w.ElapsedMilliseconds + "    v:" + v);

                if (isPub)
                {
                    v = 0;
                    w.Restart();
                    for (int i = 0; i < len; i++)
                    {
                        msPub = new MemoryStream();
                        Serializer.Serialize(msPub, graph);
                        v += (int)msPub.Length;
                    }
                    w.Stop();
                    Console.WriteLine();
                    Console.WriteLine("Time Protocol Buffer serializer= " + w.ElapsedMilliseconds + "    v:" + v);
                }

                v = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    string st = ServiceStack.Text.JsonSerializer.SerializeToString(graph);
                    v += st.Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time ServiceStack serializer= " + w.ElapsedMilliseconds + "    v:" + v);

                v = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    //StringBuilder sb = new StringBuilder(120);
                    //sb.Append('[');
                    //string[] strs = (string[])graph;
                    //for (int n = 0; n < strs.Length; n++)
                    //{
                    //    sb.Append('"');

                    //    if (strs[n] == null)
                    //        sb.Append("null");
                    //    else if (strs[n].Length == 0)
                    //        sb.Append("\"\"");
                    //    else
                    //        sb.Append(strs[n]);

                    //    sb.Append('"');
                    //    sb.Append(',');
                    //}
                    //sb.Length--;
                    //sb.Append(']');
                    //v += sb.ToString().Length;

                    v += graph.ToString().Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time StringBuilder serializer= " + w.ElapsedMilliseconds + "    v:" + v);

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
                Console.WriteLine("Time BinaryFormatter serializer= " + w.ElapsedMilliseconds + "    v:" + v);

                //v = 0;
                //w.Restart();
                //for (int i = 0; i < len; i++)
                //{
                //    string st = serializer.Serialize(graph);
                //    v += st.Length;
                //}
                //w.Stop();
                //Console.WriteLine();
                //Console.WriteLine("Time JavaScriptSerializer serializer= " + w.ElapsedMilliseconds + "    v:" + v);

                v = 0;
                w.Restart();
                for (int i = 0; i < len; i++)
                {
                    string st = JsonSerializer(ser, graph);
                    v += st.Length;
                }
                w.Stop();
                Console.WriteLine();
                Console.WriteLine("Time DataContractJsonSerializer serializer= " + w.ElapsedMilliseconds + "    v:" + v);

                break;
            }
        }

        public static void JsonTestNorthwind()
        {
            //OrdersQry ordersQry = null;
            //Table<OrdersQry> ordersQries = northwind.OrdersQries;
            //foreach (OrdersQry temp in ordersQries)
            //{
            //    ordersQry = temp;
            //    break;
            //}

            ////OrderDetail ordersQry = null;
            ////Table<OrderDetail> ordersQries = northwind.OrderDetails;
            ////foreach (OrderDetail temp in ordersQries)
            ////{
            ////    ordersQry = temp;
            ////    break;
            ////}
            ////JsonConvert.SerializeObject(ordersQry);


            //JsonTest(ordersQry);
            //Console.ReadLine();
        }

        public static string JsonSerializer(DataContractJsonSerializer ser, object graph)
        {
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, graph);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

       







        public static void TTTT()
        {
            var jsonResult = new JsonResult() { result = GetObject(), message = "成功", returncode = 0 };
            var json = JsonSerializer(jsonResult);
            serialize(jsonResult, 100000);
            Console.WriteLine();
            //deserialize(jsonResult, 1000);
            Console.Read();
        }

        public static School GetObject()
        {
            School s = new School() { name = "北京一零一中", address = "北京市海淀区圆明园遗址", phone = "0108888666", classitems = new List<ClassItems>() };
            Random r = new Random();
            for (int i = 0; i < 1; i++)
            {
                ClassItems c = new ClassItems() { grade = string.Format("高中{0}年级", i.ToString()), students = new List<Student>(), teachers = new List<Teacher>() };
                for (int j = 0; j < 1; j++)
                {
                    c.teachers.Add(new Teacher() { id = i * 10 + j, name = "教师" + (i * 10 + j), age = r.Next(18, 60), course = "课程" + j.ToString(), sex = (sbyte)r.Next(0, 1), introduce = "北京101中学教师" + (i * 10 + j) });
                }
                for (int j = 0; j < 1; j++)
                {
                    c.students.Add(new Student() { id = i * 10 + j, name = "教师" + (i * 10 + j), age = r.Next(14, 20), sex = (sbyte)r.Next(0, 1), introduce = "北京101中学学生" + (i * 10 + j), fatherName = "父亲" + (i * 10 + j), motherName = "母亲" + (i * 10 + j) });
                }
                s.classitems.Add(c);
            }

            s.Set();
            return s;
        }

        private static void serialize(JsonResult result, int count)
        {
            string s = "";
            JsonStringContext info = ShiboSerializer.GetJsonStringTypeInfos(result.GetType());
            Console.WriteLine("序列化开始({0}),单位毫秒", count);
            System.Diagnostics.Stopwatch w = new System.Diagnostics.Stopwatch();
            Console.WriteLine("Fastest.Json");
            for (int i = 0; i < 5; i++)
            {
                w.Start();
                for (int j = 0; j < count; j++)
                {
                    JsonString stream = new JsonString(defaultSize);
                    if (isInfo)
                        ShiboSerializer.Serialize(stream, result, info, sets);
                    else
                        ShiboSerializer.Serialize(stream, result, sets);
                    s = stream.ToString();
                }
                w.Stop();
                Console.Write("{0}      ", (w.ElapsedMilliseconds / (count * 1.0)).ToString());
                w.Reset();
            }
            Console.WriteLine(s);

            Console.WriteLine("\r\nServiceStack.Text");
            for (int i = 0; i < 5; i++)
            {
                w.Start();
                for (int j = 0; j < count; j++)
                {
                    s = ServiceStack.Text.JsonSerializer.SerializeToString<JsonResult>(result);
                }
                w.Stop();
                Console.Write("{0}      ", (w.ElapsedMilliseconds / (count * 1.0)).ToString());
                w.Reset();
            }
            Console.WriteLine(s);

            Console.WriteLine("\r\nNewtonsoft.Json");
            for (int i = 0; i < 5; i++)
            {
                w.Start();
                for (int j = 0; j < count; j++)
                {
                    s = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                }
                w.Stop();
                Console.Write("{0}      ", (w.ElapsedMilliseconds / (count * 1.0)).ToString());
                w.Reset();
            }
            Console.WriteLine(s);

            //Console.WriteLine("\r\nJavaScriptSerializer");
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //for (int i = 0; i < 5; i++)
            //{
            //    w.Start();
            //    for (int j = 0; j < count; j++)
            //    {
            //        s = serializer.Serialize(result);
            //    }
            //    w.Stop();
            //    Console.Write("{0}      ", (w.ElapsedMilliseconds / (count * 1.0)).ToString());
            //    w.Reset();
            //}
            //Console.WriteLine(s);

            Console.WriteLine("\r\nDataContractJsonSerializer");
            for (int i = 0; i < 5; i++)
            {
                w.Start();
                for (int j = 0; j < count; j++)
                {
                    s = JsonSerializer(result);
                }
                w.Stop();
                Console.Write("{0}      ", (w.ElapsedMilliseconds / (count * 1.0)).ToString());
                w.Reset();
            }
            Console.WriteLine(s);
        }

        private static void deserialize(JsonResult result, int count)
        {
            Console.WriteLine("反序列化开始,单位毫秒");
            System.Diagnostics.Stopwatch w = new System.Diagnostics.Stopwatch();
            Console.WriteLine("fastJson");
            string json = null;
            //var json = fastJSON.JSON.Instance.ToJSON(result);
            //for (int i = 0; i < 5; i++)
            //{
            //    w.Start();
            //    for (int j = 0; j < count; j++)
            //    {
            //        JsonResult r = fastJSON.JSON.Instance.ToObject<JsonResult>(json);
            //    }
            //    w.Stop();
            //    Console.Write("{0}      ", (w.ElapsedMilliseconds / (count * 1.0)).ToString());
            //    w.Reset();
            //}

            Console.WriteLine("\r\nServiceStack.Text");
            for (int i = 0; i < 5; i++)
            {
                w.Start();
                for (int j = 0; j < count; j++)
                {
                    JsonResult r = ServiceStack.Text.JsonSerializer.DeserializeFromString<JsonResult>(json);
                }
                w.Stop();
                Console.Write("{0}      ", (w.ElapsedMilliseconds / (count * 1.0)).ToString());
                w.Reset();
            }

            Console.WriteLine("\r\nNewtonsoft.Json");
            for (int i = 0; i < 5; i++)
            {
                w.Start();
                for (int j = 0; j < count; j++)
                {
                    JsonResult r = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonResult>(json);
                }
                w.Stop();
                Console.Write("{0}      ", (w.ElapsedMilliseconds / (count * 1.0)).ToString());
                w.Reset();
            }

            //Console.WriteLine("\r\nJavaScriptSerializer");
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //for (int i = 0; i < 5; i++)
            //{
            //    w.Start();
            //    for (int j = 0; j < count; j++)
            //    {
            //        JsonResult r = serializer.Deserialize<JsonResult>(json);
            //    }
            //    w.Stop();
            //    Console.Write("{0}      ", (w.ElapsedMilliseconds / (count * 1.0)).ToString());
            //    w.Reset();
            //}

            Console.WriteLine("\r\nDataContractJsonSerializer");
            json = JsonSerializer(result);
            for (int i = 0; i < 5; i++)
            {
                w.Start();
                for (int j = 0; j < count; j++)
                {
                    JsonResult r = JsonDeserialize<JsonResult>(json);
                    //JsonSerializer(result);
                }
                w.Stop();
                Console.Write("{0}      ", (w.ElapsedMilliseconds / (count * 1.0)).ToString());
                w.Reset();
            }
        }

        public static string JsonSerializer<T>(T t)
        {

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            MemoryStream ms = new MemoryStream();

            ser.WriteObject(ms, t);

            string jsonString = Encoding.UTF8.GetString(ms.ToArray());

            ms.Close();

            return jsonString;
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            T obj = (T)ser.ReadObject(ms);

            return obj;

        }

        public static AutoConfig Get(string json)
        {
            return JsonConvert.DeserializeObject<AutoConfig>(json);
        }
    }
}
