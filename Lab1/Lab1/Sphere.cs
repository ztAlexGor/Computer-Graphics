using System.Drawing;

namespace Lab1
{
    public class Sphere : ITraceable
    {
        private readonly float radius;
        private readonly Point center;
        protected Material material;
        protected readonly Color color;
        private readonly AABB aabb;

        public Sphere(Point c, float r, Color color, Material m = null)
        {
            radius = r;
            center = c;
            aabb = new AABB(BoxBordersInit());
            material = (m is null) ? new Lambert() : m;
            this.color = color;
        }

        public Point? GetIntersectionPoint(Beam ray)
        {
            Vector3D d = new(ray.GetDirection());
            Point o = new(ray.GetPosition());
            Vector3D k = new(center, o);

            float a = d * d;
            float b = d * 2 * k;
            float c = k * k - radius * radius;

            float D = (b * b) - (4 * a * c);

            if (D < 0)
            {
                return null;
            }

            float x1 = (-b + (float)Math.Sqrt(D)) / (2.0f * a);
            float x2 = (-b - (float)Math.Sqrt(D)) / (2.0f * a);
            return (x2 > 0) ? (o + (d * x2)) : ((x1 > 0) ? (o + (d * x1)) : null);
        }

        public Vector3D GetNormalAtPoint(Point point) => Vector3D.Normalize(new Vector3D(center, point));

        public Color GetColorAtPoint(Beam startRay, Point interPoint, BVHTree tree, List<Light> lights)
        {
            return material.RayBehaviour(startRay, interPoint, this, tree, lights);
        }

        public ITraceable Rotate(float alpha = 0, float beta = 0, float gamma = 0) =>
            new Sphere(center.Rotate(alpha, beta, gamma), radius, color, material);

        public ITraceable Scale(float sx = 0, float sy = 0, float sz = 0) =>
            new Sphere(center, radius * sx, color, material);

        public ITraceable Translate(float x = 0, float y = 0, float z = 0) =>
            new Sphere(center.Translate(x, y, z), radius, color, material);

        public float[] BoxBordersInit() => 
            new float[6] { center.X() + radius, center.X() - radius,
                           center.Y() + radius, center.Y() - radius,
                           center.Z() + radius, center.Z() - radius };

        public float[] GetBoxBorders() => aabb.GetBorders();
        public float[] GetBoxCenter() => aabb.GetCenter();
        public AABB GetAABB() => aabb;

        public Point? GetUV()
        {
            throw new NotImplementedException();
        }

        public (Point?, Point?, Point?) GetVT()
        {
            throw new NotImplementedException();
        }
    }
}