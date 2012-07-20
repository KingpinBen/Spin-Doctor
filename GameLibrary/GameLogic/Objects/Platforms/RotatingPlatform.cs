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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;
using GameLibrary.Helpers;
using GameLibrary.Graphics.Camera;
#endregion

namespace GameLibrary.GameLogic.Objects
{
    public class RotatingPlatform : PhysicsObject
    {
        #region Fields
#if EDITOR

#else
        private FixedRevoluteJoint revoluteJoint;
        private float _targetRotation;
#endif
        [ContentSerializer]
        private bool _rotatesWithLevel;
        [ContentSerializer]
        private float _motorSpeed;
        [ContentSerializer(Optional = true)]
        private bool _motorEnabled;

        #endregion

        #region Properties
#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public float MotorSpeed
        {
            get
            {
                return _motorSpeed;
            } 
            set
            {
                _motorSpeed = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public bool RotatesWithLevel
        {
            get
            {
                return _rotatesWithLevel;
            }
            set
            {
                _rotatesWithLevel = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Hidden")]
        public override bool UseBodyRotation
        {
            get
            {
                return base.UseBodyRotation;
            }
            set
            {
                
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public bool Enabled
        {
            get
            {
                return _motorEnabled;
            }
            set
            {
                _motorEnabled = value;
            }
        }
#else
        private float TargetRotation
        {
            get
            {
                return -(float)Camera.Instance.Rotation;
            }
        }
#endif


        #endregion

        #region Constructor
        public RotatingPlatform() : base() { }

        public override void Init(Vector2 position, string tex)
        {
            base.Init(position, tex);

            this._rotatesWithLevel = true;
            this._useBodyRotation = true;
            this._motorEnabled = true;
        }
        #endregion

        #region LoadContent
        /// <summary>
        /// Loads game content.
        /// </summary>
        public override void Load(ContentManager content, World world)
        {
            if (this._textureAsset != "")
                _texture = content.Load<Texture2D>(this._textureAsset);

            this._origin = new Vector2(_texture.Width * 0.5f, _texture.Height * 0.5f);

#if EDITOR
            if (this.Width == 0.0f || this.Height == 0.0f)
            {
                this.Width = this._texture.Width;
                this.Height = this._texture.Height;
            }
#else
            SetupPhysics(world);
#endif
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
#if EDITOR

#else
            if (_rotatesWithLevel && _motorEnabled)
            {
                if (this.Body.Rotation != TargetRotation)
                {
                    float amountToTurn = TargetRotation - this.Body.Rotation;

                    if ((amountToTurn > 0 && _motorSpeed < 0) ||
                        (amountToTurn < 0 && _motorSpeed > 0))
                    {
                        _motorSpeed *= -1;
                    }
                    
                    this.Body.Rotation += _motorSpeed;
                    amountToTurn -= _motorSpeed;

                    if (Math.Abs(amountToTurn) <= Math.Abs(_motorSpeed))
                    {
                        this.Body.Rotation += amountToTurn;
                    }
                }
            }

            //  Limit it so it can only be at 1 angle, the level rotation
            //this.Body.Rotation = -(float)Camera.Rotation;
#endif
        }
        #endregion

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_texture, _position, null, _tint, 0.0f, _origin, 1.0f, SpriteEffects.None, this._zLayer);
            sb.Draw(_texture, _position, null, _tint * 0.4f, MathHelper.PiOver2, _origin, 1.0f, SpriteEffects.None, this._zLayer);
        }
#else
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this._texture, this._position, null, this._tint, this.TextureRotation, this._origin, 1.0f, SpriteEffects.None, this.zLayer);
        }
#endif
        #endregion

        #region Private Methods

        #region SetUpPhysics
        protected override void SetupPhysics(World world)
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
            bool useCentroid = false;

            if (_rotatesWithLevel)
            {
                useCentroid = true;
            }

            TexVertOutput input = SpinAssist.TexToVert(world, this._texture, ConvertUnits.ToSimUnits(this._mass), useCentroid);
            Vector2 simPosition = ConvertUnits.ToSimUnits(this._position);

            if (_rotatesWithLevel)
            {
                this._origin = input.Origin;
            }
            else
            {
                this.Origin = new Vector2(this._texture.Width, this._texture.Height) * 0.5f;
            }

            this.Body = input.Body;
            this.Body.Position = simPosition;
            if (useCentroid)
            {
                this.revoluteJoint = JointFactory.CreateFixedRevoluteJoint(world, this.Body, Vector2.Zero, simPosition);
            }
            else
            {
                this.revoluteJoint = JointFactory.CreateFixedRevoluteJoint(world, this.Body, this.Body.LocalCenter, simPosition);
            }
            
            this.revoluteJoint.MaxMotorTorque = float.MaxValue;
            this.revoluteJoint.MotorEnabled = true;

            if (!_rotatesWithLevel)
            {
                this.Body.BodyType = BodyType.Dynamic;
            }
            else
            {
                this.Body.BodyType = BodyType.Dynamic;
                this.Body.Rotation = this._rotation;
                float newSpeed = 1 / _motorSpeed;
                this._motorSpeed = newSpeed;
            }

            if (this._motorEnabled)
            {
                this.revoluteJoint.MotorSpeed = _motorSpeed;
            }
            else
            {
                this.revoluteJoint.MotorSpeed = 0.0f;
            }

            this.Body.CollidesWith = Category.All & ~Category.Cat20;
            this.Body.CollisionCategories = Category.Cat20;
            
            //this.Body.IgnoreGravity = true;
            //this.Body.IgnoreCCD = true;
            this.Body.Restitution = 0.0f;
            this.Body.Friction = 3.0f;
#endif
        }
        #endregion

        #endregion
    }
}
