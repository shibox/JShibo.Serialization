using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CsvHelper;
using JShibo.Serialization.BenchMark.Entitiy;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Benchmark
{
    /// <summary>
    /// csv写入测试
    /// BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1165 (20H2/October2020Update)
    /// 11th Gen Intel Core i7-11370H 3.30GHz, 1 CPU, 8 logical and 4 physical cores
    /// .NET SDK=6.0.100-preview.7.21379.14
    ///   [Host]     : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    ///   Job-PLFMAA : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    /// 
    /// IterationCount=1  LaunchCount=1  RunStrategy=ColdStart
    /// WarmupCount=1
    /// 
    /// |            Method | Size |     N |       Mean | Error | Ratio | Rank |       Gen 0 |     Gen 1 | Allocated |
    /// |------------------ |----- |------ |-----------:|------:|------:|-----:|------------:|----------:|----------:|
    /// |         FastToCsv |  100 | 10000 |   313.8 ms |    NA |  1.00 |    1 |  34000.0000 |         - |    204 MB |
    /// | ServiceStackToCsv |  100 | 10000 | 2,115.5 ms |    NA |  6.74 |    2 | 373000.0000 | 2000.0000 |  2,232 MB |
    /// </summary>
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 1, targetCount: 1)]
    [RankColumn,MemoryDiagnoser]
    public class CsvWriterBench
    {
        static Int32Class[] data = null;
        
        [Params(100)]
        public int Size;

        [Params(10_000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            data = new Int32Class[Size];
            for (int i = 0; i < data.Length; i++)
                data[i] = ShiboSerializer.Initialize<Int32Class>();
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

        //[Benchmark]
        //public void CsvHelperToCsv()
        //{
        //    long sum = 0;
        //    for (int i = 0; i < N; i++)
        //    {
        //        var ms = new MemoryStream();
        //        using (var writer = new StreamWriter(ms))
        //        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        //        {
        //            csv.WriteRecords(data);
        //            sum += ms.Length;
        //        }
        //    }
        //}

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
