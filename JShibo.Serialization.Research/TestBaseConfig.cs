using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.BenchMark
{
    public class TestBaseConfig
    {
        public static int Seed = DateTime.Now.Millisecond;
        public static int ArrayMinSize = 10;
        public static int ArrayMaxSize = 100;


        public static bool Fastest = true;
        public static bool Soc = true;
        public static bool SocStream = true;
        public static bool MsgPack = true;
        public static bool ProtoBuf = true;
        public static bool Manual = false;
        public static bool Newtonsoft = true;
        public static bool ServiceStack = true;
        public static bool Fastjson = true;
        public static bool BinaryFormatter = false;
        public static bool JavaScriptSerializer = false;
        public static bool DataContractJsonSerializer = false;
    }

    public class TestConfig
    {
        #region 字段

        public static int capacity = 2000;
        public static byte[] socBuffer = new byte[capacity];
        public static char[] jsonBuffer = new char[capacity];
        public static bool isBuffer = true;
        public static int defaultSize = 500;
        public static bool isPub = false;
        public static bool isString = false;
        public static int testCount = 1000000;
        public static bool toString = false;
        public static bool isInfo = false;
        public static bool isConsole = false;
        /// <summary>
        /// 是否写入测试日志到数据库中
        /// </summary>
        public static bool isLog = false;
        public static SerializerSettings sets = new SerializerSettings();

        #endregion

        #region 方法



        #endregion

    }
}
