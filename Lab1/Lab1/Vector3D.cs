using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Vector3D
    {
        private float x;
        private float y;
        private float z;
        private readonly float length;
        private readonly float sqrLength;

        public Vector3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            sqrLength = x * x + y * y + z * z;
            length = (float)Math.Sqrt(sqrLength);
        }

        public Vector3D(Point a, Point b)
        {
            x = b.X() - a.X();
            y = b.Y() - a.Y();
            z = b.Z() - a.Z();
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

        public float X() => x;

        public float Y() => y;

        public float Z() => z;

        public float Length() => length;

        public float SquareLength() => sqrLength;

        public static Vector3D operator +(Vector3D a, Vector3D b) => new(a.x + b.x, a.y + b.y, a.z + b.z);

        public static Vector3D operator -(Vector3D a, Vector3D b) => new(a.x - b.x, a.y - b.y, a.z - b.z);

        public static float operator *(Vector3D a, Vector3D b) => a.x * b.x + a.y * b.y + a.z * b.z;

        public static Vector3D operator *(Vector3D v, float a) => new(v.x * a, v.y * a, v.z * a);

        public static Vector3D operator /(Vector3D v, float a) => new(v.x / a, v.y / a, v.z / a);

        public static Vector3D CrossProduct(Vector3D a, Vector3D b) => new(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);

        public static Vector3D Normalize(Vector3D v) => v / v.length;

        public static bool operator !=(Vector3D a, Vector3D b) => (a.x != b.x) || (a.y != b.y) || (a.z != b.z);

        public static bool operator ==(Vector3D a, Vector3D b) => !(a != b);

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
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
