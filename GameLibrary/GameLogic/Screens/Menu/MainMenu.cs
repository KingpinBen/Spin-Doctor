using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Graphics;
using GameLibrary.GameLogic.Screens.Menu.Options;
using GameLibrary.GameLogic;

namespace GameLibrary.GameLogic.Screens.Menu
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
            this._menuItemArray = new MenuItem[_menuArrayCount.X, _menuArrayCount.Y];
        }

        public override void Load()
        {
            base.Load();

            Vector2 MenuItemVerticalSpacing = Vector2.UnitY * 10;
            Vector2 fontHeight = Vector2.UnitY * FontManager.Instance.GetFont(Graphics.FontList.MenuOption).Height;

            MenuItem toGame = new MenuItem("Play Game", OptionType.Opt1, this.menuPosition);
            MenuItem options = new MenuItem("Options", OptionType.Opt2, this.menuPosition + (fontHeight * 2) + (MenuItemVerticalSpacing * 1));
            MenuItem exit = new MenuItem("Exit", OptionType.Opt3, this.menuPosition + (fontHeight * 3) + (MenuItemVerticalSpacing * 2));

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
            sb.Draw(ScreenManager.Textures[0], Vector2.Zero, new Rectangle(0, 0, (int)this._graphicsDevice.Viewport.Width, (int)this._graphicsDevice.Viewport.Height), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

            for (int x = 0; x < _menuArrayCount.X; x++)
            {
                for (int y = 0; y < _menuArrayCount.Y; y++)
                {
                    _menuItemArray[x, y].Draw(sb);
                }
            }
            sb.End();
        }

        protected override void CompleteAction(OptionType type)
        {
            if (type == OptionType.Opt1)
            {
                List<Screen> toAdd = new List<Screen>();
                GameplayScreen gamescreen = new GameplayScreen(0, this._graphicsDevice);
                toAdd.Add(gamescreen);

                ScreenManager.FadeOut(toAdd);
            }
            else if (type == OptionType.Opt2)
            {
                //  TODO: OPEN OPTIONS SCREEN.
            }
            else if (type == OptionType.Opt3)
            {
                ScreenManager.ExitGame();
            }
        }
    }
}
