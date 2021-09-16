using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using JShibo.Serialization.Common;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace JShibo.Serialization.Benchmark
{
    public class ToStringTests
    {
        [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 1, targetCount: 1)]
        [RankColumn]
        public class IntToString
        {
            
            private static char[] data;
            private static byte[] datab;

            [Params(1000000)]
            public int N;

            [GlobalSetup]
            public void Setup()
            {
                data = new char[100];
                datab = new byte[100];
            }

            [Benchmark(Baseline = true)]
            public unsafe void ToStringFast1PointByte()
            {
                fixed (byte* pd = &datab[0])
                {
                    for (int i = 0; i < N; i++)
                        FastToString.ToString(pd, (byte)1);
                }
            }

            [Benchmark]
            public void ToStringFast1() 
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringFast(data, 0, (uint)1);
            }

            [Benchmark]
            public unsafe void ToStringFast1Point()
            {
                fixed (char* pd = &data[11])
                {
                    for (int i = 0; i < N; i++)
                        FastToString.ToStringFast(pd, 0, (uint)1);
                }
            }

            [Benchmark]
            public void ToStringSimple1()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringSimple(data, 0, 1);
            }

            [Benchmark]
            public void ToString1()
            {
                for (int i = 0; i < N; i++)
                    ((uint)1).ToString();
            }

            [Benchmark]
            public void ToStringFast2()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringFast(data, 0, (uint)12);
            }

            [Benchmark]
            public void ToStringSimple2()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringSimple(data, 0, 12);
            }

            [Benchmark]
            public void ToString2()
            {
                for (int i = 0; i < N; i++)
                    ((uint)12).ToString();
            }

            [Benchmark]
            public void ToStringFast3()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringFast(data, 0, (uint)123);
            }

            [Benchmark]
            public void ToStringSimple3()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringSimple(data, 0, 123);
            }

            [Benchmark]
            public void ToString3()
            {
                for (int i = 0; i < N; i++)
                    ((uint)123).ToString();
            }

            [Benchmark]
            public unsafe void ToStringFast3Point()
            {
                fixed (char* pd = &data[11])
                {
                    for (int i = 0; i < N; i++)
                        FastToString.ToStringFast(pd, 0, (uint)123);
                }
            }

            [Benchmark]
            public unsafe void ToStringFast3PointByte()
            {
                fixed (byte* pd = &datab[0])
                {
                    for (int i = 0; i < N; i++)
                        FastToString.ToString(pd,(byte)123);
                }
            }

            [Benchmark]
            public void ToStringFast4()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringFast(data, 0, (uint)1234);
            }

            [Benchmark]
            public void ToStringSimple4()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringSimple(data, 0, 1234);
            }

            [Benchmark]
            public void ToString4()
            {
                for (int i = 0; i < N; i++)
                    ((uint)1234).ToString();
            }

            [Benchmark]
            public void ToStringFast5()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringFast(data, 0, (uint)12345);
            }

            [Benchmark]
            public void ToStringSimple5()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringSimple(data, 0, 12345);
            }

            [Benchmark]
            public void ToString5()
            {
                for (int i = 0; i < N; i++)
                    ((uint)12345).ToString();
            }

            [Benchmark]
            public void ToStringFast6()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringFast(data, 0, (uint)123456);
            }

            [Benchmark]
            public void ToStringSimple6()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringSimple(data, 0, 123456);
            }

            [Benchmark]
            public void ToString6()
            {
                for (int i = 0; i < N; i++)
                    ((uint)123456).ToString();
            }

            [Benchmark]
            public void ToStringFast7()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringFast(data, 0, (uint)1234567);
            }

            [Benchmark]
            public void ToStringSimple7()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringSimple(data, 0, 1234567);
            }

            [Benchmark]
            public void ToString7()
            {
                for (int i = 0; i < N; i++)
                    ((uint)1234567).ToString();
            }

            [Benchmark]
            public void ToStringFast8()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringFast(data, 0, (uint)12345678);
            }

            [Benchmark]
            public void ToStringSimple8()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringSimple(data, 0, 12345678);
            }

            [Benchmark]
            public void ToString8()
            {
                for (int i = 0; i < N; i++)
                    ((uint)12345678).ToString();
            }


        }

    }
}
