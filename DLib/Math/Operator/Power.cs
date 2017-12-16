namespace DLib.Math.Operator
{
    public static class Power
    {
        public static ulong Binary(ulong b, ulong e)
        {
            if (e == 0)
                return 1;
            ulong r = 1;
            for (; e > 0; e >>= 1)
            {
                if ((e & 1) == 1)
                    r *= b;
                b *= b;
            }
            return r;
        }

        public static ulong BinaryMod(ulong b, ulong e, ulong m)
        {
            if (e == 0)
                return 1;
            ulong r = 1;
            for (; e > 0; e >>= 1)
            {
                if ((e & 1) == 1)
                    r = (r * b) % m;
                b = (b * b) % m;
            }
            return r;
        }

        public static ulong BinaryMod2(ulong b, ulong e, ulong m)
        {
            if (e == 0)
                return 1;
            ulong r = 1;
            for (; e > 0; e >>= 1)
            {
                if ((e & 1) == 1)
                    r = Mul.Mod(r, b, m);
                b = Mul.Mod(b, b, m);
            }
            return r;
        }
    }
}
