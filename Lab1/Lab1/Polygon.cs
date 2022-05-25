using System.Drawing;

namespace Lab1
{
    public class Polygon : Plane
    {
        private readonly Color color;

        public Polygon(Point a, Point b, Point c) : base(a, b, c)
        {
            color = Color.White;
        }

        public Polygon(Point a, Point b, Point c, Color color) : base(a, b, c)
        {
            this.color = color;
        }

        public Polygon(Polygon polygon) : base(polygon)
        {
            if (polygon == null)
            {
                throw new ArgumentNullException(nameof(polygon), "Trying to copy a null polygon object!");
            }

            color = polygon.color;
        }

        public Polygon Rotate(float alpha = 0, float beta = 0, float gamma = 0) =>
            new(a.Rotate(alpha, beta, gamma), b.Rotate(alpha, beta, gamma), c.Rotate(alpha, beta, gamma), color);

        public override Point? GetIntersectionPoint(Beam ray)
        {
            float e = 0.0000001f;
            Vector3D AB = new Vector3D(base.a, base.b);
            Vector3D AC = new Vector3D(base.a, base.c);
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
            else
                return null;
        }
    }
}