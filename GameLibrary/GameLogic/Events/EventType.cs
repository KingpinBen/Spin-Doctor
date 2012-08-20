﻿using System;
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
        TRIGGER_CHANGE,
        TRIGGER_TOGGLE,
        TRIGGER_TOGGLE_SHADOW,
        

        /// <summary>
        /// Handled through EventManager rather than the object.
        /// </summary>
        TRIGGER_REMOVE,
        TRIGGER_DEATH,

        WORLD_ROTATE_CW90,
        WORLD_ROTATE_CCW90,
        WORLD_ROTATE_180,
        WORLD_ROTATE_DISABLE,
        WORLD_ROTATE_ENABLE,
        WORLD_ROTATE_TOGGLE,

        //  System
        ENGINE_CHANGE_LEVEL,
        ENGINE_ROLL_CREDITS,
        UNLOCK_ITEM_STEAMBOOTS,
    }
}
