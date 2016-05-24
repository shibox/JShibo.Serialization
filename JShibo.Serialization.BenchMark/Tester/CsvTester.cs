using JShibo.Serialization.BenchMark.Entitiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class CsvTester
    {
        public static TestResult Test(object graph)
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

            Int32Class[] v = new Int32Class[2];
            v[0] = new Int32Class();
            v[1] = new Int32Class();

            //Int32Class v = Int32Class.Init();
            string csv = ShiboSerializer.ToCsv(v);

            //CsvHelper.CsvWriter c = new CsvHelper.CsvWriter();
            

            Console.WriteLine(csv);
            return null;
        }
    }
}
