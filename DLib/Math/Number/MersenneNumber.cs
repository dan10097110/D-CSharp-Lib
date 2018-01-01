using DLib.Collection;
using Mpir.NET;

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
                    isPrime = Primes.IsPrime(Exponent) && ((Exponent & 3) != 3 || !Primes.IsPrime((Exponent << 1) + 1)) && TrialDivision() && LucasLehmerTest();
                return (bool)isPrime;
            }
        }

        public MersenneNumber(int exponent)
        {
            Exponent = exponent;
            m = mpz_t.One.ShiftLeft(exponent) - 1;
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

        bool LucasLehmerTest()
        {
            int i = (int)(System.Math.Log(Exponent) / log2);// + c);
            var s = mpz_t.Power(3, (int)System.Math.Pow(2, i - 1));
            for (; i < Exponent; i++)
                mpir.mpz_powm_ui(s, s, 2, m);
            return s == m - 3;
        }
    }
}
