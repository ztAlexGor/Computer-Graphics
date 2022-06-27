using System.Drawing;

namespace Lab1;

public abstract class Material
{
    public abstract Color RayBehaviour(Beam startRay, Point interPoint, ITraceable interObj, BVHTree tree, List<Light> lights);
}