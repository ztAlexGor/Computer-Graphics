using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Polygon : Plane
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
            Vector3D vector = ray.GetDirection();
            float d = -(normal.X() * a.X() + normal.Y() * a.Y() + normal.Z() * a.Z());
            float sumt = normal.X() * vector.X() + normal.Y() * vector.Y() + normal.Z() * vector.Z();
            if (sumt == 0)
            {
                return null;
            }

            float t = -(sumt + d) / sumt;

            Point intersectionPoint = new(vector.X() * (t + 1), vector.Y() * (t + 1), vector.Z() * (t + 1));
            if (intersectionPoint.Z() < ray.GetPosition().Z())
            {
                return null;
            }

            // Barycentric coordinates calculation
            float polygonArea = (a.Y() - c.Y()) * (b.X() - c.X()) + (b.Y() - c.Y()) * (c.X() - a.X());
            float b1 = ((intersectionPoint.Y() - c.Y()) * (b.X() - c.X()) + 
                (b.Y() - c.Y()) * (c.X() - intersectionPoint.X())) / polygonArea;
            float b2 = ((intersectionPoint.Y() - a.Y()) * (c.X() - a.X()) + 
                (c.Y() - a.Y()) * (a.X() - intersectionPoint.X())) / polygonArea;
            float b3 = ((intersectionPoint.Y() - b.Y()) * (a.X() - b.X()) + 
                (a.Y() - b.Y()) * (b.X() - intersectionPoint.X())) / polygonArea;

            // If the intersection point is inside the triangle - returning it, otherwise returning null
            return (b1 >= 0 && b1 <= 1 && b2 >= 0 && b2 <= 1 && b3 >= 0 && b3 <= 1) ? intersectionPoint : null;
        }
    }
}
