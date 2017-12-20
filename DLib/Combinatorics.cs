using System;
using System.Collections.Generic;
using System.Linq;

namespace DLib
{
    public static class Combinatorics
    {
        public static T[][] Combine<T>(T[] array) => Combine(array, i => true, i => i, false);

        public static T[][] Combine<T>(T[] array, Func<int[], bool> CondForComb, Func<T[], T[]> ActionOnComb, bool removeDoubles)
        {
            var combs = new List<T[]>();
            var c = new int[array.Length];
            for (int i = 0; i < array.Length;)
            {
                if (CondForComb(c))
                {
                    var comb = new T[array.Length];
                    for (int j = 0; j < comb.Length; j++)
                        comb[j] = array[c[j]];
                    comb = ActionOnComb(comb);
                    if (!removeDoubles || !combs.Contains(comb))
                        combs.Add(comb);
                }
                c[i]++;
                if (c[i] > array.Length)
                {
                    for (int j = 0; j <= i; c[j] = 0, j++) ;
                    i++;
                }
                else if (i > 0)
                    i--;
            }
            return combs.ToArray();
        }

        public static T[][] CombineEveryLength<T>(T[] array)
        {
            var c = new List<T[]>();
            if (array.Length > 2)
            {
                var a = array.ToList();
                for (int i = 0; i < array.Length; i++)
                {
                    a.RemoveAt(i);
                    c.AddRange(CombineEveryLength(a.ToArray()));
                    a = array.ToList();
                }
                c.AddRange(Combine(array, NoDoubles, Sort, true));
            }
            else if (array.Length == 1)
                c.Add(new T[] { array[0] });
            return c.ToArray();
        }

        public static bool NoDoubles(int[] comb)
        {
            var c = new int[comb.Length];
            for (int i = 0; i < c.Length; c[comb[i]]++, i++)
                if (c[comb[i]] == 1)
                    return false;
            return true;
        }

        public static T[] Sort<T>(T[] comb)
        {
            var l = comb.ToList();
            l.Sort();
            return l.ToArray();
        }
    }
}
