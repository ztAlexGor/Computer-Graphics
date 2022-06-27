namespace Lab1
{
    public class BVHTree
    {
        internal class Node
        {
            public Node? left, right;
            public List<ITraceable> items;
            public AABB aabb;

            public Node(List<ITraceable> items)
            {
                left = null;
                right = null;
                this.items = items;
                aabb = new AABB(items);
            }
        }

        private Node? root;
        private int capacity;

        Point? bestPoint;
        ITraceable? bestObj;
        float bestSqDist;

        public BVHTree (int capacity)
        {
            this.capacity = capacity;
            root = null;
            bestSqDist = float.MaxValue;
        }

        public void Build(List<ITraceable> items)
        {
            root = new Node(items);
            Build(root);
        }

        private void Build(Node curr)
        {
            if (curr.items.Count > capacity)
            {
                DivideItems(curr);
                Build(curr.left);
                Build(curr.right);
            }
        }

        private void DivideItems(Node curr)
        {
            int dim = curr.aabb.GetLongestDimension();

            curr.items.Sort((item1, item2) => item1.GetAABB().GetCenter()[dim].CompareTo(item2.GetAABB().GetCenter()[dim]));

            int i = curr.items.Count / 2;
            curr.left = new Node(curr.items.GetRange(0, i));
            curr.right = new Node(curr.items.GetRange(i, curr.items.Count - i));
            curr.items.Clear();
        }

        public Point? FindIntersection(Beam ray)
        {
            bestPoint = null;
            bestObj = null;
            bestSqDist = float.MaxValue;

            if (root != null) FindIntersection(root, ray);
            return bestPoint;
        }

        private void FindIntersection(Node curr, Beam ray)
        {
            if (curr.aabb.IsIntersect(ray))
            {
                if (curr.items.Count == 0)
                {
                    FindIntersection(curr.left, ray);
                    FindIntersection(curr.right, ray);
                }
                else {
                    FindIntersection(curr.items, ray);
                }
            }
        }

        private void FindIntersection(List<ITraceable> items, Beam ray)
        {
            foreach (ITraceable obj in items)
            {
                if (obj is not null)
                {
                    Point? intersectionPoint = obj.GetIntersectionPoint(ray);
                    if (intersectionPoint is not null)
                    {
                        float sqLenght = new Vector3D(ray.GetPosition(), intersectionPoint).SquareLength();
                        if (sqLenght < bestSqDist)
                        {
                            bestSqDist = sqLenght;
                            bestPoint = intersectionPoint;
                            bestObj = obj;
                        }
                    }
                }
            }
        }

        public Point? GetIntersectionPoint() => bestPoint;

        public ITraceable? GetIntersectionObj() => bestObj;




        public int Traversal() => Traversal(root);//test method to check number of all items
        private int Traversal(Node? curr)
        {
            if (curr == null) return 0;
            return curr.items.Count + Traversal(curr.left) + Traversal(curr.right);
        }

    }
}




