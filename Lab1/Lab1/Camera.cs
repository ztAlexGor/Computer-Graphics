namespace Lab1
{
    public class Camera
    {
        private Point position;
        // private Vector3D direction;
        private float alpha;
        private float beta;
        private float gamma;
        private int width;
        private int height;
        private float focalDistance;

        public Camera(Point p, Vector3D v, int sHeight, int sWidth, float f)
        {
            position = new Point(p);
            v = Vector3D.Normalize(v);
            // new (0, 1, 0)

            Vector3D vi = new(0, v.Y(), v.Z());
            //new(v.X(), 0, v.Z());
            Vector3D vj = new(v.X(), 0, v.Z());
            Vector3D vk = new(v.X(), v.Y(), 0);

            Vector3D i = new(1, 0, 0);
            Vector3D j = new(0, 1, 0);
            Vector3D k = new(0, 0, 1);
            alpha = vi.Length() != 0 ? (float)Math.Acos(vi * j / vi.Length()) : 0;
            beta = vj.Length() != 0 ? (float)Math.Acos(vj * k / vj.Length()) : 0;
            gamma = vk.Length() != 0 ? (float)Math.Acos(vk * i / vk.Length()) : 0;
            Console.WriteLine(alpha);
            Console.WriteLine(beta);
            Console.WriteLine(gamma);
            //direction = Vector3D.Normalize(v);
            width = sWidth;
            height = sHeight;
            focalDistance = f;
        }

        public Camera(Point p, float a, float b, float g, int sHeight, int sWidth, float f)
        {
            position = new Point(p);
            alpha = (float)(a * Math.PI / 180);
            beta = (float)(b * Math.PI / 180);
            gamma = (float)(g * Math.PI / 180);
            width = sWidth;
            height = sHeight;
            focalDistance = f;
        }

        public Point GetPosition() => position;

        // public Vector3D GetDirection() => direction;

        public float GetFocalDistance() => focalDistance;

        public int GetScreenWidth() => width;

        public int GetScreenHeight() => height;

        public Vector3D GetAngles() => new(alpha, beta, gamma);

        public void SetPosition(Point p) => position = new Point(p);

        // public void SetDirection(Vector3D v) => direction = new Vector3D(v);

        public void SetFocalDistance(float f) => focalDistance = f;

        public void SetWidth(int w) => width = w;

        public void SetHeight(int h) => height = h;
    }
}
