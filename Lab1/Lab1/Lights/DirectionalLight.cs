using System.Drawing;

namespace Lab1
{
    public class DirectionalLight : Light
    {
        private Vector3D direction;

        public DirectionalLight(Vector3D direction, float intensity, Color color) : base(intensity, color, 1)
        {
            this.direction = Vector3D.Normalize(direction);
        }

        public void SetDirection(Vector3D v) => direction = new Vector3D(v);

        public override Vector3D GetDirection() => direction;

        public override List<Vector3D> GetRayDirection(Vector3D n, Point p)
        {
            List<Vector3D> res = new List<Vector3D>(1);
            res.Add(-direction);
            return res;
        }
    }
}
