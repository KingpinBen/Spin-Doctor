using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.GameLogic.Screens.Menu.Options;
using Microsoft.Xna.Framework;
using GameLibrary.System;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.GameLogic.Screens.Menu
{
    public class GraphicsOptionsScreen : MenuScreen
    {
        MenuEntry resolutionsEntry;
        MenuEntry fullscreenEntry;
        MenuEntry shadowsEntry;
        MenuEntry samplingAAEntry;
        MenuEntry applyEntry;

        Resolutions resolution = Resolutions.A;
        bool fullscreen = false;
        bool samplingAA = true;
        bool drawShadows = true;

        public GraphicsOptionsScreen()
            : base("Graphics")
        {
            resolutionsEntry = new MenuEntry("Resolution: ");
            fullscreenEntry = new MenuEntry("Fullscreen: ");
            shadowsEntry = new MenuEntry("Draw Shadows: ");
            samplingAAEntry = new MenuEntry("Anti-Aliasing: ");
            applyEntry = new MenuEntry("Apply");
            MenuEntry sep = new MenuEntry("sep");
            MenuEntry backEntry = new MenuEntry("Back");

            resolutionsEntry.Origin = Graphics.UI.TextAlignment.Centre;
            fullscreenEntry.Origin = Graphics.UI.TextAlignment.Centre;
            shadowsEntry.Origin = Graphics.UI.TextAlignment.Centre;
            samplingAAEntry.Origin = Graphics.UI.TextAlignment.Centre;
            applyEntry.Origin = Graphics.UI.TextAlignment.Centre;
            backEntry.Origin = Graphics.UI.TextAlignment.Centre;

            SetMenuEntryText();

            resolutionsEntry.Selected += ResolutionEntryChange;
            fullscreenEntry.Selected += FullscreenEntryChange;
            shadowsEntry.Selected += ShadowEntryChange;
            samplingAAEntry.Selected += SamplingEntryChange;
            applyEntry.Selected += ApplyEntrySelected;
            backEntry.Selected += OnCancel;

            sep.ItemType = MenuEntryType.Separator;

            menuEntries.Add(resolutionsEntry);
            menuEntries.Add(fullscreenEntry);
            menuEntries.Add(shadowsEntry);
            menuEntries.Add(samplingAAEntry);
            menuEntries.Add(sep);
            menuEntries.Add(applyEntry);
            menuEntries.Add(backEntry);
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

        public override void Activate()
        {
            this._itemsPosition = new Vector2(
                this.ScreenManager.GraphicsDevice.Viewport.Width * 0.5f,
                this.ScreenManager.GraphicsDevice.Viewport.Height * 0.33f);

            PresentationParameters pp = ScreenManager.GraphicsDevice.PresentationParameters;
            this.fullscreen = pp.IsFullScreen;
            float height = pp.BackBufferHeight;

            if (height == 720)
            {
                resolution = Resolutions.A;
            }
            else
            {
                if (height == 800)
                {
                    resolution = Resolutions.B;
                }
                else
                {
                    if (height == 768)
                    {
                        resolution = Resolutions.C;
                    }
                    else
                    {
                        if (height == 900)
                        {
                            resolution = Resolutions.D;
                        }
                        else
                        {
                            if (height == 1050)
                            {
                                resolution = Resolutions.E;
                            }
                            else
                            {
                                if (height == 1080)
                                {
                                    resolution = Resolutions.F;
                                }
                                else
                                {
                                    if (height == 1200)
                                    {
                                        resolution = Resolutions.G;
                                    }
                                    else
                                    {
                                        resolution = Resolutions.A;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            SetMenuEntryText();
        }

        void SetMenuEntryText()
        {
            resolutionsEntry.Text = "Preferred Resolution: " + GetResolution();
            fullscreenEntry.Text = "Fullscreen: " + (fullscreen ? "Yes" : "No");
            shadowsEntry.Text = "Draw Shadows: " + (drawShadows ? "Yes" : "No");
            samplingAAEntry.Text = "Anti-Aliasing: " + (samplingAA ? "Yes" : "No");
        }

        string GetResolution()
        {
            switch (resolution)
            {
                case Resolutions.A:
                    return "1280x720";
                case Resolutions.B:
                    return "1280x800";
                case Resolutions.C:
                    return "1366x768";
                case Resolutions.D:
                    return "1440x900";
                case Resolutions.E:
                    return "1680x1050";
                case Resolutions.F:
                    return "1920x1080";
                case Resolutions.G:
                    return "1920x1200";
            }

            return "";
        }

        void ResolutionEntryChange(object sender, PlayerIndexEventArgs e)
        {
            resolution++;

            if (resolution > (Resolutions)6)
                resolution = (Resolutions)0;

            SetMenuEntryText();
        }

        void FullscreenEntryChange(object sender, PlayerIndexEventArgs e)
        {
            fullscreen = !fullscreen;
            SetMenuEntryText();
        }

        void ShadowEntryChange(object sender, PlayerIndexEventArgs e)
        {
            drawShadows = !drawShadows;
            SetMenuEntryText();
        }

        void SamplingEntryChange(object sender, PlayerIndexEventArgs e)
        {
            samplingAA = !samplingAA;
            SetMenuEntryText();
        }

        void ApplyEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            GameSettings instance = GameSettings.Instance;

            if (drawShadows == true)
            {
                instance.Shadows = SettingLevel.On;
            }
            else
            {
                instance.Shadows = SettingLevel.Off;
            }

            switch (resolution)
            {
                case Resolutions.A:
                    this.ScreenManager.ChangeDevice(1280, 720, fullscreen, samplingAA);
                    break;
                case Resolutions.B:
                    this.ScreenManager.ChangeDevice(1280, 800, fullscreen, samplingAA);
                    break;
                case Resolutions.C:
                    this.ScreenManager.ChangeDevice(1366, 768, fullscreen, samplingAA);
                    break;
                case Resolutions.D:
                    this.ScreenManager.ChangeDevice(1440, 900, fullscreen, samplingAA);
                    break;
                case Resolutions.E:
                    this.ScreenManager.ChangeDevice(1680, 1050, fullscreen, samplingAA);
                    break;
                case Resolutions.F:
                    this.ScreenManager.ChangeDevice(1920, 1080, fullscreen, samplingAA);
                    break;
                case Resolutions.G:
                    this.ScreenManager.ChangeDevice(1920, 1200, fullscreen, samplingAA);
                    break;
            }

            SaveManager.Instance.SaveSettings();
        }
    }
}
