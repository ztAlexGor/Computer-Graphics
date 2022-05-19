using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal class FigureDrawer
    {
        public static void DrawGraphically(Figure figure, Graphics graphics)
        {
            foreach (var polygon in figure.GetPolygons())
            {
                // Draw the polygon
            }
        }

        public static void DrawInConsole(Figure figure)
        {
            // Draw the figure in console
        }
    }
}
