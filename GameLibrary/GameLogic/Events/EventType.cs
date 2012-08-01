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
        TRIGGER_CHANGE_SPEED,
        TRIGGER_TOGGLE,

        /// <summary>
        /// Handled through EventManager rather than the object.
        /// </summary>
        TRIGGER_REMOVE,


        WORLD_ROTATE_CW90,
        WORLD_ROTATE_CCW90,
        WORLD_ROTATE_180,
        WORLD_ROTATE_DISABLE,
        WORLD_ROTATE_ENABLE,
        WORLD_ROTATE_TOGGLE,

        //  System
        CHANGE_LEVEL,
        
        //  Change camera.
        FIRE_POPUP
    }
}
