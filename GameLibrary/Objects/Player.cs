//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Character
//--    
//--    
//--    
//--    Description
//--    ===============
//--    Handles player character and controls.
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Introduced jumping and controls
//--    BenG - Changed how jumping works. Now with gravity
//--    BenG - Introduced player state and collision
//--    BenG - Changed how jumping works again.
//--    BenG - Fixed playerstates, started experimenting with sprites
//--           when the player jumps and added controller vibration.
//--    BenG - BUG: Fixed running and looking directions.
//--    BenG - Mid air jumping near done. Speed needs capping. Timer set for
//--           max air jump time.
//--    
//--    TBD
//--    ==============
//--    
//--
//--------------------------------------------------------------------------

#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics.Dynamics;

using GameLibrary.Assists;
using GameLibrary.Drawing;
using GameLibrary.Screens;
using System.Xml;
using FarseerPhysics.Controllers;
using GameLibrary.Managers;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

#endregion

namespace GameLibrary.Objects
{
    public sealed class Player : Character
    {
        #region Fields

        private static readonly Player playerInstance = new Player();
        private const float _movementSpeed = 22.0f;
        private const float _jumpForce = 4.2f;      // 3.2f - Waist height. Pre 24/5
        private const float _maxJumpTime = 0.54f;
        private const float _maxAirTime = 0.84f;
        private const float _midAirForce = 200.0f;
        private bool _canJump;
        private bool _canDoubleJump;
        private bool _dJumpEnabled;
        private float _grabbingRotation;
        private float _airTime;
        private WeldJoint grabbingJoint;
        
        #endregion

        #region Properties

        public bool CanJump
        {
            get
            {
                return _canJump;
            }
            internal set
            {
                _canJump = value;
            }
        }

        public bool CanDoubleJump
        {
            get
            {
                return _canDoubleJump;
            }
            internal set
            {
                _canDoubleJump = value;
            }
        }

        public bool DoubleJumpEnabled
        {
            get
            {
                return _dJumpEnabled;
            }
            set
            {
                _dJumpEnabled = value;
            }
        }

        public float GrabRotation
        {
            get
            {
                return _grabbingRotation;
            }
            set
            {
                _grabbingRotation = value;
            }
        }

        public static Player Instance
        {
            get { return playerInstance; }
        }

        public float AirTime
        {
            get { return _airTime; }
        }

        #endregion

        #region Constructor
        private Player()
            : base()
        {
            
        }
        #endregion

        #region Load
        public void Load(ContentManager content, World world, Vector2 position, bool enableDoubJump)
        {
            base.Load(content, world, position);
            SetupPlayerSettings(enableDoubJump);

            if (Animations.Count == 0) AddAnimations();
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (PlayerState == pState.Dead)
            {
                return;
            }

            //Keeps the body rotation up, moving with the camera.
            if (Camera.LevelRotating || (mainBody.Rotation != (float)-Camera.Rotation && PlayerState != pState.Swinging))
            {
                mainBody.Rotation = (float)-Camera.Rotation;
            }

            if (!mainBody.Enabled && PlayerState != pState.Climbing)
            {
                ToggleBodies(true);
            }

            if (Input.LeftCheck())
            {
                this.WheelJoint.MotorSpeed = -_movementSpeed;
                LookingDirection = SpriteEffects.FlipHorizontally;
            }
            else if (Input.RightCheck())
            {
                this.WheelJoint.MotorSpeed = _movementSpeed;
                LookingDirection = SpriteEffects.None;
            }
            else
            {
                this.WheelJoint.MotorSpeed = 0.0f;
            }


            #region How to handle each state
            switch (PlayerState)
            {
                case pState.Climbing:
                    {
                        HandleClimbing(gameTime);
                        break;
                    }
                case pState.Falling:
                    {
                        HandleAir(gameTime);
                        break;
                    }
                case pState.Jumping:
                    {
                        HandleAir(gameTime);
                        break;
                    }
                case pState.Grounded:
                    {
                        HandleMoving(gameTime);
                        break;
                    }
                case pState.Running:
                    {
                        HandleMoving(gameTime);
                        break;
                    }
                case pState.Pulling:
                    {
                        HandlePulling(gameTime);
                        break;
                    }
            }

            #endregion

            #region Press Jump
            //  Every state should be able to jump.
            if (Input.Jump())
            {
                if (this.grabbingJoint != null)
                {
                    GameplayScreen.World.RemoveJoint(grabbingJoint);
                    this.grabbingJoint = null;
                }

                if (CanJump || CanDoubleJump)
                {
                    HandleJumping(-GameplayScreen.World.Gravity);
                }
            }
            #endregion

            #region Press Interact

            if (Input.Interact())
            {
                if (grabbingJoint != null)
                {
                    GameplayScreen.World.RemoveJoint(this.grabbingJoint);
                    this.grabbingJoint = null;
                }
            }

            #endregion
        }
        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        #region Private Methods

