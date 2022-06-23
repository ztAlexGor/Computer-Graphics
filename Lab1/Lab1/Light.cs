using System.Drawing;

namespace Lab1
{
    public class Light
    {
        protected float intensity;
        protected Color color;

        public Light(float intensity, Color color)  
        {
            this.intensity = intensity;
            this.color = color;
        }
        public void SetIntensity(float intensity) { this.intensity = intensity; }
        public void SetColor(Color newColor)
        {
            color = newColor;
        }
        public float GetIntensity() => intensity;
        public Color GetColor() => color;
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
    }
}
