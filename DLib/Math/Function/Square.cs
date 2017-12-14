using System;

namespace Dlib.Math.Function
{
    public class Square : Function
    {
        Function a;

        public Square(Function a) => this.a = a;

        public override Function Clone() => new Square(a.Clone());

        public override Function Derivate() => new Factor(2, new Product(a, a.Derivate()));

        public override Function Integrate() => throw new NotImplementedException();

        public override double[] Roots() => a.Roots();

        public override string ToString() => "(" + a.ToString() + "^2" + ")";

        public override double Y(double x)
        {
            double d = a.Y(x);
            return d * d;
        }
    }
}
