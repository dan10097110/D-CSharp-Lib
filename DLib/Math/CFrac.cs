using DLib.Math.Number;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Math
{
    public class CFrac
    {
        List<int> nonP = new List<int>(), p = new List<int>();

        public int Length => p.Count > 0 ? int.MaxValue : nonP.Count;

        public int this[int i] => i < nonP.Count ? nonP[i] : p[(i - nonP.Count) % p.Count];

        public int Last() => nonP.Last();

        public CFrac(params int[] cFrac)
        {
            for (int i = 0; i < cFrac.Length; i++)
                this.nonP.Add(cFrac[i]);
        }

        public CFrac(double a)
        {
            for (; ; a = 1 / (a - (int)a))
            {
                nonP.Add((int)a);
                if (a == (int)a)
                    break;
            }
        }

        public CFrac(Rational a)
        {
            for (; ; a = a.FractionalPart().Reciprocal())
            {
                nonP.Add((int)(ulong)a.Round().Abs());
                if (a.IsInteger())
                    break;
            }
        }

        public Rational ToIthConvergent(int i)
        {
            Rational rational = 0;
            for (; i >= 0; rational = (rational + this[i]).Reciprocal(), i--) ;
            return rational.Reciprocal();
        }

        public (int numerator, int denominator) ToIthConvergentFrac(int i)
        {
            int numerator = 0, denominator = 1;
            for (; i >= 0; i--)
            {
                int tmp = (numerator + this[i] * denominator);
                numerator = denominator;
                denominator = tmp;
            }
            return (denominator, numerator);
        }

        public void Add(int item) => nonP.Add(item);

        public static CFrac FromSqrt(int s)
        {
            int m = 0, d = 1, a0 = (int)System.Math.Sqrt(s), a = a0;
            List<(int, int, int)> nonP = new List<(int, int, int)>(), p = new List<(int, int, int)>();
            nonP.Add((m, d, a));
            for (; ; )
            {
                m = d * a - m;
                d = (s - m * m) / d;
                if (d == 0)
                    break;
                a = (a0 + m) / d;
                int i = nonP.IndexOf((m, d, a));
                if (i != -1)
                {
                    for (int j = i; j < nonP.Count; j++)
                        p.Add(nonP[j]);
                    nonP.RemoveRange(i, nonP.Count - i);
                    break;
                }
                nonP.Add((m, d, a));
            }
            return new CFrac() { nonP = nonP.Select(z => z.Item3).ToList(), p = p.Select(z => z.Item3).ToList() };
        }
    }
}
