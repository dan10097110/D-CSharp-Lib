using System;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Math.Function
{
    public class Polynomial : Function
    {
        public double[] factorss;

        public int Degree => factorss.Length - 1;

        public Polynomial() { }

        public Polynomial(params (double x, double y)[] coords)
        {
            var matrix = new Matrix(coords.Length, coords.Length);
            for (int i = 0; i < matrix.Width; i++)
                for (int j = 0; j < matrix.Height; matrix[j, i] = System.Math.Pow(coords[i].x, matrix.Height - 1 - j), j++) ;
            var matrix1 = new Matrix(1, coords.Length);
            for (int i = 0; i < matrix1.Height; i++)
                matrix1[0, i] = coords[i].y;
            var vector = (matrix.Inverse() * matrix1).GetColumn(0);
            factorss = new double[vector.Length];
            for (int i = 0; i < factorss.Length; i++)
                factorss[i] = vector[factorss.Length - 1 - i];
        }

        public Polynomial(params double[] factors)
        {
            int degree = factors.Length - 1;
            for (; degree >= 0 && factors[degree] == 0; degree--) ;
            factorss = new double[degree + 1];
            for (int i = 0; i < factorss.Length; i++)
                factorss[i] = factors[i];
        }

        public Polynomial(params Power[] powers)
        {
            factorss = new double[powers.Select(n => (int)n.Exponent).Max() + 1];
            foreach (var power in powers)
                factorss[(int)power.Exponent] += power.Factor;
        }

        public Polynomial(Polynomial polynomial) => factorss = (double[])polynomial.factorss.Clone();

        public static bool operator ==(Polynomial a, Polynomial b) => a.Equals(b);

        public static bool operator !=(Polynomial a, Polynomial b) => !a.Equals(b);

        public override bool Equals(object obj) => throw new NotImplementedException();

        public static Polynomial operator +(Polynomial a, Polynomial b)
        {
            var powers = new double[System.Math.Max(a.factorss.Length, b.factorss.Length)];
            for (int i = 0; i < powers.Length; powers[i] = (i < a.factorss.Length ? a.factorss[i] : 0) + (i < b.factorss.Length ? b.factorss[i] : 0), i++) ;
            return new Polynomial() { factorss = powers };
        }

        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            int length = System.Math.Max(a.factorss.Length, b.factorss.Length);
            if (a.factorss.Length == b.factorss.Length)
                for (; a.factorss[length - 1] == b.factorss[length - 1]; length--) ;
            var powers = new double[length];
            for (int i = 0; i < powers.Length; powers[i] = (i < a.factorss.Length ? a.factorss[i] : 0) - (i < b.factorss.Length ? b.factorss[i] : 0), i++) ;
            return new Polynomial() { factorss = powers };
        }

        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            var v = b * new Power(a.factorss[0], 0);
            for (int i = 1; i < a.factorss.Length; v += (new Power(a.factorss[i], i) * b), i++) ;
            return v;
        }

        //evt noch rest zurückgeben
        public static Polynomial operator /(Polynomial a, Polynomial b)
        {
            var p = (Polynomial)a.Clone();
            var powers = new double[a.Degree - b.Degree + 1];
            for (int i = 0; i < powers.Length; powers[powers.Length - 1 - i] = p.factorss[p.Degree] / b.factorss[b.Degree], p -= (b * powers[powers.Length - 1 - i]), i++) ;
            return new Polynomial() { factorss = powers };
        }

        //b kann nur positiv sein
        public static Polynomial operator *(Polynomial a, Power b)
        {
            var powers = new double[a.factorss.Length + (int)b.Exponent];
            for (int i = 0; i < a.factorss.Length; powers[i + (int)b.Exponent] = a.factorss[i] * b.Factor, i++) ;
            return new Polynomial() { factorss = powers };
        }

        public static Polynomial operator *(Power a, Polynomial b) => b * a;

        public static Polynomial operator *(Polynomial a, double b)
        {
            var powers = new double[a.factorss.Length];
            for (int i = 0; i < powers.Length; powers[i] = a.factorss[i] * b, i++) ;
            return new Polynomial() { factorss = powers };
        }

        public static Polynomial operator *(double a, Polynomial b) => b * a;

        public static Polynomial operator /(Polynomial a, double b) => a * (1 / b);

        public double[] Intersection(Polynomial polynomial) => (this - polynomial).Roots();

        public override double[] Roots()
        {
            var roots = new List<double>();
            Polynomial p = (Polynomial)Clone();
            while (p.Degree > 0)
            {
                var root = NonlinearEquations.NewtonMethod(p, 0);
                if (root == null)
                    break;
                roots.Add((double)root);
                p /= new Polynomial(-(double)root, 1);
            }
            roots.Sort();
            return roots.ToArray();
        }

        public override double Y(double x)
        {
            double y = 0;
            for (int i = 0; i < factorss.Length; i++)
                y += (factorss[i] * System.Math.Pow(x, i));
            return y;
        }

        public override Function Derivate()
        {
            if (Degree == -1)
                return new Polynomial(0);
            else
            {
                var powers = new double[this.factorss.Length - 1];
                for (int i = 0; i < powers.Length; powers[i] = this.factorss[i + 1] * (i + 1), i++) ;
                return new Polynomial() { factorss = powers };
            }
        }

        public override Function Integrate()
        {
            var powers = new double[this.factorss.Length + 1];
            for (int i = 1; i < powers.Length; powers[i] = this.factorss[i - 1] / i, i++) ;
            return new Polynomial() { factorss = powers };
        }

        public override Function Clone() => new Polynomial(this);

        public override string ToString()
        {
            string[] s = new string[factorss.Length];
            for (int i = 0; i < s.Length; i++)
                s[i] = factorss[i] + "*x^" + i;
            return "(" + string.Join("+", s) + ")";
        }

        public static implicit operator string(Polynomial p) => p.ToString();

        public override Function Inverse() => throw new System.NotImplementedException();

        public override double Limit(double p)
        {
            throw new NotImplementedException();
        }
    }
}