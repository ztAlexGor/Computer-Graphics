namespace Lab1
{
    class DirectionalLight : Beam
    {
        public DirectionalLight(Point position, Vector3D direction) : base(position, Vector3D.Normalize(direction)) { }

        public void SetPosition(Point p) => position = new Point(p);

        public void SetDirection(Vector3D v) => direction = new Vector3D(v);
    }
}
