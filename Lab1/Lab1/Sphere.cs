using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Sphere : ISimpleObject
    {
        private readonly float radius;
        private readonly Point center;

        public Sphere(Point c, float r)
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

        public List<Beam> GenerateRays()
        {
            List<Beam> rays = new();
            for (float x = center.X() - radius; x <= center.X(); x++)
            {
                for (float y = center.Y() - radius; x <= center.Y(); y++)
                {
                    double z = Math.Sqrt(radius * radius - (Math.Pow(x - center.X(), 2) + Math.Pow(y - center.Y(), 2)));
                    if (!double.IsNaN(z))
                    {
                        rays.Add(new Beam(new Point(center), new Vector3D(x, y, (float)z)));
                        rays.Add(new Beam(new Point(center), new Vector3D(x, y, (float)-z)));
                    }
                    
                }
            }
            return rays;
        }
    }
}