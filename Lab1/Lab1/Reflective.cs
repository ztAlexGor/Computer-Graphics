using System.Drawing;

namespace Lab1;

public class Reflective : Material
{
    public override Color RayBehaviour(Beam ray, Point interPoint, ITraceable interObj, List<ITraceable> objects, List<Light> lights)
    {
        Vector3D newStartVector = ray.GetDirection() + (interObj.GetNormalAtPoint(interPoint) * 2f);
        interPoint += (interObj.GetNormalAtPoint(interPoint) * 0.00001f);
        Beam newStartRay = new(interPoint, newStartVector);
        ITraceable resObject;
        Point? newInterPoint = Scene.RayIntersect(newStartRay, objects, out resObject);
        if (newInterPoint is not null)
        {
            return resObject.GetColorAtPoint(newStartRay, newInterPoint, objects, lights);
        }

        return new Color();
    }
}