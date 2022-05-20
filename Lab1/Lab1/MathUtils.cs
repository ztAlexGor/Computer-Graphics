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

        public static (float[], bool) SolveSoLE(Matrix matrix)
        {
            static bool ApplyMaskAndCheck(float[] row, bool[] mask)
            {
                bool unknownValueFound = false;
                for (int i = 0; i < mask.Length; i++)
                {
                    if (((mask[i]) ? 0 : 1) * row[i] != 0)
                    {
                        if (unknownValueFound)
                        {
                            return false;
                        }
                        unknownValueFound = true;
                    }
                }
                return unknownValueFound;
            }
            // Init matrix vals and create updated array with
            // track of leading zeroes to impove performance
            float[][] matrixVals = matrix.Values();
            float[][] values = new float[matrixVals.Length][];
            int n = matrixVals.Length;
            int m = matrixVals[0].Length;
            for (int i = 0; i < n; i++)
            {
                values[i] = new float[m + 1];
                int leadingZeroesNumber = 0;
                bool metNonZero = false;
                for (int j = 0; j < m; j++)
                {
                    if (!metNonZero && j < m - 1)
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
                    values[i][j] = matrixVals[i][j];
                }
                values[i][m] = leadingZeroesNumber;
            }

            float[] solutions = new float[m - 1];
            bool[] mask = new bool[m - 1];
            for (int i = 0; i < m - 1; i++)
            {
                mask[i] = false;
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
                    bool metNonZero = false;
                    for (int j = 0; j < m; j++)
                    {
                        if (!metNonZero && j < m - 1)
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
                        values[row1][j] += multiplier * values[row2][j];
                    }
                    values[row1][m] = leadingZeroesNumber;
                }
                else
                {
                    k++;
                }
            }

            k = m - 1;
            int idx = 0;
            bool noSolution = false;
            while (k >= 0 && !noSolution)
            {
                while (idx < n && !ApplyMaskAndCheck(values[idx], mask) && idx++ >= 0) ;
                if (idx < n)
                {
                    float b = values[idx][m - 1];
                    int xi = 0;
                    for (int j = 0; j < m - 1; j++)
                    {
                        if (mask[j])
                        {
                            b += values[idx][j] * solutions[j];
                        }
                        else
                        {
                            if (values[idx][j] != 0)
                            {
                                xi = j;
                            }
                        }
                    }
                    solutions[xi] = -(b / values[idx][xi]);
                    mask[xi] = true;
                }
                else
                {
                    k--;
                }
            }
            return (solutions, mask.Count(x => x) == m - 1);
        }
    }
}
