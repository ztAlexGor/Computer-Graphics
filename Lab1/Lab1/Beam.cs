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
            position = new Point(p1);
            direction = Vector3D.Normalize(new Vector3D(p1, p2));
        }

        public virtual Point GetPosition() => position;

        public virtual Vector3D GetDirection() => direction;

        // NON-CONST BEHAVIOR
        // public void ApplyRotationToDirectionVector(float a, float b, float g) => direction = direction.Rotate(a, b, g);

        // CONST BEHAVIOR
        public Beam ApplyRotationToDirectionVector(float a, float b, float g) => new(position, direction.Rotate(a, b, g));
        public Beam ApplyRotationToDirectionVector(Vector3D angles) => new(position, direction.Rotate(angles));

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
