#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
#endregion

namespace GameLibrary.Assists
{
    static public class Fonts
    {
        public static SpriteFont GameFont;
        public static SpriteFont DebugFont;
        public static SpriteFont MenuFont;
        public static SpriteFont NotesFont;

        /// <summary>
        /// Load the fonts.
        /// </summary>
        /// <param name="Content"></param>
        public static void Load(ContentManager Content)
        {
            GameFont    = Content.Load<SpriteFont>("Assets/Fonts/Default");
            DebugFont   = Content.Load<SpriteFont>("Assets/Fonts/Debug");
            MenuFont    = Content.Load<SpriteFont>("Assets/Fonts/Menu");
            NotesFont   = Content.Load<SpriteFont>("Assets/Fonts/Notes");
        }
    }
}
