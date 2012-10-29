using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public class DoubleRenderer
    {
        public DoubleRenderer() 
        {
            unchecked
            {
                this.black = (int)0xff000000;
            }
        }

        int black;

        virtual public void Render(PixelBuffer buffer, MandelbrotSet set, BandMap bandMap, Palette palette, int maxCount)
        {
            DoubleComplexNumber center = (DoubleComplexNumber)set.Center;
            double size = set.Side;
            double gapX = size / buffer.SizeX;
            double gapY = size / buffer.SizeY;
            double gap = Math.Min(gapX, gapY);

            DoubleComplexNumber temp = new DoubleComplexNumber(0.0, 0.0);
            double startx = center.X - ((gap * buffer.SizeX) / 2.0);
            double starty = center.Y - ((gap * buffer.SizeY) / 2.0);
            temp.X = startx;
            for (int i = 0; i < buffer.SizeX; i++)
            {
                temp.Y = starty;
                for (int j = 0; j < buffer.SizeY; j++)
                {
                    int count = temp.CalculateCount(maxCount);
                    this.SetColor(buffer, bandMap, palette, i, j, count, maxCount);
                    temp.Y += gap;
                }
                temp.X += gap;
            }
        }


        private void SetColor(PixelBuffer buffer, BandMap bandMap, Palette palette, int x, int y, int count, int maxCount)
        {
            int band = bandMap.Map(count);
            int color = palette.GetColor(band);
            if (count == maxCount)
                color = black;
            buffer.SetValue(x, y, color);
        }
    }
}
