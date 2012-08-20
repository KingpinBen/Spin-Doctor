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
        public const string NOTE_DIRECTORY = "Assets/Images/JournalNotes/";

        //  Player settings.
        public const float PLAYER_RUN_SPEED = 16.0f;
        public const float PLAYER_JUMP_FORCE = 4.2f;
        public const float PLAYER_MIDAIR_FORCE = 200.0f;
        public const float PLAYER_MAX_AIR_TIME = 0.6f;

        //  The cooldown between level rotations
        public const float LEVEL_ROTATION_COOLDOWN = 1.3f;

        public const string SYSTEM_SAVE_DIRECTORY = "./Data/SavedGame";
        public const string SYSTEM_SETTINGS_DIRECTORY = "./Data/Settings";

        //  The time separating the credit sections showing.
        public const float SYSTEM_CREDITS_SECT_SEPARATION = 5.0f;
        public const float SYSTEM_CREDITS_FADE_MULTIPLIER = 0.5f;
        //  The time each section of the credits is onscreen for, in seconds.
        public const float SYSTEM_CREDITS_TIME_ON_SCREEN = 5.0f;

    }
}
