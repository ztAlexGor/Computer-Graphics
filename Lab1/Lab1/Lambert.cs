using System.Drawing;

namespace Lab1;

public class Lambert : Material
{
    public override Color RayBehaviour(Beam ray, Point interPoint, ITraceable interObj, List<ITraceable> objects, List<Light> lights)
    {
        Random rand = new Random();
        return Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255));
        byte r = 0;
        byte g = 0;
        byte b = 0;
        foreach (Light light in lights)
        {
            Color color = light.GetColor();
            float illuminance = light.CalculateIntensity(objects, interObj, interPoint);

            r += (byte)Math.Round(illuminance * color.R);
            g += (byte)Math.Round(illuminance * color.G);
            b += (byte)Math.Round(illuminance * color.B);
        }
        return Color.FromArgb(r, g, b);
    }
}