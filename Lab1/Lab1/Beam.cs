using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Beam 
    {
        private readonly Point start;
        private readonly Vector3D direction;

        public Beam(Point start, Vector3D direction)
        {
            this.start = new Point(start);
            this.direction = new Vector3D(direction);
        }

        public Point GetStart() => start;

        public Vector3D GetDirection() => direction;

        public static bool operator !=(Beam a, Beam b) => (a.start != b.start) || (a.direction != b.direction);

        public static bool operator ==(Beam a, Beam b) => !(a != b);

        public override bool Equals(object? obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Beam b = (Beam)obj;
                return (start == b.start) && (direction == b.direction);
            }
        }

        public override int GetHashCode() => start.GetHashCode() ^ direction.GetHashCode();
    }
}
