using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameLibrary.GameLogic
{
    internal class GameSettings
    {
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
            this._shadows = SettingLevel.Off;
        }


        private SettingLevel _shadows;
        private bool _allowDoubleJump = false;

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
    }

    public static class SessionSettings
    {
        private static bool _developmentMode = true;

        public static bool DevelopmentMode
        {
            get
            {
                return _developmentMode;
            }
        }

        public static void SetDevelopment(object sender, bool state)
        {
            _developmentMode = state;
        }
    }
}
