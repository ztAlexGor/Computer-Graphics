using System.Drawing;

namespace Lab1
{
    public abstract class SceneObject
    {
        protected Material material;
        protected Color color;
        protected AABB aabb;

        public abstract Point? GetIntersectionPoint(Beam ray);

        public abstract Vector3D GetNormalAtPoint(Point point);

        public abstract Color GetColorAtPoint(Beam startRay, Point interPoint, BVHTree tree, List<Light> lights);

        public abstract SceneObject Rotate(float alpha = 0, float beta = 0, float gamma = 0);

        public abstract SceneObject Scale(float sx = 0, float sy = 0, float sz = 0);

        public abstract SceneObject Translate(float x = 0, float y = 0, float z = 0);

        public abstract Point? GetUV();

        public abstract (Point?, Point?, Point?) GetVT();
        
        protected abstract float[] BoxBordersInit();

        public float[] GetBoxBorders() => aabb.GetBorders();

        public float[] GetBoxCenter() => aabb.GetCenter();

        public AABB GetAABB() => aabb;
    }
}
