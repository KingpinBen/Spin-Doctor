using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Helpers
{
    public static class Defines
    {
        private static string _levelDirectory = "Content/Assets/Levels/level";
        public static string Level(int id)
        {
            return _levelDirectory + id + ".xml";
        }

        public static string DEVELOPMENT_TEXTURE = "Assets/Other/Dev/Trigger";
        public const string BLANK_PIXEL = "Assets/Images/Basics/BlankPixel";
        public const string NOTE_DIRECTORY = "Assets/Images/JournalNotes/Note_";

        public const float PLAYER_RUN_SPEED = 16.0f;
        public const float PLAYER_JUMP_FORCE = 4.2f;
        public const float PLAYER_MIDAIR_FORCE = 200.0f;

        public const float LEVEL_ROTATION_COOLDOWN = 1.3f;

    }
}
