using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    abstract class BeautyEvaluator
    {
        public abstract double Evaluate(MandelbrotSet set);
    }
}
