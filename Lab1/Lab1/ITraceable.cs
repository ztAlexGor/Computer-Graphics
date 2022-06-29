using System.Drawing;

namespace Lab1
{
    public abstract class SceneObject
    {
        public abstract Point? GetIntersectionPoint(Beam ray);

        public abstract Vector3D GetNormalAtPoint(Point point);

        public abstract Color GetColorAtPoint(Beam startRay, Point interPoint, BVHTree tree, List<Light> lights);

        public abstract SceneObject Rotate(float alpha = 0, float beta = 0, float gamma = 0);

        public abstract SceneObject Scale(float sx = 0, float sy = 0, float sz = 0);

        public abstract SceneObject Translate(float x = 0, float y = 0, float z = 0);

        public abstract float[] GetBoxBorders();

        public abstract float[] GetBoxCenter();

        public abstract AABB GetAABB();

        public abstract Point? GetUV();

        public abstract (Point?, Point?, Point?) GetVT();

        //set private after changing ITraceable from interface to abstract class
        public abstract float[] BoxBordersInit();
    }
}
