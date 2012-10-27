using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public class MandelbrotSet
    {
        public ComplexNumber Center { get; private set; }
        public ComplexNumber Side { get; private set; }
        public MandelbrotSet(ComplexNumber center, ComplexNumber side)
        {
            this.Center = center;
            this.Side = side;
        }
    }
}
