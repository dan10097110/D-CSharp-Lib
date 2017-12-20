using System;
using System.Collections.Generic;
using System.Linq;

namespace DLib
{
    public static class Combinatorics
    {
        public static T[][] Combine<T>(T[] array) => Combine(array, i => true);

        public static T[][] Combine<T>(T[] array, Func<int[], bool> CondForComb)
        {
            var combs = new List<T[]>();
            var c = new int[array.Length];
            for (int i = 0, p = 0; i < array.Length;)
            {
                if (CondForComb(c))
                {
                    var comb = new T[array.Length];
                    for (int j = 0; j < comb.Length; j++)
                        comb[j] = array[c[j]];
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

        public static T[][] Combine<T>(T[] array, Func<int[], bool> CondForComb, bool sortComb, bool removeDoubles)
        {
            var combs = new List<T[]>();
            var c = new int[array.Length];
            for (int i = 0, p = 0; i < array.Length;)
            {
                if (CondForComb(c))
                {
                    var comb = new T[array.Length];
                    for (int j = 0; j < comb.Length; j++)
                        comb[j] = array[c[j]];
                    var l = comb.ToList();
                    comb = l.ToArray();
                    if(sortComb)
                        l.Sort();
                    if(!removeDoubles || !combs.Contains(comb))
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

        public static bool NoDoubles(int[] comb)
        {
            var c = new int[comb.Length];
            for (int i = 0; i < c.Length; c[comb[i]]++, i++)
                if (c[comb[i]] > 1)
                    return false;
            return true;
        }
    }
}
