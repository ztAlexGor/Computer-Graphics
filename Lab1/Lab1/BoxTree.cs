namespace Lab1
{
    public class BoxTree
    {
        private Box root;
        private int boxCapacity;
        private Point? bestPoint;
        private ITraceable? bestObj;

        public BoxTree(int boxCapacity)
        {
            root = new(this);
            this.boxCapacity = boxCapacity;
        }

        public void Build(List<ITraceable> items)
        {
            root.Build(items);
        }

        public Point? FindIntersection(Beam ray)
        {
            bestObj = null;
            bestPoint = null;
            root.FindIntersection(ray);
            return bestPoint;
        }

        public ITraceable? GetObject() => bestObj;
/*        public int Traversal() => Traversal(root);
        private int Traversal(Box curr)
        {
            if (curr == null)return 0;
            return curr.items.Count() + Traversal(curr.lChild) + Traversal(curr.rChild);
        }*/
        internal class Box
        {
            public List<ITraceable> items;
            public Box? lChild, rChild;
            private BoxTree tree;
            private float[] borders;
            private ITraceable? bestObj;

            public Box(BoxTree tree)
            {
                items = new List<ITraceable>();
                lChild = null;
                rChild = null;
                this.tree = tree;
                borders = new float[] { float.MinValue, float.MaxValue, 
                                        float.MinValue, float.MaxValue,
                                        float.MinValue, float.MaxValue };
            }

            public void Build(List<ITraceable> items)
            {
                this.items = items;
                UpdateBorders();
                if (this.items.Count > tree.boxCapacity)
                {
                    int dim = SelectDimension();
                    this.items.Sort((item1, item2) => item1.GetBoxCenter()[dim].CompareTo(item2.GetBoxCenter()[dim]));
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

            private int SelectDimension()
            {
                float max = borders[0] - borders[1];
                int maxInd = 0;
                for (int i = 1; i < 3; i++)
                {
                    if (borders[i] - borders[i + 1] > max)
                    {
                        max = borders[i] - borders[i + 1];
                        maxInd = i;
                    }
                }
                return maxInd;
            }
            
            private void BoxDecay()
            {
                int i = items.Count / 2;//FindPivot();
                lChild = new Box(tree);
                rChild = new Box(tree);
                lChild.Build(items.GetRange(0, i));
                rChild.Build(items.GetRange(i, items.Count - i));
                items.Clear();
            }

            public void FindIntersection(Beam ray)
            {
                bestObj = null;
                if (IsBoxIntersected(ray))
                {
                    if (lChild == null && rChild == null)
                    {
                        Point result = LookForIntersection(ray);
                        if (tree.bestPoint is null)
                        {
                            tree.bestPoint = result;
                            tree.bestObj = bestObj;
                        }
                        else
                        {
                            if (result is not null)
                            {
                                CheckBest(result, ray);
                            }
                        }
                    }
                    else
                    {
                        lChild.FindIntersection(ray);
                        rChild.FindIntersection(ray);
                    }
                }
            }

            private bool IsBoxIntersected(Beam ray)
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
                    if (tree.bestPoint is null)
                    {
                        return true;
                    }
                    float oldBest = new Vector3D(ray.GetPosition(), tree.bestPoint ).SquareLength();
                    float possibleBest = new Vector3D(ray.GetPosition(), p).SquareLength();
                    return possibleBest < oldBest;
                }

                return false;
            }

            private Point LookForIntersection(Beam ray)
            {
                float depth = float.MaxValue;
                Point result = null;

                foreach (ITraceable obj in items)
                {
                    if (obj is not null)
                    {
                        Point? intersectionPoint = obj.GetIntersectionPoint(ray);
                        if (intersectionPoint is not null) 
                        {
                            float sqLenght = new Vector3D(ray.GetPosition(), intersectionPoint).SquareLength();
                            if (sqLenght < depth)
                            {
                                depth = sqLenght;
                                result = intersectionPoint;
                                bestObj = obj;
                            }
                        }
                    }
                }
                return result;
            }

            private void CheckBest(Point p, Beam ray)
            {
                float oldBest = new Vector3D(ray.GetPosition(), tree.bestPoint ).SquareLength();
                float possibleBest = new Vector3D(ray.GetPosition(), p).SquareLength();
                if (oldBest > possibleBest)
                {
                    tree.bestPoint = p;
                    tree.bestObj = bestObj;
                }
            }
        }
    }
}
