using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpinEditor
{
    public struct ObjectIndex
    {
        public int Index;
        public OBJECT_TYPE Type;
        public float ZLayer;

        public ObjectIndex(OBJECT_TYPE type, int index, float z)
        {
            Type = type;
            Index = index;
            ZLayer = z;
        }
    }

    public enum OBJECT_TYPE
    {
        Physics,
        Decal
    }
}
