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
//--    Fix the fadein/fadeouts
//--    
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using GameLibrary.Assists;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using GameLibrary.Screens;
using GameLibrary.Drawing;
using System.Diagnostics;
#endregion

namespace GameLibrary.Managers
{
    public class Screen_Manager : DrawableGameComponent
    {
        #region Fields and Variables

        public static List<Screen> ScreenList { get; protected set; }

        public static new Game Game;

        public static ContentManager Content { get; set; }

        public SpriteBatch SpriteBatch { get; internal set; }

        public static GraphicsDevice Graphics
        {
            get
            {
                return Game.GraphicsDevice;
            }
        }

        public static Vector2 Viewport
        {
            get 
            { 
                return new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height); 
            }
        }

        public static float AspectRatio
        {
            get 
            { 
                return Game.GraphicsDevice.Viewport.AspectRatio; 
            }
        }

        public static bool LoadingContent { get; protected set; }

        public static Texture2D BlackPixel { get; protected set; }

#if DEBUG
        Stopwatch stopWatch = new Stopwatch();
#endif

        #endregion

        #region Constructor

        public Screen_Manager(Game game)
            : base(game)
        {
            Game = game;
            Content = new ContentManager(game.Services, "Content");
        }
        #endregion

        #region Load
        /// <summary>
        /// Load Content
        /// </summary>
        public void Load()
        {
            ScreenList = new List<Screen>();
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Input.Load();
            Audio.Load();
            Fonts.Load(Content);

            BlackPixel = Content.Load
                <Texture2D>(FileLoc.BlankPixel());
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the topscreen and checks for user input.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Update the keyboard, mouse and gamepad
            Input.Update(gameTime);
            Audio.Update();
            HUD.Update(gameTime);

            #region Development
#if Development
            DevDisplay.Update(gameTime);
#endif
            #endregion

            ScreenList[ScreenList.Count - 1].Update(gameTime);

            if (LoadingContent && ScreenList[0].IsInitialized)
                LoadingContent = false;
        }

        #endregion

        #region Draw
        /// <summary>
        /// Draws the gameplayscreen and the topscreen.
        /// </summary>
        /// <param name="sb">Spritebatch</param>
        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            stopWatch.Start();
#endif

            for (int i = 0; i < ScreenList.Count; i++)
            {
                ScreenList[i].Draw(SpriteBatch);
            }

            #region Development
#if DEBUG
            DevDisplay.Draw(gameTime, stopWatch);
            stopWatch.Reset();
#endif
            #endregion

        }
        #endregion

        #region Add Screen
        /// <summary>
        /// Adds the passed screen to the top for interaction.
        /// </summary>
        /// <param name="screen">Which screen do you want to add? (e.g. this)</param>
        public static void AddScreen(Screen screen)
        {
            screen.Load();
            ScreenList.Add(screen);
        }
        #endregion

        #region Delete Top Screen
        /// <summary>
        /// Deletes the top Screen
        /// </summary>
        /// <param name="screen"></param>
        public static void DeleteScreen()
        {
            int i = ScreenList.Count - 1;

            ScreenList[i].Unload();
            ScreenList.RemoveAt(i);
        }
        #endregion

        #region Load Level
        public static void LoadLevel(int id)
        {
            for (int i = ScreenList.Count; i > 1; i--)
                ScreenList.RemoveAt(i - 1);

            GameplayScreen.LoadLevel(id);

            LoadingScreen loading = new LoadingScreen();
            LoadingContent = true;
            loading.Load();
            ScreenList.Add(loading);
        }
        #endregion

        #region GetScreenName
        /// <summary>
        /// Returns the name of the screen.
        /// </summary>
        /// <param name="i">Use "_screenList.Count-1" for top screen.</param>
        /// <returns>The string name of the passed screen number</returns>
        public static string GetScreenName(int i)
        {
            return ScreenList[i].Name;
        }
        #endregion

        #region ExitGame
        /// <summary>
        /// Exits the game.
        /// </summary>
        public static void ExitGame()
        {
            Content.Dispose();
            Game.Exit();
        }
        #endregion

        #region Get Screen State
        public static string GetScreenState(int i)
        {
            return ScreenList[i].ScreenState.ToString();
        }
        #endregion

        #region Screen Count
        public static int GetScreenCount()
        {
            return ScreenList.Count;
        }
        #endregion
    }
}
