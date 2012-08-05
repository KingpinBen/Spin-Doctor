using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameLogic.Screens.Menu.Options;
using Microsoft.Xna.Framework;
using GameLibrary.Helpers;

namespace GameLibrary.GameLogic.Screens.Menu
{
    public class JournalScreen : MenuScreen
    {
        public JournalScreen()
            : base("Journal")
        {
            MenuEntry journalEntry;
            List<bool> collectedNotes = GameSettings.Instance.FoundEntries;
            for(int i = 0; i < GameSettings.Instance.FoundEntries.Count; i++)
            {
                journalEntry = new MenuEntry("Entry " + (i + 1).ToString());
                journalEntry.Origin = Graphics.UI.TextAlignment.Centre;
                journalEntry.Enabled = collectedNotes[i];
                journalEntry.Selected += JournalEntrySelected;
                menuEntries.Add(journalEntry);
            }

            MenuEntry back = new MenuEntry("Back");
            back.Selected += OnCancel;
            back.Origin = Graphics.UI.TextAlignment.Centre;

            MenuEntry sep = new MenuEntry("sep");
            sep.ItemType = MenuEntryType.Separator;

            menuEntries.Add(sep);
            menuEntries.Add(back);

            //  Make it select back as the default/start position in the entry list.
            selectedEntry = menuEntries.Count - 1;
        }

        public override void Activate()
        {
            this._itemsPosition = new Vector2(
                this.ScreenManager.GraphicsDevice.Viewport.Width * 0.5f, 
                this.ScreenManager.GraphicsDevice.Viewport.Height * 0.5f);
        }

        protected override void UpdateMenuEntryLocations()
        {
            Vector2 position = _itemsPosition;

            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                menuEntry.Position = position;

                position.Y += menuEntry.GetHeight(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack((TransitionAlpha * 2) * 0.33f);

            base.Draw(gameTime);
        }

        void JournalEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            this.ScreenManager.AddScreen(new MessageOverlay(Defines.NOTE_DIRECTORY + (selectedEntry + 1).ToString()), e.PlayerIndex);
        }
    }
}
