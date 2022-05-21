using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Plane : ITraceable
    {
        protected readonly Point a;
        protected readonly Point b;
        protected readonly Point c;
        protected readonly Vector3D normal;

        public Plane(Point a, Point b, Point c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            normal = Vector3D.Normalize(Vector3D.CrossProduct(new Vector3D(a, b), new Vector3D(a, c)));
        }

        public Plane(Plane plane)
        {
            if (plane == null)
            {
                throw new ArgumentNullException(nameof(plane), "Trying to copy a null plain/polygon object!");
            }
            a = plane.a;
            b = plane.b;
            c = plane.c;
            normal = Vector3D.Normalize(Vector3D.CrossProduct(new Vector3D(a, b), new Vector3D(a, c)));
        }

        public virtual Point? GetIntersectionPoint(Beam ray)
        {
            // And I've just realized it's pointless, because we have a normal vector
            // (float[], bool) res = MathUtils.SolveSoLE(MathUtils.CreatePlainMatrix(a, b, c));
            // if (!res.Item2)
            // {
            //     return false;
            // }
            Vector3D vector = ray.GetDirection();
            float d = -(normal.X() * a.X() + normal.Y() * a.Y() + normal.Z() * a.Z());
            float sumt = normal.X() * vector.X() + normal.Y() * vector.Y() + normal.Z() * vector.Z();
            if (sumt == 0)
            {
                return null;
            }
            
            float t = -(sumt + d) / sumt;

            Point intersectionPoint = new(vector.X() * (t + 1), vector.Y() * (t + 1), vector.Z() * (t + 1));

            return (intersectionPoint.Z() >= ray.GetPosition().Z()) ? intersectionPoint : null;
        }

        public Vector3D GetNormalAtPoint(Point point) => normal;
    }
}
