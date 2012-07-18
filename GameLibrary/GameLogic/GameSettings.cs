using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameLibrary.GameLogic
{
    public static class GameSettings
    {
        private static bool _drawShadows = false;
        public static bool DrawShadows
        {
            get
            {
                return _drawShadows;
            }
        }
        public static void ToggleShadows()
        {
            _drawShadows = !_drawShadows;
        }

        private static bool _allowDoubleJump = false;
        public static bool DoubleJumpEnabled
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
        public static void ToggleDoubleJump()
        {
            _allowDoubleJump = !_allowDoubleJump;
        }
    }

    public static class SessionSettings
    {
        private static bool _developmentMode = false;

        public static bool DevelopmentMode
        {
            get
            {
                return _developmentMode;
            }
        }

        public static void SetDevelopment(object sender, bool state)
        {
            if (sender is Game)
            {
                _developmentMode = state;
            }
        }
    }
}
