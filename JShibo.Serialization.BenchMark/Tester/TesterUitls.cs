using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class TesterUitls
    {
        public static string JsonSerializer(DataContractJsonSerializer ser, object graph)
        {
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, graph);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
    }
}
