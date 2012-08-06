using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GameLibrary.GameLogic;

namespace GameLibrary.System
{
    class GameData
    {
        static GameSettings Settings = GameSettings.Instance;

        public static void Read(BinaryReader reader)
        {
            Settings.CurrentLevel = reader.ReadInt32();
            //Settings.DeathCount = reader.ReadUInt32();
            for (int i = 0; i < GameSettings.Instance.FoundEntries.Count; i++)
            {
                Settings.FoundEntries[i] = reader.ReadBoolean();
            }
            Settings.BackpackEnabled = reader.ReadBoolean();
            Settings.DoubleJumpEnabled = reader.ReadBoolean();
            //Settings.MinutesPlayed = reader.ReadUInt32();
        }

        public static void Write(BinaryWriter writer)
        {
            

            writer.Write(Settings.CurrentLevel);
            //writer.Write(Settings.DeathCount);
            for (int i = 0; i < Settings.FoundEntries.Count; i++)
            {
                writer.Write(Settings.FoundEntries[i]);
            }
            writer.Write(Settings.BackpackEnabled);
            writer.Write(Settings.DoubleJumpEnabled);
            //writer.Write(Settings.MinutesPlayed);
        }
    }
}
