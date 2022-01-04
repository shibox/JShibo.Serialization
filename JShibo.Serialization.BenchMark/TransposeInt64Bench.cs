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
    /// 相比于Byte类型，手动和Emit模式差距明显缩小了
    /// 
    /// |           Method |   Size |   N |      Mean | Error | Ratio | Rank |      Gen 0 |      Gen 1 |      Gen 2 |     Allocated |
    /// |----------------- |------- |---- |----------:|------:|------:|-----:|-----------:|-----------:|-----------:|--------------:|
    /// |    UseManualFast | 100000 | 100 |  88.94 ms |    NA |  0.32 |    4 |          - |          - |          - |         480 B |
    /// |  UseManualFaster | 100000 | 100 |  70.43 ms |    NA |  0.25 |    1 |          - |          - |          - |         480 B |
    /// | UseManualFastest | 100000 | 100 |  74.15 ms |    NA |  0.26 |    2 |          - |          - |          - |         480 B |
    /// |        UseManual | 100000 | 100 | 281.28 ms |    NA |  1.00 |    6 | 12000.0000 | 12000.0000 | 12000.0000 | 800,039,840 B |
    /// |  UseManualBuffer | 100000 | 100 |  82.99 ms |    NA |  0.30 |    3 |          - |          - |          - |         480 B |
    /// |          UseEmit | 100000 | 100 | 208.85 ms |    NA |  0.74 |    5 |          - |          - |          - |     577,616 B |
    /// |   UseEmitNoCache | 100000 | 100 | 457.49 ms |    NA |  1.63 |    7 | 14000.0000 | 14000.0000 | 14000.0000 | 800,659,496 B |
    /// </summary>
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 1, targetCount: 1)]
    [RankColumn, MemoryDiagnoser]
    public class TransposeInt64Bench
    {
        static Int64Class[] dataInt64 = null;
        static long[][] buffer = new long[10][];
        static long[] v0 = null;
        static long[] v1 = null;
        static long[] v2 = null;
        static long[] v3 = null;
        static long[] v4 = null;
        static long[] v5 = null;
        static long[] v6 = null;
        static long[] v7 = null;
        static long[] v8 = null;
        static long[] v9 = null;

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
            buffer = new long[10][];
            for (int j = 0; j < buffer.Length; j++)
                buffer[j] = new long[dataInt64.Length];

            v0 = new long[dataInt64.Length];
            v1 = new long[dataInt64.Length];
            v2 = new long[dataInt64.Length];
            v3 = new long[dataInt64.Length];
            v4 = new long[dataInt64.Length];
            v5 = new long[dataInt64.Length];
            v6 = new long[dataInt64.Length];
            v7 = new long[dataInt64.Length];
            v8 = new long[dataInt64.Length];
            v9 = new long[dataInt64.Length];
        }

        

        [Benchmark]
        public void UseManualFast()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < dataInt64.Length; j++)
                {
                    v0[j] = dataInt64[i].V0;
                    v1[j] = dataInt64[1].V1;
                    v2[j] = dataInt64[2].V2;
                    v3[j] = dataInt64[3].V3;
                    v4[j] = dataInt64[4].V4;
                    v5[j] = dataInt64[5].V5;
                    v6[j] = dataInt64[6].V6;
                    v7[j] = dataInt64[7].V7;
                    v8[j] = dataInt64[8].V8;
                    v9[j] = dataInt64[9].V9;
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
                fixed (long* pv0 = v0, pv1 = v1, pv2 = v2, pv3 = v3, pv4 = v4, 
                    pv5 = v5, pv6 = v6, pv7 = v7, pv8 = v8, pv9 = v9)
                {
                    for (int j = 0; j < dataInt64.Length; j++)
                    {
                        pv0[j] = dataInt64[i].V0;
                        pv1[j] = dataInt64[1].V1;
                        pv2[j] = dataInt64[2].V2;
                        pv3[j] = dataInt64[3].V3;
                        pv4[j] = dataInt64[4].V4;
                        pv5[j] = dataInt64[5].V5;
                        pv6[j] = dataInt64[6].V6;
                        pv7[j] = dataInt64[7].V7;
                        pv8[j] = dataInt64[8].V8;
                        pv9[j] = dataInt64[9].V9;
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
                fixed (long* pv0 = v0, pv1 = v1, pv2 = v2, pv3 = v3, pv4 = v4,
                    pv5 = v5, pv6 = v6, pv7 = v7, pv8 = v8, pv9 = v9)
                {
                    long* p0 = pv0;
                    long* p1 = pv1;
                    long* p2 = pv2;
                    long* p3 = pv3;
                    long* p4 = pv4;
                    long* p5 = pv5;
                    long* p6 = pv6;
                    long* p7 = pv7;
                    long* p8 = pv8;
                    long* p9 = pv9;
                    for (int j = 0; j < dataInt64.Length; j++)
                    {
                        *p0 = dataInt64[i].V0;
                        *p1 = dataInt64[1].V1;
                        *p2 = dataInt64[2].V2;
                        *p3 = dataInt64[3].V3;
                        *p4 = dataInt64[4].V4;
                        *p5 = dataInt64[5].V5;
                        *p6 = dataInt64[6].V6;
                        *p7 = dataInt64[7].V7;
                        *p8 = dataInt64[8].V8;
                        *p9 = dataInt64[9].V9;
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
                var values = new long[10][];
                for (int j = 0; j < values.Length; j++)
                    values[j] = new long[dataInt64.Length];
                for (int j = 0; j < dataInt64.Length; j++)
                {
                    values[0][j] = dataInt64[i].V0;
                    values[1][j] = dataInt64[1].V1;
                    values[2][j] = dataInt64[2].V2;
                    values[3][j] = dataInt64[3].V3;
                    values[4][j] = dataInt64[4].V4;
                    values[5][j] = dataInt64[5].V5;
                    values[6][j] = dataInt64[6].V6;
                    values[7][j] = dataInt64[7].V7;
                    values[8][j] = dataInt64[8].V8;
                    values[9][j] = dataInt64[9].V9;
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
                for (int j = 0; j < dataInt64.Length; j++)
                {
                    values[0][j] = dataInt64[i].V0;
                    values[1][j] = dataInt64[1].V1;
                    values[2][j] = dataInt64[2].V2;
                    values[3][j] = dataInt64[3].V3;
                    values[4][j] = dataInt64[4].V4;
                    values[5][j] = dataInt64[5].V5;
                    values[6][j] = dataInt64[6].V6;
                    values[7][j] = dataInt64[7].V7;
                    values[8][j] = dataInt64[8].V8;
                    values[9][j] = dataInt64[9].V9;
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
                var ret = ShiboSerializer.ToColumns(dataInt64);
                sum += ret.Length;
            }
        }

        [Benchmark]
        public void UseEmitNoCache()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var ret = ShiboSerializer.ToColumns(dataInt64, false);
                sum += ret.Length;
            }
        }

    }

}
