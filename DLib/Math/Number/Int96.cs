namespace DLib.Math.Number
{
    public class Int96
    {
        decimal d;


        public Int96() => d = 0;

        public Int96(long p) => d = p;

        public Int96(ulong p) => d = p;

        public Int96(int p) => d = p;

        public Int96(uint p) => d = p;

        public Int96(short p) => d = p;

        public Int96(ushort p) => d = p;


        public static Int96 operator +(Int96 a, Int96 b) => new Int96() { d = a.d + b.d };

        public static Int96 operator -(Int96 a, Int96 b) => new Int96() { d = a.d - b.d };

        public static Int96 operator *(Int96 a, Int96 b) => new Int96() { d = a.d * b.d };

        public static Int96 operator /(Int96 a, Int96 b) => new Int96() { d = a.d / b.d };

        public static Int96 operator %(Int96 a, Int96 b) => new Int96() { d = a.d % b.d };

        public static Int96 operator ++(Int96 a) => new Int96() { d = a.d++ };

        public static Int96 operator --(Int96 a) => new Int96() { d = a.d-- };

        public static bool operator ==(Int96 a, Int96 b) => a.d == b.d;

        public static bool operator !=(Int96 a, Int96 b) => a.d != b.d;

        public static bool operator <(Int96 a, Int96 b) => a.d < b.d;

        public static bool operator >(Int96 a, Int96 b) => a.d > b.d;

        public static bool operator <=(Int96 a, Int96 b) => a.d <= b.d;

        public static bool operator >=(Int96 a, Int96 b) => a.d >= b.d;


        public override string ToString() => d.ToString();


        public static implicit operator Int96(long p) => new Int96(p);

        public static implicit operator Int96(ulong p) => new Int96(p);

        public static implicit operator Int96(int p) => new Int96(p);

        public static implicit operator Int96(uint p) => new Int96(p);

        public static implicit operator Int96(short p) => new Int96(p);

        public static implicit operator Int96(ushort p) => new Int96(p);

        public static implicit operator string(Int96 p) => p.ToString();
    }
}
