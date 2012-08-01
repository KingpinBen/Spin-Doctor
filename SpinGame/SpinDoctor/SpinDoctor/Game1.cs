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

#endregion

namespace SpinDoctor
{
    public class Game1 : Game
    {
        #region Fields
        private GraphicsDeviceManager _graphics;

        ScreenManager screenManager;

        uint _startLevel = 0;
        int _backbufferWidth = 1280;
        int _backbufferHeight = 720;
        bool _fullScreen = false;
        //  TODO: Change for non-dev
        bool _showStartup = false;

        #endregion

        public Game1()
        {
            this.IsMouseVisible = true;
            this.Content.RootDirectory = "Content";
            this.IsFixedTimeStep = true;
            this.Window.Title = "Spin Doctor - Development";

            _graphics = new GraphicsDeviceManager(this);
            
            ConvertUnits.SetDisplayUnitToSimUnitRatio(24f);

            // Create the screen manager component.
            screenManager = new ScreenManager(this, this._graphics);
            Components.Add(screenManager);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = _backbufferHeight;
            _graphics.PreferredBackBufferWidth = _backbufferWidth;
            _graphics.PreferMultiSampling = true;
            _graphics.IsFullScreen = _fullScreen;
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.ApplyChanges();

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += ChangeWindowSize;


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

        void ChangeWindowSize(object sender, EventArgs e)
        {
            if (Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0)
            {
                _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            }

            _graphics.ApplyChanges();
        }

        public void SetArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                //args[i].ToUpperInvariant();

                if (args[i].Contains("-dev"))
                {
                    SessionSettings.SetDevelopment(this, true);
                }
                else if (args[i].Contains("-loadlevel"))
                {
                    screenManager.StartLevel = Convert.ToInt32(args[i + 1]);
                    i++;
                }
                else if (args[i].Contains("+X"))
                {
                    _backbufferWidth = Convert.ToInt32(args[i + 1]);
                    i++;
                }
                else if (args[i].Contains("+Y"))
                {
                    _backbufferHeight = Convert.ToInt32(args[i + 1]);
                    i++;
                }
                else if (args[i].Contains("-FULLSCREEN"))
                {
                    _fullScreen = true;
                }
                else if (args[i].Contains("-SKIPSTARTUP"))
                {
                    _showStartup = false;
                }
            }
        }
    }
}
