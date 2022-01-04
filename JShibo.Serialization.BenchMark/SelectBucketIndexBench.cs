using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Benchmark
{
    /// <summary>
    /// Log和LeadingZero的方案性能差不多
    /// 
    /// |                    Method |          N |     Mean | Error | Ratio | Rank | Allocated |
    /// |-------------------------- |----------- |---------:|------:|------:|-----:|----------:|
    /// |       UseLeadingZeroCount | 1000000000 | 958.0 ms |    NA |  1.27 |    5 |     480 B |
    /// |      UseBitOperationsLog2 | 1000000000 | 752.4 ms |    NA |  1.00 |    4 |     480 B |
    /// |          UseLeadingZeroOp | 1000000000 | 667.7 ms |    NA |  0.89 |    3 |     480 B |
    /// | UseBitOperationsLog2NoSum | 1000000000 | 228.9 ms |    NA |  0.30 |    1 |     480 B |
    /// |     UseLeadingZeroOpNoSum | 1000000000 | 231.6 ms |    NA |  0.31 |    2 |     480 B |
    /// </summary>
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 1, targetCount: 1)]
    [RankColumn, MemoryDiagnoser]
    public class SelectBucketIndexBench
    {
        [Params(1000_000_000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {

        }

        [Benchmark]
        public void UseLeadingZeroCount()
        {
            var sum = 0UL;
            for (int i = 0; i < N; i++)
            {
                var idx = 28 - Lzcnt.LeadingZeroCount((uint)i);
                if (i == (8 << (int)idx))
                    idx--;
                sum += idx;
            }
        }

        [Benchmark(Baseline = true)]
        public void UseBitOperationsLog2()
        {
            var sum = 0UL;
            for (int i = 0; i < N; i++)
            {
                var idx = (uint)BitOperations.Log2((uint)i - 1 | 15) - 3;
                sum += idx;
            }
        }

        [Benchmark]
        public void UseLeadingZeroOp()
        {
            var sum = 0UL;
            for (int i = 0; i < N; i++)
            {
                var idx = 28 - Lzcnt.LeadingZeroCount(((uint)i - 1) | 15);
                sum += idx;
            }
        }

        [Benchmark]
        public void UseBitOperationsLog2NoSum()
        {
            for (int i = 0; i < N; i++)
            {
                var idx = (uint)BitOperations.Log2((uint)i - 1 | 15) - 3;
            }
        }

        [Benchmark]
        public void UseLeadingZeroOpNoSum()
        {
            for (int i = 0; i < N; i++)
            {
                var idx = 28 - Lzcnt.LeadingZeroCount(((uint)i - 1) | 15);
            }
        }

    }


}
