using Mpir.NET;

namespace DLib.Math.Operator
{
    public static class Mod
    {
        public static mpz_t MersennePrime(mpz_t n, ulong exponent, mpz_t mersenneNumber)
        {
            mpz_t s = n;
            for (; s.BitLength > (int)exponent; s = (s & mersenneNumber) + (s >> (int)exponent)) ;
            return s == mersenneNumber ? 0 : s;
        }

        public static int PowerOfTwoModM(int exponent, int m)
        {
            int r = 1;
            for (var b = Extra.ToBinaryStack(exponent); b.Count > 0; r = ((r * r) << b.Pop()) % m) ;
            return r;
        }

        public static int PowerOfTwoModM(int[] exponent, int m)
        {
            int r = 1;
            for (int i = exponent.Length - 1; i >= 0; r = ((r * r) << exponent[i]) % m, i--) ;
            return r;
        }
    }
}
