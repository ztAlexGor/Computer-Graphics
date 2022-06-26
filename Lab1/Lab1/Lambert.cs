using System.Drawing;

namespace Lab1;

public class Lambert : Material
{
    public override Color RayBehaviour(Beam ray, Point interPoint, ITraceable interObj, BoxTree tree, List<Light> lights)
    {
        int r = 0;
        int g = 0;
        int b = 0;


        foreach (Light light in lights)
        {
            Color color = light.GetColor();
            float illuminance = light.CalculateIntensity(tree, interObj, interPoint);

            r += (byte)Math.Round(illuminance * color.R);
            g += (byte)Math.Round(illuminance * color.G);
            b += (byte)Math.Round(illuminance * color.B);
        }
        return Color.FromArgb(r, g, b);
        /*        foreach (Light light in lights)
                {
                    Color color = light.GetColor();
                    float illuminance = light.CalculateIntensity(objects, interObj, interPoint);

                    r += (int)(illuminance * color.R);
                    g += (int)(illuminance * color.G);
                    b += (int)(illuminance * color.B);
                }

                //return Color.FromArgb(r, g, b);
                int maxValue = Math.Max(Math.Max(r, g), Math.Max(b, 1));
                return Color.FromArgb(r * 255 / maxValue, g * 255 / maxValue, b * 255 / maxValue);*/
    }
}