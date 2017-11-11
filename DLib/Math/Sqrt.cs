namespace DLib.Math
{
    public static class Sqrt
    {
        public static double Approx(double n, int iterations)
        {
            double root = 1;
            for (int i = 0; i < iterations; i++)
                root = root - (root * root - n) / 2 / root;
            return root;
        }
    }
}
