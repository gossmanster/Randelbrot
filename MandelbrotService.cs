using System;

namespace Randelbrot
{
    public abstract class MandelbrotService
    {
        public abstract void RenderToBuffer(MandelbrotSet set, PixelBuffer buffer);
    }

    public class AdaptiveMandelbrotService : MandelbrotService
    {
        public AdaptiveMandelbrotService()
        {
        }

        public override void RenderToBuffer(MandelbrotSet set, PixelBuffer buffer)
        {
            DoubleRenderer renderer = new DoubleRenderer();
            Palette palette = new DefaultPalette();
            int maxCount = 1024;
            var bandMap = new LogarithmicBandMap(maxCount);

            renderer.Render(buffer, set, bandMap, palette, maxCount);
        }
    }
}
