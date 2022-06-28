using System.Drawing;

namespace Lab1;

public class Lambert : Material
{
    public Lambert()
    {
        // LoadTexture(path);
    }
    public override Color RayBehaviour(Beam ray, Point interPoint, ITraceable interObj, BVHTree tree, List<Light> lights)
    {
        float r = 0, g = 0, b = 0;

        Point? uvw = interObj.GetUV();
        (Point? vt1, Point? vt2, Point? vt3) = interObj.GetVT();
        /*Color[][] colors = new Color[2][];
        for (int i = 0; i < 2; i++)
        {
            colors[i] = new Color[2];
            for (int j = 0; j < 2; j++)
            {
                colors[i][j] = 
            }
        }*/

        if (uvw is not null && vt1 is not null && vt2 is not null && vt3 is not null)
        {
            Point texturePosition = vt1 * uvw.X() + vt2 * uvw.Y() + vt3 * uvw.Z();
            float textureX = texturePosition.X() * Material.width;
            float textureY = texturePosition.Y() * Material.height;

            /*            if ((textureX <= 0.5f && textureY >= 0.5f) || (textureX >= 0.5f && textureY <= 0.5f))
                        {
                            r = 0;
                            g = 0;
                            b = 128;
                        }
                        else
                        {
                            r = 128;
                            g = 0;
                            b = 0;
                        }*/
            int tidx = Math.Min((int)((int)(textureY * Material.width) + (int)textureX), Material.width * Material.height - 1);
            
            r = (Material.texture[tidx]).R;
            g = (Material.texture[tidx]).G;
            b = (Material.texture[tidx]).B;
        }

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

    private float CalculateIlluminance(BVHTree tree, ITraceable thisObject, Point point, Light light)
    {
        Vector3D norm = thisObject.GetNormalAtPoint(point);
        float illuminance = 0;
        int rayNumber = 0;

        point += new Point(norm * 0.001f);
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


    private bool IsVisible(BVHTree tree, ITraceable thisObject, Point start, Vector3D dir, bool bIgnoreDistance)
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