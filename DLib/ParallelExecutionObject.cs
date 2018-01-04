using System;
using System.Threading.Tasks;

namespace DLib
{
    public abstract class ParallelExecutionObject
    {
        const int packageCount = 4;

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
        
        public static void SetParallel2() =>
            Loop = (fromInclusive, toExclusive, Action) =>
            {
                int c = toExclusive - fromInclusive;
                Parallel.For(0, packageCount, i =>
                {
                    for (int j = (int)(c * i / (double)packageCount), limit = (int)(c * (i + 1) / (double)packageCount); j < limit; j++)
                        Action(fromInclusive + j);
                });
            };
    }
}
