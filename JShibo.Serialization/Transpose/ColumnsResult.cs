using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Transpose
{
    public class ColumnsResult
    {
        public Type[] Types { get; set; }
        public object[] Values { get; set; }
        public string[] Names { get; set; }
        public string TableName { get; set; }
    }
}
