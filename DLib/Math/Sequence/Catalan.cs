namespace DLib.Math.Sequence
{
    public static class Catalan
    {
        public static ulong Get(ulong n)
        {
            double c = 1;
            for (ulong k = 2; k <= n; k++)
                c *= ((n + k) / (double)k);
            return (ulong)System.Math.Round(c);
        }

        public static ulong ModM(ulong n, ulong m)
        {
            return 1;
        }
    }
}
