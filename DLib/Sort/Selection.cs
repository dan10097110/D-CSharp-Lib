namespace DLib.Sort
{
    public static class Selection
    {
        public static void Start(int[] a)
        {
            for (int k = 0; k < a.Length - 1; k++)
            {
                int i = k;
                for (int j = k + 1; j < a.Length; j++)
                    if (a[j] < a[i])
                        i = j;
                Extra.Swap(ref a[k], ref a[i]);
            }
        }

        public static void Start2(int[] a)
        {
            for (int i = 0; i << 1 < a.Length; i++)
            {
                int tmp = a[i], x = i;
                for (int j = i + 1; j < a.Length - i; j++)
                    if (a[j] < a[x])
                        x = j;
                a[i] = a[x];
                a[x] = tmp;

                tmp = a[a.Length - 1 - i];
                x = i + 1;
                for (int j = i + 2; j < a.Length - i; j++)
                    if (a[j] > a[x])
                        x = j;
                a[a.Length - 1 - i] = a[x];
                a[x] = tmp;
            }
        }
        
        public static class Heap
        {
            public static void Start(int[] a)
            {
                for (int start = (a.Length - 2) >> 1; start >= 0; start--)
                    ShiftDown(a, start, a.Length - 1);
                for (int end = a.Length - 1; end > 0;)
                {
                    Extra.Swap(ref a[0], ref a[end]);
                    ShiftDown(a, 0, --end);
                }
            }

            public static void Start2(int[] a)
            {
                for (int end = 1; end < a.Length; end++)
                    ShiftUp(a, 0, end);
                for (int end = a.Length - 1; end > 0;)
                {
                    Extra.Swap(ref a[0], ref a[end]);
                    ShiftDown(a, 0, --end);
                }
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

            static void ShiftUp(int[] a, int start, int end)
            {
                for (int child = end; child > start;)
                {
                    int parent = (child - 1) >> 1;
                    if (a[parent] < a[child])
                    {
                        Extra.Swap(ref a[parent], ref a[child]);
                        child = parent;
                    }
                    else
                        return;
                }
            }

            static int IParent(int i) => (i - 1) >> 1;

            static int ILeftChild(int i) => (i << 1) + 1;

            static int IRightChild(int i) => (i << 1) + 2;
        }
    }
}
