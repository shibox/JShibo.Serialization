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
    /// 使用指针处理仍然是最快的，可以尝试使用span与unsafe结合的方式测试
    /// Emit模式因为有一些基础开销，性能稍低
    /// 
    /// BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1237 (20H2/October2020Update)
    /// 11th Gen Intel Core i7-11370H 3.30GHz, 1 CPU, 8 logical and 4 physical cores
    /// .NET SDK=6.0.100
    ///   [Host]     : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    ///   Job-TAJNIA : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
    /// 
    /// IterationCount=1  LaunchCount=1  RunStrategy=ColdStart
    /// WarmupCount=1
    /// 
    /// |           Method |    Size |   N |       Mean | Error | Ratio | Rank |      Gen 0 |      Gen 1 |      Gen 2 |       Allocated |
    /// |----------------- |-------- |---- |-----------:|------:|------:|-----:|-----------:|-----------:|-----------:|----------------:|
    /// |    UseManualFast | 1000000 | 100 |   783.2 ms |    NA |  0.91 |    3 |          - |          - |          - |           592 B |
    /// |  UseManualFaster | 1000000 | 100 |   546.1 ms |    NA |  0.63 |    1 |          - |          - |          - |           592 B |
    /// | UseManualFastest | 1000000 | 100 |   546.1 ms |    NA |  0.63 |    1 |          - |          - |          - |           592 B |
    /// |        UseManual | 1000000 | 100 |   864.5 ms |    NA |  1.00 |    4 | 12000.0000 | 12000.0000 | 12000.0000 | 1,000,040,112 B |
    /// |  UseManualBuffer | 1000000 | 100 |   662.8 ms |    NA |  0.77 |    2 |          - |          - |          - |           592 B |
    /// |          UseEmit | 1000000 | 100 | 1,789.2 ms |    NA |  2.07 |    5 | 30000.0000 | 30000.0000 | 30000.0000 | 1,000,799,104 B |
    /// </summary>
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 1, targetCount: 1)]
    [RankColumn, MemoryDiagnoser]
    public class TransposeInt8Bench
    {
        static Int8Class[] dataInt8 = null;
        static byte[][] buffer = new byte[10][];
        static byte[] v0 = null;
        static byte[] v1 = null;
        static byte[] v2 = null;
        static byte[] v3 = null;
        static byte[] v4 = null;
        static byte[] v5 = null;
        static byte[] v6 = null;
        static byte[] v7 = null;
        static byte[] v8 = null;
        static byte[] v9 = null;

        [Params(1000_000)]
        public int Size;

        [Params(100)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            dataInt8 = new Int8Class[Size];
            for (int i = 0; i < Size; i++)
            {
                dataInt8[i] = ShiboSerializer.Initialize<Int8Class>();
            }
            buffer = new byte[10][];
            for (int j = 0; j < buffer.Length; j++)
                buffer[j] = new byte[dataInt8.Length];

            v0 = new byte[dataInt8.Length];
            v1 = new byte[dataInt8.Length];
            v2 = new byte[dataInt8.Length];
            v3 = new byte[dataInt8.Length];
            v4 = new byte[dataInt8.Length];
            v5 = new byte[dataInt8.Length];
            v6 = new byte[dataInt8.Length];
            v7 = new byte[dataInt8.Length];
            v8 = new byte[dataInt8.Length];
            v9 = new byte[dataInt8.Length];
        }

        [Benchmark]
        public void UseManualFast()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < dataInt8.Length; j++)
                {
                    v0[j] = dataInt8[i].V0;
                    v1[j] = dataInt8[1].V1;
                    v2[j] = dataInt8[2].V2;
                    v3[j] = dataInt8[3].V3;
                    v4[j] = dataInt8[4].V4;
                    v5[j] = dataInt8[5].V5;
                    v6[j] = dataInt8[6].V6;
                    v7[j] = dataInt8[7].V7;
                    v8[j] = dataInt8[8].V8;
                    v9[j] = dataInt8[9].V9;
                }
                sum += buffer.Length;
            }
        }

        [Benchmark]
        public unsafe void UseManualFaster()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                fixed (byte* pv0 = v0, pv1 = v1, pv2 = v2, pv3 = v3, pv4 = v4,
                    pv5 = v5, pv6 = v6, pv7 = v7, pv8 = v8, pv9 = v9)
                {
                    for (int j = 0; j < dataInt8.Length; j++)
                    {
                        pv0[j] = dataInt8[i].V0;
                        pv1[j] = dataInt8[1].V1;
                        pv2[j] = dataInt8[2].V2;
                        pv3[j] = dataInt8[3].V3;
                        pv4[j] = dataInt8[4].V4;
                        pv5[j] = dataInt8[5].V5;
                        pv6[j] = dataInt8[6].V6;
                        pv7[j] = dataInt8[7].V7;
                        pv8[j] = dataInt8[8].V8;
                        pv9[j] = dataInt8[9].V9;
                    }
                }
                sum += buffer.Length;
            }
        }

        [Benchmark]
        public unsafe void UseManualFastest()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                fixed (byte* pv0 = v0, pv1 = v1, pv2 = v2, pv3 = v3, pv4 = v4,
                    pv5 = v5, pv6 = v6, pv7 = v7, pv8 = v8, pv9 = v9)
                {
                    byte* p0 = pv0;
                    byte* p1 = pv1;
                    byte* p2 = pv2;
                    byte* p3 = pv3;
                    byte* p4 = pv4;
                    byte* p5 = pv5;
                    byte* p6 = pv6;
                    byte* p7 = pv7;
                    byte* p8 = pv8;
                    byte* p9 = pv9;
                    for (int j = 0; j < dataInt8.Length; j++)
                    {
                        *p0 = dataInt8[i].V0;
                        *p1 = dataInt8[1].V1;
                        *p2 = dataInt8[2].V2;
                        *p3 = dataInt8[3].V3;
                        *p4 = dataInt8[4].V4;
                        *p5 = dataInt8[5].V5;
                        *p6 = dataInt8[6].V6;
                        *p7 = dataInt8[7].V7;
                        *p8 = dataInt8[8].V8;
                        *p9 = dataInt8[9].V9;
                        p0++;
                        p1++;
                        p2++;
                        p3++;
                        p4++;
                        p5++;
                        p6++;
                        p7++;
                        p8++;
                        p9++;
                    }
                }
                sum += buffer.Length;
            }
        }

        [Benchmark(Baseline = true)]
        public void UseManual()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var values = new byte[10][];
                for (int j = 0; j < values.Length; j++)
                    values[j] = new byte[dataInt8.Length];
                for (int j = 0; j < dataInt8.Length; j++)
                {
                    values[0][j] = dataInt8[i].V0;
                    values[1][j] = dataInt8[1].V1;
                    values[2][j] = dataInt8[2].V2;
                    values[3][j] = dataInt8[3].V3;
                    values[4][j] = dataInt8[4].V4;
                    values[5][j] = dataInt8[5].V5;
                    values[6][j] = dataInt8[6].V6;
                    values[7][j] = dataInt8[7].V7;
                    values[8][j] = dataInt8[8].V8;
                    values[9][j] = dataInt8[9].V9;
                }
                sum += values.Length;
            }
        }

        [Benchmark]
        public void UseManualBuffer()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var values = buffer;
                for (int j = 0; j < dataInt8.Length; j++)
                {
                    values[0][j] = dataInt8[i].V0;
                    values[1][j] = dataInt8[1].V1;
                    values[2][j] = dataInt8[2].V2;
                    values[3][j] = dataInt8[3].V3;
                    values[4][j] = dataInt8[4].V4;
                    values[5][j] = dataInt8[5].V5;
                    values[6][j] = dataInt8[6].V6;
                    values[7][j] = dataInt8[7].V7;
                    values[8][j] = dataInt8[8].V8;
                    values[9][j] = dataInt8[9].V9;
                }
                sum += buffer.Length;
            }
        }

        [Benchmark]
        public void UseEmit()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var ret = ShiboSerializer.ToColumns(dataInt8);
                sum += ret.Length;
            }
        }

    }

}
