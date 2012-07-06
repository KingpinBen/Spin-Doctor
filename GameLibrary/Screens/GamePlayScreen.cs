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

//#define Development

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
        public static int LevelID { get; set; }
        private static new bool IsInitialized = false;
        private GraphicsDevice graphicsDevice;
        private Effect shadowEffect;
        private Effect alphaEffect;
        private SpriteBatch _spriteBatch;

        private RenderTarget2D renderTargetMask;
        private RenderTarget2D renderTargetBlur;
        private RenderTarget2D RenderTargetEffect;
        private Texture2D _gameObjects;
        private Texture2D _shadowMap;

        #endregion

        #region Constructor
        public GameplayScreen(int ID) 
            : base("GamePlayScreen")
        {
            this.IsExitable = false;

            LevelID = ID;
            Level = new Level();
            World = new World(Vector2.Zero);

            this.graphicsDevice = Screen_Manager.GraphicsDevice;
            this._spriteBatch = new SpriteBatch(this.graphicsDevice);

            PresentationParameters pp = this.graphicsDevice.PresentationParameters;
            this.RenderTargetEffect = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth16);
            this.renderTargetMask = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            this.renderTargetBlur = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
        }
        #endregion

        #region Load
        public override void Load()
        {
            HUD.Load();
            LoadLevel();
            shadowEffect = _content.Load<Effect>("Assets/Other/Effects/MaskEffect");
            alphaEffect = _content.Load<Effect>("Assets/Other/Effects/AlphaTexture");
            //gamePlayEffect.Parameters["enableMonochrome"].SetValue(true);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
#if EDITOR

#else
            if (!Level.IsInitialized)
            {
                return;
            }

            // Updates the world every second
            World.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f));

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

        #region Draw
#if EDITOR
        
#else
        public override void Draw(SpriteBatch sb)
        {
            Matrix cameraTransform = Camera.TransformMatrix();

            if (shadowEffect != null)
            {
                this.graphicsDevice.SetRenderTarget(RenderTargetEffect);
            }

            this.graphicsDevice.Clear(Color.Transparent);

            this.DrawObjects(cameraTransform, SpriteSortMode.Immediate, BlendState.NonPremultiplied, false, true);
            this._gameObjects = RenderTargetEffect;

            this.graphicsDevice.SetRenderTarget(renderTargetMask);
            this.graphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            shadowEffect.CurrentTechnique.Passes[0].Apply();
            _spriteBatch.Draw(_gameObjects, Vector2.Zero, Color.White);
            _spriteBatch.End();

            _gameObjects = renderTargetMask;

            this.graphicsDevice.SetRenderTarget(renderTargetBlur);
            this.graphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            shadowEffect.CurrentTechnique.Passes[1].Apply();
            _spriteBatch.Draw(_gameObjects, Vector2.Zero, Color.White);
            _spriteBatch.End();

            _gameObjects = renderTargetBlur;

            this.graphicsDevice.SetRenderTarget(null);
            this.graphicsDevice.Clear(Color.Black);

            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null,
                    Matrix.CreateTranslation(new Vector3(-Camera.Position, 0)) *
                    Matrix.CreateRotationZ(bgRotation) *
                    Matrix.CreateScale(new Vector3(Camera.Zoom, Camera.Zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(Screen_Manager.GraphicsDevice.Viewport.Width * 0.5f, Screen_Manager.GraphicsDevice.Viewport.Height * 0.5f, 0f)));
            {
                Level.DrawBackground(sb);
            }
            sb.End();

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap, null, null, null, cameraTransform);
            Level.DrawBackdrop(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin();
            _spriteBatch.Draw(_gameObjects, Vector2.Zero, Color.White);
            _spriteBatch.End();

            this.DrawObjects(cameraTransform, SpriteSortMode.BackToFront, BlendState.AlphaBlend, true, false);

#if shit
            //if (gamePlayEffect != null)
            //{
            //    //  Set the following to draw on the rendertarget
            //    this.graphicsDevice.SetRenderTarget(RenderTargetEffect);
            //}

            //Screen_Manager.GraphicsDevice.Clear(Color.Black);

            //#region Draw Level
            //sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null,
            //        Matrix.CreateTranslation(new Vector3(-Camera.Position, 0)) *
            //        Matrix.CreateRotationZ(bgRotation) *
            //        Matrix.CreateScale(new Vector3(Camera.Zoom, Camera.Zoom, 1)) *
            //        Matrix.CreateTranslation(new Vector3(Screen_Manager.GraphicsDevice.Viewport.Width * 0.5f, Screen_Manager.GraphicsDevice.Viewport.Height * 0.5f, 0f)));
            //{
            //    Level.DrawBackground(sb);
            //}
            //sb.End();

            //sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.TransformMatrix());
            //{
            //    this.DrawObjects(sb, SpriteSortMode.
            //    Sprite_Manager.Draw(sb);
            //}
            //sb.End();
            //#endregion

            //if (gamePlayEffect != null)
            //{
            //    //  Clear the rendertarget and screen
            //    this.graphicsDevice.SetRenderTarget(null);
            //    Screen_Manager.GraphicsDevice.Clear(Color.Black);
            //}

            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //{
            //    if (gamePlayEffect != null)
            //    {
            //        //  Apply Post Processing
            //        gamePlayEffect.CurrentTechnique.Passes[0].Apply();
            //        sb.Draw(RenderTargetEffect, Vector2.Zero, Color.White);
            //    }

            //    //  Draw the HUD
            //    HUD.Draw(sb);
            //    base.Draw(sb);
            //}
            //sb.End();
#endif
        }
    

        void DrawObjects(Matrix? transform, SpriteSortMode sortMode, BlendState blendState, bool drawDecals, bool addAlpha)
        {
            if (transform == null)
                _spriteBatch.Begin(sortMode, blendState, SamplerState.AnisotropicWrap, null, null);
            else
            {
                _spriteBatch.Begin(sortMode, blendState, SamplerState.LinearWrap, null, null, null, (Matrix)transform);
            }

            if (addAlpha)
            {
                alphaEffect.CurrentTechnique.Passes[0].Apply();
            }

            Player.Instance.Draw(_spriteBatch);

            if (drawDecals)
            {
                Level.DecalManager.Draw(_spriteBatch);
            }
                
            for (int i = 0; i < Level.ObjectsList.Count; i++)
            {
                Level.ObjectsList[i].Draw(_spriteBatch);
            }
            _spriteBatch.End();
        }

#endif
        #endregion

        #region HandleInput
        private void HandleInput()
        {
            if (Player.Instance.PlayerState == pState.Dead && Input.Jump())
            {
                LoadLevel();
            }

            if (Input.GP_Start || Input.Escape)
            {
                GameMenu pause = new GameMenu();
                Screen_Manager.AddScreen(pause);
            }
        }
        #endregion

        public static void LoadLevel()
        {
#if EDITOR

#else
            IsInitialized = false;
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
            IsInitialized = true;
#endif
        }
    }
}