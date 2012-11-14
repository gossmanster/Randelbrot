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
        private int size = 80;
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

            // The more contours, the more interesting
            retval = this.ContourRenderer.NumberOfContours;

            var histogram = new Histogram();
            histogram.Evaluate(this.Buffer);
            int pointsInSet = histogram.GetValue(-1);

            // If at least some points in the set appear, it is more interesting
            if (pointsInSet > 0)
            {

                retval *= 1.6;

                // Reduce the interest if there are too many points in the set
                double r = ((double)(size * size) / pointsInSet) / 100;
                retval += r;
            }

            // The more different colors the more interesting.  Scaled so we don't get too complex.
            // Use negative to make more colors less interesting, thus reducing noisy areas
             retval += histogram.NumberOfValues / 9.0;

            // All else being equal, favor pictures of lower estimated count and thus speed
            // This is also a zoom avoidance feature
            retval -= set.EstimateMaxCount() / 100;

            if (pointsInSet > 6300)
            {
                pointsInSet++;
            }
            return retval;
        }
    }
}
