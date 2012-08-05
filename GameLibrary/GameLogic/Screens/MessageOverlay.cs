using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.GameLogic.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GameLibrary.GameLogic.Screens
{
    /// <summary>
    /// TODO: FINISH THIS.
    /// 
    /// 
    /// </summary>

    public class MessageOverlay : GameScreen
    {
        string _textureAsset;
        Texture2D _texture;
        Vector2 _position;

        InputAction moveUp;
        InputAction moveDown;
        InputAction inputContinue;

        public MessageOverlay(string textureAsset)
            : base()
        {
            this._textureAsset = textureAsset;

            this.inputContinue = new InputAction(
                new Buttons[] { Buttons.A, Buttons.B, Buttons.Back, Buttons.Start },
                new Keys[] { Keys.Space, Keys.Enter, Keys.Escape, Keys.Tab }, true);
            this.moveUp = new InputAction(new Buttons[] { Buttons.RightThumbstickUp, Buttons.LeftThumbstickUp },
                new Keys[] { Keys.Up, Keys.W, Keys.PageUp }, false);
            this.moveDown = new InputAction(new Buttons[] { Buttons.RightThumbstickDown, Buttons.LeftThumbstickDown },
                new Keys[] { Keys.Down, Keys.S, Keys.PageDown }, false);
        }

        public override void Activate()
        {
            ContentManager content = new ContentManager(this.ScreenManager.Game.Services, "Content");

            _texture = content.Load<Texture2D>(_textureAsset);

            base.Activate();
        }

        public override void HandleInput(float delta, InputState input)
        {
            PlayerIndex playerIndex;

            if (inputContinue.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                OnCancel(playerIndex);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = this.ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(_texture, _position, null, Color.White, 0.0f, new Vector2(_texture.Width, _texture.Height) * 0.5f, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.End();
        }

        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }
    }
}
