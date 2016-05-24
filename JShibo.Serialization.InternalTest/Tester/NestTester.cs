using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using JShibo.Net;
using Newtonsoft.Json;
using JShibo.Serialization.BenchMark.Entitiy;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class NestTester
    {
        private readonly static ConnectionSettings _settings;
        private readonly static ElasticClient _client;

        static NestTester()
        {
            //_settings = new ConnectionSettings(new Uri("http://218.11.176.13:9260/"), "news");
            //_settings.SetDefaultIndex("news");

            //_settings = new ConnectionSettings(new Uri("http://218.11.176.13:9260/"));
            _settings = new ConnectionSettings(new Uri("http://218.11.176.13:9260/"))
                    .MapDefaultTypeIndices(s => s.Add(typeof(Article), "news"))
                    .MapDefaultTypeNames(s => s.Add(typeof(Article), "article"));
            _client = new ElasticClient(_settings);
        }

        public static IEnumerable<Article> Search(string q)
        {
            var result =_client.Search<Article>(s => s.QueryString("title:" + q)
                .Fields(f => f.title)
                //.Fields(f => f.content)
                //.Fields(new string[]{"title","content"})
                );
            return result.Documents.ToList();
        }

        public static object Search2(string q)
        {
            string json = System.IO.File.ReadAllText("queryTemplate.txt");
            json = json.Replace("{0}", q);
            json = NetUtils.Post(json, "http://218.11.176.13:9260/news/article/_search");
            ElasticResult.RootClass result = JsonConvert.DeserializeObject<ElasticResult.RootClass>(json);
            Console.WriteLine(result);
            return result;
        }

        public class Article
        {
            public int id;
            public string title { get; set; }
            public string content;
            public string date;
            public string url;
            public string picUrls;
            public string author;
            public int authorId;
            public int area;
            public int province;
            public string brandRelation;
            public string seriesRelation;
            public string specRelation;
            public int class1;
            public int class2;
            public int kind;
            public string classurl;
            public string classUrl;
            public int replyCount;
            public long city;
            public float pvfv_m = 0.01f;
            public float tfv_m = 0.01f;
            public float tfv_pc = 0.01f;
            public float pvfv_pc = 0.01f;

            public long GetId()
            {
                return id;
            }

            public string GetUrl()
            {
                return url;
            }

            public DateTime GetDate()
            {
                DateTime d = DateTime.Parse(date);
                return d;
            }

            public string GetTitle()
            {
                return title;
            }

            public string GetContent()
            {
                return content;
            }
        }
    }
}

    

