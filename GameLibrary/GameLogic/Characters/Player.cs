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
using GameLibrary.Graphics.Drawing;
using System.Collections.Generic;
using GameLibrary.Graphics.Animation;
using GameLibrary.Audio;

#endregion

namespace GameLibrary.GameLogic.Characters
{
    public sealed class Player : Character
    {
        #region Fields

        private const float _movementSpeed = Defines.PLAYER_RUN_SPEED;
        private const float _jumpForce = Defines.PLAYER_JUMP_FORCE;
        private const float _maxAirTime = Defines.PLAYER_MAX_AIR_TIME;
        private const float _midAirForce = Defines.PLAYER_MIDAIR_FORCE;

        private bool _canJump;
        private bool _canDoubleJump;
        private bool _deadlyFall;
        private float _grabbingRotation;

        private Fixture _connectedFixture;


        #region Double Jump Sprite
        /// <summary>
        /// The sprite to copy from for the doublejump effect.
        /// Has some preset settings to use but will randomly generate
        /// some when it's copied to vary it from jump to jump.
        /// </summary>
        private Sprite _steamSprite;
        #endregion

        #endregion

        #region Properties

        public override PlayerState PlayerState
        {
            get
            {
                return base.PlayerState;
            }
            protected set
            {
                if (value == Characters.PlayerState.Climbing)
                {
                    this._mainBody.ResetDynamics();
                    this._wheelBody.ResetDynamics();
                    this._canJump = true;
                    this._canDoubleJump = true;

                    this._inAir = false;
                    this._airTime = 0.0f;
                }
                else if (value == Characters.PlayerState.Falling)
                {
                    this._lastSafePosition = ConvertUnits.ToDisplayUnits(_mainBody.Position);
                    this._canJump = false;

                    if (_playerState == Characters.PlayerState.Jumping)
                    {
                        this._airTime = 0.0f;
                    }
                }

                this._soundElapsed = 0.0f;

                base.PlayerState = value;
            }
        }


        #endregion

        #region Singleton and Load

        #region Singleton

        /// <summary>
        /// The singleton.
        /// </summary>
        private static readonly Player playerInstance = new Player();

        /// <summary>
        /// The public instance to access the Player through
        /// </summary>
        public static Player Instance
        {
            get
            {
                return playerInstance;
            }
        }

        /// <summary>
        /// The constructor can only be accessed once through the singleton
        /// so is set private
        /// </summary>
        private Player() : base() { }

        #endregion

        public override void Load(IServiceProvider provider, World world, Vector2 position)
        {
            base.Load(provider, world, position);
#if !EDITOR

            SetupPlayerSettings();

            _animations.Clear();
            AddAnimations();

            _steamSprite = new Sprite();
            _steamSprite.Init(Vector2.Zero, "Assets/Images/Effects/steam");
            _steamSprite.Load(_content, world);
            _steamSprite.CastShadows = true;
            _steamSprite.Alpha = 0.6f;
            _steamSprite.AlphaDecay = 0.05f;
            _steamSprite.RotationSpeed = 0.1f;
            _steamSprite.Scale = 0.3f;
            _steamSprite.ScaleFactor = 0.01f;
            
#endif
        }

        

        #endregion

        #region Update and Draw

