using System.Drawing;

namespace Lab1
{
    public class Sphere : ITraceable
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

        public Color GetColorAtPoint(Beam startRay, Point interPoint, List<ITraceable> objects, List<Light> lights)
        {
            throw new NotImplementedException();
        }

        public ITraceable Rotate(float alpha = 0, float beta = 0, float gamma = 0) =>
            new Sphere(center.Rotate(alpha, beta, gamma), radius);

        public ITraceable Scale(float sx = 0, float sy = 0, float sz = 0) =>
            new Sphere(center, radius * sx);

        public ITraceable Translate(float x = 0, float y = 0, float z = 0) =>
            new Sphere(center.Translate(x, y, z), radius);
    }
}