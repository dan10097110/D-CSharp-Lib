using DLib.Math.Number;
using System;
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
            while(true)
            {
                cFrac.Add((ulong)a);
                if (a == (ulong)a)
                    break;
                a = 1 / (a - (ulong)a);
            }
        }

        public CFrac(Rational a)
        {
            while (true)
            {
                cFrac.Add(a.Round().Abs());
                if (a == a.Round())
                    break;
                a = a - a.Round().Abs();
                a.Reciprocal();
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
