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
using GameLibrary.Screens;
using GameLibrary.Assists;
using GameLibrary.Managers;
using GameLibrary.Drawing;
#endregion

namespace GameLibrary.Objects
{
    #region Player State
    public enum pState
    {
        Running,
        Grounded,
        Jumping,
        Climbing,
        Swinging,
        Falling,
        Pulling,
        Dead
    }
    #endregion

    public class Character
    {
        #region Fields

        protected float charMass;
        protected float charWidth;
        protected float charHeight;
        protected Body wheelBody;
        protected Body mainBody;
        protected RevoluteJoint wheelJoint;
        protected Texture2D charTexture;
        protected pState playerState;
        protected ContentManager _content;
        protected List<Fixture> TouchingFixtures;
        private Vector2 TexturePosition;
        protected Dictionary<string, FrameAnimation> Animations;
        private string currentAnimation = null;
        protected SpriteEffects LookingDirection;
        #endregion

        #region Properties

        public pState PlayerState
        {
            get
            {
                return playerState;
            }
            protected set
            {
                playerState = value;
            }
        }

        protected RevoluteJoint WheelJoint
        {
            get
            {
                return wheelJoint;
            }
            set
            {
                wheelJoint = value;
            }
        }

        public Body WheelBody
        {
            get 
            { 
                return wheelBody; 
            }
        }

        public Body Body
        {
            get
            {
                return mainBody;
            }
        }

        public float TotalHeight
        {
            get 
            { 
                return (charHeight - 8.0f) + (charWidth / 2); 
            }
        }

        public float CharWidth
        {
            get
            {
                return charWidth;
            }
        }

        protected FrameAnimation CurrentAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(currentAnimation))
                    return Animations[currentAnimation];
                else
                    return null;
            }
        }

        public string CurrentAnimationName
        {
            get 
            { 
                return currentAnimation;
            }
            set
            {
                if (Animations.ContainsKey(value))
                    currentAnimation = value;
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
                    WheelBody.Position = new Vector2(this.WheelBody.Position.X, value.Y);
                    Body.Position = new Vector2(this.WheelBody.Position.X, value.Y);
                }
                else if (value.Y == 0)
                {
                    WheelBody.Position = new Vector2(value.X, this.WheelBody.Position.Y);
                    Body.Position = new Vector2(value.X, this.Body.Position.Y);
                }

                
            }
        }
        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Construct
        /// </summary>
        public Character()
        {
            _content = new ContentManager(Screen_Manager.Game.Services, "Content");
            Animations = new Dictionary<string, FrameAnimation>();
        }
        #endregion

        #region Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">Content manager</param>
        /// <param name="_world">the World</param>
        /// <param name="position">Starting Position</param>
        /// <param name="tex">Texture location</param>
        public virtual void Load(ContentManager content, World _world, Vector2 position)
        {
            this.charHeight = 128f;
            this.charWidth = charHeight;
            this.charMass = 60f;

            this.StartPosition = position;

            //  **Instantiate on load to get rid of any old fixtures.
            this.TouchingFixtures = new List<Fixture>();

            SetUpPhysics(_world);
        }
        #endregion

        #region Update
        public virtual void Update(GameTime gameTime)
        {
            if (this.mainBody.Enabled == false)
            {
                return;
            }

            Vector2 tempPosition = ConvertUnits.ToDisplayUnits(this.Body.Position - this.wheelBody.Position);
            this.TexturePosition = ConvertUnits.ToDisplayUnits(this.Body.Position) + (tempPosition * 0.25f);

            HandleAnimation(gameTime);
        }
        #endregion

        #region Draw
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            #region Development
#if Development
            //spriteBatch.Draw(CharTexture,
            //    new Rectangle((int)ConvertUnits.ToDisplayUnits(Body.Position.X), (int)ConvertUnits.ToDisplayUnits(Body.Position.Y), (int)CharWidth, (int)CharHeight),
            //    null, Color.White, Body.Rotation, Origin, SpriteEffects.None, 0.01f);

            ////Uncomment if you want to see the wheel. Only to show the works
            //spriteBatch.Draw(CharTexture,
            //    new Rectangle((int)ConvertUnits.ToDisplayUnits(WheelBody.Position.X), (int)ConvertUnits.ToDisplayUnits(WheelBody.Position.Y), (int)CharWidth, (int)CharWidth),
            //    null, new Color(150f, 150f, 255f, 0.3f), WheelBody.Rotation, Origin, SpriteEffects.None, 0f);

