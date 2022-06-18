namespace Lab1
{
    public class Point
    {
        protected float x;
        protected float y;
        protected float z;

        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point(Point a)
        {
            x = a.x;
            y = a.y;
            z = a.z;
        }

        public Point()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Point(float[] arr)
        {
            x = arr[0];
            y = arr[1];
            z = arr[2];
        }

        public float X() => x;

        public float Y() => y;

        public float Z() => z;

        public void Simplify()
        {
            x = (float)Math.Round(x);
            y = (float)Math.Round(y);
            z = (float)Math.Round(z);
        }
        
        public Point Rotate(float alpha, float beta, float gamma) => MathUtils.GetRotatedVector(this, alpha, beta, gamma);

        public static bool operator !=(Point a, Point b) => (a.x != b.x) || (a.y != b.y) || (a.z != b.z);

        public static bool operator ==(Point a, Point b) => !(a != b);

        public static Point operator +(Point a, Vector3D b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
        
        public static Point operator -(Point a, Vector3D b) => new(a.x - b.x, a.y - b.y, a.z - b.z);

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Point p = (Point)obj;
                return (x == p.x) && (y == p.y) && (z == p.z);
            }
        }

        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();

    }
}
