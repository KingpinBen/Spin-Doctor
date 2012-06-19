﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Assists;
using Microsoft.Xna.Framework;
using GameLibrary.Screens.Messages;
using GameLibrary.Managers;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Screens.Menu
{
    public class GameMenu : MenuScreen
    {
        #region Fields and Variables
        private Texture2D _headerTexture;

        public Vector2 MenuPosition
        {
            get
            {
                //  Middle of the screen.
                return Screen_Manager.Viewport / 2;
            }
        }

        #endregion

        public GameMenu()
            : base()
        {
            MenuArray = new Point(1, 3);
            SelectionOption = new Point(0, 0);

            MenuItemArray = new MenuOption[MenuArray.X, MenuArray.Y];
        }

        public override void Load()
        {
            base.Load();

            _headerTexture = Content.Load
                <Texture2D>("Assets/Other/Game/TitleLogo");

            #region Create the menu items
            Vector2 MenuItemVerticalSpacing = Vector2.UnitY * 10;
            Vector2 fontHeight = Vector2.UnitY * (int)Fonts.MenuFont.MeasureString("A").Y;

            //  Create the menu items
            MenuOption Resume = new MenuOption("Resume", optionType.Opt1, MenuPosition + fontHeight + (MenuItemVerticalSpacing * 0));
            MenuOption GotoHub = new MenuOption("Return To Hub", optionType.Opt2, MenuPosition + (fontHeight * 2) + (MenuItemVerticalSpacing * 1));
            MenuOption Quit = new MenuOption("Quit", optionType.Opt3, MenuPosition + (fontHeight * 3) + (MenuItemVerticalSpacing * 2));

            MenuItemArray[0, 0] = Resume;
            MenuItemArray[0, 1] = GotoHub;
            MenuItemArray[0, 2] = Quit;
            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Begin();

            base.Draw(sb);
            sb.Draw(_headerTexture, (Screen_Manager.Viewport / 2) + new Vector2(0, -_headerTexture.Height / 3),
                null, Color.White, 0f, new Vector2(_headerTexture.Width / 2, _headerTexture.Height / 2),
                0.6f, SpriteEffects.None, 0f);

            sb.End();
        }

        #region CompleteAction
        /// <summary>
        /// Depending on what type of menu option the object is, when called
        /// a certain action will occur:
        /// </summary>
        protected override void CompleteAction(optionType type)
        {
            switch (type)
            {
                case optionType.Opt1:
                    {
                        Screen_Manager.DeleteScreen(); 
                        break;
                    }
                case optionType.Opt2:
                    {
                        ConfirmBox newBox = new ConfirmBox();
                        Screen_Manager.AddScreen(newBox);
                        break;
                    }
                case optionType.Opt3:
                    {
                        Screen_Manager.ExitGame(); 
                        break;
                    }
            }
        }
        #endregion
    }
}
