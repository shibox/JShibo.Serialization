using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization
{
    public class SerializeCodeGenerator
    {
        public static string Generator(Type type)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ObjectBuffer obf = new ObjectBuffer();");
            foreach (FieldInfo field in fields)
                sb.Append("obf.Write({0}.{1});");
            return sb.ToString();
        }



    }
}
