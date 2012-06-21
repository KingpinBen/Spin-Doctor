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
using GameLibrary.Screens;
using GameLibrary.Assists;
using GameLibrary.Screens.Messages;
using GameLibrary.Managers;

#endregion

namespace SpinDoctor
{
    public class Game1 : Game
    {
        #region Fields
        private GraphicsDeviceManager _graphics;

        Screen_Manager _screenMan;

        #endregion

        #region Constructor
        public Game1()
        {
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferMultiSampling = true;
            //_graphics.IsFullScreen = true;
            _graphics.SynchronizeWithVerticalRetrace = true;
            IsFixedTimeStep = false;
            _graphics.ApplyChanges();

            this.Window.Title = "Spin Doctor - Development";

            ConvertUnits.SetDisplayUnitToSimUnitRatio(24f);
        }
        #endregion

        #region Init
        protected override void Initialize()
        {
            Window.AllowUserResizing = false;
            Window.ClientSizeChanged += ChangeWindowSize;

            _screenMan = new Screen_Manager(this);
            _screenMan.Load();
            Components.Add(_screenMan);
            

            GameplayScreen gamescreen = new GameplayScreen(0);
            Screen_Manager.AddScreen(gamescreen);

            

#if ShowStartUp
            IntroScreen introScreen = new IntroScreen();
            Screen_Manager.AddScreen(introScreen);

            //SplashScreen devSplash = new SplashScreen("Assets/Other/Dev/DevLogo");
            Screen_Manager.AddScreen(devSplash);
#endif

            base.Initialize();
        }
        #endregion

        #region Load
        protected override void LoadContent()
        {
            
        }
        #endregion

        #region Unload
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #endregion

        #region Update
        protected override void Update(GameTime gameTime)
        {
            //  Game Components are drawn from base.Update (e.g. Screen_Manager)

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        protected override void Draw(GameTime gameTime)
        {
            //  DrawableGameComponents are drawn from base

            base.Draw(gameTime);
        }
        #endregion

        private void ChangeWindowSize(object sender, EventArgs e)
        {
            if (Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0)
            {
                _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            }

            _graphics.ApplyChanges();
        }
    }
}
