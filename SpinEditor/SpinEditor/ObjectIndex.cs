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

        public ObjectIndex(OBJECT_TYPE type, int index)
        {
            Type = type;
            Index = index;
        }
    }

    public enum OBJECT_TYPE
    {
        Physics,
        Decal
    }
}
