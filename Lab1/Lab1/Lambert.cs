using System.Drawing;

namespace Lab1;

public class Lambert : Material
{
    public override Color RayBehaviour(Beam ray, Point interPoint, ITraceable interObj, List<ITraceable> objects, List<Light> lights)
    {
        float r = 0, g = 0, b = 0;

        foreach (Light light in lights)
        {
            int countOfRays = light.IsReplicated() ? 1 : 16;

            float illuminance = 0;
            for (int i = 0; i < countOfRays; i++)
            {
                illuminance+= CalculateIlluminance(objects, interObj, interPoint, light);
            }
            illuminance /= countOfRays;

            if (illuminance > 0)
            {
                Color color = light.GetColor();
                r += (color.R * illuminance);
                g += (color.G * illuminance);
                b += (color.B * illuminance);
            }
        }
        float maxValue = Math.Max(Math.Max(r, g), Math.Max(b, 1));

        return Color.FromArgb((byte)(r * 255.0f / maxValue),
                              (byte)(g * 255.0f / maxValue),
                              (byte)(b * 255.0f / maxValue));
    }

    private float CalculateIlluminance(List<ITraceable> objects, ITraceable thisObject, Point point, Light light)
    {
        Vector3D norm = thisObject.GetNormalAtPoint(point);
        Vector3D dir = light.GetRayDirection(norm, point);
        float dotProduct = norm * dir;

        if (dotProduct > 0 && IsVisible(objects, thisObject, point, dir, light.IsBeamRay()))
        {
            return light.GetIntensity() * light.GetAttenuationCoefficient(point) * dotProduct / norm.Length() / dir.Length();
        }
        return 0;
    }


    private bool IsVisible(List<ITraceable> objects, ITraceable thisObject, Point start, Vector3D dir, bool isBeam)
    {
        Beam lightRay = isBeam ? new(start, dir) : new(start, new Vector3D(start, start + dir));

        float sqDist = isBeam ? -1 : dir.SquareLength();

        foreach (ITraceable obj in objects)
        {
            if (obj is not null && obj != thisObject)
            {
                Point? intersectionPoint = obj.GetIntersectionPoint(lightRay);

                if (intersectionPoint is not null)
                {
                    if (isBeam || (new Vector3D(start, intersectionPoint)).SquareLength() < sqDist)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}