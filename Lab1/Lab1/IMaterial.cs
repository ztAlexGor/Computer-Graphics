using System.Drawing;

namespace Lab1;

public interface IMaterial
{
    public Color RayBehaviour(Beam startRay, Point interPoint, ITraceable interObj, List<ITraceable> objects, List<Light> lights);
}