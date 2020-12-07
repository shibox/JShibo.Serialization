using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark.Entitiy
{
    public class ElasticResult
    {
        public class Shards
        {

            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("successful")]
            public int Successful { get; set; }

            [JsonProperty("failed")]
            public int Failed { get; set; }
        }

        public class Hit
        {

            [JsonProperty("_index")]
            public string Index { get; set; }

            [JsonProperty("_type")]
            public string Type { get; set; }

            [JsonProperty("_id")]
            public string Id { get; set; }

            [JsonProperty("_score")]
            public double Score { get; set; }

            [JsonProperty("fields")]
            public Dictionary<string,object> Fields { get; set; }

            [JsonProperty("highlight")]
            public Dictionary<string,string[]> Highlight { get; set; }
        }

        public class Hits
        {

            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("max_score")]
            public double MaxScore { get; set; }
            
            [JsonProperty("hits")]
            public IList<Hit> HitList { get; set; }
        }

        public class RootClass
        {

            [JsonProperty("took")]
            public int Took { get; set; }

            [JsonProperty("timed_out")]
            public bool TimedOut { get; set; }

            [JsonProperty("_shards")]
            public Shards Shards { get; set; }

            [JsonProperty("hits")]
            public Hits Hits { get; set; }
        }
    }


}
