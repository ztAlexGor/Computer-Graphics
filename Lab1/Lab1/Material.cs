using System.Drawing;

namespace Lab1;

public abstract class Material
{
    public static Color worldColor = Color.FromArgb(128, 255, 255);
    protected int height = 0, width = 0;
    protected Color[]? texture;
    public abstract Color RayBehaviour(Beam startRay, Point interPoint, ITraceable interObj, BVHTree tree, List<Light> lights);

    protected void LoadTexture(string path)
    {
        StreamReader? reader = null;
        try
        {
            reader = new(path);
            reader.ReadLine();
            string? line = reader.ReadLine();

            string[] splittedLine = line.Split(' ');
            height = int.Parse(splittedLine[1]);
            width = int.Parse(splittedLine[2]);
            
            reader.ReadLine();

            texture = new Color[height * width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    texture[i * width + j] = Color.FromArgb(reader.Read(), reader.Read(), reader.Read());
                }
            }
        }
        catch (Exception)
        {
            height = 0;
            width = 0;
            texture = null;
        }
        finally
        {
            reader?.Close();
        }
    }
}