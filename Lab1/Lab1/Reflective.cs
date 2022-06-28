using System.Drawing;

namespace Lab1;

public class Reflective : Material
{
    private int maxReflectionsNumber;
    private float reflectionCoeficient;

    public Reflective(int maxReflectionsNumber, float reflectionCoeficient)
    {
        this.maxReflectionsNumber = maxReflectionsNumber;
        this.reflectionCoeficient = reflectionCoeficient;
    }

    public override Color RayBehaviour(Beam ray, Point interPoint, ITraceable interObj, BVHTree tree, List<Light> lights)
    {
        return RayBehaviour(ray, interPoint, interObj, tree, lights, maxReflectionsNumber);
    }

    private Color RayBehaviour(Beam ray, Point interPoint, ITraceable interObj, BVHTree tree, List<Light> lights, int numOfReflections)
    {
        if (numOfReflections <= 0) return Color.Black;

        Vector3D newStartVector = ray.GetDirection() + (interObj.GetNormalAtPoint(interPoint) * 2.0f);
        interPoint += (interObj.GetNormalAtPoint(interPoint) * 0.0001f);
        Beam newStartRay = new(interPoint, newStartVector);
        Point? newInterPoint = tree.FindIntersection(ray);
        ITraceable? resObject = tree.GetIntersectionObj();
        if (newInterPoint is not null)
        {
            Color tempColor = resObject.GetColorAtPoint(newStartRay, newInterPoint, tree, lights);
            return Color.FromArgb((byte)(tempColor.R * reflectionCoeficient),
                                  (byte)(tempColor.G * reflectionCoeficient),
                                  (byte)(tempColor.B * reflectionCoeficient));
        }

        return worldColor;
    }
}