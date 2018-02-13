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

        public override string ToString() => "(" + a.ToString() + "-" + b.ToString() + ")";

        public override Function Clone() => new Difference(a.Clone(), b.Clone());

        public override Function Inverse() => throw new System.NotImplementedException();
    }
}
