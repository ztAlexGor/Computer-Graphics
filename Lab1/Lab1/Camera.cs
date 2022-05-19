using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Camera
    {
        Point position;
        Vector3D direction;
        int[][] screen;

        public Camera(Point p, Vector3D v, int sHeight, int sWidth)
        {
            this.position = p;
            this.direction = Vector3D.Normalize(v);
            this.screen = new int[sHeight][sWidth];
        }

        public Point getPosition()
        {
            return this.position;
        }
        public void setPosition(Point p)
        {
            this.position = new Point(p);
        }

        public Vector3D getDirection()
        {
            return this.direction;
        }
        public void setDirection(Vector3D v)
        {
            this.direction = new Vector3D(v);
        }

        public void clearScreen()
        {
            for (int i = 0; i < this.screen.Length; i++)
                for (int j = 0; j < this.screen[0].Length; j++)
                    this.screen[i][j] = 0;
        }
    }
}
