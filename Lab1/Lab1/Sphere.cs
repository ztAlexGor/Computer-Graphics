using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Sphere : ISimpleObject
    {
        private readonly float radius;
        private readonly Point center;
        public Sphere(Point center, float radius)
        {
            radius = r;
            center = c;
        }
        public bool IntersectsWith(Point viewPoint, Vector3D viewRay)
        {
            Vector3D d = new(viewRay);
            Point o = new(viewPoint);
            Vector3D k = new(o, center);

            float a = d * d;
            float b = d * 2 * k;
            float c = k * k - radius * radius;

            float D = (b * b) - (4 * a * c);

            return (D >= 0) && (Math.Sqrt(D) * b / (2 * a) > 0);
        }
        public Point GetCenter() { return center; }
        public float GetRadius() { return radius; }
    }
}