        #region Collisions

        private void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            //if (PlayerState == pState.Dead || PlayerState == pState.Jumping) return;

            if (TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Remove(fixtureB);
            }

            //  Player separation needs to work in a certain way.
            //  the jumping state is handled in HandleJumping so
            //  on separation must check if the player has fallen 
            //  off of a ledge.
            if (TouchingFixtures.Count == 0)
            {
                //  Player shouldn't be able to initiate a jump if not touching
                //  the floor, only able to use double jump.
                this._canJump = false;
                this.PlayerState = pState.Falling;
                this.WheelJoint.MotorSpeed = 0.0f;
            }
        }
        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //  We don't want sensors to affect anything for the player as only sensors want to 
            //  sense the player rather than the other way around.
            if (fixtureB.IsSensor)
            {
                return false;
            }

            //  We don't want anything bringing the player back to life by changing state
            //  accidently by a collision.
            if (PlayerState == pState.Dead)
            {
                return true;
            }

            if (!TouchingFixtures.Contains(fixtureB))
            {
                TouchingFixtures.Add(fixtureB);
            }

            if (PlayerState != pState.Running && PlayerState != pState.Grounded)
            {
                //  Check the if the object has 
                object type = fixtureB.UserData;
                if (PlayerState == pState.Falling && _airTime >= _maxAirTime && type is int)
                {
                    if ((int)type != 1)
                    {
                        //this.Kill();
                        //return true;
                    }
                }
                //Input.VibrateGP(100f, 0.6f);
                PlayerState = pState.Grounded;
            }

            this.PlayerState = pState.Grounded;

            if (!_canJump || !_canDoubleJump)
            {
                _canJump = true;
                _canDoubleJump = true;
            }

            _airTime = 0.0f;

