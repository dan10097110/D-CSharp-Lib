using DLib.Math.Operator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Math
{
    public static class Factor
    {
        public static int? Fermat(int n, int bound)
        {
            int x = (int)System.Math.Ceiling(System.Math.Sqrt(n)), r = x * x - n;
            double f;
            for (; (f = System.Math.Sqrt(r)) % 1 != 0; r += 2 * x + 1, x++)
                if (x - (int)f > bound)
                    return null;
            return x - (int)f;
        }

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



        public static ulong Standard(ulong n) => TrialDivison(n);

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

        public static ulong TrialDivison(ulong n)
        {
            if (n < 4)
                return 1;
            if ((n & 1) == 0)
                return 2;
            if (n % 3 == 0)
                return 3;
            for (ulong d = 5; d * d <= n; d += 6)
                if (n % d == 0)
                    return d;
                else if (n % (d + 2) == 0)
                    return d;
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

        public static ulong GCD(ulong n)
        {
            ulong m = 1;
            foreach (ulong prime in Prime.Sieve.Standard((ulong)System.Math.Sqrt(n) + 1))
                m = (m * prime) % n;
            ulong factor = Math.GCD.Standard(n, m);
            return System.Math.Min(factor, n / factor);
        }

        public static ulong Rho(ulong n, ulong a, ulong c)//(c!=0;2) ^ (a>1)
        {
            ulong d = 1;
            for (ulong x = a, y = x; d == 1; x = f(x), y = f(f(y)), d = Math.GCD.Standard(x - y, n)) ;
            return System.Math.Min(d, n / d);

            ulong f(ulong u) => (u * u + c) % n;
        }

        public static int PM1(int n)
        {
            int m = 2;
            for (int i = 0; i < System.Math.Log(n); i++)
            {
                int prime = DLib.Collection.Primes.GetIth(i);
                int k = prime;
                for (; k * prime < n; k *= prime) ;
                m = (int)DLib.Math.Operator.Power.BinaryMod((ulong)m, (ulong)k, (ulong)n);
                int gcd = (int)DLib.Math.GCD.Standard((ulong)m - 1, (ulong)n);
                gcd = System.Math.Min(gcd, n / gcd);
                if (gcd != 1)
                    return gcd;
            }
            return 1;
        }

        public static int PM1(int n, int b)
        {
            int m = 2;
            foreach (int prime in DLib.Math.Prime.Sieve.Standard((ulong)b))
            {
                int k = prime;
                for (; k * prime < n; k *= prime) ;
                m = (int)DLib.Math.Operator.Power.BinaryMod((ulong)m, (ulong)k, (ulong)n);
            }
            int gcd = (int)DLib.Math.GCD.Standard((ulong)m - 1, (ulong)n);
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

        /// <summary>
        /// is not complete
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static ulong Shor(ulong n)
        {
            Random random = new Random();
            ulong a = (ulong)random.Next(2, (int)n), gcd = Math.GCD.Standard(a, n);
            if (gcd > 1)
                return gcd;

            return 1;
        }

        public static ulong ECM(ulong n) => throw new NotImplementedException();

        public static class CongruenceOfSquares
        {
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

            static Random ranGen = new Random();

            public static int Dixon(int n)
            {
                int ceilSqrt = (int)System.Math.Ceiling(System.Math.Sqrt(n)), exclSieveLimit = (int)System.Math.Exp(System.Math.Sqrt(System.Math.Log(n) * System.Math.Log(System.Math.Log(n))));
                var primes = DLib.Math.Prime.Sieve.Eratosthenes((ulong)exclSieveLimit).Select(s => (int)s).ToArray();
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
                            int gcd = (int)DLib.Math.GCD.Standard((ulong)n, (ulong)(x - (int)y));
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

            public static ulong[] CFrac(ulong n)
            {
                var fb = GetFactorBase((ulong)System.Math.Round(System.Math.Exp(System.Math.Sqrt(System.Math.Log(n) * System.Math.Log(System.Math.Log(n))) / 2)), p => true);
                var rels = new Relation[fb.Length];
                Number.Rational a = n;
                var cFrac = new CFrac(a.Round().Abs());
                for (int c = 0; c < rels.Length; c++)
                {
                    var b = a - cFrac[cFrac.Length - 1];
                    if (b.IsZero())
                        break;
                    a = b.Reciprocal();
                    cFrac.Add(a.Round().Abs());
                    var frac = cFrac.ToFrac();
                    var Q = (long)((int)System.Math.Pow(-1, cFrac.Length) * (frac.Numerator * frac.Numerator - n * frac.Denominator * frac.Denominator));
                    var fs = IsSmooth(Q, fb);
                    if (fs != null)
                        rels[c++] = new Relation((long)frac.Numerator, Q, fs);
                }
                return GetFactors(n, rels);
            }


            static long[] IsSmooth(long n, long[] factorBase)
            {
                if (n == 0)
                    return null;
                var factors = new long[factorBase.Length];
                for (uint i = 0; i < factorBase.Length; i++)
                {
                    for (; n % factorBase[i] == 0; factors[i]++, n /= factorBase[i]) ;
                    if (n == 1)
                        return factors;
                }
                return null;
            }

            static long[] GetFactorBase(ulong b, Func<ulong, bool> Condition) => Prime.Sieve.Standard(b, Condition).Select(n => (long)n).ToArray();

            static ulong[] GetFactors(ulong n, Relation[] rels)
            {
                var fs = new List<ulong>();
                for(int i = 0; i < rels.Length; i++)
                    for(int j = i + 1; j < rels.Length; j++)
                    {
                        var v = rels[i] + rels[j];
                        if(v.IsResidueSqaure)
                        {
                            ulong gcd = Math.GCD.Standard((ulong)(v.X - (long)System.Math.Sqrt(v.Residue)), n);
                            gcd = System.Math.Min(gcd, n / gcd);
                            if (gcd != 1 && !fs.Contains(gcd))
                                fs.Add(gcd);
                        }
                    }
                return fs.ToArray();
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

            static ulong GetFactor(ulong n, Relation[] rels)
            {
                for (int i = 1; i <= rels.Length; i++)
                {
                    var v = new int[i];
                    for (int j = 0; ;)
                    {
                        if (v[j] < rels.Length)
                        {
                            if (j < i - 1)
                            {
                                j++;
                                v[j] = v[j - 1];
                            }
                            else
                            {
                                Relation rel = rels[0];
                                for (int k = 1; k < i; k++)
                                    rel += rels[k];
                                if (rel.IsResidueSqaure)
                                {
                                    ulong gcd = DLib.Math.GCD.Standard((ulong)(rel.X - (long)System.Math.Sqrt(rel.Residue)), n);
                                    gcd = System.Math.Min(gcd, n / gcd);
                                    if (gcd != 1)
                                        return gcd;
                                }
                            }
                        }
                        else
                            j--;
                        if (j < 0)
                            break;
                        v[j]++;
                    }
                }
                return 1;
            }

            class Relation
            {
                public long X { get; private set; }
                public long Residue { get; private set; }
                public long[] ResidueFactors { get; private set; }

                public long[] FactorsMod2 => ResidueFactors.Select(n => n & 1).ToArray();
                public int EvenFactors => ResidueFactors.Count(n => (n & 1) == 0);
                public bool IsResidueSqaure => FactorsMod2.Sum() == 0;

                public Relation(long square, long residue, long[] factors)
                {
                    X = square;
                    Residue = residue;
                    ResidueFactors = factors;
                }

                public static Relation operator +(Relation a, Relation b) => new Relation(a.X * b.X, a.Residue * b.Residue, a.ResidueFactors.Zip(b.ResidueFactors, (x, y) => x + y).ToArray());
            }


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

            public static ulong SNFS(ulong n) => throw new NotImplementedException();

            public static ulong RS(ulong n) => throw new NotImplementedException();

            public static ulong SQUFOF(ulong n) => throw new NotImplementedException();
        }
    }
}