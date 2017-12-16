using System.Collections.Generic;
using DLib.Math.Number;

namespace DLib.Math
{
    public class CFrac
    {
        List<Natural> cFrac;

        public int Length => cFrac.Count;

        public Natural this[int i] => cFrac[i];

        public CFrac() => cFrac = new List<Natural>();

        public CFrac(params Natural[] cFrac)
        {
            this.cFrac = new List<Natural>();
            for (int i = 0; i < cFrac.Length; i++)
                this.cFrac.Add(cFrac[i].Clone());
        }

        public Rational ToFrac()
        {
            Rational rational = 0;
            for (int i = cFrac.Count - 1; i >= 0; i--, rational = (rational + cFrac[i]).Reciprocal()) ;
            return rational.Reciprocal();
        }

        public void Add(Natural item) => cFrac.Add(item.Clone());
    }
}
