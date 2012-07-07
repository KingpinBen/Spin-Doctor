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
using GameLibrary.Objects;
using GameLibrary.Assists;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Managers;
using GameLibrary.Screens;
using Microsoft.Xna.Framework.Input;
#endregion

namespace GameLibrary.Drawing
{
    public static class Camera
    {
        #region Fields

        private const float _rotationSpeed = 0.08f;
        //  90% of largest dimension
        private const float largestDimensionModifier = 0.9f;    
        private const float _gravityForce = 120.0f;
        private static bool _levelRotates;
        private static bool _allowLevelRotation;
        private static bool _levelRotating;
        private static Vector2 _worldGravity;
        private static Vector2 _cameraPosition;
        private static float _worldRotation;
        private static float _currentCameraZoom;
        //  Hold at what float zooms to show the whole level.
        private static float _fullLevelZoom;
        private static float _rotationToAdd;
        private static float _rotateDelayTimer;
        private static float _largestLevelDimension;
        private static UpIs _upIs;
        private static CameraType _cameraType;
        #endregion

        #region Properties

        #region Does Level Rotate?
        /// <summary>
        /// Is the level able to rotate?
        /// 
        /// If not, the input cannot be used to rotate it.
        /// </summary>
        public static bool LevelRotates
        {
            get
            {
                return _levelRotates;
            }
        }
        #endregion

        #region Allow Rotation
        /// <summary>
        /// Has rotating the level been temporarily disabled?
        /// 
        /// E.g. climbing the ladder
        /// </summary>
        public static bool AllowRotation
        {
            get
            {
                if (!LevelRotates) 
                    return false;
                return _allowLevelRotation;
            }
            set
            {
                _allowLevelRotation = value;
            }
        }
        #endregion

        #region Is Level rotating?
        /// <summary>
        /// Is the level currently rotating?
        /// 
        /// Used for awakening some objects (like ropes) 
        /// incase they fall asleep and don't wake.
        /// </summary>
        public static bool LevelRotating
        {
            get { return _levelRotating; }
        }
        #endregion

        public static Vector2 Position
        {
            get { return _cameraPosition; }
        }

        #region Able to rotate Delay
        /// <summary>
        /// Delay timer for the rotation.
        /// 
        /// While this is >1.0f, the player can't rotate
        /// </summary>
        public static float RotateDelayTimer
        {
            get { return _rotateDelayTimer; }
        }
        #endregion

        #region Current Rotation
        /// <summary>
        /// Current world rotation.
        /// 
        /// Used by the world gravity.
        /// </summary>
        public static float Rotation
        {
            get { return _worldRotation; }
        }
        #endregion

        public static CameraType CameraType
        {
            get
            {
                return _cameraType;
            }
        }

        #region Zoom
        /// <summary>
        /// Global zoom.
        /// 
        /// Used for the transform by the gameplay SpriteBatches. 
        /// </summary>
        public static float Zoom
        {
            get { return _currentCameraZoom; }
            internal set
            {
                _currentCameraZoom = MathHelper.Clamp(value, 0.1f, 4.0f);
            }
        }
        #endregion

        public static UpIs UpIs
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

        public static float LevelDiameter
        {
            get
            {
                return _largestLevelDimension;
            }
        }

        #endregion

        public static void Load(bool levelCanRotate, float LargestLevelDimension)
        {
            _levelRotates = levelCanRotate;
            AllowRotation = _levelRotates;
            UpIs = UpIs.Up;
            _largestLevelDimension = LargestLevelDimension;
            CalculateZoom();
            _fullLevelZoom = Zoom;
            
            _cameraType = CameraType.Free;
            _worldRotation = 0.0f;
            _cameraPosition = Vector2.Zero;
            _rotateDelayTimer = 0;
        }

        public static void Update(GameTime gameTime)
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
                if (Input.GP_RightThumbstick.X != 0)
                    _cameraPosition += SpinAssist.ModifyVectorByUp(new Vector2(Input.GP_RightThumbstick.X * 10, 0));
                if (Input.GP_RightThumbstick.Y != 0)
                    _cameraPosition -= SpinAssist.ModifyVectorByUp(new Vector2(0, Input.GP_RightThumbstick.Y * 10));
            }

            ZoomModifierInput();

            //  Debug camera control
            if (Input.GP_Back)
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

            if (_levelRotates)
            {
                HandleInput();
                HandleRotation(gameTime);
            }
        }

        #region Private Methods

        static void ZoomModifierInput()
        {
            if (Input.GP_RB || Input.IsKeyPress(Keys.Home))
                Zoom -= 0.005f;
            else if (Input.GP_LB || Input.IsKeyPress(Keys.Insert))
                Zoom += 0.005f;
        }

        static void ChangeGravity()
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

        static void HandleRotation(GameTime gameTime)
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

        static void HandleInput()
        {
            if (Player.Instance.PlayerState == pState.Dead)
            {
                return;
            }

            if (!AllowRotation)
            {
                return;
            }

            if (Input.RotateLeft() != Input.RotateRight())
            {
                if (Input.RotateLeft())
                {
                    ForceRotateLeft();
                    
                }
                else if (Input.RotateRight())
                {
                    ForceRotateRight();
                }
            }
        }

        #endregion

        #region Public Methods

        public static void ChangeCamera(CameraType type)
        {
            _cameraType = type;
        }

        #region Global Camera Changes

        public static void ForceRotateLeft()
        {
            if (_rotateDelayTimer > 0.0f) return;

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

        public static void ForceRotateRight()
        {
            if (_rotateDelayTimer > 0.0f) return;
            

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

        public static void ForceRotateHalf()
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

        public static void CalculateZoom()
        {
            _currentCameraZoom = (Screen_Manager.GraphicsDevice.Viewport.Height * 0.5f) / (_largestLevelDimension * largestDimensionModifier);
        }

        public static Matrix TransformMatrix()
        {
            Matrix _transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                                         Matrix.CreateRotationZ((float)_worldRotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3((new Vector2(Screen_Manager.GraphicsDevice.Viewport.Width, Screen_Manager.GraphicsDevice.Viewport.Height) * 0.5f), 0));
            return _transform;
        }

        #endregion
    }
}
