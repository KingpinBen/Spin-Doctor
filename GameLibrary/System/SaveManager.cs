using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GameLibrary.GameLogic;
using GameLibrary.Helpers;

namespace GameLibrary.System
{
    public class SaveManager
    {
        #region Singleton
        private static SaveManager _singleton = new SaveManager();
        public static SaveManager Instance
        {
            get { return _singleton; }
        }
        private SaveManager()
        {
            if (!Directory.Exists("./Data"))
            {
                Directory.CreateDirectory("./Data");
            }
        }
        #endregion

        private bool _saveFound;

        /// <summary>
        /// Has the save manager found a save file?
        /// </summary>
        public bool FoundSave
        {
            get
            {
                return _saveFound;
            }
        }

        /// <summary>
        /// Set up a new game.
        /// </summary>
        public void NewGame()
        {
            //  Get a local instance of the game settings.
            GameSettings instance = GameSettings.Instance;

            //  Make sure the backpack is turned off for the
            //  first few levels.
            instance.BackpackEnabled = false;

            //  Start the game from the beginning
            instance.CurrentLevel = 0;

            //  Disable double jump
            instance.DoubleJumpEnabled = false;

            //  Hide any collected notes.
            for (int i = 0; i < instance.FoundEntries.Count; i++)
            {
                instance.FoundEntries[i] = false;
            }
        }
        public void LoadGame()
        {
            string path = Defines.SYSTEM_SAVE_DIRECTORY;
            BinaryReader reader = null;

            if (File.Exists(path + ".sav"))
            {
                try
                {
                    reader = new BinaryReader(File.OpenRead(path + ".sav"));

                    GameData.Read(reader);
                }
                catch
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }

                if (reader != null)
                {
                    reader.Close();
                }

                _saveFound = true;
            }
        }
        public void SaveGame()
        {
            //  Set the save directory to a local variable
            string path = Defines.SYSTEM_SAVE_DIRECTORY;

            //  Initialize the writer and make a temp file.
            BinaryWriter writer = new BinaryWriter(File.Create(path + ".tmp"));

            //  Save the actual game data
            GameData.Write(writer);

            //  Start closing the stream
            writer.Flush();
            writer.Close();

            //  If there is already a save file, remove it
            if (File.Exists(path + ".sav"))
            {
                File.Delete(path + ".sav");
            }
            
            //  and make the temp file the new save file.
            File.Move(path + ".tmp", path + ".sav");
        }

        public void LoadSettings()
        {
            GameSettings instance = GameSettings.Instance;
            BinaryReader reader = null;

            string path = Defines.SYSTEM_SETTINGS_DIRECTORY;

            try
            {
                reader = new BinaryReader(File.OpenRead(path + ".sav"));

                ResolutionData resolution;

                resolution.Width = reader.ReadInt32();
                resolution.Height = reader.ReadInt32();
                resolution.Fullscreen = reader.ReadBoolean();

                instance.Resolution = resolution;

                instance.Shadows = (SettingLevel)reader.ReadInt32();
                instance.ParticleDetail = (SettingLevel)reader.ReadInt32();
                instance.MultiSamplingEnabled = reader.ReadBoolean();

                instance.AmbienceVolume = reader.ReadInt32();
                instance.EffectsVolume = reader.ReadInt32();
                instance.MusicVolume = reader.ReadInt32();
                instance.VoiceVolume = reader.ReadInt32();
            }
            catch
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            if (reader != null)
            {
                reader.Close();
            }
        }
        public void SaveSettings()
        {
            string path = Defines.SYSTEM_SETTINGS_DIRECTORY;
            GameSettings instance = GameSettings.Instance;
            BinaryWriter writer = new BinaryWriter(File.Create(path + ".tmp"));

            ResolutionData resolution = instance.Resolution;

            writer.Write(resolution.Width);
            writer.Write(resolution.Height);
            writer.Write(resolution.Fullscreen);
            writer.Write((int)instance.Shadows);
            writer.Write((int)instance.ParticleDetail);
            writer.Write(instance.MultiSamplingEnabled);

            writer.Write(instance.AmbienceVolume);
            writer.Write(instance.EffectsVolume);
            writer.Write(instance.MusicVolume);
            writer.Write(instance.VoiceVolume);

            writer.Flush();
            writer.Close();

            if (File.Exists(path + ".sav"))
            {
                File.Delete(path + ".sav");
            }

            File.Move(path + ".tmp", path + ".sav");
        }
    }
}
