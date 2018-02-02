namespace DLib.Sort
{
    public static class Merging
    {
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
                            Exchanging.Comb(a, start, count);
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
                            Exchanging.Comb(a, start, count);
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
                            Exchanging.Comb(a, start, System.Math.Min(a.Length - start, 2048));
                        for (int firstCount = 1; firstCount < a.Length; firstCount <<= 1)
                            for (int start = 0; start + firstCount < a.Length; start += (firstCount << 1))
                                Combine2Sections(a, start, firstCount);
                        return a;
                    }

                    public static int[] StartTri(int[] a)
                    {
                        for (int start = 0; start + 1 < a.Length; start += 2048)
                            Exchanging.Comb(a, start, System.Math.Min(a.Length - start, 2048));
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

        public static int[] Tim(int[] a)
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
    }
}
