using System.Drawing;

namespace Lab1;

public class Reflective : Material
{
    public override Color RayBehaviour(Beam ray, Point interPoint, ITraceable interObj, BVHTree tree, List<Light> lights)
    {
        Vector3D newStartVector = ray.GetDirection() + (interObj.GetNormalAtPoint(interPoint) * 2.0f);
        interPoint += (interObj.GetNormalAtPoint(interPoint) * 0.0001f);
        Beam newStartRay = new(interPoint, newStartVector);
        Point? newInterPoint = tree.FindIntersection(ray);
        ITraceable? resObject = tree.GetIntersectionObj();
        if (newInterPoint is not null)
        {
            return resObject.GetColorAtPoint(newStartRay, newInterPoint, tree, lights);
        }

        return new Color();
    }
}