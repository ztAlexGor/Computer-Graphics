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
            this.a = new Point(a);
            this.b = new Point(b);
            this.c = new Point(c);
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
            Point rayPos = ray.GetPosition();
            float d = -(normal.X() * a.X() + normal.Y() * a.Y() + normal.Z() * a.Z());
            float sumt = normal.X() * rayPos.X() + normal.Y() * rayPos.Y() + normal.Z() * rayPos.Z();
            if (sumt == 0)
            {
                return null;
            }
            
            float t = -(sumt + d) / sumt;

            Point intersectionPoint = new(rayPos.X() * (t + 1), rayPos.Y() * (t + 1), rayPos.Z() * (t + 1));

            return (intersectionPoint.Z() >= ray.GetPosition().Z()) ? intersectionPoint : null;
        }

        public Vector3D GetNormalAtPoint(Point point) => normal;
    }
}
