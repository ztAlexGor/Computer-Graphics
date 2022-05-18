using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{

    internal class Point
    {
        private float x;
        private float y;
        private float z;

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
        public float X()
        {
            return x;
        }
        public float Y()
        {
            return y;
        }
        public float Z()
        {
            return z;
        }
        public static bool operator !=(Point a, Point b) {
            if (a.x != b.x || a.y != b.y || a.z != b.z) return true;
            else return false;
        }
        public static bool operator ==(Point a, Point b) { return !(a != b); }
        
    }
}
