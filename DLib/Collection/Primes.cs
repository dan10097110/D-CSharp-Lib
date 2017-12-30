using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Collection
{






    //prime numbers in sieve are unsorted




    public static class Primes
    {
        static List<ulong> primes = new List<ulong>() { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 91, 97 };
        static ulong nextCand = primes.Last() + 2;

        public static ulong GetIth(int i)
        {
            if (i >= primes.Count)
                CalcUntilIthPrime(i + 1);
            return primes[i];
        }

        public static bool IsPrime(ulong n)
        {
            if (n < nextCand)
                return Contain(n);
            else if (n < nextCand * nextCand)
            {
                CalcUntilI(n);
                return primes.Last() == n;
            }
            else
            {
                CalcUntilI((ulong)System.Math.Sqrt(n));
                for (int i = 0; i < primes.Count; i++)
                    if (n % primes[i] == 0)
                        return false;
                return true;
            }
        }

        public static bool IsProbPrime(ulong n)
        {
            if (n <= nextCand)
                return Contain(n);
            else
            {
                CalcUntilI((ulong)System.Math.Sqrt(System.Math.Sqrt(n)));
                for (int i = 0; i < primes.Count; i++)
                    if (n % primes[i] == 0)
                        return false;
                return true;
            }
        }

        public static void CalcUntilIthPrime(int exclusiveI)
        {
            while (primes.Count < exclusiveI)
                TestNextCand();
        }

        public static void CalcUntilI(ulong inclusiveI)
        {
            if (inclusiveI < (nextCand << 1))
                while (primes.Last() < inclusiveI)
                    TestNextCand();
            else
                Sieve(inclusiveI);
        }

        static bool Contain(ulong n)
        {
            int u = 0;
            for (int o = primes.Count - 1; u != o;)
            {
                int m = (u + o) >> 1;
                if (primes[m] < n)
                    u = m + 1;
                else
                    o = m;
            }
            return primes[u] == n;
        }

        static void TestNextCand()
        {
            for (int i = 1; primes[i] * primes[i] <= nextCand; i++)
                if (nextCand % primes[i] == 0)
                {
                    nextCand += 2;
                    return;
                }
            primes.Add(nextCand);
            nextCand += 2;
        }

        static void Sieve(ulong exclusive)
        {
            BitArray sieve = new BitArray((int)exclusive + 1, true);
            for (int i = 1; i < primes.Count; i++)
                for (int j = (int)(primes[i] * primes[i]); j < sieve.Count; sieve[j] = false, j += (int)primes[i]) ;
            for (int i = primes.Count; i * i <= sieve.Count; i += 2)
                if (sieve[i])
                    for (int j = i * i; j < sieve.Count; sieve[j] = false, j += i) ;
            for (int i = (int)primes.Last() + 2; i < sieve.Count; i += 2)
                if (sieve[i])
                    primes.Add((ulong)i);
            nextCand = (ulong)(sieve.Count + ((sieve.Count + 1) & 1));
        }
    }
}