using System;
using System.Diagnostics;

namespace DLib
{
    public static class Benchmark
    {
        static Stopwatch sw = new Stopwatch();

        public static long TimeIt(Action func, ulong accuracy)
        {
            sw.Restart();
            for (ulong i = 0; i <= accuracy; i++)
                func();
            return sw.ElapsedMilliseconds / ((long)accuracy + 1);
        }

        public static long TimeIt<TResult>(Func<TResult> func, ulong accuracy)
        {
            sw.Restart();
            for (ulong i = 0; i <= accuracy; i++)
                func();
            return sw.ElapsedMilliseconds / ((long)accuracy + 1);
        }

        public static long TimeIt<T, TResult>(Func<T, TResult> func, T p, ulong accuracy)
        {
            sw.Restart();
            for (ulong i = 0; i <= accuracy; i++)
                func(p);
            return sw.ElapsedMilliseconds / ((long)accuracy + 1);
        }

        public static long TimeIt<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 p1, T2 p2, ulong accuracy)
        {
            sw.Restart();
            for (ulong i = 0; i <= accuracy; i++)
                func(p1, p2);
            return sw.ElapsedMilliseconds / ((long)accuracy + 1);
        }

        public static long TimeIt<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 p1, T2 p2, T3 p3, ulong accuracy)
        {
            sw.Restart();
            for (ulong i = 0; i <= accuracy; i++)
                func(p1, p2, p3);
            return sw.ElapsedMilliseconds / ((long)accuracy + 1);
        }

        public static long TimeIt<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, T1 p1, T2 p2, T3 p3, T4 p4, ulong accuracy)
        {
            sw.Restart();
            for (ulong i = 0; i <= accuracy; i++)
                func(p1, p2, p3, p4);
            return sw.ElapsedMilliseconds / ((long)accuracy + 1);
        }
    }
}
