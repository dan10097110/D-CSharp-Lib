using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLib.Math
{
    public class Vector
    {
        double[] array;

        public int Length => array.Length;

        public Vector(int length) => array = new double[length];

        public Vector(params double[] array) => this.array = (double[])array.Clone();

        public Vector(Vector vector) => this.array = (double[])vector.array.Clone();

        public Vector Clone() => new Vector(this);

        public override string ToString() => "( " + string.Join(", ", array) + " )";

        public static Vector operator +(Vector a, Vector b) => a.Length == b.Length ? a.array.Zip(b.array, (x, y) => x + y).ToArray() : throw new Exception("different length of vectors");

        public static Vector operator -(Vector a, Vector b) => a.Length == b.Length ? a.array.Zip(b.array, (x, y) => x - y).ToArray() : throw new Exception("different length of vectors");

        public static double operator *(Vector a, Vector b) => a.Length == b.Length ? a.array.Zip(b.array, (x, y) => x * y).Sum() : throw new Exception("different length of vectors");

        public static Vector operator *(Vector a, double b) => a.array.ToArray().Select(n => n * b).ToArray();

        public static Vector operator /(Vector a, double b) => a.array.ToArray().Select(n => n / b).ToArray();

        public double this[int i]
        {
            get => i < Length ? array[i] : throw new IndexOutOfRangeException();
            set => array[i] = i < Length ? value : throw new IndexOutOfRangeException();
        }

        public double Absolute() => System.Math.Sqrt(array.Select(n => n* n).Sum());

        public double Distance(Vector vector) => (this - vector).Absolute();

        public static implicit operator Vector(double[] array) => new Vector() { array = array };

        public static implicit operator double[] (Vector vector) => vector.array;
    }
}