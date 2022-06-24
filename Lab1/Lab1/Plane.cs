using System.Drawing;

namespace Lab1
{
    public class Plane : ITraceable
    {
        protected readonly Point a;
        protected readonly Point b;
        protected readonly Point c;
        protected readonly Vector3D normal;
        protected IMaterial material;

        public Plane(Point a, Point b, Point c, Vector3D v = null, IMaterial m = null)
        {
            this.a = new Point(a);
            this.b = new Point(b);
            this.c = new Point(c);
            normal = (v is null) ? Vector3D.Normalize(Vector3D.CrossProduct(new Vector3D(a, b), new Vector3D(a, c))) : v;
            material = (m is null) ? new Lambert() : m;
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
            Vector3D d = ray.GetDirection();
            Vector3D k = new Vector3D(ray.GetPosition(), c);

            if (d * normal != 0)
            {
                float t = (k * normal) / (d * normal);
                return t > 0 ? (ray.GetPosition() + d * t) : null;
            }

            return null;
        }

        public Vector3D GetNormalAtPoint(Point point) => normal;

        public Color GetColorAtPoint(Beam startRay, Point interPoint, List<ITraceable> objects, List<Light> lights)
        {
            return material.RayBehaviour(startRay, interPoint, this, objects, lights);
        }
    }
}
