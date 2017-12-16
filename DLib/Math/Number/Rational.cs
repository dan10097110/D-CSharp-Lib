namespace DLib.Math.Number
{
    public class Rational : INumber
    {
        public Integer Numerator { get; private set; }
        public Natural Denominator { get; private set; }

        public Rational() { }

        public Rational(double d)
        {
            Numerator = (long)(d * 1000000);
            Denominator = 1000000;
            Shorten();
        }

        public Rational(Integer integer)
        {
            Numerator = integer.Clone();
            Denominator = Natural.One;
        }

        public Rational(Rational rational)
        {
            Numerator = rational.Numerator.Clone();
            Denominator = rational.Denominator.Clone();
        }

        void Shorten()
        {
            Natural gcd = Natural.GCD(Denominator, (Natural)Numerator);
            Numerator /= gcd;
            Denominator /= gcd;
        }

        public static Rational operator *(Rational a, Rational b)
        {
            Rational r = new Rational() { Numerator = a.Numerator * b.Numerator, Denominator = a.Denominator * b.Denominator };
            r.Shorten();
            return r;
        }

        public static Rational operator /(Rational a, Rational b)
        {
            Rational r = new Rational() { Numerator = a.Numerator * b.Denominator * a.Numerator.Sign(), Denominator = a.Numerator.Abs() * b.Denominator };
            r.Shorten();
            return r;
        }

        public static Rational operator *(Rational a, Integer b)
        {
            Rational r = new Rational() { Numerator = a.Numerator * b, Denominator = a.Denominator };
            r.Shorten();
            return r;
        }

        public static Rational operator /(Rational a, Integer b)
        {
            Rational r = new Rational() { Numerator = a.Numerator * b.Sign(), Denominator = a.Denominator * b.Abs() };
            r.Shorten();
            return r;
        }

        public static Rational operator +(Rational a, Integer b)
        {
            Rational r = new Rational() { Numerator = a.Numerator + b * a.Denominator, Denominator = a.Denominator };
            r.Shorten();
            return r;
        }

        public static Rational operator -(Rational a, Integer b)
        {
            Rational r = new Rational() { Numerator = a.Numerator - b * a.Denominator, Denominator = a.Denominator };
            r.Shorten();
            return r;
        }

        public static Rational operator +(Rational a, Rational b)
        {
            Rational r = new Rational() { Numerator = a.Numerator * b.Denominator + b.Numerator * a.Denominator, Denominator = a.Denominator * b.Denominator };
            r.Shorten();
            return r;
        }

        public static Rational operator -(Rational a, Rational b)
        {
            Rational r = new Rational() { Numerator = a.Numerator * b.Denominator - b.Numerator * a.Denominator, Denominator = a.Denominator * b.Denominator };
            r.Shorten();
            return r;
        }


        public static bool operator ==(Rational a, Rational b) => Compare(a, b) == 0;

        public static bool operator !=(Rational a, Rational b) => Compare(a, b) != 0;

        public static bool operator <(Rational a, Rational b) => Compare(a, b) == -1;

        public static bool operator >(Rational a, Rational b) => Compare(a, b) == 1;

        public static bool operator <=(Rational a, Rational b) => Compare(a, b) != 1;

        public static bool operator >=(Rational a, Rational b) => Compare(a, b) != -1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>a < b: - 1; a == b: 0; a > b: == 1</b></returns>
        public static int Compare(Rational a, Rational b)
        {
            Natural lcm = Natural.LCM(a.Denominator, b.Denominator);
            return Integer.Compare(a.Numerator * lcm / a.Denominator, b.Numerator * lcm / a.Denominator);
        }


        public Rational Reciprocal() => new Rational() { Numerator = (Integer)Denominator * Numerator.Sign(), Denominator = Numerator.Abs() };

        public Integer Round() => Numerator / Denominator;

        public bool IsZero() => Numerator == 0;

        public bool IsPrime() => Numerator.Positive && Denominator == 1;


        public override string ToString() => ToDecimal().ToString();

        public double ToDecimal() => Numerator.ToDecimal() / (double)Denominator.ToDecimal();

        public Rational Clone() => new Rational(this);


        public static implicit operator Rational(Integer integer) => new Rational(integer);

        public static explicit operator Integer(Rational rational) => rational.Round();

        public static implicit operator Rational(double u) => new Rational(u);

        public static implicit operator string(Rational n) => n.ToString();
    }
}
