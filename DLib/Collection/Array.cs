using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DLib.Collection
{
    public class Array<T>
    {
        T[][] arrays;

        public ulong Length { get; private set; }

        public Array(ulong length)
        {
            arrays = new T[(int)System.Math.Ceiling(length / (double)int.MaxValue)][];
            for (int i = 0; i < arrays.Length - 1; i++)
                arrays[i] = new T[int.MaxValue];
            arrays[arrays.Length - 1] = new T[(int)(length % (int.MaxValue - 1))];
            Length = length;
        }

        public Array(List<T> list)
        {
            arrays = new T[list.Lists.Count][];
            for (int i = 0; i < arrays.Length; i++)
                arrays[i] = list.Lists[i].ToArray();
            Length = list.Count;
        }

        public T this[ulong i]
        {
            get => arrays[(int)(i / (double)int.MaxValue)][(int)(i % (int.MaxValue - 1))];
            set => arrays[(int)(i / (double)int.MaxValue)][(int)(i % (int.MaxValue - 1))] = value;
        }

        public void Foreach(Action<T> action)
        {
            foreach (T[] array in arrays)
                foreach (T item in array)
                    action(item);
        }
    }
}
