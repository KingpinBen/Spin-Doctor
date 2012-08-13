using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameLibrary.System;
using GameLibrary.Audio;

namespace GameLibrary.GameLogic
{
    public class GameSettings
    {
        #region Singleton
        private static GameSettings _singleton = new GameSettings();
        public static GameSettings Instance
        {
            get
            {
                return _singleton;
            }
        }

        private GameSettings()
        {
            this._resolution.Width = 800;
            this._resolution.Height = 600;
            this._resolution.Fullscreen = false;
        }
        #endregion

        #region Fields

        #region Serializable


        #region Player


        private List<bool> _foundEntries = new List<bool>() { false, false, false, false, false, false };
        private bool _allowDoubleJump = false;
        private int _currentLevel = 0;
        private bool _backbackFound = false;
        private uint _deathCount = 0;

        #endregion



        #region Game

        private ResolutionData _resolution;
        private SettingLevel _shadows = SettingLevel.Off;
        private SettingLevel _particleDetails = SettingLevel.Low;
        private bool _enableMultiSampling = false;

        private int _soundVolume = 10;
        private int _musicVolume = 6;


        #endregion

        

        #endregion


        private bool _fartCheat = false;
        
        #endregion

        #region Properties

        public bool MultiSamplingEnabled
        {
            get
            {
                return _enableMultiSampling;
            }
            set
            {
                _enableMultiSampling = value;
            }
        }
        public SettingLevel Shadows
        {
            get
            {
                return _shadows;
            }
            set
            {
                if (value == SettingLevel.Off || value == SettingLevel.On)
                {
                    _shadows = value;
                }
            }
        }
        public SettingLevel ParticleDetail
        {
            get
            {
                return _particleDetails;
            }
            set
            {
                if (value == SettingLevel.Low ||
                    value == SettingLevel.High)
                {
                    _particleDetails = value;
                }
            }
        }
        public ResolutionData Resolution
        {
            get
            {
                return _resolution;
            }
            set
            {
                _resolution = value;
            }
        }

        public int MusicVolume
        {
            get
            {
                return _musicVolume;
            }
            set
            {
                this._musicVolume = (int)MathHelper.Clamp(value, 0, 10);
                AudioManager.Instance.MusicVolume = value;
            }
        }
        public int SoundVolume
        {
            get
            {
                return _soundVolume;
            }
            set
            {
                this._soundVolume = (int)MathHelper.Clamp(value, 0, 10);
                AudioManager.Instance.SoundVolume = this._soundVolume;
            }
        }

        public List<bool> FoundEntries
        {
            get
            {
                return _foundEntries;
            }
            set
            {
                _foundEntries = value;
            }
        }
        public bool DoubleJumpEnabled
        {
            get
            {
                return _allowDoubleJump;
            }
            set
            {
                _allowDoubleJump = value;
            }
        }
        public void ToggleDoubleJump()
        {
            _allowDoubleJump = !_allowDoubleJump;
        }
        public int CurrentLevel
        {
            get
            {
                return _currentLevel;
            }
            set
            {
                _currentLevel = value;
            }
        }
        public bool BackpackEnabled
        {
            get
            {
                return _backbackFound;
            }
            set
            {
                _backbackFound = value;
            }
        }

        
        public bool FartCheat
        {
            get
            {
                return _fartCheat;
            }
            set
            {
                _fartCheat = value;
            }
        }

        #endregion

        #region Development

        /// <summary>
        /// TODO : Delete this and all references before the beta hand-in
        /// </summary>
        private bool _developmentMode = false;

        public bool DevelopmentMode
        {
            get
            {
                return _developmentMode;
            }
        }

        public  void SetDevelopment(object sender, bool state)
        {
            _developmentMode = state;
        }

        #endregion
    }
}
