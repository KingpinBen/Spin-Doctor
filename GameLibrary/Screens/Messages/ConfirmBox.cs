#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Drawing;
using GameLibrary.Assists;
using GameLibrary.Screens.Menu;
using GameLibrary.Managers;
using Microsoft.Xna.Framework.Content;
#endregion

namespace GameLibrary.Screens.Messages
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
        public ConfirmBox()
            : base("ConfirmBox")
        {
            this.isPopUp = true;
            this._menuArrayCount = new Point(2, 1);
            this._menuItemArray = new MenuOption[_menuArrayCount.X, _menuArrayCount.Y];
        }
        #endregion

        #region Load
        public override void Load()
        {
            base.Load();
            this.Font = Fonts.MenuFont;

            this.isPopUp = true;
            this.Text = "Are you sure?";
            this.fontWidth = Font.MeasureString(Text).X;
            this.fontHeight = Font.MeasureString("A").Y;

            this._texture = _content.Load<Texture2D>("Assets/Images/Basics/gradient");

            Vector2 screenCentre = (new Vector2(Screen_Manager.GraphicsDevice.Viewport.Width, Screen_Manager.GraphicsDevice.Viewport.Height) * 0.5f);
            Vector2 offset = new Vector2(50, 0);

            MenuOption Yes = new MenuOption("Yes", optionType.Opt1, screenCentre - offset);
            MenuOption No =  new MenuOption("No", optionType.Opt2,  screenCentre + offset);

            this._menuItemArray[1, 0] = Yes;
            this._menuItemArray[0, 0] = No;
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            if (Input.Return())
                CompleteAction(optionType.Opt2);

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch sb)
        {
            sb.Begin();

            sb.Draw(_texture, new Rectangle(
                (int)(Screen_Manager.GraphicsDevice.Viewport.Width * 0.5f) - 200,
                (int)(Screen_Manager.GraphicsDevice.Viewport.Height * 0.5f) - 130,
                400,
                175), null, Color.SandyBrown);

            base.Draw(sb);

            sb.DrawString(Font, this.Text, (new Vector2(Screen_Manager.GraphicsDevice.Viewport.Width, Screen_Manager.GraphicsDevice.Viewport.Height) * 0.5f) + Vector2.UnitY * -100, 
                Color.White, 0f, new Vector2(fontWidth / 2, fontHeight / 2), 1f, SpriteEffects.None, 0.99f);

            sb.End();
        }
        #endregion

        #region CompleteAction
        protected override void CompleteAction(optionType type)
        {
            switch (type)
            {
                case optionType.Opt1:
                    Screen_Manager.LoadLevel(0);
                    break;
                case optionType.Opt2:
                    Screen_Manager.DeleteScreen();
                    break;
            }
        }
        #endregion
    }
}
