using System.Drawing;

namespace Lab1
{
    public class PointLight : Light
    {
        private Point position;

        public PointLight(Point position, float intensity, Color color) : base(intensity, color, 1)
        {
            this.position = new(position);
            bIsRayInfinite = false;
        }

        public void SetPosition(Point p) => position = new Point(p);

        public override Point GetPosition() => position;

        public override List<Vector3D> GetRayDirection(Vector3D n, Point p)
        {
            List<Vector3D> res = new List<Vector3D>(1);
            res.Add(new Vector3D(p, position));
            return res;
        }

        public override float GetAttenuationCoefficient(Point p) => 1.0f / new Vector3D(p, position).SquareLength();
    }
}
