using JShibo.Serialization.BenchMark.Entitiy;
using JShibo.Serialization.BenchMark.Typer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class TypeTester
    {
        //public static void TestCase1()
        //{
        //    //Type type = TestUtils.DynamicCreateType();
        //    //JsonTester.TestFrom("bigjson.txt");
        //    Type type = TypeCreater.Create(100); //TypeCreater.Create();
        //    //object o = Activator.CreateInstance(type,"aa");// ShiboSerializer.Initialize(type);
        //    //object o = Activator.CreateInstance(type);
        //    object o = ShiboSerializer.Initialize(type);
        //    Stopwatch w = Stopwatch.StartNew();
        //    //ShiboSerializer.SerializeObject(o);
        //    //JsonConvert.SerializeObject(o);
        //    Console.WriteLine(w.ElapsedMilliseconds);
        //    Console.WriteLine(JsonConvert.SerializeObject(o) == ShiboSerializer.SerializeObject(o));
        //    //Console.WriteLine(ShiboSerializer.SerializeObject(o));

        //    //Type type =  TypeCreater.Create(100);
        //    //Console.WriteLine(type);
        //}

        //public static void TestCase2()
        //{
        //    Stopwatch w = Stopwatch.StartNew();
        //    for (int i = 0; i < 1000000; i++)
        //    {
        //        Type type = TypeCreater.Create((i+1) % 100+1,i);
        //        object o = ShiboSerializer.Initialize(type,i);
                
        //        byte[] bytes = ShiboSerializer.BinarySerialize(o);
        //        object o1 = ShiboSerializer.BinaryDeserialize(bytes, o.GetType());
        //        string result = i + "   " + (JsonConvert.SerializeObject(o) == JsonConvert.SerializeObject(o1));
        //        if (result.IndexOf("True") == -1)
        //            File.AppendAllText("log.txt", result+"\r\n\r\n");
        //        Console.WriteLine(result);
        //        //Console.WriteLine(i + "   " + (ShiboSerializer.SerializeObject(o) == ShiboSerializer.SerializeObject(o1)));
        //        //Console.WriteLine(JsonConvert.SerializeObject(o).Length);
        //    }
        //    Console.WriteLine("完成,耗时：" + w.ElapsedMilliseconds);
        //}

        //public static void Run()
        //{
        //    //TestCase1();
        //    TestCase2();
        //}


    }
}
