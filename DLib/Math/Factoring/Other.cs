using System;

namespace DLib.Math.Factoring
{
    public static class Other
    {
        public static ulong GCD(ulong n)
        {
            ulong m = 1;
            foreach (ulong prime in Prime.Sieve.Standard((ulong)System.Math.Sqrt(n) + 1))
                m = (m * prime) % n;
            ulong factor = Math.GCD.Standard(n, m);
            return System.Math.Min(factor, n / factor);
        }
        
        public static ulong Shor(ulong n)
        {
            throw new NotImplementedException();
            var random = new System.Random();
            ulong a = (ulong)random.Next(2, (int)n), gcd = Math.GCD.Standard(a, n);
            if (gcd > 1)
                return gcd;

            return 1;
        }
    }
}