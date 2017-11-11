using System;
using System.Collections.Generic;
using System.Linq;

namespace DLib
{
    public static class Extra
    {
        public static bool IsNumeric(string s) => double.TryParse(s, out var n);

        public static ulong Factorial(ulong n)
        {
            ulong result = 1;
            for (ulong i = 2; i <= n; result *= i++) ;
            return result;
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

        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static IEnumerable<T> SplitString<T>(string s, char splitChar, Func<string, T> parse)
        {
            for (int i; (i = s.IndexOf(splitChar)) != -1; s = s.Remove(0, i + 1))
                yield return parse(s.Substring(0, i));
            yield return parse(s);
        }

        public static ulong GetDigitSum(ulong n)
        {
            ulong sum = 0;
            for (; n != 0; n /= 10) sum += n % 10;
            return sum;
        }

        public static ulong GetDigitSum2(ulong n) => (ulong)n.ToString().ToCharArray().Select(chr => (int)chr).Sum();

        public static bool IsPerfectPower(ulong n)
        {
            for (uint b = 2; b <= System.Math.Log(n, 2); b++)
                if (System.Math.Pow(n, 1 / (double)b) % 1 == 0)
                    return true;
            return false;
        }

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
    }
}
