using JShibo.Serialization.BenchMark.Entitiy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class CsvTester
    {
        public static TestResult Test(object graph)
        {
            //RunIntObjSeries();
            RunCsvArrayTest();
            return null;
        }

        public static void RunCsvArrayTest()
        {
            //ShiboSerializer.Serialize(os, graph, sets);

            //List<int> v = new List<int>();
            //v.Add(123);
            //v.Add(456);

            //IList<bool> v = new List<bool>();
            //v.Add(true);
            //v.Add(false);

            //IList<Int32Class> v = new List<Int32Class>();
            //v.Add(new Int32Class());
            //v.Add(new Int32Class());

            int n = 100;
            Int32Class[] v = new Int32Class[n];
            for (int i = 0; i < n; i++)
                v[i] = ShiboSerializer.Initialize<Int32Class>();
            

            //Int32Class v = Int32Class.Init();
            string csv = ShiboSerializer.ToCsv(v);

            //CsvHelper.CsvWriter c = new CsvHelper.CsvWriter();


            Console.WriteLine(csv);
        }

        public static void RunCsvListTest()
        {
            //ShiboSerializer.Serialize(os, graph, sets);

            //List<int> v = new List<int>();
            //v.Add(123);
            //v.Add(456);

            //IList<bool> v = new List<bool>();
            //v.Add(true);
            //v.Add(false);

            int n = 100;
            IList<Int32Class> v = new List<Int32Class>(n);
            for (int i = 0; i < n; i++)
                v.Add(ShiboSerializer.Initialize<Int32Class>());
            string csv = ShiboSerializer.ToCsv(v);

            //CsvHelper.CsvWriter c = new CsvHelper.CsvWriter();


            Console.WriteLine(csv);
        }

        public static void RunIntObjSeries()
        {
            //Random rd = new Random(1);
            ShiboRandom rd = new ShiboRandom(1);
            List<IXYV> floats = new List<IXYV>(100000);
            for (int i = 0; i < floats.Capacity; i++)
            {
                //floats.Add(new IXYV() { X = rd.Next() >> 15, Y = rd.Next() >> 15, V = rd.Next() >> 15 });
                floats.Add(new IXYV() { X = rd.Next(), Y = rd.Next(), V = rd.Next() });
            }
            string s = ShiboSerializer.ToCsv(floats);
            //string s = JsonConvert.SerializeObject(floats);
            Stopwatch w = Stopwatch.StartNew();

            for (int i = 0; i < 10; i++)
            {
                //s = JsonConvert.SerializeObject(floats);
                s = ShiboSerializer.ToCsv(floats);
                //CsvHelper.CsvSerializer csv = new CsvHelper.CsvSerializer(null);
                //CsvHelper.CsvWriter w = new CsvHelper.CsvWriter(null);
            }


            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds + " " + s.Length);
            Console.ReadLine();
        }

        public class IXYV
        {
            public int X;
            public int Y;
            public int V;
        }

    }
}
