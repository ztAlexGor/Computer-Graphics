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

        public rayProcessing()
        {
            for(int i = 0; i < this.cam.getHeight(); i++)
            {
                for(int j = 0; j < this.cam.getWidth(); j++)
                {
                    //How iterate by screen?
                    var currScreePoint = new Point(1, 2, 3); //#CHANGE THIS
                    bool f = false;
                    foreach(SimpleObject obj in this.objects)
                    {
                        f = obj.isIntersect(this.cam.getPosition, Vector3D.Normalize(new Vector3D(this.cam.getPosition, currScreePoint)));
                    }
                    if(f)
                    {
                        System.Write('X');
                    }
                    else
                    {
                        System.Write(' ');
                    }
                }
                System.Write('\n');
            }
        }
    }
}
