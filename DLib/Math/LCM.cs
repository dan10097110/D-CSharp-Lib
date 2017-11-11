namespace DLib.Math
{
    public static class LCM
    {
        public static ulong Standard(ulong a, ulong b) => a * b / GCD.Standard(a, b);

        public static ulong Binary(ulong a, ulong b) => a * b / GCD.Binary(a, b);

        public static ulong Euclid(ulong a, ulong b) => a * b / GCD.Euclid(a, b);

        public static ulong Lehmer(ulong a, ulong b) => a * b / GCD.Lehmer(a, b);
    }
}
