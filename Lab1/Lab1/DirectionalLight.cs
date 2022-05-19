using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class DirectionalLight
    {
        Point position;
        Vector3D direction;

        public DirectionalLight(Point p, Vector3D v)
        {
            this.position = p;
            this.direction = Vector3D.Normalize(v);
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
    }
}
