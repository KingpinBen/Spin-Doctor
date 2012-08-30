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
using Microsoft.Xna.Framework.Media;
using System.IO;
#endregion

namespace GameLibrary.Audio
{
    public class AudioManager
    {
        #region Fields

        private Game _game;

        private AudioEngine _engine;
        private SoundBank _soundBank;
        private WaveBank _waveBank;

        private int _musicVolume;
        private int _effectsVolume;
        private int _voiceVolume;
        private int _ambienceVolume;


        private List<Cue> _activeSounds = new List<Cue>(255);
        Dictionary<string, Song> _musicBank = new Dictionary<string,Song>();       

        #endregion

        #region Properties

        #region Music Getter and Setters


        public int MusicVolume
        {
            get
            {
                return _musicVolume;
            }
        }


        /// <summary>
        /// Set the music volume
        /// </summary>
        /// <param name="volumeLevel">Desired volume level. 0-10</param>
        public void SetMusicVolume(int volumeLevel)
        {
            //  Cap the volume level between 0 and 10
            _musicVolume = volumeLevel;

            //  If we can use the 
            if (this._engine != null)
            {
                MediaPlayer.Volume = this._musicVolume * 0.1f;
                this._engine.GetCategory("Music").SetVolume(this._musicVolume * 0.1f);
            }
        }


        #endregion

        #region Effects Get Set

        public int EffectsVolume
        {
            get
            {
                return _effectsVolume;
            }
        }

        public void SetEffectsVolume(int volumeLevel)
        {
            //  Cap the volume level between 0 and 10
            _effectsVolume = volumeLevel;

            //  If we can use the 
            if (this._engine != null)
            {
                this._engine.GetCategory("Effects").SetVolume(this._effectsVolume * 0.1f);
            }
        }

        #endregion

        #region Voice Get Set

        public int VoiceVolume
        {
            get
            {
                return _voiceVolume;
            }
        }

        public void SetVoiceVolume(int volumeLevel)
        {
            //  Cap the volume level between 0 and 10
            _voiceVolume = volumeLevel;

            //  If we can use the 
            if (this._engine != null)
            {
                this._engine.GetCategory("Voice").SetVolume(this._voiceVolume * 0.1f);
            }
        }

        #endregion

        #region Ambience Get Set

        public int AmbienceVolume
        {
            get
            {
                return _ambienceVolume;
            }
        }

        public void SetAmbienceVolume(int volumeLevel)
        {
            //  Cap the volume level between 0 and 10
            _ambienceVolume = volumeLevel;

            //  If we can use the 
            if (this._engine != null)
            {
                this._engine.GetCategory("Background").SetVolume(this._ambienceVolume * 0.1f);
            }
        }

        #endregion

        #endregion

        #region Constructor and Load
        private static AudioManager _singleton = new AudioManager();
        public static AudioManager Instance
        {
            get
            {
                return _singleton;
            }
        }

        private AudioManager() { }
        
        public void Load(Game game)
        {
            this._game = game;

            while (_engine == null)
            {
                try
                {
                    _engine = new AudioEngine("Content\\Assets\\Audio\\Audio.XGS");

                    _waveBank = new WaveBank(_engine, "Content\\Assets\\Audio\\Wave Bank.XWB");
                    _soundBank = new SoundBank(_engine, "Content\\Assets\\Audio\\Sound Bank.XSB");
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
        }
        #endregion

        #region Update

        public void Update()
        {
            for (int i = _activeSounds.Count - 1; i >= 0; i--)
            {
                if (_activeSounds[i].IsStopped)
                {
                    _activeSounds.RemoveAt(i);
                }
            }

            _engine.Update();
        }
        #endregion

        #region Cue Control

        public Cue PlayCue(string name, bool play)
        {
            Cue cue = null;

            cue = this._soundBank.GetCue(name);

            if (play)
            {
                this.PlayCue(cue);
            }

            //  Return it in case the object wants a reference to it.
            return cue;
        }

        public void PlayCue(Cue cue)
        {
            _activeSounds.Add(cue);
            cue.Play();
        }

        public void StopCue(string name, AudioStopOptions option)
        {
            Cue cue = null;
            try
            {
                cue = _soundBank.GetCue(name);
            }
            catch
            {
                //  If name doesn't exist as a cue, exit out.
                return;
            }

            this.StopCue(cue, option);
        }

        public void StopCue(Cue cue, AudioStopOptions option)
        {
            if (cue != null)
            {
                cue.Stop(option);
            }
        }

        public void StopAllCues(AudioStopOptions option)
        {
            for (int i = 0; i < _activeSounds.Count; i++)
            {
                Cue cue = _activeSounds[i];

                if (!cue.IsStopped)
                {
                    cue.Stop(option);
                }
            }
        }

        public void StopAllSounds(AudioStopOptions option)
        {
            for(int i = 0; i < _activeSounds.Count; i++)
            {
                Cue cue = _activeSounds[i];

                if (!cue.IsStopped)
                {
                    cue.Stop(option);
                }
            }

            this.StopMusic();
        }

        #endregion

        #region Music Control

        public void LoadSong(string name, string nameAlias)
        {
            string path = Defines.SYSTEM_AUDIO_DIRECTORY;

            if (File.Exists("./Content/" + path + name + ".wma"))
            {
                Song song = this._game.Content.Load<Song>("Assets/Audio/" + name);

                if (_musicBank.ContainsKey(nameAlias))
                {
                    this._musicBank.Remove(nameAlias);
                }
                
                this._musicBank.Add(nameAlias, song);
                MediaPlayer.Play(song);
            }
        }

        public void StopMusic()
        {
            if (MediaPlayer.State != MediaState.Stopped)
            {
                MediaPlayer.Stop();
                this._musicBank.Clear();
            }
        }


        #endregion

        public void PauseSounds()
        {
            for (int i = 0; i < _activeSounds.Count; i++)
            {
                if (!_activeSounds[i].IsPaused)
                {
                    try
                    {
                        _activeSounds[i].Pause();
                    }
                    catch
                    {
                        _activeSounds[i].Stop(AudioStopOptions.Immediate);
                    }
                }
            }

            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
        }

        public void ResumeSounds()
        {
            for (int i = 0; i < _activeSounds.Count; i++)
            {
                if (_activeSounds[i].IsPaused)
                {
                    _activeSounds[i].Resume();
                }
            }

            if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
            }
        }


        public void PauseAllSounds(object sender, EventArgs e)
        {
            for (int i = 0; i < _activeSounds.Count; i++)
            {
                if (!_activeSounds[i].IsPaused)
                {
                    _activeSounds[i].Pause();
                }
            }

            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
        }

        public void ResumeAllSounds(object sender, EventArgs e)
        {
            for (int i = 0; i < _activeSounds.Count; i++)
            {
                if (_activeSounds[i].IsPaused)
                {
                    _activeSounds[i].Resume();
                }
            }

            if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
            }
        }

        
    }
}