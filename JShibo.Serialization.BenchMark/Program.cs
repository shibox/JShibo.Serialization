using BenchmarkDotNet.Running;
using System;

namespace JShibo.Serialization.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<ToStringTests.IntToString>();
            //var summary = BenchmarkRunner.Run<CsvWriterBench>();
            //var summary = BenchmarkRunner.Run<CsvWriterInt8Bench>();
            //var summary = BenchmarkRunner.Run<TransposeBench>();
            //var summary = BenchmarkRunner.Run<PoolBufferBench>();
            var summary = BenchmarkRunner.Run<TransposeInt64Bench>();
            

            Console.WriteLine("finish!");
        }
    }
}
