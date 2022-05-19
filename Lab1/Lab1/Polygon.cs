using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class Polygon
    {
        private Point a;
        private Point b;
        private Point c;
        private Color color;

        public Polygon(Point a, Point b, Point c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.color = Color.White;
        }

        public Polygon(Point a, Point b, Point c, Color color)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.color = color;
        }

        public Polygon(Polygon polygon)
        {
            if (polygon == null)
            {
                throw new ArgumentNullException();
            }

            this.a = polygon.a;
            this.b = polygon.b;
            this.c = polygon.c;
        }

    }
}
