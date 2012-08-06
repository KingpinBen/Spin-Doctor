using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.System
{
    public struct ResolutionData
    {
        public int Width;
        public int Height;
        public bool Fullscreen;

        public ResolutionData(int width, int height, bool fullscreen)
        {
            Width = width;
            Height = height;
            Fullscreen = fullscreen;
        }

        public bool CompareTo(ResolutionData compare)
        {
            if (Width != compare.Width)
            {
                return false;
            }

            if (Height != compare.Width)
            {
                return false;
            }

            if (Fullscreen != compare.Fullscreen)
            {
                return false;
            }

            return true;
        }
    }
}
