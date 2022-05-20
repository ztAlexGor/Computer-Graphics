using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Point
    {
        private readonly float x;
        private readonly float y;
        private readonly float z;

        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point(Point a)
        {
            this.x = a.x;
            this.y = a.y;
            this.z = a.z;
        }

        public float X() => x;

        public float Y() => y;

        public float Z() => z;

        public Point Rotate(float alpha, float beta, float gamma) => MathUtils.GetRotatedVector(this, alpha, beta, gamma);

        public static bool operator !=(Point a, Point b) => (a.x != b.x) || (a.y != b.y) || (a.z != b.z);

        public static bool operator ==(Point a, Point b) => !(a != b);

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Point p = (Point)obj;
                return (x == p.x) && (y == p.y) && (z == p.z);
            }
        }

        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();

    }
}
