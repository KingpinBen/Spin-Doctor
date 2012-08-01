using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Graphics;
using GameLibrary.Graphics.UI;

namespace GameLibrary.GameLogic.Screens.Menu.Options
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    public class MenuEntry
    {
        #region Fields

        string _text;
        float _selectionFade;
        Vector2 _position;
        Vector2 _origin;
        SpriteFont _font;
        TextAlignment _alignment;
        bool _separator;

        #endregion

        #region Properties

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public TextAlignment Origin
        {
            get
            {
                return _alignment;
            }
            set
            {
                _alignment = value;
                _origin = GetOrigin();
            }
        }


        public bool Separator
        {
            get
            {
                return _separator;
            }
            set
            {
                _separator = value;
            }
        }

        #endregion

        #region Events


        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;


        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }


        #endregion

        #region Initialization


        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
        public MenuEntry(string text)
        {
            this._text = text;
            this._font = FontManager.Instance.GetFont(FontList.MenuOption);
            this.Origin = TextAlignment.Left;
        }


        #endregion

        #region Update and Draw

        public virtual void Update(MenuScreen screen, bool isSelected, float delta)
        {
            float fadeSpeed = delta * 5.0f;

            if (isSelected)
                _selectionFade = Math.Min(_selectionFade + fadeSpeed, 1);
            else
                _selectionFade = Math.Max(_selectionFade - fadeSpeed, 0);
        }

        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Color.Goldenrod : Color.White;

            //// Pulsate the size of the selected menu entry.
            //double time = gameTime.TotalGameTime.TotalMilliseconds * 0.001f
            float pulsate;// = (float)Math.Sin(time * 6) + 1

            if (isSelected)
                pulsate = 0.8f;
            else
                pulsate = 0.0f;

            float scale = 1 + pulsate * 0.05f * _selectionFade;

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;

            spriteBatch.DrawString(_font, _text, _position, color, 0,
                                   _origin, scale, SpriteEffects.None, 0);
        }

        public virtual int GetHeight(MenuScreen screen)
        {
            return (int)_font.LineSpacing;
        }

        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)_font.MeasureString(Text).X;
        }

        private Vector2 GetOrigin()
        {
            switch (_alignment)
            {
                case TextAlignment.Left:
                    return new Vector2(0, _font.LineSpacing * 0.5f);
                case TextAlignment.Centre:
                    return new Vector2(_font.MeasureString(_text).X * 0.5f, _font.LineSpacing * 0.5f);
                case TextAlignment.Right:
                    return new Vector2(_font.MeasureString(_text).X, _font.LineSpacing * 0.5f);
                default:
                    return Vector2.Zero;
            }
        }


        #endregion
    }
}
