﻿using System.Drawing;

namespace Lab1
{
    public class Plane : SceneObject
    {
        protected readonly Point a;
        protected readonly Point b;
        protected readonly Point c;
        protected readonly Vector3D normal;

        public Plane(Point a, Point b, Point c, Vector3D? v = null, Color? color = null, Material? m = null)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            normal = (v is null) ? Vector3D.Normalize(Vector3D.CrossProduct(new Vector3D(a, b), new Vector3D(a, c))) : v;
            material = (m is null) ? new Lambert() : m;
            this.color = (color is null) ? Color.White : (Color)color;
            aabb = new AABB(BoxBordersInit());
        }

        public Plane(Plane plane)
        {
            if (plane == null)
            {
                throw new ArgumentNullException(nameof(plane), "Trying to copy a null plain/polygon object!");
            }
            a = plane.a;
            b = plane.b;
            c = plane.c;
            color = plane.color;
            material = plane.material;
            normal = plane.normal;
            aabb = plane.GetAABB();
        }

        public override Point? GetIntersectionPoint(Beam ray)
        {
            Vector3D d = ray.GetDirection();
            Vector3D k = new Vector3D(ray.GetPosition(), c);

            if (d * normal != 0)
            {
                float t = (k * normal) / (d * normal);
                return t > 0 ? (ray.GetPosition() + d * t) : null;
            }

            return null;
        }

        public override Vector3D GetNormalAtPoint(Point point) => normal;

        public override Color GetColorAtPoint(Beam startRay, Point interPoint, BVHTree tree, List<Light> lights) =>
            material.RayBehaviour(startRay, interPoint, this, tree, lights);

        public override SceneObject Rotate(float alpha = 0, float beta = 0, float gamma = 0) =>
            new Plane(a.Rotate(alpha, beta, gamma), b.Rotate(alpha, beta, gamma), c.Rotate(alpha, beta, gamma), color: color, m: material);

        public override SceneObject Scale(float sx = 0, float sy = 0, float sz = 0) => new Plane(this);

        public override SceneObject Translate(float x = 0, float y = 0, float z = 0) =>
            new Plane(a.Translate(x, y, z), b.Translate(x, y, z), c.Translate(x, y, z), color: color, m: material);

        protected override float[] BoxBordersInit() =>
            new float[] { float.MaxValue, float.MinValue,
                          float.MaxValue, float.MinValue,
                          float.MaxValue, float.MinValue };

        public override Point? GetUV() => null;

        public override (Point?, Point?, Point?) GetVT() => (null, null, null);
    }
}
