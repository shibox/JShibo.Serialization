using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JShibo.Serialization;

namespace JShibo.Serialization.Benchmark
{
    /// <summary>
    /// 优化版的XPool性能明显更好
    /// 
    /// BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1165 (20H2/October2020Update)
    /// 11th Gen Intel Core i7-11370H 3.30GHz, 1 CPU, 8 logical and 4 physical cores
    /// .NET SDK=6.0.100-preview.7.21379.14
    ///   [Host]     : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    ///   Job-YLUSMQ : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    /// 
    /// IterationCount=1  LaunchCount=1  RunStrategy=ColdStart
    /// WarmupCount=1
    /// 
    /// |       Method | Size |        N |      Mean | Error | Ratio | Rank | Allocated |
    /// |------------- |----- |--------- |----------:|------:|------:|-----:|----------:|
    /// |       MyPool |  100 | 10000000 | 198.88 ms |    NA |  1.19 |    3 |     480 B |
    /// | SysArrayPool |  100 | 10000000 | 167.21 ms |    NA |  1.00 |    2 |     480 B |
    /// |        XPool |  100 | 10000000 |  91.65 ms |    NA |  0.55 |    1 |     480 B |
    /// |              |      |          |           |       |       |      |           |
    /// |       MyPool | 1000 | 10000000 | 191.31 ms |    NA |  1.13 |    3 |     480 B |
    /// | SysArrayPool | 1000 | 10000000 | 168.69 ms |    NA |  1.00 |    2 |     480 B |
    /// |        XPool | 1000 | 10000000 |  95.41 ms |    NA |  0.57 |    1 |     480 B |
    /// </summary>
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 1, targetCount: 1)]
    [RankColumn, MemoryDiagnoser]
    public class PoolBufferBench
    {
        [Params(100,1000)]
        public int Size;

        [Params(10_000_000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            
        }

        [Benchmark]
        public void MyPool()
        {
            for (int i = 0; i < N; i++)
            {
                var buf = XPoolSave<char>.Rent(Size);
                XPoolSave<char>.Return(buf);
            }
        }

        [Benchmark(Baseline = true)]
        public void SysArrayPool()
        {
            for (int i = 0; i < N; i++)
            {
                var buf = ArrayPool<char>.Shared.Rent(Size);
                ArrayPool<char>.Shared.Return(buf);
            }
        }

        [Benchmark]
        public void XPool()
        {
            for (int i = 0; i < N; i++)
            {
                var buf = XPool<char>.Shared.Rent(Size);
                XPool<char>.Shared.Return(buf);
            }
        }

    }
}