        public override void Update(float delta, World world)
        {
            base.Update(delta, world);
            float worldRotation = Camera.Instance.GetWorldRotation();

            //Keeps the body rotation up, moving with the camera.
            if (Camera.Instance.IsLevelRotating || (_mainBody.Rotation != (float)-worldRotation && _playerState != PlayerState.Swinging))
            {
                this._mainBody.Rotation = (float)-worldRotation;
            }

            if (_playerState != PlayerState.Dead)
            {
                if (InputManager.Instance.Jump(true))
                {
                    if (_canDoubleJump || _canJump)
                    {
                        this.HandleJumping(-world.Gravity);
                    }
                }

                switch (_playerState)
                {
                    case PlayerState.Climbing:
                        HandleClimbing(delta, world);
                        break;
                    case PlayerState.Falling:
                        HandleAir(delta);
                        break;
                    case PlayerState.Jumping:
                        HandleAir(delta);
                        break;
                    case PlayerState.Swinging:
                        HandleSwinging(delta);
                        break;
                    default:
                        HandleMoving(delta);
                        break;
                }      
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        #endregion

        #region Private Methods



        #region Collision Events


        #region Separation


        private void WheelBody_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (_playerState == PlayerState.Dead || _playerState == PlayerState.Swinging)
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
            if (this._touchingFixtures.Count == 0 && _playerState != Characters.PlayerState.Climbing)
            {
                this._inAir = true;
                this._wheelJoint.MotorSpeed = 0.0f;
                this._airTime = 0.0f;
                this._canJump = false;
                this._lastSafePosition = ConvertUnits.ToDisplayUnits(_mainBody.Position);
            }
        }

        #endregion

        #region Collision


        /// <summary>
        /// Occurs when the wheel of the player collides with surface, including sensors
        /// </summary>
        /// <param name="fixtureA">Player fixture</param>
        /// <param name="fixtureB">Colliding body fixture</param>
        /// <param name="contact">The contact</param>
        /// <returns>True if collision has been recognised, false is ignore collision, both contact.Enabled and any checks.</returns>
        private bool WheelBody_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //  We want to recognise a collision, but break out early if a certain condition 
            //  are met.
            if (_playerState == PlayerState.Dead || _playerState == Characters.PlayerState.Swinging || 
                fixtureB.IsSensor)
            {
                //  Returning true recognises the collision but breaks out before allowing
                //  anything below to change anything related to the player.
                return true;
            }

            //  Hold a reference to every fixture that is in contact with the player
            if (!this._touchingFixtures.Contains(fixtureB))
            {
                this._touchingFixtures.Add(fixtureB);
            }


            //  Handle whatever state they're in if they're not running or idle.
            if (!(_playerState == PlayerState.Running || _playerState == PlayerState.Grounded))
            {

                //  Check if the colliding object has any UserData.
                object type = fixtureB.Body.UserData;

                //  Check if the player has exceeded max air time
                //  in case they need to die from falling.
                if (_deadlyFall)
                {
                    AudioManager.Instance.StopCue("Harland_Falling", Microsoft.Xna.Framework.Audio.AudioStopOptions.AsAuthored);

                    if (type != null && type.ToString() == "Cushion")
                    {
                        //  UserData 1 means absorbs falling damage, so don't kill.
                        AudioManager.Instance.PlayCue("Harland_Land_Cushioned", true);
                    }
                    else
                    {
                        AudioManager.Instance.PlayCue("Harland_Land", true);
                        this.Kill();
                        return true;
                    }
                }

                if (_playerState != Characters.PlayerState.Grounded)
                {
                    this.PlayerState = PlayerState.Grounded;
                }
            }
            else
            {
                if (fixtureB.Body.UserData != null && fixtureB.Body.UserData.ToString() != "")
                {
                    if (_touchingFixtures.Count == 0)
                    {
                        AudioManager.Instance.PlayCue("Footsteps_" + fixtureB.Body.UserData.ToString(), true);
                    }
                }
            }


            this._airTime = 0.0f;
            this._inAir = false;
            this._deadlyFall = false;

            

            if (!_canJump || !_canDoubleJump)
            {
                this._canJump = true;
                this._canDoubleJump = true;
                AudioManager.Instance.PlayCue("Harland_Jump", true);
            }

            return true;
        }



        #endregion

        #endregion

        

        #region Player State Dependent Methods

