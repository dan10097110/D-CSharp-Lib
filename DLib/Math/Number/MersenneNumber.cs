using DLib.Collection;
using Mpir.NET;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Math.Number
{
    public class MersenneNumber
    {
        const double log2 = 0.693147180559945309417232121458176568075500134360255254120, c = -0.66444870745388938334802927784457284837943064347344078207;
        public int Exponent { get; private set; }
        mpz_t m;
        bool? isPrime = null;
        public bool IsPrime
        {
            get
            {
                if (isPrime == null)
                    isPrime = Primes.IsPrime(Exponent) && ((Exponent & 3) != 3 || !Primes.IsPrime((Exponent << 1) + 1)) && TrialDivision()  && LucasLehmerTest();
                return (bool)isPrime;
            }
        }

        public MersenneNumber(int exponent)
        {
            Exponent = exponent;
            m = mpz_t.One.ShiftLeft(exponent) - 1;
        }

        bool Divisible(int d) => Operator.Mod.PowerOfTwoModM(Exponent, d) == 1;

        bool LucasLehmerTest()
        {
            int i = (int)(System.Math.Log(Exponent) / log2);// + c);
            var s = mpz_t.Power(3, (int)System.Math.Pow(2, i - 1));
            for (; i < Exponent; i++)
                mpir.mpz_powm_ui(s, s, 2, m);
            return s == m - 3;
        }

        bool TrialDivision()
        {
            int i = 0;
            switch (Exponent % 60)
            {
                case 1: i = 7; break;
                case 7: i = 4; break;
                case 11: i = 15; break;
                case 13: i = 3; break;
                case 17: i = 5; break;
                case 19: i = 1; break;
                case 23: i = 13; break;
                case 29: i = 2; break;
                case 31: i = 11; break;
                case 37: i = 0; break;
                case 41: i = 12; break;
                case 43: i = 8; break;
                case 47: i = 10; break;
                case 49: i = 14; break;
                case 53: i = 9; break;
                case 59: i = 6; break;
            }
            int two = Exponent << 1, six = 3 * two, ten = 5 * two;
            var d = new int[] { six, ten, Exponent << 3, six, ten, six, two, six, ten, six, Exponent << 3, ten, six, two, 22 * Exponent, two };
            for (int dividend = 1, limit = Exponent * Exponent; (dividend += d[i]) < limit; i = (i + 1) & 15)
                if (Primes.IsProbPrime(dividend) && m % dividend == 0)
                    return false;
            return true;
        }

        bool TrialDivision1()
        {
            Primes2.CalcUntilI(Exponent * Exponent);
            for (int i = 2, prime; (prime = Primes2.GetIth(i)) < Exponent * Exponent; i++)
            {
                int r = prime & 3;
                if ((r == 1 || r == 7) && ((prime - 1) >> 1) % Exponent == 0 && m % prime == 0)
                    return false;
            }
            return true;
        }

        bool TrialDivision2()
        {
            var sieve = new BitArray(Exponent * Exponent, true);
            for (int i = 3; i <= Exponent; i += 2)
                if (sieve[i])
                {
                    int r = i & 3;
                    if ((r == 1 || r == 7) && ((i - 1) >> 1) % Exponent == 0 && m % i == 0)
                        return false;
                    for (int j = i * i, k = i << 1; j < Exponent * Exponent; sieve[j] = false, j += k) ;
                }
            for (int i = Exponent + 1 + (Exponent & 1); i < Exponent * Exponent; i += 2)
                if (sieve[i])
                {
                    int r = i & 3;
                    if ((r == 1 || r == 7) && ((i - 1) >> 1) % Exponent == 0 && m % i == 0)
                        return false;
                }
            return true;
        }

        bool Pollard(int b)
        {
            var primes = Prime.Sieve.Standard((ulong)b);
            int e = primes.Aggregate(1, (acc, val) => acc * (int)val);
            int x = (int)System.Math.Pow(3, (e * Exponent) << 1);
            int gcd = (int)GCD.Standard((ulong)x - 1, (ulong)m);
            if (gcd > 1 && gcd != (int)m)
                return false;
            return true;
        }
    }

    public class MersenneNumber2
    {
        static List<int> primes = new List<int>() { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59 };
        static mpz_t prod = 2;
        static int untilIndex = 0;
        static ThreadQueue tq = new ThreadQueue(), tq2 = new ThreadQueue();

        void GenPrime()
        {
            tq.Wait();
            while (Exponent * Exponent > primes.Last())
                SquareSieve();
            tq.Next();
        }

        void CalcProd()
        {
            tq2.Wait();
            for (; primes[untilIndex] < Exponent * Exponent; untilIndex++, prod *= primes[untilIndex]) ;
            tq2.Next();
        }

        static void SquareSieve()
        {
            var sieve = new BitArray(primes.Last() * primes.Last(), true);
            for (int i = 1; i < primes.Count; i++)
                for (int j = primes[i] * primes[i], k = primes[i] << 1; j < sieve.Count; sieve[j] = false, j += k) ;
            for (int i = primes.Last() + 1; i < sieve.Count; i += 2)
                if (sieve[i])
                    primes.Add(i);
        }

        const double log2 = 0.693147180559945309417232121458176568075500134360255254120, c = -0.66444870745388938334802927784457284837943064347344078207;
        public int Exponent { get; private set; }
        mpz_t m;
        bool? isPrime = null;
        public bool IsPrime
        {
            get
            {
                if (isPrime == null)
                    isPrime = Primes.IsPrime(Exponent) && ((Exponent & 3) != 3 || !Primes.IsPrime((Exponent << 1) + 1)) && TrialDivision() && LucasLehmerTest();
                return (bool)isPrime;
            }
        }

        public MersenneNumber2(int exponent)
        {
            Exponent = exponent;
            m = mpz_t.One.ShiftLeft(exponent) - 1;
        }

        bool Divisible(int d) => Operator.Mod.PowerOfTwoModM(Exponent, d) == 1;

        bool LucasLehmerTest()
        {
            int i = (int)(System.Math.Log(Exponent) / log2);// + c);
            var s = mpz_t.Power(3, (int)System.Math.Pow(2, i - 1));
            for (; i < Exponent; i++)
                mpir.mpz_powm_ui(s, s, 2, m);
            return s == m - 3;
        }

        bool TrialDivision()
        {
            int i = 0;
            switch (Exponent % 60)
            {
                case 1: i = 7; break;
                case 7: i = 4; break;
                case 11: i = 15; break;
                case 13: i = 3; break;
                case 17: i = 5; break;
                case 19: i = 1; break;
                case 23: i = 13; break;
                case 29: i = 2; break;
                case 31: i = 11; break;
                case 37: i = 0; break;
                case 41: i = 12; break;
                case 43: i = 8; break;
                case 47: i = 10; break;
                case 49: i = 14; break;
                case 53: i = 9; break;
                case 59: i = 6; break;
            }
            int two = Exponent << 1, six = 3 * two, ten = 5 * two;
            var d = new int[] { six, ten, Exponent << 3, six, ten, six, two, six, ten, six, Exponent << 3, ten, six, two, 22 * Exponent, two };
            for (int dividend = 1, limit = Exponent * Exponent; (dividend += d[i]) < limit; i = (i + 1) & 15)
                if (Primes.IsProbPrime(dividend) && m % dividend == 0)
                    return false;
            return true;
        }

        bool Pollard(int b)
        {
            GenPrime();
            CalcProd();
            var v = mpz_t.Three;
            mpir.mpz_powm(v, v, (prod * Exponent) << 1, m);
            var gcd = mpz_t.Gcd(v - 1, m);
            if (gcd > mpz_t.One && gcd != m)
                return false;
            return true;
        }
    }
}
