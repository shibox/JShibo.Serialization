using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using JShibo.Serialization.BenchMark.Entitiy;
using JShibo.Serialization.Common;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Benchmark
{
    /// <summary>
    /// 使用二进制+缓冲区+代码生成，理论上可以比ServiceStackToCsv方案快50倍
    /// 使用二进制+Span优化比字符串模式快的不多
    /// 
    /// BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1165 (20H2/October2020Update)
    /// 11th Gen Intel Core i7-11370H 3.30GHz, 1 CPU, 8 logical and 4 physical cores
    /// .NET SDK=6.0.100
    ///   [Host]     : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    ///   Job-RUHYYE : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    /// 
    /// IterationCount=1  LaunchCount=1  RunStrategy=ColdStart
    /// WarmupCount=1
    /// 
    /// |             Method | Size |     N |        Mean | Error | Ratio | Rank |       Gen 0 |     Gen 1 |       Allocated |
    /// |------------------- |----- |------ |------------:|------:|------:|-----:|------------:|----------:|----------------:|
    /// |          FastToCsv |  100 | 10000 |    60.60 ms |    NA |  1.00 |    3 |  12000.0000 |         - |    75,600,480 B |
    /// |     FastToCsvBytes |  100 | 10000 |    51.17 ms |    NA |  0.84 |    2 |           - |         - |     1,520,816 B |
    /// | FastToCsvUseManual |  100 | 10000 |    17.69 ms |    NA |  0.29 |    1 |           - |         - |           816 B |
    /// |  ServiceStackToCsv |  100 | 10000 | 1,907.37 ms |    NA | 31.47 |    4 | 308000.0000 | 2000.0000 | 1,938,400,480 B |
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
        public void FastToCsvBytes()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var csv = ShiboSerializer.ToCsvUtf8(data);
                sum += csv.Length;
            }
        }

        [Benchmark]
        public void FastToCsvUseManual()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var csv = ToCsv(data,out var buf);
                sum += csv;
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

        /// <summary>
        /// 仅供参考对照
        /// </summary>
        /// <param name="list"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static unsafe int ToCsv(Int8Class[] list,out Span<byte> buffer)
        {
            var bytes = ArrayPool<byte>.Shared.Rent(list.Length * 50);
            buffer = new Span<byte>(bytes);
            for (int i = 0; i < list.Length; i++)
            { 
                var v = list[i];
                fixed (byte* p = buffer)
                {
                    var ptr = p;
                    var len = 0;
                    len = FastToString.ToStringNumber(ptr, v.V0);
                    ptr += len;
                    len = FastToString.ToStringNumber(ptr, v.V1);
                    ptr += len;
                    len = FastToString.ToStringNumber(ptr, v.V2);
                    ptr += len;
                    len = FastToString.ToStringNumber(ptr, v.V3);
                    ptr += len;
                    len = FastToString.ToStringNumber(ptr, v.V4);
                    ptr += len;
                    len = FastToString.ToStringNumber(ptr, v.V5);
                    ptr += len;
                    len = FastToString.ToStringNumber(ptr, v.V6);
                    ptr += len;
                    len = FastToString.ToStringNumber(ptr, v.V7);
                    ptr += len;
                    len = FastToString.ToStringNumber(ptr, v.V8);
                    ptr += len;
                    len = FastToString.ToStringNumber(ptr, v.V9);
                    ptr += len;
                }
            }
            ArrayPool<byte>.Shared.Return(bytes);
            return 0;
        }



    }


}
