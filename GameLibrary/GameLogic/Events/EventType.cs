using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.GameLogic.Events
{
    public enum EventType
    {
        // Used for targets
        NONE,   

        //  Generic
        ON_TOUCH,
        ON_LEAVE,
        ON_KILL,
        ON_ACTIVATION,
        ON_DEACTIVATION,
        
        ENABLE_COLLISION,
        DISABLE_COLLISION,

        //  Motor events
        START_MOTOR,
        STOP_MOTOR,
        CHANGE_SPEED,
        TOGGLE,

        //  System
        CHANGE_LEVEL,
        FIRE_POPUP
    }
}
