using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameLibrary.GameLogic.Controls;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.GameLogic.Screens.Splash
{
    public class SplashScreen : GameScreen
    {
        Texture2D _texture;
        Vector2 _position;
        bool _fadingOut;
        bool _fadingIn = false;
        float _elapsed;
        float _alpha;
        InputAction _skipScreen;

        public SplashScreen()
            : base()
        { }

        public override void Activate()
        {
            ContentManager content = new ContentManager(this.ScreenManager.Game.Services, "Content");

            this._texture = content.Load<Texture2D>("Assets/Other/Game/SplashBanner");
            this._position = new Vector2(
                this.ScreenManager.GraphicsDevice.Viewport.Width, 
                this.ScreenManager.GraphicsDevice.Viewport.Height) * 0.5f;

            this._skipScreen = new InputAction(
                new Buttons[] { Buttons.A, Buttons.B, Buttons.Start, Buttons.Back, Buttons.X },
                new Keys[] { Keys.Space, Keys.Enter, Keys.Escape, Keys.Tab, Keys.E }, 
                true);
        }

        public override void HandleInput(float delta, InputState input)
        {
            PlayerIndex playerIndex;

            if (_skipScreen.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                this.ExitScreen();
            }
        }

        public override void Update(float delta, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            float alphaDelta = 0.01f;
            _elapsed += delta;

            if ((_alpha == 0.0f && _elapsed >= 2.0f) && !(_fadingIn || _fadingOut))
            {
                this._fadingIn = true;
                _elapsed = 0.0f;
            }

            if (_fadingIn)
            {
                _alpha += alphaDelta;

                if (_alpha >= 1.0f)
                {
                    _fadingIn = false;
                    _elapsed = 0.0f;
                }
            }
            else if (_fadingOut)
            {
                _alpha -= alphaDelta;

                if (_alpha <= 0.0f)
                {
                    this.ExitScreen();
                }
            }
            else
            {
                if (_elapsed >= 4.0f)
                {
                    _fadingOut = true;
                }
            }

            base.Update(delta, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            this.ScreenManager.GraphicsDevice.Clear(Color.Black);

            SpriteBatch spriteBatch = this.ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(_texture, this._position, null, Color.White * _alpha, 0.0f, new Vector2(this._texture.Width, this._texture.Height) * 0.5f, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
