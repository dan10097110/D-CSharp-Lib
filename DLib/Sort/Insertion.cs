namespace DLib.Sort
{
    public static class Insertion
    {
        public static void Sequential(int[] a)
        {
            for (int i = 1; i < a.Length; i++)
            {
                int tmp = a[i], j = i;
                for (; j > 0 && a[j - 1] > tmp; a[j] = a[--j]) ;
                a[j] = tmp;
            }
        }

        public static void Binary(int[] a)
        {
            for (int i = 1; i < a.Length; i++)
            {
                int tmp = a[i], j = 0;
                for (int o = i; j != o;)
                {
                    int d = (j + o) >> 1;
                    if (a[d] < tmp)
                        j = d + 1;
                    else
                        o = d;
                }
                for (int k = i; k > j; a[k] = a[--k]) ;
                a[j] = tmp;
            }
        }
    }
}
