using System;
using System.Collections.Generic;
using System.Linq;

namespace DLib
{
    public static class Extra
    {
        public static bool IsNumeric(string s) => double.TryParse(s, out var n);

        public static bool IsPerfectPower(int n)
        {
            int l = (int)System.Math.Log(n, 2);
            for (int b = 2; b <= l; b++)
                if (System.Math.Pow(n, 1 / (double)b) % 1 == 0)
                    return true;
            return false;
        }

        public static bool IsPowerOfTwo(int x) => (x != 0) && ((x & (x - 1)) == 0);

        public static ulong Factorial(ulong n)
        {
            ulong r = 1;
            for (ulong i = 2; i <= n; r *= i++) ;
            return r;
        }

        public static int Min(params int[] n)
        {
            int min = n[0];
            for (int i = 1; i < n.Length; i++)
                if (n[i] < min)
                    min = n[i];
            return min;
        }

        public static int Max(params int[] n)
        {
            int max = n[0];
            for (int i = 1; i < n.Length; i++)
                if (n[i] > max)
                    max = n[i];
            return max;
        }

        public static ulong GetDigitSum(ulong n)
        {
            ulong sum = 0;
            for (; n != 0; n /= 10) sum += n % 10;
            return sum;
        }

        public static ulong GetDigitSum2(ulong n) => (ulong)n.ToString().ToCharArray().Select(chr => (int)chr).Sum();

        public static bool IsSmooth(ulong n, ulong b)
        {
            if (b > n)
                return false;
            ulong[] primes = Math.Prime.Sieve.Standard(b).Cast<ulong>().ToArray();
            foreach (ulong prime in primes)
            {
                while (n % prime == 0)
                    n /= prime;
                if (n == 1)
                    return true;
            }
            return false;
        }

        public static bool IsSmooth(ulong n, ulong[] factorBase)
        {
            foreach (ulong prime in factorBase)
            {
                while (n % prime == 0)
                    n /= prime;
                if (n == 1)
                    return true;
            }
            return false;
        }

        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static bool[] ToBinaryBoolArray(long n) => Convert.ToString(n, 2).ToCharArray().Select(c => c == 49 ? true : false).ToArray();

        public static (long, long) TonelliShank(long p, long n)
        {
            if ((p & 3) == 3)
                return ((long)System.Math.Pow(n, (p + 1) >> 2) % p, (long)System.Math.Pow(n, (p + 1) >> 2) % p);
            long Q = p - 1, S = 0;
            for (; (Q & 1) == 0; Q >>= 1, S++) ;
            long z = 1;
            for (; Math.Symbol.Jacobi(z, p) != -1; z++) ;
            long R = (long)System.Math.Pow(n, (Q + 1) >> 1) % p;
            for (long M = S, c = (long)System.Math.Pow(z, Q) % p, t = (long)System.Math.Pow(n, Q) % p, b = 0, i = 1; t != 1; b = (long)System.Math.Pow(c * c, M - i - 1) % p, M = i, c = (b * b) % p, t = (t * b * b) % p, R = (R * b) % p, i = 1)
            {
                long j = (t * t) % p;
                for (; j > 1 && i < M; i++, j = (j * j) % p) ;
                if (i == M || j == 0)
                    return (0, 0);
            }
            return (R, p - R);
        }

        public static Stack<int> ToBinaryStack(int i)
        {
            var v = new Stack<int>();
            for (; i > 0; i >>= 1)
                v.Push(i & 1);
            return v;
        }

        public static int BinomialCoefficient(int n, int k)
        {
            if (k == 0)
                return 1;
            if (k << 1 > n)
                k = n - k;
            int c = 1;
            for (int i = 1; i <= k; c *= ((n - k + i) / i), i++) ;
            return c;
        }

        public static int BinomialCoefficientModM(int n, int k, int m)
        {
            if (k == 0)
                return 1;
            if (k << 1 > n)
                k = n - k;
            int c = 1;
            for (int i = 1; i <= k; c = (c * (n - k + i) / i) % m, i++) ;
            return c;
        }

        public static int DivisorSum(int n)
        {
            int t = 0;
            for (int d = 1; d <= n; d++)
                if (n % d == 0)
                    t += d;
            return t;
        }

        public static bool Sorted(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
                if (array[i - 1] > array[i])
                    return false;
            return true;
        }
    }
}
