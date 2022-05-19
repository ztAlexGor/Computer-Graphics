using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class MathUtils
    {
        public static Matrix GetTransformationMatrix(float a, float b, float g)
        {
            double[][] matrix = new double[4][];
            for (int i = 0; i < 4; i++)
                matrix[i] = new double[4];

            Matrix transformX = new Matrix(new double[]{
                Math.Cos(a), 0, -Math.Sin(a), 0,
                0, 1, 0, 0,
                Math.Sin(a), 0, Math.Cos(a), 0,
                0, 0, 0, 1
            }, 4, 4);

            Matrix transformY = new Matrix(new double[]{
                1, 0, 0, 0,
                0, Math.Cos(b), Math.Sin(b), 0,
                0, -Math.Sin(b), Math.Cos(b), 0,
                0, 0, 0, 1
            }, 4, 4);

            Matrix transformZ = new Matrix(new double[]{
                Math.Cos(g), -Math.Sin(g), 0, 0,
                Math.Sin(g), Math.Cos(g), 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            }, 4, 4);

            Matrix transformDist = new Matrix(new double[]{
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, -600, 1
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
    }
}
