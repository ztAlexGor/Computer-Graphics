using System.Numerics;

namespace Lab1;

public class ObjStructure
{
    private List<float[]> v, vn, vt;
    private List<int?[]> f;

    public ObjStructure()
    {
        v = new List<float[]>();
        vn = new List<float[]>();
        vt = new List<float[]>();
        f = new List<int?[]>();
    }

    public void AddV(float[] v1) => this.v.Add(v1);
    
    public void AddVn(float[] vn1) => this.vn.Add(vn1);
    
    public void AddVt(float[] vt1) => this.vt.Add(vt1);
    
    public void AddF(int?[] f1) => this.f.Add(f1);

    public List<ITraceable> GetObjects()
    {
        List<ITraceable> res = new List<ITraceable>();
        foreach (int?[] p in f)
        {
            Polygon plgn = new Polygon(new Point(v[(int)p[6] - 1]), new Point(v[(int)p[3] - 1]), new Point(v[(int)p[0] - 1]), new Vector3D(vn[(int)p[5] - 1]));
            res.Add(plgn.Scale(10, 10, 10));
        }
        return res;
    }
}