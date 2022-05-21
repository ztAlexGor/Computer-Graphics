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

            if(D >= 0)
            {
                float x1 = -(b + Math.Sqrt(D)) / (2 * a);
                float x2 = -(b - Math.Sqrt(D)) / (2 * a);
                if(x1 > 0)
                    return o + (d * x1);
                else if(x2 > 0)
                    return o + (d * x2);
            }
            return null;
        }

        public Vector3D GetNormalAtPoint(Point point) => new(center, point);
    }
}