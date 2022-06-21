using System.Text.RegularExpressions;


namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            string inpPath = "../../../../Examples/cow.obj";
            string outPath = "../../../../Examples/result.ppm";

            if (args.Length == 2)
            {
                inpPath = Regex.Replace(args[0], "--source=", "");
                outPath = Regex.Replace(args[1], "--output=", "");
            }

            Scene scene = new Scene(inpPath);
            scene.RayProcessing(outPath);
        }
    }
}



