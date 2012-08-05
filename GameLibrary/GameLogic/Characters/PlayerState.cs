using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.GameLogic.Characters
{
    public enum PlayerState
    {
        Running,
        Grounded,
        Jumping,
        Climbing,
        Swinging,
        Falling,
        Dead
    }
}
