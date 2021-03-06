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
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework.Audio;
using GameLibrary.Audio;

namespace GameLibrary.GameLogic.Objects
{
    public abstract class DynamicObject : StaticObject
    {
        #region Fields
        [ContentSerializer(Optional = true)]
        protected Vector2 _endPosition;
        [ContentSerializer(Optional = true)]
        protected float _motorSpeed = 3.0f;
        [ContentSerializer(Optional = true)]
        protected Direction _movementDirection = Direction.Horizontal;
        [ContentSerializer(Optional = true)]
        protected bool _startsMoving = true;
        [ContentSerializer(Optional = true)]
        protected float _timeToReverse = 1.0f;
        [ContentSerializer(Optional = true)]
        protected float _startTranslation;


#if EDITOR

#else
        protected FixedPrismaticJoint _prismaticJoint;
        protected float _elapsedTimer = 0.0f;
        protected bool _isMoving = false;
        private bool _currentMovingDirection = false;
        protected Cue _soundEffect;
        protected string _soundEffectAsset = String.Empty;
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
        public bool MovingToStart
        {
            get
            {
                return _currentMovingDirection;
            }
        }
#endif
        #endregion

        #region Constructor


        public DynamicObject() : base() { }


        public override void Init(Vector2 position, string tex)
        {
            base.Init(position, tex);
            this._endPosition = position + new Vector2(200, 0);
        }


        #endregion

        public override void Update(float delta)
        {
#if !EDITOR
            if (_isMoving)
            {
                if ((_prismaticJoint.JointTranslation >= _prismaticJoint.UpperLimit && !this.MovingToStart) ||
                        (_prismaticJoint.JointTranslation <= _prismaticJoint.LowerLimit && this.MovingToStart) ||
                        _elapsedTimer > 0.0f)
                {

                    //  If so zero the speed. Fixes bouncing errors in farseer.
                    if (this._prismaticJoint.MotorSpeed != 0)
                    {
                        this._prismaticJoint.MotorSpeed = 0.0f;

                        if (_soundEffect != null && !_soundEffect.IsStopped)
                        {
                            _soundEffect.Stop(AudioStopOptions.AsAuthored);
                        }
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
                        this.Start();

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
                this._isMoving = true;
                this._prismaticJoint.MotorSpeed = _motorSpeed;
            }
#endif
        }
        #endregion

        protected override void SetupPhysics(World world) { }

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
            //  Joint needs to stay enabled, otherwise it just flops
            //  and we lose the translation.
            this._prismaticJoint.MotorSpeed = _motorSpeed;
            this._isMoving = true;

            if (_soundEffect != null && _soundEffectAsset != "")
            {
                if (!_soundEffect.IsPlaying)
                {
                    _soundEffect = AudioManager.Instance.PlayCue(_soundEffectAsset, true);
                }
            }
        }

        public override void Stop()
        {
            this._prismaticJoint.MotorSpeed = 0.0f;
            this._isMoving = false;

            if (_soundEffect != null && !_soundEffect.IsStopped)
            {
                _soundEffect.Stop(AudioStopOptions.AsAuthored);
            }
        }

        public override void Change(object sent)
        {
            //  Along with changing the objects motor speed, if the object
            //  is enabled, we'll also need to change it directly on the joint
            //  as it's not referenced.

            if (sent is float)
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
