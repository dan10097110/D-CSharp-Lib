using System;
using System.Collections.Generic;
using System.Linq;

namespace DLib
{
    public static class Combinatorics
    {
        public static T[][] Combine<T>(T[] array) => Combine(array, i => true, i => i, false);

        public static T[][] Combine<T>(T[] array, Func<int[], bool> CondForComb, Func<int[], int[]> ActionOnComb, bool removeDoubles)
        {
            var combs = new List<int[]>();
            for (var c = new int[array.Length]; ;)
            {
                if (CondForComb(c))
                {
                    var d = ActionOnComb(c);
                    if (!removeDoubles || !Contains(d))
                        combs.Add(d.ToArray());
                }
                c[0]++;
                for (int i = 0; c[i] >= array.Length; c[i]++)
                {
                    i++;
                    if (i >= array.Length)
                        return combs.Select(intToT).ToArray();
                    for (int j = 0; j < i; c[j] = 0, j++) ;
                }
            }

            T[] intToT(int[] i)
            {
                var t = new T[array.Length];
                for (int j = 0; j < t.Length; t[j] = array[i[j]], j++) ;
                return t;
            }

            bool Contains(int[] i)
            {
                for (int j = 0; j < combs.Count; j++)
                    if (Enumerable.SequenceEqual(i, combs[j]))
                        return true;
                return false;
            }
        }

        public static T[][] CombineEveryLength<T>(T[] array)
        {
            if (array.Length > 1)
            {
                var combs = new List<T[]>();
                var a = array.ToList();
                for (int i = 0; i < array.Length; i++)
                {
                    a.RemoveAt(i);
                    foreach(T[] c in CombineEveryLength(a.ToArray()))
                    {
                        if(!Contains())
                            combs.Add(c.ToArray());

                        bool Contains()
                        {
                            for (int j = 0; j < combs.Count; j++)
                                if (Enumerable.SequenceEqual(c, combs[j]))
                                    return true;
                            return false;
                        }
                    }
                    a = array.ToList();
                }
                combs.AddRange(Combine(array, NoDoubles, Sort, true));
                return combs.ToArray();
            }
            else if (array.Length == 1)
                return new T[][] { new T[] { array[0] } };
            else
                return new T[0][];
        }

        public static bool NoDoubles(int[] comb)
        {
            var c = new int[comb.Length];
            for (int i = 0; i < c.Length; c[comb[i]]++, i++)
                if (c[comb[i]] == 1)
                    return false;
            return true;
        }

        public static int[] Sort(int[] comb)
        {
            var l = comb.ToList();
            l.Sort();
            return l.ToArray();
        }
    }
}
