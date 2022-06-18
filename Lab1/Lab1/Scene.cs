namespace Lab1
{
    public class Scene
    {
        private readonly Camera cam;
        private readonly DirectionalLight light;
        private readonly List<ITraceable> objects;
        private float[] viewValues;

        public Scene()
        {
            cam = new Camera(new Point(0, 0, -0.75f), new Vector3D(0, 0, 1),720, 720, 200);
            light = new DirectionalLight(new Point(10, 20, 0), new Vector3D(2, 0.5f, 1));
            ObjStructure obj = FileWork.ReadObj("D:\\PlsYes\\C#\\CompGrafix\\Lab1\\cow.obj");
            objects = obj.GetObjects();
            viewValues = new float[cam.GetScreenHeight() * cam.GetScreenWidth()];
            ClearView();
        }

        public Scene(List<ITraceable> objArr)
        {
            cam = new Camera(new Point(0, 0, 0), new Vector3D(0, 0, 1), 20, 20, 5);
            light = new DirectionalLight(new Point(10, 20, 0), new Vector3D(0, 1, 0.5f));
            objects = objArr;
            viewValues = new float[cam.GetScreenHeight() * cam.GetScreenWidth()];
            ClearView();
        }

        private void ClearView()
        {
            for(int i = 0; i < viewValues.Length; i++)
                viewValues[i] = 0.0f;
        }

        public void RayProcessing()
        {
            int screenHeight = cam.GetScreenHeight();
            int screenWidth = cam.GetScreenWidth();
            Point camPosition = cam.GetPosition();
            Point screenNW = new(camPosition.X() - screenWidth / 2,
                                camPosition.Y() - screenHeight / 2,
                                camPosition.Z() + cam.GetFocalDistance());
            ClearView();

            for (int i = 0; i < screenHeight; i++)
            {
                for (int j = 0; j < screenWidth; j++)
                {
                    Beam ray = new(new Point(camPosition), new Vector3D(camPosition,
                        new Point(screenNW.X() + j, screenNW.Y() + i, screenNW.Z())));
                    ITraceable resObj;
                    Point? intersectionPoint = RayIntersect(ray, out resObj);
                    if (intersectionPoint is not null)
                    {
                        float view = -(resObj.GetNormalAtPoint(intersectionPoint) * light.GetDirection());
                        viewValues[i * screenWidth + j] = view >= 0 ? view : 0;
                    }
                }
            }
            //ViewOutput();
            FileWork.WritePPM(viewValues, screenHeight, screenWidth, "D:\\PlsYes\\C#\\CompGrafix\\Lab1\\Lab1image.ppm");
        }

        public Point RayIntersect(Beam ray, out ITraceable intObj)
        {
            int depth = int.MaxValue;
            Point result = null;
            intObj = null;
            foreach (ITraceable obj in objects)
            {
                if(obj is not null) {
                    Point? intersectionPoint = obj.GetIntersectionPoint(ray);
                    if (intersectionPoint is not null)
                    {
                        Vector3D objNormal = obj.GetNormalAtPoint(intersectionPoint);
                        float dotProductValue = -(objNormal * light.GetDirection());
                        if (intersectionPoint.Z() < depth)
                        {
                            depth = (int)intersectionPoint.Z();
                            result = intersectionPoint;
                            intObj = obj;
                        }
                    }
                }
            }
            return result;
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
