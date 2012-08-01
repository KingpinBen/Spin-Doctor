using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Graphics;

namespace GameLibrary.GameLogic.Screens.Menu
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    class MessageBoxScreen : GameScreen
    {
        #region Fields

        string message;
        Texture2D _gradientTexture;
        Texture2D _yesTexture;
        Texture2D _noTexture;
        SpriteFont _font;
        InputAction menuSelect;
        InputAction menuCancel;

        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        #endregion

        #region Initialization

        public MessageBoxScreen(string message)
        {
            this.message = message + "\n";

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            menuSelect = new InputAction(
                new Buttons[] { Buttons.A, Buttons.Start },
                new Keys[] { Keys.Space, Keys.Enter },
                true);
            menuCancel = new InputAction(
                new Buttons[] { Buttons.B, Buttons.Back },
                new Keys[] { Keys.Escape, Keys.Back },
                true);
        }

        public override void Activate()
        {
            ContentManager content = ScreenManager.Game.Content;
            _gradientTexture = content.Load<Texture2D>("Assets/Images/Basics/gradient");

            if (InputManager.Instance.isGamePad)
            {
                _yesTexture = content.Load<Texture2D>("Assets/Other/Controls/A");
                _noTexture = content.Load<Texture2D>("Assets/Other/Controls/B");
            }
            else
            {
                //  CHANGE TO KEYBOARD KEYS

                _yesTexture = content.Load<Texture2D>("Assets/Other/Controls/A");
                _noTexture = content.Load<Texture2D>("Assets/Other/Controls/B");
            }

            _font = FontManager.Instance.GetFont(FontList.GUI);
        }


        #endregion

        #region Handle Input

        public override void HandleInput(float delta, InputState input)
        {
            PlayerIndex playerIndex;

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (menuSelect.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                {
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));
                }

                ExitScreen();
            }
            else if (menuCancel.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                // Raise the cancelled event, then exit the message box.
                if (Cancelled != null)
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));

                ExitScreen();
            }
        }


        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack((TransitionAlpha * 2) * 0.33f);

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = _font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) * 0.5f;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);

            // Fade the popup alpha during transitions.
            Color color = Color.DarkGoldenrod * TransitionAlpha;

            string yesno = "Yes             No";
            Vector2 bgCentre = new Vector2(backgroundRectangle.Center.X, backgroundRectangle.Center.Y);
            Vector2 yesnoTextOrigin = new Vector2(_font.MeasureString(yesno).X, _font.MeasureString(yesno).Y) * 0.5f;


            spriteBatch.Begin();

            // Draw the background rectangle.
            spriteBatch.Draw(_gradientTexture, backgroundRectangle, color);

            // Draw the message box text.
            spriteBatch.DrawString(_font, message, textPosition, color);
            spriteBatch.DrawString(_font, yesno, bgCentre, color, 0.0f, yesnoTextOrigin - new Vector2(0, yesnoTextOrigin.Y), 1.0f, SpriteEffects.None, 1.0f);

            spriteBatch.Draw(_yesTexture, bgCentre - new Vector2(yesnoTextOrigin.X + _yesTexture.Width, 0), Color.White);
            spriteBatch.Draw(_noTexture, bgCentre + new Vector2(yesnoTextOrigin.X + _noTexture.Width, 0), Color.White);

            spriteBatch.End();
        }


        #endregion

        public SpriteFont FontManage { get; set; }
    }
}
