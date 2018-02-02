namespace DLib
{
    public static class Random
    {
        static System.Random r = new System.Random();

        public static int[] GetRandomPositiveIntegerArray(int length, int exclusive)
        {
            var v = new int[length];
            for (int i = 0; i < length; i++)
                v[i] = r.Next(0, exclusive);
            return v;
        }
    }
}
