using System.Globalization;

namespace Lab1;

public class FileWork
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

    public static ObjStructure ReadObj(string path)
    {
        NumberFormatInfo inf = CultureInfo.InvariantCulture.NumberFormat;
        ObjStructure obj = new ObjStructure();
        using (StreamReader reader = new StreamReader(path))
        {
            String line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Substring(0,2) == "vn")
                {
                    String[] splittedLine = line.Split(' ');
                    obj.AddVn(new float[]{float.Parse(splittedLine[3], inf), float.Parse(splittedLine[2], inf), float.Parse(splittedLine[1], inf)});
                } else if (line.Substring(0, 2) == "vt")
                {
                    String[] splittedLine = line.Split(' ');
                    obj.AddVt(new float[]{float.Parse(splittedLine[3], inf), float.Parse(splittedLine[2], inf), float.Parse(splittedLine[1], inf)});
                }else if (line[0] == 'v')
                {
                    String[] splittedLine = line.Split(' ');
                    obj.AddV(new float[]{float.Parse(splittedLine[3], inf), float.Parse(splittedLine[2], inf), float.Parse(splittedLine[1], inf)});
                }else if (line[0] == 'f')
                {
                    String[] splittedLine = line.Split(' ');
                    int?[] res = new int?[9];
                    for (int i = 1; i < splittedLine.Length; i++)
                    {
                        String[] buff = splittedLine[i].Split('/');
                        for(int j = 0; j < 3; j++)
                        {
                            if (buff[j] != "")
                                res[(i - 1) * 3 + j] = int.Parse(buff[j]);
                            else
                                res[(i - 1) * 3 + j] = null;
                        }
                    } 
                    obj.AddF(res);
                }
            }
        }
        return obj;
    }

}