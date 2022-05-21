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
        private readonly ITraceable[] objects;

        public Scene()
        {
            cam = new Camera(new Point(0, 0, 0), new Vector3D(1, 0, 0), 20, 20, 5);
            light = new DirectionalLight(new Point(10, 20, 0), new Vector3D(0, -1, 0));
            objects = new ITraceable[1] { new Sphere(new Point(20, 0, 0), 10) };
        }

        public void RayProcessing()
        {
            int screenHeight = cam.GetScreenHeight();
            int screenWidth = cam.GetScreenWidth();
            Point camPosition = cam.GetPosition();
            Point screenNW = new(camPosition.X() - screenWidth + ((screenWidth + 1) % 2) * 0.5f,
                camPosition.Y() - screenHeight + ((screenHeight + 1) % 2) * 0.5f,
                camPosition.Z() + cam.GetFocalDistance());

            float[] screenValues = new float[screenHeight * screenWidth];
            int[] ZBuffer = new int[screenHeight * screenWidth];
            for (int i = 0; i < screenHeight * screenWidth; i++)
            {
                screenValues[i] = 0.0f;
                ZBuffer[i] = int.MinValue;
            }

            for (int i = 0; i < screenHeight; i++)
            {
                for (int j = 0; j < screenWidth; j++)
                {
                    Beam ray = new(new Point(camPosition), new Vector3D(camPosition,
                        new Point(screenNW.X() + j, screenNW.Y() + i, screenNW.Z())));

                    foreach (ITraceable obj in objects)
                    {
                        Point? intersectionPoint = obj.GetIntersectionPoint(ray);
                        if (intersectionPoint is not null)
                        {
                            Vector3D objNormal = obj.GetNormalAtPoint(intersectionPoint);
                            float dotProductValue = objNormal * light.GetDirection();
                            int idx = (int)(intersectionPoint.Y() * screenHeight + intersectionPoint.X());
                            if (intersectionPoint.Z() > ZBuffer[idx])
                            {
                                ZBuffer[idx] = (int)intersectionPoint.Z();
                                screenValues[idx] = dotProductValue;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < screenHeight; i++)
            {
                for (int j = 0; j < screenWidth; j++)
                {
                    float val = screenValues[i * screenHeight + screenWidth];
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
                Console.WriteLine('#');
            }
        }
    }
}
