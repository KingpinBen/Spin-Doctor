using System;
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
                return new Vector2(Screen_Manager.GraphicsDevice.Viewport.Width, Screen_Manager.GraphicsDevice.Viewport.Height) * 0.5f;
            }
        }

        #endregion

        public GameMenu()
            : base()
        {
            _menuArrayCount = new Point(1, 3);

            _menuItemArray = new MenuOption[_menuArrayCount.X, _menuArrayCount.Y];
        }

        public override void Load()
        {
            base.Load();

            _headerTexture = _content.Load
                <Texture2D>("Assets/Other/Game/TitleLogo");

            #region Create the menu items
            Vector2 MenuItemVerticalSpacing = Vector2.UnitY * 10;
            Vector2 fontHeight = Vector2.UnitY * (int)Fonts.MenuFont.MeasureString("A").Y;

            //  Create the menu items
            MenuOption Resume = new MenuOption("Resume", optionType.Opt1, MenuPosition + fontHeight + (MenuItemVerticalSpacing * 0));
            MenuOption GotoHub = new MenuOption("Return To Hub", optionType.Opt2, MenuPosition + (fontHeight * 2) + (MenuItemVerticalSpacing * 1));
            MenuOption Quit = new MenuOption("Quit", optionType.Opt3, MenuPosition + (fontHeight * 3) + (MenuItemVerticalSpacing * 2));

            _menuItemArray[0, 0] = Resume;
            _menuItemArray[0, 1] = GotoHub;
            _menuItemArray[0, 2] = Quit;
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
            sb.Draw(_headerTexture, (new Vector2(Screen_Manager.GraphicsDevice.Viewport.Width, Screen_Manager.GraphicsDevice.Viewport.Height) * 0.5f) + new Vector2(0, -_headerTexture.Height / 3),
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
