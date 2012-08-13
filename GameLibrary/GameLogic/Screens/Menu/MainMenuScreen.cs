using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameLogic.Screens.Menu.Options;
using Microsoft.Xna.Framework;
using GameLibrary.System;

namespace GameLibrary.GameLogic.Screens.Menu
{
    public class MainMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Main Menu")
        {
            //this._mainPosition = new Vector2(this.ScreenManager.GraphicsDevice.Viewport.Width * 0.5f, this.ScreenManager.GraphicsDevice.Viewport.Height * 0.33f);

            // Create our menu entries.

            SaveManager.Instance.LoadGame();

            if (SaveManager.Instance.Loaded)
            {
                MenuEntry continueMenuEntry = new MenuEntry("Continue");
                continueMenuEntry.Origin = Graphics.UI.TextAlignment.Centre;
                continueMenuEntry.Selected += StartGame;
                menuEntries.Add(continueMenuEntry);
            }

            MenuEntry playGameMenuEntry = new MenuEntry("New Game");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Origin= Graphics.UI.TextAlignment.Centre;
            optionsMenuEntry.Origin = Graphics.UI.TextAlignment.Centre;
            exitMenuEntry.Origin = Graphics.UI.TextAlignment.Centre;

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += NewGameSelected;
            optionsMenuEntry.Selected += OptionsSelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void Activate()
        {
            base.Activate();
        }

        #endregion

        #region Handle Input

        void NewGameSelected(object sender, PlayerIndexEventArgs e)
        {
            SaveManager.Instance.NewGame();
            StartGame(sender, e);
        }

        void OptionsSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        void StartGame(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
        }


        #endregion
    }
}
