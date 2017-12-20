using System.Linq;

namespace DLib.Math
{
    public static class ArithmeticFunction
    {
        //macht keinen sinn weil combine nicht alle kombinationen sondern nur welche maximaler länge returnded;
        public static ulong Divisor(ulong n, ulong e) => (ulong)Combinatorics.CombineEveryLength(Prime.Factorise.Standard(n, Factor.TrialDivison)).Select(i => i.Aggregate(1, (acc, val) => acc * (int)val)).Aggregate(0, (acc, val) => acc + (int)System.Math.Pow(val, e));
    }
}
