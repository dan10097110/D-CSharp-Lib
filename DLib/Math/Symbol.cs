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

        public static short Jacobi(ulong a, ulong b) => Jacobi((long)a, (long)b);

        public static short Legendre(long a, long p)
        {
            short s = (short)Power.BinaryMod((ulong)a, (ulong)((p - 1) >> 1), (ulong)p);
            if (s == 0 || s == 1)
                return s;
            if (s == a - 1)
                return -1;
            return -1;
        }
    }
}
