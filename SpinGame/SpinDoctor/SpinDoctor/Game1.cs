//--------------------------------------------------------------------------
//--    
//--    Spin Doctor
//--    
//--    
//--    Description
//--    ===============
//--    Basic Game handler
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial 
//--    
//--    
//--    
//--    TBD
//--    ==============
//--    The rest of it, durrh
//--    
//--    
//--------------------------------------------------------------------------

//#define ShowStartUp

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Helpers;
using GameLibrary.GameLogic;
using GameLibrary.GameLogic.Screens;
using System.Windows.Forms;
using GameLibrary.GameLogic.Screens.Menu;
using GameLibrary.System;

#endregion

namespace SpinDoctor
{
    public class Game1 : Game
    {
        #region Fields
        private GraphicsDeviceManager _graphics;

        ScreenManager screenManager;

        bool _skipStartup = false;

        #endregion

        public Game1()
        {
            this.Content.RootDirectory = "Content";
            this.IsFixedTimeStep = false;
            this.Window.Title = "Spin Doctor Beta";

            //  Setup the graphics manager
            this._graphics = new GraphicsDeviceManager(this);
            this._graphics.PreferredBackBufferWidth = 1280;
            this._graphics.PreferredBackBufferHeight = 720;
            this._graphics.PreferMultiSampling = true;
            this._graphics.SynchronizeWithVerticalRetrace = true;
            this._graphics.ApplyChanges();
            
            ConvertUnits.SetDisplayUnitToSimUnitRatio(24f);

            // Create the screen manager component.
            screenManager = new ScreenManager(this, this._graphics);
            screenManager.SkipStartup = _skipStartup;
            Components.Add(screenManager);
        }

        protected override void Initialize()
        {
            GameSettings instance = GameSettings.Instance;

            SaveManager.Instance.LoadSettings();

            ResolutionData resolution;
            resolution.Width = _graphics.PreferredBackBufferWidth;
            resolution.Height = _graphics.PreferredBackBufferHeight;
            resolution.Fullscreen = _graphics.IsFullScreen;


            if (!instance.Resolution.CompareTo(resolution))
            {
                this._graphics.PreferredBackBufferWidth = instance.Resolution.Width;
                this._graphics.PreferredBackBufferHeight = instance.Resolution.Height;
                this._graphics.IsFullScreen = instance.Resolution.Fullscreen;
                this._graphics.PreferMultiSampling = instance.MultiSamplingEnabled;

                this._graphics.ApplyChanges();
            }

            base.Initialize();
        }

        protected override void LoadContent() { }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }

        public void SetArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i].ToLowerInvariant();

                if (args[i].Contains("-dev"))
                {
                    GameSettings.Instance.SetDevelopment(this, true);
                }
                else if (args[i].Contains("-SKIPSTARTUP"))
                {
                    _skipStartup = true;
                }
            }
        }
    }
}
