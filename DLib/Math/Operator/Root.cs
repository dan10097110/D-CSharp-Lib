using System.Runtime.InteropServices;

namespace DLib.Math.Operator
{
    public static class Root
    {
        public static class IterationBased
        {
            public static double Newton(double n, int w, int iterations)
            {
                double root = 1;
                for (int i = 0; i < iterations; root = ((w - 1) * System.Math.Pow(root, w) + n) / (w * System.Math.Pow(root, w - 1)), i++) ;
                return root;
            }

            public static double NewtonSqrt(double n, int iterations)
            {
                double root = 1;
                for (int i = 0; i < iterations; root = (root + n / root) / 2, i++) ;
                return root;
            }
        }

        public static class PrecisionBased
        {
            public static double Bisection(double n, int w, int precision) => NonlinearEquations.Bisection(n >= 1 ? 1 : 0, n >= 1 ? n : 1, m => System.Math.Pow(m, w) - n, precision);

            public static double BisectionSqrt(double n, int precision) => NonlinearEquations.Bisection(n >= 1 ? 1 : 0, n >= 1 ? n : 1, m => m * m - n, precision);

            public static double NewtonSqrt(double n, int precision)
            {
                double root = 1;
                while (true)
                {
                    double d = (root * root - n) / (2 * root);
                    if (System.Math.Round(d, precision) == 0)
                        break;
                    root -= d;
                }
                return root;
            }
        }

        public static class Approx
        {
            public static float Sqrt1(float z)
            {
                if (z == 0) return 0;
                FloatIntUnion u;
                u.tmp = 0;
                float xhalf = 0.5f * z;
                u.f = z;
                u.tmp = 0x5f375a86 - (u.tmp >> 1);
                u.f = u.f * (1.5f - xhalf * u.f * u.f);
                return u.f * z;
            }

            [StructLayout(LayoutKind.Explicit)]
            struct FloatIntUnion
            {
                [FieldOffset(0)]
                public float f;

                [FieldOffset(0)]
                public int tmp;
            }

            public static float Sqrt2(float m)
            {
                float i = 0, x1, x2 = 0;
                while ((i * i) <= m)
                    i += 0.1f;
                x1 = i;
                for (int j = 0; j < 10; j++)
                {
                    x2 = m;
                    x2 /= x1;
                    x2 += x1;
                    x2 /= 2;
                    x1 = x2;
                }
                return x2;
            }
        }

        public static class Integer
        {
            public static int Standard(int n) => BitwiseIterative(n);

            public static int Newton(int n)
            {
                for(double x = n, xNext; ; x = xNext)
                {
                    xNext = (x + n / x) / 2;
                    if (System.Math.Abs(xNext - x) < 1)
                        return (int)xNext;
                }
            }

            public static int NewtonIntegerDivison(int n)
            {
                for (int x = n; ;)
                {
                    int d = (n / x - x) >> 1;
                    if (d == 0)
                        return x + d;
                    else if (d == 1)
                        return x;
                    x += d;
                }
            }

            public static int BitwiseRecursive(int n)
            {
                if (n < 2)
                    return n;
                else
                {
                    int smallCandidate = BitwiseRecursive(n >> 2) << 1, largeCandidate = smallCandidate + 1;
                    return largeCandidate * largeCandidate > n ? smallCandidate : largeCandidate;
                }
            }

            public static int BitwiseIterative(int n)
            {
                int shift = 2, nShifted = n >> shift;
                for (; nShifted != 0 && nShifted != n; shift += 2, nShifted = n >> shift) ;
                shift -= 2;
                int result = 0;
                for (; shift >= 0; shift -= 2)
                {
                    result <<= 1;
                    int candidateResult = result + 1;
                    if (candidateResult * candidateResult <= n >> shift)
                        result = candidateResult;
                }
                return result;
            }
        }
    }
}
