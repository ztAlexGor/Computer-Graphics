using System.Drawing;

namespace Lab1;

public class Lambert : Material
{
    public Lambert(string? path = null)
    {
        if (path is not null)
        {
            LoadTexture(path);
        }
        else
        {
            texture = null;
            height = 0;
            width = 0;
        }
    }

    public override Color RayBehaviour(Beam ray, Point interPoint, SceneObject interObj, BVHTree tree, List<Light> lights)
    {
        float r = 0, g = 0, b = 0;

        //applying texture
        Point? uvw = interObj.GetUV();
        (Point? vt1, Point? vt2, Point? vt3) = interObj.GetVT();

        if (uvw is not null && vt1 is not null && vt2 is not null && vt3 is not null)
        {
            Point texturePosition = vt1 * uvw.X() + vt2 * uvw.Y() + vt3 * uvw.Z();
            float textureX = texturePosition.X() * width;
            float textureY = texturePosition.Y() * height;

            int tidx = (int)(textureY * width + textureX) % (width * height);
            r = texture[tidx].R;
            g = texture[tidx].G;
            b = texture[tidx].B;
        }

        //applying light 
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

    private float CalculateIlluminance(BVHTree tree, SceneObject thisObject, Point point, Light light)
    {
        Vector3D norm = thisObject.GetNormalAtPoint(point);
        float illuminance = 0;
        int rayNumber = 0;

        point += new Point(norm * 0.001f);
        foreach (Vector3D dir in light.GetRayDirection(norm, point))
        {
            float dotProduct = norm * dir;
            if (dotProduct > 0 && IsVisible(tree, point, dir, light.IsRayInfinite()))
            {
                illuminance += light.GetIntensity() * light.GetAttenuationCoefficient(point) * dotProduct / norm.Length() / dir.Length();
            }
            rayNumber++;
        }

        return illuminance / rayNumber;
    }


    private bool IsVisible(BVHTree tree, Point start, Vector3D dir, bool bIgnoreDistance)
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