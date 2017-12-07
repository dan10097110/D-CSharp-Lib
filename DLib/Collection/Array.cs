using System;
using System.Linq;

namespace DLib.Collection
{
    class Array<T>
    {
        public int Dimensions => length.Length;
        int[] length;
        public T[] array;

        public Array(params int[] length)
        {
            this.length = length.ToArray();
            array = new T[length.Aggregate(1, (acc, val) => acc * val)];
        }

        public Array(T[] array)
        {
            length = new int[] { array.GetLength(0) };
            array = array.ToArray();
        }

        public Array(T[,] array)
        {
            length = new int[] { array.GetLength(0), array.GetLength(1) };
            this.array = new T[length.Aggregate(1, (acc, val) => acc * val)];
            for (int i = 0; i < GetLength(0); i++)
                for(int j = 0; j < GetLength(1); j++)
                    this.array[GetIndex(i, j)] = array[i, j];
        }

        public Array(Array<T> array)
        {
            this.array = array.array.ToArray();
            length = array.length.ToArray();
        }

        public Array<T> Clone() => new Array<T>(this);

        public T Get(params int[] pos) => pos.Length == Dimensions ? array[GetIndex(pos)] : throw new ArgumentException();

        public void Set(T value, params int[] pos) => array[GetIndex(pos)] = pos.Length != Dimensions ? throw new ArgumentException() : value;

        int GetIndex(params int[] pos)
        {
            int p = 0;
            for (int i = 0; i < Dimensions; i++)
            {
                int q = pos[i];
                for (int j = 0; j < i; q *= GetLength(j), j++) ;
                p += q;
            }
            return p;
        }

        public int GetLength(int dimension) => dimension >= 0 && dimension < Dimensions ? length[dimension] : throw new ArgumentException();

        public void ToString()
        {
            for(int i = 0; i < Dimensions; i++)
            {
                for(int j = 0; j < GetLength(i); j++)
                {

                }
            }
        }
    }
}
