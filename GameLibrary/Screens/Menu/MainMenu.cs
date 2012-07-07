using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Assists;
using GameLibrary.Managers;

namespace GameLibrary.Screens.Menu
{
    public class MainMenu : MenuScreen
    {
        private Vector2 menuPosition
        {
            get
            {
                PresentationParameters pp = this._graphicsDevice.PresentationParameters;

                return new Vector2(pp.BackBufferWidth * 0.75f, pp.BackBufferHeight * 0.5f);
            }
        }

        public MainMenu(GraphicsDevice graphics)
            : base("Main Menu", graphics)
        {
            this._menuArrayCount = new Point(1, 3);
            this._menuItemArray = new MenuOption[_menuArrayCount.X, _menuArrayCount.Y];
        }

        public override void Load()
        {
            base.Load();

            Vector2 MenuItemVerticalSpacing = Vector2.UnitY * 10;
            Vector2 fontHeight = Vector2.UnitY * (int)Fonts.MenuFont.MeasureString("A").Y;

            MenuOption toGame = new MenuOption("Play Game", optionType.Opt1, this.menuPosition);
            MenuOption options = new MenuOption("Options", optionType.Opt2, this.menuPosition + (fontHeight * 2) + (MenuItemVerticalSpacing * 1));
            MenuOption exit = new MenuOption("Exit", optionType.Opt3, this.menuPosition + (fontHeight * 3) + (MenuItemVerticalSpacing * 2));

            _menuItemArray[0, 0] = toGame;
            _menuItemArray[0, 1] = options;
            _menuItemArray[0, 2] = exit;

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Screen_Manager.Textures[0], Vector2.Zero, new Rectangle(0, 0, (int)this._graphicsDevice.Viewport.Width, (int)this._graphicsDevice.Viewport.Height), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

            for (int x = 0; x < _menuArrayCount.X; x++)
            {
                for (int y = 0; y < _menuArrayCount.Y; y++)
                {
                    _menuItemArray[x, y].Draw(sb);
                }
            }
            sb.End();
        }

        protected override void CompleteAction(optionType type)
        {
            if (type == optionType.Opt1)
            {
                List<Screen> toAdd = new List<Screen>();
                GameplayScreen gamescreen = new GameplayScreen(0, this._graphicsDevice);
                toAdd.Add(gamescreen);

                Screen_Manager.FadeOut(toAdd);
            }
            else if (type == optionType.Opt2)
            {
                //  TODO: OPEN OPTIONS SCREEN.
            }
            else if (type == optionType.Opt3)
            {
                Screen_Manager.ExitGame();
            }
        }
    }
}
