using DLib.Math.Operator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Math
{
    public static class Factor
    {
        public static ulong Standard(ulong n) => TrialDivison(n);

        public static ulong Division(ulong n, IEnumerable<ulong> dividends) => Division(n, dividends, 2, n);

        public static ulong Division(ulong n, IEnumerable<ulong> dividends, ulong inclusiveMin, ulong exclusiveMax)
        {
            foreach (ulong dividend in dividends)
                if (dividend < exclusiveMax)
                    break;
                else if (dividend >= inclusiveMin && n % dividend == 0)
                    return dividend;
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

        public static ulong PM1(ulong n, ulong b)
        {
            ulong m = 2;
            foreach (ulong prime in Prime.Sieve.Standard(b))
            {
                ulong k = prime;
                for (; k * prime < b; k *= prime) ;
                m = Power.BinaryMod(m, k, n);
            }
            ulong gcd = Math.GCD.Standard(m - 1, n);
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

        public static ulong SQUFOF(ulong n) => throw new NotImplementedException();

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

            public static ulong[] Dixon(ulong n)
            {
                var fb = GetFactorBase((ulong)System.Math.Round(System.Math.Exp(System.Math.Sqrt(System.Math.Log(n) * System.Math.Log(System.Math.Log(n))) / 2)), p => Symbol.Jacobi(n, p) == 1);
                var rels = new(long, long, long[])[fb.Length];
                for (long c = 0, r1 = (long)System.Math.Ceiling(System.Math.Sqrt(n)); c < rels.Length; r1++)
                {
                    var r2 = (r1 * r1) % (long)n;
                    var fs = IsSmooth(r2, fb);
                    if (fs != null)
                        rels[c++] = (r1, r2, fs);
                }
                return GetFactors(n, rels);
            }

            public static ulong[] CFrac(ulong n)
            {
                var fb = GetFactorBase((ulong)System.Math.Round(System.Math.Exp(System.Math.Sqrt(System.Math.Log(n) * System.Math.Log(System.Math.Log(n))) / 2)), p => true);
                var rels = new(long, long, long[])[fb.Length];
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
                        rels[c++] = ((long)frac.Numerator, Q, fs);
                }
                return GetFactors(n, rels);
            }

            public static ulong QS(ulong n)
            {
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

            static long[] GetFactorBase(ulong b, Func<ulong, bool> Condition) => Prime.Sieve.Standard(b, Condition).Cast<long>().ToArray();

            static ulong[] GetFactors(ulong n, (long, long, long[])[] rels)
            {
                var fs = new List<ulong>();
                for(int i = 0; i < rels.Length; i++)
                    for(int j = i + 1; j < rels.Length; j++)
                    {
                        var v = rels[i].Item2 * rels[j].Item2;
                        if(v > 0)
                        {
                            var y = System.Math.Sqrt(v);
                            if (y % 1 == 0)
                            {
                                ulong gcd = Math.GCD.Standard((ulong)(rels[i].Item1 * rels[j].Item1 - (long)y), n);
                                gcd = System.Math.Min(gcd, n / gcd);
                                if (gcd != 1 && !fs.Contains(gcd))
                                    fs.Add(gcd);
                            }
                        }
                    }
                return fs.ToArray();
            }

            /*static ulong[] GetFactors(ulong n, (long, long, long[])[] rels)
            {

            }

            class EvolutionaryAlgorithm
            {
                public static Relation[] rel;

                public EvolutionaryAlgorithm()
                {
                    var sols = new List<Solution>();

                    sols.Sort((a, b) => a.Points > b.Points ? 1 : 0);
                }
            }

            class Relation
            {

            }

            class Solution
            {
                Random random = new Random();
                bool[] gen;
                public bool[] Sum
                {
                    get
                    {
                        var sum = new bool[EvolutionaryAlgorithm.rel.factors.Length];
                        for(int i = 0; i < sum.Length; i++)
                            for(int j = 0; j < EvolutionaryAlgorithm.rel.Length; j++)
                                sum[i] ^= EvolutionaryAlgorithm.rel.factors[j];
                        return sum;
                    }
                }
                public int Points
                {
                    get
                    {
                        int points = 0;
                        for (int i = 0; i < Sum.Length; i++)
                            if (Sum[i])
                                points++;
                        return points;
                    }
                }

                public Solution()
                {
                    gen = new bool[EvolutionaryAlgorithm.rel.Length];
                }

                public Solution(bool[] gen)
                {
                    this.gen = gen;
                }

                public Solution Mutate(int complexity)
                {
                    bool[] gen = this.gen.ToArray();
                    for (int i = 0; i < complexity; i++)
                    {
                        int r = random.Next(0, gen.Length);
                        gen[r] = !gen[r];
                    }
                    return new Solution(gen);
                }

                public static Solution Combine(Solution sol1, Solution sol2)
                {

                }
            }*/
        }
    }
}