namespace DLib.Math.Sequence
{
    public static class Lucas
    {
        public static (long P, long Q, long D) SelfridgeMethode(long n)
        {
            long D = 5, factor = 1, jac = Symbol.Jacobi(D, n);
            for (; jac != -1; D += 2, factor = -factor, jac = Symbol.Jacobi(D * factor, n)) ;
            D *= factor;
            return (1, (1 - D) >> 2, D);
        }

        public static (long U, long V) Nth(long P, long Q, long n)
        {
            long D = P * P - (Q << 2), U_i = 0, V_i = 2, Q_i = 1;
            foreach (bool bit in Extra.ToBinaryBoolArray(n))
            {
                U_i *= V_i;
                V_i = V_i * V_i - (Q_i << 1);
                Q_i *= Q_i;
                if (bit)
                {
                    long tmp = P * U_i + V_i, tmp2 = D * U_i + P * V_i;
                    U_i = ((tmp & 1) == 0 ? tmp : (tmp + n)) >> 1;
                    V_i = ((tmp2 & 1) == 0 ? tmp2 : (tmp2 + n)) >> 1;
                    Q_i *= Q;
                }
            }
            return (U_i, V_i);
        }

        public static (long U, long V, long Q) NthModM(long n, long m)
        {
            var selfridge = SelfridgeMethode(n);
            return NthModM(selfridge.P, selfridge.Q);
        }

        public static (long U, long V, long Q) NthModM(long P, long Q, long n, long m) => NthModM(P, Q, n, m, 0, 2, 1);

        public static (long U, long V, long Q) NthModM(long P, long Q, long n, long m, long U_0, long V_0, long Q_0)
        {
            long D = P * P - (Q << 2), U_i = U_0, V_i = V_0, Q_i = Q_0;
            foreach (bool bit in Extra.ToBinaryBoolArray(n))
            {
                U_i = (U_i * V_i) % m;
                V_i = (V_i * V_i - (Q_i << 1)) % m;
                Q_i = (Q_i * Q_i) % m;
                if (bit)
                {
                    long tmp = P * U_i + V_i, tmp2 = D * U_i + P * V_i;
                    U_i = (((tmp & 1) == 0 ? tmp : (tmp + m)) >> 1) % m;
                    V_i = (((tmp2 & 1) == 0 ? tmp2 : (tmp2 + m)) >> 1) % m;
                    Q_i = (Q_i * Q) % m;
                }
            }
            return (U_i, V_i, Q_i);
        }
    }
}
