//--------------------------------------------------------------------------
//--    
//--    Spin Doctor
//--    
//--    
//--    Description
//--    ===============
//--    Handles which screen to update and draw.
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial 
//--    
//--    
//--    
//--    TBD
//--    ==============
//--    
//--    
//--------------------------------------------------------------------------

//#define Development

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using GameLibrary.Graphics;
using GameLibrary.Helpers;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Audio;
using GameLibrary.GameLogic.Screens;
using GameLibrary.Graphics.UI;
#endregion

namespace GameLibrary.GameLogic
{
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields

        static List<Screen> _screenList;
        static List<Screen> _screensToAdd;
        static Game _game;
        static SpriteBatch _spriteBatch;
        static GraphicsDeviceManager _deviceManager;
        static ContentManager _content;
        static Texture2D[] _textures;
        static bool _loadingContent;
        static State _currentState;
        static float _transitionAlpha = 1.0f;
        const float _transitionTime = 0.6f;
        static float _transitionPoint = 0;

        Stopwatch stopWatch = new Stopwatch();

        #endregion

        #region Properties
        public static new Game Game
        {
            get
            {
                return _game;
            }
            internal set
            {
                _game = value;
            }
        }
        public static List<Screen> ScreenList
        {
            get
            {
                return _screenList;
            }
        }
        public static ContentManager Content
        {
            get
            {
                return _content;
            }
            internal set
            {
                _content = value;
            }
        }
        public static new GraphicsDevice GraphicsDevice
        {
            get
            {
                return _game.GraphicsDevice;
            }
        }
        public static bool LoadingContent
        {
            get
            {
                return _loadingContent;
            }
        }
        public static Texture2D[] Textures
        {
            get
            {
                return _textures;
            }
        }
        #endregion

        public ScreenManager(Game game, GraphicsDeviceManager graphicsManager)
            : base(game)
        {
            _game = game;
            _content = new ContentManager(game.Services, "Content");
            _deviceManager = graphicsManager;
            _screenList = new List<Screen>();
            _screensToAdd = new List<Screen>();
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _currentState = State.FadeIn;
        }

        public void Load()
        {
            InputManager.Instance.Load();
            FontManager.Instance.Load();
            AudioManager.Instance.Load();
            
            //  This is an array used for global textures that everything needs. In this
            //  case, a blank pixel.
            _textures = new Texture2D[1];
            _textures[0] = Content.Load<Texture2D>(FileLoc.BlankPixel());
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Instance.Update(gameTime);
            AudioManager.Instance.Update();
            HUD.Update(gameTime);

            #region Dev
            if (SessionSettings.DevelopmentMode)
            {
                DevDisplay.Instance.Update(gameTime);
            }
            #endregion

            if (ScreenList.Count > 0)
            {
                ScreenList[ScreenList.Count - 1].Update(gameTime);
            }

            #region Transition Handling
            //  We only want to do all this if the screen state doesn't equal Show.
            if (_currentState == State.FadeIn || _currentState == State.FadeOut)
            {
                //  If the state is fading in or out, we'll need to modify the alpha
                //  for the blackscreen. 
                if (!HandleTransition(gameTime))    //  Have we finished the transition?
                {
                    //  No, don't continue.
                    return;
                }

                //  We have finished transitioning

                if (_currentState == State.FadeOut) //  If the state is fading out,
                {
                    //  Change it to hidden
                    _currentState = State.Hidden;

                    //  and remove any screens ontop.
                    for (int i = ScreenList.Count; i > 1; i--)
                    {
                        ScreenList.RemoveAt(i - 1);
                    }
                }
                else
                {
                    //  If the state was fading in, change it to show.
                    _currentState = State.Show;
                }
            }
            else if (_currentState == State.Hidden) //  If we'd finished transitioning and we're now hiding
            {
                //  We want to add all the screens we want to,
                for (int i = _screensToAdd.Count - 1; i >= 0; i--)
                {
                    _screenList.Add(_screensToAdd[i]);
                }
                //  Clear it for the future and start fading in to show
                //  the new screens.
                _screensToAdd.Clear();
                _currentState = State.FadeIn;

                //  If we're supposed to be loading a level
                if (_loadingContent)
                {
                    //  Load it
                    GameplayScreen.LoadLevel();
                    //  and allow us to continue.
                    _loadingContent = false;
                }
            }
            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            if (SessionSettings.DevelopmentMode)
            {
                stopWatch.Start();
            }

            if (ScreenList.Count > 0)
            {
                for (int i = 0; i < ScreenList.Count; i++)
                {
                    ScreenList[i].Draw(_spriteBatch);
                }
            }

            if (_currentState != State.Show)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_textures[0], Vector2.Zero, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height), Color.Black * _transitionAlpha);
                _spriteBatch.End();
            }

            #region Development
            if (SessionSettings.DevelopmentMode)
            {
                DevDisplay.Instance.Draw(gameTime, stopWatch);
                stopWatch.Reset();
            }

            #endregion

        }

        public static void AddScreen(Screen screen)
        {
            screen.Load();
            ScreenList.Add(screen);
        }

        public static void DeleteScreen()
        {
            int i = ScreenList.Count - 1;

            ScreenList[i].Unload();
            ScreenList.RemoveAt(i);
        }

        public static void LoadLevel(int id)
        {
            _loadingContent = true;
            _currentState = State.FadeOut;

            LoadingScreen loading = new LoadingScreen(GraphicsDevice);
            loading.Load();
            _screensToAdd.Add(loading);
            GameplayScreen.LevelID = id;
        }

        static bool HandleTransition(GameTime gameTime)
        {
            float timeInterval = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

            if (_currentState == State.FadeIn)
            {
                _transitionAlpha = Math.Max(_transitionAlpha - timeInterval, 0);
                _transitionPoint += timeInterval;

                if (_transitionPoint >= _transitionTime)
                {
                    return true;
                }
            }
            else
            {
                _transitionAlpha = Math.Min(_transitionAlpha + timeInterval, 1);
                _transitionPoint -= timeInterval;

                if (_transitionPoint <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        #region Fadeout
        /// <summary>
        /// Kills all screens above gameplay and allows screens to be put up for after fade.
        /// Screen shouldn't be loaded but it can be. Just a waste.
        /// </summary>
        public static void FadeOut(List<Screen> screensToAdd)
        {
            _currentState = State.FadeOut;

            if (screensToAdd != null)
            {
                foreach(Screen screen in screensToAdd)
                {
                    screen.Load();
                    _screensToAdd.Add(screen);
                }
            }
        }
        #endregion

        public static void ExitGame()
        {
            _content.Dispose();
            _game.Exit();
        }
    }
}
