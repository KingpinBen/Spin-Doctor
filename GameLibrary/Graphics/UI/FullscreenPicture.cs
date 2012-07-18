//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - FullscreenPicture
//--    
//--    Description
//--    ===============
//--    Maintains an images aspect ratio while scaling it to full
//--    window/screen size. For loading screen art etc.
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    
//--    
//--    
//--    TBD
//--    ==============
//--    
//--    
//--------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameLibrary.GameLogic;

namespace GameLibrary.Graphics.UI
{
    public class FullscreenPicture
    {
        public Texture2D texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public string textureLoc { get; set; }
        public ScaleType scaleStyle { get; set; }

        public void Load(ContentManager content, string TexLoc, ScaleType style)
        {
            textureLoc = TexLoc;
            texture = content.Load<Texture2D>
                (textureLoc);

            Origin = new Vector2(texture.Width, texture.Height) * 0.5f;
            Position = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height) * 0.5f;
            this.scaleStyle = style;

            switch (scaleStyle)
            {
                case ScaleType.AutoScale:
                    {
                        //  Height is by aspect ratio as width is generally always larger than height.
                        //  this is just to make sure it works evenly.
                        if (texture.Width >= texture.Height * ScreenManager.GraphicsDevice.Viewport.AspectRatio)
                            Scale = (float)ScreenManager.GraphicsDevice.Viewport.Width / (float)texture.Width;
                        else
                            Scale = (float)ScreenManager.GraphicsDevice.Viewport.Height / (float)texture.Height;

                        break;
                    }
                case ScaleType.MaxHeight:
                    {
                        Scale = (float)ScreenManager.GraphicsDevice.Viewport.Height / (float)texture.Height;
                        break;
                    }
                case ScaleType.MaxWidth:
                    {
                        Scale = (float)ScreenManager.GraphicsDevice.Viewport.Width / (float)texture.Width;
                        break;
                    }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, Position, null, Color.White, 0f, Origin, Scale, SpriteEffects.None, 0.5f);
        }
    }
}
