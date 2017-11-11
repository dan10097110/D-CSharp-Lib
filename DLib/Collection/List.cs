using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Collection
{
    public class List<T>
    {
        System.Collections.Generic.List<System.Collections.Generic.List<T>> lists;

        public ulong Count { get; private set; }

        public System.Collections.Generic.List<System.Collections.Generic.List<T>> Lists => lists;

        public List()
        {
            Count = 0;
            lists = new System.Collections.Generic.List<System.Collections.Generic.List<T>>();
        }

        public T this[ulong i]
        {
            get => lists[(int)(i / (double)int.MaxValue)][(int)(i % (int.MaxValue - 1))];
            set => lists[(int)(i / (double)int.MaxValue)][(int)(i % (int.MaxValue - 1))] = value;
        }

        public void Add(T item)
        {
            Count++;
            if (System.Math.Ceiling(Count / (double)int.MaxValue) > lists.Count)
                lists.Add(new System.Collections.Generic.List<T>());
            lists.Last().Add(item);
        }

        public T Last()
        {
            return lists.Last().Last();
        }

        public void Foreach(Action<T> action)
        {
            foreach (System.Collections.Generic.List<T> list in lists)
                foreach (T item in list)
                    action(item);
        }
    }
}
