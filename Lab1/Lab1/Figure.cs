using System.Drawing;

namespace Lab1
{
    internal class Figure : ITraceable
    {
        private List<Polygon> polygons;

        public Figure(List<Polygon> polygons)
        {
            this.polygons = new List<Polygon>(polygons.Count);

            foreach (var polygon in polygons)
            {
                this.polygons.Add(new Polygon(polygon));
            }
        }

        public Figure()
        {
            this.polygons = new List<Polygon>();
        }

        public void AddPolygon(Polygon polygon) => polygons.Add(new Polygon(polygon));

        public List<Polygon> GetPolygons() => polygons;

        public Point? GetIntersectionPoint(Beam ray)
        {
            return null;
        }

        public Vector3D GetNormalAtPoint(Point point)
        {
            throw new NotImplementedException();
        }

        public void Rotate(float alpha = 0, float beta = 0, float gamma = 0)
        {
            // Still in progress
        }

        public Color GetColorAtPoint(Beam startRay, Point interPoint, List<ITraceable> objects, List<Light> lights)
        {
            throw new NotImplementedException();
        }
    }
}
