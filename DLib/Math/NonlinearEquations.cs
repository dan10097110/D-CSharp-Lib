using System;

namespace DLib.Math
{
    public static class NonlinearEquations
    {
        public static double SecantMethod(Function.Function p, double a, double b)
        {
            while (p.GetY(b) != 0)
            {
                double c = b - (b - a) * p.GetY(b) / (p.GetY(b) - p.GetY(a));
                a = b;
                b = c;
            }
            return b;
        }

        public static double Bisection(Function.Function p, double a, double b)
        {
            while (true)
            {
                double c = (a + b) / 2, y = p.GetY(c);
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
                double c = (a * p.GetY(b) - b * p.GetY(a)) / (p.GetY(b) - p.GetY(a));
                if (p.GetY(c) == 0)
                    return c;
                if (p.GetY(c) * p.GetY(a) > 0)
                    a = c;
                else
                    b = c;
            }
        }

        public static double NewtonMethod(Function.Polynomial p, double a)
        {
            Function.Polynomial q = p.GetIntegral(-1);
            while (p.GetY(a) != 0)
                a -= p.GetY(a) / q.GetY(a);
            return a;
        }

        public static double HalleyMethod(Function.Polynomial p, double a)
        {
            Function.Polynomial q = p.GetIntegral(-1), r = q.GetIntegral(-1);
            while (p.GetY(a) != 0)
                a -= (2 * p.GetY(a) * q.GetY(a)) / (2 * q.GetY(a) * q.GetY(a) - p.GetY(a) * r.GetY(a));
            return a;
        }
    }
}
