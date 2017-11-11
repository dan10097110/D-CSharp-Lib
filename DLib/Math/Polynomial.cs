using System;
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
            Degree = coords.Length;
            Matrix m = new Matrix(coords.Length, coords.Length);
            for (int i = 0; i < m.Width; i++)
                for (int j = 0; j < m.Height; m[j, i] = System.Math.Pow(coords[i].X, m.Height - 1 - j), j++) ;
            Matrix v = new Matrix(coords.Length, 1);
            for (int i = 0; i < v.Width; i++)
                v[i, 0] = coords[i].Y;
            Matrix w = m.Inverse() * v;
            coef = new double[w.Width];
            for (int i = 0; i < coef.Length; i++)
                coef[i] = w[i, 0];
        }

        //konstrukter für polynom
        public Polynomial(params int[] coef)
        {
            Degree = coef.Length;
            for (; coef[Degree - 1] == 0; Degree--) ;
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
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Degree; i++)
            {
                sb.Append(coef[i]);
                sb.Append("x^");
                sb.Append(i);
                if (i + 1 < Degree)
                    sb.Append("+");
            }
            return sb.ToString();
        }
    }
}
