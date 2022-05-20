using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class DirectionalLight : Beam
    {
        private Point position;
        private Vector3D direction;

        public DirectionalLight(Point position, Vector3D direction)
        {
            this.position = new Point(position);
            this.direction = new Vector3D(direction);
        }

        public override Point GetPosition() => position;

        public override Vector3D GetDirection() => direction;

        public void SetPosition(Point p) => position = new Point(p);

        public void SetDirection(Vector3D v) => direction = new Vector3D(v);
    }
}
