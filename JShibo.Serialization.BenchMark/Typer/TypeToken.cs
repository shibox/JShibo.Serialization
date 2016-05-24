using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark.Typer
{
    public class TypeToken
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsField { get; set; }

        public TypeToken()
        { 
        
        }

        public TypeToken(string name,Type type)
        {
            this.Name = name;
            this.Type = type;
            this.IsField = true;
        }

    }
}
