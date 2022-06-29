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
            cam = new Camera(new Point(50, 100, -350f), new Vector3D(0, 0, 0), 500, 500, 300);
            lights = new List<Light>();
            figures = new List<Figure>();
            tree = new BVHTree(10);

            //SetScene(inputPathName);
            SetMirrorScene();
            //SetCarScene();
            //SetCowScene();
            BuildTree();

            viewColors = new Color[cam.GetScreenHeight() * cam.GetScreenWidth()];
            normalColors = new Color[cam.GetScreenHeight() * cam.GetScreenWidth()];
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
                        SceneObject resObj = tree.GetIntersectionObj();
                        normalColors[i * screenWidth + j] = ((resObj.GetNormalAtPoint(interPoint) / 2 + new Vector3D(0.5f, 0.5f, 0.5f)) * 255.0f).ToColor();
                        viewColors[i * screenWidth + j] = resObj.GetColorAtPoint(ray, interPoint, tree, lights);
                    }
                }
            }
            FileWork.WritePPM(viewColors, screenHeight, screenWidth, outputPathName);
            FileWork.WritePPM(normalColors, screenHeight, screenWidth, "../../../../Examples/result_normals_map.ppm");
        }

        private void ClearView()
        {
            for (int i = 0; i < viewColors.Length; i++)
            {
                viewColors[i] = Material.worldColor;
                normalColors[i] = Color.FromArgb(0, 0, 0);
            }
        }

        public void BuildTree()
        {
            List<SceneObject> total = new List<SceneObject>();
            foreach (Figure f in figures)
            {
                total.AddRange(f.GetObjects());
                f.Clear();
            }
            tree.Build(total);
        }


        //Scenes
        public void SetScene(string inputPathName)
        {
            //Lights
            lights.Add(new DirectionalLight(new Vector3D(-1, -1, 1), 1, Color.DodgerBlue));
            //lights.Add(new DirectionalLight(new Vector3D(0, -1, 0), 1, Color.Red));
            //lights.Add(new PointLight(new Point(150, 0, 0), 1, Color.DeepPink));
            //lights.Add(new Light(2f, Color.White));

            Figure f = new Figure(FileWork.ReadObj(inputPathName).GetObjects(Color.White, new Lambert("../../../../Textures/tire.ppm")));

            // cow.Rotate(beta: (float)Math.PI, gamma: (float)Math.PI / 2);
            f.Scale(100, 100, 100);
            f.Translate(x: 20);
            figures.Add(f);

            Figure floor = new Figure();
            floor.AddObject(new Polygon(new Point(1000, -100, 0), new Point(-1000, -100, 0), new Point(0, -100, 1000)));
            floor.AddObject(new Polygon(new Point(-1000, -100, 0), new Point(1000, -100, 0), new Point(0, -100, -1000)));

            figures.Add(floor);
        }

        public void SetCowScene(string inputPathName = "../../../../Examples/cow.obj")
        {
            cam.SetCamera(new Camera(new Point(50, 100, -350f), new Vector3D(0, 0, 0), 600, 600, 300));

            lights.Add(new DirectionalLight(new Vector3D(-1, -1, 1), 1, Color.DodgerBlue));
            //lights.Add(new Light(0.5f, Color.White));

            Figure cow = new Figure(FileWork.ReadObj(inputPathName).GetObjects(Color.White, new Lambert("../../../../Textures/tire.ppm")));
            cow.Rotate(beta: (float)Math.PI, gamma: (float)Math.PI / 2);
            cow.Scale(300, 300, 300);

            Figure floor = new Figure();
            floor.AddObject(new Polygon(new Point(1000, -100, 0), new Point(-1000, -100, 0), new Point(0, -100, 1000)));
            floor.AddObject(new Polygon(new Point(-1000, -100, 0), new Point(1000, -100, 0), new Point(0, -100, -1000)));

            Figure mirror = new();
            mirror.AddObject(new Sphere(new Point(400, 0, -10), 100, Color.White, m: new Reflective(10, 0.8f)));

            figures.Add(cow);
            figures.Add(floor);
            figures.Add(mirror);
        }

        public void SetMirrorScene(string inputPathName = "../../../../Examples/cow.obj")
        {
            cam.SetCamera(new Camera(new Point(0, 100, -350f), new Vector3D(0, 0, 0), 800, 800, 400));

            lights.Add(new DirectionalLight(new Vector3D(-1, -1, 1), 1, Color.DodgerBlue));
            //lights.Add(new Light(0.5f, Color.White));

            Figure cow = new Figure(FileWork.ReadObj(inputPathName).GetObjects(Color.White, new Lambert("../../../../Textures/tire.ppm")));
            cow.Rotate(alpha: 0, gamma: (float)Math.PI / 2);
            cow.Scale(300, 300, 300);
            cow.Translate(y: -5);

            Figure floor = new Figure();
            floor.AddObject(new Polygon(new Point(1000, -100, 0), new Point(-1000, -100, 0), new Point(0, -100, 1000)));
            floor.AddObject(new Polygon(new Point(-1000, -100, 0), new Point(1000, -100, 0), new Point(0, -100, -1000)));

            Figure mirror = new();
            mirror.AddObject(new Polygon(new Point(300, -100, 0), new Point(0, -100, 500), new Point(0, 1000, 500), Color.White, m: new Reflective(10, 0.9f)));
            mirror.AddObject(new Polygon(new Point(0, -100, 500), new Point(-300, -100, 0), new Point(0, 1000, 500), Color.White, m: new Reflective(10, 0.9f)));
            figures.Add(cow);
            figures.Add(floor);
            figures.Add(mirror);
        }

        public void SetCarScene(string inputPathName = "../../../../Examples/car.obj")
        {
            cam.SetCamera(new Camera(new Point(0, 100, -350f), new Vector3D(0, 0, 0), 600, 600, 300));

            lights.Add(new DirectionalLight(new Vector3D(-1, -1, 1), 1, Color.DodgerBlue));
            //lights.Add(new Light(0.5f, Color.White));

            Figure car = new Figure(FileWork.ReadObj(inputPathName).GetObjects(Color.White, new Lambert("../../../../Textures/tire.ppm")));
            //car.Rotate(alpha: 0, beta: (float)Math.PI, gamma: (float)Math.PI / 2);
            car.Scale(100, 100, 100);
            //car.Translate(y: -5);

            Figure floor = new Figure();
            floor.AddObject(new Polygon(new Point(1000, 0, 0), new Point(-1000, 0, 0), new Point(0, 0, 1000)));
            floor.AddObject(new Polygon(new Point(-1000, 0, 0), new Point(1000, 0, 0), new Point(0, 0, -1000)));


            figures.Add(car);
            figures.Add(floor);
        }
    }
}
