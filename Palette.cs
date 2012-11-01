﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    public abstract class Palette
    {
        protected int[] Colors { get; private set; }

        protected int ColorCount { get; private set; }
        private Palette() { }

        protected Palette(int numberColors)
        {
            this.ColorCount = numberColors;
            this.Colors = new int[numberColors];
        }

        public int GetColor(int count)
        {
            int entry = count % this.ColorCount;
            return this.Colors[entry];
        }

        public virtual int GetMaxCountColor()
        {
            unchecked
            {
                // Opaque Black
                return (int)(0xff000000);
            }
        }
    }

    public class DefaultPalette : Palette
    {
        const int numberColors = 1024;
        public DefaultPalette()
            : base(numberColors)
        {
            int color;
            uint red, blue, green;
            for (uint i = 0; i < numberColors; i++)
            {
                double radianNorm = 3.0 / numberColors;
                double b = i * radianNorm;
                blue = (uint)(Math.Abs(Math.Sin(b)) * 256.0);
                red = (i % 48) * 4 + 64;
                green = (i % 64) * 4;

                unchecked
                {
                    color = 0xff << 24;
                    color |= (byte)(red & 0xff) << 16;
                    color |= (byte)(blue & 0xff) << 8;
                    color |= (byte)green;
                }
                this.Colors[i] = color;
            }
        }
    }
}
