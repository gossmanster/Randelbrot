using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public interface IRenderTracer
    {
        void DumpBits(string message, PixelBuffer buffer);
    }
}
