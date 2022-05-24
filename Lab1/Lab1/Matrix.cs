namespace Lab1
{
    internal class Matrix
    {
        private readonly float[][] values;
        public readonly int N;
        public readonly int M;

        public Matrix(float[][] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            N = values.Length;
            M = values[0].Length;
            this.values = new float[N][];
            for (int i = 0; i < N; i++)
            {
                this.values[i] = new float[M];
                for (int j = 0; j < M; j++)
                {
                    this.values[i][j] = values[i][j];
                }
            }
        }

        public Matrix(float[] values, int n, int m)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            N = n;
            M = m;
            this.values = new float[N][];
            for (int i = 0; i < N; i++)
            {
                this.values[i] = new float[M];
                for (int j = 0; j < M; j++)
                {
                    this.values[i][j] = values[i * N + j];
                }
            }
        }

        public float[][] Values() => this.values;

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            float[][] vals1 = matrix1.Values();
            float[][] vals2 = matrix2.Values();
            float[][] res = new float[matrix1.N][];
            for (int i = 0; i < matrix1.N; i++)
            {
                res[i] = new float[matrix2.M];
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
