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
            cam = new Camera(new Point(0, 0, 0), new Vector3D(1, 0, 0), 20, 20, 5);
            light = new DirectionalLight(new Point(10, 20, 0), new Vector3D(0, -1, 0));
            objects = new ISimpleObject[1] { new Sphere(new Point(20, 0, 0), 10) };
        }

        public void RayProcessing()
        {
            // Add plain in front of the camera, making it with a temporary one
            Plane plane = new(new Point(0, 0, 0), new Point(0, 20, 0), new Point(20, 0, 0));
            float[] display = new float[400];
            int[] raysHit = new int[400];
            for (int i = 0; i < 400; i++)
            {
                display[i] = 0.0f;
                raysHit[i] = 0;
            }

            foreach (var obj in objects)
            {
                List<Beam> rays = obj.GenerateRays();
                foreach (var ray in rays)
                {
                    Point? intersectionPoint = plane.IntersectsWith(ray.GetDirection());
                    if (intersectionPoint is not null)
                    {
                        if (Math.Abs(ray.GetDirection().X()) < Math.Abs(intersectionPoint.X()))
                        {
                            int hitX = (int)Math.Round(intersectionPoint.X());
                            int hitY = (int)Math.Round(intersectionPoint.Y());
                            if (hitX < 20 && hitX >= 0 && hitY < 20 && hitY >= 0)
                            {
                                display[hitY * 20 + hitX] += ray.GetCosBetweenAnotherBeam(light);
                                raysHit[hitY * 20 + hitX]++;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (raysHit[i * 20 + j] > 0)
                    {
                        float val = display[i * 20 + j] / raysHit[i * 20 + j];
                        if (val <= 0)
                        {
                            Console.Write(' ');
                        }
                        if (val > 0 && val < 0.2f)
                        {
                            Console.WriteLine('.');
                        }
                        if (val >= 0.2f && val < 0.5f)
                        {
                            Console.WriteLine('*');
                        }
                        if (val >= 0.5f && val < 0.8f)
                        {
                            Console.WriteLine('*');
                        }
                        if (val >= 0.8f)
                        {
                            Console.WriteLine('#');
                        }
                    }
                    else
                    {
                        Console.WriteLine(' ');
                    }
                }
                Console.WriteLine();
            }
            // Legacy
            // for (int i = 0; i < cam?.GetHeight(); i++)
            // {
            //     for (int j = 0; j < cam?.GetWidth(); j++)
            //     {
            //         //How iterate by screen?
            //         Point currScreenPoint = new(1, 2, 3); //#CHANGE THIS
            //         bool f = false;
            //         foreach (ISimpleObject obj in objects)
            //         {
            //             f = f || obj.IntersectsWith(cam.GetPosition(), Vector3D.Normalize(new Vector3D(cam.GetPosition(), currScreenPoint)));
            //         }
            //         Console.Write(f ? 'X' : ' ');
            //     }
            //     Console.Write('\n');
            // }
        }
    }
}
