using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Camera
    {
        Point position;
        Vector3D direction;
        int width;
        int height;
        float focusDistance;

        public Camera(Point p, Vector3D v, int sHeight, int sWidth, float f)
        {
            this.position = p;
            this.direction = Vector3D.Normalize(v);
            this.width = sWidth;
            this.height = sHeight;
            this.focusDistance = f;
        }

        public Point getPosition()
        {
            return this.position;
        }
        public void setPosition(Point p)
        {
            this.position = new Point(p);
        }

        public Vector3D getDirection()
        {
            return this.direction;
        }
        public void setDirection(Vector3D v)
        {
            this.direction = new Vector3D(v);
        }
        public float getFocus()
        {
            return this.focusDistance;
        }
        public void setFocus(float f)
        {
            this.focusDistance = f;
        }
        public float getWidth()
        {
            return this.width;
        }
        public void setWidth(float f)
        {
            this.width;
        }
        public float getHeight()
        {
            return this.height;
        }
        public void setHeight(float f)
        {
            this.height;
        }
    }
}
