using System.Drawing;

namespace Lab1
{
    public class Light
    {
        protected float intensity;
        protected Color color;
        private Random random;
        public Light(float intensity, Color color)  
        {
            this.intensity = intensity;
            this.color = color;
            random = new Random();
        }
        public void SetIntensity(float intensity) { this.intensity = intensity; }
        public void SetColor(Color newColor) { color = newColor; }
        public float GetIntensity() => intensity;
        public Color GetColor() => color;
        public virtual Vector3D GetRayDirection(Vector3D n, Point p)
        {
            return n.Rotate(random.Next(-89, 90) * (float)Math.PI / 180.0f,
                            random.Next(-89, 90) * (float)Math.PI / 180.0f,
                            random.Next(-89, 90) * (float)Math.PI / 180.0f);
        }
        public virtual bool IsReplicated() => false;
        public virtual float GetAttenuationCoefficient(Point p) => 1;
        public virtual bool IsBeamRay() => true;
    }

    public class DirectionalLight : Light
    {
        private Vector3D direction;

        public DirectionalLight(Vector3D direction, float intensity, Color color) : base(intensity, color) 
        {
            this.direction = Vector3D.Normalize(direction);
        }
        public void SetDirection(Vector3D v) => direction = new Vector3D(v);
        public Vector3D GetDirection() => direction;
        public override Vector3D GetRayDirection(Vector3D n, Point p) => -direction;
        public override bool IsReplicated() => true;
        public override float GetAttenuationCoefficient(Point p) => 1;
        public override bool IsBeamRay() => true;
    }

    public class PointLight : Light
    {
        private Point position;

        public PointLight(Point position, float intensity, Color color) : base(intensity, color)
        {
            this.position = new(position);
        }
        public void SetPosition(Point p) => position = new Point(p);
        public Point GetPosition() => position;
        public override Vector3D GetRayDirection(Vector3D n, Point p) => new (p, position);
        public override bool IsReplicated() => true;
        public override float GetAttenuationCoefficient(Point p) => 1.0f / new Vector3D(p, position).SquareLength();
        public override bool IsBeamRay() => false;
    }
}
