using System.Drawing;

namespace Lab1
{
    public class Scene
    {
        private readonly Camera cam;
        private readonly List<Light> lights;
        private readonly List<ITraceable> objects;
        private Color[] viewColors;

        public Scene(string inputPathName)
        {
            cam = new Camera(new Point(0, 0, -0.75f), new Vector3D(0, 0, 1), 100, 100, 50);
            lights = new List<Light>();
            //lights.Add(new DirectionalLight(new Vector3D(1, 1, 1), 1, Color.Violet));
            lights.Add(new DirectionalLight(new Vector3D(0, -1, 0), 1, Color.White));
            lights.Add(new PointLight(new Vector3D(0, 1, 0), 1, Color.Red));
            lights.Add(new Light(0.2f, Color.Green));
            viewColors = new Color[cam.GetScreenHeight() * cam.GetScreenWidth()];
            objects = FileWork.ReadObj(inputPathName).GetObjects();

            ClearView();
        }


        public Scene(List<ITraceable> objArr)
        {
            cam = new Camera(new Point(0, 0, 0), new Vector3D(0, 0, 1), 20, 20, 5);
            lights = new List<Light>();
            lights.Add(new DirectionalLight(new Vector3D(0, 1, 0.5f), 1, Color.FromArgb(255, 255, 255)));
            objects = objArr;
            viewColors = new Color[cam.GetScreenHeight() * cam.GetScreenWidth()];
            ClearView();
        }

        private void ClearView()
        {
            for(int i = 0; i < viewColors.Length; i++)
                viewColors[i] = new Color();
        }

        public void RayProcessing(string outputPathName)
        {
            int screenHeight = cam.GetScreenHeight();
            int screenWidth = cam.GetScreenWidth();
            Point camPosition = cam.GetPosition();
            Point screenNW = new(camPosition.X() - screenWidth / 2,
                                camPosition.Y() + screenHeight / 2,
                                camPosition.Z() + cam.GetFocalDistance());
            ClearView();

            for (int i = 0; i < screenHeight; i++)
            {
                for (int j = 0; j < screenWidth; j++)
                {
                    Beam ray = new(new Point(camPosition), new Vector3D(camPosition,
                        new Point(screenNW.X() + j, screenNW.Y() - i, screenNW.Z())));
                    ITraceable resObj;
                    Point? intersectionPoint = RayIntersect(ray, out resObj);
                    if (intersectionPoint is not null)
                    {

                        //!todo change light accumulation
                        byte r = 0;
                        byte g = 0;
                        byte b = 0;
                        foreach (Light light in lights)
                        {
                            Color color = light.GetColor();
                            float illuminance = light.CalculateIntensity(objects, resObj, intersectionPoint);

                            r += (byte)Math.Round(illuminance * color.R);
                            g += (byte)Math.Round(illuminance * color.G);
                            b += (byte)Math.Round(illuminance * color.B);
                        }

                        viewColors[i * screenWidth + j] = Color.FromArgb(r, g, b);
                    }
                }
            }
            //ViewOutput();
            FileWork.WritePPM(viewColors, screenHeight, screenWidth, outputPathName);
        }

        public Point RayIntersect(Beam ray, out ITraceable intObj)
        {
            float depth = float.MaxValue;
            Point result = null;
            intObj = null;
            foreach (ITraceable obj in objects)
            {
                if(obj is not null) {
                    Point? intersectionPoint = obj.GetIntersectionPoint(ray);
                    if (intersectionPoint is not null && intersectionPoint.Z() < depth){
                        depth = intersectionPoint.Z();
                        result = intersectionPoint;
                        intObj = obj;
                    }
                }
            }
            return result;
        }

        public bool RayIsIntersect(Beam ray, ITraceable thisObj)
        {
            foreach (ITraceable obj in objects)
            {
                if (obj is not null && obj != thisObj)
                {
                    Point? intersectionPoint = obj.GetIntersectionPoint(ray);
                    if (intersectionPoint is not null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool SegmentIsIntersect(Point start, Point end, ITraceable thisObj)
        {
            Beam lightRay = new(start, new Vector3D(start, end));
            float sqDist = new Vector3D(start, end).SquareLength();

            foreach (ITraceable obj in objects)
            {
                if (obj is not null && obj != thisObj)
                {
                    Point? intersectionPoint = obj.GetIntersectionPoint(lightRay);

                    
                    if (intersectionPoint is not null && (new Vector3D(start, intersectionPoint)).SquareLength() < sqDist)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
/*        private void ViewOutput()
        {
            for (int i = 0; i < cam.GetScreenWidth(); i++)
            {
                for (int j = 0; j < cam.GetScreenWidth(); j++)
                {
                    float val = viewValues[i * cam.GetScreenWidth() + j];

                    if (val <= 0)
                    {
                        Console.Write(' '.ToString().PadLeft(3));
                    }
                    else if (val > 0 && val < 0.2f)
                    {
                        Console.Write('·'.ToString().PadLeft(3));
                    }
                    else if (val >= 0.2f && val < 0.5f)
                    {
                        Console.Write('*'.ToString().PadLeft(3));
                    }
                    else if (val >= 0.5f && val < 0.8f)
                    {
                        Console.Write('O'.ToString().PadLeft(3));
                    }
                    else if (val >= 0.8f)
                    {
                        Console.Write('#'.ToString().PadLeft(3));
                    }
                }
                Console.WriteLine();
            }
        }*/
    }
}
