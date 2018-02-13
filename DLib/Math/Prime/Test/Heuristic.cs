namespace DLib.Math.Prime.Test
{
    public static class Heuristic
    {
        public static bool Selfridge(ulong n) => Problably.Fermat(n, 2) && Problably.FibonacciAlternative(n);

        public static bool BailliePSW(ulong n) => Problably.Strong(n, 2) && Problably.StrongLucas(n);

        public static bool Giuga(int n)
        {
            int m = 1;
            for (int i = 2; i < n; m = (m + (int)Operator.Power.BinaryMod((ulong)i, (ulong)(n - 1), (ulong)n)) % n, i++) ;
            return m == n - 1;
        }
    }
}
