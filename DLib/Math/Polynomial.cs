using System.Linq;
using System.Text;

namespace DLib.Math
{
    public class Polynomial
    {
        public double[] coef;

        public int Degree { get; private set; }

        public Polynomial()
        {

        }

        public Polynomial(params Coord[] coords)
        {
            Degree = coords.Length - 1;
            var m = new Matrix(coords.Length, coords.Length);
            for (int i = 0; i < m.Width; i++)
                for (int j = 0; j < m.Height; m[j, i] = System.Math.Pow(coords[i].X, m.Height - 1 - j), j++) ;
            var v = new Matrix(1, coords.Length);
            for (int i = 0; i < v.Height; i++)
                v[0, i] = coords[i].Y;
            var w = m.Inverse() * v;
            coef = new double[w.Height];
            for (int i = 0; i < coef.Length; i++)
                coef[i] = w[0, coef.Length - 1 - i];
        }

        public Polynomial(params double[] coef)
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
            int newDegree = coef.Length + i;
            var newCoef = new double[newDegree];
            for (int j = System.Math.Max(0, i); j < newDegree; newCoef[j] = coef[j - i] * (j - i), j++) ;
            return new Polynomial() { coef = newCoef, Degree = newDegree };
        }

        public double Intersection(Polynomial polynomial)
        {
            var p = new Polynomial(coef);
            return 1;
        }

        /*public double Root()
        {
            for(; ; )
        }*/

        public static Polynomial operator +(Polynomial a, Polynomial b) => new Polynomial() { coef = a.coef.Zip(b.coef, (x, y) => x + y).ToArray() };

        public static Polynomial operator -(Polynomial a, Polynomial b) => new Polynomial() { coef = a.coef.Zip(b.coef, (x, y) => x - y).ToArray() };

        public Polynomial Clone() => new Polynomial(this);

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = coef.Length - 1; i >= 0; i--)
            {
                sb.Append(coef[i] + "x^" + i);
                if (i > 0)
                    sb.Append("+");
            }
            return sb.ToString();
        }

        public double GetY(double x)
        {
            double y = 0;
            for (int i = 0; i < coef.Length; y += (coef[i] * System.Math.Pow(x, i)), i++) ;
            return y;
        }

        public static implicit operator string(Polynomial p) => p.ToString();

        public static double Sekantenverfahren(Polynomial p, double a, double b)
        {
            while (p.GetY(b) != 0)
            {
                double c = b - (b - a) * p.GetY(b) / (p.GetY(b) - p.GetY(a));
                a = b;
                b = c;
            }
            return b;
        }
        
        public static double NewtonVerfahren(Polynomial p, double a)
        {
            Polynomial q = p.GetIntegral(-1);
            while (p.GetY(a) != 0)
                a -= p.GetY(a) / q.GetY(a);
            return a;
        }

        public static double Bisektion(Polynomial p, double a, double b)
        {
            while (true)
            {
                double c = (a + b) / 2, y = p.GetY(c);
                if (y == 0)
                    return c;
                else if (y > 0)
                    b = c;
                else
                    a = c;
            }
        }

        public static double HalleyVerfahren(Polynomial p, double a)
        {
            Polynomial q = p.GetIntegral(-1), r = q.GetIntegral(-1);
            while (p.GetY(a) != 0)
                a -= (2 * p.GetY(a) * q.GetY(a)) / (2 * q.GetY(a) * q.GetY(a) - p.GetY(a) * r.GetY(a));
            return a;
        }
    }
}
