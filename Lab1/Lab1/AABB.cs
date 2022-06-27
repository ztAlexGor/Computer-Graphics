namespace Lab1
{
    public class AABB
    {
        private float[] borders; // 0, 2, 4 - max  |  1, 3, 5 - min
        private float[] center;
        //private Point min, max, center;

        public AABB(float[] boxBorders)
        {
            borders = boxBorders;
            center = ComputeCenter();
        }

        public AABB(List<ITraceable> items)
        {
            borders = new float[] { float.MinValue, float.MaxValue,
                                    float.MinValue, float.MaxValue,
                                    float.MinValue, float.MaxValue };

            foreach (ITraceable item in items)
            {
                float[] itemBorders = item.GetAABB().GetBorders();
                for (int i = 0; i < 3; i++)
                {
                    borders[i * 2] = Math.Max(itemBorders[i * 2], borders[i * 2]);
                    borders[i * 2 + 1] = Math.Min(itemBorders[i * 2 + 1], borders[i * 2 + 1]);
                }
            }
            center = ComputeCenter();
        }

        private float[] ComputeCenter() =>
            new float[] { borders[1] + (borders[0] - borders[1]) / 2,
                          borders[3] + (borders[2] - borders[3]) / 2,
                          borders[5] + (borders[4] - borders[5]) / 2 };
        public float[] GetBorders() => borders;
        public float[] GetCenter() => center;

        public int GetLongestDimension()
        {
            float sizeX = borders[0] - borders[1];
            float sizeY = borders[2] - borders[3];
            float sizeZ = borders[4] - borders[5];

            float maxSize = Math.Max(Math.Max(sizeX, sizeY), sizeZ);

            if (sizeX == maxSize) return 0;
            if (sizeY == maxSize) return 1;
            return 2;
        }








        public bool IsIntersect(Beam ray)
        {
            Point sPoint = ray.GetPosition();
            Vector3D dirVector = ray.GetDirection();
            float[] start = new float[3] { sPoint.X(), sPoint.Y(), sPoint.Z() };
            float[] direction = new float[] { dirVector.X(), dirVector.Y(), dirVector.Z() };
            for (int i = 0; i < 3; i++)
            {
                if (direction[i] != 0f)
                {
                    float t1 = (borders[i * 2 + 1] - start[i]) / direction[i];
                    float t2 = (borders[i * 2] - start[i]) / direction[i];
                    if (t1 <= 0 && t2 <= 0)
                    {
                        return false;
                    }

                    if (t1 > 0)
                    {
                        Point final = sPoint + (dirVector * t1);
                        if (IsItIn(final, i, ray))
                        {
                            return true;
                        }
                    }

                    if (t2 > 0)
                    {
                        Point final = sPoint + (dirVector * t2);
                        if (IsItIn(final, i, ray))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool IsItIn(Point p, int i, Beam ray)
        {
            float[] point = new float[3] { p.X(), p.Y(), p.Z() };

            if (point[(i + 1) % 3] >= borders[((i + 1) % 3) * 2 + 1] &&
                point[(i + 1) % 3] <= borders[((i + 1) % 3) * 2] &&
                point[(i + 2) % 3] >= borders[((i + 2) % 3) * 2 + 1] &&
                point[(i + 2) % 3] <= borders[((i + 2) % 3) * 2])
            {
                return true;
                /*if (tree.bestPoint is null)
                {
                    return true;
                }
                float oldBest = new Vector3D(ray.GetPosition(), tree.bestPoint).SquareLength();
                float possibleBest = new Vector3D(ray.GetPosition(), p).SquareLength();
                return possibleBest < oldBest;*/
            }

            return false;
        }
        /*        public AABB(Point min, Point max)
                {
                    this.min = new Point(min);
                    this.max = new Point(max);
                    center = new Point((min + max) / 2);
                }*/
        /*        public AABB(List<ITraceable> items)
                {
                    min = new Point(float.MaxValue, float.MaxValue, float.MaxValue);
                    max = new Point(float.MinValue, float.MinValue, float.MinValue);

                    foreach (ITraceable item in items)
                    {
                        if (min.X() < )
                        if (min > item.GetAABB().GetMin())
                        {
                            min = item.GetAABB().GetMin();
                        }

                        if (max < item.GetAABB().GetMax())
                        {
                            max = item.GetAABB().GetMax();
                        }
                    }
                }*/

        /*        public Point GetMin() => min;
                public Point GetMax() => max;
                public Point GetCenter() => center;*/


        /*        public int GetLongestDimension()
                {
                    float sizeX = max.X() - min.X();
                    float sizeY = max.Y() - min.Y();
                    float sizeZ = max.Z() - min.Z();

                    float maxSize = Math.Max(Math.Max(sizeX, sizeY), sizeZ);

                    if (sizeX == maxSize)return 0;
                    if (sizeY == maxSize)return 1;
                    return 2;
                }*/

        /*        public int Compare(AABB other, int dimension)
                {
                    if (dimension == 0) return center.X().CompareTo(other.GetCenter().X());
                    if (dimension == 1) return center.Y().CompareTo(other.GetCenter().Y());
                    return center.Z().CompareTo(other.GetCenter().Z());
                }*/
    }
}
