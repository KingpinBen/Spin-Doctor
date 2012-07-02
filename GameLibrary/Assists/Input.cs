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
using GameLibrary.Drawing;
using GameLibrary.Screens;
#endregion

namespace GameLibrary.Assists
{
    public static class Input
    {
        #region Fields

        private static KeyboardState _currentKeyboardState, _lastKeyboardState;
        private static MouseState _currentMouseState, _lastMouseState;
        private static GamePadState _currentGamepadState, _lastGamepadState;

        public static GamePadState CurrentGpState
        {
            get { return _currentGamepadState; }
        }

        public static MouseState CurrentMouseState
        {
            get { return _currentMouseState; }
        }

        public static KeyboardState CurrentKbState
        {
            get { return _currentKeyboardState; }
        }

        public static Vector2 Cursor { get; internal set; }

        public static float VibrationTime { get; internal set; }
        public static float VibrationIntensity { get; internal set;}

        public static bool isGamePad { get; set; }
        #endregion

        public static void Load()
        {
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
                isGamePad = true;
            else 
                isGamePad = false;
        }

        #region Key Down Properties
        #region PC

        #region Function Keys
        public static bool F12
        {
            get { return IsNewKeyPress(Keys.F12); }
        }

        public static bool F11
        {
            get { return IsNewKeyPress(Keys.F11); }
        }

        public static bool F10
        {
            get { return IsNewKeyPress(Keys.F10); }
        }

        public static bool F9
        {
            get { return IsNewKeyPress(Keys.F9); }
        }

        public static bool F8
        {
            get { return IsNewKeyPress(Keys.F8); }
        }

        public static bool F7
        {
            get { return IsNewKeyPress(Keys.F7); }
        }

        public static bool F6
        {
            get { return IsNewKeyPress(Keys.F6); }
        }

        public static bool F5
        {
            get
            {
                return IsNewKeyPress(Keys.F5);
            }
        }

        public static bool F4
        {
            get
            {
                return IsNewKeyPress(Keys.F4);
            }
        }

        public static bool F3
        {
            get
            {
                return IsNewKeyPress(Keys.F3);
            }
        }

        public static bool F2
        {
            get
            {
                return IsNewKeyPress(Keys.F2);
            }
        }

        public static bool F1
        {
            get
            {
                return IsNewKeyPress(Keys.F1);
            }
        }

        
        public static bool Quit
        {
            get
            {
                return (IsNewKeyPress(Keys.Escape));
            }
        }
        #endregion 

        #region Arrow Keys
        public static bool Up
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

        public static bool Down
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

        public static bool Left
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

        public static bool Right
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
        public static bool Enter
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

        public static bool Space
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

        public static bool Tab
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

        public static bool SHIFT_L
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

        public static bool SHIFT_R
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

        public static bool CTRL
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
        
        public static bool Escape
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


        public static bool One
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

        public static bool Two
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

        public static bool Three
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

        public static bool Four
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

        public static bool E
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

        public static bool W
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

        public static bool A
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

        public static bool S
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

        public static bool D
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

        public static Vector2 MouseXY
        {
            get
            {
                return new Vector2(_currentMouseState.X, _currentMouseState.Y);
            }
        }

        public static bool lMouseButton
        {
            get { return LMouseButtonPress(); }
        }

        public static bool rMouseButton
        {
            get { return RMouseButtonPress(); }
        }

        #endregion 
        #endregion

        #region GamePad

        #region ABXY
        public static bool GP_A
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

        public static bool GP_B
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

        public static bool GP_X
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

        public static bool GP_Y
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
        public static bool GP_LB
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
        public static bool GP_LT
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
        public static bool GP_RB
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
        public static bool GP_RT
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
        public static bool GP_DPLeft
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

        public static bool GP_DPRight
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

        public static bool GP_DPDown
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

        public static bool GP_DPUp
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

        public static bool GP_Start
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

        public static bool GP_Back
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

        public static bool IsKeyPress(Keys keys)
        {
            return _currentKeyboardState.IsKeyDown(keys);
        }

        public static bool LMouseButtonPress()
        {
            return (_lastMouseState.LeftButton == ButtonState.Released) && (_currentMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool RMouseButtonPress()
        {
            return (_lastMouseState.RightButton == ButtonState.Released) && (_currentMouseState.RightButton == ButtonState.Pressed);
        }

        public static bool IsNewKeyPress(Keys keys) //triggers when the key was NOT depressed during the last statecheck
        {
            return (_lastKeyboardState.IsKeyUp(keys)) && (_currentKeyboardState.IsKeyDown(keys));
        }

        #endregion

        #region Gamepad
        public static bool LeftThumbstick()
        {
            if (_currentGamepadState.ThumbSticks.Left != Vector2.Zero)
                return true;
            return false;
        }

        public static bool RightThumbstick()
        {
            if (_currentGamepadState.ThumbSticks.Right != Vector2.Zero)
                return true;
            return false;
        }

        public static Vector2 GP_LeftThumbstick
        {
            get { return _currentGamepadState.ThumbSticks.Left; }
        }

        public static Vector2 GP_RightThumbstick
        {
            get { return _currentGamepadState.ThumbSticks.Right; }
        }

        public static bool IsNewGpPress(Buttons btn)
        {
            return (_lastGamepadState.IsButtonUp(btn)) && (_currentGamepadState.IsButtonDown(btn));
        }

        public static bool IsGpPressed(Buttons btn)
        {
            return _currentGamepadState.IsButtonDown(btn);
        }
        #endregion

        #endregion

        #region Update
        public static void Update(GameTime gameTime)
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

        public static bool Interact()
        {
            if (isGamePad)
                return GP_Y;
            else
                return E;
        }

        public static bool Jump()
        {
            if (isGamePad)
                return GP_A;
            else
                return Space;
        }

        public static bool Menu()
        {
            if (isGamePad)
                return GP_Start;
            else
                return Escape;
        }

        public static bool Return()
        {
            if (isGamePad)
                return GP_B;
            else 
                return Escape;
        }

        public static bool MenuSelect()
        {
            if (isGamePad)
                return GP_A;
            else
                return lMouseButton;
        }

        public static bool Grab()
        {
            if (isGamePad)
                return GP_X;
            else
                return E;
        }

        public static bool LeftCheck()
        {
            if (isGamePad)
            {
                if (_currentGamepadState.ThumbSticks.Left.X < -0.5f)
                    return true;
                return false;
            }
            else
            {
                if (D) return false;
                return A;
            }
        }

        public static bool RightCheck()
        {
            if (isGamePad)
            {
                if (_currentGamepadState.ThumbSticks.Left.X > 0.5f)
                    return true;
                return false;
            }
            else
            {
                if (A) return false;
                return D;
            }
        }

        public static bool UpCheck()
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

        public static bool DownCheck()
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

        public static bool RotateLeft()
        {
            if (isGamePad)
                return GP_LT;
            else
                return Left;
        }

        public static bool RotateRight()
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
        public static void VibrateGP(float time, float intensity)
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

        public static string GetInputName()
        {
            if (isGamePad)
                return "Game Pad";
            return "Keyboard and Mouse";
        }
    }
}

