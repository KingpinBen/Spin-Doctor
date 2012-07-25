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

        MenuEntry ungulateMenuEntry;
        MenuEntry languageMenuEntry;
        MenuEntry fullscreenMenuEntry;
        MenuEntry elfMenuEntry;

        Resolutions graphics_Resolution= Resolutions.C;

        string[] languages = { "C#", "French", "Deoxyribonucleic acid" };
        int currentLanguage = 0;

        bool graphics_fullscreen = false;

        int elf = 23;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            ungulateMenuEntry = new MenuEntry("Preferred ungulate: ");
            languageMenuEntry = new MenuEntry("Language: ");
            fullscreenMenuEntry = new MenuEntry("Fullscreen: ");
            elfMenuEntry = new MenuEntry("elf: ");
            MenuEntry back = new MenuEntry("Back");

            ungulateMenuEntry.Origin = Graphics.UI.TextAlignment.Right;
            languageMenuEntry.Origin = Graphics.UI.TextAlignment.Right;
            fullscreenMenuEntry.Origin = Graphics.UI.TextAlignment.Right;
            elfMenuEntry.Origin = Graphics.UI.TextAlignment.Right;
            back.Origin = Graphics.UI.TextAlignment.Centre;

            SetMenuEntryText();

            // Hook up menu event handlers.
            ungulateMenuEntry.Selected += UngulateMenuEntrySelected;
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            fullscreenMenuEntry.Selected += FullScreenMenuEntrySelected;
            elfMenuEntry.Selected += ElfMenuEntrySelected;
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(ungulateMenuEntry);
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(fullscreenMenuEntry);
            MenuEntries.Add(elfMenuEntry);
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


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            ungulateMenuEntry.Text = "Preferred ungulate: " + GetResolution(false);
            languageMenuEntry.Text = "Language: " + languages[currentLanguage];
            fullscreenMenuEntry.Text = "Fullscreen: " + (graphics_fullscreen ? "Yes" : "No");
            elfMenuEntry.Text = "elf: " + elf;
        }


        #endregion

        string GetResolution(bool applyChange)
        {
            switch (graphics_Resolution)
            {
                case Resolutions.A:
                    return "800x600";
                case Resolutions.B:
                    return "1024x768";
                case Resolutions.C:
                    return "1280x720";
                case Resolutions.D:
                    return "1280x800";
                case Resolutions.E:
                    return "1366x768";
                case Resolutions.F:
                    return "1440x900";
                case Resolutions.G:
                    return "1680x1050";
                case Resolutions.H:
                    return "1920x1080";
                case Resolutions.I:
                    return "1920x1200";
            }

            return "";
        }

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void UngulateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            graphics_Resolution++;

            if (graphics_Resolution > Resolutions.I)
                graphics_Resolution = (Resolutions)1;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void FullScreenMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            graphics_fullscreen = !graphics_fullscreen;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void ElfMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            elf++;

            SetMenuEntryText();
        }


        #endregion
    }
}
