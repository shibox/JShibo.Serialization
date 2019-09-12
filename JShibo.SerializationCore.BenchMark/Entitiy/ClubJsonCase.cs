using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace JShibo.Serialization.BenchMark.Entitiy
{
    public class Result
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("RowCount")]
        public int RowCount { get; set; }

        [JsonProperty("picItems")]
        public IList<string> PicItems { get; set; }
    }

    public class ClubJsonCase
    {

        [JsonProperty("returncode")]
        public int Returncode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }
    }
}


