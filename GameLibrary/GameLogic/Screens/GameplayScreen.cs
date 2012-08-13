using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using GameLibrary.GameLogic.Controls;
using GameLibrary.GameLogic.Screens.Menu;
using FarseerPhysics.Dynamics;
using GameLibrary.Graphics;
using GameLibrary.Levels;
using System.Xml;
using GameLibrary.Helpers;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content.Pipeline;
using GameLibrary.Graphics.Camera;
using GameLibrary.Graphics.UI;
using GameLibrary.GameLogic.Events;
using GameLibrary.GameLogic.Objects;
using GameLibrary.GameLogic.Characters;
using GameLibrary.System;

namespace GameLibrary.GameLogic.Screens
{
    public class GameplayScreen : GameScreen
    {
        #region Fields

        private ContentManager _content;

        private World _world = new World(Vector2.Zero);

        private Level _level;

        private float pauseAlpha;
        private float _bgRotation;

        private InputAction pauseAction;

        private Effect _silhouetteEffect;
        private Effect _alphaEffect;

        private RenderTarget2D rtMask;
        private RenderTarget2D rtBlur;
        private RenderTarget2D rtEffect;

        private Texture2D _objectsTexture;

        #endregion

        #region Properties

        #region Level ID

        public int CurrentLevelID
        {
            get
            {
                return GameSettings.Instance.CurrentLevel;
            }
            set
            {
                GameSettings.Instance.CurrentLevel = value;
                SaveManager.Instance.SaveGame();
                ScreenManager.LoadLevel(this);
            }
        }

        #endregion

        #region World
        public World World
        {
            get
            {
                return _world;
            }
        }
        #endregion

        #endregion

        #region Initialization

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start},
                new Microsoft.Xna.Framework.Input.Keys[] { Microsoft.Xna.Framework.Input.Keys.Escape },
                true);
        }

        public override void Activate()
        {
#if EDITOR

#else
            _content = new ContentManager(this.ScreenManager.Game.Services, "Content");

            //  Setup the Hud elements
            HUD.Instance.Load(this.ScreenManager);

            //  Initiate and setup the level class.
            //  Load level in a moment
            Camera.Instance.SetGameplayScreen(this);
            LoadLevel();

            //  Setup the SpriteManager.
            SpriteManager.Instance.Load(this.ScreenManager);

            //  Load anything control related
            InputManager.Instance.Load();

            //  Setup use of shadows if they're enabled.
            if (GameSettings.Instance.Shadows == SettingLevel.On)
            {
                PresentationParameters pp = this.ScreenManager.GraphicsDevice.PresentationParameters;

                rtMask = new RenderTarget2D(this.ScreenManager.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
                rtBlur = new RenderTarget2D(this.ScreenManager.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
                rtEffect = new RenderTarget2D(this.ScreenManager.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);

                _silhouetteEffect = _content.Load<Effect>("Assets/Other/Effects/MaskEffect");
                _alphaEffect = _content.Load<Effect>("Assets/Other/Effects/AlphaTexture");
            }


            //  Simulate something large loading because why not..
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
#endif

        }

        public override void Deactivate()
        {
            base.Deactivate();
        }


        #endregion

        #region Update and Draw

        public override void Update(float delta, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(delta, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 16, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 16, 0);
            }

            if (IsActive)
            {
                
                HUD.Instance.Update(delta);
                InputManager.Instance.Update(delta);
                Camera.Instance.Update(delta);

                _world.Step(delta);
                _level.Update(delta);
                
                SpriteManager.Instance.Update(delta);

                if (GameSettings.Instance.DevelopmentMode)
                {
                    DevDisplay.Instance.Update(delta);
                }

                _bgRotation = -Camera.Instance.Rotation;
            }
        }

        public override void HandleInput(float delta, InputState input)
        {
#if EDITOR

#else
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                if (Player.Instance.PlayerState == PlayerState.Dead && InputManager.Instance.Jump(true))
                {
                    LoadLevel();
                }

                //  TODO: REMOVE FOR BETA HANDIN
                if (InputManager.Instance.IsNewGpPress(Buttons.DPadDown) || InputManager.Instance.IsNewKeyPress(Microsoft.Xna.Framework.Input.Keys.F1))
                {
                    GameSettings.Instance.ToggleDoubleJump();
                }
            }
#endif
        }

        public override void Draw(GameTime gameTime)
        {
#if EDITOR

#else
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            Matrix cameraTransform = Camera.Instance.TransformMatrix();

            #region Get the mask for the shadows

            if (GameSettings.Instance.Shadows == SettingLevel.On)
            {
                graphics.SetRenderTarget(rtEffect);


                graphics.Clear(Color.Transparent);

                this.DrawObjects(spriteBatch, SpriteSortMode.Immediate, BlendState.NonPremultiplied, false, true);
                this._objectsTexture = rtEffect;

                graphics.SetRenderTarget(rtMask);
                graphics.Clear(Color.Transparent);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                _silhouetteEffect.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(_objectsTexture, Vector2.Zero, Color.White);
                spriteBatch.End();

                _objectsTexture = rtMask;

                graphics.SetRenderTarget(rtBlur);
                graphics.Clear(Color.Transparent);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                _silhouetteEffect.CurrentTechnique.Passes[1].Apply();
                spriteBatch.Draw(_objectsTexture, Vector2.Zero, Color.White);
                spriteBatch.End();

                _objectsTexture = rtBlur;

                graphics.SetRenderTarget(null);
            }

#endregion

<<<<<<< HEAD
            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(Color.Black);
=======

            graphics.Clear(Color.Black);
>>>>>>> Tech Doc revisions

            if (_level.RoomType == RoomType.Rotating)
            {
                _level.DrawBackground();
                _level.DrawBackdrop(ref cameraTransform);
            }

            this.DrawObjects(spriteBatch, SpriteSortMode.BackToFront, BlendState.AlphaBlend, true, false);

            #region Draw the shadows

            if (GameSettings.Instance.Shadows == SettingLevel.On)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                spriteBatch.Draw(_objectsTexture, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.98f);
                spriteBatch.End();
            }

            #endregion

            spriteBatch.Begin();
            HUD.Instance.Draw(spriteBatch);
            spriteBatch.End();

            if (GameSettings.Instance.DevelopmentMode)
            {
                DevDisplay.Instance.Draw(this.ScreenManager.GraphicsDevice);
            }

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
#endif
        }


        #endregion

