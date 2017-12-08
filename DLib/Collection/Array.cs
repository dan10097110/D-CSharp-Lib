using System;
using System.Linq;
using System.Text;

namespace DLib.Collection
{
    public class Array<T>
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
            this.array = array.ToArray();
        }

        public Array(T[,] array)
        {
            length = new int[] { array.GetLength(1), array.GetLength(0) };
            this.array = new T[length.Aggregate(1, (acc, val) => acc * val)];
            for (int i = 0; i < array.GetLength(0); i++)
                for(int j = 0; j < array.GetLength(1); j++)
                    this.array[GetIndex(j, i)] = array[i, j];
        }

        public Array(T[,,] array)
        {
            length = new int[] { array.GetLength(2), array.GetLength(1), array.GetLength(0) };
            this.array = new T[length.Aggregate(1, (acc, val) => acc * val)];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    for(int k = 0; k < array.GetLength(2); k++)
                        this.array[GetIndex(k, j, i)] = array[i, j, k];
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

        public override string ToString()
        {
            var sb = new StringBuilder();
            var pos = new int[Dimensions];
            var v = new T[GetLength(0)];
            for (int d = Dimensions; ;)
            {
                for (; d > 0; d--)
                    sb.Append("(");
                for (; pos[0] < GetLength(0); pos[0]++)
                    v[pos[0]] = Get(pos);
                sb.Append(string.Join(", ", v));
                for (; pos[d] >= GetLength(d); pos[d - 1] = 0, pos[d]++)
                {
                    sb.Append(")");
                    d++;
                    if (d >= Dimensions)
                        return sb.ToString();
                }
                sb.Append(", ");
            }
        }
    }
}
