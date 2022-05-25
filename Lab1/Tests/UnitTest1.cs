using System.Collections;

namespace Tests
{
    public class UnitTest1
    {
        [Theory]
        [ClassData(typeof(BoolSphereAndBeamTestData))]
        public void IsThereSphereIntersect(Sphere s, Beam ray, bool res)
        {
            bool result = (s.GetIntersectionPoint(ray) is not null);
            Assert.Equal(res, result);
        }
        
        [Theory]
        [ClassData(typeof(PointSphereAndBeamTestData))]
        public void CorrectSphereIntersect(Sphere s, Beam ray, Point? res)
        {
            Point result = s.GetIntersectionPoint(ray);
            if (res is null)
            {
                bool f = result is null;
                Assert.Equal(true, f);
            }
            else
            {
                result.Simplify();
                Assert.Equal(res, result);
            }
        }
        
        [Theory]
        [ClassData(typeof(PointPlaneAndBeamTestData))]
        public void CorrectPlaneIntersect(Plane p, Beam ray, Point? res)
        {
            Point result = p.GetIntersectionPoint(ray);
            if (res is null)
            {
                bool f = result is null;
                Assert.Equal(true, f);
            }
            else
            {
                result.Simplify();
                Assert.Equal(res, result);
            }
        }
        
        [Theory]
        [ClassData(typeof(PointMultipleTestData))]
        public void CorrectMultipleIntersection(Sphere sp, Plane pl, Polygon pol, Beam ray, Point res)
        {
            ITraceable[] objArr = {sp, pl, pol};
            Scene s = new Scene(objArr);

            ITraceable resObj;
            Point? result = s.RayIntersect(ray, out resObj);

            if (res is null)
            {
                bool f = result is null;
                Assert.Equal(true, f);
            }
            else
            {
                result.Simplify();
                Assert.Equal(res, result);
            }
        }

