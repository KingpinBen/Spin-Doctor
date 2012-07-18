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
using System.Xml;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using GameLibrary.Graphics;
using GameLibrary.Helpers;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Graphics.Camera;
using GameLibrary.GameLogic.Screens;

#endregion

namespace GameLibrary.GameLogic
{
    public sealed class Player : Character
    {
        #region Fields

        private static readonly Player playerInstance = new Player();
        private const float _movementSpeed = 16.0f;
        private const float _jumpForce = 4.2f;      // 3.2f - Waist height. Pre 24/5
        private const float _maxJumpTime = 0.54f;
        private const float _maxAirTime = 0.84f;
        private const float _midAirForce = 200.0f;
        private bool _canJump;
        private bool _canDoubleJump;
        private float _grabbingRotation;
        private float _airTime;
        
        #endregion

        #region Properties

        public bool CanJump
        {
            get
            {
                return _canJump;
            }
        }

        public bool CanDoubleJump
        {
            get
            {
                return _canDoubleJump;
            }
        }

        public bool DoubleJumpEnabled
        {
            get
            {
                return GameSettings.DoubleJumpEnabled;
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

        public Fixture PlayerHitBox
        {
            get
            {
                return this._mainBody.FixtureList[this._mainBody.FixtureList.Count - 1];
            }
        }

        #endregion

        public Player() : base() { }

        public override void Load(ContentManager content, World world, Vector2 position)
        {
            base.Load(content, world, position);

            SetupPlayerSettings();

            if (_animations.Count == 0)
                AddAnimations();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Keeps the body rotation up, moving with the camera.
            if (Camera.Instance.IsLevelRotating || (this._mainBody.Rotation != (float)-Camera.Instance.Rotation && PlayerState != PlayerState.Swinging))
            {
                this._mainBody.Rotation = (float)-Camera.Instance.Rotation;
            }

            if (PlayerState == PlayerState.Dead)
            {
                return;
            }

            if (!this._mainBody.Enabled && PlayerState != PlayerState.Climbing)
            {
                this.ToggleBodies(true);
            }

            if (InputManager.Instance.LeftCheck())
            {
                this.WheelJoint.MotorSpeed = -_movementSpeed;
                _lookingDirection = SpriteEffects.FlipHorizontally;
            }
            else if (InputManager.Instance.RightCheck())
            {
                this.WheelJoint.MotorSpeed = _movementSpeed;
                this._lookingDirection = SpriteEffects.None;
            }
            else
            {
                this.WheelJoint.MotorSpeed = 0.0f;
            }

            #region How to handle each state
            switch (PlayerState)
            {
                case PlayerState.Climbing:
                    HandleClimbing(gameTime);
                    break;
                case PlayerState.Falling:
                    HandleAir(gameTime);
                    break;
                case PlayerState.Jumping:
                    HandleAir(gameTime);
                    break;
                case PlayerState.Pulling:
                    HandlePulling(gameTime);
                    break;
                case PlayerState.Swinging:
                    HandleSwinging(gameTime);
                    break;
                default:
                    HandleMoving(gameTime);
                    break;
            }

            #endregion

            #region Press Jump
            //  Every state should be able to jump.
            if (InputManager.Instance.Jump())
            {
                if (CanJump || CanDoubleJump)
                {
                    HandleJumping(-GameplayScreen.World.Gravity);
                }
            }
            #endregion
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(FontManager.Instance.GetFont(Graphics.FontList.Debug).Font, "AirTime: " + _airTime, ConvertUnits.ToDisplayUnits(this.Body.Position), Color.White);
            base.Draw(spriteBatch);
        }

        #region Private Methods

        #region Collisions

        private void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (PlayerState == PlayerState.Dead || PlayerState == PlayerState.Swinging)
            {
                return;
            }

            if (this._touchingFixtures.Contains(fixtureB))
            {
                this._touchingFixtures.Remove(fixtureB);
            }

            //  Player separation needs to work in a certain way.
            //  the jumping state is handled in HandleJumping so
            //  on separation must check if the player has fallen 
            //  off of a ledge.
            if (this._touchingFixtures.Count == 0)
            {
                //  Player shouldn't be able to initiate a jump if not touching
                //  the floor, only able to use double jump.
                this._canJump = false;
                this.PlayerState = PlayerState.Falling;
                this.WheelJoint.MotorSpeed = 0.0f;
            }
        }

        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //  We don't want anything bringing the player back to life by changing state
            //  accidently by a collision.
            if (PlayerState == PlayerState.Dead)
            {
                return true;
            }

            //  We don't want sensors to affect anything for the player as only sensors want to 
            //  sense the player rather than the other way around.
            if (fixtureB.IsSensor)
            {
                return true;
            }

            if (!this._touchingFixtures.Contains(fixtureB))
            {
                this._touchingFixtures.Add(fixtureB);
            }

            if (PlayerState != PlayerState.Running && PlayerState != PlayerState.Grounded)
            {
                //  Check the if the object has 
                object type = fixtureB.UserData;
                if (PlayerState == PlayerState.Falling && _airTime >= _maxAirTime && type is int)
                {
                    if ((int)type != 1)
                    {
                        this.Kill();
                        return true;
                    }
                }
                //Input.VibrateGP(100f, 0.6f);
                PlayerState = PlayerState.Grounded;
            }

            this.PlayerState = PlayerState.Grounded;

            if (!_canJump || !_canDoubleJump)
            {
                this._canJump = true;
                this._canDoubleJump = true;
            }

            this._airTime = 0.0f;

            return true;
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

        private void SetupPlayerSettings()
        {
            float height = ConvertUnits.ToSimUnits(_charHeight);
            float width = ConvertUnits.ToSimUnits(_charWidth);

            this._wheelBody.OnCollision += Body_OnCollision;
            this._wheelBody.OnSeparation += Body_OnSeparation;
            //this.WheelBody.AngularDamping = 10.0f;
            //this.WheelBody.LinearDamping = 0.7f;
            this._wheelBody.CollisionCategories = Category.Cat12;
            this._mainBody.CollisionCategories = Category.Cat10;
            this._mainBody.IsSensor = false;

            Fixture hitbox = FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(56), height, 0.0f, ConvertUnits.ToSimUnits(new Vector2(0, 28)), _mainBody);
            
            this._mainBody.FixtureList[this._mainBody.FixtureList.Count - 1].IsSensor = true;
            
            this._playerState = PlayerState.Grounded;
            this._canJump = true;
            this._airTime = 0.0f;

            if (GameSettings.DoubleJumpEnabled)
            {
                _canDoubleJump = true;
            }
            else
            {
                _canDoubleJump = false;
            }
        }

