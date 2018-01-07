using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace DLib.Math.Prime
{
    public static class Sieve
    {
        public static IEnumerable<ulong> Standard(ulong exclusiveMax) => Eratosthenes(exclusiveMax);

        public static IEnumerable<ulong> Standard(ulong exclusiveMax, Func<ulong, bool> condition) => Eratosthenes(exclusiveMax, condition);

        /// <summary>
        /// equal to optimized Euler sieve
        /// </summary>
        /// <param name="exclusiveMax"></param>
        /// <returns></returns>
        public static IEnumerable<ulong> Eratosthenes(ulong exclusiveMax) => Eratosthenes(exclusiveMax, p => true);

        public static IEnumerable<ulong> Eratosthenes(ulong exclusiveMax, Func<ulong, bool> condition)
        {
            yield return 2;
            uint fsqrt = (uint)System.Math.Sqrt(exclusiveMax);
            var sieve = new BitArray((int)exclusiveMax, true);
            for (uint i = 3; i <= fsqrt; i += 2)
                if (sieve[(int)i])
                {
                    if (condition(i))
                        yield return i;
                    for (ulong j = i * i, k = i << 1; j < exclusiveMax; sieve[(int)j] = false, j += k) ;
                }
            for (ulong i = fsqrt + 1 + (fsqrt & 1); i < exclusiveMax; i += 2)
                if (sieve[(int)i] && condition(i))
                    yield return i;
        }

        public static IEnumerable<int> Segmented(int inclusive)
        {
            int size = (int)System.Math.Sqrt(inclusive), sqrt = (int)System.Math.Sqrt(size);
            var primes = new List<int>() { 2 };
            var seg = new BitArray(size, true);
            for (int i = 3; i <= sqrt; i += 2)
                if (seg[i])
                {
                    primes.Add(i);
                    for (int j = i * i, k = i << 1; j < seg.Length; seg[j] = false, j += k) ;
                }
            for (int i = sqrt + 2 - ((sqrt + 1) & 1); i < seg.Length; i += 2)
                if (seg[i])
                    primes.Add(i);
            for (int offset = size; offset < inclusive; offset += size)
            {
                seg.SetAll(true);
                for (int i = 1, j; (j = primes[i] * primes[i]) < offset + size; i++)
                {
                    j = Mod((j - offset), primes[i]);
                    j = j + ((offset + j & 1) == 0 ? primes[i] : 0);
                    for (int k = primes[i] << 1; j < size; seg[j] = false, j += k) ;
                }
                for (int i = (offset + 1) & 1; i < seg.Length; i += 2)
                    if (seg[i])
                        primes.Add(i + offset);
            }
            return primes;

            int Mod(int x, int m)
            {
                int r = x % m;
                return r < 0 ? r + m : r;
            }
        }

        public static IEnumerable<ulong> Atkin(ulong exclusiveMax)
        {
            BitArray sieve = new BitArray((int)exclusiveMax, false);
            for (ulong x = 0; x < exclusiveMax; x++)
            {
                ulong xx = x * x, xx3 = 3 * xx, xx4 = xx << 2;
                for (ulong y = 0, n, m, yy = y * y; (n = xx3 + yy) < exclusiveMax; y++, yy = y * y)
                {
                    if (n < exclusiveMax && n % 12 == 7)
                        sieve[(int)n] = !sieve[(int)n];
                    n = xx4 + yy;
                    if (n < exclusiveMax)
                    {
                        m = n % 12;
                        if ((m == 1 || m == 5))
                            sieve[(int)n] = !sieve[(int)n];
                    }
                }
            }
            for (ulong x = 0; x < exclusiveMax; x++)
            {
                ulong xx3 = 3 * x * x;
                for (ulong y = 0, n; y < x; y++)
                {
                    n = xx3 - y * y;
                    if (n < exclusiveMax && n % 12 == 11)
                        sieve[(int)n] = !sieve[(int)n];
                }
            }
            yield return 2;
            yield return 3;
            uint r = (uint)System.Math.Sqrt(exclusiveMax);
            for (uint i = 5; i <= r; i++)
                if (sieve[(int)i])
                {
                    yield return i;
                    for (uint ii = i * i, j = ii; j < exclusiveMax; j += ii)
                        sieve[(int)j] = false;
                }
            for (ulong i = r + 1; i < exclusiveMax; i++)
                if (sieve[(int)i])
                    yield return i;
        }

        public static IEnumerable<ulong> Sundaram(ulong exclusiveMax)
        {
            BitArray sieve = new BitArray((int)exclusiveMax >> 1, true);
            for (uint i = 1; ((i + 1) * i) << 1 < sieve.Length; i++)
                for (uint n = ((i + 1) * i) << 1, iM2P1 = (i << 1) + 1; n < sieve.Length; sieve[(int)n] = false, n += iM2P1) ;
            yield return 2;
            for (uint i = 1; i < sieve.Length; i++)
                if (sieve[(int)i])
                    yield return (i << 1) + 1;
        }

        public static class Incremental//nicht hoch 2 sondern vlt linear
        {
            static Thread thread;
            static Collection.Long.List<ulong> primes = new Collection.Long.List<ulong>();
            static Stopwatch sw = new Stopwatch();

            public static ulong Count => primes.Count;
            public static TimeSpan Time => sw.Elapsed;
            public static bool Failed { get; private set; }

            public static void BeginAsync()
            {
                thread = new Thread(Sieve);
                thread.Start();
            }

            public static Collection.Long.Array<ulong> EndAsync()
            {
                thread.Abort();
                sw.Stop();
                return new Collection.Long.Array<ulong>(primes);
            }

            static void Sieve()
            {
                try
                {
                    Failed = false;
                    sw.Restart();
                    primes = new Collection.Long.List<ulong>();
                    primes.Add(3);
                    while (true)
                    {
                        var bits = new Collection.Long.BitArray(primes.Last() * ((primes.Last() - 1) >> 1));
                        for (ulong j = 0; j < primes.Count; j++)
                        {
                            ulong prime = primes[j];
                            ulong i = (prime - ((primes.Last() + 1) % prime)) % prime;
                            if ((i & 1) == 0)
                                i += prime;
                            i >>= 1;
                            for (; i < bits.Length; bits[i] = true, i += prime) ;
                        }
                        ulong offset = primes.Last() + 2;
                        for (ulong i = 0; i < bits.Length; i += 1)
                            if (!bits[i])
                                primes.Add((uint)((i << 1) + offset));
                    }
                }
                catch
                {
                    Failed = true;
                }
            }
        }
    }
}
