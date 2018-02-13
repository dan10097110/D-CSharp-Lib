using DLib.Math.Operator;

namespace DLib.Math
{
    public static class Symbol
    {
        public static short Jacobi(long a, long b)
        {
            if (a <= 1)
                return 1;
            long n = a, k = 0;
            for (; (n & 1) == 0; n >>= 1, k++) ;
            return (sbyte)((((k * (b * b - 1) >> 3) + ((n - 1) * (b - 1) >> 2)) & 1) == 0 ? Jacobi(b % n, n) : -Jacobi(b % n, n));
        }

        public static int Jacobi2(int a, int b)
        {
            int x = a, y = b, j = 1;
            while (x != 1)
            {
                int x1 = x, k = 0;
                for (; (x1 & 1) == 0; x1 >>= 1, k++) ;
                if ((k & 1) == 1)
                    j = (int)System.Math.Pow(-1, (x1 - 1) / 2 * (y - 1) / 2);
                if ((x1 & 1) != 1)
                {
                    j = (int)System.Math.Pow(-1, (x1 - 1) / 2 * (y - 1) / 2);
                    y = x1;
                    x = y % x1;
                }
                else
                    x = 1;
            }
            return j;
        }

        public static short Jacobi(ulong a, ulong b) => Jacobi((long)a, (long)b);

        public static short? Legendre(long a, long prime)
        {
            short s = (short)Power.BinaryMod((ulong)a, (ulong)((prime - 1) >> 1), (ulong)prime);
            if (s == 0 || s == 1)
                return s;
            if (s == a - 1)
                return -1;
            return null;
        }
    }
}
