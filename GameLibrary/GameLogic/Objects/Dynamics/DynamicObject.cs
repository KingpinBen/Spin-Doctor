//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Dynamic Object
//--    
//--    Description
//--    ===============
//--    For use with objects using prismatic joints and automatic dynamic movement
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Fixed the update code to better correct for translation differences.
//--    
//--    TBD
//--    ==============
//--    
//--    
//--------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using GameLibrary.Helpers;

namespace GameLibrary.GameLogic.Objects
{
    public abstract class DynamicObject : PhysicsObject
    {
        #region Fields
        [ContentSerializer]
        protected Vector2 _endPosition;
        [ContentSerializer]
        protected float _motorSpeed;
        [ContentSerializer]
        protected Direction _movementDirection;
        [ContentSerializer]
        protected bool _startsMoving;
        [ContentSerializer]
        protected float _timeToReverse;
        [ContentSerializer(Optional = true)]
        protected float _startTranslation;


#if EDITOR

#else
        [ContentSerializerIgnore]
        protected FixedPrismaticJoint _prismaticJoint;
        [ContentSerializerIgnore]
        protected float _elapsedTimer;
        [ContentSerializerIgnore]
        protected bool _isMoving;
        [ContentSerializerIgnore]
        private bool _currentMovingDirection;
#endif
        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public virtual Direction MovementDirection
        {
            get
            {
                return _movementDirection;
            }
            set
            {
                _movementDirection = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public virtual float MotorSpeed
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
        public virtual Vector2 EndPosition
        {
            get
            {
                return _endPosition;
            }
            set
            {
                _endPosition = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public virtual bool StartsMoving
        {
            get
            {
                return _startsMoving;
            }
            set
            {
                _startsMoving = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public virtual float TimeToReverse
        {
            get 
            { 
                return _timeToReverse; 
            }
            set
            {
                _timeToReverse = value;
            }
        }
#else
        [ContentSerializerIgnore]
        public FixedPrismaticJoint PrismaticJoint
        {
            get
            {
                return _prismaticJoint;
            }
            protected set

            {
                _prismaticJoint = value;
            }
        }
        [ContentSerializerIgnore]
        public bool MovingToStart
        {
            get
            {
                return _currentMovingDirection;
            }
        }
        [ContentSerializerIgnore]
        public virtual Direction MovementDirection
        {
            get
            {
                return _movementDirection;
            }
            protected set
            {
                _movementDirection = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual float MotorSpeed
        {
            get
            {
                return _motorSpeed;
            }
            protected set
            {
                _motorSpeed = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual Vector2 EndPosition
        {
            get
            {
                return _endPosition;
            }
            protected set
            {
                _endPosition = value;
            }
        }
        [ContentSerializerIgnore]
        public virtual bool StartsMoving
        {
            get
            {
                return _startsMoving;
            }
            protected set
            {
                _startsMoving = value;
            }
        }
        [ContentSerializerIgnore]
        public float TimeToReverse
        {
            get
            {
                return _timeToReverse;
            }
            protected set
            {
                _timeToReverse = value;
            }
        }
#endif
        #endregion

        #region Constructor and Initialization

        public DynamicObject()
            : base()
        {
            
        }

        public override void Init(Vector2 position, string tex)
        {
            base.Init(position, tex);

            this._movementDirection = Direction.Horizontal;
            this._mass = 100.0f;
            this._motorSpeed = 3.0f;
            this._timeToReverse = 1.0f;
            this._startsMoving = true;
            this._endPosition = position + new Vector2(200, 0);
        }

        #endregion

        public override void Load(ContentManager content, World world)
        {
            if (this._textureAsset != "")
                _texture = content.Load<Texture2D>(this._textureAsset);

            this._origin = new Vector2(_texture.Width, _texture.Height) * 0.5f;

#if EDITOR
            if (_width == 0 || _height == 0)
            {
                this.Width = this._texture.Width;
                this.Height = this._texture.Height;
            }
#else
            this._elapsedTimer = 0.0f;
            this._currentMovingDirection = false;
            this.RegisterObject();


            if (_startsMoving)
            {
                _isMoving = true;
            }
#endif
        }

        public override void Update(float delta)
        {
#if EDITOR

#else
            if (_isMoving)
            {
                if ((this.PrismaticJoint.JointTranslation >= this.PrismaticJoint.UpperLimit && !this.MovingToStart) ||
                    (this.PrismaticJoint.JointTranslation <= this.PrismaticJoint.LowerLimit && this.MovingToStart) ||
                    _elapsedTimer > 0.0f)
                {

                    //  If so zero the speed. Fixes bouncing errors in farseer.
                    if (this._prismaticJoint.MotorSpeed != 0)
                    {
                        this._prismaticJoint.MotorSpeed = 0.0f;
                    }

                    //  Update stationary timers
                    this._elapsedTimer += delta;

                    //  Check if the stationary time exceeds the pause time
                    if (_elapsedTimer >= _timeToReverse)
                    {
                        //  Swap direction
                        this._currentMovingDirection = !this._currentMovingDirection;

                        //  Reverse the motor speed and apply to the motor.
                        this._motorSpeed = -this._motorSpeed;
                        this._prismaticJoint.MotorSpeed = this._motorSpeed;

                        //  Zero timer for next go around
                        this._elapsedTimer = 0.0f;
                    }
                }
            }
#endif
        }

        #region Private Methods

        #region Setup Joint
        /// <summary>
        /// Call during the method in which you create the body for the object.
        /// Make sure it's used afterwards otherwise you'll have issues.
        /// </summary>
        /// <param name="world"></param>
        protected virtual void SetUpJoint(World world)
        {
#if EDITOR

#else
            this._currentMovingDirection = false;
            Vector2 axis = Vector2.Zero;

            if (_movementDirection == Direction.Horizontal)
            {
                if (_endPosition.X < _position.X)
                {
                    axis = new Vector2(-1, 0);
                }
                else
                {
                    axis = new Vector2(1, 0);
                }
            }
            else
            {
                if (_endPosition.Y < _position.Y)
                {
                    axis = new Vector2(0, -1);
                }
                else
                {
                    axis = new Vector2(0, 1);
                }
            }

            this._prismaticJoint = JointFactory.CreateFixedPrismaticJoint(world, this.Body, ConvertUnits.ToSimUnits(this.Position), axis);

            if (_movementDirection == Direction.Horizontal)
            {
                this._prismaticJoint.UpperLimit = Math.Abs(ConvertUnits.ToSimUnits(_endPosition.X - Position.X));
            }
            else
            {
                this._prismaticJoint.UpperLimit = Math.Abs(ConvertUnits.ToSimUnits(_endPosition.Y - Position.Y));
            }

            this._prismaticJoint.LowerLimit = 0.0f;
            this._prismaticJoint.LimitEnabled = true;
            this._prismaticJoint.MotorEnabled = true;
            this._prismaticJoint.MaxMotorForce = float.MaxValue;

            if (_startsMoving)  
            {
                this._prismaticJoint.MotorSpeed = this.MotorSpeed;
            }
#endif
        }
        #endregion



        #endregion

        #region Events

#if !EDITOR
        public override void Toggle()
        {
            if (_isMoving)
            {
                this.Stop();
            }
            else
            {
                this.Start();
            }
        }

        public override void Start()
        {
            this._prismaticJoint.MotorSpeed = _motorSpeed;
            this._isMoving = true;
        }

        public override void Stop()
        {
            this._prismaticJoint.MotorSpeed = 0.0f;
            this._isMoving = false;
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
                    this._prismaticJoint.MotorSpeed = this._motorSpeed;
                }
            }
        }

        
#endif
        #endregion

    }
}
