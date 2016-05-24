using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class TopQueueTester
    {
        public unsafe static void TestCase1()
        {
            int n = 10000000;
            int v = 0;
            IntTopQueue queue = new IntTopQueue(10);
            Stopwatch w = Stopwatch.StartNew();
            
            //IntScoreTopQueue queue = new IntScoreTopQueue(10);
            //for (int i = 0; i < n; i++)
            //{
            //    //queue.Insert(i, n - i);
            //    //queue.FastInsert(i, i);
            //    //v+=n-1;


            //}

            int[] ids = FastInsert(new int[] { }, new int[] { });
            w.Stop();
            Console.WriteLine("cost:" + w.ElapsedMilliseconds+ "  " + v);
            for (int m = 0; m < ids.Length; m++)
                Console.WriteLine(ids[m]);

            //Console.WriteLine(JsonConvert.SerializeObject(queue.GetTops(0, 10), Formatting.Indented));
        }

        public static unsafe int[] FastInsert(int[] keys, int[] values)
        {
            int score1 = 0, size = 10;
            int id = 0, score = 0;
            int* ids = stackalloc int[size+1];
            int* scores = stackalloc int[size+1];
            //int[] ids = new int[size + 1];
            //int[] scores = new int[size + 1];

            for (int n = 0; n < 10000000; n++)
            {
                id = score =  n;
                if (score > score1)
                {
                    int i = 1;
                    int j = 2;
                    int k = 3;
                    if (scores[k] <= scores[j])
                    {
                        j = k;
                    }
                    while (j <= size && scores[j] <= score)
                    {
                        ids[i] = ids[j];
                        scores[i] = scores[j];
                        i = j;
                        j = i << 1;
                        k = j + 1;
                        if (k <= size && scores[k] <= scores[j])
                        {
                            j = k;
                        }
                    }
                    ids[i] = id;
                    scores[i] = score;
                    score1 = scores[1];
                }
            }

            int[] result = new int[size];
            for (int m = 0; m < size; m++)
                result[m] = ids[m];
            return result;
        }

    }
}
