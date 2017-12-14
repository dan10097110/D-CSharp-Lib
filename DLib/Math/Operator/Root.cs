namespace DLib.Math
{
    public static class Root
    {
        public static double Newton(double n, int w, int iterations)
        {
            double root = 1;
            for (int i = 0; i < iterations; root = ((w - 1) * System.Math.Pow(root, w) + n) / (w * System.Math.Pow(root, w - 1)), i++) ;
            return root;
        }
        
        public static double NewtonSqrt(double n, int iterations)
        {
            double root = 1;
            for (int i = 0; i < iterations; root = (root + n / root) / 2, i++) ;
            return root;
        }

        public static double Bisection(double n, int w, int precision) => NonlinearEquations.Bisection(n >= 1 ? 1 : 0, n >= 1 ? n : 1, m => System.Math.Pow(m, w) - n, precision);
    }
}
