namespace DLib.Math
{
    public class Rational
    {
        Integer dividend;
        Natural divisor;

        public Rational() { }

        public Rational(double d)
        {
            dividend = (long)(d * 1000000);
            divisor = 1000000;
            Shorten();
        }

        public Rational(Integer integer)
        {
            dividend = integer.Clone();
            divisor = Natural.One;
        }

        public Rational(Rational rational)
        {
            dividend = rational.dividend.Clone();
            divisor = rational.divisor.Clone();
        }

        void Shorten()
        {
            Natural gcd = Natural.GCD(divisor, (Natural)dividend);
            dividend /= gcd;
            divisor /= gcd;
        }

        public static Rational operator *(Rational a, Rational b)
        {
            Rational r = new Rational() { dividend = a.dividend * b.dividend, divisor = a.divisor * b.divisor };
            r.Shorten();
            return r;
        }

        public static Rational operator /(Rational a, Rational b)
        {
            Rational r = new Rational() { dividend = a.dividend * b.divisor * a.dividend.Sign(), divisor = a.dividend.Abs() * b.divisor };
            r.Shorten();
            return r;
        }

        public static Rational operator *(Rational a, Integer b)
        {
            Rational r = new Rational() { dividend = a.dividend * b, divisor = a.divisor };
            r.Shorten();
            return r;
        }

        public static Rational operator /(Rational a, Integer b)
        {
            Rational r = new Rational() { dividend = a.dividend * b.Sign(), divisor = a.divisor * b.Abs() };
            r.Shorten();
            return r;
        }

        public static Rational operator +(Rational a, Integer b)
        {
            Rational r = new Rational() { dividend = a.dividend + b * a.divisor, divisor = a.divisor };
            r.Shorten();
            return r;
        }

        public static Rational operator -(Rational a, Integer b)
        {
            Rational r = new Rational() { dividend = a.dividend - b * a.divisor, divisor = a.divisor };
            r.Shorten();
            return r;
        }

        public static Rational operator +(Rational a, Rational b)
        {
            Rational r = new Rational() { dividend = a.dividend * b.divisor + b.dividend * a.divisor, divisor = a.divisor * b.divisor };
            r.Shorten();
            return r;
        }

        public static Rational operator -(Rational a, Rational b)
        {
            Rational r = new Rational() { dividend = a.dividend * b.divisor - b.dividend * a.divisor, divisor = a.divisor * b.divisor };
            r.Shorten();
            return r;
        }


        public Rational Inverse() => new Rational() { dividend = (Integer)divisor * dividend.Sign(), divisor = dividend.Abs() };

        public Integer Round() => dividend / divisor;


        public override string ToString() => ToDecimal().ToString();

        public double ToDecimal() => dividend.ToDecimal() / (double)divisor.ToDecimal();

        public Rational Clone() => new Rational(this);


        public static implicit operator Rational(Integer integer) => new Rational(integer);

        public static explicit operator Integer(Rational rational) => rational.Round();

        public static implicit operator Rational(double u) => new Rational(u);

        public static implicit operator string(Rational n) => n.ToString();
    }
}
