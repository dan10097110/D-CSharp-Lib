using System;
using System.Collections.Generic;

namespace DLib.Math.Prime
{
    public static class Factorise
    {
        public static ulong[] Standard(ulong n, Func<ulong, ulong> GetFactor)
        {
            var factors = new List<ulong>();
            while(true)
            {
                ulong u = GetFactor(n);
                factors.Add(u);
                if (u == 1)
                    break;
                n /= u;
            }
            factors.Sort();
            return factors.ToArray();
        }
    }
}
