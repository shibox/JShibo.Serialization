using BenchmarkDotNet.Attributes;
using JShibo.Serialization.Common;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Benchmark
{
    public class ToStringTests
    {
        [RPlotExporter, RankColumn]
        public class IntToString
        {
            
            private char[] data;

            [Params(10000,100000)]
            public int N;

            [GlobalSetup]
            public void Setup()
            {
                data = new char[100];
                //new Random(42).NextBytes(data);
            }

            [Benchmark]
            public void ToStringFast1() 
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringFast(data, 0, (uint)1);
            }

            [Benchmark]
            public void ToStringSimple1()
            {
                for (int i = 0; i < N; i++)
                    FastToString.ToStringSimple(data, 0, 1);
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


        }

    }
}
