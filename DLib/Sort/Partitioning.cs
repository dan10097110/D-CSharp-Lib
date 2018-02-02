namespace DLib.Sort
{
    public static class Partitioning
    {
        public static class Quick
        {
            static System.Random r = new System.Random();

            public static int[] Start(int[] a)
            {
                Intern(a, 0, a.Length - 1);
                return a;
            }

            static void Intern(int[] a, int u, int o)
            {
                if (u < o)
                {
                    int p = Partition(a, u, o);
                    Intern(a, u, p - 1);
                    Intern(a, p + 1, o);
                }
            }

            static int Partition(int[] a, int u, int o)
            {
                int pivot = a[o], i = u - 1, tmp;
                for (int j = u; j < o; j++)
                    if (a[j] <= pivot)
                    {
                        tmp = a[++i];
                        a[i] = a[j];
                        a[j] = tmp;
                    }
                Extra.Swap(ref a[o], ref a[i + 1]);
                return i + 1;
            }
        }

        public static class IntroSort
        {
            public static int[] Start(int[] a)
            {
                Intern(a, 0, a.Length - 1, ((int)System.Math.Log(a.Length)) << 1);
                return a;
            }

            static void Intern(int[] a, int u, int o, int maxDepth)
            {
                if (maxDepth <= 0)
                {
                    for (int start = (o - 1) >> 1; start >= u; start--)
                        ShiftDown(a, start, o);
                    for (int end = o; end > u;)
                    {
                        Extra.Swap(ref a[u], ref a[end]);
                        ShiftDown(a, u, --end);
                    }
                }
                else if (o > u)
                {
                    int p = Partition(a, u, o);
                    Intern(a, u, p - 1, maxDepth - 1);
                    Intern(a, p + 1, o, maxDepth - 1);
                }
            }

            static int Partition(int[] a, int u, int o)
            {
                int pivot = a[o], i = u - 1, tmp;
                for (int j = u; j < o; j++)
                    if (a[j] <= pivot)
                    {
                        tmp = a[++i];
                        a[i] = a[j];
                        a[j] = tmp;
                    }
                Extra.Swap(ref a[o], ref a[i + 1]);
                return i + 1;
            }

            static void ShiftDown(int[] a, int start, int end)
            {
                for (int root = start; (root << 1) + 1 <= end;)
                {
                    int child = (root << 1) + 1, swap = root;
                    if (a[swap] < a[child])
                        swap = child;
                    if (child + 1 <= end && a[swap] < a[child + 1])
                        swap = child + 1;
                    if (swap == root)
                        return;
                    else
                    {
                        Extra.Swap(ref a[root], ref a[swap]);
                        root = swap;
                    }
                }
            }
        }
    }
}
