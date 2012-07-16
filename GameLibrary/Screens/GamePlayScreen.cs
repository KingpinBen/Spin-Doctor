﻿//--------------------------------------------------------------------------
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
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;
using System.IO;
using GameLibrary.System;
using Microsoft.Xna.Framework.Content.Pipeline;
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
        public GameplayScreen(int ID, GraphicsDevice graphics) 
            : base("GamePlayScreen", graphics)
        {
            this.IsExitable = false;

            LevelID = ID;
            Level = new Level();
            World = new World(Vector2.Zero);

            this._graphicsDevice = graphics;
            this._spriteBatch = new SpriteBatch(graphics);


        }
        #endregion

        #region Load
        public override void Load()
        {
            HUD.Load();
            LoadLevel();
            shadowEffect = _content.Load<Effect>("Assets/Other/Effects/MaskEffect");
            alphaEffect = _content.Load<Effect>("Assets/Other/Effects/AlphaTexture");

            PresentationParameters pp = this._graphicsDevice.PresentationParameters;
            this.RenderTargetEffect = new RenderTarget2D(this._graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth16);
            this.renderTargetMask = new RenderTarget2D(this._graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            this.renderTargetBlur = new RenderTarget2D(this._graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
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

            if (GameSettings.DrawShadows)
            {
                this._graphicsDevice.SetRenderTarget(RenderTargetEffect);


                this._graphicsDevice.Clear(Color.Transparent);

                this.DrawObjects(cameraTransform, SpriteSortMode.Immediate, BlendState.NonPremultiplied, false, true);
                this._gameObjects = RenderTargetEffect;

                this._graphicsDevice.SetRenderTarget(renderTargetMask);
                this._graphicsDevice.Clear(Color.Transparent);

                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                shadowEffect.CurrentTechnique.Passes[0].Apply();
                _spriteBatch.Draw(_gameObjects, Vector2.Zero, Color.White);
                _spriteBatch.End();

                _gameObjects = renderTargetMask;

                this._graphicsDevice.SetRenderTarget(renderTargetBlur);
                this._graphicsDevice.Clear(Color.Transparent);

                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                shadowEffect.CurrentTechnique.Passes[1].Apply();
                _spriteBatch.Draw(_gameObjects, Vector2.Zero, Color.White);
                _spriteBatch.End();

                _gameObjects = renderTargetBlur;
            }

            this._graphicsDevice.SetRenderTarget(null);
            this._graphicsDevice.Clear(Color.Black);

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

            this.DrawObjects(cameraTransform, SpriteSortMode.BackToFront, BlendState.AlphaBlend, true, false);

            if (GameSettings.DrawShadows)
            {
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                _spriteBatch.Draw(_gameObjects, Vector2.Zero, Color.White);
                _spriteBatch.End();
            }

            _spriteBatch.Begin();
            HUD.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    

        void DrawObjects(Matrix transform, SpriteSortMode sortMode, BlendState blendState, bool drawDecals, bool addAlpha)
        {
            if (transform == null)
            {
                _spriteBatch.Begin(sortMode, blendState, SamplerState.AnisotropicWrap, null, null);
            }
            else
            {
                _spriteBatch.Begin(sortMode, blendState, SamplerState.LinearWrap, null, null, null, transform);
            }

            if (addAlpha)
            {
                alphaEffect.CurrentTechnique.Passes[0].Apply();
            }

            if (drawDecals)
            {
                Level.DecalManager.Draw(_spriteBatch);
            }

            Player.Instance.Draw(_spriteBatch);

            for (int i = 0; i < Level.ObjectsList.Count; i++)
            {
                Level.ObjectsList[i].Draw(_spriteBatch);
            }

            if (drawDecals)
            {
                Sprite_Manager.Draw(_spriteBatch);
            }

            _spriteBatch.End();
        }

#endif
        #endregion

        #region HandleInput
        private void HandleInput()
        {
            if (Player.Instance.PlayerState == pState.Dead && InputManager.Instance.Jump())
            {
                LoadLevel();
            }

            if (InputManager.Instance.IsNewGpPress(Buttons.DPadUp) || InputManager.Instance.IsNewKeyPress(Microsoft.Xna.Framework.Input.Keys.F8))
            {
                GameSettings.ToggleShadows();
            }

            if (InputManager.Instance.IsNewGpPress(Buttons.DPadDown) || InputManager.Instance.IsNewKeyPress(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                GameSettings.ToggleDoubleJump();
            }

            if (InputManager.Instance.GP_Start || InputManager.Instance.Escape)
            {
                GameMenu pause = new GameMenu(this._graphicsDevice);
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
            if (SessionSettings.DevelopmentMode)
            {
                DevDisplay.Load(World);
            }
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

            try
            {
                using (XmlReader read = XmlReader.Create(FileLoc.Level(LevelID)))
                {
                    Level = IntermediateSerializer.Deserialize<Level>(read, null);
                }
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Something went wrong when trying to load level: '" + LevelID + "'");
                string value = e.InnerException.ToString();
                ErrorReport.GenerateReport(value, null);
            }
            catch (InvalidContentException e)
            {
                MessageBox.Show("Something went wrong deserializing level: '" + LevelID + "'");
                string value = e.Message.ToString();
                ErrorReport.GenerateReport(value, null);
            }
            
            Level.Load(World);

            HUD.RefreshHUD();
            Sprite_Manager.Clear();
            IsInitialized = true;
#endif
        }
    }
}