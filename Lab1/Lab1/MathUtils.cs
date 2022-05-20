using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class MathUtils
    {
        public static Matrix CreatePlainMatrix(Point a, Point b, Point c) => new(new float[] { a.X(), a.Y(), a.Z(), 1.0f,
            b.X(), b.Y(), b.Z(), 1.0f,
            c.X(), c.Y(), c.Z(), 1.0f
        }, 3, 4);

        public static Matrix GetTransformationMatrix(float a, float b, float g)
        {
            double[][] matrix = new double[4][];
            for (int i = 0; i < 4; i++)
                matrix[i] = new double[4];

            Matrix transformX = new(new float[] {
                (float)Math.Cos(a), 0, -(float)Math.Sin(a), 0,
                0, 1.0f, 0, 0,
                (float)Math.Sin(a), 0, (float)Math.Cos(a), 0,
                0, 0, 0, 1.0f
            }, 4, 4);

            Matrix transformY = new(new float[] {
                1.0f, 0, 0, 0,
                0, (float)Math.Cos(b), (float)Math.Sin(b), 0,
                0, -(float)Math.Sin(b), (float)Math.Cos(b), 0,
                0, 0, 0, 1.0f
            }, 4, 4);

            Matrix transformZ = new(new float[] {
                (float)Math.Cos(g), -(float)Math.Sin(g), 0, 0,
                (float)Math.Sin(g), (float)Math.Cos(g), 0, 0,
                0, 0, 1.0f, 0,
                0, 0, 0, 1.0f
            }, 4, 4);

            Matrix transformDist = new(new float[] {
                1.0f, 0, 0, 0,
                0, 1.0f, 0, 0,
                0, 0, 1.0f, 0,
                0, 0, -600.0f, 1.0f
            }, 4, 4);

            return transformX * transformY * transformZ * transformDist;
        }

        // Idk, it has to be remade, because a point cannot be rotated
        public static Point GetRotatedVector(Point point, float a, float b, float g)
        {
            // TBD
            Matrix rotationMatrix = MathUtils.GetTransformationMatrix(a, b, g);
            return point;
        }

        public static float[] SolveSoLE(Matrix matrix)
        {
            int CountLeadingZeroes(float[] row)
            {
                int zeroesCounter = 0;
                for (int i = 0; i < row.Length - 1; i++)
                {
                    zeroesCounter += (row[i] == 0) ? 1 : 0;
                }
                return zeroesCounter;
            }
            // Init matrix vals and create updated array with
            // track of leading zeroes to impove performance
            float[][] matrixVals = matrix.Values();
            float[][] values = new float[matrixVals.Length][];
            int n = matrixVals.Length;
            int m = matrixVals[0].Length;
            for (int i = 0; i < n; i++)
            {
                values[i] = new float[m + 2];
                int leadingZeroesNumber = 0;
                int zeroesCounter = 0;
                bool metNonZero = false;
                for (int j = 0; j < m; j++)
                {
                    if (!metNonZero)
                    {
                        if (matrixVals[i][j] == 0)
                        {
                            leadingZeroesNumber++;
                        }
                        else
                        {
                            metNonZero = true;
                        }
                    }
                    zeroesCounter += (matrixVals[i][j] == 0) ? 1 : 0;
                    values[i][j] = matrixVals[i][j];
                }
                values[i][m] = leadingZeroesNumber;
                values[i][m + 1] = zeroesCounter;
            }

            float[] solutions = new float[m - 1];
            bool[] solutionsFlag = new bool[m - 1];
            for (int i = 0; i < m - 1; i++)
            {
                solutionsFlag[i] = false;
                solutions[i] = 0;
            }

            // legshooting starts here
            int k = 0;
            while (k < m)
            {
                int row1 = -1;
                int row2 = 0;
                for (int i = 0; i < n; i++)
                {
                    if (values[i][m] == k)
                    {
                        row2 = row1;
                        row1 = i;
                    }
                }
                if (row1 != -1 && row2 != -1)
                {
                    float multiplier = -(values[row1][k] / values[row2][k]);
                    int leadingZeroesNumber = 0;
                    int zeroesCounter = 0;
                    bool metNonZero = false;
                    for (int j = 0; j < m; j++)
                    {
                        if (!metNonZero)
                        {
                            if (matrixVals[row1][j] == 0)
                            {
                                leadingZeroesNumber++;
                            }
                            else
                            {
                                metNonZero = true;
                            }
                        }
                        zeroesCounter += (values[row1][j] == 0) ? 1 : 0;
                        values[row1][j] += multiplier * values[row2][j];
                    }
                    values[row1][m] = leadingZeroesNumber;
                    values[row1][m + 1] = zeroesCounter;
                }
                else
                {
                    k++;
                }
            }

            k = m - 1;
            int i = 0;
            bool noSolution = false;
            while (k >= 0 && !noSolution)
            {
                while (i < n && values[i][m + 1] != k && i++ >= 0) ;
                if (i == n)
                {
                    noSolution = true;
                }
                else
                {
                    // Solve the equation
                }
            }
        }
    }
}
