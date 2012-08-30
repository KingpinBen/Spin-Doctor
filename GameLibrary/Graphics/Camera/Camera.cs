//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Camera
//--    
//--    Description
//--    ===============
//--    Tracks player movement and stores rotation
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Implemented player tracking
//--    BenG - Allows rotation of the level
//--    BenG - Added controls for changing how the camera moves
//--    BenG - Finally fixed the small error in rotation. Was caused by
//--           _toAdd being smaller than the rotationspeed.
//--    BenG - Using an enum for the player state for more control.
//--    BenG - Updated fields and properties.
//--    BenG - Fixed forcerotating allowing 2 in quick succession through 
//--           object use and player control.
//--    
//--    
//--    
//--------------------------------------------------------------------------

#define Development

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Helpers;
using GameLibrary.GameLogic;
using GameLibrary.GameLogic.Screens;
using GameLibrary.GameLogic.Characters;
using GameLibrary.Audio;
#endregion

namespace GameLibrary.Graphics.Camera
{
    public class Camera
    {
        #region Singleton


        private static Camera _singleton = new Camera();

        public static Camera Instance
        {
            get
            {
                return _singleton;
            }
        }

        private Camera() { }


        #endregion

        #region Fields
        GameplayScreen _gameScreen;
        private const float _rotationSpeed = 0.08f;
        //  90% of largest dimension
        private const float largestDimensionModifier = 0.9f;    
        private const float _gravityForce = 140.0f;
        private bool _levelRotates;
        private bool _allowLevelRotation;
        private bool _levelRotating;
        private Vector2 _cameraPosition;
        private float _worldRotation;
        private float _currentCameraZoom;
        //  Hold at what float zooms to show the whole level.
        private float _fullLevelZoom;
        private float _rotationToAdd;
        private float _rotateDelayTimer;
        private float _largestLevelDimension;
        private UpIs _upIs = UpIs.Up;
        private CameraType _cameraType = CameraType.Level;
        #endregion

        #region Properties

        /// <summary>
        /// Is it a rotating level?
        /// </summary>
        public bool LevelRotates
        {
            get
            {
                return _levelRotates;
            }
        }

        /// <summary>
        /// Is it currently possible to rotate
        /// the level?
        /// </summary>
        public bool AllowRotation
        {
            get
            {
                if (!LevelRotates)
                {
                    return false;
                }

                return _allowLevelRotation;
            }
            set
            {
                _allowLevelRotation = value;
            }
        }

        /// <summary>
        /// Is the level currently rotating?
        /// 
        /// Used for awakening some objects (like ropes) 
        /// incase they fall asleep and don't wake.
        /// </summary>
        public bool IsLevelRotating
        {
            get { return _levelRotating; }
        }

        public Vector2 Position
        {
            get { return _cameraPosition; }
        }

        public float Zoom
        {
            get
            {
                return _currentCameraZoom;
            }
            internal set
            {
                _currentCameraZoom = MathHelper.Clamp(value, 0.1f, 4.0f);
            }
        }

        public void SetUpIs(UpIs val)
        {
            this._upIs = val;
            this.changeGravity();
        }


        #endregion

        public void Load(GameplayScreen gameScreen, bool levelCanRotate, float LargestLevelDimension)
        {
            this._gameScreen = gameScreen;
            this._levelRotates = levelCanRotate;
            this.AllowRotation = _levelRotates;
            this.SetUpIs(UpIs.Up);
            this._largestLevelDimension = LargestLevelDimension;
            this.calculateZoom();
            this._fullLevelZoom = Zoom;

            this._worldRotation = 0.0f;
            this._cameraPosition = Vector2.Zero;
            this._rotateDelayTimer = 0;
            this._rotationToAdd = 0.0f;
        }

