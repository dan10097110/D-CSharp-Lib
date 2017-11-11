using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Math
{
    public static class Factor
    {
        public static ulong Standard(ulong n)
        {
            return PrimeDivision(n);
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

        public static ulong PrimeDivision(ulong n)
        {
            foreach (ulong prime in Prime.Sieve.Standard((ulong)System.Math.Sqrt(n) + 1))
                if (n % prime == 0)
                    return prime;
            return 1;
        }

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

            ulong f(ulong u)
            {
                return (u * u + c) % n;
            }
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

        public static ulong Fermat(ulong n)
        {
            ulong x = (ulong)System.Math.Ceiling(System.Math.Sqrt(n));
            double y = System.Math.Sqrt(x * x - n);
            for (; y % 1 != 0 && x - y > 1; x++, y = System.Math.Sqrt(x * x - n)) ;
            return (ulong)System.Math.Max(1, x - y);
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

        /*public static ulong[] CFrac(ulong n)
        {
            var factorBase = GetFactorBase((ulong)Math.Round(Math.Exp(Math.Sqrt(Math.Log(n) * Math.Log(Math.Log(n))) / 2)), p => true);
            return GetFactorsFromQuadraticRelations(n, GetRelations((ulong)factorBase.Length));

            (ulong, ulong, ulong[])[] GetRelations(ulong count)
            {
                double a = n;
                var relations = new(ulong, ulong, ulong[])[count];
                var cFrac = new List<ulong>() { (ulong)a };
                for(ulong i = 0; i < count; i++)
                {
                    double b = a - cFrac.Last();
                    if (b == 0)
                        break;
                    a = 1 / b;
                    if (a == 0)
                        break;
                    cFrac.Add((ulong)a);

                    var frac = Class1.CFracToFrac(cFrac.ToArray());
                    frac.Item1 = (ulong)Math.Pow(-1, cFrac.Count) * (frac.Item1 * frac.Item1 - n * frac.Item2 * frac.Item2);
                    var factors = IsSmooth(frac.Item2, factorBase);
                    if (factors != null)
                    {
                        relations[i] = (frac.Item1, frac.Item2, factors);
                        i++;
                    }
                }
                return relations;
            }

            (ulong, ulong, ulong[])[] GetRelations(ulong count)
            {
                Rational a = n;
                var relations = new(ulong, ulong, ulong[])[count];
                var cFrac = new CFrac(a.Round().Abs());
                for (ulong i = 0; i < count; i++)
                {
                    Rational b = a - cFrac[cFrac.Length - 1];
                    if (b == 0)
                        break;
                    a = 1 / b;
                    cFrac.Add(a.Round().Abs());

                    var rational = cFrac.ToFrac();
                    rational.Item1 = (ulong)Math.Pow(-1, cFrac.Length) * (rational.Item1 * rational.Item1 - n * rational.Item2 * rational.Item2);
                    var factors = IsSmooth(rational.Item2, factorBase);
                    if (factors != null)
                    {
                        relations[i] = (rational.Item1, rational.Item2, factors);
                        i++;
                    }
                }
                return relations;
            }
        }*/

        public static ulong[] Dixon(ulong n)
        {
            var factorBase = GetFactorBase((ulong)System.Math.Round(System.Math.Exp(System.Math.Sqrt(System.Math.Log(n) * System.Math.Log(System.Math.Log(n))) / 2)), p => Symbol.Jacobi(n, p) == 1);
            return GetFactorsFromQuadraticRelations(n, GetRelations((ulong)factorBase.Length));

            (ulong, ulong, ulong[])[] GetRelations(ulong count)
            {
                var relations = new(ulong, ulong, ulong[])[count];
                for (ulong i = 0, relation = (ulong)System.Math.Ceiling(System.Math.Sqrt(n)); i < (ulong)relations.Length; relation++)
                {
                    var factors = IsSmooth((relation * relation) % n, factorBase);
                    if (factors != null)
                    {
                        relations[i] = (relation, (relation * relation) % n, factors);
                        i++;
                    }
                }
                return relations;
            }
        }

        static ulong[] IsSmooth(ulong n, ulong[] factorBase)
        {
            if (n == 0)
                return null;
            var factors = new ulong[factorBase.Length];
            for (uint i = 0; i < factorBase.Length; i++)
            {
                for (; n % factorBase[i] == 0; factors[i]++, n /= factorBase[i]) ;
                if (n == 1)
                    return factors;
            }
            return null;
        }

        static ulong[] GetFactorBase(ulong b, Func<ulong, bool> Condition) => Prime.Sieve.Standard(b, Condition).Cast<ulong>().ToArray();

        static ulong[] GetFactorsFromQuadraticRelations(ulong n, (ulong, ulong, ulong[])[] relations)
        {
            return null;
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

        public static ulong SQUFOF(ulong n)
        {
            return 1;
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

        public static ulong GNFS(ulong n)
        {
            return 1;
        }

        public static ulong SNFS(ulong n)
        {
            return 1;
        }

        public static ulong RS(ulong n)
        {
            return 1;
        }

        public static ulong ECM(ulong n)
        {
            return 1;
        }
    }
}