﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randelbrot
{
    class ContourRenderer : Renderer
    {
        public ContourRenderer() : base(null) { }
        public ContourRenderer(IRenderTracer tracer) : base(tracer) { }

        private DoubleComplexNumber temp = new DoubleComplexNumber(0.0, 0.0);

        // We access these *a lot* and I don't completely trust the compiler 
        // in any case other than having these be fields (though who knows, maybe the stack is faster)
        private int sizex, sizey;
        private int maxCount;
        private PixelBuffer buffer;
        private BandMap bandMap;

        // Returns 0 if out of bounds
        // Returns -1 for points inside the set (max count)
        // Returns 1 -> N for bands
        private int GetOrCalculateBand(int x, int y, out bool calculated)
        {
            calculated = false;
            if ((x < 0) || (y < 0) || (x >= sizex) || (y >= sizey))
            {
                return 0;
            }
            int band = this.buffer.GetValue(x, y);

            // If not yet computed
            if (band == 0)
            {
                calculated = true;
                temp.X = this.XCoordinates[x];
                temp.Y = this.YCoordinates[y];
                int count = temp.CalculateCount(this.maxCount);
                band = this.bandMap.Map(count);
                buffer.SetValue(x, y, band);
            }
            return band;
        }

        private bool Crawl(int firstX, int firstY, int band)
        {
            bool crawled = false;
	        int x,y,xinc,yinc;
            bool done = false;
	        xinc = 1;
	        x = firstX;y = firstY;
            bool calculated = false;
	        while(!done)
	        {
                if (this.GetOrCalculateBand(x + xinc, y, out calculated) != band)
                {
                    crawled = true;
                    yinc = xinc;
                }
                else
                {
                    yinc = -1 * xinc;
                    x += xinc;
                    done = ((firstX == x) && (firstY == y));
                }
                if (this.GetOrCalculateBand(x, y + yinc, out calculated) != band)
                {
                    crawled = true;
                    xinc = -1 * yinc;
                }
                else
                {
                    xinc = yinc;
                    y += yinc;
                    done = ((firstX == x) && (firstY == y));
                }
	        }
            return crawled;
        }


        // Returns 0 if out of bounds OR uninitialized
        // Note this is different than GetOrCal
        private int GetBand(int x, int y)
        {
            if ((x < 0) || (y < 0) || (x >= sizex) || (y >= sizey))
            {
                return 0;
            }
            int band = this.buffer.GetValue(x, y);

            return band;
        }

        private void FillToOtherSideOfBand(int x,int y,int inc, int band)
        {
	        int temp;

            int test = GetBand(x + inc, y);
	        if ((test == 0) || (test == band))
	        {
           //     this.DumpBuffer("StartFillToOtherSideOfBand", this.buffer);
		        temp = x+inc;
                test = GetBand(temp, y);
		        while((test == 0) || (test == band))
		        {
			        if (temp >= this.sizex) break;
			        if (temp < 1) break;
                    this.buffer.SetValue(temp, y, band);
                    temp += inc;
                    test = GetBand(temp, y);
		        }
	        }
        }

        private void FillCrawl(int firstX, int firstY, int band)
        {
            this.DumpBuffer("StartFillCrawl", this.buffer);
            int x, y, xinc, yinc;
            bool done = false;
            xinc = 1;
            x = firstX; y = firstY;
            while (!done)
            {
                if (this.GetBand(x + xinc, y) != band)
                    yinc = xinc;
                else
                {
                    yinc = -1 * xinc;
                    if (yinc < 0)
                        this.FillToOtherSideOfBand(x, y, 1, band);
                    else
                        this.FillToOtherSideOfBand(x, y, -1, band);
                    x += xinc;
                    done = ((firstX == x) && (firstY == y));
                }
                if (this.GetBand(x, y + yinc) != band)
                    xinc = -1 * yinc;
                else
                {
                    xinc = yinc;
                    y += yinc;
                    if (yinc < 0)
                        this.FillToOtherSideOfBand(x, y, 1, band);
                    else
                        this.FillToOtherSideOfBand(x, y, -1, band);
                    done = ((firstX == x) && (firstY == y));
                }
            }
            this.DumpBuffer("EndFillCrawl", this.buffer);
        }

        public override void Render(PixelBuffer buffer, MandelbrotSet set, BandMap bandMap, int maxCount)
        {
            this.sizex = buffer.SizeX;
            this.sizey = buffer.SizeY;
            this.maxCount = maxCount;
            this.bandMap = bandMap;
            this.buffer = buffer;
            this.InitializeCoordinateMap(sizex, sizey, set);

            for (int i = 0; i < sizex; i++)
            {
                // Keep track of the last band and how many pixels into that band we are
                // Start crawling after we see a few pixels of the same band
                int lastBand = 0;
                int numberOfPointsFoundInBand = 0;
                int startOfBand = 0;
                bool calculated = false;
                for (int j = 0; j < sizey; j++)
                {
                    int band = GetOrCalculateBand(i, j, out calculated);
                    System.Diagnostics.Debug.Assert(band != 0); // This should only be returned for points out of range, which should be none
                    if (calculated && (band == lastBand))
                    {
                        numberOfPointsFoundInBand++;
                    }
                    else
                    {
                        startOfBand = j;
                        lastBand = band;
                        numberOfPointsFoundInBand = 1;
                    }
                    // If we are 4 or more pixels into the band, start crawling
                    if (numberOfPointsFoundInBand > 4)
                    {
                        if (this.Crawl(i, startOfBand, band))
                        {
                            this.FillCrawl(i, startOfBand, band);
                        }
                        numberOfPointsFoundInBand = 0;
                        startOfBand = j + 1;
                    }
                }
            }

        }
    }
}