        public void Update(float delta)
        {
            Player playerInstance = Player.Instance;
            InputManager input = InputManager.Instance;

            if (_cameraType == CameraType.Level && _cameraPosition != Vector2.Zero)
            {
                this._cameraPosition = Vector2.Zero;
            }
            else if (_cameraType == CameraType.Focus)
            {
                if (playerInstance.PlayerState != PlayerState.Dead)
                {
                    this._cameraPosition = ConvertUnits.ToDisplayUnits(playerInstance.GetMainBody().Position);
                }
            }
#if Development
            else if (_cameraType == CameraType.Free)
            {
                if (InputManager.Instance.RightThumbstick.X != 0)
                    _cameraPosition += SpinAssist.ModifyVectorByUp(new Vector2(InputManager.Instance.RightThumbstick.X * 10, 0));
                if (InputManager.Instance.RightThumbstick.Y != 0)
                    _cameraPosition -= SpinAssist.ModifyVectorByUp(new Vector2(0, InputManager.Instance.RightThumbstick.Y * 10));
            }

            ZoomModifierInput();

            //  Debug camera control
            if (InputManager.Instance.GP_Back)
            {
                //  Cycle through
                switch (_cameraType)
                {
                    case CameraType.Free:
                        _cameraType = CameraType.Level;
                        break;
                    case CameraType.Level:
                        _cameraType = CameraType.Focus;
                        break;
                    case CameraType.Focus:
                        _cameraType = CameraType.Free;
                        break;
                }
            }
#endif
                this.handleInput();
                this.handleRotation(delta);
        }



        #region Private Methods

        private void changeGravity()
        {
#if !EDITOR

            switch (_upIs)
            {
                case UpIs.Up:
                    {
                        _gameScreen.World.Gravity = new Vector2(0, _gravityForce);
                        break;
                    }
                case UpIs.Down:
                    {
                        _gameScreen.World.Gravity = new Vector2(0, -_gravityForce);
                        break;
                    }
                case UpIs.Left:
                    {
                        _gameScreen.World.Gravity = new Vector2(_gravityForce, 0);
                        break;
                    }
                case UpIs.Right:
                    {
                        _gameScreen.World.Gravity = new Vector2(-_gravityForce, 0);
                        break;
                    }
            }
#endif
        }


        private void handleRotation(float delta)
        {
            //  If the delay timer hasn't cooled down,
            //  reduce the timer
            if (_rotateDelayTimer > 0)
            {
                //  by delta time
                _rotateDelayTimer -= delta;

                //  If it's passed 0s cooldown, reset it to 0 seconds.
                if (_rotateDelayTimer < 0)
                {
                    _rotateDelayTimer = 0.0f;
                }
            }

            //  Nothing else needs to happen if theres 
            //  nothing to add.
            if (_rotationToAdd != 0)
            {
                if (_rotationToAdd > 0)   //  Left
                {
                    _worldRotation += _rotationSpeed;
                    _rotationToAdd -= _rotationSpeed;

                    if (_rotationToAdd < _rotationSpeed)
                    {
                        _worldRotation += _rotationToAdd;
                        _rotationToAdd = 0f;
                        _levelRotating = false;
                    }
                }
                else  //  Right
                {
                    _worldRotation -= _rotationSpeed;
                    _rotationToAdd += _rotationSpeed;

                    if (_rotationToAdd > -_rotationSpeed)
                    {
                        _worldRotation += _rotationToAdd;
                        _rotationToAdd = 0f;
                        _levelRotating = false;
                    }
                }
            }
        }


        private void handleInput()
        {
            //  If the players dead, don't allow input.
            if (Player.Instance.PlayerState == PlayerState.Dead)
            {
                return;
            }

            //  Check if it's a rotating level and that rotation isn't 
            //  locked.
            if (_allowLevelRotation && _levelRotates)
            {
                //  If the room rotation delay cooldown timer is up
                //  check if the player wants to rotate.

                if (_rotateDelayTimer == 0.0f)
                {
                    if (InputManager.Instance.RotateLeft(true))
                    {
                        this.ForceRotateLeft();

                        this._levelRotating = true;
                        this._rotateDelayTimer = Defines.LEVEL_ROTATION_COOLDOWN;
                    }
                    else if (InputManager.Instance.RotateRight(true))
                    {
                        this.ForceRotateRight();

                        this._levelRotating = true;
                        this._rotateDelayTimer = Defines.LEVEL_ROTATION_COOLDOWN;
                    }
                }
            }
        }


