using System;
using System.Collections.Generic;
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
                    double y = System.Math.Sqrt(yy);
                    if (y % 1 == 0)
                    {
                        int gcd = (int)GCD.Standard((ulong)n, (ulong)(x - (int)y));
                        gcd = System.Math.Min(gcd, n / gcd);
                        if (gcd != 1)
                            return gcd;
                    }
                    int[] factorisationYY = Factorisation(yy, primes);
                    if (factorisationYY != null)
                        relations[count++] = (x, yy, factorisationYY);
                }
            }
            return GetFactor(n, relations);
        }

        public static int CFrac(int n)
        {
            ulong exclSieveLimit = (ulong)System.Math.Exp(System.Math.Sqrt(System.Math.Log(n) * System.Math.Log(System.Math.Log(n))));
            var primesList = Prime.Sieve.Standard(exclSieveLimit/*, p => p == 2 || System.Math.Pow(n, (p - 1) / 2) % p <= 1*/).Select(z => (int)z).ToList();
            primesList.Insert(0, -1);
            var primes = primesList.ToArray();
            var relations = new List<(int x, int yy, int[] factorisationYY)>();
            var cFrac = Math.CFrac.FromSqrt(n);
            for (int i = 0; i < cFrac.Length && relations.Count < primes.Length; i++)
            {
                var frac = cFrac.ToIthConvergentFrac(i);
                int x = frac.numerator, yy = (int)System.Math.Pow(-1, i) * (x * x - n * frac.denominator * frac.denominator);
                if (yy > 0)
                {
                    double y = System.Math.Sqrt(yy);
                    if (y % 1 == 0)
                    {
                        int gcd = (int)GCD.Standard((ulong)n, (ulong)(x - (int)y));
                        gcd = System.Math.Min(gcd, n / gcd);
                        if (gcd != 1)
                            return gcd;
                    }
                }
                var factorisation = Factorisation(yy, primes);
                if (factorisation != null)
                    relations.Add((x, yy, factorisation));
            }
            return GetFactor(n, relations.ToArray());
        }

        public static int[] Factorisation(int n, int[] primes)
        {
            if (n == 0)
                return null;
            int[] factorisation = new int[primes.Length];
            int i = 0;
            if (primes[i] == -1)
            {
                if (n < 0)
                    factorisation[i] = 1;
                n = -n;
                i++;
            }
            for (; i < primes.Length; i++)
            {
                for (; n % primes[i] == 0; n /= primes[i], factorisation[i]++) ;
                if (n == 1)
                    return factorisation;
            }
            return null;
        }

        public static int GetFactor(int n, (int x, int yy, int[] factorisationYY)[] relations)
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