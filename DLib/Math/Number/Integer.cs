namespace DLib.Math.Number
{
    public class Integer
    {
        Natural natural;
        public bool Positive { get; private set; }


        public static Integer Zero => new Integer() { natural = Natural.Zero, Positive = true };

        public static Integer One => new Integer() { natural = Natural.One, Positive = true };

        public static Integer Two => new Integer() { natural = Natural.Two, Positive = true };

        public static Integer Three => new Integer() { natural = Natural.Three, Positive = true };

        public static Integer MinusOne => new Integer() { natural = Natural.One, Positive = false };

        public static Integer MinusTwo => new Integer() { natural = Natural.Two, Positive = false };

        public static Integer MinusThree => new Integer() { natural = Natural.Three, Positive = false };


        public Integer() { }

        public Integer(long u)
        {
            Positive = u >= 0;
            natural = new Natural((ulong)u);
        }

        public Integer(Natural natural)
        {
            this.natural = natural.Clone();
            Positive = true;
        }

        public Integer(Integer integer)
        {
            natural = integer.natural.Clone();
            Positive = integer.Positive;
        }


        public static bool operator ==(Integer a, Integer b) => Compare(a, b) == 0;

        public static bool operator !=(Integer a, Integer b) => Compare(a, b) != 0;

        public static bool operator <(Integer a, Integer b) => Compare(a, b) == -1;

        public static bool operator >(Integer a, Integer b) => Compare(a, b) == 1;

        public static bool operator <=(Integer a, Integer b) => Compare(a, b) != 1;

        public static bool operator >=(Integer a, Integer b) => Compare(a, b) != -1;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>a < b: - 1; a == b: 0; a > b: == 1</b></returns>
        public static int Compare(Integer a, Integer b)
        {
            int i = Natural.Compare(a.Abs(), b.Abs());
            if (i == 0 && a.IsZero())
                return 0;
            return (a.Positive ? 1 : -1) * (a.Positive != b.Positive ? 1 : i);
        }


        public static Integer operator ++(Integer a) => a + Natural.One;

        public static Integer operator --(Integer a) => a - Natural.One;

        public static Integer operator +(Integer a, Integer b)
        {
            if (a.Positive == b.Positive)
                return new Integer() { natural = a.Abs() + b.Abs(), Positive = a.Positive };
            if (a.Abs() > b.Abs())
                return new Integer() { natural = a.Abs() - b.Abs(), Positive = a.Positive };
            else
                return new Integer() { natural = b.Abs() - a.Abs(), Positive = b.Positive };
        }

        public static Integer operator -(Integer a, Integer b) => a + new Integer() { natural = b.natural, Positive = !b.Positive };

        public static Integer operator *(Integer a, Integer b) => new Integer() { natural = a.natural * b.natural, Positive = a.Positive == b.Positive };

        public static Integer operator /(Integer a, Integer b) => a.Positive == b.Positive ? a.Abs() / b.Abs() : ((Integer)(a.Abs() / b.Abs())).Invert();

        public static Natural operator %(Integer a, Integer b) => a.Positive ? a % b.Abs() : ((a.Abs() % b.Abs()) - b.Abs());
        
        public static Integer operator <<(Integer a, int i) => new Integer() { natural = a.natural << i, Positive = a.Positive };

        public static Integer operator >>(Integer a, int i) => new Integer() { natural = a.natural >> i, Positive = a.Positive };
        

        public static Integer Power(Integer b, Natural e) => new Integer() { natural = Natural.Power(b.Abs(), e), Positive = b.Positive ? true : (e & 1) == 0 };
        

        public Natural Abs() => natural.Clone();

        public int Sign() => Positive ? 1 : -1;

        public Integer Invert() => new Integer() { natural = natural, Positive = !Positive };


        public bool IsZero() => natural == Natural.Zero;


        public Integer Clone() => new Integer(this);

        public override string ToString() => ToDecimal().ToString();

        public long ToDecimal() => (long)natural.ToDecimal() * (Positive ? 1 : -1);
        
        public static implicit operator Integer(Natural natural) => new Integer(natural);

        public static implicit operator Integer(long u) => new Integer(u);

        public static implicit operator string(Integer n) => n.ToString();

        public static explicit operator Natural(Integer integer) => integer.Abs();

        public static explicit operator long(Integer integer) => integer.ToDecimal();
    }
}
