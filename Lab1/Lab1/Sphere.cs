using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Sphere : ITraceable
    {
        private readonly float radius;
        private readonly Point center;

        public Sphere(Point c, float r)
        {
            radius = r;
            center = c;
        }

        public Point? GetIntersectionPoint(Beam ray)
        {
            Vector3D d = new(ray.GetDirection());
            Point o = new(ray.GetPosition());
            Vector3D k = new(o, center);

            float a = d * d;
            float b = d * 2 * k;
            float c = k * k - radius * radius;

            float D = (b * b) - (4 * a * c);

            if (D < 0)
            {
                return null;
            }

            float x1 = -(b + (float)Math.Sqrt(D)) / (2.0f * a);
            float x2 = -(b - (float)Math.Sqrt(D)) / (2.0f * a);
            return (x1 > 0) ? (o + (d * x1)) : ((x2 > 0) ? (o + (d * x2)) : null);
        }

        public Vector3D GetNormalAtPoint(Point point) => new(center, point);
    }
}