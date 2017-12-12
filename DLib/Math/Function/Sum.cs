using System;
using System.Collections.Generic;

namespace DLib.Math.Function
{
    public class Sum : Function
    {
        Function a, b;

        public Sum(Function a, Function b)
        {
            this.a = a;
            this.b = b;
        }

        public override double Y(double x) => a.Y(x) + b.Y(x);

        public override Function Derivate() => new Sum(a.Derivate(), b.Derivate());

        public override Function Integrate() => new Sum(a.Integrate(), b.Integrate());
        
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
                p = new Quotient(p.Clone(), new Difference(new Power(1, 1), new Power((double)root, 0)));
                Console.WriteLine(root);
                Console.WriteLine(p);
            }
            roots.Sort();
            return roots.ToArray();
        }

        public override string ToString() => "(" + a.ToString() + "+" + b.ToString() + ")";

        public override Function Clone() => new Sum(a.Clone(), b.Clone());
    }
}
