using System;

namespace DLib.Math.Prime
{
    public static class Problably
    {
        public static bool Lucas(ulong n)
        {
            if (n == 2)
                return true;
            if (n < 2 || (n & 1) == 0 || System.Math.Sqrt(n) % 1 == 0)
                return false;
            return Sequence.Lucas.NthModM((long)(n + 1), (long)n).U == 0;
        }

        public static bool StrongLucas(ulong n)
        {
            if (n == 2)
                return true;
            if (n < 2 || (n & 1) == 0 || System.Math.Sqrt(n) % 1 == 0)
                return false;
            ulong d = (n + 1) >> 1, s = 1;
            for (; (d & 1) == 0; d >>= 1, s++) ;
            var selfridge = Sequence.Lucas.SelfridgeMethode((long)n);
            var lucas = Sequence.Lucas.NthModM(selfridge.P, selfridge.Q, (long)d, (long)n);
            if (lucas.U == 0 || lucas.V == 0)
                return true;
            for (ulong r = 1; r < s; r++)
            {
                d <<= 1;
                if (Sequence.Lucas.NthModM(selfridge.P, selfridge.Q, (long)d, (long)n).V == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// is not working
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool StrongLucasBaseP(ulong n, ulong P)
        {
            if (n == 2)
                return true;
            if (n < 2 || (n & 1) == 0 || System.Math.Sqrt(n) % 1 == 0)
                return false;
            ulong d = (ulong)((long)n - Symbol.Jacobi(P * P + 4, n)), s = 0;
            for (; (d & 1) == 0; d >>= 1, s++) ;
            var lucas = Sequence.Lucas.NthModM((long)P, -1, (long)d, (long)n);
            if (lucas.U == 0 || lucas.V == 0)
                return true;
            for (ulong r = 1; r < s; r++)
            {
                d <<= 1;
                if (Sequence.Lucas.NthModM((long)P, -1, (long)d, (long)n).V == 0)
                    return true;
            }
            return false;
        }

        public static bool ExtraStrongLucas(ulong n)
        {
            if (n == 2)
                return true;
            if (n < 2 || (n & 1) == 0 || System.Math.Sqrt(n) % 1 == 0)
                return false;
            ulong d = (n + 1) >> 1, s = 1;
            for (; (d & 1) == 0; d >>= 1, s++) ;
            long P = 3, jac = Symbol.Jacobi(5, n);
            for (; jac != -1; P++, jac = Symbol.Jacobi(P * P - 4, (long)n)) ;
            var lucas = Sequence.Lucas.NthModM(P, 1, (long)d, (long)n);
            if ((lucas.U == 0 && (lucas.V == 2 || lucas.V == (long)n - 2)) || lucas.V == 0)
                return true;
            for (ulong r = 1; r < s - 1; r++)
            {
                d <<= 1;
                if (Sequence.Lucas.NthModM(P, 1, (long)d, (long)n).V == 0)
                    return true;
            }
            return false;
        }

        public static bool Poulet(ulong n) => Fermat(n, 2);

        public static bool Carmichael(ulong n)
        {
            for (ulong a = 2; a < n; a++)
                if (!Fermat(n, a))
                    return false;
            return true;
        }

        public static bool Fermat(ulong n, ulong a)
        {
            if (n == 2)
                return true;
            if (n < 2 || (n & 1) == 0)
                return false;
            return Power.BinaryMod(a, n - 1, n) == 1;
        }

        public static bool Strong(ulong n, ulong a)
        {
            if (n == 2)
                return true;
            if (n < 2 || (n & 1) == 0)
                return false;
            ulong d = (n - 1) >> 1, s = 1;
            for (; (d & 1) == 0; d >>= 1, s++) ;
            a = Power.BinaryMod(a, d, n);
            if (a == 1 || a == n - 1)
                return true;
            for (ulong r = 1; r < s; r++)
            {
                a = (a * a) % n;
                if (a == 1)
                    return false;
                if (a == n - 1)
                    return true;
            }
            return false;
        }

        public static bool Euler(ulong n, ulong a)
        {
            if (n == 2)
                return true;
            if (n < 2 || (n & 1) == 0)
                return false;
            a = Power.BinaryMod(a, (n - 1) >> 1, n);
            return a == 1 || a == n - 1;
        }

        public static bool EulerJacobi(ulong n, ulong a)
        {
            if (n == 2)
                return true;
            if (n < 2 || (n & 1) == 0)
                return false;
            long jac = Symbol.Jacobi(a, n);
            return (long)Power.BinaryMod(a, (n - 1) >> 1, n) == (jac < 0 ? (long)n - 1 : jac);
        }

        public static bool Catalan(ulong n, ulong a)
        {
            if (n == 2)
                return true;
            if (n < 2 || (n & 1) == 0)
                return false;
            return Sequence.Catalan.Get((n - 1) >> 1) % n == ((((n - 1) >> 1) & 1) == 0 ? 2 : n - 2);
        }

        public static bool Fibonacci(ulong n) => Sequence.Lucas.NthModM(1, -1, (long)n, (long)n).V == 1;

        public static bool FibonacciAlternative(ulong n)
        {
            if (n < 2)
                return false;
            if (n == 5)
                return true;
            return Sequence.Lucas.NthModM(1, -1, (long)n - Symbol.Jacobi(5, n), (long)n).U == 0;
        }

        public static bool Frobenius(ulong n)
        {
            if (n == 2)
                return true;
            if (n < 2 || (n & 1) == 0 || System.Math.Sqrt(n) % 1 == 0)
                return false;
            var selfridge = Sequence.Lucas.SelfridgeMethode((long)n);
            if (GCD.Standard(n, (ulong)(selfridge.D * selfridge.Q) << 1) > 1)
                return false;
            var lucas = Sequence.Lucas.NthModM(selfridge.P, selfridge.Q, (long)n + 1, (long)n);
            return lucas.U == 0 && (lucas.V - (selfridge.Q << 1)) % (long)n == 0;
        }

        public static bool Pell(ulong n)
        {
            if (n < 2)
                return false;
            return Sequence.Lucas.NthModM(2, -1, (long)n - Symbol.Jacobi(8, n), (long)n).U == 0;
        }

        public static bool PellJacobi(ulong n)
        {
            if (n < 2)
                return false;
            return Sequence.Lucas.NthModM(2, -1, (long)n, (long)n).U == Symbol.Jacobi(2, n);
        }

        public static bool PellAlternative(ulong n)
        {
            if (n == 2)
                return true;
            if (n < 2 || (n & 1) == 0)
                return false;
            return Sequence.Lucas.NthModM(2, -1, (long)n, (long)n).V == 2;
        }

        public static bool Perrin(ulong n) => Sequence.Perrin.IterativeModM(n, n) == 0;
    }
}
