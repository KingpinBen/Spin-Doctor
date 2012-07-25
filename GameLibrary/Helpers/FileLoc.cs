using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Helpers
{
    public static class FileLoc
    {
        private static string LevelLoc = "Content/Assets/Levels/level";
        private static string blankPixel = "Assets/Images/Basics/BlankPixel";
        private static string devTexture = "Assets/Other/Dev/Trigger";

        public static string Level(uint levelID)
        {
            return LevelLoc + levelID + ".xml";
        }

        public static string BlankPixel()
        {
            return blankPixel;
        }

        public static string DevTexture()
        {
            return devTexture;
        }
    }
}
