using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLib.Math.Sequence
{
    public static class Perrin
    {
        public static ulong Recursive(ulong n) => n < 1 ? 3 : n == 1 ? 0 : n == 2 ? 2 : Recursive(n - 2) + Recursive(n - 3);

        public static ulong RecursiveModM(ulong n, ulong m) => n < 1 ? 3 : n == 1 ? 0 : n == 2 ? 2 : ((Recursive(n - 2) + Recursive(n - 3)) % m);

        public static ulong Iterative(ulong n)
        {
            ulong a = 3, b = 0, c = 2;
            for (ulong i = 0; i < n; i++)
            {
                ulong d = a + b;
                a = b;
                b = c;
                c = d;
            }
            return a;
        }

        public static ulong IterativeModM(ulong n, ulong m)
        {
            ulong a = 3 % m, b = 0, c = 2 % m;
            for (ulong i = 0; i < n; i++)
            {
                ulong d = (a + b) % m;
                a = b;
                b = c;
                c = d;
            }
            return a;
        }
    }
}