        #region PlayerState Dependent Methods

        void HandleJumping(Vector2 Gravity)
        {
            Vector2 force = Gravity;

            //  First jump
            if (CanJump)
            {
                if (PlayerState == PlayerState.Climbing)
                {
                    force *= 3.0f;

                    if (InputManager.Instance.LeftCheck())
                    {
                        force += SpinAssist.ModifyVectorByUp(new Vector2(-150, 0));
                    }
                    else if (InputManager.Instance.RightCheck())
                    {
                        force += SpinAssist.ModifyVectorByUp(new Vector2(150, 0));
                    }

                    this.ToggleBodies(true);
                    this.WheelBody.ApplyLinearImpulse(force);
                    this.PlayerState = PlayerState.Jumping;
                }
                else
                {

                    force *= _jumpForce;
                    WheelBody.FixtureList[0].Body.ApplyLinearImpulse(force);
                    this.PlayerState = PlayerState.Jumping;
                    _canJump = false;
                }
            }
            //  Second jump (steam jump)
            else if (this.DoubleJumpEnabled && _canDoubleJump)
            {
                force *= _jumpForce;

                // Apply 2 forces. Up and then directiona;
                if (Math.Abs(InputManager.Instance.GP_LeftThumbstick.X) >= 0.2)
                {
                    Vector2 direction = new Vector2(InputManager.Instance.GP_LeftThumbstick.X, 0);
                    direction.Normalize();
                    direction *= 15.0f;

                    WheelBody.ApplyLinearImpulse(direction);
                }

                WheelBody.FixtureList[0].Body.ApplyLinearImpulse(force);
                _canDoubleJump = false;
                this.PlayerState = PlayerState.Jumping;
            }
        }

        void HandleMoving(GameTime gameTime)
        {
            if (WheelJoint.MotorSpeed == 0)
                this.PlayerState = PlayerState.Grounded;
            else
                this.PlayerState = PlayerState.Running;

        }

        #region Death and Killing the player

        void HandleDeath()
        {
            this.PlayerState = PlayerState.Dead;
            this.Body.SleepingAllowed = true;
            WheelBody.Friction = 1f;

            WheelJoint.MotorSpeed = 0f;
        }

