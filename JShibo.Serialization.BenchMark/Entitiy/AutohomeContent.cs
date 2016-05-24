using JShibo.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark.Entitiy
{
    [Table(Name = "Autohome_ContentDb0")]
    public class AutohomeContent
    {
        [Column(Name = "id")]
        public long id { get; set; }
        //[Column(Name = "url")]
        //public string url { get; set; }
        //[Column(Name = "referrer")]
        //public string referrer { get; set; }
        //[Column(Name = "host")]
        //public string host { get; set; }
        [Column(Name = "anchorTitle")]
        public string anchorTitle { get; set; }
        //[Column(Name = "keywords")]
        //public string keywords { get; set; }
        //[Column(Name = "description")]
        //public string description { get; set; }
        [Column(Name = "headerTitle")]
        public string headerTitle { get; set; }
        //[Column(Name = "bodyTitle")]
        //public string bodyTitle { get; set; }
        [Column(Name = "body")]
        public string body { get; set; }
        [Column(Name = "length")]
        public int length { get; set; }
        [Column(Name = "type")]
        public byte type { get; set; }
        //[Column(Name = "digest")]
        //public string digest { get; set; }
        [Column(Name = "lastModified")]
        public DateTime lastModified { get; set; }
        [Column(Name = "lastCrawlDate")]
        public DateTime lastCrawlDate { get; set; }
        [Column(Name = "publishDate")]
        public DateTime publishDate { get; set; }
        [Column(Name = "boost")]
        public decimal boost { get; set; }
        //[Column(Name = "collection")]
        //public string collection { get; set; }
        [Column(Name = "isDelete")]
        public int isDelete { get; set; }

        public static long Test()
        {
            FileStream stream = new FileStream("Autohome_ContentDb0.bin", FileMode.Create, FileAccess.ReadWrite, FileShare.None, 1024 * 1024 * 16);
            DataContext context = new DataContext("Data Source = autohome-search-data-r; Initial Catalog = AutoSpider; User Id = autoWriter; Password = autohomeWriter;");
            //DataContext context = new DataContext("Data Source = autohome-search-data-r; Initial Catalog = AutoCrawler; User Id = autoReader; Password = autohomeReader;");
            IEnumerable<AutohomeContent> table = context.ExecuteQuery<AutohomeContent>("select top 1 * from [Autohome_ContentDb0] where isDelete= 0");
            //Table<AutohomeContent> table = context.GetTable<AutohomeContent>();
            int count = 0;
            AutohomeContent con = null;
            foreach (AutohomeContent ac in table)
            {
                con = ac;
                //size += ShiboSerializer.BinSerialize(ac).Length;
                ShiboSerializer.BinarySerialize(stream,ac);
                count++;
                if (count >= 1)
                    break;
            }

            Stopwatch w = new Stopwatch();
            w.Start();
            ShiboSerializer.BinarySerialize(stream, con);
            stream.Close();
            w.Stop();
            return w.ElapsedMilliseconds;
            //return size;
        }
    }
}
