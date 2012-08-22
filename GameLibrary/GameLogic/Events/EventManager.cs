using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameLogic.Screens;
using GameLibrary.GameLogic.Objects;
using GameLibrary.GameLogic.Objects.Triggers;
using GameLibrary.Helpers;
using GameLibrary.Graphics.Camera;
using GameLibrary.GameLogic.Characters;
using GameLibrary.GameLogic.Screens.Menu;
using GameLibrary.Audio;
using GameLibrary.Graphics.UI;
using Microsoft.Xna.Framework.Audio;

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

        #region Fields
        private GameplayScreen gameScreen;
        private Dictionary<string, List<Event>> eventObjects;
        private Dictionary<string, NodeObject> objects;
        private List<Event> timedEvents;
        #endregion

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
            if (timedEvents.Count > 0)
            {
                for (int i = 0; i < timedEvents.Count; i++)
                {
                    timedEvents[i].EventDelay -= delta;
                }

                for (int i = timedEvents.Count - 1; i >= 0; i--)
                {
                    if (timedEvents[i].EventDelay <= 0)
                    {
                        FireEvent(timedEvents[i]);
                        timedEvents.RemoveAt(i);
                    }
                }
            }
#endif
        }

#if !EDITOR

        #region Private Methods

        /// <summary>
        /// Handles the actual event
        /// </summary>
        /// <param name="objEvent">The event to fire</param>
        private void FireEvent(Event objEvent)
        {
            switch (objEvent.EventType)
            {
                case EventType.AUDIO_PAUSE_SONGS:
                    {
                        AudioManager.Instance.PauseSounds();

                        break;
                    }
                case EventType.AUDIO_RESUME_SONGS:
                    {
                        AudioManager.Instance.ResumeSounds();

                        break;
                    }
                case EventType.AUDIO_STOP_SOUNDS:
                    {
                        AudioManager.Instance.StopAllCues(AudioStopOptions.AsAuthored);

                        break;
                    }


                #region ChangeLevel
                case EventType.ENGINE_CHANGE_LEVEL:
                    {
                        try
                        {
                            this.gameScreen.CurrentLevelID = Convert.ToInt32(objEvent.Argument);
                        }
                        catch
                        {
                            ErrorReport.GenerateReport("Tried loading a level with an invalid target level definition in\nlevel " + this.gameScreen.CurrentLevelID, null);
                        }
                    }
                    break;
                #endregion
                #region TriggerDeath
                case EventType.TRIGGER_DEATH:
                    {
                        Player.Instance.Kill();
                    }
                    break;
                #endregion
                #region TriggerStop
                case EventType.TRIGGER_STOP:
                    {
                        NodeObject output;

                        if (this.objects.TryGetValue(objEvent.TargetName, out output))
                        {
                            output.Stop();
                        }

                        break;
                    }
                    #endregion
                #region TriggerStart
                case EventType.TRIGGER_START:
                    {
                        NodeObject output;

                        if (this.objects.TryGetValue(objEvent.TargetName, out output))
                        {
                            output.Start();
                        }
                        break;
                    }
                    #endregion
                #region TriggerToggle
                case EventType.TRIGGER_TOGGLE:
                    {
                        NodeObject output;

                        if (this.objects.TryGetValue(objEvent.TargetName, out output))
                        {
                            output.Toggle();
                        }

                        break;
                    }
                    #endregion
                #region TriggerEnable
                case EventType.TRIGGER_ENABLE:
                    {
                        NodeObject output;

                        if (this.objects.TryGetValue(objEvent.TargetName, out output))
                        {
                            output.Enable();
                        }

                        break;
                    }
                    #endregion
                #region TriggerDisable
                case EventType.TRIGGER_DISABLE:
                    {
                        NodeObject output;

                        if (this.objects.TryGetValue(objEvent.TargetName, out output))
                        {
                            output.Disable();
                        }

                        break;
                    }
                    #endregion
                #region TriggerChangeSpeed
                case EventType.TRIGGER_CHANGE_SPEED:
                    {
                        NodeObject output;

                        if (this.objects.TryGetValue(objEvent.TargetName, out output))
                        {
                            output.Change(objEvent.Argument);
                        }
                    }

                    break;
                #endregion
                #region TriggerChangeSpeed
                case EventType.TRIGGER_CHANGE:
                    {
                        NodeObject output;

                        if (this.objects.TryGetValue(objEvent.TargetName, out output))
                        {
                            output.Change(objEvent.Argument);
                        }
                    }

                    break;
                #endregion
                #region TriggerRemove
                case EventType.TRIGGER_REMOVE:
                    {
                        NodeObject output;

                        if (this.objects.TryGetValue(objEvent.TargetName, out output))
                        {
                            this.gameScreen.RemoveObject(output);
                        }

                        break;
                    }
                    #endregion
                #region TriggerToggleShadow
                case EventType.TRIGGER_TOGGLE_SHADOW:
                    {
                        NodeObject output;

                        if (this.objects.TryGetValue(objEvent.TargetName, out output))
                        {
                            output.ToggleShadows();
                        }
                    }
                    break;
                #endregion

                #region WorldRotateCCW90
                case EventType.WORLD_ROTATE_CCW90:
                    {
                        Camera.Instance.ForceRotateLeft();
                    }

                    break;
                    #endregion
                #region WorldRotateCW90
                case EventType.WORLD_ROTATE_CW90:
                    {
                        Camera.Instance.ForceRotateRight();
                    }

                    break;
                    #endregion
                #region WorldRotate180
                case EventType.WORLD_ROTATE_180:
                    {
                        Camera.Instance.ForceRotateHalf();
                    }

                    break;
                #endregion
                #region WorldRotateEnable
                case EventType.WORLD_ROTATE_ENABLE:
                    {
                        Camera.Instance.ChangeLevelRotateAbility(true);
                    }

                    break;
                #endregion
                #region WorldRotateToggle
                case EventType.WORLD_ROTATE_TOGGLE:
                    {
                        Camera.Instance.ChangeLevelRotateAbility(!Camera.Instance.LevelRotates);
                    }

                    break;
                    #endregion
                #region WorldRotateDisable
                case EventType.WORLD_ROTATE_DISABLE:
                    {
                        Camera.Instance.ChangeLevelRotateAbility(false);
                    }

                    break;
                #endregion

                #region Play Song
                case EventType.AUDIO_PLAY_SONG:
                    {
                        string songName = objEvent.TargetName;

                        AudioManager.Instance.LoadSong(songName, songName + "1");

                        break;
                    }
                #endregion

                case EventType.UNLOCK_ITEM_STEAMBOOTS:
                    {
                        GameSettings.Instance.DoubleJumpEnabled = true;
                        break;
                    }
                case EventType.ENGINE_ROLL_CREDITS:
                    {
                        this.gameScreen.ScreenManager.AddScreen(new CreditRollScreen(), gameScreen.ControllingPlayer);
                        break;
                    }
                case EventType.ENGINE_FORCE_MAIN_MENU:
                    {
                        LoadingScreen.Load(this.gameScreen.ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
                        break;
                    }

                case EventType.ENGINE_HUD_MESSAGE:
                    {
                        int num = 0;
                        if (objEvent.Argument != null)
                            num = (int)objEvent.Argument;

                        HUD.Instance.CreateTemporaryPopup(objEvent.TargetName, num);
                        break;
                    }

                default:
                    break;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Fire all the events that the object has.
        /// </summary>
        /// <param name="objName">The name of the object</param>
        public void FireEvent(string objName)
        {
            //  If the name is empty, it's neither targettable nor able to fire an
            //  event, so we'll just leave it.
            if (objName != null)
            {
                //  The objects events that we'll output to and grab from.
                List<Event> objEvents;

                //  We check if the fired objects name is in the list and if
                //  it is, we output it's value to the above list, so we don't
                //  have to keep finding it inside the dictionary
                if (this.eventObjects.TryGetValue(objName, out objEvents))
                {
                    //  Start triggering all the objects events
                    for (int i = 0; i < objEvents.Count; i++)
                    {
                        if (objEvents[i].EventDelay > 0)
                        {
                            //  If an event is on a timer, we'll add it to a list to fire 
                            //  when it's ready
                            timedEvents.Add(objEvents[i]);
                        }
                        else
                        {
                            //  If theres no timer, we'll just fire it now.
                            FireEvent(objEvents[i]);
                        }
                    }
                }
            }
        }

        #region Register and Deregister Events

        /// <summary>
        /// Add an object that will be used for an event.
        /// </summary>
        /// <param name="obj">The object</param>
        public void RegisterObject(NodeObject obj)
        {
            //  No need to register anything without a name.
            if (obj.Name != null && obj.Name != "")
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

        public void DeregisterObject(NodeObject obj)
        {
            if (obj.Name != null || obj.Name != "")
            {
                if (obj.ObjectEvents.Count > 0)
                {
                    this.eventObjects.Remove(obj.Name);
                }
            }
        }

        #endregion

        public void ClearObjects()
        {
            this.eventObjects.Clear();
            this.objects.Clear();
            this.timedEvents.Clear();
        }

        #endregion

#endif
    }
}
