using System;
using System.Text;

namespace DLib.Math
{
    public class Matrix
    {
        double[,] matrix;

        public int Width => matrix.GetLength(0);
        public int Height => matrix.GetLength(1);

        public Matrix(int width, int height) => matrix = new double[width, height];

        public double this[int i, int j]
        {
            get => i < Width && j < Height ? matrix[i, j] : throw new IndexOutOfRangeException();
            set
            {
                if (i < Width && j < Height)
                    matrix[i, j] = value;
                else
                    throw new IndexOutOfRangeException();
            }
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

        public static Matrix operator *(Matrix a, Matrix b)
        {
            var matrix = new Matrix(b.Width, a.Height);
            for (int i = 0; i < matrix.Width; i++)
                for (int j = 0; j < matrix.Height; j++)
                    matrix[i, j] = a.GetRow(j) * b.GetColumn(i);
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

        public Vector GetRow(int i)
        {
            Vector vector = new Vector(Width);
            for (int j = 0; j < vector.Length; j++)
                vector[j] = matrix[j, i];
            return vector;
        }

        public Vector GetColumn(int i)
        {
            Vector vector = new Vector(Height);
            for (int j = 0; j < vector.Length; j++)
                vector[j] = matrix[i, j];
            return vector;
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
                    sb.Append(matrix[j, i]);
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
            Matrix inverse = new Matrix(Width, Height);
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    Matrix m = new Matrix(Width - 1, Height - 1);
                    for (int k = 0; k < Width; k++)
                        if (i != k)
                            for (int l = 0; l < Height; l++)
                                if (j != l)
                                    m[k - (k > i ? 1 : 0), l - (l > j ? 1 : 0)] = this[k, l];
                    inverse[i, j] = m.Determinant();
                }
            for (int i = 0; i < Height; i++)
                for (int j = (i & 1) == 0 ? 1 : 0; j < Width; inverse[j, i] = -inverse[j, i], j += 2) ;
            Matrix inverse2 = new Matrix(Width, Height);
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; inverse2[i, j] = inverse[j, i], j++) ;
            return inverse2 / Determinant();
        }

        public double Determinant()
        {
            if (Width != Height)
                return 0;
            if (Width == 2 && Height == 2)
                return matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
            double d = 0;
            for (int i = 0; i < Width; i++)
            {
                Matrix m = new Matrix(Width - 1, Height - 1);
                for (int j = 0; j < Width; j++)
                    if (i != j)
                        for (int k = 0; k < Height - 1; k++)
                            m[j - (j > i ? 1 : 0), k] = this[j, k + 1];
                d += (m.Determinant() * ((i & 1) == 0 ? 1 : -1) * this[i, 0]);
            }
            return d;
        }
    }
}
