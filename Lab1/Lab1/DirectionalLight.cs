using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class DirectionalLight
    {
        private Point position;
        private Vector3D direction;

        public DirectionalLight(Point p, Vector3D v)
        {
            this.position = p;
            this.direction = Vector3D.Normalize(v);
        }

        public Point GetPosition() => position;

        public Vector3D GetDirection() => direction;

        public void SetPosition(Point p) => position = new Point(p);

        public void SetDirection(Vector3D v) => direction = new Vector3D(v);
    }
}
