namespace Lab1
{
    public class AABB
    {
        private float[] borders; // 0, 2, 4 - max  |  1, 3, 5 - min
        private float[] center;

        public AABB(float[] boxBorders)
        {
            borders = boxBorders;
            center = ComputeCenter();
        }

        public AABB(List<SceneObject> items)
        {
            borders = new float[] { float.MinValue, float.MaxValue,
                                    float.MinValue, float.MaxValue,
                                    float.MinValue, float.MaxValue };

            foreach (SceneObject item in items)
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


        public bool IsIntersect(Beam ray, float bestDist = float.MaxValue)
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


                    Point final = sPoint + (dirVector * t1);
                    if (IsItIn(final, i, ray, t1 * t2 > 0 ? bestDist : float.MaxValue))
                    {
                        return true;
                    }

                    final = sPoint + (dirVector * t2);
                    if (IsItIn(final, i, ray, t1 * t2 > 0 ? bestDist : float.MaxValue))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsItIn(Point p, int i, Beam ray, float bestDist)
        {
            float[] point = new float[3] { p.X(), p.Y(), p.Z() };

            if (point[(i + 1) % 3] >= borders[((i + 1) % 3) * 2 + 1] &&
                point[(i + 1) % 3] <= borders[((i + 1) % 3) * 2] &&
                point[(i + 2) % 3] >= borders[((i + 2) % 3) * 2 + 1] &&
                point[(i + 2) % 3] <= borders[((i + 2) % 3) * 2])
            {
                return bestDist >= new Vector3D(ray.GetPosition(), p).SquareLength();
            }
            return false;
        }
    }
}