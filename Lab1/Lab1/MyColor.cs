using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class MyColor
    {
        private readonly byte r;
        private readonly byte g;
        private readonly byte b;

        public MyColor(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        public MyColor(MyColor c)
        {
            r = c.r;
            g = c.g;
            b = c.b;
        }
        public byte R() => r;
        public byte G() => g;
        public byte B() => b;
        public static bool operator !=(MyColor a, MyColor b) => (a.r != b.r) || (a.g != b.g) || (a.b != b.b);
        public static bool operator ==(MyColor a, MyColor b) => !(a != b);
        public override bool Equals(object? obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                MyColor c = (MyColor)obj;
                return (r == c.r) && (g == c.g) && (b == c.b);
            }
        }
        public override int GetHashCode() => r.GetHashCode() ^ g.GetHashCode() ^ b.GetHashCode();
    }
}
