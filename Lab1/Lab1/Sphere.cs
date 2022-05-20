using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Sphere
    {
        float radius;
        Point center;

        public Sphere(float r, Point c)
        {
            this.radius = r;
            this.center = c;
        }

        public bool isIntersect(Camera view)
        {
            var d = view.getDirection();
            var o = view.getPosition();
            var k = new Vector3D(o, center);

            var a = d * d;
            var b = 2 * d * k;
            var c = k * k - radius * radius;

            var D = (b * b) - (4 * a * c);

            if(D >= 0)
            {
                return (Math.Sqrt(D) * b) / (2 * a) > 0;
            }
            return false;
        }
    }
}