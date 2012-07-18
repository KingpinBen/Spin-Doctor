#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Graphics;
using GameLibrary.GameLogic.Controls;
using GameLibrary.GameLogic.Screens.Menu.Options;
using GameLibrary.GameLogic.Screens.Menu;
#endregion

namespace GameLibrary.GameLogic.Screens.Messages
{
    public class ConfirmBox : MenuScreen
    {
        #region Fields and Variables
        public string Text { get; protected set; }
        private float fontHeight;
        private float fontWidth;
        private Texture2D _texture;
        #endregion

        #region Constructor
        public ConfirmBox(GraphicsDevice graphics)
            : base("ConfirmBox", graphics)
        {
            this.isPopUp = true;
            this._menuArrayCount = new Point(2, 1);
            this._menuItemArray = new MenuItem[_menuArrayCount.X, _menuArrayCount.Y];
        }
        #endregion

        #region Load
        public override void Load()
        {
            base.Load();
            this.Font = FontManager.Instance.GetFont(Graphics.FontList.MenuTitle).Font;

            this.isPopUp = true;
            this.Text = "Are you sure?";
            this.fontWidth = Font.MeasureString(Text).X;
            this.fontHeight = Font.MeasureString("A").Y;

            this._texture = _content.Load<Texture2D>("Assets/Images/Basics/gradient");

            Vector2 screenCentre = (new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height) * 0.5f);
            Vector2 offset = new Vector2(50, 0);

            MenuItem Yes = new MenuItem("Yes", OptionType.Opt1, screenCentre - offset);
            MenuItem No = new MenuItem("No", OptionType.Opt2, screenCentre + offset);

            this._menuItemArray[1, 0] = Yes;
            this._menuItemArray[0, 0] = No;
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.Return())
            {
                CompleteAction(OptionType.Opt2);
            }

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch sb)
        {
            sb.Begin();

            sb.Draw(_texture, new Rectangle(
                (int)(ScreenManager.GraphicsDevice.Viewport.Width * 0.5f) - 200,
                (int)(ScreenManager.GraphicsDevice.Viewport.Height * 0.5f) - 130,
                400,
                175), null, Color.SandyBrown);

            base.Draw(sb);

            sb.DrawString(Font, this.Text, (new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height) * 0.5f) + Vector2.UnitY * -100, 
                Color.White, 0f, new Vector2(fontWidth / 2, fontHeight / 2), 1f, SpriteEffects.None, 0.99f);

            sb.End();
        }
        #endregion

        #region CompleteAction
        protected override void CompleteAction(OptionType type)
        {
            switch (type)
            {
                case OptionType.Opt1:
                    ScreenManager.LoadLevel(0);
                    break;
                case OptionType.Opt2:
                    ScreenManager.DeleteScreen();
                    break;
            }
        }
        #endregion
    }
}
