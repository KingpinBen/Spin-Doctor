#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using GameLibrary.GameLogic.Controls;
using GameLibrary.GameLogic.Screens;
using GameLibrary.Graphics;
using GameLibrary.GameLogic.Screens.Menu;
using GameLibrary.GameLogic.Screens.Splash;
using GameLibrary.System;
using GameLibrary.Audio;
#endregion

namespace GameLibrary.GameLogic
{
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields

        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> tempScreensList = new List<GameScreen>();

        InputState input = new InputState();

        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D blankTexture;

        bool isInitialized;
        bool skipStartup = false;
        GraphicsDeviceManager _deviceManager;

        #endregion

        #region Properties

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public SpriteFont Font
        {
            get { return font; }
        }

        public Texture2D BlankTexture
        {
            get { return blankTexture; }
        }

        public new GraphicsDevice GraphicsDevice
        {
            get
            {
                return _deviceManager.GraphicsDevice;
            }
        }

        public bool SkipStartup
        {
            get
            {
                return skipStartup;
            }
            set
            {
                skipStartup = value;
            }
        }

        #endregion

        #region Initialization

        public ScreenManager(Game game, GraphicsDeviceManager device)
            : base(game)
        {
            this._deviceManager = device;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.GraphicsDevice.DeviceReset += new EventHandler<EventArgs>(GraphicsDevice_DeviceReset);

            isInitialized = true;
        }

        void GraphicsDevice_DeviceReset(object sender, EventArgs e)
        {
            for (int i = 0; i < screens.Count; i++)
            {
                screens[i].Activate();
            }
        }

        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            ContentManager content = new ContentManager(Game.Services, "Content");

            FontManager.Instance.Load(content);
            AudioManager.Instance.Load();

            AddScreen(new BackgroundScreen(), null);
            AddScreen(new MainMenuScreen(), null);
            AddScreen(new SplashScreen(), null);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = FontManager.Instance.GetFont(FontList.MenuTitle);
            blankTexture = content.Load<Texture2D>("Assets/Images/Basics/BlankPixel");

            foreach (GameScreen screen in screens)
            {
                screen.Activate();
            }
        }

        protected override void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            foreach (GameScreen screen in screens)
            {
                screen.Unload();
            }
        }


        #endregion

        #region Update and Draw


        public override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

            input.Update();
            AudioManager.Instance.Update();
            tempScreensList.Clear();

            foreach (GameScreen screen in screens)
                tempScreensList.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (tempScreensList.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = tempScreensList[tempScreensList.Count - 1];

                tempScreensList.RemoveAt(tempScreensList.Count - 1);

                // Update the screen.
                screen.Update(delta, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus && screen.AcceptInput)
                    {
                        screen.HandleInput(delta, input);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                    {
                        coveredByOtherScreen = true;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }


        #endregion

        #region Public Methods

        public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
        {
            screen.ControllingPlayer = controllingPlayer;
            screen.ScreenManager = this;
            screen.IsExiting = false;

            // If we have a graphics device, we can begin to properly init
            if (isInitialized)
            {
                screen.Activate();
            }

            screens.Add(screen);
        }


        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            
            if (isInitialized)
            {
                screen.Deactivate();
                screen.Unload();
            }

            screens.Remove(screen);
            tempScreensList.Remove(screen);
        }

        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        public void FadeBackBufferToBlack(float alpha)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(blankTexture, GraphicsDevice.Viewport.Bounds, Color.Black * alpha);
            spriteBatch.End();
        }

        public void LoadLevel(GameplayScreen gameScreen)
        {
            LoadingScreen.Load(this, true, gameScreen.ControllingPlayer, gameScreen);
        }

        public void ChangeDevice(int x, int y, bool fullscreen, bool sampling)
        {
            GameSettings instance = GameSettings.Instance;

            instance.Resolution = new ResolutionData(x, y, fullscreen);
            instance.MultiSamplingEnabled = sampling;

            this._deviceManager.PreferredBackBufferWidth = x;
            this._deviceManager.PreferredBackBufferHeight = y;
            this._deviceManager.IsFullScreen = fullscreen;
            this._deviceManager.PreferMultiSampling = sampling;

            this._deviceManager.ApplyChanges();
        }

        #endregion
    }
}
