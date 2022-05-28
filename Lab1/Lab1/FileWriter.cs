namespace Lab1;

public class FileWriter
{
    public static void WritePPM(float[] view, int h, int w, string path)
    {
        using (StreamWriter writer = new StreamWriter(path, false))
        {
            writer.Write("P6\n" + h + " " + w + "\n255\n");
        }

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Append)))
        {
            foreach (float v in view)
            {
                int[] p = { (int)Math.Round(255 * v), (int)Math.Round(255 * v), (int)Math.Round(255 * v) };
                foreach (int c in p)
                {
                    writer.Write((Byte)c);
                }
            }
        }
    }
}