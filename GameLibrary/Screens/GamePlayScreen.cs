//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - GameplayScreen
//--    
//--    
//--    Description
//--    ===============
//--    The main gameplay screen - Deserializes levels etc.
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Introduced level/player/cam from SM testing. DynamicGrav
//--    BenG - GPS now does the level loading rather than the level class.
//--           Can save everything we want to know about the level now.
//--    
//--    
//--    TBD
//--    ==============
//--    The rest of it, durrh
//--    
//--    
//--------------------------------------------------------------------------

#define Development

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using GameLibrary.Assists;
using GameLibrary.Drawing;
using GameLibrary.Objects;
using GameLibrary.Screens.Menu;
using GameLibrary.Managers;
using FarseerPhysics.DebugViews;
using FarseerPhysics;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Collision;
using Microsoft.Xna.Framework.Content;
#endregion

namespace GameLibrary.Screens
{
    public class GameplayScreen : Screen
    {
        #region Fields and Variables

        public static World World { get; set; }

        protected static float bgRotation;

        public static Level Level { get; internal set; }
        private static int LevelID;
        private GraphicsDevice graphicsDevice;
        private Effect gamePlayEffect;
        private RenderTarget2D RenderTargetEffect;

        #endregion

        #region Constructor
        public GameplayScreen(int ID) 
            : base("GamePlayScreen", 0.5f)
        {
            LevelID = ID;
            this.IsExitable = false;
            Level = new Level();
            this.graphicsDevice = Screen_Manager.Graphics;

            PresentationParameters pp = this.graphicsDevice.PresentationParameters;

            RenderTargetEffect = new RenderTarget2D(Screen_Manager.Graphics, pp.BackBufferWidth, pp.BackBufferHeight);
        }
        #endregion

        #region Load
        public override void Load()
        {
            IsInitialized = false;
            HUD.Load();
            //gamePlayEffect = Content.Load<Effect>("Assets/Effects/BlackAndWhite");
            //gamePlayEffect.Parameters["enableMonochrome"].SetValue(false);

            LoadLevel(LevelID);

            IsInitialized = true;

            FadeIn();
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
#if EDITOR

#else
            if (!Level.IsInitialized)
            {
                return;
            }

            // Updates the world every second. Make it either first or last
            World.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.001));

            Camera.Update(gameTime);
            Level.Update(gameTime);
            HandleInput();

            #region Camera Fix
            //  Camera fix for non-rotating spritebatches

            if (Camera.CameraType == CameraType.Focus)
                bgRotation = Camera.Rotation;
            else
                bgRotation = 0f;
            #endregion

            Sprite_Manager.Update(gameTime);
#endif
        }
        #endregion

        #region Draw
#if EDITOR
        
#else
        public override void Draw(SpriteBatch sb)
        {
            if (gamePlayEffect != null)
            {
                //  Set the following to draw on the rendertarget
                this.graphicsDevice.SetRenderTarget(RenderTargetEffect);
            }

            Screen_Manager.Graphics.Clear(Color.Black);

            #region Draw Level
            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null,
                    Matrix.CreateTranslation(new Vector3(-Camera.Position, 0)) *
                    Matrix.CreateRotationZ(bgRotation) *
                    Matrix.CreateScale(new Vector3(Camera.Zoom, Camera.Zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(Screen_Manager.Graphics.Viewport.Width / 2, Screen_Manager.Graphics.Viewport.Height / 2, 0f)));
            {
                Level.DrawBackground(sb);
            }
            sb.End();

            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.TransformMatrix());
            {
                Level.DrawGameplay(sb);
                Sprite_Manager.Draw(sb);
            }
            sb.End();
            #endregion

            if (gamePlayEffect != null)
            {
                //  Clear the rendertarget and screen
                this.graphicsDevice.SetRenderTarget(null);
                Screen_Manager.Graphics.Clear(Color.Black);
            }

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            {
                if (gamePlayEffect != null)
                {
                    //  Apply Post Processing
                    gamePlayEffect.CurrentTechnique.Passes[0].Apply();
                    sb.Draw(RenderTargetEffect, Vector2.Zero, Color.White);
                }

                //  Draw the HUD
                HUD.Draw(sb);
                base.Draw(sb);
            }
            sb.End();
        }
#endif
        #endregion

        #region HandleInput
        private void HandleInput()
        {
            if (Player.Instance.PlayerState == pState.Dead && Input.GP_X)
                ReloadLevel();

            if (Input.GP_Start || Input.Escape)
            {
                GameMenu pause = new GameMenu();
                Screen_Manager.AddScreen(pause);
            }
        }
        #endregion

        #region Level loading/reloading

        /// <summary>
        /// Resets everything in the level
        /// </summary>
        public static void ReloadLevel()
        {
#if EDITOR

#else
            World = new World(Vector2.Zero);
            Camera.UpIs = UpIs.Up;

            #region Development
#if Development
            DevDisplay.Load(World);
#endif
            #endregion

            //  If theres anything in Level, clear it.
            if (Level != null)
            {
                if (Level.Content != null)
                {
                    Level.Unload();
                }

                Level = new Level();
            }

            using (XmlReader read = XmlReader.Create( FileLoc.Level(LevelID) ))
            {
                Level = IntermediateSerializer.Deserialize<Level>(read, null);
            }
            
            Level.Load(World);

            HUD.RefreshHUD();
            Sprite_Manager.Clear();       
#endif
        }

        /// <summary>
        /// Use to change the level
        /// </summary>
        /// <param name="ID">The ID of the level to load</param>
        public static void LoadLevel(int ID)
        {
            LevelID = ID;

            ReloadLevel();
        }
        #endregion
    }
}