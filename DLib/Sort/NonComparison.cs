namespace DLib.Sort
{
    public static class NonComparison
    {
        public static class Bucket
        {
            public static int[] Start(int[] a)
            {
                int j = 0;
                for (int i = 1; i < a.Length; i++)
                    if (a[i] > a[j])
                        j = i;
                Intern(a, (int)System.Math.Log10(a[j]), 0, a.Length);
                return a;
            }

            public static void Intern(int[] a, int n, int u, int o)
            {
                if (n >= 0 && o > 1 + u)
                    for (int digit = 0, i1 = u, i2 = o - 1; digit < 5; o = i2 + 1, u = i1, digit++)
                    {
                        for (int i = i1; i <= i2;)
                        {
                            int m = a[i];
                            for (int p = 0; p < n; m /= 10, p++) ;//abbrevhen wenn n< 10 
                            int q = m % 10;
                            if (q == digit)
                            {
                                int tmp = a[i1];
                                a[i1++] = a[i];
                                a[i] = tmp;
                            }
                            else if (q + digit == 9)
                            {
                                int tmp = a[i2];
                                a[i2--] = a[i];
                                a[i] = tmp;
                                continue;
                            }
                            i++;
                        }
                        Intern(a, n - 1, u, i1);
                        Intern(a, n - 1, i2 + 1, o);
                    }
            }
        }

        public static int[] Radix(int[] a)
        {
            int m = a[0];
            for (int i = 1; i < a.Length; i++)
                if (a[i] > m)
                    m = a[i];
            for (int exp = 1; m / exp > 0; exp *= 10)
            {
                int[] count = new int[10], output = new int[a.Length];
                for (int i = 0; i < a.Length; i++)
                    count[(a[i] / exp) % 10]++;
                for (int i = 1; i < 10; i++)
                    count[i] += count[i - 1];
                for (int i = a.Length - 1; i >= 0; i--)
                    output[--count[(a[i] / exp) % 10]] = a[i];
                for (int i = 0; i < a.Length; i++)
                    a[i] = output[i];
            }
            return a;
        }

        public static int[] Counting(int[] a)
        {
            int min = a[0], max = a[0];
            for (int i = 1; i < a.Length; i++)
                if (a[i] < min)
                    min = a[i];
                else if (a[i] > max)
                    max = a[i];
            int[] b = new int[max - min + 1];
            for (int i = 0; i < a.Length; i++)
                b[a[i] - min] += 1;
            for (int i = 0, j = 0; i < b.Length; i++)
                for (int k = 0; k < b[i]; k++)
                    a[j++] = i + min;
            return a;
        }

        public static int[] Flash(int[] a)
        {
            int min = a[0], max = a[0];
            for (int i = 1; i < a.Length; i++)
                if (a[i] < min)
                    min = a[i];
                else if (a[i] > max)
                    max = a[i];
            double tmp = (a.Length - 1) / (double)(max - min);
            int[] b = new int[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                int j = (int)(tmp * (a[i] - min));
                if (b[j] == 0)
                    b[j] = a[i];
                else
                    for (int k = 0; ;)
                    {
                        if (b[j + k] == 0)
                            b[j + k] = a[i];
                        else if (b[j - k] == 0)
                            b[j - k] = a[i];
                        k++;
                        if (j + k >= a.Length)
                        {
                            for (; j - k < 0; k++)
                                if (b[j - k] == 0)
                                    b[j - k] = a[i];
                            break;
                        }
                        else if (j - k < 0)
                        {
                            for (; j + k < 0; k++)
                                if (b[j + k] == 0)
                                    b[j + k] = a[i];
                            break;
                        }
                    }
            }
            return Exchanging.Cocktail(a);
        }

        public static int[] Bead(int[] a)
        {
            int max = 0;
            for (int i = 0; i < a.Length;)
                if (a[i] > max)
                    max = a[i];
            var v = new bool[max, a.Length];
            for (int i = 0; i < a.Length; i++)
                for (int j = 0; j < a[i]; v[j, i] = true, j++) ;
            for(int i = 0; i < max; i++)
                for(int j = 0, c = 0; j < a.Length; j++)
                    if(v[j, i])
                    {
                        v[j, i] = false;
                        v[c, i] = true;
                        c++;
                    }
            for (int i = 0; i < a.Length; i++)
            {
                int j = 0;
                for (; v[j, i]; j++) ;
                a[i] = j;
            }
            return a;
        }
    }
}
