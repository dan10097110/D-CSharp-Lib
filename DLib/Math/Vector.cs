using System;
using System.Text;

namespace DLib.Math
{
    public class Vector
    {
        double[] vector;

        public int Length => vector.Length;

        public Vector(int length) => vector = new double[length];

        public Vector(params double[] array) => vector = (double[])array.Clone();

        public Vector(Vector vector) => this.vector = (double[])vector.vector.Clone();

        public Vector Clone() => new Vector(this);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("( ");
            for (int i = 0; i < Length; i++)
            {
                sb.Append(vector[i]);
                if (i + 1 < Length)
                    sb.Append(", ");
            }
            sb.Append(" )");
            return sb.ToString();
        }

        public static Vector operator +(Vector a, Vector b)
        {
            if (a.Length != b.Length)
                throw new Exception("different length of vectors");
            var vector = new Vector(a.Length);
            for (int i = 0; i < vector.Length; i++)
                vector[i] = a[i] + b[i];
            return vector;
        }

        public static Vector operator -(Vector a, Vector b)
        {
            if (a.Length != b.Length)
                throw new Exception("different length of vectors");
            var vector = new Vector(a.Length);
            for (int i = 0; i < vector.Length; i++)
                vector[i] = a[i] - b[i];
            return vector;
        }

        public static Vector operator *(Vector a, double b)
        {
            var vector = new Vector(a.Length);
            for (int i = 0; i < vector.Length; i++)
                vector[i] = a[i] * b;
            return vector;
        }

        public static double operator *(Vector a, Vector b)
        {
            if (a.Length != b.Length)
                throw new Exception("different length of vectors");
            double d = 0;
            for (int i = 0; i < a.Length; i++)
                d += (a[i] * b[i]);
            return d;
        }

        public static Vector operator /(Vector a, double b)
        {
            var vector = new Vector(a.Length);
            for (int i = 0; i < vector.Length; i++)
                vector[i] = a[i] / b;
            return vector;
        }

        public double this[int i]
        {
            get => i < Length ? vector[i] : throw new IndexOutOfRangeException();
            set
            {
                if (i < Length)
                    vector[i] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public double Absolute()
        {
            double i = 0;
            foreach (double d in vector)
                i += (d * d);
            return System.Math.Sqrt(i);
        }

        public double Distance(Vector vector) => (this - vector).Absolute();
    }
}
