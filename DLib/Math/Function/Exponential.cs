using System;

namespace DLib.Math.Function
{
    public class Exponential : Function
    {
        public double Factor { get; private set; }
        public double Base { get; private set; }

        public Exponential(double factor, double b)
        {
            Factor = factor;
            Base = b;
        }

        public static bool operator ==(Exponential a, Exponential b) => a.Equals(b);

        public static bool operator !=(Exponential a, Exponential b) => !a.Equals(b);

        public override bool Equals(object obj) => Base == ((Exponential)obj).Base && Factor == ((Exponential)obj).Factor;

        public static Exponential operator +(Exponential a, Exponential b) => a.Base == b.Base ? new Exponential(a.Factor + b.Factor, a.Base) : throw new ArgumentException("Bases do not fit");

        public static Exponential operator -(Exponential a, Exponential b) => a.Base == b.Base ? new Exponential(a.Factor - b.Factor, a.Base) : throw new ArgumentException("Bases do not fit");

        public static Exponential operator *(Exponential a, Exponential b) => new Exponential(a.Factor * b.Factor, a.Base * b.Base);

        public static Exponential operator /(Exponential a, Exponential b) => new Exponential(a.Factor / b.Factor, a.Base / b.Base);

        public static Exponential operator *(Exponential a, double b) => new Exponential(a.Factor * b, a.Base);

        public static Exponential operator /(Exponential a, double b) => new Exponential(a.Factor * b, a.Base);

        public override Function Derivate() => new Exponential(Factor * System.Math.Log(Base), Base);

        public override Function Integrate() => new Exponential(Factor / System.Math.Log(Base), Base);

        public override double Y(double x) => Factor * System.Math.Pow(Base, x);

        public override double[] Roots() => new double[0];

        public override string ToString() => Factor + Base + "^x";

        public override Function Clone() => new Polynomial(Factor, Base);
    }
}
