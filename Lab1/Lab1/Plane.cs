using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Plane
    {
        Point a;
        Point b;
        Point c;
        Vector3D normal;

        Plane(Point a, Point b, Point c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            normal = Vector3D.Normalize(Vector3D.CrossProduct(new Vector3D(a, b), new Vector3D(a, c)));
        }
        bool Intersect()
    }
}
