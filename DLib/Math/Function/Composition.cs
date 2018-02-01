using System;

namespace DLib.Math.Function
{
    public class Composition : Function
    {
        Function a, b;

        public Composition(Function a, Function b)
        {
            this.a = a;
            this.b = b;
        }

        public override double Y(double x) => a.Y(b.Y(x));

        public override Function Derivate() => new Product(new Composition(a.Derivate(), b), b.Derivate());

        public override Function Integrate() => throw new NotImplementedException();

        public override string ToString() => "(" + a.ToString() + "Verkettung" + b.ToString() + ")";

        public override Function Clone() => new Composition(a.Clone(), b.Clone());
    }
}