namespace Lab1;

public interface IMaterial
{
    public MyColor RayBehaviour(Beam ray, ITraceable interObj, List<ITraceable> objects, List<Light> lights);
}