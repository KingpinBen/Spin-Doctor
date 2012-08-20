using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameLogic.Screens.Menu.Options;
using Microsoft.Xna.Framework;
using GameLibrary.Helpers;
using GameLibrary.Audio;

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

            float height = menuEntries[15].GetHeight(this);

            //  Remove3x the height to centre them on the screen.
            position.Y -= height * 3;

            //  Create a 4x4 grid with all the entries in.
            float width = (menuEntries[15].GetWidth(this) * 0.5f) + 20; 

            for (int i = 0; i < 4; i++)
            {
                int j = i * 4; 
                menuEntries[j].Position = new Vector2(position.X - (width + 100 * 2), position.Y);
                menuEntries[j + 1].Position = new Vector2(position.X - width, position.Y); ;
                menuEntries[j + 2].Position = new Vector2(position.X + width, position.Y); ;
                menuEntries[j + 3].Position = new Vector2(position.X + (width + 100 * 2), position.Y); ;

                position.Y += menuEntries[i].GetHeight(this);
            }

            menuEntries[16].Position = new Vector2(_itemsPosition.X, position.Y);
            position.Y += menuEntries[0].GetHeight(this);

            menuEntries[17].Position = new Vector2(_itemsPosition.X, position.Y);
        }

        public override void Draw(GameTime gameTime)
        {
            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack((TransitionAlpha * 2) * 0.33f);

            base.Draw(gameTime);
        }

        void JournalEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            this.ScreenManager.AddScreen(new MessageOverlay(selectedEntry + 1), e.PlayerIndex);
            AudioManager.Instance.PlayCue("Journal_Turn_Page", true);
        }
    }
}
