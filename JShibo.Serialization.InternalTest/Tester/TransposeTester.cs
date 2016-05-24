using JShibo.Serialization.BenchMark.Entitiy;
using JShibo.Serialization.Transpose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class TransposeTester
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

            //IList<PrimitiveTypeClass> v = new List<PrimitiveTypeClass>();
            //v.Add(new PrimitiveTypeClass());
            //v.Add(new PrimitiveTypeClass());

            Int32Class[] v = new Int32Class[2];
            v[0] = new Int32Class();
            v[1] = new Int32Class();

            //Int32Class v = Int32Class.Init();
            ColumnsResult result = ShiboSerializer.ToColumns(v);

            //CsvHelper.CsvWriter c = new CsvHelper.CsvWriter();


            Console.WriteLine(result);
            return null;
        }

    }

    public class PrimitiveTypeClass
    {
        public byte V0 { get; set; }
        
        public short V2 { get; set; }
        
        public int V4 { get; set; }

        public long V5 { get; set; }

        public int V6 { get; set; }
    }

    }
