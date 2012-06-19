//#define Development

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Assists;
using GameLibrary.Managers;
using GameLibrary.Drawing;

namespace GameLibrary.Screens
{
    public class IntroScreen : Screen
    {
        #region Fields
        private AnimatedText text;
        public FullscreenPicture background { get; set; }

#if Development
        PrimGrid grid;
#endif
        #endregion

        public IntroScreen()
            : base("IntroScreen", 1)
        {

        }

        #region Load Content
        public override void Load()
        {
            text = new AnimatedText("Press Any Key to Continue", ImgType.None, 2f, Alignment.Centre);
            text.AnchorPoint = ScreenAnchorLocation.Centre;
            text.TextType = AnimatedType.Grow;

            background = new FullscreenPicture();
            //  TODO : Load it and draw.


#if Development
            grid = new PrimGrid(4, 6);
#endif

            base.Load();
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            if (Input.isGamePad)
            {
                if (Input.Menu() || Input.GP_A || Input.GP_B)
                    FadeOut();
            }
            else
            {
                if (Input.Menu() || Input.Space || Input.lMouseButton || Input.Enter)
                    FadeOut();
            }

            text.Update(gameTime);

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.Black);

            #region Development
#if Development
            grid.Draw();
#endif
            #endregion

            sb.Begin();
            text.Draw(sb);

            if (ScreenState != State.Show)
                base.Draw(sb);
            sb.End();
        }
        #endregion
    }
}
