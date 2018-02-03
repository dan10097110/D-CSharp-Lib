using Mpir.NET;
using System.Collections.Generic;

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

        public static int PowerOfTwoModM(Stack<int> exponent, int m)
        {
            int r = 1;
            for (exponent = new Stack<int>(exponent); exponent.Count > 0; r = ((r * r) << exponent.Pop()) % m) ;
            return r;
        }
    }
}
