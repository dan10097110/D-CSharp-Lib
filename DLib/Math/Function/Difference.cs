using System.Collections.Generic;

namespace DLib.Math.Function
{
    public class Difference : Function
    {
        Function a, b;

        public Difference(Function a, Function b)
        {
            this.a = a;
            this.b = b;
        }

        public override double Y(double x) => a.Y(x) - b.Y(x);

        public override Function Derivate() => new Difference(a.Derivate(), b.Derivate());

        public override Function Integrate() => new Difference(a.Integrate(), b.Integrate());

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
            }
            roots.Sort();
            return roots.ToArray();
        }

        public override string ToString() => "(" + a.ToString() + "-" + b.ToString() + ")";

        public override Function Clone() => new Difference(a.Clone(), b.Clone());
    }
}
