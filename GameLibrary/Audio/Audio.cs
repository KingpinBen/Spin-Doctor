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
        private int _soundVolume;
        private int _musicVolume;
        private bool _isPlayingMusic;

        /// <summary>
        /// Temporary banks for testing.
        /// Switch to array when we have something here.
        /// </summary>
        private SoundBank _soundBank;
        private WaveBank _waveBank;

        #endregion

        #region Properties

        public int SoundVolume
        {
            get
            {
                return _soundVolume;
            }
            set
            {
                _soundVolume = (int)MathHelper.Clamp(_soundVolume, 0, 10);

                if (this._engine != null)
                {
                    this._engine.GetCategory("Sound").SetVolume(this._soundVolume * 0.1f);
                }
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
                _musicVolume = (int)MathHelper.Clamp(_musicVolume, 0, 10);

                if (this._engine != null)
                {
                    this._engine.GetCategory("Music").SetVolume(this._musicVolume * 0.1f);
                }
            }
        }


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
                        ErrorReport.GenerateReport("Couldn't find an active sound device.", null);
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