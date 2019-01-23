namespace DLib.Sort
{
    public static class Random
    {
        public static System.Random random = new System.Random();

        public static void Bozo(int[] array)
        {
            while (!Extra.Sorted(array))
                Extra.Swap(ref array[random.Next(0, array.Length)], ref array[random.Next(0, array.Length)]);
        }

        public static void Bozo1(int[] array)
        {
            while (!Extra.Sorted(array))
            {
                int i = random.Next(0, array.Length), j = random.Next(0, array.Length);
                if (i > j)
                    Extra.Swap(ref i, ref j);
                if (array[i] > array[j])
                    Extra.Swap(ref array[i], ref array[j]);
            }
        }
    }
}
