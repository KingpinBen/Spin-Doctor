using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GameLibrary.GameLogic;

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

        private bool _loaded;

        private const string savePath = "./Data/SavedGame";
        private const string settingsPath = "./Data/Settings";

        /// <summary>
        /// Has the save manager found a save file?
        /// </summary>
        public bool Loaded
        {
            get
            {
                return _loaded;
            }
        }

        public void NewGame()
        {
            GameSettings instance = GameSettings.Instance;

            instance.BackpackEnabled = false;
            instance.CurrentLevel = 0;
            instance.DoubleJumpEnabled = false;
            for (int i = 0; i < instance.FoundEntries.Count; i++)
            {
                instance.FoundEntries[i] = false;
            }
        }
        public void LoadGame()
        {
            BinaryReader reader = null;

            if (File.Exists(savePath + ".sav"))
            {
                try
                {
                    reader = new BinaryReader(File.OpenRead(savePath + ".sav"));

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

                _loaded = true;
            }
        }
        public void SaveGame()
        {
            //  Initialize the writer and make a temp file.
            BinaryWriter writer = new BinaryWriter(File.Create(savePath + ".tmp"));
            //  Save the actual game data
            GameData.Write(writer);
            //  Start closing the stream
            writer.Flush();
            writer.Close();

            //  If there is already a save file, remove it
            if (File.Exists(savePath + ".sav"))
            {
                File.Delete(savePath + ".sav");
            }
            
            //  and make the temp file the new save file.
            File.Move(savePath + ".tmp", savePath + ".sav");
        }

        public void LoadSettings()
        {
            GameSettings instance = GameSettings.Instance;
            BinaryReader reader = null;

            try
            {
                reader = new BinaryReader(File.OpenRead(settingsPath + ".sav"));

                ResolutionData resolution;

                resolution.Width = reader.ReadInt32();
                resolution.Height = reader.ReadInt32();
                resolution.Fullscreen = reader.ReadBoolean();

                instance.Resolution = resolution;

                instance.Shadows = (SettingLevel)reader.ReadInt32();
                instance.ParticleDetail = (SettingLevel)reader.ReadInt32();
                instance.MultiSamplingEnabled = reader.ReadBoolean();
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
            GameSettings instance = GameSettings.Instance;
            BinaryWriter writer = new BinaryWriter(File.Create(settingsPath + ".tmp"));

            ResolutionData resolution = instance.Resolution;

            writer.Write(resolution.Width);
            writer.Write(resolution.Height);
            writer.Write(resolution.Fullscreen);
            writer.Write((int)instance.Shadows);
            writer.Write((int)instance.ParticleDetail);
            writer.Write(instance.MultiSamplingEnabled);

            writer.Flush();
            writer.Close();

            if (File.Exists(settingsPath + ".sav"))
            {
                File.Delete(settingsPath + ".sav");
            }

            File.Move(settingsPath + ".tmp", settingsPath + ".sav");
        }
    }
}
