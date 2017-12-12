using System;
using System.Collections.Generic;

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

        public override double Y(double x) => a.Y(x) == 0 || b.Y(x) == 0 ? 0 : a.Y(x) * b.Y(x);

        public override Function Derivate() => new Sum(new Product(a.Derivate(), b), new Product(a, b.Derivate()));

        public override Function Integrate() => throw new NotImplementedException();// => new Difference(new Product(a.Integrate(), b), new Product(a.Integrate(), b.Derivate()).Integrate());

        public override double[] Roots()
        {
            var roots = new List<double>();
            Function p = Clone();
            while (true)
            {
                var root = NonlinearEquations.NewtonMethod(p, 0);
                if (root == null)
                    break;
                roots.Add((double)root);
                p = new Quotient(p.Clone(), new Polynomial(-(double)root, 1));
            }
            roots.Sort();
            return roots.ToArray();
        }

        public override string ToString() => "(" + a.ToString() + "*" + b.ToString() + ")";

        public override Function Clone() => new Product(a.Clone(), b.Clone());
    }
}
