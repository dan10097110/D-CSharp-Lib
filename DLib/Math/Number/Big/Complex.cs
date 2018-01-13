namespace DLib.Math.Number.Big
{
    public class Complex : INumber
    {
        public Rational Real { get; private set; }
        public Rational Imaginary { get; private set; }

        public Complex() { }

        public Complex(Rational real, Rational imaginary)
        {
            Real = real.Clone();
            Imaginary = imaginary.Clone();
        }

        public static Complex operator +(Complex a, Complex b) => new Complex() { Real = a.Real + b.Real, Imaginary = a.Imaginary + b.Imaginary };

        public static Complex operator -(Complex a, Complex b) => new Complex() { Real = a.Real - b.Real, Imaginary = a.Imaginary - b.Imaginary };

        public static Complex operator *(Complex a, Complex b) => new Complex() { Real = a.Real * b.Real - a.Imaginary * b.Imaginary, Imaginary = a.Imaginary * b.Real + a.Real * b.Imaginary };

        public static Complex operator /(Complex a, Complex b) => new Complex() { Real = (a.Real * b.Real + a.Imaginary * b.Imaginary) / (b.Real * b.Real + b.Imaginary * b.Imaginary), Imaginary = (a.Imaginary * b.Real - a.Real * b.Imaginary) / (b.Real * b.Real + b.Imaginary * b.Imaginary) };


        public static bool operator ==(Complex a, Complex b) => Compare(a, b);

        public static bool operator !=(Complex a, Complex b) => Compare(a, b);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>a == b</b></returns>
        public static bool Compare(Complex a, Complex b) => a.Real == b.Real && a.Imaginary == b.Imaginary;


        public Complex Clone() => new Complex(Real, Imaginary);

        public override string ToString() => "(" + Real.ToString() + Imaginary.ToString() + "i)";
    }
}
