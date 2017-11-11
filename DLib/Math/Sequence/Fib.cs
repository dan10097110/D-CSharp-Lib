namespace DLib.Math.Sequence
{
    public static class Fib
    {
        const double sqrt5 = 2.2360679775;

        public static ulong Recursive(ulong n) => n < 2 ? n : Recursive(n - 1) + Recursive(n - 2);

        public static ulong Iterative(ulong n)
        {
            ulong a = 0, b = 1;
            for (ulong i = 1; i < n; i++)
            {
                ulong tmp = a + b;
                a = b;
                b = tmp;
            }
            return b;
        }

        public static ulong Binet(ulong n) => (ulong)System.Math.Round((System.Math.Pow((1 + sqrt5) / 2, n) - System.Math.Pow((1 - sqrt5) / 2, n)) / sqrt5);

        public static long LucasModM(ulong n, ulong m) => Sequence.Lucas.NthModM(1, -1, (long)n, (long)m).U;

        public static long Lucas(ulong n) => Sequence.Lucas.Nth(1, -1, (long)n).U;
    }
}
