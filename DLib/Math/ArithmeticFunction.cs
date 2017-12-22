using System.Linq;

namespace DLib.Math
{
    public static class ArithmeticFunction
    {
        public static ulong Divisor(ulong n, ulong e) => (ulong)Combinatorics.CombineEveryLength(Prime.Factorise.Optimised(n)).Select(i => i.Aggregate(1, (acc, val) => acc * (int)val)).Aggregate(0, (acc, val) => acc + (int)System.Math.Pow(val, e)) + 1;
    }
}
