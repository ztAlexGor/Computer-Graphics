﻿namespace Lab1
{
    public class Vector3D : Point
    {
        private readonly float length;
        private readonly float sqrLength;

        public Vector3D(float x, float y, float z) : base(x, y, z, 0)
        {
            sqrLength = x * x + y * y + z * z;
            length = (float)Math.Sqrt(sqrLength);
        }

        public Vector3D(float?[] arr) : base(arr)
        {
            sqrLength = x * x + y * y + z * z;
            length = (float)Math.Sqrt(sqrLength);
        }

        public Vector3D(Point start, Point end)
        {
            x = end.X() - start.X();
            y = end.Y() - start.Y();
            z = end.Z() - start.Z();
            sqrLength = x * x + y * y + z * z;
            length = (float)Math.Sqrt(sqrLength);
        }

        public Vector3D(Vector3D a)
        {
            x = a.X();
            y = a.Y();
            z = a.Z();
            sqrLength = a.sqrLength;
            length = a.length;
        }

        public Vector3D(Point a)
        {
            x = a.X();
            y = a.Y();
            z = a.Z();
            sqrLength = x * x + y * y + z * z;
            length = (float)Math.Sqrt(sqrLength);
        }

        public float Length() => length;

        public float SquareLength() => sqrLength;

        public override Point GetMultipliedByMatrix(Matrix matrix)
        {
            float[][] vals = matrix.Values();
            float[] pointVals = { x, y, z, w };
            float[] newVals = new float[matrix.N];
            for (int i = 0; i < matrix.N; i++)
            {
                newVals[i] = 0;
                for (int j = 0; j < matrix.M; j++)
                {
                    newVals[i] += vals[i][j] * pointVals[j];
                }
            }
            return new Vector3D(newVals[0], newVals[1], newVals[2]);
        }

        public override Vector3D Rotate(float alpha, float beta, float gamma) => (Vector3D)MathUtils.GetRotatedPoint(this, alpha, beta, gamma);

        public override Vector3D Rotate(Vector3D angles) => (Vector3D)MathUtils.GetRotatedPoint(this, angles.X(), angles.Y(), angles.Z());

        public override Vector3D Scale(float sx, float sy, float sz) => (Vector3D)MathUtils.GetScaledPoint(this, sx, sy, sz);

        public static Vector3D operator +(Vector3D a, Vector3D b) => new(a.x + b.x, a.y + b.y, a.z + b.z);

        public static Vector3D operator -(Vector3D a, Vector3D b) => new(a.x - b.x, a.y - b.y, a.z - b.z);

        public static Vector3D operator -(Vector3D a) => new(-a.x, -a.y, -a.z);

        public static float operator *(Vector3D a, Vector3D b) => a.x * b.x + a.y * b.y + a.z * b.z;

        public static Vector3D operator *(Vector3D v, float a) => new(v.x * a, v.y * a, v.z * a);

        public static Vector3D operator /(Vector3D v, float a) => new(v.x / a, v.y / a, v.z / a);

        public static Vector3D CrossProduct(Vector3D a, Vector3D b) => new(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);

        public static Vector3D Normalize(Vector3D v) => v / v.length;

        public static bool operator !=(Vector3D a, Vector3D b) => (a.x != b.x) || (a.y != b.y) || (a.z != b.z);

        public static bool operator ==(Vector3D a, Vector3D b) => !(a != b);

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Vector3D p = (Vector3D)obj;
                return (x == p.x) && (y == p.y) && (z == p.z);
            }
        }

        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
    }
}
