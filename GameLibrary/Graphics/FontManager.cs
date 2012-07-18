#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using GameLibrary.Graphics;
using GameLibrary.GameLogic;
#endregion

namespace GameLibrary.Graphics
{
    public class FontManager
    {
        private static FontManager _singleton;
        public static FontManager Instance
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new FontManager();
                }

                return _singleton;
            }
        }

        ContentManager _content;

        FontType[] _fonts;

        private FontManager()
        {
            _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _fonts = new FontType[6];
            _fonts[0] = new FontType();
            _fonts[1] = new FontType();
            _fonts[2] = new FontType();
            _fonts[3] = new FontType();
            _fonts[4] = new FontType();
            _fonts[5] = new FontType();
        }

        public void Load()
        {
            _content.Unload();

            _fonts[0].Load(_content.Load<SpriteFont>("Assets/Fonts/Default"));  //  Game
            _fonts[1].Load(_content.Load<SpriteFont>("Assets/Fonts/Menu")); //  Menu Title
            _fonts[2].Load(_content.Load<SpriteFont>("Assets/Fonts/Menu")); //  MenuOption
            _fonts[2].Spacing = 1;
            _fonts[3].Load(_content.Load<SpriteFont>("Assets/Fonts/Menu"));  //  GUI
            _fonts[4].Load(_content.Load<SpriteFont>("Assets/Fonts/Notes"));    //  Notes
            _fonts[5].Load(_content.Load<SpriteFont>("Assets/Fonts/Debug"));    // Debug
        }

        public void UnloadContent()
        {
            if (this._content != null)
            {
                _content.Unload();
            }
        }

        public FontType GetFont(FontList fontName)
        {
            return _fonts[(int)fontName];
        }
    }
}
