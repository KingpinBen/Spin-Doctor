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
//--    TBD
//--    ==============
//--    Make it work properly with other objects
//--    
//--    
//-------------------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Screens;
using GameLibrary.Assists;
using GameLibrary.Drawing;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Managers;
using FarseerPhysics.Dynamics.Contacts;
using System.ComponentModel;
#endregion

namespace GameLibrary.Objects.Triggers
{
    public class Trigger : PhysicsObject
    {
        #region Fields

        [ContentSerializer]
        private bool _showHelp;
        [ContentSerializer]
        private float _triggerWidth;
        [ContentSerializer]
        private float _triggerHeight;
        [ContentSerializer]
        protected string _message = " to use.";

#if EDITOR
        private Texture2D _devTexture;
#else
        [ContentSerializerIgnore]
        protected List<Fixture> TouchingFixtures = new List<Fixture>();
        [ContentSerializerIgnore]
        private bool _triggered;
#endif

        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public virtual bool ShowHelp
        {
            get
            {
                return _showHelp;
            }
            set
            {
                _showHelp = value;
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
#else
        [ContentSerializerIgnore]
        public virtual string Message
        {
            get
            {
                return _message;
            }
            protected set
            {
                _message = value;
            }
        }
        [ContentSerializerIgnore]
        public bool ShowHelp
        {
            get
            {
                return _showHelp;
            }
            protected set
            {
                _showHelp = value;
            }
        }
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
                return _triggerWidth;
            }
            protected set
            {
                _triggerWidth = value;
            }
        }
                [ContentSerializerIgnore]
        public float TriggerHeight
        {
            get
            {
                return _triggerHeight;
            }
            protected set
            {
                _triggerHeight = value;
            }
        }
#endif
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a multiple use trigger.
        /// </summary>
        /// <param name="position">Position of trigger</param>
        /// <param name="tHeight">trigger height</param>
        /// <param name="tWidth">trigger width</param>
        public Trigger()
        {

        }
        

        public virtual void Init(Vector2 position, float tWidth, float tHeight)
        {
            this._width = tWidth;
            this._height = tHeight;
            this._position = position;
            this._tint = Color.White;
            this.ShowHelp = true;
            this.Message = " to use.";
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);
#if EDITOR
            _devTexture = content.Load
                <Texture2D>(FileLoc.DevTexture());
#else
            //  Adds a space if there isn't one. Used to place the words correctly in the hud.
            //  Should really go in the editor... Remind Sam.
            if (ShowHelp)
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

            this.Triggered = false;

            SetUpTrigger(world);
#endif
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            
        }
        #endregion

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_devTexture, Position,
                new Rectangle(0, 0, (int)Width, (int)Height),
                Color.White, this._rotation, new Vector2(this.Width/2, this.Height/2), 1f, SpriteEffects.None, 0f);
        }
#else
        public override void Draw(SpriteBatch sb)
        {
#if Development
            sb.Draw(_devTexture, ConvertUnits.ToDisplayUnits(Body.Position),
                new Rectangle(0, 0, (int)Width, (int)Height),
                Color.White, Body.Rotation, new Vector2(this.Width/2, this.Height/2), 1f, SpriteEffects.None, 0f);
#endif
        }
#endif
        #endregion

        #region Protected/Private Methods

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
#if EDITOR
            return true;
#else
            if (!TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Add(fixtureB);
            }

            if (!Triggered)
            {
                this.Triggered = true;

                if (this.ShowHelp)
                {
                    HUD.ShowOnScreenMessage(true, Message);
                }
            }

            return true;
#endif

        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
#if EDITOR

#else
            if (TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Remove(fixtureB);
            }

            if (TouchingFixtures.Count == 0)
            {
                this.Triggered = false;

                if (this.ShowHelp && HUD.ShowPopup)
                {
                    HUD.ShowOnScreenMessage(false);
                }
            }
#endif
        }
        #endregion

        #region Setup Trigger
        protected virtual void SetUpTrigger(World world)
        {
#if EDITOR
#else
            Vector2 trigPos = Position;
            trigPos.Y -= Height / 2;

            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(_triggerWidth), ConvertUnits.ToSimUnits(_triggerHeight), 0f);
            this.Body.Position = ConvertUnits.ToSimUnits(trigPos);
            this.Body.IsSensor = true;
            //this.Body.CollidesWith = Category.Cat10;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
#endif
        }
        #endregion

        #endregion
    }
}
