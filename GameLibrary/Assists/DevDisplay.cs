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
using GameLibrary.Screens;
using GameLibrary.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Objects;
using GameLibrary.Managers;
using FarseerPhysics.DebugViews;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using System.Diagnostics;
#endregion

namespace GameLibrary.Assists
{
    public static class DevDisplay
    {
        #region Fields/Variables

        private static DebugViewXNA _debugView;
        private const float MeterInPixels = 24f;

        private static int frameCounter;
        private static int frameRate;
        private static TimeSpan elapsedTime;
        private static World _world;

        #endregion

        public static void Load(World world)
        {
            LoadCreateDebugView(world);
            _world = world;
        }

        #region Update
        public static void Update(GameTime gameTime)
        {
            FPSCount(gameTime);
        }
        #endregion

        #region Draw

        public static void Draw(GameTime gameTime, Stopwatch stopWatch)
        {
            Draw_DebugData();

            // Increment every draw cycle to calculate the FPS
            frameCounter++;
            Console.Clear();

            Console.WriteLine("---------------------[ GENERAL ]---------------------");
            for (int i = 0; i < Screen_Manager.GetScreenCount(); i++)
            {
                Console.WriteLine("Screen [" + i + "] = [" + 
                    Screen_Manager.GetScreenName(i) + "]" + ". TT: "  + Screen_Manager.ScreenList[i].TransitionTime  + ". " + Screen_Manager.ScreenList[i].ScreenState);
            }

            Console.Write("FPS: " + frameRate.ToString());
            Console.WriteLine(".     SpriteCount: "  + Sprite_Manager.ListCount());
            Console.WriteLine("Gamepad Input?: " + Input.isGamePad.ToString());
            Console.WriteLine("---------------[ CAMERA & ROTATION ]----------------");
            Console.WriteLine("World Rotation: " + Camera.Rotation / Math.PI);
            Console.WriteLine("Rotating?: " + Camera.LevelRotating + ". CanRotate? " + Camera.LevelRotates);
            Console.Write("Up Is: " + Camera.UpIs.ToString());
            Console.WriteLine(".    Zoom: " + Camera.Zoom.ToString());
            Console.WriteLine("--------------------[ CONTROLS ]--------------------");
            Console.WriteLine("LeftThumbstick : " + new Vector2((float)Math.Round(Input.GP_LeftThumbstick.X, 2), (float)Math.Round(Input.GP_LeftThumbstick.Y, 2)).ToString());
            Console.WriteLine("ButtonsPushed  : " + Input.CurrentGpState.Buttons.ToString());
            Console.WriteLine("---------------------[ PLAYER ]---------------------");
            Console.WriteLine("BodyPosition: " + Player.Instance.Body.Position.ToString());
            Console.WriteLine("WheelPosition: " + Player.Instance.WheelBody.Position.ToString());
            Console.Write("State: " + Player.Instance.PlayerState.ToString());
            Console.Write("Jump: " + Player.Instance.CanJump.ToString()); Console.WriteLine(". DoubJump: " + Player.Instance.CanDoubleJump.ToString());
            Console.WriteLine("DrawTime: " + stopWatch.ElapsedMilliseconds / 1000.0f + "ms.");
        }
        #endregion

        #region FPS
        /// <summary>
        /// Updates the FPS reset check.
        /// </summary>
        /// <param name="gameTime"></param>
        private static void FPSCount(GameTime gameTime)
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
        private static void LoadCreateDebugView(World world)
        {
            //create and configure the debug view
            _debugView = new DebugViewXNA(world);
            _debugView.AppendFlags(DebugViewFlags.DebugPanel | DebugViewFlags.AABB | DebugViewFlags.PerformanceGraph | DebugViewFlags.ContactPoints | DebugViewFlags.Controllers);
            _debugView.DefaultShapeColor = Color.Blue;
            _debugView.SleepingShapeColor = Color.LightGray;
            _debugView.LoadContent(Screen_Manager.Graphics, Screen_Manager.Content);
        }

        private static void Draw_DebugData()
        {
            Matrix projection = Matrix.CreateOrthographicOffCenter(0f, Screen_Manager.Viewport.X / MeterInPixels,
                Screen_Manager.Viewport.Y / MeterInPixels, 0f, 0f, 1f);

            Matrix view =
                Matrix.CreateTranslation(new Vector3((-Camera.Position / MeterInPixels) - Camera.LevelOrigin, 0f)) *
                Matrix.CreateScale(Camera.Zoom, Camera.Zoom, 0f) *
                Matrix.CreateRotationZ(Camera.Rotation) *
                Matrix.CreateTranslation(new Vector3(((Screen_Manager.Viewport / 2) / MeterInPixels), 0f));

            _debugView.RenderDebugData(ref projection, ref view);
        }
        #endregion
    }
}
