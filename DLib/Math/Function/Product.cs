using System;

namespace DLib.Math.Function
{
    class Product : Function
    {
        Function a, b;

        public Product(Function a, Function b)
        {
            this.a = a;
            this.b = b;
        }

        public override double Y(double x)
        {
            double n = a.Y(x);
            if (n == 0)
                return 0;
            return b.Y(x) * n;
        }

        public override Function Derivate() => new Sum(new Product(a.Derivate(), b), new Product(a, b.Derivate()));

        public override Function Integrate() => throw new NotImplementedException();// => new Difference(new Product(a.Integrate(), b), new Product(a.Integrate(), b.Derivate()).Integrate());

        public override string ToString() => "(" + a.ToString() + "*" + b.ToString() + ")";

        public override Function Clone() => new Product(a.Clone(), b.Clone());
    }
}
