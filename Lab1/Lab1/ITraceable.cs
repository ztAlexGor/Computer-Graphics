using System.Drawing;

namespace Lab1
{
    public interface ITraceable
    {
        public Point? GetIntersectionPoint(Beam ray);

        public Vector3D GetNormalAtPoint(Point point);

        public Color GetColorAtPoint(Beam startRay, Point interPoint, List<ITraceable> objects, List<Light> lights);

        public ITraceable Rotate(float alpha = 0, float beta = 0, float gamma = 0);

        public ITraceable Scale(float sx = 0, float sy = 0, float sz = 0);

        public ITraceable Translate(float x = 0, float y = 0, float z = 0);
    }
}
