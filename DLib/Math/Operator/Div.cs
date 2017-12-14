namespace DLib.Math.Operator
{
    public static class Div
    {
        public static double NewtonReciprocal(double n, int iterations)
        {
            double div = 1;
            for (int i = 0; i < iterations; div = (2 - n * div) * div, i++) ;
            return div;
        }
    }
}
