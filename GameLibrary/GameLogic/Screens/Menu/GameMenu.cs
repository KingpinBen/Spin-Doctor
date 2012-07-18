using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Graphics;
using GameLibrary.GameLogic.Screens.Menu.Options;
using GameLibrary.GameLogic;
using GameLibrary.GameLogic.Screens.Messages;

namespace GameLibrary.GameLogic.Screens.Menu
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
                return new Vector2(this._graphicsDevice.Viewport.Width, this._graphicsDevice.Viewport.Height) * 0.5f;
            }
        }

        #endregion

        public GameMenu(GraphicsDevice graphics)
            : base(graphics)
        {
            this._menuArrayCount = new Point(1, 4);
            this._menuItemArray = new MenuItem[_menuArrayCount.X, _menuArrayCount.Y];
        }

        public override void Load()
        {
            base.Load();

            _headerTexture = _content.Load
                <Texture2D>("Assets/Other/Game/TitleLogo");

            #region Create the menu items
            Vector2 MenuItemVerticalSpacing = Vector2.UnitY * 10;
            Vector2 fontHeight = Vector2.UnitY * FontManager.Instance.GetFont(Graphics.FontList.MenuOption).Height;

            //  Create the menu items
            MenuItem Resume = new MenuItem("Resume", OptionType.Opt1, MenuPosition + fontHeight + (MenuItemVerticalSpacing * 0));
            MenuItem ResetLevel = new MenuItem("Restart Level", OptionType.Opt2, MenuPosition + (fontHeight * 2) + (MenuItemVerticalSpacing));
            MenuItem GotoHub = new MenuItem("Return To Hub", OptionType.Opt3, MenuPosition + (fontHeight * 3) + (MenuItemVerticalSpacing * 2));
            MenuItem Quit = new MenuItem("Quit", OptionType.Opt4, MenuPosition + (fontHeight * 4) + (MenuItemVerticalSpacing * 3));

            _menuItemArray[0, 0] = Resume;
            _menuItemArray[0, 1] = ResetLevel;
            _menuItemArray[0, 2] = GotoHub;
            _menuItemArray[0, 3] = Quit;
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
            sb.Draw(_headerTexture, (new Vector2(this._graphicsDevice.Viewport.Width, this._graphicsDevice.Viewport.Height) * 0.5f) + new Vector2(0, -_headerTexture.Height / 3),
                null, Color.White, 0f, new Vector2(_headerTexture.Width / 2, _headerTexture.Height / 2),
                0.6f, SpriteEffects.None, 0f);

            sb.End();
        }

        #region CompleteAction
        /// <summary>
        /// Depending on what type of menu option the object is, when called
        /// a certain action will occur:
        /// </summary>
        protected override void CompleteAction(OptionType type)
        {
            switch (type)
            {
                case OptionType.Opt1:
                    {
                        ScreenManager.DeleteScreen(); 
                        break;
                    }
                case OptionType.Opt2:
                    {
                        GameplayScreen.LoadLevel();
                        ScreenManager.DeleteScreen();
                        break;
                    }
                case OptionType.Opt3:
                    {
                        ConfirmBox newBox = new ConfirmBox(this._graphicsDevice);
                        ScreenManager.AddScreen(newBox);
                        break;
                    }
                case OptionType.Opt4:
                    {
                        ScreenManager.ExitGame(); 
                        break;
                    }
                default: break;
            }
        }
        #endregion
    }
}
