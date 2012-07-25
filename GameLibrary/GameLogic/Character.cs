//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Character
//--    
//--    Description
//--    ===============
//--    Creates character bodies and wheels
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Fixed angle with rotation
//--    BenG - No longer inherits from PhysicsObject
//--    BenG - Implemented a better way to handle collisions for state changing.
//--    BenG - Fixed a bug with the touchingfixtures were the fixtures were being carried to the next level.
//--    
//--    
//--    TBD
//--    ==============
//--    
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using GameLibrary.Helpers;
using GameLibrary.Graphics.Animation;
#endregion

namespace GameLibrary.GameLogic
{
    public class Character
    {
        #region Fields

        protected float _charMass;
        protected float _charWidth;
        protected float _charHeight;
        protected Body _wheelBody;
        protected Body _mainBody;
        protected RevoluteJoint _wheelJoint;
        protected Texture2D _charTexture;
        protected PlayerState _playerState;
        protected ContentManager _content;
        protected List<Fixture> _touchingFixtures;
        private Vector2 _texturePosition;
        protected Dictionary<string, FrameAnimation> _animations;
        private string _currentAnimation = null;
        protected SpriteEffects _lookingDirection;
        #endregion

        #region Properties

        public PlayerState PlayerState
        {
            get
            {
                return _playerState;
            }
            protected set
            {
                _playerState = value;
            }
        }

        protected RevoluteJoint WheelJoint
        {
            get
            {
                return _wheelJoint;
            }
            set
            {
                _wheelJoint = value;
            }
        }

        public Body WheelBody
        {
            get 
            { 
                return _wheelBody; 
            }
        }

        public Body Body
        {
            get
            {
                return _mainBody;
            }
        }

        public float TotalHeight
        {
            get 
            { 
                return (_charHeight - 8.0f) + (_charWidth * 0.5f); 
            }
        }

        public float CharWidth
        {
            get
            {
                return _charWidth;
            }
        }

