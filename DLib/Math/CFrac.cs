using DLib.Math.Number;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Math
{
    public class CFrac
    {
        List<Natural> cFrac = new List<Natural>();

        public int Length => cFrac.Count;

        public Natural this[int i] => cFrac[i];

        public Natural Last() => cFrac.Last();

        public CFrac(params Natural[] cFrac)
        {
            for (int i = 0; i < cFrac.Length; i++)
                this.cFrac.Add(cFrac[i]);
        }

        public CFrac(double a)
        {
            for (; ; a = 1 / (a - (ulong)a))
            {
                cFrac.Add((ulong)a);
                if (a == (ulong)a)
                    break;
            }
        }

        public CFrac(Rational a)
        {
            for (; ; a = a.FractionalPart().Reciprocal())
            {
                cFrac.Add(a.Round().Abs());
                if (a.IsInteger())
                    break;
            }
        }

        public Rational ToFrac()
        {
            Rational rational = 0;
            for (int i = cFrac.Count - 1; i >= 0; rational = (rational + cFrac[i]).Reciprocal(), i--) ;
            return rational.Reciprocal();
        }

        public void Add(Natural item) => cFrac.Add(item);
    }
}
