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
            MenuEntry journalMenuEntry = new MenuEntry("Open Journal");
            MenuEntry sep = new MenuEntry("sep");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            resumeGameMenuEntry.Origin = TextAlignment.Centre;
            restartGameMenuEntry.Origin = TextAlignment.Centre;
            HUBGameMenuEntry.Origin = TextAlignment.Centre;
            journalMenuEntry.Origin = TextAlignment.Centre;
            quitGameMenuEntry.Origin = TextAlignment.Centre;

            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            restartGameMenuEntry.Selected += RestartLevelMenuEntrySelected;
            HUBGameMenuEntry.Selected += ReturnHUBMenuEntrySelected;
            journalMenuEntry.Selected += JournalMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            sep.ItemType = MenuEntryType.Separator;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(restartGameMenuEntry);
            MenuEntries.Add(HUBGameMenuEntry);
            MenuEntries.Add(journalMenuEntry);
            MenuEntries.Add(sep);
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
            LoadingScreen.Load(ScreenManager, false, PlayerIndex.One, new GameplayScreen());
        }

        void JournalMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            this.ScreenManager.AddScreen(new JournalScreen(), e.PlayerIndex);
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
            GameScreen[] screenList = this.ScreenManager.GetScreens();

            for (int i = 0; i < screenList.Length; i++)
            {
                if (screenList[i].GetType() == typeof(GameplayScreen))
                {
                    ((GameplayScreen)screenList[i]).CurrentLevelID = 0;
                }
            }
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }


        #endregion

        public override void Draw(GameTime gameTime)
        {
            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack((TransitionAlpha * 2) * 0.33f);

            base.Draw(gameTime);
        }
    }
}
