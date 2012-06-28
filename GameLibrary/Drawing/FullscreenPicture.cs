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
using GameLibrary.Managers;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Drawing
{
    /// <summary>
    /// 3 styles of scaling:
    /// AutoScale - 
    ///     Keeps the whole image onscreen, scaling it to be the largest it can be without
    ///     the pixels being offscreen.
    /// MaxWidth -
    ///     Scales the image so the width is the same as the viewport width.
    /// MaxHeight -
    ///     Scales the image so the height is the same as the viewport height. 
    ///     
    /// Autoscale is default. Change before loading.
    /// </summary>
    public enum ScaleStyle
    {
        AutoScale,
        MaxWidth,
        MaxHeight
    }

    /// <summary>
    /// Used for any picture you want to be fullscreen and to automatically scale to fit the whole
    /// screen regardless of the viewport size and aspect ratio. Tadah.
    /// </summary>
    public class FullscreenPicture
    {
        public Texture2D texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public string textureLoc { get; set; }
        public ScaleStyle scaleStyle { get; set; }

        public void Load(ContentManager content, string TexLoc)
        {
            textureLoc = TexLoc;
            texture = content.Load<Texture2D>
                (textureLoc);

            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Position = (new Vector2(Screen_Manager.GraphicsDevice.Viewport.Width, Screen_Manager.GraphicsDevice.Viewport.Height) * 0.5f);

            switch (scaleStyle)
            {
                case ScaleStyle.AutoScale:
                    {
                        //  Height is by aspect ratio as width is generally always larger than height.
                        //  this is just to make sure it works evenly.
                        if (texture.Width >= texture.Height * Screen_Manager.GraphicsDevice.Viewport.AspectRatio)
                            Scale = (float)Screen_Manager.GraphicsDevice.Viewport.Width / (float)texture.Width;
                        else
                            Scale = (float)Screen_Manager.GraphicsDevice.Viewport.Height / (float)texture.Height;

                        break;
                    }
                case ScaleStyle.MaxHeight:
                    {
                        Scale = (float)Screen_Manager.GraphicsDevice.Viewport.Height / (float)texture.Height;
                        break;
                    }
                case ScaleStyle.MaxWidth:
                    {
                        Scale = (float)Screen_Manager.GraphicsDevice.Viewport.Width / (float)texture.Width;
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
