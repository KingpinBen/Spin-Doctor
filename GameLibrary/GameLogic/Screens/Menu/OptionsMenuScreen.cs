using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameLogic.Screens.Menu.Options;
using Microsoft.Xna.Framework;

namespace GameLibrary.GameLogic.Screens.Menu
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry graphicsEntry;
        MenuEntry audioEntry;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            graphicsEntry = new MenuEntry("Display");
            audioEntry = new MenuEntry("Audio");
            MenuEntry sep = new MenuEntry("Sep");
            MenuEntry back = new MenuEntry("Back");

            graphicsEntry.Origin = Graphics.UI.TextAlignment.Centre;
            audioEntry.Origin = Graphics.UI.TextAlignment.Centre;
            sep.ItemType = MenuEntryType.Separator;
            back.Origin = Graphics.UI.TextAlignment.Centre;

            // Hook up menu event handlers.
            graphicsEntry.Selected += GraphicsEntrySelected;
            audioEntry.Selected += AudioEntrySelected;

            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(graphicsEntry);
            MenuEntries.Add(audioEntry);
            menuEntries.Add(sep);
            MenuEntries.Add(back);
        }

        public override void Activate()
        {
            this._itemsPosition = new Vector2(
                this.ScreenManager.GraphicsDevice.Viewport.Width * 0.5f,
                this.ScreenManager.GraphicsDevice.Viewport.Height * 0.33f);
        }

        protected override void UpdateMenuEntryLocations()
        {
            Vector2 position = _itemsPosition;

            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry entry = menuEntries[i];

                position.X = _itemsPosition.X;
                entry.Position = position;
                position.Y += entry.GetHeight(this);
            }
        }

        #endregion

        #region Handle Input

        void GraphicsEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new GraphicsOptionsScreen(), e.PlayerIndex);
        }

        void AudioEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new AudioOptionsScreen(), e.PlayerIndex);
        }


        #endregion
    }
}
