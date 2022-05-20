using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    interface ISimpleObject
    {
        public bool IntersectsWith(Point viewPoint, Vector3D viewRay);
    }
}
