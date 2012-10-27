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
                this.white = (int)0xffffffff;
                this.black = (int)0xff000000;
            }
        }

        int white, black;

        public void Render(PixelBuffer buffer, MandelbrotSet set, Palette palette, int maxCount)
        {
            DoubleComplexNumber center = (DoubleComplexNumber)set.Center;
            DoubleComplexNumber size = (DoubleComplexNumber)set.Side;
            double gapX = size.X / buffer.SizeX;
            double gapY = size.Y / buffer.SizeY;
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
                    this.SetColor(buffer, palette, i, j, count, maxCount);
                    temp.Y += gap;
                }
                temp.X += gap;
            }
        }

        private void SetColor(PixelBuffer buffer, Palette palette, int x, int y, int count, int maxCount)
        {
            int color = palette.GetColor(count);
            if (count == maxCount)
                color = black;
            buffer.SetValue(x, y, color);
        }
    }
}
