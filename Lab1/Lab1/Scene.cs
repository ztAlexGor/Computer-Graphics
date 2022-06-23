using System.Drawing;

namespace Lab1
{
    public class Scene
    {
        private readonly Camera cam;
        private readonly List<Light> lights;
        private readonly List<ITraceable> objects;
        private float[] viewValues;
        private Color[] viewColors;

        public Scene(string inputPathName)
        {
            cam = new Camera(new Point(0, 0, -0.75f), new Vector3D(0, 0, 1), 300, 300, 150);
            lights = new List<Light>();
            //lights.Add(new DirectionalLight(new Vector3D(1, 1, 1), 1, Color.Violet));
            lights.Add(new DirectionalLight(new Vector3D(0, -1, 0), 1, Color.Blue));
            lights.Add(new DirectionalLight(new Vector3D(0, 1, 0), 1, Color.Yellow));
            viewValues = new float[cam.GetScreenHeight() * cam.GetScreenWidth()];
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
            viewValues = new float[cam.GetScreenHeight() * cam.GetScreenWidth()];
            viewColors = new Color[cam.GetScreenHeight() * cam.GetScreenWidth()];
            ClearView();
        }

        private void ClearView()
        {
            for(int i = 0; i < viewValues.Length; i++)
                viewValues[i] = 0.0f;
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
                        List<Color> colors = new List<Color>();
                        List<float> illuminances = new List<float>();

                        foreach (Light light in lights)
                        {
                            if (light.GetType() == typeof(DirectionalLight))
                            {
                                DirectionalLight l = (DirectionalLight)light;
                                Beam lightRay = new(intersectionPoint, -l.GetDirection());
                                if (RayIsIntersect(lightRay, resObj))
                                {
                                    viewValues[i * screenWidth + j] = 0;
                                }
                                else
                                {
                                    float view = -(resObj.GetNormalAtPoint(intersectionPoint) * l.GetDirection());
                                    viewValues[i * screenWidth + j] = view >= 0 ? view : 0;
                                    
                                    colors.Add(l.GetColor());
                                    illuminances.Add(l.GetIntensity() * viewValues[i * screenWidth + j]);
                                }
                            }
                            else if(light.GetType() == typeof(PointLight))
                            {
                                PointLight l = (PointLight)light;
                                if (SegmentIsIntersect(intersectionPoint, l.GetPosition(), resObj))
                                {
                                    viewValues[i * screenWidth + j] = 0;
                                }
                                else
                                {
                                    Vector3D dist = new(intersectionPoint, l.GetPosition());
                                    float view = -(resObj.GetNormalAtPoint(intersectionPoint) * dist);
                                    viewValues[i * screenWidth + j] = view >= 0 ? view : 0;

                                    colors.Add(l.GetColor());
                                    illuminances.Add(l.GetIntensity() / dist.Length() * dist.Length() * viewValues[i * screenWidth + j]);
                                }
                            }
                            else if (light.GetType() == typeof(Light))
                            {
                                colors.Add(light.GetColor());
                                illuminances.Add(light.GetIntensity());
                            }

                            byte r = 0;
                            byte g = 0;
                            byte b = 0;
                            for (int k = 0; k < colors.Count; k++)
                            {
                                r += (byte)Math.Round(illuminances[k] * colors[k].R);
                                g += (byte)Math.Round(illuminances[k] * colors[k].G);
                                b += (byte)Math.Round(illuminances[k] * colors[k].B);
                            }

                            viewColors[i * screenWidth + j] = Color.FromArgb(r, g, b);
                        }
                    }
                }
            }
            ViewOutput();
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
        private void ViewOutput()
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
        }
    }
}
