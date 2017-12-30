using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DLib.Collection
{
    public static class Primes
    {
        static List<int> primes = new List<int>() { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 91, 97 };
        static int nextCand = primes.Last() + 2;
        static bool calc = false;

        public static int GetIth(int i)
        {
            while (calc)
                Thread.Sleep(1);
            if (i >= primes.Count)
                CalcUntilIthPrime(i + 1);
            return primes[i];
        }

        public static bool IsPrime(int n)
        {
            while (calc)
                Thread.Sleep(1);
            if (n < nextCand)
                return Contain(n);
            else if (n < nextCand * nextCand)
            {
                CalcUntilI(n);
                return primes.Last() == n;
            }
            else
            {
                CalcUntilI((int)System.Math.Sqrt(n));
                for (int i = 0; i < primes.Count; i++)
                    if (n % primes[i] == 0)
                        return false;
                return true;
            }
        }

        public static bool IsProbPrime(int n)
        {
            while (calc)
                Thread.Sleep(1);
            if (n <= nextCand)
                return Contain(n);
            else
            {
                CalcUntilI((int)System.Math.Sqrt(System.Math.Sqrt(n)));
                for (int i = 0; i < primes.Count; i++)
                    if (n % primes[i] == 0)
                        return false;
                return true;
            }
        }

        public static void CalcUntilIthPrime(int exclusiveI)
        {
            while (calc)
                Thread.Sleep(1);
            calc = true;
            while (primes.Count < exclusiveI)
                TestNextCand();
            calc = false;
        }

        public static void CalcUntilI(int inclusiveI)
        {
            while (calc)
                Thread.Sleep(1);
            calc = true;
            if (inclusiveI < (nextCand << 1))
                while (primes.Last() < inclusiveI)
                    TestNextCand();
            else
                Sieve(inclusiveI);
            calc = false;
        }

        static bool Contain(int n)
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

        static void Sieve(int exclusive)
        {
            var sieve = new BitArray(exclusive + 1, true);
            for (int i = 1; i < primes.Count; i++)
                for (int j = primes[i] * primes[i]; j < sieve.Count; sieve[j] = false, j += primes[i]) ;
            for (int i = nextCand; i * i <= sieve.Count; i += 2)
                if (sieve[i])
                {
                    primes.Add(i);
                    for (int j = i * i; j < sieve.Count; sieve[j] = false, j += i) ;
                }
            int fsqrt = (int)System.Math.Sqrt(sieve.Count);
            for (int i = System.Math.Max(primes.Last() + 2, fsqrt + 1 + (fsqrt & 1)); i < sieve.Count; i += 2)
                if (sieve[i])
                    primes.Add(i);
            nextCand = sieve.Count + ((sieve.Count + 1) & 1);
        }
    }
}