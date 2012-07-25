using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameLogic.Screens;
using GameLibrary.GameLogic.Objects;
using GameLibrary.GameLogic.Objects.Triggers;
using GameLibrary.Helpers;

namespace GameLibrary.GameLogic.Events
{
    public class EventManager
    {
        #region Singleton and Instance Creation
        private static EventManager _singleton = new EventManager();
        private EventManager() { }

        public static EventManager Instance
        {
            get
            {
                return _singleton;
            }
        }
        #endregion

        private GameplayScreen gameScreen;
        private Dictionary<string, List<Event>> eventObjects;
        private Dictionary<string, NodeObject> objects;
        private List<Event> timedEvents;

        public void Load(GameplayScreen gameScreen)
        {
            this.eventObjects = new Dictionary<string, List<Event>>();
            this.timedEvents = new List<Event>();
            this.objects = new Dictionary<string, NodeObject>();
            this.gameScreen = gameScreen;
        }

        public void Update(float delta)
        {
#if !EDITOR
            for (int i = 0; i < timedEvents.Count; i++)
            {
                timedEvents[i].EventDelay -= delta;
            }

            for (int i = timedEvents.Count - 1; i >= 0; i--)
            {
                if (timedEvents[i].EventDelay <= 0)
                {
                    FireEvent(timedEvents[i]);
                }
            }
#endif
        }

        

#if !EDITOR
        /// <summary>
        /// Add an object that will be used for an event.
        /// </summary>
        /// <param name="obj">The object</param>
        public void RegisterObject(NodeObject obj)
        {
            //  No need to register anything without a name.
            if (obj.Name != null)
            {
                //  As the object has a name, we'll be using it for an event
                //  at some point.
                this.objects.Add(obj.Name, obj);

                //  Only some objects will need to register events, so only add 
                //  the ones that need to.
                if (obj.ObjectEvents.Count > 0)
                {
                    this.eventObjects.Add(obj.Name, obj.ObjectEvents);
                }
            }

        }

        public void FireEvent(string objName)
        {
            //  If the name is empty, it's neither targettable nor able to fire an
            //  event, so we'll just leave it.
            if (objName == null)
            {
                return;
            }

            //  The objects events
            List<Event> objEvents;

            //  We check if the fired objects name is in the list and if
            //  it is, we output it's value to the above list, so we don't
            //  have to keep finding it inside the dictionary
            if (this.eventObjects.TryGetValue(objName, out objEvents))
            {
                //  Start triggering all the objects events
                for (int i = 0; i < objEvents.Count; i++)
                {
                    //  Check if theres a delay on when the event should actually
                    //  fire from when the event is triggered
                    if (objEvents[i].EventDelay > 0)
                    {
                        timedEvents.Add(objEvents[i]); 
                    }
                    else
                    {
                        FireEvent(objEvents[i]);
                    }
                }
            }
        }

        private void FireEvent(Event objEvent)
        {
            switch (objEvent.EventType)
            {
                case EventType.CHANGE_LEVEL:
                    uint id = 0;
                    try
                    {
                        id = Convert.ToUInt32(objEvent.Argument);
                    }
                    catch
                    {
                        ErrorReport.GenerateReport(objEvent.ObjectName +
                            " tried to fire a changelevel without a convertable argument", null);
                    }
                    this.gameScreen.CurrentLevelID = id;
                    break;
                case EventType.STOP_MOTOR:
                    {
                        NodeObject output;

                        if (this.objects.TryGetValue(objEvent.TargetName, out output))
                        {
                            output.Stop();
                        }

                        break;
                    }
                case EventType.START_MOTOR:
                    {
                        NodeObject output;

                        if (this.objects.TryGetValue(objEvent.TargetName, out output))
                        {
                            output.Start();
                        }
                        break;
                    }
                case EventType.TOGGLE:
                    {
                        NodeObject output;

                        if (this.objects.TryGetValue(objEvent.TargetName, out output))
                        {
                            output.Toggle();
                        }

                        break;
                    }
            }
        }
#endif

        public int GetEventCount()
        {
            return this.eventObjects.Count;
        }
        public int GetTimedEventCount()
        {
            return this.timedEvents.Count;
        }
        public int GetObjectsCount()
        {
            return this.objects.Count;
        }

        public void ClearObjects()
        {
            this.eventObjects.Clear();
        }

    }
}
