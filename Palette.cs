using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public class Palette
    {
        public enum Style
        {
            Default
        }

        private Style style;
        private int[] colors;

        const int numberColors = 1024;
        public Palette(Style style)
        {
            this.colors = new int[numberColors];
            this.style = style;
            this.Initialize();
        }

        public int GetColor(int count)
        {
            return this.colors[count % numberColors];
        }

        private void Initialize()
        {
            int color;
            uint red, blue, green;
            for (uint i = 0; i < numberColors; i++)
            {
                red = i / 4;
                green = (i % 16) * 16;
                blue = (i % 64) * 4;

                unchecked
                {
                    color = 0xff << 24;
                    color |= (byte)(red & 0xff) << 16;
                    color |= (byte)(blue & 0xff) << 8;
                    color |= (byte)green;
                }
                this.colors[i] = color;
            }
        }


    }
}