        protected FrameAnimation CurrentAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentAnimation))
                {
                    return _animations[_currentAnimation];
                }
                else
                {
                    return null;
                }
            }
        }

        public string CurrentAnimationName
        {
            get 
            { 
                return _currentAnimation;
            }
            set
            {
                if (_animations.ContainsKey(value))
                {
                    _currentAnimation = value;
                }
            }
        }

        #region StartPosition
        /// <summary>
        /// Where to spawn the character.
        /// </summary>
        public Vector2 StartPosition { get; protected set; }
        #endregion

        #region SetPosition
        /// <summary>
        /// Alters the X coordinate of the player. 
        /// 
        /// Used for objects like ladders to shift the player correctly
        /// into place. 
        /// *Will probably have to change to use y too eventually.
        /// </summary>
        public Vector2 SetPosition
        {
            set
            {
                if (value.X == 0)
                {
                    this._wheelBody.Position = new Vector2(this._wheelBody.Position.X, value.Y);
                    Body.Position = new Vector2(this._wheelBody.Position.X, value.Y);
                }
                else if (value.Y == 0)
                {
                    WheelBody.Position = new Vector2(value.X, this._wheelBody.Position.Y);
                    Body.Position = new Vector2(value.X, this.Body.Position.Y);
                }

                
            }
        }
        #endregion

        #endregion

        public Character()
        {
            _animations = new Dictionary<string, FrameAnimation>();
        }

        public virtual void Load(Game game, World _world, Vector2 position)
        {
            _content = new ContentManager(game.Services, "Content");

            this._charHeight = 128f;
            this._charWidth = _charHeight;
            this._charMass = 60f;

            this.StartPosition = position;

            //  **Instantiate on load to get rid of any old fixtures.
            this._touchingFixtures = new List<Fixture>();

            SetUpPhysics(_world);
        }

        public virtual void Update(float delta, World world)
        {
            if (this._mainBody.Enabled == false)
            {
                return;
            }

            Vector2 tempPosition = ConvertUnits.ToDisplayUnits(this.Body.Position - this._wheelBody.Position);
            this._texturePosition = ConvertUnits.ToDisplayUnits(this.WheelBody.Position) + (tempPosition);

            HandleAnimation(delta);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CurrentAnimation.CurrentAnimationTexture,
            this._texturePosition, CurrentAnimation.CurrentRect, Color.White,
            this._mainBody.Rotation, CurrentAnimation.FrameOrigin, 0.43f, _lookingDirection, 0.3f);
        } 

        #region Animations


        /// <summary>
        /// Adds required aniations to the dict.
        /// </summary>
        protected virtual void AddAnimations()
        {
            _content.Unload();

            string spriteSheetLocation = "Assets/Images/Spritesheets/";

            Texture2D running = _content.Load<Texture2D>(spriteSheetLocation + "Running-Sheet");
            Texture2D idle = _content.Load<Texture2D>(spriteSheetLocation + "Idle1-Sheet");
            Texture2D falling = _content.Load<Texture2D>(spriteSheetLocation + "Falling-Sheet");
            Texture2D jumping = _content.Load<Texture2D>(spriteSheetLocation + "New-Jump-SHEET-");
            Texture2D climbing = _content.Load<Texture2D>(spriteSheetLocation + "Ladder-Sheet");
            Texture2D death = _content.Load<Texture2D>(spriteSheetLocation + "DeathSpriteSheet");

            _currentAnimation = "Idle";
            _animations.Add("Run",       new FrameAnimation(running, 24, new Point(322, 443), 9.0f, new Point(6, 4), false, 30));
            _animations.Add("Idle", new FrameAnimation(idle, 20, new Point(242, 454), 10.0f, new Point(5, 4), false, 16));
            _animations.Add("Falling", new FrameAnimation(falling, 20, new Point(275, 472), 17.0f, new Point(5, 4), false));
            _animations.Add("Jumping", new FrameAnimation(jumping, 21, new Point(262, 473), 42.0f, new Point(4, 6), true));
            _animations.Add("Climbing", new FrameAnimation(climbing, 20, new Point(207, 462), 10.0f, new Point(5, 4), false, 24));
            _animations.Add("Dead", new FrameAnimation(death, 21, new Point(550, 458), 20.0f, new Point(4, 5), true, 40));
        }


        /// <summary>
        /// Changeds animation dependant on PlayerState.
        /// </summary>
        protected virtual void HandleAnimation(float delta)
        {
            string oldAnimation = CurrentAnimationName;

            #region Handle Animation States
            //  Change the animation depending on the player
            //  state.
            if (PlayerState == PlayerState.Grounded)
            {
                CurrentAnimationName = "Idle";
            }
            else if (PlayerState == PlayerState.Running)
            {
                CurrentAnimationName = "Run";
            }
            else if (PlayerState == PlayerState.Falling)
            {
                CurrentAnimationName = "Falling";
            }
            else if (PlayerState == PlayerState.Jumping)
            {
                CurrentAnimationName = "Jumping";
            }
            else if (PlayerState == PlayerState.Climbing)
            {
                CurrentAnimationName = "Climbing";
            }
            else if (PlayerState == PlayerState.Dead)
            {
                CurrentAnimationName = "Dead";
            }
            #endregion

            //  If the animation has changed, reset its current frame
            if (oldAnimation != CurrentAnimationName)
            {
                CurrentAnimation.ResetCurrentFrame();
            }

            //  Play through the animation
            if (CurrentAnimation != null)
            {
                CurrentAnimation.Update(delta);
            }
        }


        #endregion

        protected void SetUpPhysics(World world)
        {
            float height = _charHeight * 0.6f;
            float mass = ConvertUnits.ToSimUnits(_charMass);

            //  Body
            this._mainBody = new Body(world);
            Fixture mainBodyFixture = FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(CharWidth * 0.4f), ConvertUnits.ToSimUnits(height), 8, 0.0f, _mainBody);
            this._mainBody.BodyType = BodyType.Dynamic;
            this._mainBody.Position = ConvertUnits.ToSimUnits(StartPosition);
            this._mainBody.Restitution = 0.0f;
            this._mainBody.Friction = 0.0f;
            
            //  Wheel
            float MotorPivot = (height) - 8.0f;
            this._wheelBody = BodyFactory.CreateBody(world);
            this._wheelBody.Position = ConvertUnits.ToSimUnits(StartPosition + new Vector2(0, MotorPivot));
            this._wheelBody.BodyType = BodyType.Dynamic;
            CircleShape circle1 = new CircleShape(ConvertUnits.ToSimUnits(28), mass);
            this._wheelBody.CreateFixture(circle1);
            this._wheelBody.Restitution = 0.0f;
            this._wheelBody.Friction = 3.0f;

            //  Motor
            this._wheelJoint = new RevoluteJoint(Body, WheelBody, ConvertUnits.ToSimUnits(Vector2.UnitY * MotorPivot), Vector2.Zero);
            this._wheelJoint.MotorEnabled = true;
            this._wheelJoint.MotorSpeed = 0f;
            this._wheelJoint.MaxMotorTorque = 10000f;
            world.AddJoint(_wheelJoint);

            //  Settings
            this._wheelBody.IgnoreCollisionWith(Body);
            this._mainBody.IgnoreCollisionWith(WheelBody);

            this._wheelBody.SleepingAllowed = false;
            this._mainBody.SleepingAllowed = false;
        }
    }
}
