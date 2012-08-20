using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameLogic.Controls;
using Microsoft.Xna.Framework;
using GameLibrary.GameLogic.Screens.Menu.Options;
using GameLibrary.System;

namespace GameLibrary.GameLogic.Screens.Menu
{
    public class AudioOptionsScreen : MenuScreen
    {
        GameSettings _settingsInstance = GameSettings.Instance;

        MenuEntry _ambienceEntry;
        MenuEntry _musicEntry;
        MenuEntry _voiceEntry;
        MenuEntry _effectsEntry;

        public AudioOptionsScreen()
            : base("Audio")
        {
            this._ambienceEntry = new MenuEntry("Sound Volume: ");
            this._musicEntry = new MenuEntry("Music Volume: ");
            this._voiceEntry = new MenuEntry("Voice Volume: ");
            this._effectsEntry = new MenuEntry("Effects Volume: ");
            MenuEntry sep = new MenuEntry("sep");
            MenuEntry back = new MenuEntry("Back");

            this._ambienceEntry.Origin = Graphics.UI.TextAlignment.Centre;
            this._musicEntry.Origin = Graphics.UI.TextAlignment.Centre;
            this._voiceEntry.Origin = Graphics.UI.TextAlignment.Centre;
            this._effectsEntry.Origin = Graphics.UI.TextAlignment.Centre;
            back.Origin = Graphics.UI.TextAlignment.Centre;

            sep.ItemType = MenuEntryType.Separator;

            back.Selected += OnCancel;
            

            menuEntries.Add(_ambienceEntry);
            menuEntries.Add(_effectsEntry);
            menuEntries.Add(_musicEntry);
            menuEntries.Add(_voiceEntry);
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
                switch (selectedEntry)
                {
                    case 0:
                        {
                            _settingsInstance.AmbienceVolume--;
                        }
                        break;
                    case 1:
                        {
                            _settingsInstance.EffectsVolume--;
                        }
                        break;
                    case 2:
                        {
                            _settingsInstance.MusicVolume--;
                        }
                        break;
                    case 3:
                        {
                            _settingsInstance.VoiceVolume--;
                        }
                        break;
                    default:
                        break;
                }
            }

            if (menuRight.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                switch (selectedEntry)
                {
                    case 0:
                        {
                            _settingsInstance.AmbienceVolume++;
                        }
                        break;
                    case 1:
                        {
                            _settingsInstance.EffectsVolume++;
                        }
                        break;
                    case 2:
                        {
                            _settingsInstance.MusicVolume++;
                        }
                        break;
                    case 3:
                        {
                            _settingsInstance.VoiceVolume++;
                        }
                        break;
                    default:
                        break;
                }
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
            this._ambienceEntry.Text = "Ambience Volume: " + _settingsInstance.AmbienceVolume;
            this._musicEntry.Text = "Music Volume: " + _settingsInstance.MusicVolume;
            this._effectsEntry.Text = "Effects Volume: " + _settingsInstance.EffectsVolume;
            this._voiceEntry.Text = "Voice Volume: " + _settingsInstance.VoiceVolume;
        }

        public override void Activate()
        {
            this._itemsPosition = new Vector2(
                this.ScreenManager.GraphicsDevice.Viewport.Width * 0.5f,
                this.ScreenManager.GraphicsDevice.Viewport.Height * 0.33f);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            SaveManager.Instance.SaveSettings();
            base.OnCancel(playerIndex);
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
    }
}
