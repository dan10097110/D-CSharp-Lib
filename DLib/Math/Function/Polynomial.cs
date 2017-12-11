using System.Collections.Generic;
using System.Linq;

namespace DLib.Math.Function
{
    public class Polynomial : Function
    {
        public Power[] powers;
        
        public int AppendsCount
        {
            get
            {
                int c = 0;
                for(int i = 0; i < powers.Length; i++)
                    if (powers[i].Factor != 0)
                        c++;
                return c;
            }
        }
        public int Degree => powers.Length - 1;

        public Polynomial() { }

        public Polynomial(params Coord[] coords)
        {
            var matrix = new Matrix(coords.Length, coords.Length);
            for (int i = 0; i < matrix.Width; i++)
                for (int j = 0; j < matrix.Height; matrix[j, i] = System.Math.Pow(coords[i].X, matrix.Height - 1 - j), j++) ;
            var matrix1 = new Matrix(1, coords.Length);
            for (int i = 0; i < matrix1.Height; i++)
                matrix1[0, i] = coords[i].Y;
            var vector = (matrix.Inverse() * matrix1).GetColumn(0);
            powers = new Power[vector.Length];
            for (int i = 0; i < powers.Length; i++)
                powers[i] = new Power(vector[powers.Length - 1 - i], i);
        }

        public Polynomial(params double[] factors)
        {
            int degree = factors.Length - 1;
            for (; factors[degree] == 0; degree--) ;
            powers = new Power[degree + 1];
            for (int i = 0; i < powers.Length; i++)
                powers[i] = new Power(factors[i], i);
        }

        //schließt mehrfache nicht aus
        public Polynomial(params Power[] powers) => this.powers = powers.ToList().OrderBy(p => p.Exponent).ToArray();

        public Polynomial(Polynomial polynomial) => powers = (Power[])polynomial.powers.Clone();

        public static bool operator ==(Polynomial a, Polynomial b) => a.Equals(b);

        public static bool operator !=(Polynomial a, Polynomial b) => !a.Equals(b);

        //nicht feritgf
        public override bool Equals(object obj)
        {
            return true;
        }

        public static Polynomial operator +(Polynomial a, Polynomial b)
        {
            var powers = new Power[System.Math.Max(a.powers.Length, b.powers.Length)];
            for (int i = 0; i < powers.Length; powers[i] = a.GetAddend(i) + b.GetAddend(i), i++) ;
            return new Polynomial() { powers = powers };
        }

        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            int length = System.Math.Max(a.powers.Length, b.powers.Length);
            if (a.powers.Length == b.powers.Length)
                for (; a.powers[length - 1] == b.powers[length - 1]; length--) ;
            var powers = new Power[length];
            for (int i = 0; i < powers.Length; powers[i] = a.GetAddend(i) - b.GetAddend(i), i++) ;
            return new Polynomial() { powers = powers };
        }

        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            var v = b * a.powers[0];
            for (int i = 1; i < a.powers.Length; v += (a.powers[i] * b), i++) ;
            return v;
        }

        //evt noch rest zurückgeben
        public static Polynomial operator /(Polynomial a, Polynomial b)
        {
            var p = a.Clone();
            var powers = new Power[a.Degree - b.Degree + 1];
            for (int i = 0; i < powers.Length; powers[powers.Length - 1 - i] = p.powers[p.Degree] / b.powers[b.Degree], p -= (b * powers[powers.Length - 1 - i]), i++) ;
            return new Polynomial() { powers = powers };
        }

        //b kann nur positiv sein
        public static Polynomial operator *(Polynomial a, Power b)
        {
            var powers = new Power[a.powers.Length + (int)b.Exponent];
            for (int i = 0; i < b.Exponent; i++)
                powers[i] = new Power(0, i);
            for (int i = 0; i < a.powers.Length; powers[i + (int)b.Exponent] = a.powers[i] * b, i++) ;
            return new Polynomial() { powers = powers };
        }

        public static Polynomial operator *(Power a, Polynomial b) => b * a;

        public static Polynomial operator *(Polynomial a, double b)
        {
            var powers = new Power[a.powers.Length];
            for (int i = 0; i < powers.Length; powers[i] = a.powers[i] * b, i++) ;
            return new Polynomial() { powers = powers };
        }

        public static Polynomial operator /(Polynomial a, double b) => a * (1 / b);

        public double[] Intersection(Polynomial polynomial) => (this - polynomial).Roots();

        public double[] Roots()
        {
            var roots = new List<double>();
            Polynomial p = Clone();
            while (p.Degree > 0)
            {
                double root = p.Root();
                roots.Add(root);
                p /= new Polynomial(-root, 1);
            }
            roots.Sort();
            return roots.ToArray();
        }

        public override double Root() => NonlinearEquations.NewtonMethod(this, 0);

        public override double GetY(double x)
        {
            double y = 0;
            foreach (var p in powers)
                y += p.GetY(x);
            return y;
        }

        public Power GetAddend(int i) => i > Degree ? new Power(0, i) : powers[i].Clone();

        public Polynomial GetDerivation()
        {
            var powers = new Power[this.powers.Length - 1];
            for (int i = 0; i < powers.Length; powers[i] = this.powers[i + 1].GetDerivation(), i++) ;
            return new Polynomial() { powers = powers };
        }

        public Polynomial GetIntegral()
        {
            var powers = new Power[this.powers.Length + 1];
            powers[0] = new Power(0, 0);
            for (int i = 1; i < powers.Length; powers[i] = this.powers[i - 1].GetIntegral(), i++) ;
            return new Polynomial() { powers = powers };
        }

        public Polynomial GetDerivation(int i)
        {
            if (i < 0)
                return GetIntegral(-i);
            var p = this;
            for (; i >= 0; i--)
                p = p.GetDerivation();
            return p;
        }

        public Polynomial GetIntegral(int i)
        {
            if (i < 0)
                return GetDerivation(-i);
            var p = this;
            for (; i >= 0; i--)
                p = p.GetIntegral();
            return p;
        }

        public Polynomial Clone() => new Polynomial(this);

        public override string ToString()
        {
            string[] s = new string[powers.Length];
            for (int i = 0; i < s.Length; i++)
                s[i] = powers[i];
            return string.Join("+", s);
        }

        public static implicit operator string(Polynomial p) => p.ToString();
    }
}