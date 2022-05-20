using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Camera
    {
        private Point position;
        private Vector3D direction;
        private int width;
        private int height;
        private float focusDistance;

        public Camera(Point p, Vector3D v, int sHeight, int sWidth, float f)
        {
            position = p;
            direction = Vector3D.Normalize(v);
            width = sWidth;
            height = sHeight;
            focusDistance = f;
        }

        public Point GetPosition() => position;

        public Vector3D GetDirection() => direction;

        public float GetFocus() => focusDistance;

        public float GetWidth() => width;

        public float GetHeight() => height;

        public void SetPosition(Point p) => position = new Point(p);

        public void SetDirection(Vector3D v) => direction = new Vector3D(v);

        public void SetFocus(float f) => focusDistance = f;

        public void SetWidth(int w) => width = w;

        public void SetHeight(int h) => height = h;
    }
}
