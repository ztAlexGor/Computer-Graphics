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
        private readonly Point a;
        private readonly Point b;
        private readonly Point c;
        private readonly Color color;

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
            this.color = polygon.color;
        }

        public Polygon Rotate(float alpha = 0, float beta = 0, float gamma = 0) => 
            new(a.Rotate(alpha, beta, gamma), b.Rotate(alpha, beta, gamma), c.Rotate(alpha, beta, gamma), color);
    }
}
