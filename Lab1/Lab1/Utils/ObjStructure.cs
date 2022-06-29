using System.Drawing;

namespace Lab1;

public class ObjStructure
{
    private List<float?[]> v, vn, vt;
    private List<int?[]> f;

    public ObjStructure()
    {
        v = new List<float?[]>();
        vn = new List<float?[]>();
        vt = new List<float?[]>();
        f = new List<int?[]>();
    }

    public void AddV(float?[] v1) => v.Add(v1);
    
    public void AddVn(float?[] vn1) => vn.Add(vn1);

    public void AddVt(float?[] vt1) => vt.Add(vt1);
    
    public void AddF(int?[] f1) => f.Add(f1);

    public List<SceneObject> GetObjects(Color color, Material material)
    {
        List<SceneObject> res = new List<SceneObject>();
        foreach (int?[] p in f)
        {
            Polygon plgn = new Polygon(new Point(v[(int)p[6] - 1]), new Point(v[(int)p[3] - 1]), new Point(v[(int)p[0] - 1]), color, m: material);

            if (p[7] is not null && p[4] is not null && p[1] is not null)
            {
                plgn.AddVt(new Point(vt[(int)p[7] - 1]), new Point(vt[(int)p[4] - 1]), new Point(vt[(int)p[1] - 1]));
            }

            res.Add(plgn);
        }

        Clear();
        return res;
    }

    private void Clear()
    {
        v.Clear();
        vn.Clear();
        vt.Clear();
        f.Clear();
    }
}