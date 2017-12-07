using System.Text;

namespace DLib.Math
{
    public class Polynomial
    {
        double[] coef;

        public int Degree { get; private set; }

        public Polynomial()
        {

        }

        public Polynomial(params Coord[] coords)
        {
            Degree = coords.Length - 1;
            var m = new Matrix(coords.Length, coords.Length);
            for (int i = 0; i < m.Width; i++)
                for (int j = 0; j < m.Height; m[i, j] = System.Math.Pow(coords[i].X, m.Height - 1 - j), j++) ;
            var v = new Vector(coords.Length);
            for (int i = 0; i < v.Length; i++)
                v[i] = coords[i].Y;
            var w = m.Inverse() * v;
            coef = new double[w.Height];
            for (int i = 0; i < coef.Length; i++)
                coef[i] = w[0, i];
        }

        public Polynomial(params int[] coef)
        {
            Degree = coef.Length - 1;
            for (; coef[Degree] == 0; Degree--) ;
            this.coef = new double[Degree];
            for (int i = 0; i < Degree; i++)
                this.coef[i] = coef[i];
        }

        public Polynomial(Polynomial polynom)
        {
            Degree = polynom.Degree;
            coef = (double[])polynom.coef.Clone();
        }

        public Polynomial GetIntegral(int i)
        {
            int newDegree = Degree + i;
            var newCoef = new double[newDegree];
            for (int j = System.Math.Max(0, i); i < newDegree; newCoef[j] = coef[j - i] * (j - i), i++) ;
            return new Polynomial() { coef = newCoef, Degree = newDegree };
        }

        public Polynomial Clone() => new Polynomial(this);

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < coef.Length; i++)
            {
                sb.Append(coef[i]);
                sb.Append("x^");
                sb.Append(i);
                if (i + 1 < coef.Length)
                    sb.Append("+");
            }
            return sb.ToString();
        }

        public double GetY(double x)
        {
            double y = 0;
            for (int i = 0; i < coef.Length; y += (coef[i] * System.Math.Pow(x, coef.Length - 1 - i)), i++) ;
            return y;
        }

        public static implicit operator string(Polynomial p) => p.ToString();
    }
}
