using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Helpers;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.GameLogic.Events
{
    public class Event
    {
        #region Fields 

        [ContentSerializer]
        private string _objectName;
        [ContentSerializer]
        private string _targetName;
        [ContentSerializer]
        private EventType _eventType;
        [ContentSerializer]
        private float _eventDelay;
        [ContentSerializer]
        private float? _argument = 0.0f;

        #endregion

        #region Properties

        /// <summary>
        /// The name of the object to which the event
        /// belongs to.
        /// </summary>
        [ContentSerializerIgnore]
        public string ObjectName
        {
            get
            {
                return _objectName;
            }
            set
            {
                _objectName = value;
            }
        }

        /// <summary>
        /// The name of the object to target.
        /// </summary>
        [ContentSerializerIgnore]
        public string TargetName
        {
            get
            {
                return _targetName;
            }
            set
            {
                _targetName = value;
            }
        }

        /// <summary>
        /// The type of event that will fire on the target object.
        /// </summary>
        [ContentSerializerIgnore]
        public EventType EventType
        {
            get
            {
                return _eventType;
            }
            set
            {
                _eventType = value;
            }
        }

        /// <summary>
        /// The delay time between when it's been triggered to
        /// fire and when it fires.
        /// </summary>
        [ContentSerializerIgnore]
        public float EventDelay
        {
            get
            {
                return _eventDelay;
            }
            set
            {
                _eventDelay = value;
            }
        }

        /// <summary>
        /// Certain event types will use an argument, such as changing
        /// and setting the rotational speed of something.
        /// </summary>
        [ContentSerializerIgnore]
        public float? Argument
        {
            get
            {
                return _argument;
            }
            set
            {
                _argument = value;
            }
        }

        #endregion

        public Event()
        {
            this._objectName = "This";
            this._targetName = "Target";
            this._eventType = Events.EventType.NONE;
            this._eventDelay = 0.0f;
        }

        public Event(string name, string target, EventType type, float delay, int arg)
        {
            this.ObjectName = name;
            this.TargetName = target;
            this.EventDelay = delay;
            this.EventType = type;
            this.Argument = arg;
        }
    }
}
