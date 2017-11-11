using System;
using System.Collections.Generic;

namespace DLib.Math.Prime.Test
{
    public static class Probabilistic
    {
        public static bool TrialDivision(ulong n, ulong limit, List<ulong> primes) => TrialDivision(n, i => limit, primes);

        public static bool TrialDivision(ulong n, Func<ulong, ulong> Limit, List<ulong> primes)
        {
            ulong l = Limit(n);
            for (int i = 1; i < primes.Count && primes[i] <= l; i++)
                if (n % primes[i] == 0)
                    return false;
            return true;
        }

        public static bool TrialDivision(ulong n, List<ulong> primes)
        {
            ulong sqrt = (ulong)System.Math.Sqrt(n);
            for (int i = 0; i < primes.Count && primes[i] <= sqrt; i++)
                if (n % primes[i] == 0)
                    return false;
            return true;
        }

        public static bool Fermat(ulong n, ulong iterations)
        {
            Random random = new Random();
            for (ulong i = 0, a = (ulong)random.Next(2, (int)(n - 1)); i < iterations; i++, a = (ulong)random.Next(2, (int)(n - 1)))
                if (GCD.Standard(n, a) != 1 && !Problably.Fermat(n, a))
                    return false;
            return true;
        }

        public static bool SolovayStrassen(ulong n, ulong iterations)
        {
            Random random = new Random();
            for (ulong i = 0; i < iterations; i++)
                if (!Problably.EulerJacobi(n, (ulong)random.Next(2, (int)n)))
                    return false;
            return true;
        }

        public static bool MillerRabin(ulong n, ulong iterations)
        {
            Random random = new Random();
            ulong d = n - 1, r = 0;
            for (; (d & 1) == 0; d >>= 1, r++) ;
            for (ulong i = 0; i < iterations; i++)
            {
                c:
                ulong x = Power.BinaryMod((ulong)random.Next(2, (int)(n - 1)), d, n);
                if (x == 1 || x == n - 1)
                    continue;
                for (ulong j = 0; j < r - 1; j++)
                {
                    x = (x * x) % n;
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        goto c;
                }
                return false;
            }
            return true;
        }

        public static bool EllipticCurve(ulong n)
        {
            return false;
        }

        public static bool QFT(ulong n)
        {
            return false;
        }

        public static bool Lucas(ulong n, ulong iterations)//returns true if n is prime, false if n is possibly composite
        {
            Random random = new Random();
            ulong[] factorisation = Factorise.Standard(n - 1);
            for (ulong i = 0; i < iterations; i++)
            {
                c:
                ulong a = (ulong)random.Next(2, (int)n);
                if (GCD.Standard(a, n) > 1)
                    return false;
                if (Power.BinaryMod(a, n - 1, n) == 1)
                {
                    foreach (uint prime in factorisation)
                        if (Power.BinaryMod(a, (n - 1) / prime, n) == 1)
                            goto c;
                    return true;
                }
                else
                    return false;
            }
            return false;
        }
    }
}
