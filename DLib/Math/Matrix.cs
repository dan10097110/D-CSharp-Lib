using DLib.Collection;
using System;
using System.Text;

namespace DLib.Math
{
    public class Matrix
    {
        double[,] array;

        public int Width => array.GetLength(0);
        public int Height => array.GetLength(1);
        
        public Matrix() { }

        public Matrix(int width, int height) => array = new double[width, height];

        public Matrix(double[,] array) => this.array = (double[,])array.Clone();

        public Matrix(Matrix vector) => this.array = (double[,])vector.array.Clone();

        public Matrix Clone() => new Matrix(this);

        public double this[int i, int j]
        {
            get => i < Width && j < Height ? array[i, j] : throw new IndexOutOfRangeException();
            set => array[i, j] = i < Width && j < Height ? value : throw new IndexOutOfRangeException();
        }

        public Vector GetRow(int i)
        {
            var vector = new Vector(Width);
            for (int j = 0; j < Width; j++)
                vector[j] = array[j, i];
            return vector;
        }

        public Vector GetColumn(int i)
        {
            var vector = new Vector(Height);
            for (int j = 0; j < Height; j++)
                vector[j] = array[i, j];
            return vector;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.Width != b.Width || a.Height != b.Height)
                throw new Exception("different length of vectors");
            var matrix = new Matrix(a.Width, a.Height);
            for (int i = 0; i < a.Width; i++)
                for (int j = 0; j < a.Height; j++)
                    matrix[i, j] = a[i, j] + b[i, j];
            return matrix;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.Width != b.Width || a.Height != b.Height)
                throw new Exception("different length of vectors");
            var matrix = new Matrix(a.Width, a.Height);
            for (int i = 0; i < a.Width; i++)
                for (int j = 0; j < a.Height; j++)
                    matrix[i, j] = a[i, j] - b[i, j];
            return matrix;
        }

        public static Matrix operator *(Matrix a, double b)
        {
            var matrix = new Matrix(a.Width, a.Height);
            for (int i = 0; i < a.Width; i++)
                for (int j = 0; j < a.Height; j++)
                    matrix[i, j] = a[i, j] * b;
            return matrix;
        }

        public static Matrix operator /(Matrix a, double b)
        {
            var matrix = new Matrix(a.Width, a.Height);
            for (int i = 0; i < a.Width; i++)
                for (int j = 0; j < a.Height; j++)
                    matrix[i, j] = a[i, j] / b;
            return matrix;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if(a.Width != b.Height)
                throw new Exception("different length of vectors");
            var matrix = new Matrix(b.Width, a.Height);
            for (int i = 0; i < matrix.Width; i++)
                for (int j = 0; j < matrix.Height; j++)
                    matrix[i, j] = a.GetRow(j) * b.GetColumn(i);
            return matrix;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("( ");
            for (int i = 0; i < Height; i++)
            {
                sb.Append(i);
                sb.Append(": ( ");
                for (int j = 0; j < Width; j++)
                {
                    sb.Append(array[i, j]);
                    if (j + 1 < Width)
                        sb.Append(", ");
                }
                sb.Append(" )");
                if (i + 1 < Height)
                    sb.Append(", ");
            }
            sb.Append(" )");
            return sb.ToString();
        }

        public Matrix Inverse()
        {
            if (Width != Height)
                return null;
            var det = Determinant();
            var preInverse = new Matrix(Width, Height);
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    var m = new Matrix(Width - 1, Height - 1);
                    for (int k = 0; k < Width; k++)
                        if (i != k)
                            for (int l = 0; l < Height; l++)
                                if (j != l)
                                    m[k - (k > i ? 1 : 0), l - (l > j ? 1 : 0)] = this[k, l];
                    preInverse[i, j] = m.Determinant();
                }
            for (int i = 0; i < Height; i++)
                for (int j = (i & 1) == 0 ? 1 : 0; j < Width; preInverse[j, i] = -preInverse[j, i], j += 2) ;
            var inverse = new Matrix(Width, Height);
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; inverse[i, j] = preInverse[j, i], j++) ;
            return inverse / det;
        }

        public double Determinant()
        {
            if (Width != Height)
                return 0;
            if (Width == 1 && Height == 1)
                return array[0, 0];
            if (Width == 2 && Height == 2)
                return array[0, 0] * array[1, 1] - array[1, 0] * array[0, 1];
            double det = 0;
            for (int i = 0; i < Width; i++)
            {
                var m = new Matrix(Width - 1, Height - 1);
                for (int j = 0; j < Width; j++)
                    if (i != j)
                        for (int k = 0; k < Height - 1; k++)
                            m[j - (j > i ? 1 : 0), k] = this[j, k + 1];
                det += (m.Determinant() * ((i & 1) == 0 ? 1 : -1) * this[i, 0]);
            }
            return det;
        }

        public static implicit operator Matrix(double[,] array) => new Matrix() { array = array };

        public static implicit operator Matrix(Vector vector)
        {
            var v = new double[vector.Length, 1];
            for (int i = 0; i < vector.Length; i++)
                v[i, 0] = vector[i];
            return v;
        }

        public static implicit operator string(Matrix matrix) => matrix.ToString();

        public static implicit operator double[,] (Matrix matrix) => matrix.array;
    }

    public class Matrix2
    {
        Array<double> array;

        public int Width => array.GetLength(0);
        public int Height => array.GetLength(1);

        public Matrix2() { }

        public Matrix2(int width, int height) => array = new Array<double>(width, height);

        public Matrix2(Matrix2 matrix) => array = matrix.array.Clone();

        public Matrix2 Clone() => new Matrix2(this);

        public double this[int i, int j]
        {
            get => i < Width && j < Height ? array.Get(i, j) : throw new IndexOutOfRangeException();
            set => array.Set(i < Width && j < Height ? value : throw new IndexOutOfRangeException(), i, j);
        }

        public Vector GetRow(int i)
        {
            var vector = new Vector(Width);
            for (int j = 0; j < Width; j++)
                vector[j] = array.Get(j, i);
            return vector;
        }

        public Vector GetColumn(int i)
        {
            var vector = new Vector(Height);
            for (int j = 0; j < Height; j++)
                vector[j] = array.Get(i, j);
            return vector;
        }

        public static Matrix2 operator +(Matrix2 a, Matrix2 b)
        {
            if (a.Width != b.Width || a.Height != b.Height)
                throw new Exception("different length of vectors");
            var matrix = new Matrix2(a.Width, a.Height);
            for (int i = 0; i < a.Width; i++)
                for (int j = 0; j < a.Height; j++)
                    matrix[i, j] = a[i, j] + b[i, j];
            return matrix;
        }

        public static Matrix2 operator -(Matrix2 a, Matrix2 b)
        {
            if (a.Width != b.Width || a.Height != b.Height)
                throw new Exception("different length of vectors");
            var matrix = new Matrix2(a.Width, a.Height);
            for (int i = 0; i < a.Width; i++)
                for (int j = 0; j < a.Height; j++)
                    matrix[i, j] = a[i, j] - b[i, j];
            return matrix;
        }

        public static Matrix2 operator *(Matrix2 a, double b)
        {
            var matrix = new Matrix2(a.Width, a.Height);
            for (int i = 0; i < a.Width; i++)
                for (int j = 0; j < a.Height; j++)
                    matrix[i, j] = a[i, j] * b;
            return matrix;
        }

        public static Matrix2 operator /(Matrix2 a, double b)
        {
            var matrix = new Matrix2(a.Width, a.Height);
            for (int i = 0; i < a.Width; i++)
                for (int j = 0; j < a.Height; j++)
                    matrix[i, j] = a[i, j] / b;
            return matrix;
        }

        public static Matrix2 operator *(Matrix2 a, Matrix2 b)
        {
            if (a.Width != b.Height)
                throw new Exception("different length of vectors");
            var matrix = new Matrix2(b.Width, a.Height);
            for (int i = 0; i < matrix.Width; i++)
                for (int j = 0; j < matrix.Height; j++)
                    matrix[i, j] = a.GetRow(j) * b.GetColumn(i);
            return matrix;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("( ");
            for (int i = 0; i < Height; i++)
            {
                sb.Append(i);
                sb.Append(": ( ");
                for (int j = 0; j < Width; j++)
                {
                    sb.Append(array.Get(i, j));
                    if (j + 1 < Width)
                        sb.Append(", ");
                }
                sb.Append(" )");
                if (i + 1 < Height)
                    sb.Append(", ");
            }
            sb.Append(" )");
            return sb.ToString();
        }

        public Matrix2 Inverse()
        {
            if (Width != Height)
                return null;
            var det = Determinant();
            var preInverse = new Matrix2(Width, Height);
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    var m = new Matrix2(Width - 1, Height - 1);
                    for (int k = 0; k < Width; k++)
                        if (i != k)
                            for (int l = 0; l < Height; l++)
                                if (j != l)
                                    m[k - (k > i ? 1 : 0), l - (l > j ? 1 : 0)] = this[k, l];
                    preInverse[i, j] = m.Determinant();
                }
            for (int i = 0; i < Height; i++)
                for (int j = (i & 1) == 0 ? 1 : 0; j < Width; preInverse[j, i] = -preInverse[j, i], j += 2) ;
            var inverse = new Matrix2(Width, Height);
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; inverse[i, j] = preInverse[j, i], j++) ;
            return inverse / det;
        }

        public double Determinant()
        {
            if (Width != Height)
                return 0;
            if (Width == 1 && Height == 1)
                return array.Get(0,0);
            if (Width == 2 && Height == 2)
                return array.Get(0, 0) * array.Get(1, 1) - array.Get(1, 0) * array.Get(0, 1);
            double det = 0;
            for (int i = 0; i < Width; i++)
            {
                var m = new Matrix2(Width - 1, Height - 1);
                for (int j = 0; j < Width; j++)
                    if (i != j)
                        for (int k = 0; k < Height - 1; k++)
                            m[j - (j > i ? 1 : 0), k] = this[j, k + 1];
                det += (m.Determinant() * ((i & 1) == 0 ? 1 : -1) * this[i, 0]);
            }
            return det;
        }

        public static implicit operator Matrix2(Vector vector)
        {
            var v = new Matrix2(vector.Length, 1);
            for (int i = 0; i < vector.Length; i++)
                v[i, 0] = vector[i];
            return v;
        }

        public static implicit operator string(Matrix2 matrix) => matrix.ToString();
    }
}