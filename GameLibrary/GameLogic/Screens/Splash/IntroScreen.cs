﻿//#define Development

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Graphics.UI;

namespace GameLibrary.GameLogic.Screens.Splash
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

        public IntroScreen(GraphicsDevice graphics)
            : base("IntroScreen", graphics)
        {

        }

        #region Load Content
        public override void Load()
        {
            text = new AnimatedText("Press Any Key to Continue", ButtonIcon.None, 2f, Alignment.Centre);
            text.AnchorPoint = ScreenAnchorLocation.Centre;
            text.TextType = AnimatedTextType.Grow;

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
            if (InputManager.Instance.isGamePad)
            {
                if (InputManager.Instance.Menu() || InputManager.Instance.GP_A || InputManager.Instance.GP_B)
                    ScreenManager.FadeOut(null);
            }
            else
            {
                if (InputManager.Instance.Menu() || InputManager.Instance.Space || InputManager.Instance.lMouseButton || InputManager.Instance.Enter)
                    ScreenManager.FadeOut(null);
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
            sb.End();
        }
        #endregion
    }
}