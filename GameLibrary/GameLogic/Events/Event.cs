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
        private uint MAX_ARGS = 4;
        [ContentSerializer]
        private string _objectName;
        [ContentSerializer]
        private string _targetName;
        [ContentSerializer]
        private EventType _eventType;
        [ContentSerializer]
        private float _eventDelay;
        [ContentSerializer]
        private object _argument;

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
        [ContentSerializerIgnore]
        public object Argument
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

        public Event()
        {
            this._objectName = "Enter THIS objects name here";
            this._targetName = "Enter the TARGETS name here";
            this._eventType = Events.EventType.NONE;
            this._eventDelay = 0.0f;
            this._argument = new object[MAX_ARGS];
        }

        public Event(string name, string target, EventType type, float delay, params object[] args)
        {
            if (args != null)
            {
                if (args.Length > MAX_ARGS)
                {
                    string error = "Object: " + name + " contains too many arguments.";

                    MessageBox.Show(error);
                    ErrorReport.GenerateReport(error + args.ToString(), null);
                }
                else
                {
                    this.Argument = args;
                }
            }

            this.ObjectName = name;
            this.TargetName = target;
            this.EventDelay = delay;
            this.EventType = type;
            
        }
    }
}
