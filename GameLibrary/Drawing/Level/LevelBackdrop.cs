//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - Levelbackdrop
//--
//--    
//--    Description
//--    ===============
//--    Holds the objects for the background that rotate with the level.
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Changes background depending on room type
//--    
//--    TBD
//--    ==============
//--    
//--    
//--    
//-------------------------------------------------------------------------------

#define EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameLibrary.Drawing
{
    public class LevelBackdrop
    {
        private Texture2D _roomShell;
        private Texture2D _levelBackground;
        Point roomDimensions;
        Color backgroundTint;

        public Color Tint
        {
            get
            {
                return backgroundTint;
            }
            set
            {
                this.backgroundTint = value;
            }
        }

        public LevelBackdrop() { }

        public void Load(ContentManager content, Vector2 roomDimensions, RoomThemeEnum type, string backgroundFile)
        {
            this.roomDimensions = new Point((int)roomDimensions.X, (int)roomDimensions.Y);
            LoadType(content, type, backgroundFile);
        }

        public void DrawShell(SpriteBatch sb)
        {
            sb.Draw(_roomShell, Vector2.Zero, null, Color.White, 0.0f, new Vector2(_roomShell.Width / 2, _roomShell.Height / 2), 6.75f, SpriteEffects.None, 1f);
            this.Draw(sb);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_levelBackground, new Rectangle(-this.roomDimensions.X / 2, -this.roomDimensions.Y / 2, this.roomDimensions.X, this.roomDimensions.Y),
                null, backgroundTint, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);

            //sb.Draw(_roomShell, Vector2.Zero, null, Color.Red, 0.0f, this._origin, 1.0f, SpriteEffects.None, 0.5f);
            //sb.Draw(_roomShell, Vector2.Zero, null, Color.White, MathHelper.PiOver2, this._origin, 1.0f, SpriteEffects.None, 0.5f);
            //sb.Draw(_roomShell, Vector2.Zero, null, Color.Blue, MathHelper.Pi, this._origin, 1.0f, SpriteEffects.None, 0.5f);
            //sb.Draw(_roomShell, Vector2.Zero, null, Color.Green, MathHelper.PiOver2 + MathHelper.Pi, this._origin, 1.0f, SpriteEffects.None, 0.5f);
        }

        /// <summary>
        /// Makes the artistic choices on what art should be displayed for each level.
        /// </summary>
        /// <param name="content">Content</param>
        /// <param name="type">What sort of style the room is.</param>
        private void LoadType(ContentManager content, RoomThemeEnum roomType, string backgroundFile)
        {
            this._levelBackground = content.Load<Texture2D>(backgroundFile);

            switch (roomType)
            {
                case RoomThemeEnum.General:
                    this._roomShell = content.Load<Texture2D>("Assets/Images/Textures/RoomSetup/RingWood");
                    break;

                case RoomThemeEnum.Industrial:
                    this._roomShell = content.Load<Texture2D>("Assets/Images/Textures/RoomSetup/RingWood");
                    break;

                case RoomThemeEnum.Medical:
                    this._roomShell = content.Load<Texture2D>("Assets/Images/Textures/RoomSetup/RingWood");
                    break;

                case RoomThemeEnum.Study:
                    this._roomShell = content.Load<Texture2D>("Assets/Images/Textures/RoomSetup/RingWood");
                    break;
            }
        }
    }
}
