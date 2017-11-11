namespace DLib.Math
{
    public static class GCD
    {
        public static ulong Standard(params ulong[] n)
        {
            ulong gcd = n[1];
            for (int i = 1; i < n.Length; i++)
                gcd = Standard(gcd, n[i]);
            return gcd;
        }

        static ulong Standard(ulong a, ulong b) => Euclid(a, b);

        public static ulong Binary(ulong a, ulong b)
        {
            if (a == 0)
                return b;
            if (b == 0)
                return a;
            uint shift = 0;
            for (; ((a | b) & 1) == 0; a >>= 1, b >>= 1, shift++) ;
            for (; (a & 1) == 0; a >>= 1) ;
            while (b != 0)
            {
                for (; (b & 1) == 0; b >>= 1) ;
                if (a > b)
                    Extra.Swap(ref a, ref b);
                b -= a;
            }
            return a << (int)shift;
        }

        public static ulong Euclid(ulong a, ulong b)
        {
            for (; b != 0;)
            {
                ulong c = a % b;
                a = b;
                b = c;
            }
            return a;
        }

        public static ulong Lehmer(ulong a, ulong b) => 1;
    }
}
