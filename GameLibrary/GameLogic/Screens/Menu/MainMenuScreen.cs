using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameLogic.Screens.Menu.Options;
using Microsoft.Xna.Framework;
using GameLibrary.System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.GameLogic.Screens.Menu
{
    public class MainMenuScreen : MenuScreen
    {
        #region Fields

        private ContentManager content;
        private Texture2D _gameLogo;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Main Menu")
        {
            //  Check if there is already a game save file.
            SaveManager.Instance.LoadGame();

            //  If a save has been found, we want to display a way of continuing
            //  the game.
            if (SaveManager.Instance.FoundSave)
            {
                MenuEntry continueMenuEntry = new MenuEntry("Continue");
                continueMenuEntry.Origin = Graphics.UI.TextAlignment.Centre;
                continueMenuEntry.Selected += StartGame;
                menuEntries.Add(continueMenuEntry);
            }

            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("New Game");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry creditsMenuEntry = new MenuEntry("Credits");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Origin= Graphics.UI.TextAlignment.Centre;
            optionsMenuEntry.Origin = Graphics.UI.TextAlignment.Centre;
            creditsMenuEntry.Origin = Graphics.UI.TextAlignment.Centre;
            exitMenuEntry.Origin = Graphics.UI.TextAlignment.Centre;

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += NewGameSelected;
            optionsMenuEntry.Selected += OptionsSelected;
            creditsMenuEntry.Selected += CreditsSelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void Activate()
        {
            base.Activate();

            if (content == null)
            {
                this.content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            this._gameLogo = content.Load<Texture2D>("Assets/Other/Game/TitleLogo");
        }

        #endregion

        #region Handle Input

        private void NewGameSelected(object sender, PlayerIndexEventArgs e)
        {
            SaveManager.Instance.NewGame();
            StartGame(sender, e);
        }

        private void OptionsSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        private void CreditsSelected(object sender, PlayerIndexEventArgs e)
        {

            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new CreditHandler(), new GameplayScreen());
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit the game?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        private void StartGame(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
        }


        #endregion

        public override void Draw(GameTime gameTime)
        {
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            Vector2 logoPosition = new Vector2(graphics.Viewport.Width * 0.75f,
                graphics.Viewport.Height * 0.25f);

            float scale = 0.5f;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                if (menuEntry.ItemType != MenuEntryType.Separator)
                    menuEntry.Draw(this, isSelected, gameTime);
            }

            spriteBatch.Draw(_gameLogo, logoPosition, null,
                Color.White * TransitionAlpha,
                0.0f, new Vector2(_gameLogo.Width, _gameLogo.Height) * 0.5f, scale, SpriteEffects.None,0.0f);

            spriteBatch.End();
        }
    }
}
