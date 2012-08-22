//-------------------------------------------------------------------------------
//--    
//--    Input.cs
//--    
//--    Description:
//--    ============
//--    Handles all input from the user through properties.
//--    
//--    Revision list
//--    =============
//--    initial version
//--    BenG - Added an InputMethod bool to check what controlset it should
//--            be using.
//--    BenG - Added some minor bool returns at the bottom for more easily
//--           checking for calls like 'Jump' or 'Interact'.
//--    BenG - Added Xbox controller vibration support.
//--    BenG - If a gamepad is connected at startup, it's now default unless changed.
//--    
//--    Supports
//--    ========
//--    Gamepad, Keyboard and Mouse Input
//--    
//--    TBD
//--    ===
//--    
//--    
//-------------------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Threading;
using GameLibrary.GameLogic.Characters;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace GameLibrary.GameLogic.Controls
{
    internal class InputManager
    {
        #region Singleton
        private static InputManager _singleton = null;
        private static object _singletonLock = new object();
        public static InputManager Instance
        {
            get
            {
                if (InputManager._singleton == null)
                {
                    object obj;
                    Monitor.Enter(obj = InputManager._singletonLock);
                    try
                    {
                        InputManager._singleton = new InputManager();
                    }
                    finally
                    {
                        Monitor.Exit(obj);
                    }
                }
                return InputManager._singleton;
            }
        }

        private InputManager()
        {
        }
        #endregion

        #region Fields

        private KeyboardState _currentKeyboardState, _lastKeyboardState;
        private MouseState _currentMouseState, _lastMouseState;
        private GamePadState _currentGamepadState, _lastGamepadState;

        private Texture2D[] _buttonTextures = new Texture2D[7];

        private bool _isGamePad = false;

        public float VibrationTime { get; internal set; }
        public float VibrationIntensity { get; internal set;}
        #endregion

        #region Properties

        public bool IsGamepad
        {
            get
            {
                return _isGamePad;
            }
        }

        public Texture2D[] ButtonTextures
        {
            get
            {
                return _buttonTextures;
            }
        }
        /// <summary>
        /// A, B, X, Y, LT, RT, LStick
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Texture2D GetButtonTexture(int i)
        {
            return _buttonTextures[i];
        }

        #endregion

        public void Load(ContentManager content)
        {
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                this._isGamePad = true;
            }

            string path = "Assets/Other/Controls/";

            if (_isGamePad)
            {
                this._buttonTextures[0] = content.Load<Texture2D>(path + "xboxControllerButtonA");
                this._buttonTextures[1] = content.Load<Texture2D>(path + "xboxControllerButtonB");
                this._buttonTextures[2] = content.Load<Texture2D>(path + "xboxControllerButtonY");
                this._buttonTextures[3] = content.Load<Texture2D>(path + "xboxControllerButtonX");
                this._buttonTextures[4] = content.Load<Texture2D>(path + "xboxControllerLeftTrigger");
                this._buttonTextures[5] = content.Load<Texture2D>(path + "xboxControllerRightTrigger");
                this._buttonTextures[6] = content.Load<Texture2D>(path + "xboxControllerLeftThumbstick");
            }
            else
            {
                this._buttonTextures[0] = content.Load<Texture2D>(path + "key-space");
                this._buttonTextures[1] = content.Load<Texture2D>(path + "key-esc");
                this._buttonTextures[2] = content.Load<Texture2D>(path + "key-e");
                this._buttonTextures[3] = _buttonTextures[2];
                this._buttonTextures[4] = content.Load<Texture2D>(path + "key-left");
                this._buttonTextures[5] = content.Load<Texture2D>(path + "key-right");
                this._buttonTextures[6] = content.Load<Texture2D>(path + "key-wasd");
            }
        }

        #region Key Down Properties

        #region PC

        #region Function Keys
        public bool F12
        {
            get { return IsNewKeyPress(Keys.F12); }
        }

        public bool F11
        {
            get { return IsNewKeyPress(Keys.F11); }
        }

        public bool F10
        {
            get { return IsNewKeyPress(Keys.F10); }
        }

        public bool F9
        {
            get { return IsNewKeyPress(Keys.F9); }
        }

        public bool F8
        {
            get { return IsNewKeyPress(Keys.F8); }
        }

        public bool F7
        {
            get { return IsNewKeyPress(Keys.F7); }
        }

        public bool F6
        {
            get { return IsNewKeyPress(Keys.F6); }
        }

        public bool F5
        {
            get
            {
                return IsNewKeyPress(Keys.F5);
            }
        }

        public bool F4
        {
            get
            {
                return IsNewKeyPress(Keys.F4);
            }
        }

        public bool F3
        {
            get
            {
                return IsNewKeyPress(Keys.F3);
            }
        }

        public bool F2
        {
            get
            {
                return IsNewKeyPress(Keys.F2);
            }
        }

        public bool F1
        {
            get
            {
                return IsNewKeyPress(Keys.F1);
            }
        }

        
        public bool Quit
        {
            get
            {
                return (IsNewKeyPress(Keys.Escape));
            }
        }
        #endregion 

        #region Arrow Keys
        public bool Up
        {
            get
            {
                if (IsKeyPress(Keys.Up))
                {
                    return true;
                }
                else
                {
                    return false;
                }                
            }
        }

        public bool Down
        {
            get
            {
                if (IsKeyPress(Keys.Down))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }         

        public bool Left
        {
            get
            {
                
                if (IsKeyPress(Keys.Left))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            } 
        }       

        public bool Right
        {
            get
            {
                if (IsKeyPress(Keys.Right))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
        }
        #endregion

        #region Normal Keys
        public bool Enter
        {
            get
            {
                if (IsNewKeyPress(Keys.Enter))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Space
        {
            get
            {
                if(IsNewKeyPress(Keys.Space))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
        }         

        public bool Tab
        {
            get
            {
                if(IsNewKeyPress(Keys.Tab) )
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
        }

        public bool SHIFT_L
        {
            get
            {
                if (IsNewKeyPress(Keys.LeftShift) )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public bool SHIFT_R
        {
            get
            {
                if (IsKeyPress(Keys.RightShift))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public bool CTRL
        {
            get
            {
                if (IsNewKeyPress(Keys.LeftControl))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        
        public bool Escape
        {
            get
            {
                if (IsNewKeyPress(Keys.Escape))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }


        public bool One
        {
            get
            {
                if (IsKeyPress(Keys.NumPad1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Two
        {
            get
            {
                if (IsKeyPress(Keys.NumPad2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Three
        {
            get
            {
                if (IsKeyPress(Keys.NumPad3))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Four
        {
            get
            {
                if (IsKeyPress(Keys.NumPad4))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool E
        {
            get
            {
                if (IsNewKeyPress(Keys.E))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool W
        {
            get
            {
                if (IsKeyPress(Keys.W))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool A
        {
            get
            {
                if (IsKeyPress(Keys.A))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public bool S
        {
            get
            {
                if (IsKeyPress(Keys.S))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public bool D
        {
            get
            {
                if (IsKeyPress(Keys.D))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        #endregion

        #region Mouse Stuff

        public Vector2 MouseXY
        {
            get
            {
                return new Vector2(_currentMouseState.X, _currentMouseState.Y);
            }
        }

        public bool lMouseButton
        {
            get { return LMouseButtonPress(); }
        }

        public bool rMouseButton
        {
            get { return RMouseButtonPress(); }
        }

        #endregion 
        #endregion

        #region GamePad

        #region ABXY
        public bool GP_A
        {
            get
            {
                if (IsNewGpPress(Buttons.A))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool GP_B
        {
            get
            {
                if (IsNewGpPress(Buttons.B))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool GP_X
        {
            get
            {
                if (IsNewGpPress(Buttons.X))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool GP_Y
        {
            get
            {
                if (IsNewGpPress(Buttons.Y))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Triggers
        public bool GP_LB
        {
            get
            {
                if (IsGpPressed(Buttons.LeftShoulder))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool GP_LT
        {
            get
            {
                if (IsGpPressed(Buttons.LeftTrigger))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool GP_RB
        {
            get
            {
                if (IsGpPressed(Buttons.RightShoulder))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool GP_RT
        {
            get
            {
                if (IsGpPressed(Buttons.RightTrigger))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Others
        public bool GP_DPLeft
        {
            get
            {
                if (IsNewGpPress(Buttons.DPadLeft))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool GP_DPRight
        {
            get
            {
                if (IsNewGpPress(Buttons.DPadRight))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool GP_DPDown
        {
            get
            {
                if (IsNewGpPress(Buttons.DPadDown))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool GP_DPUp
        {
            get
            {
                if (IsNewGpPress(Buttons.DPadUp))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool GP_Start
        {
            get
            {
                if (IsNewGpPress(Buttons.Start))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool GP_Back
        {
            get
            {
                if (IsNewGpPress(Buttons.Back))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region Input Functions

        #region PC

        public bool IsKeyPress(Keys keys)
        {
            return _currentKeyboardState.IsKeyDown(keys);
        }

        public bool LMouseButtonPress()
        {
            return (_lastMouseState.LeftButton == ButtonState.Released) && (_currentMouseState.LeftButton == ButtonState.Pressed);
        }

        public bool RMouseButtonPress()
        {
            return (_lastMouseState.RightButton == ButtonState.Released) && (_currentMouseState.RightButton == ButtonState.Pressed);
        }

        public bool IsNewKeyPress(Keys keys)
        {
            return (_lastKeyboardState.IsKeyUp(keys)) && (_currentKeyboardState.IsKeyDown(keys));
        }

        #endregion

        #region Gamepad

        public Vector2 LeftThumbstick
        {
            get
            {
                return _currentGamepadState.ThumbSticks.Left;
            }
        }

        public Vector2 RightThumbstick
        {
            get
            {
                return _currentGamepadState.ThumbSticks.Right;
            }
        }

        public bool IsNewGpPress(Buttons btn)
        {
            return (_lastGamepadState.IsButtonUp(btn)) && (_currentGamepadState.IsButtonDown(btn));
        }

        public bool IsGpPressed(Buttons btn)
        {
            return _currentGamepadState.IsButtonDown(btn);
        }
        #endregion

        #endregion

        #region Update
        public void Update(float delta)
        {
            _lastGamepadState = _currentGamepadState;
            _currentGamepadState = GamePad.GetState(PlayerIndex.One);
            _lastKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _lastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            if (_isGamePad)
            {
                if (VibrationTime > 0)
                {
                    VibrationTime -= delta;
                    GamePad.SetVibration(PlayerIndex.One, VibrationIntensity, VibrationIntensity);
                }
                else
                {
                    VibrationTime = 0;
                    GamePad.SetVibration(PlayerIndex.One, 0, 0);
                }
            }
        }
        #endregion

        #region Gameplay

        public bool Interact(bool newPress)
        {
            if (Player.Instance.PlayerState == PlayerState.Dead)
            {
                return false;
            }

            if (_isGamePad)
            {
                if (newPress)
                {
                    return IsNewGpPress(Buttons.Y);
                }
                else
                {
                    return IsGpPressed(Buttons.Y);
                }
            }
            else
            {
                if (newPress)
                {
                    return IsNewKeyPress(Keys.E);
                }
                else
                {
                    return IsKeyPress(Keys.E);
                }
            }
        }

        public bool Jump(bool newPress)
        {
            if (_isGamePad)
            {
                if (newPress)
                {
                    return IsNewGpPress(Buttons.A);
                }
                else
                {
                    return IsGpPressed(Buttons.A);
                }
            }
            else
            {
                if (newPress)
                {
                    return IsNewKeyPress(Keys.Space);
                }
                else
                {
                    return IsKeyPress(Keys.Space);
                }
            }
        }

        public bool Grab(bool newPress)
        {
            if (_isGamePad)
            {
                if (newPress)
                {
                    return IsNewGpPress(Buttons.X);
                }
                else
                {
                    return IsGpPressed(Buttons.X);
                }
            }
            else
            {
                if (newPress)
                {
                    return IsNewKeyPress(Keys.E);
                }
                else
                {
                    return IsKeyPress(Keys.E);
                }
            }
        }

        public bool MoveLeft(bool newPress)
        {
            if (_isGamePad)
            {
                if (newPress)
                {
                    return IsNewGpPress(Buttons.LeftThumbstickLeft);
                }
                else
                {
                    return IsGpPressed(Buttons.LeftThumbstickLeft);
                }
            }
            else
            {
                if (this.D)
                {
                    return false;
                }

                if (newPress)
                {
                    return IsNewKeyPress(Keys.A);
                }
                else
                {
                    return IsKeyPress(Keys.A);
                }
            }
        }

        public bool MoveRight(bool newPress)
        {
            if (_isGamePad)
            {
                if (newPress)
                {
                    return IsNewGpPress(Buttons.LeftThumbstickRight);
                }
                else
                {
                    return IsGpPressed(Buttons.LeftThumbstickRight);
                }
            }
            else
            {
                if (this.A)
                {
                    return false;
                }

                if (newPress)
                {
                    return IsNewKeyPress(Keys.D);
                }
                else
                {
                    return IsKeyPress(Keys.D);
                }
            }
        }

        public bool MoveUp(bool newPress)
        {
            if (_isGamePad)
            {
                if (newPress)
                {
                    return IsNewGpPress(Buttons.LeftThumbstickUp);
                }
                else
                {
                    return (this.LeftThumbstick.Y >= 0.35f);
                }
            }
            else
            {
                if (this.S)
                {
                    return false;
                }

                if (newPress)
                {
                    return IsNewKeyPress(Keys.W);
                }
                else
                {
                    return IsKeyPress(Keys.W);
                }
            }
        }

        public bool MoveDown(bool newPress)
        {
            if (_isGamePad)
            {
                if (newPress)
                {
                    return IsNewGpPress(Buttons.LeftThumbstickDown);
                }
                else
                {
                    return (this.LeftThumbstick.Y <= -0.35f);
                }
            }
            else
            {
                if (this.W)
                {
                    return false;
                }

                if (newPress)
                {
                    return IsNewKeyPress(Keys.S);
                }
                else
                {
                    return IsKeyPress(Keys.S);
                }
            }
        }

        public bool RotateLeft(bool newPress)
        {
            if (_isGamePad)
            {
                if (newPress)
                {
                    return IsNewGpPress(Buttons.LeftTrigger);
                }
                else
                {
                    return IsGpPressed(Buttons.LeftTrigger);
                }
            }
            else
            {
                if (this.Right)
                {
                    return false;
                }

                if (newPress)
                {
                    return IsNewKeyPress(Keys.Left);
                }
                else
                {
                    return IsKeyPress(Keys.Left);
                }
            }
        }

        public bool RotateRight(bool newPress)
        {
            if (_isGamePad)
            {
                if (newPress)
                {
                    return IsNewGpPress(Buttons.RightTrigger);
                }
                else
                {
                    return IsGpPressed(Buttons.RightTrigger);
                }
            }
            else
            {
                if (IsKeyPress(Keys.Left))
                {
                    return false;
                }

                if (newPress)
                {
                    return IsNewKeyPress(Keys.Right);
                }
                else
                {
                    return IsKeyPress(Keys.Right);
                }
            }
        }

        #endregion

        #region Controller Vibrate
        /// <summary>
        /// Vibrate the xbox controller. Only works for the xbox controller
        /// </summary>
        /// <param name="time">How long in milliseconds</param>
        /// <param name="intensity">float between 0.0 and 1.0</param>
        public void VibrateGP(float time, float intensity)
        {
            //  Added this outside isGamePad check for development check incase you're
            //  using a PC input.
            if (intensity > 1.0f || intensity < 0.0f)   
                throw new Exception("Vibration intensity is too high. Needs to be between 0.0f and 1.0f");
            else
            {
                if (_isGamePad)
                {
                    VibrationTime = time;
                    VibrationIntensity = intensity;
                }
            }
        }
        #endregion
    }
}

