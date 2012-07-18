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
#endregion

namespace GameLibrary.GameLogic.Controls
{
    internal class InputManager
    {
        #region Fields

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

        private KeyboardState _currentKeyboardState, _lastKeyboardState;
        private MouseState _currentMouseState, _lastMouseState;
        private GamePadState _currentGamepadState, _lastGamepadState;

        public GamePadState CurrentGpState
        {
            get { return _currentGamepadState; }
        }

        public MouseState CurrentMouseState
        {
            get { return _currentMouseState; }
        }

        public KeyboardState CurrentKbState
        {
            get { return _currentKeyboardState; }
        }

        public Vector2 Cursor { get; internal set; }

        public float VibrationTime { get; internal set; }
        public float VibrationIntensity { get; internal set;}

        public bool isGamePad { get; set; }
        #endregion

        private InputManager()
        {
        }

        public void Load()
        {
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
                isGamePad = true;
            else 
                isGamePad = false;
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
                //return IsKeyPress(Keys.Tab);
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
                //return IsKeyPress(Keys.Tab);
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
                //return IsKeyPress(Keys.Tab);
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
                //return IsKeyPress(Keys.Tab);
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

        public bool IsNewKeyPress(Keys keys) //triggers when the key was NOT depressed during the last statecheck
        {
            return (_lastKeyboardState.IsKeyUp(keys)) && (_currentKeyboardState.IsKeyDown(keys));
        }

        #endregion

        #region Gamepad
        public bool LeftThumbstick()
        {
            if (_currentGamepadState.ThumbSticks.Left != Vector2.Zero)
                return true;
            return false;
        }

        public bool RightThumbstick()
        {
            if (_currentGamepadState.ThumbSticks.Right != Vector2.Zero)
                return true;
            return false;
        }

        public Vector2 GP_LeftThumbstick
        {
            get { return _currentGamepadState.ThumbSticks.Left; }
        }

        public Vector2 GP_RightThumbstick
        {
            get { return _currentGamepadState.ThumbSticks.Right; }
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
        public void Update(GameTime gameTime)
        {
            if (isGamePad)
            {
                _lastGamepadState = _currentGamepadState;
                _currentGamepadState = GamePad.GetState(PlayerIndex.One);

                if (VibrationTime > 0)
                {
                    VibrationTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    GamePad.SetVibration(PlayerIndex.One, VibrationIntensity, VibrationIntensity);
                }
                else
                {
                    VibrationTime = 0;
                    GamePad.SetVibration(PlayerIndex.One, 0, 0);
                }
            }
            else
            {
                _lastKeyboardState = _currentKeyboardState;
                _currentKeyboardState = Keyboard.GetState();

                _lastMouseState = _currentMouseState;
                _currentMouseState = Mouse.GetState();

                Cursor = new Vector2(_currentMouseState.X, _currentMouseState.Y);
            }
        }
        #endregion

        #region Gameplay

        public bool Interact()
        {
            if (isGamePad)
            {
                return GP_Y;
            }
            else
            {
                return E;
            }
        }

        public bool Jump()
        {
            if (isGamePad)
            {
                return GP_A;
            }
            else
            {
                return Space;
            }
        }

        public bool Menu()
        {
            if (isGamePad)
            {
                return GP_Start;
            }
            else
            {
                return Escape;
            }
        }

        public bool Return()
        {
            if (isGamePad)
            {
                return GP_B;
            }
            else
            {
                return Escape;
            }
        }

        public bool MenuSelect()
        {
            if (isGamePad)
            {
                return GP_A;
            }
            else
            {
                return Enter;
            }
        }

        public bool Grab()
        {
            if (isGamePad)
            {
                return GP_X;
            }
            else
            {
                return E;
            }
        }

        public bool LeftCheck()
        {
            if (isGamePad)
            {
                if (_currentGamepadState.ThumbSticks.Left.X < -0.5f)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if (this.D)
                {
                    return false;
                }

                return A;
            }
        }

        public bool RightCheck()
        {
            if (isGamePad)
            {
                if (_currentGamepadState.ThumbSticks.Left.X > 0.5f)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (this.A)
                {
                    return false;
                }
                return D;
            }
        }

        public bool UpCheck()
        {
            if (isGamePad)
            {
                if (_currentGamepadState.ThumbSticks.Left.Y > 0.5f)
                    return true;
                return false;
            }
            else
            {
                if (S) return false;
                return W;
            }
        }

        public bool DownCheck()
        {
            if (isGamePad)
            {
                if (_currentGamepadState.ThumbSticks.Left.Y < -0.5f)
                    return true;
                return false;
            }
            else
            {
                if (W) return false;
                return S;
            }
        }

        public bool RotateLeft()
        {
            if (isGamePad)
                return GP_LT;
            else
                return Left;
        }

        public bool RotateRight()
        {
            if (isGamePad)
                return GP_RT;
            else
                return Right;
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
                if (isGamePad)
                {
                    VibrationTime = time;
                    VibrationIntensity = intensity;
                }
            }
        }
        #endregion

        public string GetInputName()
        {
            if (isGamePad)
                return "Game Pad";
            return "Keyboard and Mouse";
        }
    }
}

