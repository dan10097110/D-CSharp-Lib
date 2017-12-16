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
    }
}
