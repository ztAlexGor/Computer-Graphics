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
            cam = new Camera(new Point(0, 0, -0.75f), new Vector3D(0, 0, 0), 120, 120, 60);
            lights = new List<Light>();
            lights.Add(new DirectionalLight(new Vector3D(1, -1, 1), 1, Color.DodgerBlue));
            //lights.Add(new DirectionalLight(new Vector3D(0, -1, 0), 1, Color.White));
            //lights.Add(new PointLight(new Point(0, -1f, 0), 1, Color.DeepPink));
            lights.Add(new Light(0.2f, Color.Red));
            viewColors = new Color[cam.GetScreenHeight() * cam.GetScreenWidth()];
            objects = FileWork.ReadObj(inputPathName).GetObjects();
            objects.Add(new Plane(new Point(0, -1, 0), new Point(0, 0, 1), new Point(1, 0, 1), m: new Reflective()));
            objects.Add(new Plane(new Point(1, 0, 0), new Point(0, 0, 1), new Point(0, 1, 1), m: new Reflective()));
            ClearView();
        }


        public Scene(List<ITraceable> objArr)
        {
            cam = new Camera(new Point(0, 0, 0), new Vector3D(0, 0, 1), 128, 128, 70);
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
            Point screenNW = new(camPosition.X() - screenWidth / 2 + ((screenWidth % 2 == 0) ? 0.5f : 0),
                                camPosition.Y() + screenHeight / 2 - ((screenHeight % 2 == 0) ? 0.5f : 0),
                                camPosition.Z() + cam.GetFocalDistance());
            ClearView();

            for (int i = 0; i < screenHeight; i++)
            {
                for (int j = 0; j < screenWidth; j++)
                {
                    Beam ray = new Beam(new Point(camPosition), 
                        new Vector3D(camPosition, new Point(screenNW.X() + j, screenNW.Y() - i, screenNW.Z())))
                        .ApplyRotationToDirectionVector(cam.GetAngles());
                    ITraceable resObj;
                    Point? interPoint = RayIntersect(ray, objects, out resObj);
                    if (interPoint is not null)
                    {
                        viewColors[i * screenWidth + j] = resObj.GetColorAtPoint(ray, interPoint, objects, lights);
                    }
                }
            }
            //ViewOutput();
            FileWork.WritePPM(viewColors, screenHeight, screenWidth, outputPathName);
        }

        public static Point RayIntersect(Beam ray, List<ITraceable> objects, out ITraceable intObj)
        {
            float depth = float.MaxValue;
            Point result = null;
            intObj = null;
            foreach (ITraceable obj in objects)
            {
                if (obj is not null)
                {
                    Point? intersectionPoint = obj.GetIntersectionPoint(ray);
                    if (intersectionPoint is not null) 
                    {
                        float sqLenght = new Vector3D(ray.GetPosition(), intersectionPoint).SquareLength();
                        if (sqLenght < depth)
                        {
                            depth = sqLenght;
                            result = intersectionPoint;
                            intObj = obj;
                        }
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
            for (int i = 0; i < cam.GetScreenHeight(); i++)
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
