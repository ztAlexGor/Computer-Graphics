using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Plane
    {
        private Point a;
        private Point b;
        private Point c;
        private Vector3D normal;

        public Plane(Point a, Point b, Point c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            normal = Vector3D.Normalize(Vector3D.CrossProduct(new Vector3D(a, b), new Vector3D(a, c)));
        }

        public bool IntersectsWith(Vector3D vector)
        {
            // TBD
            return true;
        }
    }
}
