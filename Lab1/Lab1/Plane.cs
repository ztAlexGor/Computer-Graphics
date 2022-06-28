using System.Drawing;

namespace Lab1
{
    public class Plane : ITraceable
    {
        protected readonly Point a;
        protected readonly Point b;
        protected readonly Point c;
        protected readonly Vector3D normal;
        protected Material material;
        protected readonly Color color;
        protected AABB aabb;

        public Plane(Point a, Point b, Point c, Color color, Vector3D v = null, Material m = null)
        {
            this.a = new Point(a);
            this.b = new Point(b);
            this.c = new Point(c);
            normal = (v is null) ? Vector3D.Normalize(Vector3D.CrossProduct(new Vector3D(a, b), new Vector3D(a, c))) : v;
            material = (m is null) ? new Lambert() : m;
            this.color = color;
            aabb = new AABB(BoxBordersInit());
        }
        public Plane(Point a, Point b, Point c, Vector3D v = null, Material m = null)
        {
            this.a = new Point(a);
            this.b = new Point(b);
            this.c = new Point(c);
            normal = (v is null) ? Vector3D.Normalize(Vector3D.CrossProduct(new Vector3D(a, b), new Vector3D(a, c))) : v;
            material = (m is null) ? new Lambert() : m;
            color = Color.White;
            aabb = new AABB(new float[] { float.MaxValue, float.MinValue,
                                          float.MaxValue, float.MinValue,
                                          float.MaxValue, float.MinValue });
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
            color = plane.color;
            material = plane.material;
            normal = Vector3D.Normalize(Vector3D.CrossProduct(new Vector3D(a, b), new Vector3D(a, c)));
            aabb = plane.GetAABB();
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

        public Color GetColorAtPoint(Beam startRay, Point interPoint, BVHTree tree, List<Light> lights) =>
            material.RayBehaviour(startRay, interPoint, this, tree, lights);

        public virtual ITraceable Rotate(float alpha = 0, float beta = 0, float gamma = 0) =>
            new Plane(a.Rotate(alpha, beta, gamma), b.Rotate(alpha, beta, gamma), c.Rotate(alpha, beta, gamma), color);

        public virtual ITraceable Scale(float sx = 0, float sy = 0, float sz = 0) => new Plane(this);

        public virtual ITraceable Translate(float x = 0, float y = 0, float z = 0) =>
            new Plane(a.Translate(x, y, z), b.Translate(x, y, z), c.Translate(x, y, z), color);

        public float[] GetBoxBorders() => aabb.GetBorders();

        public float[] GetBoxCenter() => aabb.GetCenter();

        public AABB GetAABB() => aabb;

        public virtual float[] BoxBordersInit() =>
            new float[] { float.MaxValue, float.MinValue,
                          float.MaxValue, float.MinValue,
                          float.MaxValue, float.MinValue };

        public virtual Point? GetUV() => null;

        public virtual (Point?, Point?, Point?) GetVT() => (null, null, null);
    }
}
