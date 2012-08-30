//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - Trigger
//--    
//--    
//--    Description
//--    ===============
//--    Functions are a multiuse trigger.
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Fixed development view to see triggers.
//--    BenG - Vastly changed how triggers collide with things. Greatly increases
//--           accuracy on objects entering and leaving the sensor.
//--    
//--    
//--    
//-------------------------------------------------------------------------------

//#define Development

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics.Contacts;
using System.ComponentModel;
using GameLibrary.Helpers;
using GameLibrary.Graphics.UI;
using GameLibrary.Graphics;
using GameLibrary.GameLogic.Events;
using GameLibrary.GameLogic.Characters;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Audio;
#endregion

namespace GameLibrary.GameLogic.Objects.Triggers
{
    public class Trigger : StaticObject
    {
        #region Fields

        [ContentSerializer(Optional = true)]
        protected bool _showHelp;
        [ContentSerializer(Optional = true)]
        protected TriggerType _triggerType;
        [ContentSerializer(Optional = true)]
        protected float _triggerWidth;
        [ContentSerializer(Optional = true)]
        protected float _triggerHeight;
        [ContentSerializer(Optional = true)]
        protected string _message = " to use.";
        [ContentSerializer(Optional = true)]
        private bool _triggerOnce = true;

#if EDITOR || Development
        private Texture2D _devTexture;
#endif

#if !EDITOR
        protected bool _triggered = false;
        private bool _fired = false;
        protected InteractType _interactType;
#endif


        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public virtual TriggerType TriggerType
        {
            get
            {
                return _triggerType;
            }
            set
            {
                _triggerType = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public virtual float TriggerWidth
        {
            get
            {
                return _triggerWidth;
            }
            set
            {
                _triggerWidth = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public virtual float TriggerHeight
        {
            get
            {
                return _triggerHeight;
            }
            set
            {
                _triggerHeight = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public virtual string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public bool TriggerOnce
        {
            get
            {
                return _triggerOnce;
            }
            set
            {
                _triggerOnce = value;
            }
        }

#else
        [ContentSerializerIgnore]
        public bool Triggered
        {
            get
            {
                return _triggered;
            }
            protected set
            {
                _triggered = value;
            }
        }
        [ContentSerializerIgnore]
        public float TriggerWidth
        {
            get
            {
                if (_orientation == Objects.Orientation.Up || _orientation == Objects.Orientation.Down)
                {
                    return _triggerWidth;
                }
                else
                {
                    return _triggerHeight;
                }
            }
            protected set
            {
                if (_orientation == Objects.Orientation.Up || _orientation == Objects.Orientation.Down)
                {
                    _triggerWidth = value;
                }
                else
                {
                    _triggerHeight = value;
                }
            }
        }
        [ContentSerializerIgnore]
        public float TriggerHeight
        {
            get
            {
                if (_orientation == Objects.Orientation.Up || _orientation == Objects.Orientation.Down)
                {
                    return _triggerHeight;
                }
                else
                {
                    return _triggerWidth;
                }
            }
            protected set
            {
                if (_orientation == Objects.Orientation.Up || _orientation == Objects.Orientation.Down)
                {
                    _triggerHeight = value;
                }
                else
                {
                    _triggerWidth = value;
                }
            }
        }
#endif
        #endregion

        #region Constructor

        public Trigger() { }        

        public override void Init(Vector2 position)
        {
            this.TriggerWidth = 200;
            this.TriggerHeight = 200;
            this._position = position;
            this._message = " to use.";
            this._zLayer = 0.5f;
        }
        #endregion

        public override void Load(ContentManager content, World world)
        {
#if Development && !EDITOR
            _devTexture = content.Load
                <Texture2D>(Defines.DEVELOPMENT_TEXTURE);
#endif

#if EDITOR
            _devTexture = content.Load
                <Texture2D>(Defines.DEVELOPMENT_TEXTURE);

            if (this.Width == 0.0f || this.Height == 0.0f)
            {
                this.Width = TriggerWidth;
                this.Height = TriggerHeight;
            }
#else
            this.Triggered = false;
            this.ChooseMessage();
            this.RegisterObject();
            this.SetupPhysics(world);
#endif


        }

        #region Update and Draw

        public override void Update(float delta)
        {
#if !EDITOR
            if (_triggerOnce && _fired)
            {
                return;
            }

            if (_triggered)
            {
                if (Player.Instance.PlayerState == PlayerState.Dead)
                {
                    ChangeTriggered(false);
                    return;
                }

                if (_triggerType == TriggerType.PlayerInput)
                {
                    if (InputManager.Instance.Interact(true))
                    {
                        this.FireEvent();
                        AudioManager.Instance.PlayCue("Switch", true);
                    }
                }
                else
                {
                    this.FireEvent();
                }
            }

#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_devTexture, this._position,
                new Rectangle(0, 0, (int)_width, (int)_height),
                Color.White * 0.3f, this._rotation, new Vector2(this._width, this._height) * 0.5f, 1f, SpriteEffects.None, 0.01f);
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
#if Development
            sb.Draw(_devTexture, this._position,
                new Rectangle(0, 0, (int)_width, (int)_height),
                Color.White * 0.3f, this.Body.Rotation, new Vector2(this._width, this._height) * 0.5f, 1f, SpriteEffects.None, 0.01f);
#endif
        }
#endif
        #endregion

        #endregion

        #region Private Methods

#if !EDITOR

        protected override void FireEvent()
        {
            base.FireEvent();

            if (_triggerType == TriggerType.Automatic)
            {
                this._fired = true;
                ChangeTriggered(false);
            }

            if (_triggerOnce)
            {
                EventManager.Instance.DeregisterObject(this);
                this.Disable();
                this.ChangeTriggered(false);
            }
        }

        #region Collisions
        /// <summary>
        /// Event handler on collision
        /// 
        /// ============================
        /// Notes:
        ///     HUD show messages should be called on/off in collision. Calling in update with if's 
        ///     turns it off with other prefabs updates.
        /// 
        /// </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (_touchingFixtures.Count == 0 && Player.Instance.CheckBodyBox(fixtureB))
            {
                _touchingFixtures.Add(fixtureB);

                if (!_triggered)
                {
                    ChangeTriggered(true);
                }

                return true;
            }

            //  If it's not the player, we can just ignore it.
            return true;
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            //  Only the players body boxes are added to the touchingfixtures list.
            if (_touchingFixtures.Contains(fixtureB))
            {
                _touchingFixtures.Remove(fixtureB);

                //  So if it's empty, the player has fully left the trigger.
                if (_touchingFixtures.Count == 0)
                {
                    //  so turn it off.
                    ChangeTriggered(false);

                    if (!_triggerOnce)
                    {
                        this._fired = false;
                    }
                }
            }
        }

        protected virtual void ChangeTriggered(bool state)
        {
            if (state)
            {
                if (_triggerType == TriggerType.PlayerInput)
                {
                    HUD.Instance.ShowOnScreenMessage(true, _message, 3);
                }
            }
            else
            {
                HUD.Instance.ShowOnScreenMessage(false, "", 3);
            }

            this._triggered = state;
        }

        #endregion

        protected override void SetupPhysics(World world)
        {
            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(_width), ConvertUnits.ToSimUnits(_height), 0f);
            this.Body.Position = ConvertUnits.ToSimUnits(this._position);
            this.Body.IsSensor = true;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;

            this.Body.Enabled = _enabled;

            if (_triggerType == TriggerType.Automatic)
            {

            }
            else
            {
                ChooseMessage();
            }
        }

        public override void Enable()
        {
            base.Enable();
            ChangeTriggered(false);
        }

        public override void Disable()
        {
            base.Disable();
            ChangeTriggered(false);
        }

        protected void ChooseMessage()
        {
#if !EDITOR
            if (_triggerType == TriggerType.Automatic)
            {
                _message = null;
            }
            else
            {
                if (_message == "")
                {
                    switch (_interactType)
                    {
                        case InteractType.Continue:
                            _message = " to continue";
                            break;
                        case InteractType.Grab:
                            _message = " to grab";
                            break;
                        case InteractType.Open:
                            _message = " to open";
                            break;
                        case InteractType.PickUp:
                            _message = " to pick up";
                            break;
                        case InteractType.Use:
                            _message = " to use";
                            break;
                    }
                }
            }
#endif
        }

#endif

        #endregion
    }
}
