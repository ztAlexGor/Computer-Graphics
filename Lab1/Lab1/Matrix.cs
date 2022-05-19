using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Matrix
    {
        private readonly double[][] values;
        public readonly int N;
        public readonly int M;

        public Matrix(double[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            N = values.Length;
            M = values[0].Length;
            this.values = new double[N][];
            for (int i = 0; i < N; i++)
            {
                this.values[i] = new double[M];
                for (int j = 0; j < M; j++)
                {
                    this.values[i][j] = values[i][j];
                }
            }
        }

        public Matrix(double[] values, int n, int m)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            N = n;
            M = m;
            this.values = new double[N][];
            for (int i = 0; i < N; i++)
            {
                this.values[i] = new double[M];
                for (int j = 0; j < M; j++)
                {
                    this.values[i][j] = values[i * N + j];
                }
            }
        }

        public double[][] Values() => this.values;

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            double[][] vals1 = matrix1.Values();
            double[][] vals2 = matrix2.Values();
            double[][] res = new double[matrix1.N][];
            for (int i = 0; i < matrix1.N; i++)
            {
                res[i] = new double[matrix2.M];
                for (int j = 0; j < matrix2.M; j++)
                {
                    res[i][j] = 0;
                    for (int k = 0; k < matrix2.M; k++)
                    {
                        res[i][j] += vals1[i][k] * vals2[k][j];
                    }
                }
            }
            return new Matrix(res);
        }
    }
}
