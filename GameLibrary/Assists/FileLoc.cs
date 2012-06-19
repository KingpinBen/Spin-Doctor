using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Assists
{
    public static class FileLoc
    {
        private static string LevelLoc = "Content/Assets/Other/Xml/level";
        private static string blankPixel = "Assets/Sprites/Basics/BlankPixel";
        private static string devTexture = "Assets/Other/Dev/Trigger";

        public static string Level(int levelID)
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
