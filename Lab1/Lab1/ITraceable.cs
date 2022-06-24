using System.Drawing;

namespace Lab1
{
    public interface ITraceable
    {
        public Point? GetIntersectionPoint(Beam ray);

        public Vector3D GetNormalAtPoint(Point point);

        public Color GetColorAtPoint(Beam startRay, Point interPoint, List<ITraceable> objects, List<Light> lights);
    }
}
