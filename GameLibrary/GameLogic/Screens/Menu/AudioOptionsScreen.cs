using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameLogic.Controls;
using Microsoft.Xna.Framework;
using GameLibrary.GameLogic.Screens.Menu.Options;

namespace GameLibrary.GameLogic.Screens.Menu
{
    public class AudioOptionsScreen : MenuScreen
    {
        GameSettings _settingsInstance = GameSettings.Instance;

        MenuEntry _soundEntry;
        MenuEntry _musicEntry;

        public AudioOptionsScreen()
            : base("Audio")
        {
            this._soundEntry = new MenuEntry("Sound Volume:");
            this._musicEntry = new MenuEntry("Music Volume:");
            MenuEntry sep = new MenuEntry("sep");
            MenuEntry back = new MenuEntry("Back");

            this._soundEntry.Origin = Graphics.UI.TextAlignment.Centre;
            this._musicEntry.Origin = Graphics.UI.TextAlignment.Centre;
            back.Origin = Graphics.UI.TextAlignment.Centre;

            sep.ItemType = MenuEntryType.Separator;

            back.Selected += OnCancel;
            

            menuEntries.Add(this._soundEntry);
            menuEntries.Add(this._musicEntry);
            menuEntries.Add(sep);
            menuEntries.Add(back);

        }

        public override void HandleInput(float delta, InputState input)
        {
            PlayerIndex playerIndex;

            #region Move Up and Down the List
            // Move to the previous menu entry?
            if (menuUp.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                {
                    selectedEntry = menuEntries.Count - 1;
                }

                while (menuEntries[selectedEntry].ItemType == MenuEntryType.Separator || !menuEntries[selectedEntry].Enabled)
                {
                    selectedEntry--;

                    if (selectedEntry < 0)
                    {
                        selectedEntry = menuEntries.Count - 1;
                    }
                }
            }

            // Move to the next menu entry?
            if (menuDown.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                {
                    selectedEntry = 0;
                }

                while (menuEntries[selectedEntry].ItemType == MenuEntryType.Separator || !menuEntries[selectedEntry].Enabled)
                {
                    selectedEntry++;
                    if (selectedEntry >= menuEntries.Count)
                    {
                        selectedEntry = 0;
                    }
                }
            }
            #endregion

            #region Cycle through the levels

            if (menuLeft.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                
            }

            if (menuRight.Evaluate(input, ControllingPlayer, out playerIndex))
            {

            }

            if (menuSelect.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                OnSelectEntry(selectedEntry, playerIndex);
            }
            else if (menuCancel.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                OnCancel(playerIndex);
            }

            #endregion

            this.SetMenuEntryText();
        }

        void SetMenuEntryText()
        {
            this._soundEntry.Text = "Sound Volume: " + _settingsInstance.SoundVolume;
            this._musicEntry.Text = "Music Volume: " + _settingsInstance.MusicVolume;
        }
    }
}
