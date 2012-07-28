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

namespace GameLibrary.GameLogic.Screens
{
    public class GameplayScreen : GameScreen
    {
        #region Fields

        int _currentLevelID;

        public World World
        {
            get
            {
                return _world;
            }
        }
        World _world = new World(Vector2.Zero);
        public Level Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }
        Level _level;
        public int CurrentLevelID
        {
            get
            {
                return _currentLevelID;
            }
            set
            {
                _currentLevelID = value;
                ScreenManager.LoadLevel(this);
            }
        }

        float pauseAlpha;

        InputAction pauseAction;

        #endregion

        #region Initialization

        public GameplayScreen(int levelToLoad)
        {
            this._currentLevelID = levelToLoad;
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start},
                new Microsoft.Xna.Framework.Input.Keys[] { Microsoft.Xna.Framework.Input.Keys.Escape },
                true);          
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
                _world.Step(delta);
                HUD.Instance.Update(delta);
                Camera.Instance.Update(delta);
                InputManager.Instance.Update(delta);
                SpriteManager.Instance.Update(delta);
                _level.Update(delta);

            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
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
                if (Player.Instance.PlayerState == PlayerState.Dead && InputManager.Instance.Jump())
                {
                    LoadLevel();
                }

                if (InputManager.Instance.IsNewGpPress(Buttons.DPadDown) || InputManager.Instance.IsNewKeyPress(Microsoft.Xna.Framework.Input.Keys.F1))
                {
                    GameSettings.ToggleDoubleJump();
                }
            }
#endif
        }

        public override void Draw(GameTime gameTime)
        {
#if EDITOR

#else
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

           
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap, null, null, null, Camera.Instance.TransformMatrix());
            Level.DrawBackdrop(spriteBatch);
            spriteBatch.End();

            this.DrawObjects(spriteBatch, SpriteSortMode.BackToFront, BlendState.AlphaBlend, true, false);
            
            
            spriteBatch.Begin();
            HUD.Instance.Draw(spriteBatch);
            spriteBatch.End();


            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha * 0.7f);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }

            if (SessionSettings.DevelopmentMode)
            {
                DevDisplay.Instance.Draw(this.ScreenManager.GraphicsDevice);
            }
#endif
        }


        #endregion

        public override void Activate()
        {
            #if EDITOR

#else
            //  Initiate and setup the level class.
            //  Load level in a moment
            //this._level = new Level();
            //this._level.Load(this);
            Camera.Instance.SetGameplayScreen(this);
            LoadLevel();

            //  Setup the Hud elements
            HUD.Instance.Load(this.ScreenManager);

            //  Setup the SpriteManager.
            SpriteManager.Instance.Load(this.ScreenManager);

            //  Load anything control related
            InputManager.Instance.Load();

            

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

#if !EDITOR
        private void LoadLevel()
        {

            if (this._level != null)
            {
                this._level.Content.Unload();
            }

            this._level = new Level();
            this._world = new World(Vector2.Zero);
            Camera.Instance.SetUpIs(UpIs.Up);
            EventManager.Instance.Load(this);
            
            try
            {
                using (XmlReader reader = XmlReader.Create(FileLoc.Level(this._currentLevelID)))
                {
                    this._level = IntermediateSerializer.Deserialize<Level>(reader, null);
                }
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Something went wrong when trying to load level: '" + this._currentLevelID + "'\nThe level wasn't found.");
                ErrorReport.GenerateReport("The level " + this._currentLevelID + " could not be found.\n" + e.ToString(), null);
                ScreenManager.Game.Exit();
            }
            catch (InvalidContentException e)
            {
                MessageBox.Show("Something went wrong deserializing level: '" + this._currentLevelID + "'.\nInvalid Content");
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

                MessageBox.Show("Something went wrong loading level " + this._currentLevelID + ".\n" + error);
                ErrorReport.GenerateReport(error, null);
            }

            //  Now the level has been set up, if we have development
            //  mode on, turn on the display.
            if (SessionSettings.DevelopmentMode)
            {
                DevDisplay.Load(this.ScreenManager, World);
            }

        }

        void DrawObjects(SpriteBatch spriteBatch, SpriteSortMode sortMode, BlendState blendState, bool drawDecals, bool addAlpha)
        {
            Matrix cameraTransform = Camera.Instance.TransformMatrix();
            GraphicsDevice graphicsDevice = ScreenManager.GraphicsDevice;

            if (cameraTransform == null)
            {
                spriteBatch.Begin(sortMode, blendState, SamplerState.AnisotropicWrap, null, null);
            }
            else
            {
                spriteBatch.Begin(sortMode, blendState, SamplerState.LinearWrap, null, null, null, cameraTransform);
            }

            //if (addAlpha)
            //{
            //    alphaEffect.CurrentTechnique.Passes[0].Apply();
            //}

            if (drawDecals)
            {
                this._level.DecalManager.Draw(spriteBatch);
                //SpriteManager.Instance.Draw(spriteBatch);
            }

            Player.Instance.Draw(spriteBatch);

            for (int i = 0; i < Level.ObjectsList.Count; i++)
            {
                Level.ObjectsList[i].Draw(spriteBatch, graphicsDevice);
            }

            spriteBatch.End();
        }
#endif
    }
}
