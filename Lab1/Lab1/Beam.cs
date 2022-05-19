using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Beam {
        private Point start;
        private Vector3D direction;
        public Beam(Point start, Vector3D direction)
        {
            this.start = new Point(start);
            this.direction = new Vector3D(direction);
        }
        public Point GetStart()
        {
            return this.start;
        }
        public Vector3D GetDirection()
        {
            return this.direction;
        }
        public static bool operator !=(Beam a, Beam b)
        {
            if (a.start != b.start || a.direction != b.direction) return true;
            else return false;
        }
        public static bool operator ==(Beam a, Beam b)
        {
            return !(a != b);
        }
    }
}
