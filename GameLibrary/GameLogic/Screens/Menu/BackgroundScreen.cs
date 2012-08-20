using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace GameLibrary.GameLogic.Screens.Menu
{
    public class BackgroundScreen : GameScreen
    {
        #region Fields

        Texture2D _texture;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Activate()
        {
            ContentManager content = new ContentManager(ScreenManager.Game.Services, "Content");

            this._texture = content.Load<Texture2D>("Assets/Other/Game/TITLE IMAGE");
        }

        #endregion

        #region Update and Draw

        public override void Update(float delta, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(delta, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            //  Make sure the full picture is shown at once.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            //  Position in the centre of the screen.
            Vector2 position = new Vector2(viewport.Width, viewport.Height) * 0.5f;

            //  Scale it so it always fits on screen.
            float scale = (float)viewport.Width / (float)_texture.Width;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            spriteBatch.Draw(_texture, position, null,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha), 0.0f, 
                             new Vector2(_texture.Width, _texture.Height) * 0.5f, scale, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }


        #endregion
    }
}