        private void calculateZoom()
        {
            _currentCameraZoom = (_gameScreen.ScreenManager.GraphicsDevice.Viewport.Height * 0.5f) / ((_largestLevelDimension * 0.5f) * largestDimensionModifier);
        }



        #endregion



        #region Public Methods

        public void ChangeCamera(CameraType type)
        {
            _cameraType = type;
        }

        #region Global Camera Changes

        public void ForceRotateLeft()
        {
            _rotationToAdd -= MathHelper.PiOver2;

            switch (_upIs)
            {
                case UpIs.Up:
                    {
                        this.SetUpIs(UpIs.Right);
                        break;
                    }

                case UpIs.Left:
                    {
                        this.SetUpIs(UpIs.Up);
                        break;
                    }

                case UpIs.Down:
                    {
                        this.SetUpIs(UpIs.Left);
                        break;
                    }

                case UpIs.Right:
                    {
                        this.SetUpIs(UpIs.Down);
                        break;
                    }
            }

            Player.Instance.GetMainBody().ResetDynamics();
            AudioManager.Instance.PlayCue("Rotate_AntiClockwise", true);
        }

        public void ForceRotateRight()
        {
            _rotationToAdd += MathHelper.PiOver2;


            switch (_upIs)
            {
                case UpIs.Up:
                    {
                        this.SetUpIs(UpIs.Left);
                        break;
                    }

                case UpIs.Left:
                    {
                        this.SetUpIs(UpIs.Down);
                        break;
                    }

                case UpIs.Down:
                    {
                        this.SetUpIs(UpIs.Right);
                        break;
                    }

                case UpIs.Right:
                    {
                        this.SetUpIs(UpIs.Up);
                        break;
                    }
            }

            Player.Instance.GetMainBody().ResetDynamics();
            AudioManager.Instance.PlayCue("Rotate_Clockwise", true);
        }

        public void ForceRotateHalf()
        {
            _rotationToAdd += MathHelper.Pi;
            _levelRotating = true;
            _rotateDelayTimer = 2.0f;

            switch (_upIs)
            {
                case UpIs.Up:
                    {
                        this.SetUpIs(UpIs.Down);
                        break;
                    }

                case UpIs.Left:
                    {
                        this.SetUpIs(UpIs.Right);
                        break;
                    }

                case UpIs.Down:
                    {
                        this.SetUpIs(UpIs.Up);
                        break;
                    }

                case UpIs.Right:
                    {
                        this.SetUpIs(UpIs.Left);
                        break;
                    }
            }

            Player.Instance.GetMainBody().ResetDynamics();
        }

        #endregion

        #region Events

        public void ChangeLevelRotateAbility(bool rotate)
        {
            this._levelRotates = rotate;
        }

        #endregion

        #endregion

        #region Getters and Setters

        public float GetWorldRotation()
        {
            return _worldRotation;
        }

        public float GetLevelDiameter()
        {
            return _largestLevelDimension;
        }

        public Matrix GetTransformMatrix()
        {
            Matrix _transform = Matrix.CreateTranslation(new Vector3(-_cameraPosition, 0)) *
                                         Matrix.CreateRotationZ((float)_worldRotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3((new Vector2(_gameScreen.ScreenManager.GraphicsDevice.Viewport.Width, _gameScreen.ScreenManager.GraphicsDevice.Viewport.Height) * 0.5f), 0));
            return _transform;
        }

        public GameplayScreen GetGameplayScreen()
        {
            return this._gameScreen;
        }

        public void SetGameplayScreen(GameplayScreen screen)
        {
            this._gameScreen = screen;
        }

        public UpIs GetUpIs()
        {
            return this._upIs;
        }

        #endregion



        #region Development Only
#if Development
        private void ZoomModifierInput()
        {
            if (InputManager.Instance.GP_RB || InputManager.Instance.IsKeyPress(Keys.Home))
                Zoom -= 0.005f;
            else if (InputManager.Instance.GP_LB || InputManager.Instance.IsKeyPress(Keys.Insert))
                Zoom += 0.005f;
        }
#endif
        #endregion
    }
}
