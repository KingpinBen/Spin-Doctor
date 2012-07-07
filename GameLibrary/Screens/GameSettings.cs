using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Screens
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
}
