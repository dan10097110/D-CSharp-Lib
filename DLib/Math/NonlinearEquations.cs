using System;

namespace DLib.Math
{
    public static class NonlinearEquations
    {
        public static double SecantMethod(Function.Function p, double a, double b)
        {
            while (p.Y(b) != 0)
            {
                double c = b - (b - a) * p.Y(b) / (p.Y(b) - p.Y(a));
                a = b;
                b = c;
            }
            return b;
        }

        public static double Bisection(Function.Function p, double a, double b)
        {
            while (true)
            {
                double c = (a + b) / 2, y = p.Y(c);
                if (y == 0)
                    return c;
                else if (y > 0)
                    b = c;
                else
                    a = c;
                if (a == b)
                    throw new Exception("no root in the intervall");
            }
        }

        public static double Bisection(double a, double b, Func<double, double> Func, int precision)
        {
            while (true)
            {
                double c = (a + b) / 2, d = System.Math.Round(Func(c), precision);
                if (d == 0)
                    return c;
                else if (d > 0)
                    b = c;
                else
                    a = c;
            }
        }

        public static double FalsePositionMethod(Function.Function p, double a, double b)
        {
            while (true)
            {
                double c = (a * p.Y(b) - b * p.Y(a)) / (p.Y(b) - p.Y(a));
                if (p.Y(c) == 0)
                    return c;
                if (p.Y(c) * p.Y(a) > 0)
                    a = c;
                else
                    b = c;
            }
        }

        public static double? NewtonMethod(Function.Function p, double a)
        {
            var q = p.Derivate();
            while (System.Math.Round(p.Y(a), 6) != 0)
            {
                double d = p.Y(a) / q.Y(a);
                if (d == 0)
                    return null;
                a -= d;
            }
            return a;
        }

        public static double HalleyMethod(Function.Function p, double a)
        {
            Function.Function q = p.Derivate(), r = q.Derivate();
            while (p.Y(a) != 0)
                a -= (2 * p.Y(a) * q.Y(a)) / (2 * q.Y(a) * q.Y(a) - p.Y(a) * r.Y(a));
            return a;
        }
    }
}