        [Theory]
        [ClassData(typeof(PointPolygonAndBeamTestData))]
        public void CorrectPolygonIntersection(Polygon p, Beam ray, Point? res)
        {
            Point result = p.GetIntersectionPoint(ray);
            if (res is null)
            {
                bool f = result is null;
                Assert.Equal(true, f);
            }
            else
            {
                result.Simplify();
                Assert.Equal(res, result);
            }
        }
    }
    
    
    public class BoolSphereAndBeamTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            Sphere s = new Sphere(new Point(5, 5, 5), 3);
            yield return new object[] { s, new Beam(new Point(0, 0, 0), new Vector3D(1, 1, 1)), true};
            yield return new object[] { s, new Beam(new Point(0, 0, 0), new Vector3D(0, 0, 1)), false};
            yield return new object[] { s, new Beam(new Point(10, 10, 10), new Vector3D(1, 1, 1)), false};
            yield return new object[] { s, new Beam(new Point(5, 5, 5), new Vector3D(1, 1, 1)), true};
            yield return new object[] { s, new Beam(new Point(0, 0, 0), new Vector3D(8, 5, 5)), true};
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class PointSphereAndBeamTestData : IEnumerable<object[]> 
    {
        public IEnumerator<object[]> GetEnumerator()
        {
             Sphere s = new Sphere(new Point(0, 0, 10), 5);
             Point p = new Point(0, 0, 0);
             yield return new object[] { s, new Beam(p, new Vector3D(0, 0, 1)), new Point(0, 0, 5)};
             yield return new object[] { s, new Beam(p, new Vector3D(0, 1, 0)), null};
             yield return new object[] { s, new Beam(p, new Vector3D(0, 0, -1)), null};
             yield return new object[] { s, new Beam(new Point(0, 0, 10), new Vector3D(0, 0, -1)), new Point(0, 0, 5)};
             yield return new object[] { s, new Beam(new Point(5, 0, 0), new Vector3D(0, 0, 1)), new Point(5, 0, 10)};
         }
        
         IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public class PointPlaneAndBeamTestData : IEnumerable<object[]> 
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            Plane p1 = new Plane(new Point(0, 0, 0), new Point(0, 0, 1), new Point(1, 0, 0));
            Plane p2 = new Plane(new Point(0, -1, 0), new Point(-1, 0, 1), new Point(1, 0, 1));
            Plane p3 = new Plane(new Point(0, 0, 0), new Point(2, 0, 0), new Point(0, -1, 1));
            yield return new object[] { p1, new Beam(new Point(0, 0, 0), new Point(0, 0, 1)), null};
            yield return new object[] { p1, new Beam(new Point(0, -1, 0), new Point(0, -1, 1)), null};
            yield return new object[] { p1, new Beam(new Point(0, -1, 0), new Point(0, 0, 1)), new Point(0, 0, 1)};
            yield return new object[] { p2, new Beam(new Point(-1, 0, 1), new Point(1, 0, 1)), null};
            yield return new object[] { p2, new Beam(new Point(-1, 1, 1), new Point(1, 1, 1)), null};
            yield return new object[] { p2, new Beam(new Point(0, 0, 0), new Point(0, -1, 0)), new Point(0, -1, 0)};
            yield return new object[] { p3, new Beam(new Point(2, 0, 0), new Point(0, 0, 0)), null};
            yield return new object[] { p3, new Beam(new Point(3, 0, 0), new Point(1, -1, 1)), null};
            yield return new object[] { p3, new Beam(new Point(0, 0, -1), new Point(0, 0, 0)), new Point(0, 0, 0)};
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class PointMultipleTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            Beam view = new Beam(new Point(0, 0, 0), new Vector3D(0, 0, 1));
            Sphere s = new Sphere(new Point(0, 0, 10), 3);
            yield return new object[] {s, new Plane(new Point(0, 0, 10), new Point(0, -1, 10), new Point(1, 0, 10)), null, view, new Point(0, 0, 7)};
            yield return new object[] {s, new Plane(new Point(0, 0, 6), new Point(0, -1, 6), new Point(1, 0, 6)), null, view, new Point(0, 0, 6)};
            yield return new object[] {s, new Plane(new Point(0, 0, 6), new Point(0, -1, 6), new Point(1, 0, 6)), new Polygon(new Point(-1, -1, 5), new Point(-1, 1, 5), new Point(1, 0, 5)), view, new Point(0, 0, 5)};
            yield return new object[] {s, new Plane(new Point(0, 0, 6), new Point(0, -1, 6), new Point(1, 0, 6)), new Polygon(new Point(0, -1, 5), new Point(1, -1, 5), new Point(1, 0, 5)), view, new Point(0, 0, 6)};
            yield return new object[] {null, new Plane(new Point(-1, 0, 0), new Point(-1, -1, 0), new Point(-1, 0, 6)), new Polygon(new Point(-1, -1, 6), new Point(0, -2, 6), new Point(1, -1, 6)), view, null};
            yield return new object[] {s, null, new Polygon(new Point(0, 1, 6), new Point(0, -2, 7), new Point(0, 0, 8)), view, new Point(0, 0, 7)};
            yield return new object[] {null, new Plane(new Point(0, -1, 5), new Point(0, 1, 7), new Point(1, 0, 6)), null, view, new Point(0, 0, 6)};
            yield return new object[] {new Sphere(new Point(3.00001f, 0, 10), 3), new Plane(new Point(0, 0, 10), new Point(0, -1, 10), new Point(1, 0, 10)), null, view, new Point(0, 0, 10)};
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class PointPolygonAndBeamTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            Point p1 = new Point(0, 0, 0);
            Point p2 = new Point(0, 0, 5);
            yield return new object[] {new Polygon(new Point(0, 0, 5), new Point(-1, -1, 5), new Point(1, -1, 5)), new Beam(p1, new Vector3D(0, 0, 1)), new Point(0, 0, 5) };
            yield return new object[] {new Polygon(new Point(0, -3, 0), new Point(-3, 0, 2), new Point(3, 0, 2)), new Beam(p1, new Vector3D(0, 0, 1)), new Point(0, 0, 2) };
            yield return new object[] {new Polygon(new Point(0, -3, 0), new Point(-3, 0, 2), new Point(3, -1, 2)), new Beam(p1, new Vector3D(0, 0, 1)), null };
            yield return new object[] {new Polygon(new Point(0, -3, 0), new Point(-3, 0, 2), new Point(3, 0, 2)), new Beam(p2, new Vector3D(0, 0, 1)), null };
            yield return new object[] {new Polygon(new Point(-1, 0, 1), new Point(1, 0, 1), new Point(0, 0, 3)), new Beam(p1, new Vector3D(0, 0, 1)), null };
            yield return new object[] {new Polygon(new Point(0, -2, 0), new Point(0, 0, 2), new Point(3, 1, 4)), new Beam(p1, new Vector3D(0, -1, 1)), new Point(0, -1, 1) };
            yield return new object[] {new Polygon(new Point(0, -2, 0), new Point(0, 0, 2), new Point(3, 1, 4)), new Beam(p2, new Vector3D(0, -1, -1)), null };
            yield return new object[] {new Polygon(new Point(0, -2, 0), new Point(0, 0, 2), new Point(3, 1, 4)), new Beam(p2, new Vector3D(0, -2, -5)), new Point(0, -2, 0) };
            yield return new object[] {new Polygon(new Point(0, -10, 5), new Point(-10, 10, 5), new Point(10, 10, 5)), new Beam(p1, new Vector3D(1, 1, 1)), new Point(5, 5, 5) };
            yield return new object[] {new Polygon(new Point(0, -10, 5), new Point(-10, 10, 5), new Point(10, 10, 5)), new Beam(p2, new Vector3D(1, 1, 1)), null };
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}