            return true;
        }
        #endregion

        #region Handle All Movements

        #region Handle Jumping
        private void HandleJumping(Vector2 Gravity)
        {
            Vector2 force = Gravity;

            //  First jump
            if (CanJump)
            {
                force *= _jumpForce;

                if (PlayerState == pState.Climbing)
                {
                    if (Input.LeftCheck())
                    {
                        force *= SpinAssist.ModifyVectorByUp(new Vector2(force.Y * 4f, -_jumpForce));
                    }
                    else if (Input.RightCheck())
                    {
                        force *= SpinAssist.ModifyVectorByUp(new Vector2(-force.Y * 4f, -_jumpForce));
                    }

                    ToggleBodies(true);
                    WheelBody.ApplyLinearImpulse(force);
                    this.PlayerState = pState.Jumping;

                    return;
                }

                WheelBody.FixtureList[0].Body.ApplyLinearImpulse(force);
                this.PlayerState = pState.Jumping;
                _canJump = false;
            }
                //  Second jump (steam jump)
            else if (_dJumpEnabled && _canDoubleJump)
            {
                force *= _jumpForce;

                // Apply 2 forces. Up and then directiona;
                if (Math.Abs(Input.GP_LeftThumbstick.X) >= 0.2)
                {
                    Vector2 direction = new Vector2(Input.GP_LeftThumbstick.X, 0);
                    direction.Normalize();
                    direction *= 15.0f;

                    WheelBody.ApplyLinearImpulse(direction);
                }

                WheelBody.FixtureList[0].Body.ApplyLinearImpulse(force);
                _canDoubleJump = false;
                this.PlayerState = pState.Jumping;
            }
        }

        #endregion

        #region Handle Running and Idling

        void HandleMoving(GameTime gameTime)
        {
            if (WheelJoint.MotorSpeed == 0)
                this.PlayerState = pState.Grounded;
            else
                this.PlayerState = pState.Running;

        }
        #endregion

        #region Handle Death
        /// <summary>
        /// Contains changes to the character to handle the death
        /// </summary>
        private void HandleDeath()
        {
            this.PlayerState = pState.Dead;
            this.Body.SleepingAllowed = true;
            WheelBody.Friction = 1f;

            WheelJoint.MotorSpeed = 0f;
        }
        #endregion

        #region Handle Climbing
        private void HandleClimbing(GameTime gameTime)
        {
            Vector2 direction = Vector2.Zero;

            if (Input.isGamePad)
            {
                //  Using a % mod on movement speed fixes a separation issue with 
                //  ladders where separation should disconnect player doesn't.
                if (Input.GP_LeftThumbstick.Y <= -0.25f)
                {
                    ToggleBodies(true);

                    direction = (GameplayScreen.World.Gravity / 6f);
                }
                else if (Input.GP_LeftThumbstick.Y >= 0.25f)
                {
                    ToggleBodies(true);

                    direction = -(GameplayScreen.World.Gravity / 6f);
                }
                else
                    ToggleBodies(false);
            } 
            else 
            {
                if (Input.W)
                {
                    ToggleBodies(true);

                    direction = -(GameplayScreen.World.Gravity / 6f);
                }
                else
                if (Input.S)
                {
                    ToggleBodies(true);

                    direction = (GameplayScreen.World.Gravity / 6f);
                }
                else 
                    ToggleBodies(false);
            }

            if (WheelJoint.MotorSpeed != 0.0f) WheelJoint.MotorSpeed = 0.0f;
                this.WheelBody.LinearVelocity = direction;
        }
        #endregion

        private void HandleSwinging(GameTime gameTime)
        {
            mainBody.Rotation = GrabRotation;
        }

        private void HandleAir(GameTime gameTime)
        {
            _airTime += ((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f) * Math.Abs(SpinAssist.ModifyVectorByUp(Vector2.Normalize(this.Body.LinearVelocity)).Y);

            if (PlayerState != pState.Falling)
            {
                //  Gives the check a minimum value with 0.3
                //  whilst also checking a max.
                if (_airTime >= _maxJumpTime && _airTime > 0.3f)
                {
                    this.PlayerState = pState.Falling;
                    _airTime = 0.0f;
                }
            }

            if (Input.LeftCheck())
            {
                this.WheelBody.ApplyForce(SpinAssist.ModifyVectorByUp(new Vector2(-_midAirForce, 0)));
            }
            else if (Input.RightCheck())
            {
                this.WheelBody.ApplyForce(SpinAssist.ModifyVectorByUp(new Vector2(_midAirForce, 0)));
            }
        }

        private void HandlePulling(GameTime gameTime)
        {
            if (Input.LeftCheck())
            {
                this.WheelJoint.MotorSpeed = -_movementSpeed;
            }
            else if (Input.RightCheck())
            {
                this.WheelJoint.MotorSpeed = _movementSpeed;
            }
            else
            {
                this.WheelJoint.MotorSpeed = 0.0f;
            }
        }

        #endregion

        #region Kill Player
        /// <summary>
        /// Allows global usage for objects to cause the death of the player.
        /// </summary>
        public void Kill()
        {
            HandleDeath();
        }
        #endregion

        #region Apply a Force
        /// <summary>
        /// Allows objects to force the player in a certain direction
        /// whether it be jumping or blowing the player away.
        /// </summary>
        /// <param name="dir">target direction vector</param>
        /// <param name="force">force to apply</param>
        public void ApplyForce(Vector2 dir, float force)
        {
            //  Linear velocity must be made void before 
            //  reapplying another force, otherwise it scales
            //  and becomes wildly incorrect.

            Body.ResetDynamics();
            WheelBody.ResetDynamics();

            WheelBody.ApplyLinearImpulse(dir * force);
        }
        #endregion

        #region Toggle Body Activity
        public void ToggleBodies(bool active)
        {
            if (Body.Enabled == active) return;

            //  toggle
            Body.Enabled = active;
            WheelBody.Enabled = active;

            if (active)
            {
                //  Wake them up
                Body.Awake = true;
                WheelBody.Awake = true;
            }
        }
        #endregion

        #region Join Ladder
        public void JoinLadder(Vector2 MoveTo)
        {
            _canDoubleJump = true;
            _canJump = true;

            if (Player.Instance.PlayerState != pState.Climbing)
            {
                PlayerState = pState.Climbing;
            }

            this.SetPosition = MoveTo;
        }
        #endregion

        #region Force Falling
        /// <summary>
        /// For use with ladder disconnects and maybe other things.
        /// </summary>
        public void ForceFall()
        {
            this.ToggleBodies(true);
            if (PlayerState != pState.Grounded || PlayerState == pState.Running)
            {
                PlayerState = pState.Falling;
            }
        }
        #endregion

        #region Player specific body settings

        private void SetupPlayerSettings(bool enableDoubleJump)
        {
            this.WheelBody.OnCollision += Body_OnCollision;
            this.WheelBody.OnSeparation += Body_OnSeparation;
            this.WheelBody.AngularDamping = 10.0f;
            this.WheelBody.LinearDamping = 0.7f;
            this.WheelBody.CollisionCategories = Category.Cat12;
            this.mainBody.CollisionCategories = Category.Cat10;
            
            this.PlayerState = pState.Grounded;
            this._canJump = true;
            this._dJumpEnabled = enableDoubleJump;
            this._airTime = 0.0f;

            if (_dJumpEnabled)
                _canDoubleJump = true;
            else
                _canDoubleJump = false;
        }

        #endregion

        public void CreateWeldToPlayer(Fixture fixture, Contact contact)
        {
            if (grabbingJoint != null) return;

            PlayerState = pState.Pulling;
            grabbingJoint = JointFactory.CreateWeldJoint(GameplayScreen.World, this.Body, fixture.Body, contact.Manifold.LocalPoint);
        }

        public void GrabRope()
        {
            PlayerState = pState.Swinging;
        }

        #endregion
    }
}
