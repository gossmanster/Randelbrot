using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public abstract class BeautyEvaluator
    {
        public abstract double Evaluate(MandelbrotSet set);
    }

    public class DefaultBeautyEvaluator : BeautyEvaluator
    {
        private ContourRenderer ContourRenderer { get; set; }
        private PixelBuffer Buffer { get; set; }
        private BandMap BandMap { get; set; }
        private int size = 50;
        public DefaultBeautyEvaluator()
        {
            this.ContourRenderer = new ContourRenderer();
            this.Buffer = new PixelBuffer(size, size);        
        }
        public override double Evaluate(MandelbrotSet set)
        {
            double retval = 0.0;
            var bandMap = new LogarithmicBandMap(set.EstimateMaxCount());

            this.Buffer.Clear();
            this.ContourRenderer.Render(this.Buffer, set, bandMap, set.EstimateMaxCount());

            retval = this.ContourRenderer.NumberOfContours;

            var histogram = new Histogram();
            histogram.Evaluate(this.Buffer);
            int pointsInSet = histogram.GetValue(-1);
            if (pointsInSet > 0)
            {
                retval *= 1.6;
                if (pointsInSet < (0.3 * size * size))
                {
                    retval *= 1.6;
                }
            }

            return retval;
        }
    }
}
