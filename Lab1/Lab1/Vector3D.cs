namespace Lab1
{
    internal class Vector3D : Point
    {
        private readonly float length;
        private readonly float sqrLength;

        public Vector3D(float x, float y, float z) : base(x, y, z)
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
