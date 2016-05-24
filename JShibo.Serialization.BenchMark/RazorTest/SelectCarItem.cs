using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark.RazorTest
{
    public class SelectCarItem : IComparable<SelectCarItem>
    {

        [JsonProperty("seriesid")]
        public int Seriesid { get; set; }

        [JsonProperty("seriesname")]
        public string Seriesname { get; set; }

        [JsonProperty("brandname")]
        public string Brandname { get; set; }

        [JsonProperty("seriesminprice")]
        public int Seriesminprice { get; set; }

        [JsonProperty("seriesmaxprice")]
        public int Seriesmaxprice { get; set; }

        [JsonProperty("seriesrank")]
        public double Seriesrank { get; set; }

        [JsonProperty("Serieslevel")]
        public string Serieslevel { get; set; }

        public int CompareTo(SelectCarItem other)
        {
            return other.Seriesrank.CompareTo(this.Seriesrank);
        }
    }

    /// <summary>
    /// 最终找车结果的Model
    /// </summary>
    public class SelectCarModel
    {
        public string Title { get; set; }
        public IList<SelectCarItem> CarList { get; set; }
        public string More { get; set; }
        public bool IsBrand { get; set; }
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string Logo { get; set; }
        public string OfficialUrl { get; set; }
        public IList<Branditem> BrandList { get; set; }
    }

    public class Branditem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

    }

}
