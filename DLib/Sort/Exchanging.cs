namespace DLib.Sort
{
    public static class Exchanging
    {
        public static int[] Bubble(int[] a)
        {
            bool b = true;
            for (int i = 0; b; i++)
            {
                b = false;
                for (int j = 0; j + 1 < a.Length - i; j++)
                    if (a[j] > a[j + 1])
                    {
                        Extra.Swap(ref a[j], ref a[j + 1]);
                        b = true;
                    }
            }
            return a;
        }

        public static int[] Cocktail(int[] a)
        {
            for (int i = 0; ; i++)
            {
                bool b = false;
                for (int j = i; j + 1 < a.Length - i; j++)
                    if (a[j] > a[j + 1])
                    {
                        Extra.Swap(ref a[j], ref a[j + 1]);
                        b = true;
                    }
                if (!b)
                    return a;
                b = false;
                for (int j = a.Length - i - 2; j - 1 >= i; j--)
                    if (a[j] < a[j - 1])
                    {
                        Extra.Swap(ref a[j], ref a[j - 1]);
                        b = true;
                    }
                if (!b)
                    return a;
            }
        }

        public static int[] Gnome(int[] a)
        {
            for (int i = 0, j = 0; i < a.Length;)
                if (i == 0 || a[i] >= a[i - 1])
                    i = ++j;
                else
                {
                    int tmp = a[i];
                    a[i] = a[i - 1];
                    a[--i] = tmp;
                }
            return a;
        }

        public static int[] Shell(int[] a)
        {
            int[] gapps = new int[(int)System.Math.Log(a.Length + 1, 2)];
            gapps[gapps.Length - 1] = 1;
            for (int i = gapps.Length - 1; i > 0; i--)
                gapps[i - 1] = ((gapps[i] + 1) << 1) - 1;
            foreach (int gap in gapps)
                for (int j = gap; j < a.Length; j++)
                {
                    int tmp = a[j], k = j;
                    for (; k >= gap && a[k - gap] > tmp; a[k] = a[k -= gap]) ;
                    a[k] = tmp;
                }
            return a;
        }

        public static int[] Comb(int[] a)
        {
            for (int gap = a.Length - 1; gap > 1; gap = (int)(gap / 1.3))
                for (int i = 0; i + gap < a.Length; i++)
                    if (a[i] > a[i + gap])
                        Extra.Swap(ref a[i], ref a[i + gap]);
            for (int i = 0; ; i++)
            {
                bool b = false;
                for (int j = i; j + 1 < a.Length - i; j++)
                    if (a[j] > a[j + 1])
                    {
                        Extra.Swap(ref a[j], ref a[j + 1]);
                        b = true;
                    }
                if (!b)
                    return a;
                b = false;
                for (int j = a.Length - i - 2; j - 1 >= i; j--)
                    if (a[j] < a[j - 1])
                    {
                        Extra.Swap(ref a[j], ref a[j - 1]);
                        b = true;
                    }
                if (!b)
                    return a;
            }
        }

        public static void Comb(int[] a, int start, int count)
        {
            for (int gap = a.Length - 1; gap > 1; gap = (int)(gap / 1.3))
                for (int i = start; i + gap < start + count; i++)
                    if (a[i] > a[i + gap])
                        Extra.Swap(ref a[i], ref a[i + gap]);
            for (int i = start; ; i++)
            {
                bool b = false;
                for (int j = i; j + 1 < start + count - i; j++)
                    if (a[j] > a[j + 1])
                    {
                        Extra.Swap(ref a[j], ref a[j + 1]);
                        b = true;
                    }
                if (!b)
                    return;
                b = false;
                for (int j = start + count - i - 2; j - 1 >= i; j--)
                    if (a[j] < a[j - 1])
                    {
                        Extra.Swap(ref a[j], ref a[j - 1]);
                        b = true;
                    }
                if (!b)
                    return;
            }
        }
    }
}
