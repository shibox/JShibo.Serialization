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
    /// 当前实现方案将byte类型数字转换成字符串明显更快
    /// BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1165 (20H2/October2020Update)
    /// 11th Gen Intel Core i7-11370H 3.30GHz, 1 CPU, 8 logical and 4 physical cores
    /// .NET SDK=6.0.100-preview.7.21379.14
    ///   [Host]     : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    ///   Job-RUHYYE : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    /// 
    /// IterationCount=1  LaunchCount=1  RunStrategy=ColdStart
    /// WarmupCount=1
    /// 
    /// |            Method | Size |     N |        Mean | Error | Ratio | Rank |       Gen 0 |     Gen 1 | Allocated |
    /// |------------------ |----- |------ |------------:|------:|------:|-----:|------------:|----------:|----------:|
    /// |         FastToCsv |  100 | 10000 |    81.07 ms |    NA |  1.00 |    1 |  12000.0000 |         - |     72 MB |
    /// | ServiceStackToCsv |  100 | 10000 | 1,971.43 ms |    NA | 24.32 |    2 | 309000.0000 | 2000.0000 |  1,854 MB |
    /// </summary>
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 1, targetCount: 1)]
    [RankColumn, MemoryDiagnoser]
    public class CsvWriterInt8Bench
    {
        static Int8Class[] data = null;

        [Params(100)]
        public int Size;

        [Params(10_000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            data = new Int8Class[Size];
            for (int i = 0; i < data.Length; i++)
                data[i] = ShiboSerializer.Initialize<Int8Class>();
        }

        [Benchmark(Baseline = true)]
        public void FastToCsv()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var csv = ShiboSerializer.ToCsv(data);
                sum += csv.Length;
            }
        }

        [Benchmark]
        public void ServiceStackToCsv()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var csv = ServiceStack.Text.CsvSerializer.SerializeToCsv(data);
                sum += csv.Length;
            }

        }

    }


}
