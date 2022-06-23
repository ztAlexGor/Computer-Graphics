namespace Lab1
{
    public interface ITraceable
    {
        public Point? GetIntersectionPoint(Beam ray);

        public Vector3D GetNormalAtPoint(Point point);

        public MyColor GetColorAtPoint(Point point, List<ITraceable> objects, List<Light> lights);
    }
}
