using DLib.Math.Operator;
using System;
using System.Linq;

namespace DLib.Math.Prime.Test
{
    public static class Deterministic
    {
        public static bool TrialDivision(ulong n) => Factoring.Special.TrialDivison(n) == 1;

        public static bool PrimeDivision(ulong n) => Factoring.Special.PrimeDivisionIntegratedSieve(n) == 1;

        public static bool Fermat(ulong n)
        {
            for (ulong a = 2; a < n - 1; a++)
                if (GCD.Standard(n, a) != 1 && !Problably.Fermat(n, a))
                    return false;
            return true;
        }

        public static bool SolovayStrassen(ulong n)
        {
            for (ulong a = 2; a < n; a++)
                if (!Problably.EulerJacobi(n, a))
                    return false;
            return true;
        }

        public static bool Wilson(ulong n)
        {
            ulong m = 2;
            for (ulong i = 3; i < n; i++)
                m = (m * i) % n;
            return m + 1 == n;
        }

        public static bool Pepin(ulong exponent)//fermat numbers
        {
            ulong n = (ulong)System.Math.Pow(2, System.Math.Pow(2, exponent));
            return Power.BinaryMod(3, n >> 1, n + 1) == n;
        }

        public static bool Pocklington(ulong n)
        {
            ulong[] primes = Sieve.Standard((ulong)System.Math.Log(n - 1)).Cast<ulong>().ToArray();
            ulong f = n - 1, sqrt = (ulong)System.Math.Sqrt(n);
            foreach (ulong prime in primes)
            {
                if (f / prime < sqrt)
                    goto g;
                while (f % prime == 0)
                {
                    if (f / prime < sqrt)
                        goto g;
                    f /= prime;
                }
            }
            g:
            ulong[] factors = Factorise.Standard(f, Factoring.Special.TrialDivison);
            for (uint i = 2; i < n; i++)
                if (Power.BinaryMod(i, n - 1, n) == 1)
                {
                    bool isPrime = true;
                    foreach (uint factor in factors)
                        if (GCD.Standard((uint)Power.Binary(i, (n - 1) / factor) - 1, n) != 1)
                        {
                            isPrime = false;
                            break;
                        }
                    if (isPrime)
                        return true;
                }
            return false;
        }

        public static bool Lucas(ulong n)
        {
            Random random = new Random();
            ulong[] factorisation = Factorise.Standard(n - 1, Factoring.Special.TrialDivison);
            for (ulong a = 2; a < n;)
            {
                c:
                if (GCD.Standard(a, n) > 1)
                    return false;
                if (Power.BinaryMod(a, n - 1, n) == 1)
                {
                    foreach (uint prime in factorisation)
                        if (Power.BinaryMod(a, (n - 1) / prime, n) == 1)
                        {
                            a++;
                            goto c;
                        }
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Applies Miller test to n. 
        /// </summary>
        /// <param name="n">number to be tested</param>
        /// <returns>true if n is prime otherwise false</returns>
        public static bool MillerTest(ulong n)
        {
            if (n == 2)
                return true;
            if (n < 2 || (n & 1) == 0)
                return false;
            ulong d = (n - 1) >> 1, s = 1;
            for (; (d & 1) == 0; d >>= 1, s++) ;
            ulong max = (ulong)System.Math.Min(n - 1, System.Math.Floor(2 * System.Math.Pow(System.Math.Log(n), 2)));
            for (ulong a = 2; a <= max; a++)
            {
                ulong m = Power.BinaryMod(a, d, n);
                if (m != 1 && m != n - 1)
                {
                    for (ulong r = 1; r < s; r++)
                        if ((m = (m * m) % n) == n - 1)
                            goto c;
                    return false;
                }
                c:;
            }
            return true;
        }

        public static bool AKS(ulong n) => throw new NotImplementedException();

        public static bool APR(ulong n) => throw new NotImplementedException();

        public static bool LucasLehmerRiesel(ulong n) => throw new NotImplementedException();

        public static bool Proth(ulong n) => throw new NotImplementedException();//proth numbers
    }
}