        #region Kill Player
        /// <summary>
        /// Objects can use this to kill the player.
        /// </summary>
        public void Kill()
        {
            HandleDeath();
        }
        #endregion

        #endregion

        #region Climbing and Ladders

        void HandleClimbing(GameTime gameTime)
        {
            Vector2 direction = Vector2.Zero;

            if (InputManager.Instance.isGamePad)
            {
                //  Using a % mod on movement speed fixes a separation issue with 
                //  ladders where separation should disconnect player doesn't.
                if (InputManager.Instance.GP_LeftThumbstick.Y <= -0.25f)
                {
                    ToggleBodies(true);
                    this.CurrentAnimation.SetPlayback(true);
                    direction = (GameplayScreen.World.Gravity / 6f);
                }
                else if (InputManager.Instance.GP_LeftThumbstick.Y >= 0.25f)
                {
                    ToggleBodies(true);
                    this.CurrentAnimation.SetPlayback(false);
                    direction = -(GameplayScreen.World.Gravity / 6f);
                }
                else
                {
                    ToggleBodies(false);
                }
            }
            else
            {
                if (InputManager.Instance.W)
                {
                    ToggleBodies(true);
                    this.CurrentAnimation.SetPlayback(true);
                    direction = -(GameplayScreen.World.Gravity / 6f);
                }
                else if (InputManager.Instance.S)
                {
                    ToggleBodies(true);
                    this.CurrentAnimation.SetPlayback(false);
                    direction = (GameplayScreen.World.Gravity / 6f);
                }
                else
                {
                    ToggleBodies(false);
                }
            }

            if (WheelJoint.MotorSpeed != 0.0f) WheelJoint.MotorSpeed = 0.0f;
            this.WheelBody.LinearVelocity = direction;
        }

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

        public void JoinLadder(Vector2 MoveTo)
        {
            _canDoubleJump = true;
            _canJump = true;

            if (Player.Instance.PlayerState != PlayerState.Climbing)
            {
                PlayerState = PlayerState.Climbing;
            }

            this.SetPosition = MoveTo;
        }

        #region Force Falling
        /// <summary>
        /// For use with ladder disconnects and maybe other things.
        /// </summary>
        public void ForceFall()
        {
            this.ToggleBodies(true);
            if (PlayerState != PlayerState.Grounded || PlayerState == PlayerState.Running)
            {
                PlayerState = PlayerState.Falling;
            }
        }
        #endregion

        #endregion

        #region Swinging and Rope

        void HandleSwinging(GameTime gameTime)
        {
            this._mainBody.Rotation = GrabRotation;

            if (InputManager.Instance.LeftCheck())
            {
                this.WheelBody.ApplyForce(SpinAssist.ModifyVectorByUp(new Vector2(-_midAirForce * 1.5f, 0)));
            }
            else if (InputManager.Instance.RightCheck())
            {
                this.WheelBody.ApplyForce(SpinAssist.ModifyVectorByUp(new Vector2(_midAirForce * 1.5f, 0)));
            }
        }

        public void GrabRope()
        {
            PlayerState = PlayerState.Swinging;
        }

        #endregion

        void HandleAir(GameTime gameTime)
        {
            _airTime += ((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f) * Math.Abs(SpinAssist.ModifyVectorByUp(Vector2.Normalize(this.Body.LinearVelocity)).Y);

            if (PlayerState != PlayerState.Falling)
            {
                //  Gives the check a minimum value with 0.3
                //  whilst also checking a max.
                if (_airTime >= _maxJumpTime && _airTime > 0.3f)
                {
                    this.PlayerState = PlayerState.Falling;
                    _airTime = 0.0f;
                }
            }

            if (InputManager.Instance.LeftCheck())
            {
                this.WheelBody.ApplyForce(SpinAssist.ModifyVectorByUp(new Vector2(-_midAirForce, 0)));
            }
            else if (InputManager.Instance.RightCheck())
            {
                this.WheelBody.ApplyForce(SpinAssist.ModifyVectorByUp(new Vector2(_midAirForce, 0)));
            }
        }

        void HandlePulling(GameTime gameTime)
        {
            if (InputManager.Instance.LeftCheck())
            {
                this.WheelJoint.MotorSpeed = -_movementSpeed;
            }
            else if (InputManager.Instance.RightCheck())
            {
                this.WheelJoint.MotorSpeed = _movementSpeed;
            }
            else
            {
                this.WheelJoint.MotorSpeed = 0.0f;
            }
        }

        #endregion

        #endregion
    }
}
