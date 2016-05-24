using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.BenchMark
{
    public class CsvTester
    {
        public static TestResult Test(object graph)
        {
            //ShiboSerializer.Serialize(os, graph, sets);

            //List<int> v = new List<int>();
            //v.Add(123);
            //v.Add(456);

            IList<bool> v = new List<bool>();
            v.Add(true);
            v.Add(false);
            
            //Int32Class v = Int32Class.Init();
            string csv = ShiboSerializer.ToCsv(v);
            Console.WriteLine(csv);
            return null;
        }
    }
}
