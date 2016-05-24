using JShibo.Collections.Sort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark
{
    public class IntTopQueue
    {
        #region Int32

        private int size;
        private int maxSize;

        protected int[] ids;
        protected int[] scores;
        int score1 = 0;

        #endregion

        #region 构造函数

        public IntTopQueue()
            : this(2)
        {

        }

        public IntTopQueue(int maxSize)
        {
            size = 10;
            int heapSize;
            if (0 == maxSize)
                heapSize = 2;
            else
                heapSize = maxSize + 1;
            ids = new int[heapSize];
            scores = new int[heapSize];
            this.maxSize = maxSize;
        }

        public void Set(int size, int maxSize, int[] ids, int[] scores)
        {
            this.size = size;
            this.maxSize = maxSize;
            this.ids = ids;
            this.scores = scores;
        }

        public unsafe void Set(int size, int maxSize, int* ids, int* scores)
        {
            this.size = size;
            this.maxSize = maxSize;
            for (int i = 0; i < maxSize; i++)
            {
                this.ids[i] = ids[i];
                this.scores[i] = scores[i];
            }
        }

        #endregion

        #region 方法

        public TopScoreDoc[] GetTops(int offset, int num)
        {
            TopScoreDoc[] retVal = new TopScoreDoc[0];
            do
            {
                if (num == 0) break;
                int size = Size();
                if (size == 0) break;

                if (offset < 0 || offset >= size)
                {
                    //throw new IllegalArgumentException("Invalid offset: " + offset);
                }

                TopScoreDoc[] fieldDocs = new TopScoreDoc[size];
                for (int i = size - 1; i >= 0; i--)
                {
                    fieldDocs[i] = Pop();
                }
                /*
                if (logger.isDebugEnabled()){
                    for (int i=0;i<fieldDocs.length;++i){
                        logger.debug(fieldDocs[i]);
                    }
                }
                */

                int count = Math.Min(num, (size - offset));
                retVal = new TopScoreDoc[count];
                int n = offset + count;
                // if distance is there for 1 hit, it's there for all
                for (int i = offset; i < n; ++i)
                {
                    TopScoreDoc hit = fieldDocs[i];
                    retVal[i - offset] = hit;
                }
            } while (false);
            return retVal;
        }

        public void Put(int id, int score)
        {
            size++;
            ids[size] = id;
            scores[size] = score;
            if (size == 0)
                score1 = score;
            UpHeap();
        }

        public void Insert(int id, int score)
        {
            if (size < maxSize)
            {
                Put(id, score);
            }
            else if (size > 0 && score > scores[1])
            {
                ids[1] = id;
                scores[1] = score;
                DownHeap();
            }
        }

        /// <summary>
        /// 有预测性的快速添加，提高性能
        /// </summary>
        /// <param name="id"></param>
        /// <param name="score"></param>
        public void FastInsert(int id, int score)
        {
            //if (score > score1)// scores[1])
            //{
            //    ids[1] = id;
            //    scores[1] = score;
            //    score1 = score;
            //    DownHeap();
            //}



            if (score > score1)
            {
                //ids[1] = id;
                //scores[1] = score;
                //score1 = score;
                //DownHeap(id,score);

                int i = 1;
                int j = 2;
                int k = 3;
                if (scores[k] <= scores[j])
                {
                    j = k;
                }
                while (j <= size && scores[j] <= score)// lessThan(ids[j], scores[j], id, score))
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

        public TopScoreDoc Pop()
        {
            //if (size > 0)
            //{
            //    ScoreDoc result = heap[1];			  // save first value
            //    heap[1] = heap[size];			  // move last to first
            //    heap[size] = null;			  // permit GC of objects
            //    size--;
            //    downHeap();				  // adjust heap
            //    return result;
            //}
            //else
            //    return null;


            TopScoreDoc result = new TopScoreDoc(ids[1], scores[1]);			  // save first value
            ids[1] = ids[size];			  // move last to first
            scores[1] = scores[size];
            //heap[size] = null;			  // permit GC of objects
            size--;
            DownHeap();				  // adjust heap
            return result;
        }

        public int Size()
        {
            return size;
        }

        public void Clear()
        {
            //for (int i = 0; i <= size; i++)
            //    heap[i] = null;
            size = 0;
        }

        private void UpHeap()
        {
            //int i = size;
            //ScoreDoc node = heap[i];			  // save bottom node
            //int j = i >> 1;
            //while (j > 0 && lessThan(node, heap[j]))
            //{
            //    heap[i] = heap[j];			  // shift parents down
            //    i = j;
            //    j = j >> 1;
            //}
            //heap[i] = node;				  // install saved node

            int i = size;
            int id = ids[i];
            int score = scores[i];
            //ScoreDoc node = heap[i];			  // save bottom node
            int j = i >> 1;
            while (j > 0 && score <= scores[j])// lessThan(id,score, ids[j],scores[j]))
            {
                //heap[i] = heap[j];			  // shift parents down

                ids[i] = ids[j];
                scores[i] = scores[j];
                i = j;
                j = j >> 1;
            }
            ids[i] = id;
            scores[i] = score;
            //heap[i] = node;	
        }

        private void DownHeap()
        {
            int i = 1;
            int id = ids[i];
            int score = scores[i];
            int j = i << 1;				  // find smaller child
            int k = j + 1;
            if (k <= size && scores[k] <= scores[j])
            {
                j = k;
            }
            while (j <= size && scores[j] <= score)// lessThan(ids[j], scores[j], id, score))
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
        }

        private void DownHeap(int id,int score)
        {
            //int i = 1;
            //int id = ids[i];
            //int score = scores[i];
            //int j = i << 1;				  // find smaller child
            //int k = j + 1;

            //int i = 1;
            //int j = 2;
            //int k = 3;
            //if (k <= size && scores[k] <= scores[j])
            //{
            //    j = k;
            //}
            //while (j <= size && scores[j] <= score)// lessThan(ids[j], scores[j], id, score))
            //{
            //    ids[i] = ids[j];
            //    scores[i] = scores[j];
            //    i = j;
            //    j = i << 1;
            //    k = j + 1;
            //    if (k <= size && scores[k] <= scores[j])
            //    {
            //        j = k;
            //    }
            //}
            //ids[i] = id;
            //scores[i] = score;


            int i = 1;
            int j = 2;
            int k = 3;
            if (k <= size && scores[k] <= scores[j])
            {
                j = k;
            }
            while (j <= size && scores[j] <= score)// lessThan(ids[j], scores[j], id, score))
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
        }

        #endregion
    }

    public class IntTopQueueUnsafe
    {
        #region Int32

        private int size;
        private int maxSize;

        protected int[] ids;
        protected int[] scores;
        int score1 = 0;

        #endregion

        #region 构造函数

        public IntTopQueueUnsafe()
            : this(2)
        {

        }

        public IntTopQueueUnsafe(int maxSize)
        {
            size = 10;
            int heapSize;
            if (0 == maxSize)
                heapSize = 2;
            else
                heapSize = maxSize + 1;
            ids = new int[heapSize];
            scores = new int[heapSize];
            this.maxSize = maxSize;
        }

        //public void Set(int size, int maxSize, int[] ids, int[] scores)
        //{
        //    this.size = size;
        //    this.maxSize = maxSize;
        //    this.ids = ids;
        //    this.scores = scores;
        //}

        //public unsafe void Set(int size, int maxSize, int* ids, int* scores)
        //{
        //    this.size = size;
        //    this.maxSize = maxSize;
        //    for (int i = 0; i < maxSize; i++)
        //    {
        //        this.ids[i] = ids[i];
        //        this.scores[i] = scores[i];
        //    }
        //}

        #endregion

        #region 方法

        public TopScoreDoc[] GetTops(int offset, int num)
        {
            TopScoreDoc[] retVal = new TopScoreDoc[0];
            do
            {
                if (num == 0) break;
                int size = Size();
                if (size == 0) break;

                if (offset < 0 || offset >= size)
                {
                    //throw new IllegalArgumentException("Invalid offset: " + offset);
                }

                TopScoreDoc[] fieldDocs = new TopScoreDoc[size];
                for (int i = size - 1; i >= 0; i--)
                {
                    fieldDocs[i] = Pop();
                }
                /*
                if (logger.isDebugEnabled()){
                    for (int i=0;i<fieldDocs.length;++i){
                        logger.debug(fieldDocs[i]);
                    }
                }
                */

                int count = Math.Min(num, (size - offset));
                retVal = new TopScoreDoc[count];
                int n = offset + count;
                // if distance is there for 1 hit, it's there for all
                for (int i = offset; i < n; ++i)
                {
                    TopScoreDoc hit = fieldDocs[i];
                    retVal[i - offset] = hit;
                }
            } while (false);
            return retVal;
        }

        public void Put(int id, int score)
        {
            size++;
            ids[size] = id;
            scores[size] = score;
            if (size == 0)
                score1 = score;
            UpHeap();
        }

        public void Insert(int id, int score)
        {
            if (size < maxSize)
            {
                Put(id, score);
            }
            else if (size > 0 && score > scores[1])
            {
                ids[1] = id;
                scores[1] = score;
                DownHeap();
            }
        }

        /// <summary>
        /// 有预测性的快速添加，提高性能
        /// </summary>
        /// <param name="id"></param>
        /// <param name="score"></param>
        //public void FastInsert(int id, int score)
        //{
        //    if (score > score1)
        //    {
        //        int i = 1;
        //        int j = 2;
        //        int k = 3;
        //        if (scores[k] <= scores[j])
        //        {
        //            j = k;
        //        }
        //        while (j <= size && scores[j] <= score)
        //        {
        //            ids[i] = ids[j];
        //            scores[i] = scores[j];
        //            i = j;
        //            j = i << 1;
        //            k = j + 1;
        //            if (k <= size && scores[k] <= scores[j])
        //            {
        //                j = k;
        //            }
        //        }
        //        ids[i] = id;
        //        scores[i] = score;
        //        score1 = scores[1];
        //    }
        //}

        public unsafe void FastInsert(int[] keys, int[] values)
        {
            int id = 0, score = 0;
            int* ids = stackalloc int[10];
            int* scores = stackalloc int[10];

            for (int n = 0; n < keys.Length; n++)
            {

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
            
        }

        public TopScoreDoc Pop()
        {
            //if (size > 0)
            //{
            //    ScoreDoc result = heap[1];			  // save first value
            //    heap[1] = heap[size];			  // move last to first
            //    heap[size] = null;			  // permit GC of objects
            //    size--;
            //    downHeap();				  // adjust heap
            //    return result;
            //}
            //else
            //    return null;


            TopScoreDoc result = new TopScoreDoc(ids[1], scores[1]);			  // save first value
            ids[1] = ids[size];			  // move last to first
            scores[1] = scores[size];
            //heap[size] = null;			  // permit GC of objects
            size--;
            DownHeap();				  // adjust heap
            return result;
        }

        public int Size()
        {
            return size;
        }

        public void Clear()
        {
            //for (int i = 0; i <= size; i++)
            //    heap[i] = null;
            size = 0;
        }

        private void UpHeap()
        {
            //int i = size;
            //ScoreDoc node = heap[i];			  // save bottom node
            //int j = i >> 1;
            //while (j > 0 && lessThan(node, heap[j]))
            //{
            //    heap[i] = heap[j];			  // shift parents down
            //    i = j;
            //    j = j >> 1;
            //}
            //heap[i] = node;				  // install saved node

            int i = size;
            int id = ids[i];
            int score = scores[i];
            //ScoreDoc node = heap[i];			  // save bottom node
            int j = i >> 1;
            while (j > 0 && score <= scores[j])// lessThan(id,score, ids[j],scores[j]))
            {
                //heap[i] = heap[j];			  // shift parents down

                ids[i] = ids[j];
                scores[i] = scores[j];
                i = j;
                j = j >> 1;
            }
            ids[i] = id;
            scores[i] = score;
            //heap[i] = node;	
        }

        private void DownHeap()
        {
            int i = 1;
            int id = ids[i];
            int score = scores[i];
            int j = i << 1;				  // find smaller child
            int k = j + 1;
            if (k <= size && scores[k] <= scores[j])
            {
                j = k;
            }
            while (j <= size && scores[j] <= score)// lessThan(ids[j], scores[j], id, score))
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
        }

        private void DownHeap(int id, int score)
        {
            //int i = 1;
            //int id = ids[i];
            //int score = scores[i];
            //int j = i << 1;				  // find smaller child
            //int k = j + 1;

            //int i = 1;
            //int j = 2;
            //int k = 3;
            //if (k <= size && scores[k] <= scores[j])
            //{
            //    j = k;
            //}
            //while (j <= size && scores[j] <= score)// lessThan(ids[j], scores[j], id, score))
            //{
            //    ids[i] = ids[j];
            //    scores[i] = scores[j];
            //    i = j;
            //    j = i << 1;
            //    k = j + 1;
            //    if (k <= size && scores[k] <= scores[j])
            //    {
            //        j = k;
            //    }
            //}
            //ids[i] = id;
            //scores[i] = score;


            int i = 1;
            int j = 2;
            int k = 3;
            if (k <= size && scores[k] <= scores[j])
            {
                j = k;
            }
            while (j <= size && scores[j] <= score)// lessThan(ids[j], scores[j], id, score))
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
        }

        #endregion
    }

    public class IntScoreTopQueue
    {
        #region Int32

        private int size;
        private int maxSize;

        protected int[] ids;
        protected int[] scores;
        int score1 = 0;

        #endregion

        #region 构造函数

        public IntScoreTopQueue()
            : this(2)
        {

        }

        public IntScoreTopQueue(int maxSize)
        {
            size = 10;
            int heapSize;
            if (0 == maxSize)
                heapSize = 2;
            else
                heapSize = maxSize + 1;
            ids = new int[heapSize];
            scores = new int[heapSize];
            this.maxSize = maxSize;
        }

        public void Set(int size, int maxSize, int[] ids, int[] scores)
        {
            this.size = size;
            this.maxSize = maxSize;
            this.ids = ids;
            this.scores = scores;
        }

        public unsafe void Set(int size, int maxSize, int* ids, int* scores)
        {
            this.size = size;
            this.maxSize = maxSize;
            for (int i = 0; i < maxSize; i++)
            {
                this.ids[i] = ids[i];
                this.scores[i] = scores[i];
            }
        }

        #endregion

        #region 方法

        public TopScoreDoc[] GetTops(int offset, int num)
        {
            TopScoreDoc[] retVal = new TopScoreDoc[0];
            do
            {
                if (num == 0) break;
                int size = Size();
                if (size == 0) break;

                if (offset < 0 || offset >= size)
                {
                    //throw new IllegalArgumentException("Invalid offset: " + offset);
                }

                TopScoreDoc[] fieldDocs = new TopScoreDoc[size];
                for (int i = size - 1; i >= 0; i--)
                {
                    fieldDocs[i] = Pop();
                }
                /*
                if (logger.isDebugEnabled()){
                    for (int i=0;i<fieldDocs.length;++i){
                        logger.debug(fieldDocs[i]);
                    }
                }
                */

                int count = Math.Min(num, (size - offset));
                retVal = new TopScoreDoc[count];
                int n = offset + count;
                // if distance is there for 1 hit, it's there for all
                for (int i = offset; i < n; ++i)
                {
                    TopScoreDoc hit = fieldDocs[i];
                    retVal[i - offset] = hit;
                }
            } while (false);
            return retVal;
        }

        public void Put(int id, int score)
        {
            size++;
            ids[size] = id;
            scores[size] = score;
            if (size == 0)
                score1 = score;
            UpHeap();
        }

        public void Insert(int id, int score)
        {
            if (size < maxSize)
            {
                Put(id, score);
            }
            else if (size > 0 && score > scores[1])
            {
                ids[1] = id;
                scores[1] = score;
                DownHeap();
            }
        }

        /// <summary>
        /// 有预测性的快速添加，提高性能
        /// </summary>
        /// <param name="id"></param>
        /// <param name="score"></param>
        public void FastInsert(int id, int score)
        {
            //if (score > score1)// scores[1])
            //{
            //    ids[1] = id;
            //    scores[1] = score;
            //    score1 = score;
            //    DownHeap();
            //}
            if (score > scores[1])
            {
                //ids[1] = id;
                //scores[1] = score;
                //score1 = score;
                DownHeap(id, score);
            }
        }

        public TopScoreDoc Pop()
        {
            //if (size > 0)
            //{
            //    ScoreDoc result = heap[1];			  // save first value
            //    heap[1] = heap[size];			  // move last to first
            //    heap[size] = null;			  // permit GC of objects
            //    size--;
            //    downHeap();				  // adjust heap
            //    return result;
            //}
            //else
            //    return null;


            TopScoreDoc result = new TopScoreDoc(ids[1], scores[1]);			  // save first value
            ids[1] = ids[size];			  // move last to first
            scores[1] = scores[size];
            //heap[size] = null;			  // permit GC of objects
            size--;
            DownHeap();				  // adjust heap
            return result;
        }

        public int Size()
        {
            return size;
        }

        public void Clear()
        {
            //for (int i = 0; i <= size; i++)
            //    heap[i] = null;
            size = 0;
        }

        private void UpHeap()
        {
            //int i = size;
            //ScoreDoc node = heap[i];			  // save bottom node
            //int j = i >> 1;
            //while (j > 0 && lessThan(node, heap[j]))
            //{
            //    heap[i] = heap[j];			  // shift parents down
            //    i = j;
            //    j = j >> 1;
            //}
            //heap[i] = node;				  // install saved node

            int i = size;
            int id = ids[i];
            int score = scores[i];
            //ScoreDoc node = heap[i];			  // save bottom node
            int j = i >> 1;
            while (j > 0 && score <= scores[j])// lessThan(id,score, ids[j],scores[j]))
            {
                //heap[i] = heap[j];			  // shift parents down

                ids[i] = ids[j];
                scores[i] = scores[j];
                i = j;
                j = j >> 1;
            }
            ids[i] = id;
            scores[i] = score;
            //heap[i] = node;	
        }

        private void DownHeap()
        {
            int i = 1;
            int id = ids[i];
            int score = scores[i];
            int j = i << 1;				  // find smaller child
            int k = j + 1;
            if (k <= size && scores[k] <= scores[j])
            {
                j = k;
            }
            while (j <= size && scores[j] <= score)// lessThan(ids[j], scores[j], id, score))
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
        }

        private void DownHeap(int id, int score)
        {
            //int i = 1;
            //int id = ids[i];
            //int score = scores[i];
            //int j = i << 1;				  // find smaller child
            //int k = j + 1;

            //int i = 1;
            //int j = 2;
            //int k = 3;
            //if (k <= size && scores[k] <= scores[j])
            //{
            //    j = k;
            //}
            //while (j <= size && scores[j] <= score)// lessThan(ids[j], scores[j], id, score))
            //{
            //    ids[i] = ids[j];
            //    scores[i] = scores[j];
            //    i = j;
            //    j = i << 1;
            //    k = j + 1;
            //    if (k <= size && scores[k] <= scores[j])
            //    {
            //        j = k;
            //    }
            //}
            //ids[i] = id;
            //scores[i] = score;


            int i = 1;
            int j = 2;
            int k = 3;
            if (k <= size && scores[k] <= scores[j])
            {
                j = k;
            }
            while (j <= size && scores[j] <= score)// lessThan(ids[j], scores[j], id, score))
            {
                //ids[i] = ids[j];
                scores[i] = scores[j];
                i = j;
                j = i << 1;
                k = j + 1;
                if (k <= size && scores[k] <= scores[j])
                {
                    j = k;
                }
            }
            //ids[i] = id;
            scores[i] = score;
        }

        #endregion
    }
}
