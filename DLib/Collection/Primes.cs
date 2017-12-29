using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLib.Collection
{
    public static class Primes
    {
        static List<ulong> primes = new List<ulong>() { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47 };
        static ulong nextCand = 3;
        
        public static ulong GetIth(int i)
        {
            if (i >= primes.Count)
                CalcUntilIthPrime(i + 1);
            return primes[i];
        }

        public static bool IsPrime(ulong n)
        {
            if(n <= primes.Last())
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
            else
            {
                CreateUntilI(n);
                return primes.Last() == n;
            }
        }

        static void CalcUntilIthPrime(int exclusiveI)
        {
            while (primes.Count < exclusiveI)
                TestNextCand();
        }

        static void CalcUntilI(ulong inclusiveI)
        {
            while (primes.Last() < inclusiveI)
                TestNextCand();
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
    }
}