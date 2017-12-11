using System.Collections.Generic;
using System.Linq;

namespace DLib.Math.Function
{
    public abstract class Function
    {
        public abstract double GetY(double x);

        public abstract double Root();

        public abstract double[] Roots();

        public abstract override string ToString();

        public abstract Function GetDerivation();

        public abstract Function GetIntegral();

        public Function GetDerivation(int i)
        {
            if (i < 0)
                return GetIntegral(-i);
            var p = this;
            for (; i >= 0; i--)
                p = p.GetDerivation();
            return p;
        }

        public Function GetIntegral(int i)
        {
            if (i < 0)
                return GetDerivation(-i);
            var p = this;
            for (; i >= 0; i--)
                p = p.GetIntegral();
            return p;
        }

        public double[] Extrema()
        {
            Function d1 = GetDerivation(), d2 = d1.GetDerivation();
            return Roots().Where(d => d2.GetY(d) != 0).ToArray();
        }

        public double[] Maxima()
        {
            Function d1 = GetDerivation(), d2 = d1.GetDerivation();
            return Roots().Where(d => d2.GetY(d) < 0).ToArray();
        }

        public double[] Minima()
        {
            Function d1 = GetDerivation(), d2 = d1.GetDerivation();
            return Roots().Where(d => d2.GetY(d) > 0).ToArray();
        }

        public double[] Inflections()
        {
            Function d2 = GetDerivation(2), d3 = d2.GetDerivation();
            return d2.Roots().Where(d => d3.GetY(d) != 0).ToArray();
        }

        public double[] Saddles()
        {
            Function d1 = GetDerivation(), d2 = d1.GetDerivation(), d3 = d2.GetDerivation();
            return Roots().Where(d => d2.GetY(d) == 0 && d3.GetY(d) != 0).ToArray();
        }
    }
}