        void HandleJumping(Vector2 Gravity)
        {
            if (_playerState == Characters.PlayerState.Swinging)
            {
                return;
            }

            Vector2 force = Gravity;
            InputManager input = InputManager.Instance;

            //  First jump
            if (_canJump)
            {
                if (_playerState == PlayerState.Climbing)
                {
                    if (!(input.MoveLeft(false) || input.MoveRight(false)))
                    {
                        return;
                    }

                    force *= 3.0f;

                    if (input.MoveLeft(false))
                    {
                        Vector2 additionalForce = SpinAssist.ModifyVectorByUp(new Vector2(-150, -force.Y * 0.5f));
                        Vector2.Add(ref force, ref additionalForce, out force);
                        this._lookingDirection = SpriteEffects.FlipHorizontally;
                    }
                    else if (InputManager.Instance.MoveRight(false))
                    {
                        Vector2 additionalForce = SpinAssist.ModifyVectorByUp(new Vector2(150, -force.Y * 0.5f));
                        Vector2.Add(ref force, ref additionalForce, out force);
                        this._lookingDirection = SpriteEffects.None;
                    }

                    this.ToggleBodies(true);
                    this._wheelBody.ApplyLinearImpulse(force);
                    this.PlayerState = PlayerState.Jumping;
                    this._canJump = false;
                }
                else
                {

                    force *= _jumpForce;
                    _wheelBody.FixtureList[0].Body.ApplyLinearImpulse(force);
                    this.PlayerState = PlayerState.Jumping;
                    this._canJump = false;
                    this._airTime = 0.0f;
                    AudioManager.Instance.PlayCue("Harland_Grunt", true);
                }
            }
            //  Second jump (steam jump)
            else if (GameSettings.Instance.DoubleJumpEnabled && _canDoubleJump)
            {
                //  We only want to 0 the Up/Down depending on the
                //  orientation.
                #region Reset the Y dynamics.
                if (Math.Abs(Gravity.Y) > 0)
                {
                    this._mainBody.LinearVelocity = new Vector2(_mainBody.LinearVelocity.X, 0);
                    this._wheelBody.LinearVelocity = new Vector2(_mainBody.LinearVelocity.X, 0);
                }
                else
                {
                    this._mainBody.LinearVelocity = new Vector2(0, _mainBody.LinearVelocity.Y);
                    this._wheelBody.LinearVelocity = new Vector2(0, _mainBody.LinearVelocity.Y);
                }

                #endregion

                force *= _jumpForce;

                // Apply 2 forces. Up and then direction
                if (Math.Abs(InputManager.Instance.LeftThumbstick.X) >= 0.2)
                {
                    Vector2 direction = new Vector2(InputManager.Instance.LeftThumbstick.X, 0);
                    direction.Normalize();
                    direction *= 15.0f;

                    _wheelBody.ApplyLinearImpulse(direction);
                }

                //  Disable the possibilty to double jump.
                this._canDoubleJump = false;

                //  Reset any previous jump/fall timers.
                this._airTime = 0.0f;
                this._deadlyFall = false;
                this._lastSafePosition = ConvertUnits.ToDisplayUnits(_mainBody.Position);

                //  Switch the animation and reset it incase the player
                //  is already jumping.
                this.PlayerState = PlayerState.Jumping;
                this.CurrentAnimation.ResetCurrentFrame();

                //  Apply the force
                this._wheelBody.FixtureList[0].Body.ApplyLinearImpulse(force);

                //  then create the steam plume.
                this.CreateSteamPlume();
                AudioManager.Instance.PlayCue("Harland_Jump_Steam", true);
            }
        }

        void HandleMoving(float delta)
        {
            InputManager input = InputManager.Instance;

            if (!_inAir)
            {
                if (input.MoveLeft(false))
                {
                    this._wheelJoint.MotorSpeed = -_movementSpeed;
                    this._lookingDirection = SpriteEffects.FlipHorizontally;
                }
                else if (input.MoveRight(false))
                {
                    this._wheelJoint.MotorSpeed = _movementSpeed;
                    this._lookingDirection = SpriteEffects.None;
                }
                else
                {
                    this._wheelJoint.MotorSpeed = 0.0f;
                }

                if (_wheelJoint.MotorSpeed == 0)
                {
                    if (_playerState != Characters.PlayerState.Grounded)
                    {
                        this.PlayerState = PlayerState.Grounded;
                        this._soundElapsed = 0.25f;
                    }
                }
                else
                {
                    if (_playerState != Characters.PlayerState.Running)
                    {
                        this.PlayerState = PlayerState.Running;
                    }

                    if (_touchingFixtures.Count > 0)
                    {
                        this._soundElapsed -= delta;

                        if (_soundElapsed <= 0.0f)
                        {
                            this.PlayFootsteps();
                            this._soundElapsed = 0.5f;
                        }
                    }
                }
            }
        }

