using System;
using System.Threading.Tasks;

namespace DLib
{
    public abstract class ParallelExecutionObject
    {
        protected static Action<int, int, Action<int>> Loop = (fromInclusive, toExclusive, Action) => {
            for (; fromInclusive < toExclusive; fromInclusive++)
                Action(fromInclusive);
        };

        public static void SetSerial()
        {
            Loop = (fromInclusive, toExclusive, Action) => {
                for (; fromInclusive < toExclusive; fromInclusive++)
                    Action(fromInclusive);
            };
        }

        public static void SetParallel() => Loop = (fromInclusive, toExclusive, Action) => Parallel.For(fromInclusive, toExclusive, Action);

        const int c2 = 10, c1 = 1 << c2;

        public static void SetParallel2() =>
            Loop = (fromInclusive, toExclusive, Action) =>
            {
                int m = (toExclusive - fromInclusive) & (c1 - 1);
                for (int j = 0; j < m; j++)
                    Action(fromInclusive + j);
                Parallel.For(0, (toExclusive - fromInclusive + (m == 0 ? 0 : m - c1)) >> c2, i =>
                {
                    for (int j = 0; j < c1; j++)
                        Action(fromInclusive + (i << c2) + j + m);
                });
            };
    }
}
