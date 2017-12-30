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

        public override string ToString() => "(" + a.ToString() + "+" + b.ToString() + ")";

        public override Function Clone() => new Sum(a.Clone(), b.Clone());
    }
}
