using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Math.Factoring
{
    public static class Special
    {
        public static int Standard(int n) => TrialDivision(n);

        public static int? Fermat(int n, int bound)
        {
            int x = (int)System.Math.Ceiling(System.Math.Sqrt(n)), r = x * x - n;
            double f;
            for (; (f = System.Math.Sqrt(r)) % 1 != 0; r += 2 * x + 1, x++)
                if (x - (int)f > bound)
                    return null;
            return x - (int)f;
        }

        public static ulong Fermat(ulong n)
        {
            ulong x = (ulong)System.Math.Ceiling(System.Math.Sqrt(n)), r = x * x - n;
            for (; System.Math.Sqrt(r) % 1 != 0; r += 2 * x + 1, x++) ;
            return x - (ulong)System.Math.Sqrt(r);
        }

        public static ulong Legendre(ulong n)
        {
            for (ulong x = (ulong)System.Math.Ceiling(System.Math.Sqrt(n + 1)); x < n; x++)
                for (ulong y = 1; y < System.Math.Floor(System.Math.Sqrt(x * x - n) + 1); y++)
                    if ((x * x - y * y) % n == 0 && x + y != n)
                        return Math.GCD.Standard(x - y, n);
            return 1;
        }

        public static int FermatTrialDivision(int n)
        {
            double sqrt = System.Math.Sqrt(n);
            int boundFermat = (int)(System.Math.Sqrt(sqrt) + sqrt),
                boundTrial = (int)(boundFermat - System.Math.Sqrt((boundFermat * boundFermat) - n));
            int? f = TrialDivision(n, boundTrial);
            if (f != null)
                return (int)f;
            f = Fermat(n, boundFermat);
            if (f != null)
                return (int)f;
            return 1;
        }

        public static ulong Division(ulong n, IEnumerable<ulong> dividends) => Division(n, dividends, 2, n);

        public static ulong Division(ulong n, IEnumerable<ulong> dividends, ulong inclusiveMin, ulong exclusiveMax)
        {
            lock (dividends)
            {
                foreach (ulong dividend in dividends)
                    if (dividend >= exclusiveMax)
                        break;
                    else if (dividend >= inclusiveMin && n % dividend == 0)
                        return dividend;
            }
            return 1;
        }

        public static int TrialDivision(int n) => (int)TrialDivision(n, (int)System.Math.Sqrt(n));

        public static int? TrialDivision(int n, int bound)
        {
            if (n < 4)
                return 1;
            if ((n & 1) == 0)
                return 2;
            if (n % 3 == 0)
                return 3;
            for (int d = 5; d <= bound; d += 6)
                if (n % d == 0)
                    return d;
                else if (n % (d + 2) == 0)
                    return d;
            return null;
        }

        public static ulong Lehman(ulong n)
        {
            foreach (ulong prime in Prime.Sieve.Standard((ulong)System.Math.Pow(n, 1 / 3f) + 1))
                if (n % prime == 0)
                    return prime;
            for (ulong i = 1; i <= System.Math.Pow(n, 1 / 3f); i++)
                for (ulong x = (ulong)System.Math.Ceiling(System.Math.Sqrt(n * i >> 2)); x < System.Math.Round(System.Math.Ceiling(System.Math.Sqrt(n * i >> 2)) + System.Math.Pow(n, 1 / 6f) / System.Math.Sqrt(i << 4)); x++)
                {
                    double y = System.Math.Sqrt(x * x - i * n << 2);
                    if (y % 1 == 0)
                    {
                        ulong gcd = Math.GCD.Standard((ulong)(x + y), n);
                        return System.Math.Min(gcd, n / gcd);
                    }
                }
            return 1;
        }

        public static ulong PrimeDivision(ulong n) => Division(n, Prime.Sieve.Standard((ulong)System.Math.Sqrt(n) + 1));

        public static ulong PrimeDivisionIntegratedSieve(ulong n)
        {
            BitArray sieve = new BitArray((int)(System.Math.Sqrt(n) + 1), true);
            uint fsqrt = (uint)System.Math.Sqrt(sieve.Length);
            for (uint i = 2; i <= fsqrt; i++)
                if (sieve[(int)i])
                {
                    if (n % i == 0)
                        return i;
                    for (ulong j = i * i; j < (ulong)sieve.Length; sieve[(int)j] = false, j += i) ;
                }
            for (ulong i = fsqrt + 1; i < (ulong)sieve.Length; i++)
                if (sieve[(int)i])
                    if (n % i == 0)
                        return i;
            return 1;
        }

        public static ulong RhoFloyd(ulong n, ulong a, ulong c)//(c!=0;2) ^ (a>1)
        {
            ulong d = 1;
            for (ulong x = a, y = x; d == 1; x = f(x), y = f(f(y)), d = Math.GCD.Standard((ulong)System.Math.Abs((float)(x - y)), n)) ;
            return System.Math.Min(d, n / d);

            ulong f(ulong u) => (u * u + c) % n;
        }

        public static int PM1(int n)
        {
            int m = 2;
            for (int i = 0; i < System.Math.Log(n); i++)
            {
                int prime = Collection.Primes.GetIth(i);
                int k = prime;
                for (; k * prime < n; k *= prime) ;
                m = (int)Operator.Power.BinaryMod((ulong)m, (ulong)k, (ulong)n);
                int gcd = (int)GCD.Standard((ulong)m - 1, (ulong)n);
                gcd = System.Math.Min(gcd, n / gcd);
                if (gcd != 1)
                    return gcd;
            }
            return 1;
        }

        public static int PM1(int n, int b)
        {
            int m = 2;
            foreach (int prime in Prime.Sieve.Standard((ulong)b))
            {
                int k = prime;
                for (; k * prime < n; k *= prime) ;
                m = (int)Operator.Power.BinaryMod((ulong)m, (ulong)k, (ulong)n);
            }
            int gcd = (int)GCD.Standard((ulong)m - 1, (ulong)n);
            return System.Math.Min(gcd, n / gcd);
        }

        public static ulong PP1(ulong n) => PP1(n, (ulong)(System.Math.Sqrt(n) + 1));

        public static ulong PP1(ulong n, ulong B)
        {
            ulong[] primes = Prime.Sieve.Standard(B).Cast<ulong>().ToArray();
            for (ulong a = 3; a < n; a++)
            {
                ulong D = a * a - 4;
                foreach (ulong p in primes)
                    if (Symbol.Jacobi(D, p) == -1)
                        for (ulong m = p + 1; m < n; m += p + 1)
                        {
                            ulong factor = Math.GCD.Standard(n, (ulong)(Sequence.Lucas.NthModM((long)a, 1, (long)m, (long)n).V - 2));
                            if (factor > 1 && factor != n)
                                return factor;
                        }
            }
            return 1;
        }


        public static int RhoBrent(int n)
        {
            throw new NotImplementedException();
            int x = 2, y = x, d = 1, i = 0;
            while (d == 1)// || d == n)
            {
                i++;
                x = (x * x + 1) % n;
                if (DLib.Extra.IsPowerOfTwo(i))
                    y = x;
                d = (int)DLib.Math.GCD.Standard((ulong)n, (ulong)System.Math.Abs(x - y));
            }
            return d;
        }

        public static ulong SNFS(ulong n) => throw new NotImplementedException();

        public static ulong[] Euler(ulong n)//funktiniert nicht
        {
            ulong[] x = new ulong[2], y = new ulong[2];
            x[0] = (ulong)System.Math.Floor(System.Math.Sqrt(n));
            for (int i = 0; i < 2; i++, x[1] = x[0] - 1)
            {
                y[i] = n - x[i] * x[i];
                if (y[i] >= (n >> 1) + (ulong)i)
                    return new ulong[] { 1, n };
                while (System.Math.Sqrt(y[i]) % 1 != 0)
                {
                    x[i] -= 1;
                    y[i] = n - x[i] * x[i];
                    if (y[i] >= (n >> 1) + (ulong)i)
                        return new ulong[] { 1, n };
                }
                y[i] = (ulong)System.Math.Sqrt(y[i]);
            }
            ulong a = Math.GCD.Standard(x[0] + x[1], y[1] + y[0]), b = Math.GCD.Standard(x[0] - x[1], y[1] - y[0]), c = Math.GCD.Standard(x[0] - x[1], y[1] + y[0]), d = Math.GCD.Standard(x[0] + x[1], y[1] - y[0]), e = Math.GCD.Standard(b * b + a * a, n), f = Math.GCD.Standard(c * c + d * d, n);
            return n / e == f ? new ulong[] { n / e, e } : new ulong[] { n / e, e, n / f, f };
        }

        public static ulong ECM(ulong n) => throw new NotImplementedException();
    }
}