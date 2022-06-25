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

        public void AddItem(ITraceable item)
        {
            alpha.AddItem(item);
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

            public void AddItem(ITraceable item)
            {
                if (items.Count == tree.boxCapacity)
                {
                    BoxDecay();
                }
                else
                {
                    items.Add(item);
                    UpdateBorders();
                }
            }

            private void UpdateBorders()
            {
                float[] itemBorders = items[items.Count - 1].GetBoxBorders();
                for (int i = 0; i < 3; i++)
                {
                    borders[2 * i] = (borders[2 * i] < itemBorders[2 * i]) ? itemBorders[2 * i] : borders[2 * i];
                    borders[2 * i + 1] = (borders[2 * i + 1] > itemBorders[2 * i + 1]) ? itemBorders[2 * i + 1] : borders[2 * i + 1];
                }
            }

            private void BoxDecay()
            {
                FindPivot();
                lChild = new Box(tree, this);
                rChild = new Box(tree, this);
                foreach (ITraceable item in items)
                {
                    if (item.GetBoxBorders()[(int)pivot[0] * 2 + 1] < pivot[1])
                    {
                        lChild.AddItem(item);
                    }

                    if (item.GetBoxBorders()[(int)pivot[0] * 2] > pivot[1])
                    {
                        rChild.AddItem(item);
                    }
                }
                items.Clear();
            }

            private void FindPivot()
            {
                float xDiff = borders[0] - borders[1];
                float yDiff = borders[2] - borders[3];
                float zDiff = borders[4] - borders[5];
                if (xDiff > yDiff && xDiff > zDiff)
                {
                    pivot[0] = 0;
                    pivot[1] = borders[1] + xDiff / 2;
                }
                else if (yDiff > zDiff)
                {
                    pivot[0] = 1;
                    pivot[1] = borders[3] + yDiff / 2;
                }
                else
                {
                    pivot[0] = 2;
                    pivot[1] = borders[5] + zDiff / 2;
                }
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