#endif
            #endregion

            spriteBatch.Draw(CurrentAnimation.CurrentAnimationTexture,
            this.TexturePosition, CurrentAnimation.CurrentRect, Color.White,
            this.mainBody.Rotation, CurrentAnimation.FrameOrigin, 0.43f, LookingDirection, 0.3f);
        } 
        #endregion

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

            currentAnimation = "Idle";
            Animations.Add("Run",       new FrameAnimation(running, 24, new Point(322, 443), 9.0f, new Point(6, 4), false, 30));
            Animations.Add("Idle", new FrameAnimation(idle, 20, new Point(242, 454), 10.0f, new Point(5, 4), false, 16));
            Animations.Add("Falling", new FrameAnimation(falling, 20, new Point(275, 472), 17.0f, new Point(5, 4), false));
            Animations.Add("Jumping",   new FrameAnimation(jumping, 21, new Point(262, 473), 42.0f, new Point(4, 6), true));
            Animations.Add("Climbing",  new FrameAnimation(climbing, 20, new Point(207, 462), 10.0f, new Point(5, 4), false, 24));
            Animations.Add("Dead", new FrameAnimation(death, 21, new Point(550, 458), 20.0f, new Point(4, 5), true, 40));
        }
        /// <summary>
        /// Changeds animation dependant on PlayerState.
        /// </summary>
        protected virtual void HandleAnimation(GameTime gameTime)
        {
            string oldAnimation = CurrentAnimationName;

            if (PlayerState == pState.Grounded)
                CurrentAnimationName = "Idle";
            else if (PlayerState == pState.Running)
                CurrentAnimationName = "Run";
            else if (PlayerState == pState.Falling)
                CurrentAnimationName = "Falling";
            else if (PlayerState == pState.Jumping)
                CurrentAnimationName = "Jumping";
            else if (PlayerState == pState.Climbing)
                CurrentAnimationName = "Climbing";
            else if (PlayerState == pState.Dead)
                CurrentAnimationName = "Dead";

            if (oldAnimation != CurrentAnimationName)
            {
                CurrentAnimation.ResetCurrentFrame();
            }

            if (CurrentAnimation != null)
                CurrentAnimation.Update(gameTime);
        }
        #endregion

        #region SetupPhysics
        /// <summary>
        /// Creates the physics and body of a moveable character.
        /// </summary>
        /// <param name="world">World</param>
        /// <param name="mass">Mass of character</param>
        protected void SetUpPhysics(World world)
        {
            //  Body
            this.mainBody = new Body(world);
            Fixture mainBodyFixture = FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(CharWidth * 0.5f), 0.0f, mainBody);
            //this.mainBody = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(CharWidth * 0.4f), 0);
            this.mainBody.BodyType = BodyType.Dynamic;
            this.mainBody.Position = ConvertUnits.ToSimUnits(StartPosition);
            this.mainBody.Restitution = 0f;
            this.mainBody.Friction = 0.0f;
            
            //  Wheel
            float MotorPivot = (charHeight * 0.5f) - 8.0f;
            this.wheelBody = BodyFactory.CreateBody(world);
            this.wheelBody.Position = ConvertUnits.ToSimUnits(StartPosition + new Vector2(0, MotorPivot));
            this.wheelBody.BodyType = BodyType.Dynamic;
            CircleShape circle1 = new CircleShape(ConvertUnits.ToSimUnits(28), ConvertUnits.ToSimUnits(charMass));
            this.wheelBody.CreateFixture(circle1);
            this.wheelBody.Restitution = 0.0f;
            this.wheelBody.Friction = 3.0f;

            //  Motor
            this.wheelJoint = new RevoluteJoint(Body, WheelBody, ConvertUnits.ToSimUnits(Vector2.UnitY * MotorPivot), Vector2.Zero);
            this.wheelJoint.MotorEnabled = true;
            this.wheelJoint.MotorSpeed = 0f;
            this.wheelJoint.MaxMotorTorque = float.MaxValue;
            world.AddJoint(wheelJoint);

            //  Settings
            this.wheelBody.IgnoreCollisionWith(Body);
            this.mainBody.IgnoreCollisionWith(WheelBody);

            this.wheelBody.SleepingAllowed = false;
            this.mainBody.SleepingAllowed = false;
        }
        #endregion
    }
}
