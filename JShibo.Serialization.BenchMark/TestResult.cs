using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JShibo.Serialization.BenchMark
{
    public class TestResult
    {
        public bool Success { get; set; }
        public int Time { get; set; }
        public int CompileTime { get; set; }
        public string Json { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public int TestCount { get; set; }
        public List<Took> Tooks { get; set; }
        

        public TestResult()
        {
            Tooks = new List<Took>();
        }
    }

    /// <summary>
    /// 计算耗时和倍率
    /// </summary>
    public class Took
    {
        public string Name { get; set; }
        public float Cost { get; set; }
        public string Json { get; set; }
    }
}
