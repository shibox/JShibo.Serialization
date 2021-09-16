using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using JShibo.Serialization.BenchMark.Entitiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Benchmark
{
    /// <summary>
    /// 重用缓冲区能明显的提升性能
    /// BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1237 (20H2/October2020Update)
    /// 11th Gen Intel Core i7-11370H 3.30GHz, 1 CPU, 8 logical and 4 physical cores
    /// .NET SDK=6.0.100-rc.1.21458.32
    ///   [Host]     : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    ///   Job-NMYOWA : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    /// 
    /// IterationCount=1  LaunchCount=1  RunStrategy=ColdStart
    /// WarmupCount=1
    /// 
    /// |               Method |   Size |   N |     Mean | Error | Ratio | Rank |      Gen 0 |      Gen 1 |      Gen 2 |     Allocated |
    /// |--------------------- |------- |---- |---------:|------:|------:|-----:|-----------:|-----------:|-----------:|--------------:|
    /// |       UseManualInt64 | 100000 | 100 | 275.3 ms |    NA |  1.00 |    2 | 14000.0000 | 14000.0000 | 14000.0000 | 800,040,448 B |
    /// | UseManualInt64Buffer | 100000 | 100 | 107.3 ms |    NA |  0.39 |    1 |          - |          - |          - |             - |
    /// |         UseEmitInt64 | 100000 | 100 | 429.3 ms |    NA |  1.56 |    3 | 17000.0000 | 17000.0000 | 17000.0000 | 800,659,144 B |
    /// </summary>
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 1, targetCount: 1)]
    [RankColumn, MemoryDiagnoser]
    public class TransposeInt64Bench
    {
        static Int64Class[] dataInt64 = null;
        static long[][] ints = new long[10][];

        [Params(100_000)]
        public int Size;

        [Params(100)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            dataInt64 = new Int64Class[Size];
            for (int i = 0; i < Size; i++)
            {
                dataInt64[i] = ShiboSerializer.Initialize<Int64Class>();
            }
            ints = new long[10][];
            for (int j = 0; j < ints.Length; j++)
                ints[j] = new long[dataInt64.Length];
        }

        [Benchmark(Baseline = true)]
        public void UseManualInt64()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var ints = new long[10][];
                for (int j = 0; j < ints.Length; j++)
                    ints[j] = new long[dataInt64.Length];
                for (int j = 0; j < dataInt64.Length; j++)
                {
                    ints[0][j] = dataInt64[i].V0;
                    ints[1][j] = dataInt64[1].V1;
                    ints[2][j] = dataInt64[2].V2;
                    ints[3][j] = dataInt64[3].V3;
                    ints[4][j] = dataInt64[4].V4;
                    ints[5][j] = dataInt64[5].V5;
                    ints[6][j] = dataInt64[6].V6;
                    ints[7][j] = dataInt64[7].V7;
                    ints[8][j] = dataInt64[8].V8;
                    ints[9][j] = dataInt64[9].V9;
                }
                sum += ints.Length;
            }
        }

        [Benchmark]
        public void UseManualInt64Buffer()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < dataInt64.Length; j++)
                {
                    ints[0][j] = dataInt64[i].V0;
                    ints[1][j] = dataInt64[1].V1;
                    ints[2][j] = dataInt64[2].V2;
                    ints[3][j] = dataInt64[3].V3;
                    ints[4][j] = dataInt64[4].V4;
                    ints[5][j] = dataInt64[5].V5;
                    ints[6][j] = dataInt64[6].V6;
                    ints[7][j] = dataInt64[7].V7;
                    ints[8][j] = dataInt64[8].V8;
                    ints[9][j] = dataInt64[9].V9;
                }
                sum += ints.Length;
            }
        }

        [Benchmark]
        public void UseEmitInt64()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var ret = ShiboSerializer.ToColumns(dataInt64);
                sum += ret.Length;
            }
        }

    }

}
