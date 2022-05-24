namespace Lab1
{
    internal interface ITraceable
    {
        public Point? GetIntersectionPoint(Beam ray);

        public Vector3D GetNormalAtPoint(Point point);
    }
}
