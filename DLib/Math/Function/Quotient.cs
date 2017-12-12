using System;
using System.Collections.Generic;

namespace DLib.Math.Function
{
    public class Quotient : Function
    {
        Function a, b;

        public Quotient(Function a, Function b)
        {
            this.a = a;
            this.b = b;
        }

        public override double Y(double x)
        {
            double n = a.Y(x), m = b.Y(x);
            if (n == 0 && m == 0)
                return new Quotient(a.Derivate(), b.Derivate()).Y(x);
            else
                return n / m;
        }

        public override Function Derivate() => new Quotient(new Sum(new Product(a.Derivate(), b), new Product(a, b.Derivate())), new Product(b, b));

        public override Function Integrate() => throw new NotImplementedException();

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

        public override string ToString() => "(" + a.ToString() + "/" + b.ToString() + ")";

        public override Function Clone() => new Quotient(a.Clone(), b.Clone());
    }
}
