namespace Lab1
{
    public class Point
    {
        protected float x;
        protected float y;
        protected float z;
        protected float w;

        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            w = 1;
        }
        public Point(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Point(Point a)
        {
            x = a.x;
            y = a.y;
            z = a.z;
            w = 1;
        }

        public Point()
        {
            x = 0;
            y = 0;
            z = 0;
            w = 1;
        }

        public Point(float[] arr)
        {
            x = arr[0];
            y = arr[1];
            z = arr[2];
            w = 1;
        }

        public float X() => x;

        public float Y() => y;

        public float Z() => z;

        public float W() => w;

        public void Simplify()
        {
            x = (float)Math.Round(x);
            y = (float)Math.Round(y);
            z = (float)Math.Round(z);
        }

        public virtual Point GetMultipliedByMatrix(Matrix matrix)
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
            return new Point(newVals[0], newVals[1], newVals[2], newVals[3]);
        }
        
        public virtual Point Rotate(float alpha, float beta, float gamma) => MathUtils.GetRotatedPoint(this, alpha, beta, gamma);

        public virtual Point Rotate(Vector3D angles) => MathUtils.GetRotatedPoint(this, angles.X(), angles.Y(), angles.Z());

        public virtual Point Scale(float sx, float sy, float sz) => MathUtils.GetScaledPoint(this, sx, sy, sz);

        public virtual Point Translate(float x, float y, float z) => new(this.x + x, this.y + y, this.z + z);
        public static bool operator !=(Point a, Point b) => (a.x != b.x) || (a.y != b.y) || (a.z != b.z);

        public static bool operator ==(Point a, Point b) => !(a != b);

        public static Point operator +(Point a, Vector3D b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
        
        public static Point operator -(Point a, Vector3D b) => new(a.x - b.x, a.y - b.y, a.z - b.z);

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
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
