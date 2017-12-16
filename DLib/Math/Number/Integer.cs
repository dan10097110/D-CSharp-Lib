namespace DLib.Math.Number
{
    public class Integer
    {
        Natural natural;
        public bool Positive { get; private set; }

        public Integer() { }

        public Integer(long u)
        {
            Positive = u >= 0;
            natural = new Natural((ulong)u);
        }

        public Integer(Natural natural)
        {
            natural = natural.Clone();
            Positive = true;
        }

        public Integer(Integer integer)
        {
            natural = integer.natural.Clone();
            Positive = integer.Positive;
        }


        public static Integer operator +(Integer a, Integer b)
        {
            if (a.Positive == b.Positive)
                return new Integer() { natural = Abs(a) + Abs(b), Positive = a.Positive };
            if (Abs(a) > Abs(b))
                return new Integer() { natural = Abs(a) - Abs(b), Positive = a.Positive };
            else
                return new Integer() { natural = Abs(b) - Abs(a), Positive = b.Positive };
        }

        public static Integer operator ++(Integer a) => a + Natural.One;

        public static Integer operator -(Integer a, Integer b) => a + new Integer() { natural = b.natural, Positive = !b.Positive };

        public static Integer operator --(Integer a) => a - Natural.One;

        public static Integer operator *(Integer a, Integer b) => new Integer() { natural = a.natural * b.natural, Positive = a.Positive == b.Positive };

        public static Natural operator %(Integer a, Integer b) => a.Positive ? a % Abs(b) : Abs((Abs(a) % Abs(b)) - Abs(b));

        public static Integer operator /(Integer a, Integer b) => a.Positive == b.Positive ? a.Abs() / b.Abs() : ((Integer)(a.Abs() / b.Abs())).Invert();


        public static Integer operator <<(Integer a, int i) => new Integer() { natural = a.natural << i, Positive = a.Positive };

        public static Integer operator >>(Integer a, int i) => new Integer() { natural = a.natural >> i, Positive = a.Positive };


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
        public static int Compare(Integer a, Integer b) => (a.Positive ? 1 : -1) * (a.Positive != b.Positive ? 1 : Natural.Compare(Abs(a), Abs(b)));


        public static Integer Power(Integer b, Natural e) => new Integer() { natural = Natural.Power(Abs(b), e), Positive = b.Positive ? true : (e & 1) == 0 };


        public static Natural Abs(Integer a) => a.natural.Clone();

        public Natural Abs() => natural.Clone();

        public int Sign() => Positive ? 1 : -1;

        public static Integer Invert(Integer a) => new Integer() { natural = a.natural, Positive = !a.Positive };

        public Integer Invert() => new Integer() { natural = natural, Positive = !Positive };


        public override string ToString() => ToDecimal().ToString();

        public long ToDecimal() => (long)natural.ToDecimal() * (Positive ? 1 : -1);


        public Integer Clone() => new Integer(this);


        public static implicit operator Integer(Natural natural) => new Integer(natural);

        public static explicit operator Natural(Integer integer) => Abs(integer);

        public static explicit operator long(Integer integer) => integer.ToDecimal();

        public static implicit operator Integer(long u) => new Integer(u);

        public static implicit operator string(Integer n) => n.ToString();
    }
}
