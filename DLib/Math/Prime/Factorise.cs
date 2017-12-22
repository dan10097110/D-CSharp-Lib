using System;
using System.Collections.Generic;

namespace DLib.Math.Prime
{
    public static class Factorise
    {
        public static ulong[] Standard(ulong n, Func<ulong, ulong> GetFactor)
        {
            var factors = new List<ulong>();
            while (true)
            {
                ulong u = GetFactor(n);
                if (u == 1)
                {
                    factors.Add(n);
                    break;
                }
                factors.Add(u);
                n /= u;
            }
            factors.Sort();
            return factors.ToArray();
        }

        public static ulong[] Optimised(ulong n)
        {
            var factors = new List<ulong>();
            for (; (n & 1) == 0; n >>= 1)
                factors.Add(2);
            for (; n % 3 == 0; n /= 3)
                factors.Add(3);
            for (ulong u = 5; u * u <= n; u += 2)
            {
                for (; n % u == 0; n /= u)
                    factors.Add(u);
                u += 4;
                for (; n % u == 0; n /= u)
                    factors.Add(u);
            }
            factors.Sort();
            return factors.ToArray();
        }
    }
}
