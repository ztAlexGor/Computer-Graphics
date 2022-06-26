using System.Drawing;

namespace Lab1;

public class Lambert : Material
{
    public override Color RayBehaviour(Beam ray, Point interPoint, ITraceable interObj, BoxTree tree, List<Light> lights)
    {
        float r = 0, g = 0, b = 0;

        foreach (Light light in lights)
        {
            float illuminance = CalculateIlluminance(tree, interObj, interPoint, light);

            if (illuminance > 0)
            {
                Color color = light.GetColor();
                r += (color.R * illuminance);
                g += (color.G * illuminance);
                b += (color.B * illuminance);
            }
        }
        float maxValue = Math.Max(Math.Max(r, g), b);
        if (maxValue > 255)
        {
            return Color.FromArgb((byte)(r * 255.0f / maxValue),
                                  (byte)(g * 255.0f / maxValue),
                                  (byte)(b * 255.0f / maxValue));
        }
        return Color.FromArgb((byte)r, (byte)g, (byte)b);

    }

    private float CalculateIlluminance(BoxTree tree, ITraceable thisObject, Point point, Light light)
    {
        Vector3D norm = thisObject.GetNormalAtPoint(point);
        float illuminance = 0;
        int rayNumber = 0;

        foreach (Vector3D dir in light.GetRayDirection(norm, point))
        {
            float dotProduct = norm * dir;

            if (dotProduct > 0 && IsVisible(tree, thisObject, point, dir, light.IsRayInfinite()))
            {
                illuminance += light.GetIntensity() * light.GetAttenuationCoefficient(point) * dotProduct / norm.Length() / dir.Length();
            }
            rayNumber++;
        }

        return illuminance / rayNumber;
    }


    private bool IsVisible(BoxTree tree, ITraceable thisObject, Point start, Vector3D dir, bool bIgnoreDistance)
    {
        Beam lightRay = bIgnoreDistance ? new(start, dir) : new(start, new Vector3D(start, start + dir));
        Point? intersectionPoint = tree.FindIntersection(lightRay);

        if (intersectionPoint is not null && 
            (bIgnoreDistance || new Vector3D(start, intersectionPoint).SquareLength() < dir.SquareLength()))
        {
            return false;
        }
        return true;
    }
}