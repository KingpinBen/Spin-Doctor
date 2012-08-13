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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Graphics.Camera;

namespace GameLibrary.Levels
{
    public class LevelBackdrop
    {
        private Texture2D _roomShell;
        private Texture2D _levelBackground;
        private Point roomDimensions;
        private Color backgroundTint;
        private float _shellScale;

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

        public void Load(ref ContentManager content, Vector2 roomDimensions, RoomTheme theme, RoomType type, string backgroundFile)
        {
            this.roomDimensions = new Point((int)roomDimensions.X, (int)roomDimensions.Y);
            this._levelBackground = content.Load<Texture2D>(backgroundFile);

            if (type == RoomType.Rotating)
            {
                switch (theme)
                {
                    case RoomTheme.General:
                        this._roomShell = content.Load<Texture2D>("Assets/Images/Textures/RoomSetup/RingWood");
                        break;

                    case RoomTheme.Industrial:
                        this._roomShell = content.Load<Texture2D>("Assets/Images/Textures/RoomSetup/RingWood");
                        break;

                    case RoomTheme.Medical:
                        this._roomShell = content.Load<Texture2D>("Assets/Images/Textures/RoomSetup/RingWood");
                        break;

                    case RoomTheme.Study:
                        this._roomShell = content.Load<Texture2D>("Assets/Images/Textures/RoomSetup/RingWood");
                        break;
                }

                //  The width of the ring multiplied by 2 (one per side)
                float shellWidth = 30 * 2;
                this._shellScale = Camera.Instance.LevelDiameter / (_roomShell.Width - shellWidth);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_roomShell, Vector2.Zero, null, Color.White, 0.0f, new Vector2(_roomShell.Width, _roomShell.Height) * 0.5f, _shellScale, SpriteEffects.None, 1f);

            sb.Draw(_levelBackground, new Rectangle(-(int)(this.roomDimensions.X * 0.5f), -(int)(this.roomDimensions.Y * 0.5f), this.roomDimensions.X, this.roomDimensions.Y),
                new Rectangle(0, 0, this.roomDimensions.X, this.roomDimensions.Y), backgroundTint, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
