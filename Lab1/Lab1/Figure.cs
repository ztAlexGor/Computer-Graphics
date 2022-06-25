using System.Drawing;

namespace Lab1
{
    public class Figure
    {
        private List<ITraceable> polygons;

        public Figure(List<ITraceable> polygons)
        {
            this.polygons = new List<ITraceable>(polygons.Count);

            foreach (Polygon polygon in polygons)
            {
                this.polygons.Add(new Polygon(polygon));
            }
        }
        
        public Figure()
        {
            this.polygons = new List<ITraceable>();
        }

        public void AddPolygon(ITraceable polygon) => polygons.Add(polygon);//copy!!

        public List<ITraceable> GetPolygons() => polygons;


        public void Rotate(float alpha = 0, float beta = 0, float gamma = 0)
        {
            for (int i = 0; i < polygons.Count; i++)
            {
                polygons[i] = polygons[i].Rotate(alpha, beta, gamma);
            }
        }

        public void Scale(float x = 0, float y = 0, float z = 0)
        {
            for (int i = 0; i < polygons.Count; i++)
            {
                polygons[i] = polygons[i].Scale(x, y, z);
            }
        }

        public void Translate(float x = 0, float y = 0, float z = 0)
        {
            for (int i = 0; i < polygons.Count; i++)
            {
                polygons[i] = polygons[i].Translate(x, y, z);
            }
        }




/*        public Color GetColorAtPoint(Beam startRay, Point interPoint, List<ITraceable> objects, List<Light> lights)
        {
            throw new NotImplementedException();
        }

                public Point? GetIntersectionPoint(Beam ray)
        {
            return null;
        }

        public Vector3D GetNormalAtPoint(Point point)
        {
            throw new NotImplementedException();
        }
*/
    }
}
