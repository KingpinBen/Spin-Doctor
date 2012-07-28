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

        TRIGGER_ENABLE,
        TRIGGER_DISABLE,

        TRIGGER_START,
        TRIGGER_STOP,
        TRIGGER_CHANGESPEED,
        TRIGGER_TOGGLE,

        TRIGGER_REMOVE,

        //  System
        CHANGE_LEVEL,
        FIRE_POPUP
    }
}
