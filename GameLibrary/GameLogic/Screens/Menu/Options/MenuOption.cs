//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - MenuOption
//--    
//--    
//--    
//--    Description
//--    ===============
//--    Makes each item in a menu a separate element
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Fixed development things and option detection
//--    BenG - Moved CompleteAction out and into the MenuScreens for menu
//--           dependant option outcomes.
//--    
//--    
//--    TBD
//--    ==============
//--    The rest of it, durrh
//--    
//--
//--    
//--    
//--------------------------------------------------------------------------

//#define Development

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameLibrary.Graphics;
#endregion

namespace GameLibrary.GameLogic.Screens.Menu.Options
{
    public class MenuItem
    {
        #region Fields/variables

        public string Name { get; internal set; }

        public int Height { get; internal set; }

        public int Width { get; internal set; }

        public Vector2 Position { get; set; }

        public bool Highlighted { get; internal set; }

        public OptionType OptionType { get; protected set; }

        /// <summary>
        /// Sets the colour for the option.
        /// </summary>
        public Color NewColor { get; internal set; }

        /// <summary>
        /// Used to fade between colours.
        /// </summary>
        public float FadeTimer { get; internal set; }

        #region Development
#if Development
        private PrimitiveBatch pb;
#endif
        #endregion
        #endregion

        #region Constructor
        public MenuItem(string Name, OptionType option, Vector2 position)
        {
            Vector2 textSize = FontManager.Instance.GetFont(Graphics.FontList.MenuOption).MeasureString(Name);

            this.Name = Name;
            this.Position = position;
            this.Width = (int)textSize.X;
            this.Height = (int)textSize.Y;
            this.OptionType = option;
            this.NewColor = Color.DarkGoldenrod;

#if Development
            pb = new PrimitiveBatch(Screen_Manager.Graphics);
#endif

        }
        #endregion

        #region Update
        public void Update(GameTime gameTime)
        {
            if (!Highlighted && FadeTimer == 0.0f) return;

            float FadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 7;

            if (Highlighted)
                this.FadeTimer = Math.Min(FadeTimer + FadeSpeed, 1f);
            else
                this.FadeTimer = Math.Max(FadeTimer - FadeSpeed, 0f);

            NewColor = Color.Lerp(Color.DarkGoldenrod, Color.White, FadeTimer);
        }
        #endregion

        #region Draw

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(FontManager.Instance.GetFont(Graphics.FontList.MenuOption).Font, Name, Position, NewColor, 0f, new Vector2((Width - 30) / 2, Height / 2), 1f, SpriteEffects.None, 0f);

            #region Development
#if Development
            sb.End();
            pb.Begin(PrimitiveType.LineList);
            //Top Left to Right
            pb.AddVertex(new Vector2(Position.X - (Width / 2),
                Position.Y - (Height / 2)), Color.Green);
            pb.AddVertex(new Vector2(Position.X + (Width / 2),
                Position.Y - (Height / 2)), Color.Green);

            // Top left to bottom left
            pb.AddVertex(new Vector2(Position.X - (Width / 2),
                Position.Y - (Height / 2)), Color.Green);
            pb.AddVertex(new Vector2(Position.X - (Width / 2),
                Position.Y + (Height / 2)), Color.Green);

            //Bottom Left to Bottom Right
            pb.AddVertex(new Vector2(Position.X - (Width / 2),
                Position.Y + (Height / 2)), Color.Green);
            pb.AddVertex(new Vector2(Position.X + (Width / 2),
                Position.Y + (Height / 2)), Color.Green);

            // Bottom Right to Top right
            pb.AddVertex(new Vector2(Position.X + (Width / 2),
                Position.Y + (Height / 2)), Color.Green);
            pb.AddVertex(new Vector2(Position.X + (Width / 2),
                Position.Y - (Height / 2)), Color.Green);

            //pb.AddVertex(Position, Color.Pink);
            pb.End();
            sb.Begin();
#endif
            #endregion
        }
        #endregion
    }
}
