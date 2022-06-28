using System.Drawing;

namespace Lab1
{
    public class Scene
    {
        private readonly Camera cam;
        private readonly List<Light> lights;
        private readonly List<Figure> figures;
        private Color[] viewColors;
        private Color[] normalColors;
        private BVHTree tree;
        public Scene(string inputPathName)
        {
            cam = new Camera(new Point(50, 100, -350f), new Vector3D(0, 0, 0), 100, 100, 50);
            viewColors = new Color[cam.GetScreenHeight() * cam.GetScreenWidth()];
            normalColors = new Color[cam.GetScreenHeight() * cam.GetScreenWidth()];
            lights = new List<Light>();
            figures = new List<Figure>();
            tree = new BVHTree(4);

            ClearView();
            SetScene(inputPathName);
            
        }

        public void SetScene(string inputPathName)
        {
            //Lights
            //lights.Add(new DirectionalLight(new Vector3D(-1, -1, 1), 1, Color.DodgerBlue));
            //lights.Add(new DirectionalLight(new Vector3D(0, -1, 0), 1, Color.Red));
            //lights.Add(new PointLight(new Point(150, 0, 0), 1, Color.DeepPink));
            //lights.Add(new Light(2f, Color.White));

            //Figures
            Figure cow = new Figure(FileWork.ReadObj(inputPathName).GetObjects());
            // Figure 

            cow.Rotate(beta: (float)Math.PI, gamma: (float)Math.PI / 2);
            cow.Scale(300, 300, 300);
            cow.Translate(x: 20);
            figures.Add(cow);

            Figure floor = new Figure();
            floor.AddPolygon(new Polygon( new Point(1000, -100, 0), new Point(-1000, -100, 0), new Point(0, -100, 1000)));
            floor.AddPolygon(new Polygon(new Point(-1000, -100, 0), new Point(1000, -100, 0), new Point(0, -100, -1000)));

            figures.Add(floor);

            List<ITraceable> total = new List<ITraceable>();
            foreach (Figure f in figures)
            {
                total.AddRange(f.GetPolygons());
            }
            tree.Build(total);

            //Figure mirror = new();
            //mirror.AddPolygon(new Plane(new Point(200, 0, 0), new Point(0, 0, 200), new Point(200, 200, 0), Color.White, m: new Reflective()));
            //figures.Add(mirror);
            //objects.Add(new Plane(new Point(1, 0, 0), new Point(0, 0, 1), new Point(0, 1, 1), m: new Reflective()));
        }

        public Scene(List<Figure> figArr)
        {
            cam = new Camera(new Point(0, 0, 0), new Vector3D(0, 0, 1), 128, 128, 70);
            lights = new List<Light>();
            lights.Add(new DirectionalLight(new Vector3D(0, 1, 0.5f), 1, Color.FromArgb(255, 255, 255)));
            figures = figArr;
            viewColors = new Color[cam.GetScreenHeight() * cam.GetScreenWidth()];
            ClearView();
        }

        private void ClearView()
        {
            for(int i = 0; i < viewColors.Length; i++)
            {
                viewColors[i] = Material.worldColor;
            }  
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

                    Point? interPoint = tree.FindIntersection(ray);
                    if (interPoint is not null)
                    {
                        ITraceable resObj = tree.GetIntersectionObj();
                        normalColors[i * screenWidth + j] = ((resObj.GetNormalAtPoint(interPoint) / 2 + new Vector3D(0.5f, 0.5f, 0.5f)) * 255.0f).ToColor();
                        viewColors[i * screenWidth + j] = resObj.GetColorAtPoint(ray, interPoint, tree, lights);
                    }
                }
            }
            FileWork.WritePPM(viewColors, screenHeight, screenWidth, outputPathName);
            FileWork.WritePPM(normalColors, screenHeight, screenWidth, "../../../../Examples/result_normalsmap.ppm");
        }

        // public static Point RayIntersect(Beam ray, List<ITraceable> objects, ITraceable[] intObj)
        // {
        //     float depth = float.MaxValue;
        //     Point result = null;
        //     intObj = null;
        //
        //     foreach (ITraceable obj in objects)
        //     {
        //         if (obj is not null)
        //         {
        //             Point? intersectionPoint = obj.GetIntersectionPoint(ray);
        //             if (intersectionPoint is not null) 
        //             {
        //                 float sqLenght = new Vector3D(ray.GetPosition(), intersectionPoint).SquareLength();
        //                 if (sqLenght < depth)
        //                 {
        //                     depth = sqLenght;
        //                     result = intersectionPoint;
        //                     intObj[0] = obj;
        //                 }
        //             }
        //         }
        //     }
        //     return result;
        // }
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
