using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Beam 
    {
        protected Point position;
        protected Vector3D direction;

        public Beam(Point position, Vector3D direction)
        {
            this.position = new Point(position);
            this.direction = new Vector3D(direction);
        }

        public virtual Point GetPosition() => position;

        public virtual Vector3D GetDirection() => direction;

        public float GetCosBetweenAnotherBeam(Beam beam) => 
            direction * beam.GetDirection() / (direction.Length() * beam.GetDirection().Length());

        public static bool operator !=(Beam a, Beam b) => (a.position != b.position) || (a.direction != b.direction);

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
                return (position == b.position) && (direction == b.direction);
            }
        }

        public override int GetHashCode() => position.GetHashCode() ^ direction.GetHashCode();
    }
}
