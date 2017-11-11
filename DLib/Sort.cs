using System;

namespace DLib
{
    public static class Sort
    {
        public static class Swap
        {
            public static int[] BubbleSort(int[] a)
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

            public static int[] CocktailSort(int[] a)
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

            public static int[] GnomeSort(int[] a)
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

            public static int[] ShellSort(int[] a)
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

            public static int[] CombSort(int[] a)
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
        }

        public static class Selection
        {
            public static int[] Start(int[] a)
            {
                for (int k = 0; k < a.Length - 1; k++)
                {
                    int i = k;
                    for (int j = k + 1; j < a.Length; j++)
                        if (a[j] < a[i])
                            i = j;
                    Extra.Swap(ref a[k], ref a[i]);
                }
                return a;
            }

            public static int[] Start2(int[] a)
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
                return a;
            }
        }

        public static class Insertion
        {
            public static int[] Start(int[] a)
            {
                for (int i = 1; i < a.Length; i++)
                {
                    int tmp = a[i], j = i;
                    for (; j > 0 && a[j - 1] > tmp; a[j] = a[--j]) ;
                    a[j] = tmp;
                }
                return a;
            }

            public static int[] StartBinary(int[] a)
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
                return a;
            }
        }

        public static class Heap
        {
            public static int[] Start(int[] a)
            {
                for (int start = (a.Length - 2) >> 1; start >= 0; start--)
                    ShiftDown(a, start, a.Length - 1);
                for (int end = a.Length - 1; end > 0;)
                {
                    Extra.Swap(ref a[0], ref a[end]);
                    ShiftDown(a, 0, --end);
                }
                return a;
            }

            public static int[] Start2(int[] a)
            {
                for (int end = 1; end < a.Length; end++)
                    ShiftUp(a, 0, end);
                for (int end = a.Length - 1; end > 0;)
                {
                    Extra.Swap(ref a[0], ref a[end]);
                    ShiftDown(a, 0, --end);
                }
                return a;
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

        public static class Quick
        {
            static Random r = new Random();

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

        public static class Merge
        {
            public class Rekursiv
            {
                public static int[] Start(int[] a)
                {
                    Intern(a, 0, a.Length);
                    return a;
                }

                static void Intern(int[] a, int start, int count)
                {
                    if (count > 1)
                    {
                        Intern(a, start, count >> 1);
                        Intern(a, start + (count >> 1), count >> 1);
                        Combine2Sections(a, start, count);
                    }
                }

                public static int[] StartTri(int[] a)
                {
                    InternTri(a, 0, a.Length);
                    return a;
                }

                static void InternTri(int[] a, int start, int count)
                {
                    if (count > 2)
                    {
                        int firstCount = count / 3;
                        InternTri(a, start, firstCount);
                        InternTri(a, start + firstCount, firstCount);
                        InternTri(a, start + (firstCount << 1), count - (firstCount << 1));
                        Combine3Sections(a, start, count);
                    }
                    else if (count > 1)
                    {
                        Intern(a, start, count >> 1);
                        Intern(a, start + (count >> 1), count >> 1);
                        Combine2Sections(a, start, count);
                    }
                }

                public class Combined
                {
                    public static int[] Start(int[] a)
                    {
                        Intern(a, 0, a.Length);
                        return a;
                    }

                    static void Intern(int[] a, int start, int count)
                    {
                        if (count > 2048)
                        {
                            int firstCount = count >> 1;
                            Intern(a, start, firstCount);
                            Intern(a, start + firstCount, count - firstCount);
                            Combine2Sections(a, start, count);
                        }
                        else if (count > 1)
                            CombSort(a, start, count);
                    }

                    public static int[] StartTri(int[] a)
                    {
                        InternTri(a, 0, a.Length);
                        return a;
                    }

                    static void InternTri(int[] a, int start, int count)
                    {
                        if (count > 2048)
                        {
                            int firstCount = count / 3;
                            InternTri(a, start, firstCount);
                            InternTri(a, start + firstCount, firstCount);
                            InternTri(a, start + (firstCount << 1), count - (firstCount << 1));
                            Combine3Sections(a, start, count);
                        }
                        else if (count > 1)
                            CombSort(a, start, count);
                    }
                }

                static void Combine2Sections(int[] a, int start, int count)
                {
                    int[] firstSection = new int[count >> 1];
                    for (int i = 0; i < (count >> 1); i++)
                        firstSection[i] = a[start + i];
                    for (int exclusiveIndex = start + count, j = 0, k = start + firstSection.Length; ;)
                        if (firstSection[j] < a[k])
                        {
                            a[start++] = firstSection[j++];
                            if (j >= firstSection.Length)
                                return;
                        }
                        else
                        {
                            a[start++] = a[k++];
                            if (k >= exclusiveIndex)
                            {
                                while (j < firstSection.Length)
                                    a[start++] = firstSection[j++];
                                return;
                            }
                        }
                }

                static void Combine3Sections(int[] a, int start, int count)
                {
                    int firstCount = count / 3;
                    int[] firstTwoSections = new int[firstCount << 1];
                    for (int i = 0; i < firstTwoSections.Length; i++)
                        firstTwoSections[i] = a[start + i];
                    for (int exclusiveIndex = start + count, i = start, j = 0, k = firstCount, l = start + firstTwoSections.Length; ;)
                    {
                        if (firstTwoSections[j] < firstTwoSections[k] && firstTwoSections[j] < a[l])
                        {
                            a[i++] = firstTwoSections[j++];
                            if (j >= firstCount)
                                while (true)
                                    if (firstTwoSections[k] < a[l])
                                    {
                                        a[i++] = firstTwoSections[k++];
                                        if (k >= firstTwoSections.Length)
                                            return;
                                    }
                                    else
                                    {
                                        a[i++] = a[l++];
                                        if (l >= exclusiveIndex)
                                        {
                                            while (k < firstTwoSections.Length)
                                                a[i++] = firstTwoSections[k++];
                                            return;
                                        }
                                    }
                        }
                        else if (firstTwoSections[k] < firstTwoSections[j] && firstTwoSections[k] < a[l])
                        {
                            a[i++] = firstTwoSections[k++];
                            if (k >= firstTwoSections.Length)
                                while (true)
                                    if (firstTwoSections[j] < a[l])
                                    {
                                        a[i++] = firstTwoSections[j++];
                                        if (j >= firstCount)
                                            return;
                                    }
                                    else
                                    {
                                        a[i++] = a[l++];
                                        if (l >= exclusiveIndex)
                                        {
                                            while (j < firstCount)
                                                a[i++] = firstTwoSections[j++];
                                            return;
                                        }
                                    }
                        }
                        else
                        {
                            a[i++] = a[l++];
                            if (l >= exclusiveIndex)
                                while (true)
                                    if (firstTwoSections[j] < firstTwoSections[k])
                                    {
                                        a[i++] = firstTwoSections[j++];
                                        if (j >= firstCount)
                                        {
                                            while (k < firstTwoSections.Length)
                                                a[i++] = firstTwoSections[k++];
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        a[i++] = firstTwoSections[k++];
                                        if (k >= firstTwoSections.Length)
                                        {
                                            while (j < firstCount)
                                                a[i++] = firstTwoSections[j++];
                                            return;
                                        }
                                    }
                        }
                    }
                }
            }

            public class Iterativ
            {
                public static int[] Start(int[] a)
                {
                    for (int firstCount = 1; firstCount < a.Length; firstCount <<= 1)
                        for (int start = 0; start + firstCount < a.Length; start += (firstCount << 1))
                            Combine2Sections(a, start, firstCount);
                    return a;
                }

                public class Combined
                {
                    public static int[] Start(int[] a)
                    {
                        for (int start = 0; start + 1 < a.Length; start += 2048)
                            CombSort(a, start, System.Math.Min(a.Length - start, 2048));
                        for (int firstCount = 1; firstCount < a.Length; firstCount <<= 1)
                            for (int start = 0; start + firstCount < a.Length; start += (firstCount << 1))
                                Combine2Sections(a, start, firstCount);
                        return a;
                    }

                    public static int[] StartTri(int[] a)
                    {
                        for (int start = 0; start + 1 < a.Length; start += 2048)
                            CombSort(a, start, System.Math.Min(a.Length - start, 2048));
                        for (int size = 2048; size < a.Length; size *= 3)
                            for (int u = 0; u + size < a.Length; u += size * 3)
                                Combine3Sections(a, u, size);
                        return a;
                    }
                }

                static void Combine2Sections(int[] a, int start, int firstCount)
                {
                    int[] firstSection = new int[firstCount];
                    for (int i = 0; i < firstCount; i++)
                        firstSection[i] = a[start + i];
                    for (int exclusiveIndex = start + System.Math.Min(a.Length - start, firstCount << 1), i = start, j = 0, k = start + firstCount; ;)
                        if (firstSection[j] < a[k])
                        {
                            a[i++] = firstSection[j++];
                            if (j >= firstCount)
                                return;
                        }
                        else
                        {
                            a[i++] = a[k++];
                            if (k >= exclusiveIndex)
                            {
                                while (j < firstCount)
                                    a[i++] = firstSection[j++];
                                return;
                            }
                        }
                }

                static void Combine3Sections(int[] a, int start, int firstCount)
                {
                    int range = System.Math.Min(a.Length - start, firstCount * 3);
                    int[] b = new int[range];
                    for (int i = 0; i < range; i++)
                        b[i] = a[start + i];
                    for (int i = start, yLimit = range - firstCount, x = 0, y = firstCount, z = yLimit; i < start + range; i++)
                    {
                        if (x < firstCount && y < yLimit && z < range)
                        {
                            bool akb = b[x] < b[y], akc = b[x] < b[z], bkc = b[y] < b[z];
                            if (akb && akc)
                            {
                                a[i] = b[x];
                                x++;
                            }
                            else if (!akb && bkc)
                            {
                                a[i] = b[y];
                                y++;
                            }
                            else
                            {
                                a[i] = b[z];
                                z++;
                            }
                        }
                        else if (x < firstCount && y < yLimit)
                        {
                            if (b[x] < b[y])
                            {
                                a[i] = b[x];
                                x++;
                            }
                            else
                            {
                                a[i] = b[y];
                                y++;
                            }
                        }
                        else if (x < firstCount && z < range)
                        {
                            if (b[x] < b[z])
                            {
                                a[i] = b[x];
                                x++;
                            }
                            else
                            {
                                a[i] = b[z];
                                z++;
                            }
                        }
                        else if (y < yLimit && z < range)
                        {
                            if (b[y] < b[y])
                            {
                                a[i] = b[y];
                                y++;
                            }
                            else
                            {
                                a[i] = b[z];
                                z++;
                            }
                        }
                        else if (x < firstCount)
                        {
                            a[i] = b[x];
                            x++;
                        }
                        else if (y < yLimit)
                        {
                            a[i] = b[y];
                            y++;
                        }
                        else
                        {
                            a[i] = b[z];
                            z++;
                        }
                    }
                }
            }
        }

        public static void CombSort(int[] a, int start, int count)
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

        public static int[] TimSort(int[] a)
        {
            for (int u = 1; u < a.Length;)
            {
                int o = u + 2;
                if (o < a.Length)
                {
                    if (a[u] <= a[u + 1])
                    {
                        for (; o < a.Length && a[o - 1] <= a[o]; o++) ;
                        int[] firstSection = new int[o - u];
                        for (int i = 0; i < firstSection.Length; i++)
                            firstSection[i] = a[u + i];
                        for (int i = o - 1, j = firstSection.Length - 1, k = u - 1; ;)
                            if (firstSection[j] >= a[k])
                            {
                                a[i--] = firstSection[j--];
                                if (j < 0)
                                    break;
                            }
                            else
                            {
                                a[i--] = a[k--];
                                if (k < 0)
                                {
                                    while (j >= 0)
                                        a[i--] = firstSection[j--];
                                    break;
                                }
                            }
                    }
                    else
                    {
                        for (; o < a.Length && a[o - 1] > a[o]; o++) ;
                        int[] firstSection = new int[o - u];
                        for (int i = 0; i < firstSection.Length; i++)
                            firstSection[i] = a[u + i];
                        for (int i = o - 1, j = 0, k = u - 1; ;)
                            if (firstSection[j] >= a[k])
                            {
                                a[i--] = firstSection[j++];
                                if (j >= firstSection.Length)
                                    break;
                            }
                            else
                            {
                                a[i--] = a[k--];
                                if (k < 0)
                                {
                                    while (j < firstSection.Length)
                                        a[i--] = firstSection[j++];
                                    break;
                                }
                            }
                    }
                }
                else if (u + 1 < a.Length)
                {
                    int tmp = a[u + 1], j = 0;
                    for (int h = u + 1; j != h;)
                    {
                        int d = (j + h) >> 1;
                        if (a[d] < tmp)
                            j = d + 1;
                        else
                            h = d;
                    }
                    for (int k = u + 1; k > j; a[k] = a[--k]) ;
                    a[j] = tmp;
                    break;
                }
                u = o;
            }
            return a;
        }

        public static int[] RadixSort(int[] a)
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

        public static int[] CountingSort(int[] a)
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

        public static int[] FlashSort(int[] a)
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
            return Swap.CocktailSort(a);
        }

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
    }
}