#if !EDITOR
        private void LoadLevel()
        {
            int LevelID = GameSettings.Instance.CurrentLevel;

            if (this._level != null)
            {
                this._level.Content.Unload();
            }

            this._level = new Level();
            this._world = new World(Vector2.Zero);
            Camera.Instance.SetUpIs(UpIs.Up);
            EventManager.Instance.Load(this);
            SpriteManager.Instance.Clear();
            HUD.Instance.RefreshHUD();
            
            try
            {
                using (XmlReader reader = XmlReader.Create(Defines.Level(LevelID)))
                {
                    this._level = IntermediateSerializer.Deserialize<Level>(reader, null);
                }
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Something went wrong when trying to load level: '" + LevelID + "'\nThe level wasn't found.");
                ErrorReport.GenerateReport("The level " + LevelID + " could not be found.\n" + e.ToString(), null);
                ScreenManager.Game.Exit();
            }
            catch (InvalidContentException e)
            {
                MessageBox.Show("Something went wrong deserializing level: '" + LevelID + "'.\nInvalid Content");
                string value = "Invalid Content declared.\n" + e.ToString();
                ErrorReport.GenerateReport(value, null);
            }

            try
            {
                this._level.Load(this);
            }
            catch (ContentLoadException e)
            {
                string error = e.InnerException.ToString();

                MessageBox.Show("Something went wrong loading level " + LevelID + ".\n" + error);
                ErrorReport.GenerateReport(error, null);
            }

            //  Now the level has been set up, if we have development
            //  mode on, turn on the display.
            if (GameSettings.Instance.DevelopmentMode)
            {
                DevDisplay.Load(this.ScreenManager, World);
            }

        }

        void DrawObjects(SpriteBatch spriteBatch, SpriteSortMode sortMode, BlendState blendState, bool drawDecals, bool shadowPass)
        {
            Matrix cameraTransform = Camera.Instance.TransformMatrix();
            NodeObject obj = new NodeObject();
            GraphicsDevice graphics = this.ScreenManager.GraphicsDevice;

            spriteBatch.Begin(sortMode, blendState, SamplerState.AnisotropicWrap, null, null, null, cameraTransform);
            
            if (drawDecals)
            {
                this._level.DecalManager.Draw(spriteBatch);
                SpriteManager.Instance.Draw(spriteBatch);
            }

            Player.Instance.Draw(spriteBatch);

            for (int i = 0; i < _level.ObjectsList.Count; i++)
            {
                obj = _level.ObjectsList[i];

                //  If it is the shadow pass and the object doesn't cast 
                //  shadows, move on.
                if (shadowPass && !obj.CastShadows)
                {
                    continue;
                }

                obj.Draw(spriteBatch, graphics);
            }

            spriteBatch.End();
        }

        public void RemoveObject(NodeObject obj)
        {
            if (obj.GetBody() != null)
            {
                this._world.RemoveBody(obj.GetBody());
            }

            this._level.GetObjectsToRemove().Add(obj);
        }
#endif
    }
}
