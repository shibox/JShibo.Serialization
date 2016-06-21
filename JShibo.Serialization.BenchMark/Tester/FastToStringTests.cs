using JShibo.Serialization.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class FastToStringTests
    {
        public unsafe static void RunIntToString()
        {
            char[] buffer = new char[1024];
            int pos = 0;
            int v = 5;
            string s = v.ToString();
            fixed(char* pd = &buffer[0])
            {
                Stopwatch w = Stopwatch.StartNew();
                for (int i = 0; i < 100000000; i++)
                {
                    pos = 0;
                    //FastToString.ToString1(pd, ref pos, v);
                    //s = v.ToString();
                    //int size = (v < 0) ? FastToString.StringSize(-v) + 1 : FastToString.StringSize(v);
                    //FastToString.getChars(v, size, buffer);
                    //pos = 10;// FastToString.StringSize(v);
                    //pos = FastToString.getChars(v, pd);
                    //FastToString.getChars(v, pos - 1, pd);
                    //pos = FastToString.ToStringFast(pd, pos, v);
                }
                w.Stop();
                if (new string(pd, 0, pos) != s)
                    Console.WriteLine("error");
                Console.WriteLine(w.ElapsedMilliseconds + " " + buffer);
            }
        }

        public unsafe static void RunUIntToString()
        {
            char[] buffer = new char[1024];
            int pos = 0;
            uint v = uint.MaxValue - int.MaxValue-1;
            //uint v = 52;
            string s = v.ToString();
            fixed (char* pd = &buffer[0])
            {
                Stopwatch w = Stopwatch.StartNew();
                for (int i = 0; i < 100000000; i++)
                {
                    pos = 0;
                    //FastToString.ToString2(pd, ref pos, v);
                    //s = v.ToString();
                    pos = FastToString.ToStringFast(pd, pos, v);
                }
                w.Stop();
                if (new string(pd, 0, pos) != s)
                    Console.WriteLine("error");
                Console.WriteLine(w.ElapsedMilliseconds + " " + buffer);
            }
        }

        public unsafe static void RunIntToStringValidity()
        {
            char[] buffer = new char[1024];
            int pos = 0;
            fixed (char* pd = &buffer[0])
            {
                Stopwatch w = Stopwatch.StartNew();
                int n = 0;
                for (int i = int.MinValue; i < int.MaxValue; i++)
                {
                    pos = 0;
                    FastToString.ToString1(pd, ref pos, i);
                    //if (new string(pd, 0, pos) != i.ToString())
                    //    Console.WriteLine("error" + i.ToString());
                    n++;
                    if (n % 100000000 == 0)
                        Console.WriteLine(n);
                }
                w.Stop();
                Console.WriteLine(w.ElapsedMilliseconds + " " + buffer);
            }
        }

        public static void Run()
        {
            //RunIntToStringValidity();
            //RunIntToString();
            RunUIntToString();
        }


    }
}
