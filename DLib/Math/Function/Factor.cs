namespace DLib.Math.Function
{
    public class Factor : Function
    {
        Function a;
        double f;

        public Factor(double f, Function a)
        {
            this.a = a;
            this.f = f;
        }

        public override Function Clone() => new Factor(f, a.Clone());

        public override Function Derivate() => new Factor(f, a.Derivate());

        public override Function Integrate() => new Factor(f, a.Integrate());

        public override double[] Roots() => a.Roots();

        public override string ToString() => "(" + f + "*" + a.ToString() + ")";

        public override double Y(double x) => a.Y(x) * f;
    }
}
