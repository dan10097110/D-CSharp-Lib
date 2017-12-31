using DLib.Math.Operator;
using Mpir.NET;
using System.Collections.Generic;

namespace DLib.Math.Prime
{
    public static class Mersenne
    {
        const double sqrt2 = 1.414;

        public static bool Test(ulong exponent, ref ulong startI, ref mpz_t startS, List<ulong> primes)
        {
            double sqrtExp = System.Math.Sqrt(exponent);
            if (!Prime.Test.Probabilistic.Division(exponent, primes, 3, (ulong)sqrtExp + 1))
                return false;
            lock (primes)
            {
                int i = primes.Count;
                for (; i > 0 && primes[i - 1] > exponent; i--) ;
                if (i > 0 && primes[i - 1] != exponent)
                    primes.Insert(i, exponent);
            }
            if ((exponent & 3) == 3 && Prime.Test.Probabilistic.Division((exponent << 1) + 1, primes, 3, (ulong)(sqrtExp * sqrt2) + 1))
                return false;
            mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1;
            return TrialDivision(exponent, mersenneNumber, primes) && LucasLehmerTest.Start10(exponent, mersenneNumber, ref startI, ref startS);
        }
        
        public static bool Test2(ulong exponent, ref ulong startI, ref mpz_t startS, List<ulong> primes)
        {
            double sqrtExp = System.Math.Sqrt(exponent);
            if (!Prime.Test.Probabilistic.Division(exponent, primes, 3, (ulong)System.Math.Sqrt(exponent) + 1) || ((exponent & 3) == 3 && Prime.Test.Probabilistic.Division((exponent << 1) + 1, primes, 3, (ulong)(sqrtExp + sqrt2) + 1)))
                return false;
            mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1;
            return TrialDivision(exponent, mersenneNumber, primes) && LucasLehmerTest.Start10(exponent, mersenneNumber, ref startI, ref startS);
        }
        
        public static bool Test(ulong exponent, ref ulong startI, ref mpz_t startS)
        {
            if (!Collection.Primes.IsPrime((int)exponent) || ((exponent & 3) == 3 && Collection.Primes.IsPrime(((int)exponent << 1) + 1)))
                return false;
            mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1;
            return TrialDivision(exponent, mersenneNumber) && LucasLehmerTest.Fastest(exponent, mersenneNumber, ref startI, ref startS);
        }

        public static class LucasLehmerTest
        {
            public static bool Fastest(ulong exponent, mpz_t mersenneNumber, ref ulong startI, ref mpz_t startS) => Start10(exponent, mersenneNumber, ref startI, ref startS);

            public static bool Start01(ulong exponent)//2669
            {
                mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1, s = 4;
                for (ulong i = 2; i < exponent; s = s < 2 ? mersenneNumber : s - 2, i++)
                    mpir.mpz_powm(s, s, 2, mersenneNumber);
                return s == 0;
            }

            public static bool Start02(ulong exponent)//2751
            {
                mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1, s = 3;
                for (ulong i = 1; i < exponent; i++)
                    mpir.mpz_powm(s, s, 2, mersenneNumber);
                return s == mersenneNumber - 3;
            }

            public static bool Start03(ulong exponent)//3199
            {
                mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1, s = 4;
                for (ulong i = 2; i < exponent; i++)
                    s = Mod.MersennePrime((s * s) - 2, exponent, mersenneNumber);
                return s == 0;
            }

            public static bool Start04(ulong exponent)//3441
            {
                mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1, s = 3;
                for (ulong i = 1; i < exponent; i++)
                    s = Mod.MersennePrime(s * s, exponent, mersenneNumber);
                return s == mersenneNumber - 3;
            }

            public static bool Start05(ulong exponent)//3313
            {
                mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1, s = 4;
                for (ulong i = 2; i < exponent; i++)
                    s = ((s * s) - 2) % mersenneNumber;
                return s == 0;
            }

            public static bool Start06(ulong exponent)//3421
            {
                mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1, s = 3;
                for (ulong i = 1; i < exponent; i++)
                    s = (s * s) % mersenneNumber;
                return s == mersenneNumber - 3;
            }

            public static bool Start07(ulong exponent)//3745
            {
                mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1;
                return mpz_t.Three.PowerMod(mpz_t.One.ShiftLeft((int)exponent - 1) - 1, mersenneNumber) == mersenneNumber - 1;
            }

            public static bool Start08(ulong exponent)//3863
            {
                mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1;
                return mpz_t.Three.PowerMod(mpz_t.One.ShiftLeft((int)exponent - 1), mersenneNumber) == mersenneNumber - 3;
            }

            //1,3
            public static bool Start09(ulong exponent, mpz_t mersenneNumber, ref ulong startI, ref mpz_t startS)//3751
            {
                mpz_t s = startS.Clone();
                for (ulong i = startI; i < exponent; i++)
                    mpir.mpz_powm_ui(s, s, 2, mersenneNumber);
                if (startS < mersenneNumber)
                {
                    startI++;
                    startS *= startS;
                }
                return s == mersenneNumber - 3;
            }

            //1,3
            public static bool Start10(ulong exponent, mpz_t mersenneNumber, ref ulong startI, ref mpz_t startS)//3815
            {
                mpz_t s = startS.Clone();
                if ((startI & 1) == 0)
                    mpir.mpz_powm_ui(s, s, 2, mersenneNumber);
                for (ulong i = startI + ((startI - 1) & 1); i < exponent; i += 2)
                    mpir.mpz_powm_ui(s, s, 4, mersenneNumber);
                if (startS < mersenneNumber)
                {
                    startI++;
                    startS *= startS;
                }
                return s == mersenneNumber - 3;
            }//fastest

            //1,3
            public static bool Start11(ulong exponent, ref ulong startI, ref mpz_t startS)//3969
            {
                mpz_t mersenneNumber = mpz_t.One.ShiftLeft((int)exponent) - 1, s = startS, e = mpz_t.One.ShiftLeft((int)(exponent - startI));
                if (s < mersenneNumber)
                {
                    startI++;
                    startS *= startS;
                }
                return s.PowerMod(e, mersenneNumber) == mersenneNumber - 3;
            }
        }

        public static bool TrialDivision(ulong exponent, mpz_t mersenneNumber, List<ulong> primes)
        {
            int i = 0;
            switch (exponent % 60)
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
            ulong two = exponent << 1, six = 3 * two, ten = 5 * two;
            var d = new ulong[] { six, ten, exponent << 3, six, ten, six, two, six, ten, six, exponent << 3, ten, six, two, 22 * exponent, two };
            for (ulong dividend = 1, limit = exponent * exponent; (dividend += d[i]) < limit; i = (i + 1) & 15)
                if (Prime.Test.Probabilistic.Division(dividend, primes, 7, (ulong)System.Math.Sqrt(System.Math.Sqrt(dividend))) && mersenneNumber % dividend == 0)
                    return false;
            return true;
        }

        public static bool TrialDivision(ulong exponent, mpz_t mersenneNumber)
        {
            int i = 0;
            switch (exponent % 60)
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
            ulong two = exponent << 1, six = 3 * two, ten = 5 * two;
            var d = new ulong[] { six, ten, exponent << 3, six, ten, six, two, six, ten, six, exponent << 3, ten, six, two, 22 * exponent, two };
            for (ulong dividend = 1, limit = exponent * exponent; (dividend += d[i]) < limit; i = (i + 1) & 15)
                if (Collection.Primes.IsProbPrime((int)dividend) && mersenneNumber % dividend == 0)
                    return false;
            return true;
        }
    }
}
