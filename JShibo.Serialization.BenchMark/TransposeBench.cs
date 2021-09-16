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
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 1, targetCount: 1)]
    [RankColumn, MemoryDiagnoser]
    public class TransposeBench
    {
        static Int8Class[] dataInt8 = null;
        static Int32Class[] dataInt32 = null;
        static Int64Class[] dataInt64 = null;

        [Params(1000_000)]
        public int Size;

        [Params(10)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            dataInt8 = new Int8Class[Size];
            dataInt32 = new Int32Class[Size];
            dataInt64 = new Int64Class[Size];
            for (int i = 0; i < dataInt8.Length; i++)
            {
                dataInt8[i] = ShiboSerializer.Initialize<Int8Class>();
                dataInt32[i] = ShiboSerializer.Initialize<Int32Class>();
                dataInt64[i] = ShiboSerializer.Initialize<Int64Class>();
            }
        }

        [Benchmark(Baseline = true)]
        public void UseManualInt8()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var bytes = new byte[10][];
                for (int j = 0; j < bytes.Length; j++)
                    bytes[j] = new byte[dataInt8.Length];
                for (int j = 0; j < dataInt8.Length; j++)
                {
                    bytes[0][j] = dataInt8[i].V0;
                    bytes[1][j] = dataInt8[1].V1;
                    bytes[2][j] = dataInt8[2].V2;
                    bytes[3][j] = dataInt8[3].V3;
                    bytes[4][j] = dataInt8[4].V4;
                    bytes[5][j] = dataInt8[5].V5;
                    bytes[6][j] = dataInt8[6].V6;
                    bytes[7][j] = dataInt8[7].V7;
                    bytes[8][j] = dataInt8[8].V8;
                    bytes[9][j] = dataInt8[9].V9;
                }
                sum += bytes.Length;
            }
        }

        [Benchmark]
        public void UseEmitInt8()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var ret = ShiboSerializer.ToColumns(dataInt8);
                sum += ret.Length;
            }
        }

        [Benchmark]
        public void UseManualInt32()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var ints = new int[10][];
                for (int j = 0; j < ints.Length; j++)
                    ints[j] = new int[dataInt8.Length];
                for (int j = 0; j < dataInt8.Length; j++)
                {
                    ints[0][j] = dataInt32[i].V0;
                    ints[1][j] = dataInt32[1].V1;
                    ints[2][j] = dataInt32[2].V2;
                    ints[3][j] = dataInt32[3].V3;
                    ints[4][j] = dataInt32[4].V4;
                    ints[5][j] = dataInt32[5].V5;
                    ints[6][j] = dataInt32[6].V6;
                    ints[7][j] = dataInt32[7].V7;
                    ints[8][j] = dataInt32[8].V8;
                    ints[9][j] = dataInt32[9].V9;
                }
                sum += ints.Length;
            }
        }

        [Benchmark]
        public void UseEmitInt32()
        {
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                var ret = ShiboSerializer.ToColumns(dataInt32);
                sum += ret.Length;
            }
        }

        [Benchmark]
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
