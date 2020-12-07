using BenchmarkDotNet.Running;
using System;

namespace JShibo.Serialization.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ToStringTests.IntToString>();
            Console.WriteLine("finish!");
        }
    }
}
