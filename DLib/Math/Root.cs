namespace DLib.Math
{
    public static class Root
    {
        public static double Approx(double n, int iterations)
        {
            double root = 1;
            for (int i = 0; i < iterations; i++)
                root = root - (root * root - n) / 2 / root;
            return root;
        }

        public static double Bisection(double n, int w, int precision) => NonlinearEquations.Bisection(n >= 1 ? 1 : 0, n >= 1 ? n : 1, m => System.Math.Pow(m, w) - n, precision);
    }
}
