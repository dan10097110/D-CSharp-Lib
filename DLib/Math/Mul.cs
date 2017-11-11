namespace DLib.Math
{
    public static class Mul
    {
        public static ulong Binary(ulong a, ulong b)
        {
            if (b == 0)
                return 0;
            ulong r = 0;
            for (; a > 0; a >>= 1, b <<= 1)
                if ((a & 1) == 1)
                    r += b;
            return r;
        }

        public static ulong Mod(ulong a, ulong b, ulong m)
        {
            a = a % m;
            b = b % m;
            if (b == 0)
                return 0;
            ulong r = 0;
            for (; a > 0 && b > 0; a >>= 1, b = (b << 1) % m)
                if ((a & 1) == 1)
                    r = (r + b) % m;
            return r;
        }
    }
}
