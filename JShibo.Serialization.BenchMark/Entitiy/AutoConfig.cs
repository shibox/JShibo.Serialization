using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.BenchMark.Entitiy
{
    [Serializable]
    public class AutoConfig
    {
        public string message { get; set; }
        public int returncode { get; set; }
        public result result { get; set; }
    }

    [Serializable]
    public class result
    {
        public int seriesid { get; set; }
        public paramtypeitem[] paramtypeitems { get; set; }
    }

    [Serializable]
    public class paramtypeitem
    {
        public string name { get; set; }
        public paramitem[] paramitems { get; set; }
    }

    [Serializable]
    public class paramitem
    {
        public string name { get; set; }
        public valueitem[] valueitems { get; set; }
    }

    [Serializable]
    public class valueitem
    {
        public int specid { get; set; }
        public string value { get; set; }
    }
}
