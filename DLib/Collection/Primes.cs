using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Collection
{
    public static class Primes
    {
        static List<int> primes = new List<int>() { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
        static int nextCand = primes.Last() + 2;
        static ThreadQueue threadQueue = new ThreadQueue();

        public static int GetIth(int i)
        {
            threadQueue.Wait();
            if (i >= primes.Count)
                CalcUntilIthPrime(i + 1);
            int p = primes[i];
            threadQueue.Next();
            return p;
        }

        public static bool IsPrime(int n)
        {
            threadQueue.Wait();
            bool b = true;
            if (n < nextCand)
                b = Contain(n);
            else if (n < nextCand * nextCand)
            {
                CalcUntilI(n);
                b = primes.Last() == n;
            }
            else
            {
                CalcUntilI((int)System.Math.Sqrt(n));
                for (int i = 0; i < primes.Count; i++)
                    if (n % primes[i] == 0)
                    {
                        b = false;
                        break;
                    }
            }
            threadQueue.Next();
            return b;
        }

        public static bool IsProbPrime(int n)
        {
            threadQueue.Wait();
            bool b = true;
            if (n < nextCand)
                b = Contain(n);
            else
            {
                //CalcUntilI((int)System.Math.Sqrt(System.Math.Sqrt(n)));
                CalcUntilI((int)System.Math.Log(n));
                for (int i = 0; i < primes.Count; i++)
                    if (n % primes[i] == 0)
                    {
                        b = false;
                        break;
                    }
            }
            threadQueue.Next();
            return b;
        }

        static void CalcUntilIthPrime(int exclusiveI)
        {
            while (primes.Count < exclusiveI)
                TestNextCand();
        }

        static void CalcUntilI(int inclusiveI)
        {
            if (inclusiveI < (nextCand << 1))
                while (primes.Last() < inclusiveI)
                    TestNextCand();
            else
                Sieve(inclusiveI + 1);
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
            var sieve = new BitArray(exclusive, true);
            for (int i = 1; i < primes.Count; i++)
                for (int j = primes[i] * primes[i], k = primes[i] << 1; j < sieve.Count; sieve[j] = false, j += k) ;
            for (int i = nextCand; i * i <= sieve.Count; i += 2)
                if (sieve[i])
                {
                    primes.Add(i);
                    for (int j = i * i, k = i << 1; j < sieve.Count; sieve[j] = false, j += k) ;
                }
            for (int i = primes.Last() + 2; i < sieve.Count; i += 2)
                if (sieve[i])
                    primes.Add(i);
            nextCand = sieve.Count + ((sieve.Count + 1) & 1);
        }
    }
}