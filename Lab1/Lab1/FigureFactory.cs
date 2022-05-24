namespace Lab1
{
    internal class FigureFactory
    {
        public static Figure CreateSphere()
        {
            return new Figure();
        }
        public static Figure CreateParalelipiped(float a, float b)
        {
            return new Figure();
        }
        public static Figure CreateCube(float a)
        {
            return CreateParalelipiped(a, a);
        }
        public static Figure CreatePlane()
        {
            return new Figure();
        }
        public static Figure Create()
        {
            return new Figure();
        }
    }
}
