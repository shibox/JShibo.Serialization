using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using JShibo.Serialization.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Benchmark
{
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 1, targetCount: 1)]
    [RankColumn, MemoryDiagnoser]
    public class GuidCopyBench
    {
        static Guid data = Guid.NewGuid();
        static string guid = string.Empty;

        [Params(100_000_000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            guid = data.ToString();
        }

        [Benchmark(Baseline = true)]
        public unsafe void FastToCsv()
        {
            long sum = 0;
            char* buffer = stackalloc char[100];
            for (int i = 0; i < N; i++)
            {
                fixed (char* pd = guid)
                {
                    Utils.FastCopyGuid(buffer, pd);
                    sum += 36;
                }
            }
        }

        [Benchmark]
        public unsafe void FastToCsvBytes()
        {
            long sum = 0;
            char* buffer = stackalloc char[100];
            for (int i = 0; i < N; i++)
            {
                fixed (char* pd = guid)
                {
                    Utils.FastCopyGuid(buffer, pd);
                    sum += 36;
                }
            }
        }


    }

}
