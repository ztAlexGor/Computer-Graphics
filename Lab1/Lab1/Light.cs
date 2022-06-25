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
        public virtual float CalculateIntensity(List<ITraceable> objects, ITraceable thisObject, Point p)
        {
            return intensity;
        }
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
        public override float CalculateIntensity(List<ITraceable> objects, ITraceable thisObject, Point p)
        {
            Vector3D norm = thisObject.GetNormalAtPoint(p);
            float dotProduct = -(norm * direction);

            if (dotProduct > 0)
            {
                Beam lightRay = new(p, -direction);
                if (IsVisible(objects, thisObject, lightRay))
                {
                    return intensity * dotProduct / norm.Length() / direction.Length();
                }
            }
            return 0;
            
        }
        private bool IsVisible(List<ITraceable> objects, ITraceable thisObj, Beam ray)
        {
            foreach (ITraceable obj in objects)
            {
                if (obj is not null && obj != thisObj)
                {
                    Point? intersectionPoint = obj.GetIntersectionPoint(ray);
                    if (intersectionPoint is not null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
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
        public override float CalculateIntensity(List<ITraceable> objects, ITraceable thisObject, Point p)
        {
            Vector3D dist = new(position, p);

            Vector3D norm = thisObject.GetNormalAtPoint(p);
            float dotProduct = -(norm * dist);

            if (dotProduct > 0 && IsVisible(objects, thisObject, p, position))
            {
                return intensity /*/ (dist.Length() * dist.Length())*/ * dotProduct / norm.Length() / dist.Length();
            }
            return 0;
        }
        private bool IsVisible(List<ITraceable> objects, ITraceable thisObject, Point start, Point end)
        {
            Beam lightRay = new(start, new Vector3D(start, end));
            float sqDist = new Vector3D(start, end).SquareLength();

            foreach (ITraceable obj in objects)
            {
                if (obj is not null && obj != thisObject)
                {
                    Point? intersectionPoint = obj.GetIntersectionPoint(lightRay);


                    if (intersectionPoint is not null && (new Vector3D(start, intersectionPoint)).SquareLength() < sqDist)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
