using DLib.Math;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Math.Function
{
    public abstract class Function
    {
        public abstract double Y(double x);

        public abstract Function Derivate();

        public abstract Function Integrate();

        public Function Derivate(int i)
        {
            if (i < 0)
                return Integrate(-i);
            var p = this;
            for (; i >= 0; i--)
                p = p.Derivate();
            return p;
        }

        public Function Integrate(int i)
        {
            if (i < 0)
                return Derivate(-i);
            var p = this;
            for (; i >= 0; i--)
                p = p.Integrate();
            return p;
        }

        public double[] Intersection(Function a) => new Difference(this, a).Roots();

        public virtual double[] Roots()
        {
            var roots = new List<double>();
            Polynomial p = new Polynomial(1);
            while (true)
            {
                var root = NonlinearEquations.NewtonMethod(new Quotient(this, p), 0);
                if (root == null)
                    break;
                roots.Add((double)root);
                p *= new Polynomial(-(double)root, 1);
            }
            roots.Sort();
            return roots.ToArray();
        }

        //evt aus vorzeichenwechsel betrachten
        public double[] Extrema()
        {
            Function d1 = Derivate(), d2 = d1.Derivate();
            return Roots().Where(d => d2.Y(d) != 0).ToArray();
        }

        public double[] Maxima()
        {
            Function d1 = Derivate(), d2 = d1.Derivate();
            return Roots().Where(d => d2.Y(d) < 0).ToArray();
        }

        public double[] Minima()
        {
            Function d1 = Derivate(), d2 = d1.Derivate();
            return Roots().Where(d => d2.Y(d) > 0).ToArray();
        }

        public double[] Inflections()
        {
            Function d2 = Derivate(2), d3 = d2.Derivate();
            return d2.Roots().Where(d => d3.Y(d) != 0).ToArray();
        }

        public double[] Saddles()
        {
            Function d1 = Derivate(), d2 = d1.Derivate(), d3 = d2.Derivate();
            return Roots().Where(d => d2.Y(d) == 0 && d3.Y(d) != 0).ToArray();
        }

        public abstract override string ToString();

        public abstract Function Clone();
    }
}