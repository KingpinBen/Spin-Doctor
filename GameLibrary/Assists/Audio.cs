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
#endregion

namespace GameLibrary.Assists
{
    public class Audio
    {
        #region Variables

        public static AudioEngine AudioEngine { get; internal set; }
        public static SoundBank SoundBank { get; internal set; }
        public static WaveBank WaveBank { get; internal set; }

        #endregion

        #region Constructor
        public Audio()
        {
            
        }
        #endregion

        #region Load
        public static void Load()
        {
            //  >>Having no enabled audio drivers can cause this to fail<<
            AudioEngine = new AudioEngine(          "Content/Assets/Audio/SDAudio.xgs");
            WaveBank = new WaveBank(AudioEngine,    "Content/Assets/Audio/SDWaveBank.xwb", 0, (short)16);
            SoundBank = new SoundBank(AudioEngine, "Content/Assets/Audio/SDSoundBank.xsb");
            
        }
        #endregion

        #region Update
        public static void Update()
        {
            AudioEngine.Update();
        }
        #endregion
    }
}