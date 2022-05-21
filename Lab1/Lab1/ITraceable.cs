using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal interface ITraceable
    {
        public Point? GetIntersectionPoint(Beam ray);

        public Vector3D GetNormalAtPoint(Point point);
    }
}
