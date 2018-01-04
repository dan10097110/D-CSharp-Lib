using System;
using System.Linq;

namespace DLib.Math.Factoring
{
    public static class General
    {
        static Random ranGen = new Random();

        public static int Standard(int n) => Dixon(n);

        public static int Dixon(int n)
        {
            int ceilSqrt = (int)System.Math.Ceiling(System.Math.Sqrt(n)), exclSieveLimit = (int)System.Math.Exp(System.Math.Sqrt(System.Math.Log(n) * System.Math.Log(System.Math.Log(n))));
            var primes = Prime.Sieve.Eratosthenes((ulong)exclSieveLimit).Select(s => (int)s).ToArray();
            var relations = new(int x, int yy, int[] factorisationYY)[primes.Length];
            for (int count = 0; count < relations.Length;)
            {
                int x = ranGen.Next(ceilSqrt, n);
                if (relations.Count(a => a.x == x) == 0)
                {
                    int yy = (x * x) % n;
                    int[] factorisationYY = Factorisation(yy, primes);
                    double y = System.Math.Sqrt(yy);
                    if (y % 1 == 0)
                    {
                        int gcd = (int)GCD.Standard((ulong)n, (ulong)(x - (int)y));
                        gcd = System.Math.Min(gcd, n / gcd);
                        if (gcd != 1)
                            return gcd;
                    }
                    if (factorisationYY != null)
                        relations[count++] = (x, yy, factorisationYY);
                }
            }
            return GetFactor(n, relations);
        }

        public static int CFrac(int n)
        {
            var primes = Prime.Sieve.Standard((ulong)System.Math.Round(System.Math.Exp(System.Math.Sqrt(System.Math.Log(n) * System.Math.Log(System.Math.Log(n))) / 2))).ToArray();
            var relations = new(int x, int yy, int[] factorisationYY)[primes.Length];
            Number.Rational a = n;
            var cFrac = new CFrac(a.Round().Abs());
            for (int c = 0; c < relations.Length; c++)
            {
                var b = a - cFrac[cFrac.Length - 1];
                if (b.IsZero())
                    break;
                a = b.Reciprocal();
                cFrac.Add(a.Round().Abs());
                var frac = cFrac.ToFrac();
                var Q = (int)System.Math.Pow(-1, cFrac.Length) * (int)(frac.Numerator * frac.Numerator - (ulong)n * frac.Denominator * frac.Denominator);
                var factorisation = Factorisation(Q, primes.Select(z => (int)z).ToArray());
                if (factorisation != null)
                    relations[c++] = ((int)frac.Numerator, Q, factorisation);
            }
            return GetFactor(n, relations);
        }

        static int[] Factorisation(int n, int[] primes)
        {
            if (n == 0)
                return null;
            int[] factorisation = new int[primes.Length];
            for (int i = 0; i < primes.Length; i++)
            {
                for (; n % primes[i] == 0; n /= primes[i], factorisation[i]++) ;
                if (n == 1)
                    return factorisation;
            }
            return null;
        }

        static int GetFactor(int n, (int x, int yy, int[] factorisationYY)[] relations)
        {
            for (int count = 2; count <= relations.Length; count++)
            {
                var v = new int[count];
                for (int i = 0; ;)
                {
                    if (v[i] < relations.Length)
                    {
                        if (i < count - 1)
                        {
                            i++;
                            v[i] = v[i - 1];
                        }
                        else
                        {
                            int newYY = 1;
                            for (int j = 0; j < count; j++)
                                newYY *= relations[v[j]].yy;
                            double newY = System.Math.Sqrt(newYY);
                            if (newY % 1 == 0)
                            {
                                int newX = 1;
                                for (int k = 0; k < count; k++)
                                    newX *= relations[v[k]].x;
                                int gcd = (int)DLib.Math.GCD.Standard((ulong)n, (ulong)(newX - (int)newY));
                                gcd = System.Math.Min(gcd, n / gcd);
                                if (gcd != 1)
                                    return gcd;
                            }
                        }
                    }
                    else
                        i--;
                    if (i < 0)
                        break;
                    v[i]++;
                }
            }
            return 1;
        }


        public static ulong QS(ulong n)
        {
            throw new NotImplementedException();
            double lnN = System.Math.Log(n);
            double L = System.Math.Exp(System.Math.Sqrt(lnN * System.Math.Log(lnN))), S = System.Math.Sqrt(L);
            ulong[] P = Prime.Sieve.Standard((ulong)S, p => Symbol.Jacobi(n, p) == 1).Cast<ulong>().ToArray();
            ulong ceilSqrt = (ulong)System.Math.Ceiling(System.Math.Sqrt(n)), ceilSqrt2 = ceilSqrt << 1;
            ulong[] q = new ulong[(int)L];
            for (ulong i = 0, qx = ceilSqrt * ceilSqrt - n; i < L; i++, qx += ceilSqrt2 + 1 + (i << 1))
                q[i] = qx;
            foreach (ulong p in P)
            {
                var qx = Extra.TonelliShank((long)p, (long)n);
                var x = ((long)System.Math.Sqrt(qx.Item1 + (long)n), (long)System.Math.Sqrt(qx.Item2 + (long)n));//zwischenschritt sparen
                var i = (x.Item1 - (long)ceilSqrt, x.Item2 - (long)ceilSqrt);
                for (; ; i.Item1 += (long)p, i.Item2 += (long)p)
                {

                }
            }
            return 1;
        }

        public static ulong GNFS(ulong n) => throw new NotImplementedException();

        public static ulong RS(ulong n) => throw new NotImplementedException();

        public static ulong SQUFOF(ulong n) => throw new NotImplementedException();
    }
}