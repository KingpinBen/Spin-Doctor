//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - RotatingPlatform
//--    
//--    
//--    Description
//--    ===============
//--    Dynamically rotating platform
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Rewrote. Fixed most issues.
//--    
//--    
//--    TBD
//--    ==============
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using GameLibrary.Drawing;
using GameLibrary.Assists;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace GameLibrary.Objects
{
    public class RotatingPlatform : PhysicsObject
    {
        #region Fields

        private FixedRevoluteJoint revoluteJoint;
        [ContentSerializer]
        private bool _rotatesWithLevel;

        #endregion

        #region Properties
        [ContentSerializerIgnore]
        public bool RotatesWithLevel
        {
            get
            {
                return _rotatesWithLevel;
            }

#if EDITOR
            set
#else
            protected set
#endif
            {
                _rotatesWithLevel = value;
            }
        }

        #endregion

        #region Constructor
        public RotatingPlatform() : base() { }

        public override void Init(Vector2 position, string tex)
        {
            base.Init(position, tex);

            this._rotatesWithLevel = true;
        }
        #endregion

        #region LoadContent
        /// <summary>
        /// Loads game content.
        /// </summary>
        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(this._textureAsset);
            this.Width = this._texture.Width;
            this.Height = this._texture.Height;
            GetRotationFromOrientation();
#if EDITOR

#else
            SetUpPhysics(world);
#endif
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
#if EDITOR
#else
            if (!_rotatesWithLevel)
            {
                return;
            }

            //  Limit it so it can only be at 1 angle, the level rotation
            this.Body.Rotation = -(float)Camera.Rotation;
#endif
        }
        #endregion

#if EDITOR

#else
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this._texture, this._position, null, this._tint, TextureRotation, new Vector2(this._texture.Width / 2, this._texture.Height / 2), 1.0f, SpriteEffects.None, this.zLayer);
        }
#endif

        #region new methods

        #region SetUpPhysics
        protected override void SetUpPhysics(World world)
        {
            /*  Okay, this created some issues (there are still issues I know) but lemme tell you whats going down.
             *  The bodytype being static seems to be the only way to get it working the way it should. Using the joint limits
             *  to rotate the body caused wheel disjointment from the player or just caused the player to drop straight through the
             *  platform body. This actually just happened when the platform body was dynamic either way but static fixed it.
             *  
             *  it was probably being caused due to the level rotation speed and farseer not being able to apply the physics 
             *  buffer in time.
             *  
             * At the moment, the main issue is that the player moves towards the edges of the platform when it rotates. This 
             * unfortunately may need redoing again.
             */

#if EDITOR
#else

            TexVertOutput input = SpinAssist.TexToVert(world, this._texture, ConvertUnits.ToSimUnits(this._mass));

            this.Origin = input.Origin;

            this.Body = input.Body;
            this.Body.Position = ConvertUnits.ToSimUnits(this.Position);
            this.Body.BodyType = BodyType.Static;
            this.Body.Restitution = 0.0f;
            this.Body.Friction = 3.0f;

            this.revoluteJoint = JointFactory.CreateFixedRevoluteJoint(world, this.Body, ConvertUnits.ToSimUnits(Vector2.Zero), ConvertUnits.ToSimUnits(this.Position));
            this.revoluteJoint.MaxMotorTorque = float.MaxValue;
#endif
        }
        #endregion

        #region Change RotatewithLevel
        /// <summary>
        /// Toggles it off it on, or on if off.
        /// </summary>
        public void ToggleRotateWithLevel()
        {
            if (_rotatesWithLevel)
            {
                ToggleRotateWithLevel(false);
            }
            else
            {
                ToggleRotateWithLevel(true);
            }
        }

        public void ToggleRotateWithLevel(bool set)
        {
            _rotatesWithLevel = set;
        }
        #endregion

        #endregion
    }
}
