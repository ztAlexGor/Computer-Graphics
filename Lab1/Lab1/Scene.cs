using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Scene
    {
        private readonly Camera cam;
        private readonly DirectionalLight light;
        private readonly ISimpleObject[] objects;

        public Scene()
        {
            this.cam = new Camera(new Point(0, 0, 0), new Vector3D(1, 0, 0), 20, 20, 5);
            this.light = new DirectionalLight(new Point(10, 20, 0), new Vector3D(0, -1, 0));
            this.objects = new ISimpleObject[1]{ new Sphere(new Point(20, 0, 0), 10) };
        }

        public void RayProcessing()
        {
            for(int i = 0; i < cam?.GetHeight(); i++)
            {
                for(int j = 0; j < cam?.GetWidth(); j++)
                {
                    //How iterate by screen?
                    Point currScreePoint = new(1, 2, 3); //#CHANGE THIS
                    bool f = false;
                    foreach(ISimpleObject obj in objects)
                    {
                        f = obj.IntersectsWith(cam.GetPosition(), Vector3D.Normalize(new Vector3D(cam.GetPosition(), currScreePoint)));
                    }
                    System.Write(f ? 'X' : ' ');
                }
                System.Write('\n');
            }
        }
    }
}
