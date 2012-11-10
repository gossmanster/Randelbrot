using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    struct PriorityQueueItem<T>
    {
        public PriorityQueueItem(T element, double evaluationValue)
        {
            this.element = element;
            this.evaluationValue = evaluationValue;
        }
        public double evaluationValue;
        public T element;
    }

    public class PriorityQueue<T>
    {
        List<PriorityQueueItem<T>> theHeap;
        Func<T, double> evaluator;
        int size;
        public PriorityQueue(int size, Func<T,double> evaluator)
        {
            this.theHeap = new List<PriorityQueueItem<T>>(size + 1);
            this.theHeap.Add(new PriorityQueueItem<T>(default(T), 0.0));  // Reserve the 0th element
            this.evaluator = evaluator;
            this.size = size;
        }

        private int Parent(int i)
        {
            if (i == 1)
                return 1;
            return (i / 2);
        }

        private int LessChild(int i)
        {
            return Math.Min(i * 2, this.theHeap.Count-1);
        }

        private int GreaterChild(int i)
        {
            return Math.Min((i * 2) + 1, this.theHeap.Count-1);
        }

        private void Swap(int i, int j)
        {
            PriorityQueueItem<T> t = this.theHeap[i];
            this.theHeap[i] = this.theHeap[j];
            this.theHeap[j] = t;
        }

        private bool Compare(int i, int j)
        {
            return (this.theHeap[i].evaluationValue > this.theHeap[j].evaluationValue);
        }


        // FloatUp and SinkDown raise or lower items into their correct
        // position in the PriorityQueue
        private void FloatUp(int item)
        {
            int i = item;
            int j = this.Parent(i);
            while (this.Compare(i,j))
            {
                this.Swap(i,j);
                i = j;
                j = this.Parent(i);
            }
        }

        private void SinkDown(int item)
        {
            int len = this.theHeap.Count;
            int i = item;
            while (i < len)
            {
                int c1 = this.LessChild(i);
                int c2 = this.GreaterChild(i);
                int c = c2;
                if (this.Compare(c1, c))
                {
                    c = c1;
                }
                if (this.Compare(c, i))
                {
                    this.Swap(c, i);
                    i = c;
                }
                else
                {
                    break;
                }
            }
        }

        public void Push(T element)
        {
            var item = new PriorityQueueItem<T>(element, this.evaluator(element));
            this.theHeap.Add(item);
            this.FloatUp(this.theHeap.Count - 1);
        }

        public T Pop()
        {
            T retval = this.theHeap[1].element;
            var last = this.theHeap[this.theHeap.Count - 1];
            this.theHeap.RemoveAt(this.theHeap.Count - 1);
            if (this.theHeap.Count > 1)
            {
                this.theHeap[1] = last;
                this.SinkDown(1);
            }
            return retval;
        }

        public bool IsEmpty()
        {
            // Account for the 0th element
            return (this.theHeap.Count < 2);
        }

        public void TrimExcess()
        {
            if (this.theHeap.Count + 1 > this.size)
            {
                var newPQ = new PriorityQueue<T>(this.size, this.evaluator);
                for (int i = 0; i < this.size; i++)
                {
                    newPQ.Push(this.Pop());
                }
                this.theHeap = newPQ.theHeap;
            }
        }
    }

#if DEBUG
    public class PriorityQueueTests
    {
        private double Evaluate(double d)
        {
            return d;
        }

        public void Test1()
        {
            var pq = new PriorityQueue<double>(10, this.Evaluate);
            var random = new Random();
            for (int i = 1; i < 20; i++)
            {
                double v = random.NextDouble();
                pq.Push(v);
            }
            pq.TrimExcess();
            while (!pq.IsEmpty())
            {
                double d = pq.Pop();
                System.Diagnostics.Debug.WriteLine(d.ToString());
            }
        }

        public static void RunTests()
        {
            var thisPtr = new PriorityQueueTests();
            thisPtr.Test1();
        }

    }
#endif
}
