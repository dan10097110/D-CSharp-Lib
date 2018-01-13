namespace DLib.Math.Number
{
    public class UInt128
    {
        ulong first, second;

        public UInt128(ulong u)
        {
            first = 0;
            second = u;
        }

        public UInt128(ulong first, ulong second)
        {
            this.first = first;
            this.second = second;
        }


        public static bool operator ==(UInt128 a, UInt128 b) => a.CompareTo(b) == 0;

        public static bool operator !=(UInt128 a, UInt128 b) => a.CompareTo(b) != 0;

        public static bool operator >(UInt128 a, UInt128 b) => a.CompareTo(b) == 1;

        public static bool operator <=(UInt128 a, UInt128 b) => a.CompareTo(b) != 1;

        public static bool operator <(UInt128 a, UInt128 b) => a.CompareTo(b) == -1;

        public static bool operator >=(UInt128 a, UInt128 b) => a.CompareTo(b) != -1;

        public int CompareTo(UInt128 a)
        {
            if (first == a.first)
                return second == a.second ? 0 : (second < a.second ? -1 : 1);
            else if (first < a.first)
                return -1;
            else
                return 1;
        }


        public static UInt128 operator +(UInt128 a, UInt128 b) => Add(a, b);

        public static UInt128 operator *(UInt128 a, UInt128 b) => Mul(a.ToULong(), b.ToULong());

        public static UInt128 operator /(UInt128 a, ulong d) => Div(a, d);

        public static ulong operator %(UInt128 a, ulong m) => Mod(a, m);


        public static UInt128 Add(UInt128 a, UInt128 b)
        {
            double d = a.second / (double)ulong.MaxValue + b.second / (double)ulong.MaxValue;
            return new UInt128(a.first + b.first + (ulong)d, (ulong)System.Math.Round((d - (ulong)d) * ulong.MaxValue));
        }

        public static UInt128 Mul(ulong a, ulong b)
        {
            double d = (a / (double)ulong.MaxValue) * b;
            return new UInt128((ulong)d, (ulong)System.Math.Round((d - (ulong)d) * ulong.MaxValue));
        }

        public static UInt128 Div(UInt128 a, ulong m)
        {
            double d = a.first / m;
            return new UInt128((ulong)d, (ulong)(a.second / (double)m)) + new UInt128((ulong)((d - (ulong)d) * ulong.MaxValue));
        }

        public static ulong Mod(UInt128 a, ulong m) => AddMod(MulMod(a.first, ulong.MaxValue, m), a.second % m, m);

        public static ulong AddMod(ulong a, ulong b, ulong m)
        {
            a %= m;
            b %= m;
            double d = a / (double)ulong.MaxValue + b / (double)ulong.MaxValue;
            if ((ulong)d == 0)
                return (ulong)System.Math.Round((d - (ulong)d) * ulong.MaxValue) % m;
            return AddMod(MulMod((ulong)d, ulong.MaxValue, m), (ulong)System.Math.Round((d - (ulong)d) * ulong.MaxValue) % m, m);
        }

        public static ulong MulMod(ulong a, ulong b, ulong m)
        {
            a %= m;
            b %= m;
            double d = (a / (double)ulong.MaxValue) * b;
            if ((ulong)d == 0)
                return (ulong)System.Math.Round((d - (ulong)d) * ulong.MaxValue) % m;
            return AddMod(MulMod((ulong)d, ulong.MaxValue, m), (ulong)System.Math.Round((d - (ulong)d) * ulong.MaxValue) % m, m);
        }


        public static UInt128 Power(ulong c, ulong e)
        {
            if (e == 0)
                return 1;
            UInt128 r = 1, b = c;
            for (; e > 0; e >>= 1)
            {
                if ((e & 1) == 1)
                    r *= b;
                b *= b;
            }
            return r;
        }


        public ulong ToULong()
        {
            if (first > 0)
                throw new System.Exception("UInt128 > UInt64");
            return second;
        }

        public static implicit operator UInt128(ulong u) => new UInt128(u);
    }
}
