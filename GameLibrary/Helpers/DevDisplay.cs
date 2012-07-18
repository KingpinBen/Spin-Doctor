//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - DevDisplay
//--    
//--    
//--    
//--    Description
//--    ===============
//--    Shows some debugging tools ingame when defined.
//--    Not all that important to keep up to date...
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Dynamically changes locations depending on how much it should
//--           show. Lists all screens in List.
//--    BenG - Added an FPS counter
//--    
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.DebugViews;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using System.Diagnostics;
using GameLibrary.Graphics.Camera;
using GameLibrary.GameLogic.Controls;
using GameLibrary.GameLogic;
using GameLibrary.GameLogic.Screens;
using GameLibrary.Graphics;
#endregion

namespace GameLibrary.Helpers
{
    internal class DevDisplay
    {
        #region Fields/Variables

        private static DevDisplay _singleton = null;
        public static DevDisplay Instance
        {
            get
            {
                return _singleton;
            }
        }

        private DebugViewXNA _debugView;
        private const float MeterInPixels = 24f;

        private int frameCounter;
        private int frameRate;
        private int averageFPS;
        private TimeSpan elapsedTime;
        private World _world;

        public float FPS
        {
            get
            {
                return frameRate;
            }
        }

        #endregion

        private DevDisplay(World world)
        {
            LoadCreateDebugView(world);
            _world = world;
        }

        public static void Load(World world)
        {
            _singleton = new DevDisplay(world);
        }

        #region Update
        public void Update(GameTime gameTime)
        {
            FPSCount(gameTime);
        }
        #endregion

        #region Draw

        public void Draw(GameTime gameTime, Stopwatch stopWatch)
        {
            Draw_DebugData();

            // Increment every draw cycle to calculate the FPS
            frameCounter++;
            Console.Clear();

            Console.WriteLine("---------------------[ GENERAL ]---------------------");
            for (int i = 0; i < ScreenManager.ScreenList.Count; i++)
            {
                Console.WriteLine("Screen [" + i + "] = [" + 
                    ScreenManager.ScreenList[i].Name + "]" + ".");
            }
            Console.WriteLine("---------------------[ RENDER ]---------------------");

            Console.Write("FPS: " + frameRate.ToString() + ".  SpriteCount: " + SpriteManager.Instance.ListCount());
            Console.WriteLine("DrawTime: " + stopWatch.ElapsedMilliseconds + "ms.");
            
            Console.WriteLine("---------------[ CAMERA & ROTATION ]----------------");
            Console.WriteLine("World Rotation: " + Camera.Instance.Rotation / Math.PI + ". Up is: " + Camera.Instance.UpIs.ToString());
            Console.WriteLine("Rotating?: " + Camera.Instance.IsLevelRotating + ". CanRotate? " + Camera.Instance.LevelRotates);
            Console.WriteLine("Zoom: " + Camera.Instance.Zoom.ToString());

            Console.WriteLine("--------------------[ CONTROLS ]--------------------"); 
            Console.Write("Gamepad Input?: " + InputManager.Instance.isGamePad.ToString());
            if (InputManager.Instance.isGamePad)
            {
                Console.WriteLine(". LeftThumbstick : " + new Vector2((float)Math.Round(InputManager.Instance.GP_LeftThumbstick.X, 2), (float)Math.Round(InputManager.Instance.GP_LeftThumbstick.Y, 2)).ToString());
                Console.WriteLine("ButtonsPushed  : " + InputManager.Instance.CurrentGpState.Buttons.ToString());
            }
            else
            {
                Console.WriteLine(".  Buttons pushed: " + InputManager.Instance.CurrentKbState.GetPressedKeys().ToString());
            }

            Console.WriteLine("---------------------[ PLAYER ]---------------------");
            Console.WriteLine("Position: " + Player.Instance.Body.Position.ToString());
            Console.Write("State: " + Player.Instance.PlayerState.ToString());
            Console.WriteLine("Jump: " + Player.Instance.CanJump.ToString() + ". DoubJump: " + Player.Instance.CanDoubleJump.ToString());
            Console.WriteLine("---------------------[ PHYSICS ]---------------------");
            Console.Write("Bodies: " + GameplayScreen.World.Island.BodyCount + ". Contacts" + GameplayScreen.World.Island.ContactCount);
        }
        #endregion

        #region FPS
        /// <summary>
        /// Updates the FPS reset check.
        /// </summary>
        /// <param name="gameTime"></param>
        private void FPSCount(GameTime gameTime)
        {
            
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }
        #endregion

        #region Farseer Debug
        void LoadCreateDebugView(World world)
        {
            //create and configure the debug view
            _debugView = new DebugViewXNA(world);
            _debugView.AppendFlags(DebugViewFlags.DebugPanel | DebugViewFlags.AABB | DebugViewFlags.PerformanceGraph | DebugViewFlags.ContactPoints | DebugViewFlags.Controllers);
            _debugView.DefaultShapeColor = Color.Blue;
            _debugView.SleepingShapeColor = Color.LightGray;
            _debugView.LoadContent(ScreenManager.GraphicsDevice, ScreenManager.Content);
        }

        void Draw_DebugData()
        {
            Matrix projection = Matrix.CreateOrthographicOffCenter(0f, ScreenManager.GraphicsDevice.Viewport.Width / MeterInPixels,
                ScreenManager.GraphicsDevice.Viewport.Height / MeterInPixels, 0f, 0f, 1f);

            Matrix view =
                Matrix.CreateTranslation(new Vector3((-Camera.Instance.Position / MeterInPixels), 0f)) *
                Matrix.CreateScale(Camera.Instance.Zoom, Camera.Instance.Zoom, 0f) *
                Matrix.CreateRotationZ(Camera.Instance.Rotation) *
                Matrix.CreateTranslation(new Vector3(((new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height) * 0.5f) / MeterInPixels), 0f));
            _debugView.RenderDebugData(ref projection, ref view);
        }
        #endregion
    }
}
