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
#endregion

namespace GameLibrary.Drawing
{
    public static class Camera
    {
        #region Fields

        private const float RotationSpeed = 0.08f;
        private const float largestDimensionModifier = 0.9f;    //80% of largest dimension
        private const float _gravityForce = 120.0f;
        private static bool _levelRotates;
        private static bool _allowLevelRotation;
        private static bool _levelRotating;
        private static Vector2 _worldGravity;
        private static Vector2 _cameraPosition;
        private static float _worldRotation;
        private static float _currentCameraZoom;
        private static float _fullLevelZoom;
        private static float _rotationToAdd;
        private static float _rotateDelayTimer;
        private static float _largestLevelDimension;

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
        public static bool AllowRotation
        {
            get
            {
                if (!LevelRotates) return false;
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

        public static Vector2 WorldGravity
        {
            get
            {
                return _worldGravity;
            }
            set
            {
                _worldGravity = value;
            }
        }

        public static Vector2 LevelOrigin
        {
            get { return Vector2.Zero; }
        }

        public static Vector2 Position
        {
            get { return _cameraPosition; }
        }

        public static CameraType CameraType { get; internal set; }

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

        #region Zoom
        /// <summary>
        /// Global zoom.
        /// 
        /// Used for the transform by the gameplay SpriteBatches. 
        /// </summary>
        public static float Zoom
        {
            get { return _currentCameraZoom; }
            set
            {
                _currentCameraZoom = value;
            }
        }
        #endregion

        #region Level Size Zoom
        /// <summary>
        /// Holds the particular level target zoom. The normal zoom will probably
        /// or could change during the game/level so it's important for that it's held
        /// somewhere incase we need to lerp it.
        /// </summary>
        public static float FullLevelZoom
        {
            get
            {
                return _fullLevelZoom;
            }
            internal set
            {
                _fullLevelZoom = value;
            }
        }
        #endregion

        #region Rotation still to add
        public static float RotationToAdd
        {
            get { return _rotationToAdd; }
        }
        #endregion

        public static UpIs UpIs { get; internal set; }

        public static float LevelDiameter
        {
            get
            {
                return _largestLevelDimension;
            }
        }

        #endregion

        #region Load
        public static void Load(bool levelCanRotate, float LargestLevelDimension)
        {
            _levelRotates = levelCanRotate;
            AllowRotation = _levelRotates;
            UpIs = UpIs.Up;
            _largestLevelDimension = LargestLevelDimension;
            //  1000.0f needs to be either the levels width or height
            //  whichever is larger. Pass in by parameter.
            _currentCameraZoom = (Screen_Manager.Viewport.Y / 2.0f) / (LargestLevelDimension * largestDimensionModifier);
            _fullLevelZoom = Zoom;

            CameraType = CameraType.Free;
            _worldRotation = 0.0f;
            _cameraPosition = LevelOrigin;
            _rotateDelayTimer = 0;
        }
        #endregion

        #region Update
        public static void Update(GameTime gameTime)
        {
            HandleInput();

            HandleRotation(gameTime);

            if (CameraType == CameraType.Level && Position != LevelOrigin)
            {
                //  TODO:
                //  Chase vector from current camera position to Level Origin

                _cameraPosition = LevelOrigin;
            }
            else if (CameraType == CameraType.Focus)
            {
                //  TODO:
                //  Create a variablee for target focus.
                //  Chase vector from current camera position to Focus position.
                _cameraPosition = ConvertUnits.ToDisplayUnits(Player.Instance.Body.Position);
            }

            #region Free Camera (Development)
            else if (CameraType == CameraType.Free)
            {
                if (Input.GP_RightThumbstick.X != 0)
                    _cameraPosition += SpinAssist.ModifyVectorByUp(new Vector2(Input.GP_RightThumbstick.X * 10, 0));
                if (Input.GP_RightThumbstick.Y != 0)
                    _cameraPosition -= SpinAssist.ModifyVectorByUp(new Vector2(0, Input.GP_RightThumbstick.Y * 10));
            }

            #endregion
        }
        #endregion

        #region HandleInput
        /// <summary>
        /// All key presses that call functions should be in here.
        /// </summary>
        private static void HandleInput()
        {
            #region Development
#if Development
            ZoomModifierInput();

            //  Debug camera control
            if (Input.GP_Back)
            {
                switch (CameraType)
                {
                    case CameraType.Free:
                        CameraType = CameraType.Level;
                        break;
                    case CameraType.Level:
                        CameraType = CameraType.Focus;
                        break;
                }
            }
#endif
            #endregion

            if (Player.Instance.PlayerState == pState.Dead)
            {
                return;
            }

            if (!LevelRotates || !AllowRotation)
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

        #region HandleRotation
        private static void HandleRotation(GameTime gameTime)
        {
            //  No need to handle the rotation if the level can't 
            //  rotate.
            if (!_levelRotates) return;

            if (_rotateDelayTimer > 0) 
                _rotateDelayTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (_rotateDelayTimer < 0) 
                _rotateDelayTimer = 0;

            //  Nothing else needs to happen if theres 
            //  nothing to add.
            if (_rotationToAdd == 0)
            {
                _levelRotating = false;
                return;
            }

            if (_rotationToAdd > 0)   //  Left
            {
                _worldRotation += RotationSpeed;
                _rotationToAdd -= RotationSpeed;

                if (_rotationToAdd < RotationSpeed)
                {
                    _worldRotation += RotationToAdd;
                    _rotationToAdd = 0f;
                }
            }
            else if (_rotationToAdd < 0)  //  Right
            {
                _worldRotation -= RotationSpeed;
                _rotationToAdd += RotationSpeed;

                if (_rotationToAdd > -RotationSpeed)
                {
                    _worldRotation += RotationToAdd;
                    _rotationToAdd = 0f;
                }
            }
        }
        #endregion

        #region TransformMatrix
        // View Matrix
        public static Matrix TransformMatrix()
        {
            Matrix _transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                                         Matrix.CreateRotationZ((float)_worldRotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(Screen_Manager.Viewport / 2, 0));
            return _transform;
        }
        #endregion

        #region ZoomModifieringInput
        /// <summary>
        /// Dev Only
        /// </summary>
        private static void ZoomModifierInput()
        {
            if (Input.GP_RB)
                Zoom -= 0.005f;
            else if (Input.GP_LB)
                Zoom += 0.005f;
        }
        #endregion

        public static void ChangeCamera(CameraType type)
        {
            CameraType = type;
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
        #endregion
    }
}
