using System.Linq;

namespace DLib.Math
{
    public static class ArithmeticFunction
    {
        public static ulong Sigma(ulong n, ulong e) => (ulong)Combinatorics.CombineEveryLength(Prime.Factorise.Optimised(n)).Select(i => i.Aggregate(1, (acc, val) => acc * (int)val)).Aggregate(0, (acc, val) => acc + (int)System.Math.Pow(val, e)) + 1;

        public static class Pi
        {
            public static ulong Approx(ulong n) => (ulong)(n / System.Math.Log(n));

            public static ulong Exact(ulong n) => (ulong)Prime.Sieve.Standard(n).Count();
        }
    }
}
