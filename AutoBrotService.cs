using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public class AutoBrotService
    {
        private PriorityList<MandelbrotSet> candidates;
        private BeautyEvaluator evaluator;
        private Random random = new Random();


        public AutoBrotService(MandelbrotSet startSet, BeautyEvaluator evaluator)
        {
            this.evaluator = evaluator;
            this.candidates = new PriorityList<MandelbrotSet>(1000, evaluator.Evaluate);
            this.candidates.Push(startSet);
        }

        private MandelbrotSet randomChild(MandelbrotSet set)
        {
            double newSide = (this.random.NextDouble() * set.Side / 2) + set.Side / 4;
            double newCX = ((this.random.NextDouble() - 0.5) * set.Side / 2) + set.Center.X;
            double newCY = ((this.random.NextDouble() - 0.5) * set.Side / 2) + set.Center.Y;
            var newSet = new MandelbrotSet(new DoubleComplexNumber(newCX, newCY), newSide);
            return newSet;
        }

        private List<MandelbrotSet> generateCandidates(MandelbrotSet set)
        {
            var retval = new List<MandelbrotSet>(10);

            var newSet = new MandelbrotSet(set.Center, set.Side / 2);
            retval.Add(newSet);
            newSet = new MandelbrotSet(set.Center, set.Side * 0.7);
            retval.Add(newSet);
            newSet = new MandelbrotSet(new DoubleComplexNumber(set.Center.X - set.Side / 4, set.Center.Y - set.Side / 4), set.Side / 2);
            retval.Add(newSet);
            newSet = new MandelbrotSet(new DoubleComplexNumber(set.Center.X + set.Side / 4, set.Center.Y - set.Side / 4), set.Side / 2);
            retval.Add(newSet);
            newSet = new MandelbrotSet(new DoubleComplexNumber(set.Center.X - set.Side / 4, set.Center.Y + set.Side / 4), set.Side / 2);
            retval.Add(newSet);
            newSet = new MandelbrotSet(new DoubleComplexNumber(set.Center.X + set.Side / 4, set.Center.Y + set.Side / 4), set.Side / 2);
            retval.Add(newSet);

            for (int i = 0; i < 5; i++)
            {
                retval.Add(this.randomChild(set));
            }
            return retval;
        }

        public MandelbrotSet Pop()
        {
            return this.candidates.Pop();
        }

        public MandelbrotSet Peek()
        {
            return this.candidates.Peek();
        }

        public void Generate()
        {
            var newCandidates = this.generateCandidates(this.candidates.Peek());
            this.candidates.Push(newCandidates);
        }

        public MandelbrotSet PopAndGenerate()
        {
            var newCandidates = this.generateCandidates(this.candidates.Peek());
            var retval = this.candidates.Pop();
            this.candidates.Push(newCandidates);

            return retval;
        }

        public int Count
        {
            get
            {
                return this.candidates.Count;
            }
        }

        public MandelbrotSet this[int i]
        {
            get
            {
                return this.candidates[i];
            }
        }

        public double Evaluation(int i)
        {
            return this.candidates.Evaluation(i);
        }
    }
}
