using System;
using System.Diagnostics;

namespace DLib
{
    public static class Benchmark
    {
        public static long TimeIt<TResult>(Func<TResult> func, ulong accuracy)
        {
            func();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (ulong i = 0; i < accuracy; i++)
                func();
            return sw.ElapsedMilliseconds / (long)accuracy;
        }

        public static long TimeIt<T, TResult>(Func<T, TResult> func, T p, ulong accuracy)
        {
            func(p);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (ulong i = 0; i < accuracy; i++)
                func(p);
            return sw.ElapsedMilliseconds / (long)accuracy;
        }

        public static long TimeIt<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 p1, T2 p2, ulong accuracy)
        {
            func(p1, p2);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (ulong i = 0; i < accuracy; i++)
                func(p1, p2);
            return sw.ElapsedMilliseconds / (long)accuracy;
        }

        public static long TimeIt<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 p1, T2 p2, T3 p3, ulong accuracy)
        {
            func(p1, p2, p3);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (ulong i = 0; i < accuracy; i++)
                func(p1, p2, p3);
            return sw.ElapsedMilliseconds / (long)accuracy;
        }

        public static long TimeIt<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, T1 p1, T2 p2, T3 p3, T4 p4, ulong accuracy)
        {
            func(p1, p2, p3, p4);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (ulong i = 0; i < accuracy; i++)
                func(p1, p2, p3, p4);
            return sw.ElapsedMilliseconds / (long)accuracy;
        }
    }
}
