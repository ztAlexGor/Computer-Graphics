namespace Lab1
{
    public class Beam 
    {
        protected Point position;
        protected Vector3D direction;

        public Beam(Point position, Vector3D direction)
        {
            this.position = new Point(position);
            this.direction = Vector3D.Normalize(direction);
        }

        public Beam(Point p1, Point p2)
        {
            this.position = p1;
            this.direction = Vector3D.Normalize(new Vector3D(p1, p2));
        }

        public virtual Point GetPosition() => position;

        public virtual Vector3D GetDirection() => direction;

        // UNUSED
        // public float GetCosBetweenAnotherBeam(Beam beam) => 
        //    direction * beam.GetDirection() / (direction.Length() * beam.GetDirection().Length());

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
