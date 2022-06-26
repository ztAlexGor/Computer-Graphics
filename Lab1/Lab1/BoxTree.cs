namespace Lab1
{
    public class BoxTree
    {
        private Box alpha;
        private int boxCapacity;

        public BoxTree(int boxCapacity)
        {
            alpha = new(this);
            this.boxCapacity = boxCapacity;
        }

        public void Build(List<ITraceable> items)
        {
            alpha.Build(items);
        }

        public List<ITraceable> FindBox(Beam ray)
        {
            return alpha.FindBox(ray);
        }

        internal class Box
        {
            private List<ITraceable> items;
            private Box parent, lChild, rChild;
            private BoxTree tree;
            private float[] borders;
            private float[] pivot;

            public Box(BoxTree tree, Box parent = null)
            {
                items = new List<ITraceable>();
                this.parent = parent;
                lChild = null;
                rChild = null;
                this.tree = tree;
                borders = new float[]
                    { float.MinValue, float.MaxValue, float.MinValue, float.MaxValue, float.MinValue, float.MaxValue };
                pivot = new float[] { -1f, 0f };
            }

            public void Build(List<ITraceable> items)
            {
                this.items = items;
                UpdateBorders();
                if (this.items.Count > tree.boxCapacity)
                {
                    SelectDimension();
                    this.items.Sort((item1, item2) => item1.GetBoxCenter()[(int)pivot[0]].CompareTo(item2.GetBoxCenter()[(int)pivot[0]]));
                    BoxDecay();
                }
            }

            private void UpdateBorders()
            {
                foreach (ITraceable item in items)
                {
                    float[] itemBorders = item.GetBoxBorders();
                    for (int i = 0; i < 3; i++)
                    {
                        borders[i * 2] = (borders[i * 2] < itemBorders[i * 2]) ? itemBorders[i * 2] : borders[i * 2];
                        borders[i * 2 + 1] = (borders[i * 2 + 1] > itemBorders[i * 2 + 1]) ? itemBorders[i * 2 + 1] : borders[i * 2 + 1];
                    }
                }
            }

            private void SelectDimension()
            {
                float[] diff = new float[]
                {
                    borders[0] - borders[1],
                    borders[2] - borders[3],
                    borders[4] - borders[5]
                };
                if (diff[0] > diff[1] && diff[0] > diff[2])
                {
                    pivot[0] = 0;
                }

                if (diff[1] > diff[2])
                {
                    pivot[0] = 1;
                }

                pivot[0] = 2;
            }
            
            private void BoxDecay()
            {
                int i = FindPivot();
                lChild = new Box(tree, this);
                rChild = new Box(tree, this);
                lChild.Build(items.GetRange(0, i));
                rChild.Build(items.GetRange(i, items.Count - i));
                items.Clear();
            }

            private int FindPivot()
            {
                int i = items.Count / 2;
                float leftLast = items[i - 1].GetBoxCenter()[(int)pivot[0]];
                float rightFirst = items[i].GetBoxCenter()[(int)pivot[0]];
                pivot[1] = leftLast + (rightFirst - leftLast);
                return i;
            }

            public List<ITraceable> FindBox(Beam ray)
            {
                if (IsBoxIntersected(ray))
                {
                    if (lChild == null && rChild == null)
                    {
                        return items;
                    }

                    List<ITraceable> merged = lChild.FindBox(ray);
                    merged.AddRange(rChild.FindBox(ray));
                    return merged;
                }

                return new List<ITraceable>();
            }

            private bool IsBoxIntersected(Beam ray)
            {
                float e = 0.0001f;
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
                            if (IsItIn(final, i))
                            {
                                return true;
                            }
                        }

                        if (t2 > 0)
                        {
                            Point final = sPoint + (dirVector * t1);
                            if (IsItIn(final, i))
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }

            private bool IsItIn(Point p, int i)
            {
                float[] point = new float[3] { p.X(), p.Y(), p.Z() };
                float e = 0.0001f;

                return point[(i + 1) % 3] >= borders[((i + 1) % 3) * 2 + 1] &&
                       point[(i + 1) % 3] <= borders[((i + 1) % 3) * 2] &&
                       point[(i + 2) % 3] >= borders[((i + 2) % 3) * 2 + 1] &&
                       point[(i + 2) % 3] <= borders[((i + 2) % 3) * 2];
            }
        }
    }
}
