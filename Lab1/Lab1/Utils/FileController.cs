using System.Globalization;
using System.Drawing;

namespace Lab1;

public class FileWork
{
    public static readonly int VAR_NUMBER = 3;
    public static void WritePPM(Color[] view, int h, int w, string path)
    {
        using (StreamWriter writer = new(path, false))
        {
            writer.Write("P6\n" + h + " " + w + "\n255\n");
        }

        using (BinaryWriter writer = new(File.Open(path, FileMode.Append)))
        {
            foreach (Color v in view)
            {
                writer.Write(v.R);
                writer.Write(v.G);
                writer.Write(v.B);
            }
        }
    }

    public static ObjStructure ReadObj(string path)
    {
        NumberFormatInfo inf = CultureInfo.InvariantCulture.NumberFormat;
        ObjStructure obj = new();
        StreamReader? reader = null;
        try
        {
            reader = new(path);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] splittedLine;
                switch (line[0])
                {
                    case 'v':
                        switch (line[1])
                        {
                            case 'n':
                                splittedLine = line.Split(' ');
                                obj.AddVn(new float?[] { float.Parse(splittedLine[3], inf), float.Parse(splittedLine[2], inf), float.Parse(splittedLine[1], inf) });
                                break;
                            case 't':
                                splittedLine = line.Split(' ');
                                obj.AddVt(new float?[] { float.Parse(splittedLine[2], inf), float.Parse(splittedLine[1], inf)});
                                break;
                            default:
                                splittedLine = line.Split(' ');
                                obj.AddV(new float?[] { float.Parse(splittedLine[3], inf), float.Parse(splittedLine[2], inf), float.Parse(splittedLine[1], inf) });
                                break;
                        }
                        break;

                    case 'f':
                        splittedLine = line.Split(' ');
                        int?[] res = new int?[splittedLine.Length * VAR_NUMBER];
                        for (int i = 1; i < splittedLine.Length; i++)
                        {
                            string[] buff = splittedLine[i].Split('/');
                            for (int j = 0; j < VAR_NUMBER; j++)
                            {
                                res[(i - 1) * VAR_NUMBER + j] = (buff[j] != "") ? int.Parse(buff[j]) : null;
                            }
                        }
                        obj.AddF(res);
                        break;

                    default:
                        break;
                }
            }
        } 
        catch (Exception){ }
        finally
        {
            reader?.Close();
        }
        return obj;
    }

}