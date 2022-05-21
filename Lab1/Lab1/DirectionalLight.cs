using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class DirectionalLight : Beam
    {
        public DirectionalLight(Point position, Vector3D direction) : base(position, direction) { }

        public void SetPosition(Point p) => position = new Point(p);

        public void SetDirection(Vector3D v) => direction = new Vector3D(v);
    }
}
