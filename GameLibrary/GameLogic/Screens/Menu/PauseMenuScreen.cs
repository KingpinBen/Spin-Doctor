using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameLogic.Screens.Menu.Options;
using GameLibrary.Graphics.UI;
using Microsoft.Xna.Framework;

namespace GameLibrary.GameLogic.Screens.Menu
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        #region Initialization

        public PauseMenuScreen()
            : base("Paused")
        {
            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry restartGameMenuEntry = new MenuEntry("Restart Level");
            MenuEntry HUBGameMenuEntry = new MenuEntry("Return to Hub");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            resumeGameMenuEntry.Origin = TextAlignment.Centre;
            restartGameMenuEntry.Origin = TextAlignment.Centre;
            HUBGameMenuEntry.Origin = TextAlignment.Centre;
            quitGameMenuEntry.Origin = TextAlignment.Centre;

            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            restartGameMenuEntry.Selected += RestartLevelMenuEntrySelected;
            HUBGameMenuEntry.Selected += ReturnHUBMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(restartGameMenuEntry);
            MenuEntries.Add(HUBGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        #endregion

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

        #region Handle Input

        void RestartLevelMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

        }

        void ReturnHUBMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Return to Hub?";

            MessageBoxScreen confirmReturnMessageBox = new MessageBoxScreen(message);

            confirmReturnMessageBox.Accepted += ConfirmReturnMessageBoxAccepted;

            ScreenManager.AddScreen(confirmReturnMessageBox, ControllingPlayer);
        }

        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        void ConfirmReturnMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, PlayerIndex.One, new GameplayScreen(0));
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen(this.ScreenManager.StartLevel));
        }


        #endregion
    }
}
