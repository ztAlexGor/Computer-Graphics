﻿using System.Drawing;

namespace Lab1
{
    public class Scene
    {
        private readonly Camera cam;
        private readonly DirectionalLight light;
        private readonly List<ITraceable> objects;
        private float[] viewValues;

        public Scene(string inputPathName)
        {
            cam = new Camera(new Point(0, 0, 0.75f), new Vector3D(0, 0, -1), 128, 128, 70);
            light = new DirectionalLight(new Vector3D(1, 1, 1), 1, Color.FromArgb(255, 255, 255, 255));
            viewValues = new float[cam.GetScreenHeight() * cam.GetScreenWidth()];
            objects = FileWork.ReadObj(inputPathName).GetObjects();

            ClearView();
        }


        public Scene(List<ITraceable> objArr)
        {
            cam = new Camera(new Point(0, 0, 0), new Vector3D(0, 0, 1), 20, 20, 5);
            light = new DirectionalLight(new Vector3D(0, 1, 0.5f), 1, Color.FromArgb(255, 255, 255));
            objects = objArr;
            viewValues = new float[cam.GetScreenHeight() * cam.GetScreenWidth()];
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
                        Beam lightRay = new(intersectionPoint, -light.GetDirection());

                        if (RayIsIntersect(lightRay, resObj))
                        {
                            viewValues[i * screenWidth + j] = 0;
                        }
                        else
                        {
                            float view = -(resObj.GetNormalAtPoint(intersectionPoint) * light.GetDirection());
                            viewValues[i * screenWidth + j] = view >= 0 ? view : 0;
                            //viewValues[i * screenWidth + j] = view >= 0 ? view : -view;
                        }
                        
                    }
                }
            }
            //ViewOutput();
            FileWork.WritePPM(viewValues, screenHeight, screenWidth, outputPathName);
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
                    if (intersectionPoint is not null)
                    {
                        Vector3D objNormal = obj.GetNormalAtPoint(intersectionPoint);
                        float dotProductValue = -(objNormal * light.GetDirection());
                        if (intersectionPoint.Z() < depth)
                        {
                            depth = intersectionPoint.Z();
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
