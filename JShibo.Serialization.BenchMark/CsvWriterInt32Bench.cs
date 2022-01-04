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
    /// 对于int类型，都是一些比较大的数字的时候，性能差距就不是很明显了
    /// 
    /// 100个元素的差距
    /// |            Method | Size |     N |       Mean | Error | Ratio | Rank |       Gen 0 |     Gen 1 | Allocated |
    /// |------------------ |----- |------ |-----------:|------:|------:|-----:|------------:|----------:|----------:|
    /// |         FastToCsv |  100 | 10000 |   379.4 ms |    NA |  1.00 |    2 |  33000.0000 |         - |    204 MB |
    /// |    FastToCsvBytes |  100 | 10000 |   332.0 ms |    NA |  0.87 |    1 |           - |         - |      1 MB |
    /// | ServiceStackToCsv |  100 | 10000 | 2,131.5 ms |    NA |  5.62 |    3 | 372000.0000 | 2000.0000 |  2,230 MB |
    /// 
    /// 
    /// </summary>
    [SimpleJob(RunStrategy.ColdStart, launchCount: 1, warmupCount: 1, targetCount: 1)]
    [RankColumn, MemoryDiagnoser]
    public class CsvWriterInt32Bench
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

        //[Benchmark]
        //public void FastToCsvUseManual()
        //{
        //    long sum = 0;
        //    for (int i = 0; i < N; i++)
        //    {
        //        var csv = ToCsv(data, out var buf);
        //        sum += csv;
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

        ///// <summary>
        ///// 仅供参考对照
        ///// </summary>
        ///// <param name="list"></param>
        ///// <param name="buffer"></param>
        ///// <returns></returns>
        //public static unsafe int ToCsv(Int32Class[] list, out Span<byte> buffer)
        //{
        //    var bytes = ArrayPool<byte>.Shared.Rent(list.Length * 50);
        //    buffer = new Span<byte>(bytes);
        //    for (int i = 0; i < list.Length; i++)
        //    {
        //        var v = list[i];
        //        fixed (byte* p = buffer)
        //        {
        //            var ptr = p;
        //            var len = 0;
        //            len = FastToString.ToStringNumber(ptr, v.V0);
        //            ptr += len;
        //            len = FastToString.ToStringNumber(ptr, v.V1);
        //            ptr += len;
        //            len = FastToString.ToStringNumber(ptr, v.V2);
        //            ptr += len;
        //            len = FastToString.ToStringNumber(ptr, v.V3);
        //            ptr += len;
        //            len = FastToString.ToStringNumber(ptr, v.V4);
        //            ptr += len;
        //            len = FastToString.ToStringNumber(ptr, v.V5);
        //            ptr += len;
        //            len = FastToString.ToStringNumber(ptr, v.V6);
        //            ptr += len;
        //            len = FastToString.ToStringNumber(ptr, v.V7);
        //            ptr += len;
        //            len = FastToString.ToStringNumber(ptr, v.V8);
        //            ptr += len;
        //            len = FastToString.ToStringNumber(ptr, v.V9);
        //            ptr += len;
        //        }
        //    }
        //    ArrayPool<byte>.Shared.Return(bytes);
        //    return 0;
        //}



    }

}
