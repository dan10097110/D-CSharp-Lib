namespace DLib.Math.Prime.Test
{
    public static class Heuristic
    {
        public static bool Selfridge(ulong n) => Problably.Fermat(n, 2) && Problably.FibonacciAlternative(n);

        public static bool BailliePSW(ulong n) => Problably.Strong(n, 2) && Problably.StrongLucas(n);
    }
}
