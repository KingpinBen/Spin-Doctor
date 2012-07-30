using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.GameLogic.Events
{
    public enum EventType
    {
        /// <summary>
        /// Used by the event manager to declare a target with no event output.
        /// </summary>
        NONE,   

        /// <summary>
        /// Handled by the object.
        /// </summary>
        TRIGGER_ENABLE,
        TRIGGER_DISABLE,
        TRIGGER_START,
        TRIGGER_STOP,
        TRIGGER_CHANGESPEED,
        TRIGGER_TOGGLE,

        /// <summary>
        /// Handled through EventManager rather than the object.
        /// </summary>
        TRIGGER_REMOVE,
        ROTATE_CW90,
        ROTATE_CCW90,
        ROTATE_180,

        //  System
        CHANGE_LEVEL,
        FIRE_POPUP
    }
}
