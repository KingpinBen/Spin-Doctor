//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Audio
//--    
//--    Current Version: 1.000
//--    
//--    Description
//--    ===============
//--    Audio handler
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    
//--    
//--    
//--    
//--    TBD
//--    ==============
//--    Should this be static? I don't know how many things should be static...
//--    but this'd be one of them I'd assume.
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Windows.Forms;
using System.Diagnostics;
using GameLibrary.Helpers;
#endregion

namespace GameLibrary.Audio
{
    public class AudioManager
    {
        private static AudioManager _singleton = null;
        public static AudioManager Instance
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new AudioManager();
                }

                return _singleton;
            }
        }

        #region Fields

        private AudioEngine _engine;
        private SoundBank[] _soundBanks;
        private WaveBank[] _waveBanks;
        private uint _soundVolume;
        private uint _musicVolume;
        private bool _isMusicEnabled;
        private bool _isSoundEnabled;

        /// <summary>
        /// Temporary banks for testing.
        /// Switch to array when we have something here.
        /// </summary>
        private SoundBank _soundBank;
        private WaveBank _waveBank;

        #endregion

        #region Properties

        #region Sound Getters and Setters


        /// <summary>
        /// Sound level get/set
        /// </summary>
        public uint SoundVolume
        {
            get
            {
                return _soundVolume;
            }
            set
            {
                SetSoundVolume(value);
            }
        }

        /// <summary>
        /// Is sound currently enabled?
        /// </summary>
        /// <returns>Is the sound enabled?</returns>
        public bool GetEnabledSound()
        {
            return _isSoundEnabled;
        }

        /// <summary>
        /// Set the current volume level between 0 and 10
        /// </summary>
        /// <param name="Volume Level">What should the volume level be?</param>
        public void SetSoundVolume(uint volumeLevel)
        {
            _soundVolume = (uint)MathHelper.Clamp(_soundVolume, 0, 10);

            if (this._engine != null)
            {
                this._engine.GetCategory("Sound").SetVolume(this._soundVolume * 0.1f);

                if (_soundVolume == 0)
                {
                    this._isSoundEnabled = false;
                }
                else
                {
                    this._isSoundEnabled = true;
                }
            }
        }



        #endregion

        #region Music Related Getter and Setters



        /// <summary>
        /// Music Volume GetSet
        /// </summary>
        public uint MusicVolume
        {
            get
            {
                return _musicVolume;
            }
            set
            {
                SetMusicVolume(value);
            }
        }

        /// <summary>
        /// Is music enabled?
        /// </summary>
        /// <returns>Is music enabled</returns>
        public bool GetEnabledMusic()
        {
            return _isMusicEnabled;
        }

        /// <summary>
        /// Set the music volume
        /// </summary>
        /// <param name="volumeLevel">Desired volume level. 0-10</param>
        public void SetMusicVolume(uint volumeLevel)
        {
            //  Cap the volume level between 0 and 10
            _musicVolume = (uint)MathHelper.Clamp(volumeLevel, 0, 10);

            //  If we can use the 
            if (this._engine != null)
            {
                this._engine.GetCategory("Music").SetVolume(this._musicVolume * 0.1f);

                if (_musicVolume == 0)
                {
                    this._isMusicEnabled = false;
                }
                else
                {
                    this._isMusicEnabled = true;
                }
            }
        }


        #endregion

        #endregion

        private AudioManager() { }

        public void Load()
        {
            while (_engine == null)
            {
                try
                {
                    _engine = new AudioEngine("Content/Assets/Audio/SDAudio.xgs");
                }
                catch (InvalidOperationException e)
                {
                    DialogResult result = MessageBox.Show("Couldn't find a sound device.", "Error loading audio engine", MessageBoxButtons.AbortRetryIgnore);
                    if (result == DialogResult.Abort)
                    {
                        ErrorReport.GenerateReport("Couldn't find an active sound device.\n" + e.ToString(), null);
                    }
                    else if (result == DialogResult.Retry)
                    {
                        Process.GetCurrentProcess().Kill();
                        return;
                    }
                    else if (result == DialogResult.Ignore)
                    {
                        return;
                    }
                }
            }

            _waveBank = new WaveBank(_engine, "Content/Assets/Audio/SDWaveBank.xwb", 0, (short)16);
            _soundBank = new SoundBank(_engine, "Content/Assets/Audio/SDSoundBank.xsb");
            
        }

        public void Update()
        {
            _engine.Update();
        }

        public void PlayCue(Banks bank, string name)
        {
            this._soundBanks[(int)bank].PlayCue(name);
        }
    }
}