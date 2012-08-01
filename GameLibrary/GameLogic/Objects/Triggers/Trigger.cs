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

#define Development

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
#endregion

namespace GameLibrary.GameLogic.Objects.Triggers
{
    public class Trigger : StaticObject
    {
        #region Fields

        [ContentSerializer]
        protected TriggerType _triggerType;
        [ContentSerializer]
        protected float _triggerWidth;
        [ContentSerializer]
        protected float _triggerHeight;
        [ContentSerializer]
        protected string _message = " to use.";
        [ContentSerializer]
        private bool _triggerOnce = true;

#if EDITOR || Development
        private Texture2D _devTexture;
#endif

#if !EDITOR
        protected List<Fixture> TouchingFixtures = new List<Fixture>();
        protected bool _triggered = false;
        private bool _fired = false;
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
                <Texture2D>(FileLoc.DevTexture());
#endif

#if EDITOR
            _devTexture = content.Load
                <Texture2D>(FileLoc.DevTexture());

            if (this.Width == 0.0f || this.Height == 0.0f)
            {
                this.Width = TriggerWidth;
                this.Height = TriggerHeight;
            }
#else
            this.Triggered = false;
            this.RegisterObject();
            this.SetupTrigger(world);
#endif


        }

        public override void Update(float delta)
        {
#if !EDITOR
            //  If it's set to fire once and has been fired, we don't 
            //  need to go any further.
            if (_triggerOnce && _fired)
            {
                return;
            }

            //  First check if it's been enabled.
            if (_enabled && (_triggered && !_fired))
            {
                //  Fire off all the events.
                this.FireEvent();

                this._fired = true;
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

        #region Private Methods

#if !EDITOR

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
            if (!TouchingFixtures.Contains(fixtureB) && Player.Instance.CheckBodyBox(fixtureB))
            {
                TouchingFixtures.Add(fixtureB);
            }
            else
            {
                //  If it's not the player, we can just ignore it.
                return true;
            }

            if (!Triggered)
            {
                this.Triggered = true;

                if (_triggerType == TriggerType.PlayerInput)
                {
                    HUD.Instance.ShowOnScreenMessage(true, _message);
                }
            }

            return true;
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            //  Only the players body boxes are added to the touchingfixtures list.
            if (TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Remove(fixtureB);
            }

            //  So if it's empty, the player has fully left the trigger.
            if (TouchingFixtures.Count == 0)
            {
                //  so turn it off.
                this._triggered = false;

                if (!this._triggerOnce)
                {
                    this._fired = false;
                }
                
                if (_triggerType == TriggerType.PlayerInput && HUD.Instance.ShowPopup)
                {
                    HUD.Instance.ShowOnScreenMessage(false);
                }
            }
        }

        #endregion

        protected virtual void SetupTrigger(World world)
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
                if (_message.Length == 0)
                {
                    _message = " FORGOTTEN TO PLACE A MESSAGE.";
                }

                if (_message[0] != ' ')
                {
                    _message.Insert(0, " ");
                }
            }
        }

        public override void Enable()
        {
            base.Enable();
            this._triggered = false;
        }

        public override void Disable()
        {
            base.Disable();
            this._triggered = false;
        }

#endif

        #endregion
    }
}
