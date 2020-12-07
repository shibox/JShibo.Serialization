using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark.Typer
{
    public class TypeMeta
    {
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
        public List<TypeToken> Tokens { get; set; }
        public bool IsSaveAssembly { get; set; }

        public TypeMeta()
        {
            Tokens = new List<TypeToken>();
            //IsSaveAssembly = true;
        }
    }
}
