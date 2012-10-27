using System;
using System.Collections.Generic;

namespace Randelbrot
{
    // Abstracted to allow for other representations like fixed-point numbers
    public abstract class ComplexNumber
    {
        public abstract int CalculateCount(int maxCount);
        public abstract void Add(ComplexNumber other);
        public abstract ComplexNumber Clone();
        public static ComplexNumber operator+(ComplexNumber left, ComplexNumber right)
        {
            ComplexNumber retval = left.Clone();
            retval.Add(right);
            return retval;
        }
    }

    // Specific implementation for double based math
    public class DoubleComplexNumber : ComplexNumber
    {
        public double X { get;  set; }
        public double Y { get; set; }
        public DoubleComplexNumber(double x, double y)
        {
            this.X = x; this.Y = y;
        }
        public DoubleComplexNumber(DoubleComplexNumber other)
        {
            this.X = other.X; this.Y = other.Y;
        }

        public void Add(DoubleComplexNumber other)
        {
            this.X += other.X;
            this.Y += other.Y;
        }

        // Overrides from ComplexNumber
        public override ComplexNumber Clone()
        {
            return new DoubleComplexNumber(this);
        }
        public override void Add(ComplexNumber other)
        {
            DoubleComplexNumber doubleOther = (DoubleComplexNumber)other;
            this.Add(doubleOther);
        }
        public override int CalculateCount(int maxCount)
        {
            double tx = this.X;
            double ty = this.Y;
            double x2, y2;
            int count = 0;

            x2 = tx * tx; y2 = ty * ty;

            while ((x2 + y2 < 4.0) && (count < maxCount))
            {
                ty = 2 * tx * ty + this.Y;
                tx = x2 - y2 + this.X;
                x2 = tx * tx;
                y2 = ty * ty;
                count++;
            }
            return (count);
        }
    }   
}
