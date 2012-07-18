using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameLibrary.Graphics
{
    public class FontType
    {
        SpriteFont _font;
        int _fontHeight;
        int _spacing;

        public int Spacing
        {
            get
            {
                return _spacing;
            }
            set
            {
                _spacing = value;
            }
        }
        public int Height
        {
            get
            {
                return _fontHeight;
            }
        }
        public SpriteFont Font
        {
            get
            {
                return _font;
            }
        }

        public FontType()
        {
            this._fontHeight = 0;
            this._spacing = 0;
        }

        public void Load(SpriteFont font)
        {
            this._font = font;
            this._fontHeight = (int)font.MeasureString("Y").Y;
            this._spacing = 0;
        }

        public Vector2 MeasureString(string text)
        {
            Vector2 dims = _font.MeasureString(text);
            dims.Y += this._spacing * 2;
            return dims;
        }
    }
}
