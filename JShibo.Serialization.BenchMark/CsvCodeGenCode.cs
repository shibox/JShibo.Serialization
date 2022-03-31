using JShibo.Serialization.BenchMark.Entitiy;
using JShibo.Serialization.Common;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Benchmark
{
    public class CsvCodeGenCode
    {
        public static void Run()
        {
            var Size = 10;
            var data = new Int8Class[Size];
            for (int i = 0; i < data.Length; i++)
                data[i] = ShiboSerializer.Initialize<Int8Class>();


            var result = ConvertInt8Class(data);
            var str = Encoding.UTF8.GetString(result);
            Console.WriteLine(result);
        }

        public unsafe static void ConvertPrimitiveTypeClass(PrimitiveTypeClass[] list)
        {
            var varSize = 0;

            for (int i = 0; i < list.Length; i++)
            {
                var v = list[i];
                varSize += v.V10.Length * 3 + 1;
            }
            var baseSize = 52;
            var headerSize = 36;
            var header = Encoding.UTF8.GetBytes("V0,V1,V2,V3,V4,V5,V6,V7,V10,V12,V13");
            var len = 0;
            var bytes = ArrayPool<byte>.Shared.Rent(list.Length * (baseSize + 1) + headerSize * 3);
            var buffer = new Span<byte>(bytes);
            header.CopyTo(buffer);
            

            for (int i = 0; i < list.Length; i++)
            {
                var v = list[i];
                fixed (byte* p = buffer)
                {
                    var ptr = p;
                    len = FastToString.ToStringNumber(ptr, v.V0);
                    ptr += len;
                    //len = FastToString.ToStringNumber(ptr, v.V1);
                    //ptr += len;
                    //len = FastToString.ToStringNumber(ptr, v.V2);
                    //ptr += len;
                    //len = FastToString.ToStringNumber(ptr, v.V3);
                    //ptr += len;
                    //len = FastToString.ToStringNumber(ptr, v.V4);
                    //ptr += len;
                    //len = FastToString.ToStringNumber(ptr, v.V5);
                    //ptr += len;
                    //len = FastToString.ToStringNumber(ptr, v.V6);
                    //ptr += len;
                    //len = FastToString.ToStringNumber(ptr, v.V7);
                    //ptr += len;
                    //len = FastToString.ToStringNumber(ptr, v.V10);
                    //ptr += len;
                    //len = FastToString.ToStringNumber(ptr, v.V12);
                    //ptr += len;
                    //len = FastToString.ToStringNumber(ptr, v.V13);
                    //ptr += len;
                }
            }







        }


        static byte[] header = Encoding.UTF8.GetBytes("V0,V1,V2,V3,V4,V5,V6,V7,V8,V9\n");

        public unsafe static ArraySegment<byte> ConvertInt8Class(Int8Class[] list)
        {
            var varSize = 0;

            for (int i = 0; i < list.Length; i++)
            {
                var v = list[i];

            }
            var baseSize = 40;
            var headerSize = 30;
            
            var len = 0;
            var bytes = ArrayPool<byte>.Shared.Rent(list.Length * (baseSize + 1) + headerSize);
            var buffer = new Span<byte>(bytes);
            header.CopyTo(buffer);

            fixed (byte* pd = &buffer[header.Length])
            {
                var ptr = pd;
                for (int i = 0; i < list.Length; i++)
                {
                    var v = list[i];


                    len = FastToString.ToStringNumber(ptr, v.V0);
                    ptr += len;
                    *ptr = (byte)',';
                    ptr++;
                    len = FastToString.ToStringNumber(ptr, v.V1);
                    ptr += len;
                    *ptr = (byte)',';
                    ptr++;
                    len = FastToString.ToStringNumber(ptr, v.V2);
                    ptr += len;
                    *ptr = (byte)',';
                    ptr++;
                    len = FastToString.ToStringNumber(ptr, v.V3);
                    ptr += len;
                    *ptr = (byte)',';
                    ptr++;
                    len = FastToString.ToStringNumber(ptr, v.V4);
                    ptr += len;
                    *ptr = (byte)',';
                    ptr++;
                    len = FastToString.ToStringNumber(ptr, v.V5);
                    ptr += len;
                    *ptr = (byte)',';
                    ptr++;
                    len = FastToString.ToStringNumber(ptr, v.V6);
                    ptr += len;
                    *ptr = (byte)',';
                    ptr++;
                    len = FastToString.ToStringNumber(ptr, v.V7);
                    ptr += len;
                    *ptr = (byte)',';
                    ptr++;
                    len = FastToString.ToStringNumber(ptr, v.V8);
                    ptr += len;
                    *ptr = (byte)',';
                    ptr++;
                    len = FastToString.ToStringNumber(ptr, v.V9);
                    ptr += len;
                    *ptr = (byte)'\n';
                    ptr++;
                }
                var size = header.Length + (int)(ptr - pd);
                return new ArraySegment<byte>(bytes, 0, size);
            }









        }

    }
}
