using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Figure
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
    }
}