        void PlayFootsteps()
        {
            if (_touchingFixtures.Count > 0)
            {
                if (_touchingFixtures[0].Body.UserData != null)
                {
                    //  Get the type of material it should play.
                    string materialType = _touchingFixtures[0].Body.UserData.ToString();

                    //  If the material type is a type we don't want, just escape.
                    if (materialType == null || materialType == "Static" || materialType == "None")
                    {
                        return;
                    }

                    //  Play the material type footstep sound
                    AudioManager.Instance.PlayCue("Footsteps_" + materialType, true);
                }
            }
        }

        #region Death and Killing the player

        void HandleDeath()
        {
            this.PlayerState = PlayerState.Dead;
            this._mainBody.SleepingAllowed = true;
            this._wheelBody.Friction = 3f;
            this._wheelJoint.MotorSpeed = 0f;
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

        void HandleClimbing(float delta, World world)
        {
            Vector2 direction = Vector2.Zero;

            if (InputManager.Instance.MoveUp(false))
            {
                this.ToggleBodies(true);
                this.CurrentAnimation.SetPlayback(false);
                direction = -(world.Gravity / 6f);
            }
            else if (InputManager.Instance.MoveDown(false))
            {
                this.ToggleBodies(true);
                this.CurrentAnimation.SetPlayback(true);
                direction = (world.Gravity / 6f);
            }
            else
            {
                ToggleBodies(false);
            }

            this._wheelBody.LinearVelocity = direction;
        }



        public void JoinLadder(Vector2 MoveTo)
        {
            if (_playerState != PlayerState.Climbing)
            {
                this.PlayerState = PlayerState.Climbing;
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

            if (_playerState == Characters.PlayerState.Dead)
            {
                return;
            }

            if (!(_playerState == PlayerState.Running || _playerState == PlayerState.Grounded))
            {
                this.PlayerState = PlayerState.Falling;
            }
        }
        #endregion

        #endregion

        #region Swinging and Rope

        void HandleSwinging(float delta)
        {
            this._mainBody.Rotation = _grabbingRotation;

            if (InputManager.Instance.MoveLeft(false))
            {
                this._wheelBody.ApplyForce(SpinAssist.ModifyVectorByUp(new Vector2(-_midAirForce * 1.5f, 0)));
            }
            else if (InputManager.Instance.MoveRight(false))
            {
                this._wheelBody.ApplyForce(SpinAssist.ModifyVectorByUp(new Vector2(_midAirForce * 1.5f, 0)));
            }
        }

        public void GrabRope()
        {
            this.PlayerState = PlayerState.Swinging;
            this._airTime = 0.0f;
            this._inAir = false;
            this._deadlyFall = false;
        }

        #endregion

        void HandleAir(float delta)
        {
            Vector2 bodyPos = ConvertUnits.ToDisplayUnits(_mainBody.Position);
            float distance = SpinAssist.ModifyVectorByUp(bodyPos - _lastSafePosition).Y;

            if (_inAir && (Math.Abs(distance) > 550) && !_deadlyFall)
            {
                _deadlyFall = true;
                AudioManager.Instance.PlayCue("Harland_Falling", true);
            }

            if (_playerState != Characters.PlayerState.Falling)
            {
                if (CurrentAnimation.Completed)
                {
                    this.PlayerState = PlayerState.Falling;
                }
            }

            if (InputManager.Instance.MoveLeft(false))
            {
                UpIs upIs = Camera.Instance.GetUpIs();
                float speedLimit = 25;

                this._wheelBody.ApplyForce(SpinAssist.ModifyVectorByUp(new Vector2(-_midAirForce, 0)));

                if (upIs == UpIs.Up || upIs == UpIs.Down)
                {
                    this._wheelBody.LinearVelocity = new Vector2(
                        MathHelper.Clamp(_wheelBody.LinearVelocity.X, -speedLimit, speedLimit),
                        _wheelBody.LinearVelocity.Y);
                }
                else 
                {
                    this._wheelBody.LinearVelocity = new Vector2(
                        _wheelBody.LinearVelocity.X,
                        MathHelper.Clamp(_wheelBody.LinearVelocity.Y, -speedLimit, speedLimit));
                }

                this._lookingDirection = SpriteEffects.FlipHorizontally;
            }
            else if (InputManager.Instance.MoveRight(false))
            {
                UpIs upIs = Camera.Instance.GetUpIs();
                float speedLimit = 25;

                this._wheelBody.ApplyForce(SpinAssist.ModifyVectorByUp(new Vector2(_midAirForce, 0)));

                if (upIs == UpIs.Up || upIs == UpIs.Down)
                {
                    this._wheelBody.LinearVelocity = new Vector2(
                        MathHelper.Clamp(_wheelBody.LinearVelocity.X, -speedLimit, speedLimit),
                        _wheelBody.LinearVelocity.Y);
                }
                else
                {
                    this._wheelBody.LinearVelocity = new Vector2(
                        _wheelBody.LinearVelocity.X,
                        MathHelper.Clamp(_wheelBody.LinearVelocity.Y, -speedLimit, speedLimit));
                }

                this._lookingDirection = SpriteEffects.None;
            }
        }

        #endregion



        #region Create Steam Plume

        /// <summary>
        /// Create
        /// </summary>
        private void CreateSteamPlume()
        {
            //  Get a random initial rotation as rotation speed is predecided.
            float rotation = SpinAssist.GetRandom(0, (float)(Math.PI * 2));
            _steamSprite.Rotation = rotation;

            //  Randomize how quickly it should dissipate
            float alphadecay = SpinAssist.GetRandom(0.02f, 0.08f);
            _steamSprite.AlphaDecay = alphadecay;

            //  Check if the fart cheat is on
            if (GameSettings.Instance.FartCheat)
            {
                //  Make it come from his buttocks and turn it green..
                _steamSprite.Tint = Color.LightGreen.ToVector3();
                _steamSprite.Position = ConvertUnits.ToDisplayUnits(_mainBody.Position);
            }
            else
            {
                //  Otherwise from the wheel.
                _steamSprite.Position = ConvertUnits.ToDisplayUnits(_wheelBody.Position);
            }

            //  Then add it to the SpriteManager for handling.
            SpriteManager.Instance.AddSprite((Sprite)_steamSprite.Clone());
        }

        #endregion



        /// <summary>
        /// Adds required aniations to the dict.
        /// </summary>
        protected override void AddAnimations()
        {
            base.AddAnimations();

            string spriteSheetLocation = "Assets/Images/Spritesheets/";

            //Texture2D running = _content.Load<Texture2D>(spriteSheetLocation + "Running-Sheet");
            Texture2D running = _content.Load<Texture2D>(spriteSheetLocation + "HarlandRun");
            Texture2D idle = _content.Load<Texture2D>(spriteSheetLocation + "HarlandIdle");
            Texture2D falling = _content.Load<Texture2D>(spriteSheetLocation + "HarlandFall");
            //  Jump1
            //  Texture2D jumping = _content.Load<Texture2D>(spriteSheetLocation + "HarlandJump");
            //  Jump2
            Texture2D jumping = _content.Load<Texture2D>(spriteSheetLocation + "HarlandJump2");
            Texture2D climbing = _content.Load<Texture2D>(spriteSheetLocation + "HarlandLadder");
            Texture2D death = _content.Load<Texture2D>(spriteSheetLocation + "HarlandDeath");
            Texture2D swinging = _content.Load<Texture2D>(spriteSheetLocation + "HarlandSwing");

            //_animations.Add("Run",       new FrameAnimation(running, 24, new Point(322, 443), 9.0f, new Point(6, 4), false, 30));
            _animations.Add("Run", new FrameAnimation(running, 24, new Point(341, 357), 57.0f, new Point(6, 4), false, 30));
            _animations["Run"].Scale = 0.45f;

            _animations.Add("Idle", new FrameAnimation(idle, 21, new Point(268, 468), 67, new Point(6, 4), false, 16));
            _animations["Idle"].Scale = 0.34f;

            _animations.Add("Falling", new FrameAnimation(falling, 21, new Point(291, 462), 100.0f, new Point(6, 4), false));
            _animations["Falling"].Scale = 0.38f;
            //  Jump1
            //  _animations.Add("Jumping", new FrameAnimation(jumping, 16, new Point(471, 480), 0, new Point(6, 3), true));
            //  Jump2
            _animations.Add("Jumping", new FrameAnimation(jumping, 22, new Point(313, 464), 0, new Point(6, 4), true));
            _animations["Jumping"].Scale = 0.38f;
            _animations.Add("Climbing", new FrameAnimation(climbing, 20, new Point(233, 503), 0, new Point(6, 4), false, 30));
            _animations["Climbing"].Scale = 0.41f;
            _animations.Add("Dead", new FrameAnimation(death, 26, new Point(341, 279), 70, new Point(6, 5), true, 48));
            _animations["Dead"].Scale = 0.56f;
            _animations.Add("Swinging", new FrameAnimation(swinging, 1, new Point(640, 488), 0, new Point(1, 1), true, 1));
            _animations["Swinging"].Scale = 0.32f;
        }

        #region Player Specific Settings

        /// <summary>
        /// Set up some player specific settings, such as the hitbox
        /// </summary>
        private void SetupPlayerSettings()
        {
            float height = ConvertUnits.ToSimUnits(_charHeight);
            float width = ConvertUnits.ToSimUnits(_charWidth);

            this._wheelBody.OnCollision += WheelBody_OnCollision;
            this._wheelBody.OnSeparation += WheelBody_OnSeparation;

            this._mainBody.IsSensor = false;

            float fixtureWidth = ConvertUnits.ToSimUnits(56);

            Fixture grabBox = FixtureFactory.AttachRectangle(fixtureWidth * 0.5f, fixtureWidth * 0.5f, 0.0f, new Vector2(0, -height * 0.4f), this._mainBody);

            Fixture hitbox = FixtureFactory.AttachRectangle(fixtureWidth, height, 0.0f, ConvertUnits.ToSimUnits(new Vector2(0, 18)), _mainBody);
            hitbox.IsSensor = true;

            this.PlayerState = PlayerState.Grounded;
            this._canJump = true;
            this._inAir = false;
            this._airTime = 0.0f;

            this._canDoubleJump = GameSettings.Instance.DoubleJumpEnabled;

            this._wheelBody.CollisionCategories = Category.Cat10;
            this._mainBody.CollisionCategories = Category.Cat10;
        }

        #endregion

        #endregion

        #region Public Methods

        public void ToggleBodies(bool active)
        {
            if (_wheelBody.Enabled != active)
            {
                //  Wake them up
                this._mainBody.Awake = true;
                this._wheelBody.Awake = true;

                //  toggle
                this._mainBody.ResetDynamics();
                this._mainBody.IgnoreGravity = !active;
                this._wheelBody.Enabled = active;
            }
        }

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

            this._mainBody.ResetDynamics();
            this._wheelBody.ResetDynamics();

            this._wheelBody.ApplyLinearImpulse(dir * force);
            this._mainBody.ApplyLinearImpulse(dir * force);
        }
        #endregion

        #region Gets

        public Body GetWheelBody()
        {
            return _wheelBody;
        }

        public Body GetMainBody()
        {
            return _mainBody;
        }

        #endregion


        #region Sets

        public void SetRotation(float rotation)
        {
            this._grabbingRotation = rotation;
        }

        #endregion


        #region Checks

        public bool CheckWheelFixture(Fixture fixture)
        {
            return fixture == _wheelBody.FixtureList[0];
        }

        public bool CheckGrabFixture(Fixture fixture)
        {
            if (_playerState == Characters.PlayerState.Dead)
            {
                return false;
            }

            return fixture == _mainBody.FixtureList[_mainBody.FixtureList.Count - 2];
        }

        public bool CheckHitBoxFixture(Fixture fixture)
        {
            if (_playerState == Characters.PlayerState.Dead)
            {
                return false;
            }

            return fixture == _mainBody.FixtureList[this._mainBody.FixtureList.Count - 1];
        }

        public bool CheckBodyBox(Fixture fixture)
        {
            if (_playerState == Characters.PlayerState.Dead)
            {
                return false;
            }

            if (fixture == _mainBody.FixtureList[0] || fixture == _wheelBody.FixtureList[0])
            {
                return true;
            }

            return false;
        }

        #endregion


        #endregion
    }
}
