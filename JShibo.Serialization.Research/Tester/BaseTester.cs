using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JShibo.Serialization.Common;
using JShibo.Serialization.BenchMark.Entitiy;
using Newtonsoft.Json;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class BaseTester
    {
        #region 基础测试

        public static void Int32Parser()
        {
            string s = "123456789,";
            int v = 0;
            int pos = 0;
            Stopwatch w = Stopwatch.StartNew();
            for (int j = 0; j < 10000000; j++)
            {
                //v = int.Parse(s);
                //v = FastToValue.ToInt32(s, ref pos);
                //v = FastToValue.ToInt32(s);
                //v = FastToValue.ToInt32Fast(s, ref pos);
                pos = 0;
                //v = FastToValue.ToInt32For(s);
            }
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds + "-" + v);
        }

        public unsafe static void Int32Writer()
        {
            int v = -534011720;
            char[] buffer = new char[32];
            int count = 6;
            Stopwatch w = Stopwatch.StartNew();
            for (int j = 0; j < 1000000; j++)
            {
                //v.ToString();
                //v = int.Parse(s);
                //v = FastToValue.ToInt32(s, ref pos);
                //FastToValue.CustomWriteInt(v, buffer);
                //FastToString.ToStringSimple(buffer, 0, v);
                count += FastToString.ToStringFast(buffer, count, v);
                //FastToString.ToStringFast(buffer,ref count, v);
                //fixed (char* pd = &buffer[count])
                //{
                //    count += FastToString.ToStringFast(pd, count, v);

                //}


                //FastToString.CustomWriteUInt(buffer, 0, v);
                //FastToString.ToString(buffer, 0, v);
                //v = FastToValue.ToInt32For(s);
                //string s = FastToString.ToString(v);
                //Console.WriteLine(s);
                count = 6;
            }
            w.Stop();
            Console.WriteLine(buffer);
            Console.WriteLine(new string(buffer,0,count));
            Console.WriteLine(w.ElapsedMilliseconds + "     " + v);
        }

        public static void GuidWriter()
        {
            Guid guid = Guid.NewGuid();
            Console.WriteLine(guid);

            char[] buffer = new char[50];
            Stopwatch w = Stopwatch.StartNew();
            for (int j = 0; j < 10000000; j++)
            {
                //v = int.Parse(s);
                //v = FastToValue.ToInt32(s, ref pos);
                FastToString.WriteGuid(guid, buffer);
                //FastToString.WriteGuid(buffer,0,guid);
                //FastToString.ToString(buffer, 0, guid);
                //v = FastToValue.ToInt32For(s);
                //guid.ToString();
            }
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds + "-" + guid);
            Console.WriteLine(new string(buffer, 0, 36));
        }

        public unsafe static void DateTimeWriter()
        {
            //DateTime[] times = new DateTime[10];
            //for (int i = 0; i < times.Length; i++)
            //{
            //    times[i] = DateTime.Now;
            //}
            //int n = sizeof(int);
            //double[] times = new double[10];
            //for (int i = 0; i < times.Length; i++)
            //{
            //    times[i] = 446;
            //}
            decimal v = 451254;
            int[] ints = decimal.GetBits(v);
            byte[] _buffer = new byte[300];
            Buffer.BlockCopy(ints, 0, _buffer, 0, ints.Length * 4);

            decimal value = 0;
            fixed (byte* pd = &_buffer[0])
                value = *((decimal*)pd);
            
            //DecimalStruct s = new DecimalStruct(v);

            //decimal d = new decimal(
            Console.WriteLine(v);

            //byte[] _buffer = new byte[300];
            //fixed (byte* pd = &_buffer[0])
            //{
            //    fixed (int* psrc = &v)
            //    {
            //        int* src = psrc;
            //        *((int*)pd) = *src++;
            //        *((int*)(pd + 4)) = *src++;
            //        *((int*)(pd + 8)) = *src++;
            //        *((int*)(pd + 12)) = *src++;
            //    }
            //}


            //Buffer.BlockCopy(times, 0, bytes, 0, times.Length * 8);
            //Console.WriteLine(bytes.Length);
        }

        public static void MultipleTypeMethodTest()
        {
            PrimitiveTypeClass v = ShiboSerializer.Initialize<PrimitiveTypeClass>(4578424);
            string json = ShiboSerializer.Serialize(v);
            byte[] bytes = ShiboSerializer.BinarySerialize(v);

            Console.WriteLine(json == JsonConvert.SerializeObject(v));
            v = ShiboSerializer.BinaryDeserialize<PrimitiveTypeClass>(bytes);
            Console.WriteLine(json == JsonConvert.SerializeObject(v));
        }

        #endregion

        public static void Run()
        {
            //Int32Parser();
            //Int32Writer();
            //GuidWriter();

            //DateTimeWriter();
            //MultipleTypeMethodTest();
        }
    }
}
