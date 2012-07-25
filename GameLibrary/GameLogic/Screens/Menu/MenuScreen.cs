using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameLogic.Screens.Menu.Options;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Graphics;

namespace GameLibrary.GameLogic.Screens.Menu
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    public abstract class MenuScreen : GameScreen
    {
        #region Fields

        protected List<MenuEntry> menuEntries = new List<MenuEntry>();
        protected int selectedEntry = 0;
        string menuTitle;
        SpriteFont _titleFont;
        protected Vector2 _itemsPosition;

        InputAction menuUp;
        InputAction menuDown;
        InputAction menuSelect;
        InputAction menuCancel;

        #endregion

        #region Properties


        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }


        #endregion

        #region Initialization

        public MenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;
            this._titleFont = FontManager.Instance.GetFont(FontList.MenuTitle);

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            menuUp = new InputAction(
                new Buttons[] { Buttons.DPadUp, Buttons.LeftThumbstickUp },
                new Keys[] { Keys.Up },
                true);
            menuDown = new InputAction(
                new Buttons[] { Buttons.DPadDown, Buttons.LeftThumbstickDown },
                new Keys[] { Keys.Down },
                true);
            menuSelect = new InputAction(
                new Buttons[] { Buttons.A, Buttons.Start },
                new Keys[] { Keys.Enter, Keys.Space },
                true);
            menuCancel = new InputAction(
                new Buttons[] { Buttons.B, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);
        }

        public override void Activate()
        {
            _itemsPosition = new Vector2(0, ScreenManager.GraphicsDevice.Viewport.Height * 0.75f);

            base.Activate();
        }

        #endregion

        #region Handle Input

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;

            // Move to the previous menu entry?
            if (menuUp.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (menuDown.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }

            if (menuSelect.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                OnSelectEntry(selectedEntry, playerIndex);
            }
            else if (menuCancel.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                OnCancel(playerIndex);
            }
        }

        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }

        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }


        #endregion

        #region Update and Draw

        protected virtual void UpdateMenuEntryLocations()
        {
            Vector2 position = _itemsPosition;

            for (int i = menuEntries.Count - 1; i >= 0; i--)
            {
                MenuEntry menuEntry = menuEntries[i];

                position.X = (ScreenManager.GraphicsDevice.Viewport.Width - 40.0f ) - menuEntry.GetWidth(this) * 0.5f;
                menuEntry.Position = position;

                position.Y -= menuEntry.GetHeight(this);
            }
        }

        public override void Update(float delta, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(delta, otherScreenHasFocus, coveredByOtherScreen);

            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, delta);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 titlePosition = new Vector2(graphics.Viewport.Width * 0.5f, 80);
            Vector2 titleOrigin = _titleFont.MeasureString(menuTitle) * 0.5f;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            spriteBatch.DrawString(_titleFont, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }


        #endregion
    }
}
