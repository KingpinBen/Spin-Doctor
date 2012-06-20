﻿//--------------------------------------------------------------------------
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
using GameLibrary.Assists;

namespace GameLibrary.Objects
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

        [ContentSerializerIgnore]
        protected FixedPrismaticJoint _prismaticJoint;
        [ContentSerializerIgnore]
        protected float _elapsedTimer;
        [ContentSerializerIgnore]
        protected bool _isMoving;
        [ContentSerializerIgnore]
        private bool _currentMovingDirection;
        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore]
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
        [ContentSerializerIgnore]
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
        [ContentSerializerIgnore]
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
        [ContentSerializerIgnore]
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
        [ContentSerializerIgnore]
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

        #region Constructor
        public DynamicObject()
            : base()
        {
            
        }
        #endregion

        #region Load
        /// <summary>
        /// Must be called to create the objects joint.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="world"></param>
        public override void Load(ContentManager content, World world)
        {
            this._elapsedTimer = 0.0f;
            this._currentMovingDirection = false;

            if (_startsMoving)
            {
                _isMoving = true;
            }
        }
        #endregion

        
#if !EDITOR
        #region Update
        public override void Update(GameTime gameTime)
        {
            if (!_isMoving) return;

            if  ((this.PrismaticJoint.JointTranslation >= this.PrismaticJoint.UpperLimit && !this.MovingToStart) ||
                (this.PrismaticJoint.JointTranslation <= this.PrismaticJoint.LowerLimit && this.MovingToStart) ||
                _elapsedTimer > 0.0f)
            {
                //  If so zero the speed. Fixes bouncing errors in farseer.
                if (this._prismaticJoint.MotorSpeed != 0)
                {
                    this._prismaticJoint.MotorSpeed = 0.0f;
                }

                //  Update stationary timers
                this._elapsedTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

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
        #endregion
#endif

        #region Setup Joint
        /// <summary>
        /// Call during the method in which you create the body for the object.
        /// Make sure it's used afterwards otherwise you'll have issues.
        /// </summary>
        /// <param name="world"></param>
        protected void SetUpJoint(World world)
        {
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
            else if (_movementDirection == Direction.Vertical)
            {
                if (_endPosition.Y < _position.Y)
                {
                    axis = new Vector2(0, 1);
                }
                else
                {
                    axis = new Vector2(0, -1);
                }
            }

            this._prismaticJoint = JointFactory.CreateFixedPrismaticJoint(world, this.Body, ConvertUnits.ToSimUnits(this.Position), axis);

            if (_movementDirection == Direction.Horizontal)
            {
                this._prismaticJoint.UpperLimit = ConvertUnits.ToSimUnits(_endPosition.X - Position.X);

                if (axis.X == -1)
                {
                    this._prismaticJoint.UpperLimit *= -1;
                }
            }
            else if (_movementDirection == Direction.Vertical)
            {
                this._prismaticJoint.UpperLimit = ConvertUnits.ToSimUnits(_endPosition.Y - Position.Y);

                if (axis.Y == 1)
                {
                    this._prismaticJoint.UpperLimit *= -1;
                }
            }
            else 
                throw new Exception("Invalid direction in serializing on dynamicObject");

            this._prismaticJoint.LowerLimit = 0.0f;
            this._prismaticJoint.LimitEnabled = true;
            this._prismaticJoint.MotorEnabled = true;
            this._prismaticJoint.MaxMotorForce = float.MaxValue;
            this._prismaticJoint.MotorSpeed = this.MotorSpeed;
        }
        #endregion

    }
}
