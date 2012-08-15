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
using GameLibrary.Graphics;
#endregion

namespace GameLibrary.GameLogic.Objects
{
    public class RotatingPlatform : StaticObject
    {
        #region Fields

        [ContentSerializer(Optional = true)]
        private bool _rotatesWithLevel;
        [ContentSerializer(Optional = true)]
        private float _motorSpeed;
        [ContentSerializer(Optional = true)]
        private bool _motorEnabled;
        [ContentSerializer(Optional = true)]
        private bool _useShape = true;
        [ContentSerializer(Optional = true)]
        private ObjectShape _shapeType = ObjectShape.Quadrilateral;

#if EDITOR

#else
        private FixedAngleJoint _angleJoint;
        private FixedRevoluteJoint _revoluteJoint;
        private float _targetRotation;
#endif

        #endregion

        #region Properties
#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public bool UseBasicShape
        {
            get
            {
                return _useShape;
            }
            set
            {
                _useShape = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public ObjectShape ShapeType
        {
            get
            {
                return _shapeType;
            }
            set
            {
                _shapeType = value;
            }
        }
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
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public override bool Enabled
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
                return _rotation - Camera.Instance.Rotation;
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
            this._motorEnabled = true;
            this._useShape = true;
            this._motorSpeed = 5;
            this._shapeType = ObjectShape.Quadrilateral;
        }
        #endregion

        #region Update
        public override void Update(float delta)
        {
#if !EDITOR
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
            sb.Draw(_texture, _position, new Rectangle(0, 0, (int)this._width, (int)this._height), _tint, _rotation, new Vector2(this._width, this._height) * 0.5f, 1.0f, SpriteEffects.None, this._zLayer);
            sb.Draw(_texture, _position, new Rectangle(0, 0, (int)this._width, (int)this._height), _tint * 0.4f, _rotation + MathHelper.Pi, new Vector2(this._width, this._height) * 0.5f, 1.0f, SpriteEffects.None, this._zLayer + 0.01f);
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            sb.Draw(this._texture, ConvertUnits.ToDisplayUnits(this.Body.Position), null, this._tint, this.Body.Rotation, this._origin, 1.0f, SpriteEffects.None, this.zLayer);
        }
#endif
        #endregion

        #region Events
#if EDITOR
#else

        public override void Toggle()
        {

            if (this._revoluteJoint.MotorSpeed == 0.0f)
            {
                this._revoluteJoint.MotorSpeed = this._motorSpeed;
            }
            else
            {
                this._revoluteJoint.MotorSpeed = 0.0f;
            }

        }

        public override void Start()
        {
            this._revoluteJoint.MotorSpeed = this._motorSpeed;
        }

        public override void Stop()
        {
            this._revoluteJoint.MotorSpeed = 0.0f;
        }

        public override void Change(object sent)
        {
            //  Along with changing the objects motor speed, if the object
            //  is enabled, we'll also need to change it directly on the joint
            //  as it's not referenced.

            if (sent is int)
            {
                if (_motorSpeed < 0)
                {
                    this._motorSpeed = -(int)sent;
                }
                else
                {
                    this._motorSpeed = (int)sent;
                }

                if (_enabled)
                {
                    this._revoluteJoint.MotorSpeed = _motorSpeed;
                }
            }
        }

#endif
        #endregion

        #region Private Methods

        protected override void SetupPhysics(World world)
        {
#if !EDITOR
            Vector2 simPosition = ConvertUnits.ToSimUnits(this._position);

            if (_useShape)
            {
                float simHeight = ConvertUnits.ToSimUnits(this._height);
                float simWidth = ConvertUnits.ToSimUnits(this._width);
                this.Body = new Body(world);
                this.Body.Position = simPosition;

                this._origin = new Vector2(this._texture.Width, this._texture.Height) * 0.5f;

                switch (_shapeType)
                {
                    case ObjectShape.Quadrilateral:
                        {
                            Fixture fixture = FixtureFactory.AttachRectangle(simWidth, simHeight, _mass, Vector2.Zero, Body);
                            break;
                        }
                    case ObjectShape.Circle:
                        {
                            Fixture fixture = FixtureFactory.AttachCircle(simWidth * 0.5f, _mass, this.Body);
                            break;
                        }
                }

                this._revoluteJoint = JointFactory.CreateFixedRevoluteJoint(world, Body, Vector2.Zero, simPosition);
            }
            else
            {
                bool useCentroid = false;

                if (_rotatesWithLevel)
                {
                    useCentroid = true;
                }

                TexVertOutput input = SpinAssist.TexToVert(world, _texture, _mass, false);
                
                this._origin = Vector2.Zero;
                this.Body = input.Body;


                this.Body.Position = simPosition;
                if (useCentroid)
                {
                    this._revoluteJoint = JointFactory.CreateFixedRevoluteJoint(world, Body, this.Body.LocalCenter, simPosition);
                }
                else
                {
                    this._revoluteJoint = JointFactory.CreateFixedRevoluteJoint(world, Body, this.Body.LocalCenter, simPosition);
                }

                this._revoluteJoint.MaxMotorTorque = float.MaxValue;
                this._revoluteJoint.MotorEnabled = true;

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
                    this._revoluteJoint.MotorSpeed = _motorSpeed;
                }
                else
                {
                    this._revoluteJoint.MotorSpeed = 0.0f;
                }
            }
            this.Body.CollidesWith = Category.All & ~Category.Cat20;
            this.Body.CollisionCategories = Category.Cat20;

            this.Body.Friction = 3.0f;
#endif
        }
        #endregion
    }
}
