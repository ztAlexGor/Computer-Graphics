using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Scene
    {
        Camera cam;
        DirectionalLight light;
        SimpleObject[] objects;

        public Scene()
        {
            this.cam = new Camera(new Point(0, 0, 0), new Vector3D(1, 0, 0), 20, 20, 5);
            this.light = new DirectionalLight(new Point(10, 20, 0), new Vector3D(0, -1, 0));
            this.objects = new SimpleObject[1]{ new Sphere(new Point(20, 0, 0), 10) };
        }
    }
}
