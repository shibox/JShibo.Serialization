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
    /// 内存池性能测试，性能差异不大？
    /// BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1165 (20H2/October2020Update)
    /// 11th Gen Intel Core i7-11370H 3.30GHz, 1 CPU, 8 logical and 4 physical cores
    /// .NET SDK=6.0.100-preview.7.21379.14
    ///   [Host]     : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    ///   Job-YLUSMQ : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    /// 
    /// IterationCount=1  LaunchCount=1  RunStrategy=ColdStart
    /// WarmupCount=1
    /// 
    /// |       Method | Size |        N |     Mean | Error | Ratio | Rank | Allocated |
    /// |------------- |----- |--------- |---------:|------:|------:|-----:|----------:|
    /// |       MyPool |  100 | 10000000 | 190.6 ms |    NA |  1.00 |    2 |         - |
    /// | SysArrayPool |  100 | 10000000 | 180.4 ms |    NA |  0.95 |    1 |     288 B |
    /// |              |      |          |          |       |       |      |           |
    /// |       MyPool | 1000 | 10000000 | 201.8 ms |    NA |  1.00 |    2 |         - |
    /// | SysArrayPool | 1000 | 10000000 | 189.1 ms |    NA |  0.94 |    1 |     288 B |
    /// 
    /// 
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

        [Benchmark(Baseline = true)]
        public void MyPool()
        {
            for (int i = 0; i < N; i++)
            {
                var buf = CharsBufferManager.GetBuffer(Size);
                CharsBufferManager.SetBuffer(buf);
            }
        }

        [Benchmark]
        public void SysArrayPool()
        {
            for (int i = 0; i < N; i++)
            {
                var buf = ArrayPool<char>.Shared.Rent(Size);
                ArrayPool<char>.Shared.Return(buf);
            }
        }
        
    }
}
