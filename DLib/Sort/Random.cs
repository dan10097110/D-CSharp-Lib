namespace DLib.Sort
{
    public static class Random
    {
        public static System.Random random = new System.Random();

        public static void Bozo(int[] array)
        {
            while (!Sorted(array))
                Extra.Swap(ref array[random.Next(0, array.Length)], ref array[random.Next(0, array.Length)]);
        }

        public static void Bozo1(int[] array)
        {
            while (!Sorted(array))
            {
                int i = random.Next(0, array.Length), j = random.Next(0, array.Length);
                if (i > j)
                    Extra.Swap(ref i, ref j);
                if (array[i] > array[j])
                    Extra.Swap(ref array[i], ref array[j]);
            }
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
