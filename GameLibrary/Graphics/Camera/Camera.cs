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
//--    TBD
//--    ==============
//--    (?)Move rotation out into Gameplay for gameglobal accessibility.
//--    Make it so objects are able to rotate the level.
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
#endregion

namespace GameLibrary.Graphics.Camera
{
    public class Camera
    {
        private static Camera _singleton = null;
        public static Camera Instance
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new Camera();
                }

                return _singleton;
            }
        }

        #region Fields

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
        private UpIs _upIs;
        private CameraType _cameraType;
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

        public float RotateDelayTimer
        {
            get
            {
                return _rotateDelayTimer;
            }
        }

        /// <summary>
        /// Current world rotation.
        /// 
        /// Used by the world gravity.
        /// </summary>
        public float Rotation
        {
            get
            {
                return _worldRotation;
            }
        }

        public CameraType CameraType
        {
            get
            {
                return _cameraType;
            }
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

        public UpIs UpIs
        {
            get
            {
                return _upIs;
            }
            set
            {
                _upIs = value;
                ChangeGravity();
            }
        }

        public float LevelDiameter
        {
            get
            {
                return _largestLevelDimension;
            }
        }

        #endregion

        private Camera()
        {
        }

        public void Load(bool levelCanRotate, float LargestLevelDimension)
        {
            this._levelRotates = levelCanRotate;
            this.AllowRotation = _levelRotates;
            this.UpIs = UpIs.Up;
            this._largestLevelDimension = LargestLevelDimension;
            this.CalculateZoom();
            this._fullLevelZoom = Zoom;

            this._cameraType = CameraType.Level;
            this._worldRotation = 0.0f;
            this._cameraPosition = Vector2.Zero;
            this._rotateDelayTimer = 0;
            this._rotationToAdd = 0.0f;
            
        }

        public void Update(GameTime gameTime)
        {
            if (_cameraType == CameraType.Level && Position != Vector2.Zero)
            {
                //  TODO:
                //  Chase vector from current camera position to Level Origin

                _cameraPosition = Vector2.Zero;
            }
            else if (_cameraType == CameraType.Focus)
            {
                //  TODO:
                //  Create a variablee for target focus.
                //  Chase vector from current camera position to Focus position.
                _cameraPosition = ConvertUnits.ToDisplayUnits(Player.Instance.Body.Position);
            }
#if Development
            else if (_cameraType == CameraType.Free)
            {
                if (InputManager.Instance.GP_RightThumbstick.X != 0)
                    _cameraPosition += SpinAssist.ModifyVectorByUp(new Vector2(InputManager.Instance.GP_RightThumbstick.X * 10, 0));
                if (InputManager.Instance.GP_RightThumbstick.Y != 0)
                    _cameraPosition -= SpinAssist.ModifyVectorByUp(new Vector2(0, InputManager.Instance.GP_RightThumbstick.Y * 10));
            }

            ZoomModifierInput();

            //  Debug camera control
            if (InputManager.Instance.GP_Back)
            {
                switch (_cameraType)
                {
                    case CameraType.Free:
                        _cameraType = CameraType.Level;
                        break;
                    case CameraType.Level:
                        _cameraType = CameraType.Focus;
                        break;
                }
            }
#endif
                HandleInput();
                HandleRotation(gameTime);
        }

        #region Private Methods

        private void ZoomModifierInput()
        {
            if (InputManager.Instance.GP_RB || InputManager.Instance.IsKeyPress(Keys.Home))
                Zoom -= 0.005f;
            else if (InputManager.Instance.GP_LB || InputManager.Instance.IsKeyPress(Keys.Insert))
                Zoom += 0.005f;
        }

        private void ChangeGravity()
        {
            switch (UpIs)
            {
                case UpIs.Up:
                    {
                        GameplayScreen.World.Gravity = new Vector2(0, _gravityForce);
                        break;
                    }
                case UpIs.Down:
                    {
                        GameplayScreen.World.Gravity = new Vector2(0, -_gravityForce);
                        break;
                    }
                case UpIs.Left:
                    {
                        GameplayScreen.World.Gravity = new Vector2(_gravityForce, 0);
                        break;
                    }
                case UpIs.Right:
                    {
                        GameplayScreen.World.Gravity = new Vector2(-_gravityForce, 0);
                        break;
                    }
            }
        }

        private void HandleRotation(GameTime gameTime)
        {
            if (_rotateDelayTimer > 0)
            {
                _rotateDelayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (_rotateDelayTimer < 0)
            {
                _rotateDelayTimer = 0;
            }

            //  Nothing else needs to happen if theres 
            //  nothing to add.
            if (_rotationToAdd == 0)
            {
                _levelRotating = false;
                return;
            }

            if (_rotationToAdd > 0)   //  Left
            {
                _worldRotation += _rotationSpeed;
                _rotationToAdd -= _rotationSpeed;

                if (_rotationToAdd < _rotationSpeed)
                {
                    _worldRotation += _rotationToAdd;
                    _rotationToAdd = 0f;
                }
            }
            else if (_rotationToAdd < 0)  //  Right
            {
                _worldRotation -= _rotationSpeed;
                _rotationToAdd += _rotationSpeed;

                if (_rotationToAdd > -_rotationSpeed)
                {
                    _worldRotation += _rotationToAdd;
                    _rotationToAdd = 0f;
                }
            }
        }

        private void HandleInput()
        {
            if (Player.Instance.PlayerState == PlayerState.Dead)
            {
                return;
            }

            if (!AllowRotation || !_levelRotates)
            {
                return;
            }

            if (InputManager.Instance.RotateLeft() != InputManager.Instance.RotateRight())
            {
                if (InputManager.Instance.RotateLeft())
                {
                    ForceRotateLeft();
                    
                }
                else if (InputManager.Instance.RotateRight())
                {
                    ForceRotateRight();
                }
            }
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
            if (_rotateDelayTimer > 0.0f)
            {
                return;
            }

            _rotationToAdd -= MathHelper.PiOver2;
            _levelRotating = true;
            _rotateDelayTimer = 2.0f;

            switch (UpIs)
            {
                case UpIs.Up:
                    {
                        UpIs = UpIs.Right;
                        break;
                    }

                case UpIs.Left:
                    {
                        UpIs = UpIs.Up;
                        break;
                    }

                case UpIs.Down:
                    {
                        UpIs = UpIs.Left;
                        break;
                    }

                case UpIs.Right:
                    {
                        UpIs = UpIs.Down;
                        break;
                    }
            }

            ChangeGravity();
            Player.Instance.Body.ResetDynamics();
        }

        public void ForceRotateRight()
        {
            if (_rotateDelayTimer > 0.0f)
            {
                return;
            }
            

            _rotationToAdd += MathHelper.PiOver2;
            _levelRotating = true;
            _rotateDelayTimer = 2.0f;

            switch (UpIs)
            {
                case UpIs.Up:
                    {
                        UpIs = UpIs.Left;
                        break;
                    }

                case UpIs.Left:
                    {
                        UpIs = UpIs.Down;
                        break;
                    }

                case UpIs.Down:
                    {
                        UpIs = UpIs.Right;
                        break;
                    }

                case UpIs.Right:
                    {
                        UpIs = UpIs.Up;
                        break;
                    }
            }

            ChangeGravity();
            Player.Instance.Body.ResetDynamics();
        }

        public void ForceRotateHalf()
        {
            if (_rotateDelayTimer > 0.0f)
            {
                return;
            }

            _rotationToAdd += MathHelper.Pi;
            _levelRotating = true;
            _rotateDelayTimer = 2.0f;

            switch (UpIs)
            {
                case UpIs.Up:
                    {
                        UpIs = UpIs.Down;
                        break;
                    }

                case UpIs.Left:
                    {
                        UpIs = UpIs.Right;
                        break;
                    }

                case UpIs.Down:
                    {
                        UpIs = UpIs.Up;
                        break;
                    }

                case UpIs.Right:
                    {
                        UpIs = UpIs.Left;
                        break;
                    }
            }

            ChangeGravity();
            Player.Instance.Body.ResetDynamics();
        }

        #endregion

        public void CalculateZoom()
        {
            _currentCameraZoom = (ScreenManager.GraphicsDevice.Viewport.Height * 0.5f) / (_largestLevelDimension * largestDimensionModifier);
        }

        public Matrix TransformMatrix()
        {
            Matrix _transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                                         Matrix.CreateRotationZ((float)_worldRotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3((new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height) * 0.5f), 0));
            return _transform;
        }

        #endregion
    }
}
