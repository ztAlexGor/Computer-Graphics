using System.Drawing;

namespace Lab1
{
    public class Polygon : Plane
    {
        private float[] boxBorders;
        public Polygon(Point a, Point b, Point c) : base(a, b, c, Color.White) { BoxBordersInit(); }

        public Polygon(Point a, Point b, Point c, Vector3D v) : base(a, b, c, Color.White, v) { BoxBordersInit(); }

        public Polygon(Point a, Point b, Point c, Color color) : base(a, b, c, color) { BoxBordersInit(); }

        public Polygon(Polygon polygon) : base(polygon)
        {
            if (polygon == null)
            {
                throw new ArgumentNullException(nameof(polygon), "Trying to copy a null polygon object!");
            }
        }

        public override ITraceable Rotate(float alpha = 0, float beta = 0, float gamma = 0) =>
            new Polygon(a.Rotate(alpha, beta, gamma), b.Rotate(alpha, beta, gamma), c.Rotate(alpha, beta, gamma), color);

        public override ITraceable Scale(float sx = 0, float sy = 0, float sz = 0) =>
            new Polygon(a.Scale(sx, sy, sz), b.Scale(sx, sy, sz), c.Scale(sx, sy, sz), color);

        public override ITraceable Translate(float x = 0, float y = 0, float z = 0) =>
            new Polygon(a.Translate(x, y, z), b.Translate(x, y, z), c.Translate(x, y, z), color);

        public override Point? GetIntersectionPoint(Beam ray)
        {
            float e = 0.00001f;
            Vector3D AB = new(base.a, base.b);
            Vector3D AC = new(base.a, base.c);
            Vector3D h = Vector3D.CrossProduct(ray.GetDirection(), AC);
            float a = AB * h;
            if (a > -e && a < e)
                return null;
            float f = 1.0f / a;
            Vector3D s = new(base.a, ray.GetPosition());
            float u = f * (s * h);
            if (u < 0.0f || u > 1.0f)
                return null;
            Vector3D q = Vector3D.CrossProduct(s, AB);
            float v = f * (ray.GetDirection() * q);
            if (v < 0.0f || u + v > 1.0f)
                return null;
            float t = f * (AC * q);
            if (t > e)
                return ray.GetPosition() + (ray.GetDirection() * t);
            return null;
        }

        private void BoxBordersInit()
        {
            boxBorders = new float[6];
            boxBorders[0] = MaxBorder(a.X(), b.X(), c.X());
            boxBorders[1] = MinBorder(a.X(), b.X(), c.X());
            boxBorders[2] = MaxBorder(a.Y(), b.Y(), c.Y());
            boxBorders[3] = MinBorder(a.Y(), b.Y(), c.Y());
            boxBorders[4] = MaxBorder(a.Z(), b.Z(), c.Z());
            boxBorders[5] = MinBorder(a.Z(), b.Z(), c.Z());
        }

        private float MaxBorder(float a, float b, float c)
        {
            if (a > b && a > c)
            {
                return a;
            }
            if (b > c)
                return b;

            return c;
        }
        
        private float MinBorder(float a, float b, float c)
        {
            if (a < b && a < c)
            {
                return a;
            }
            if (b < c)
                return b;

            return c;
        }
        public float[] GetBoxBorders()
        {
            return boxBorders;
        }
    }
}