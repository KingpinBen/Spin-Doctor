//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - SpriteManager
//--    
//--    Description
//--    ===============
//--    
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - The list now clears on level load
//--    
//--    
//--    TBD
//--    ==============
//--    Make.. nicer?
//--    
//--    
//--------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameLibrary.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Managers
{
    public static class Sprite_Manager
    {
        #region Variables

        private static List<Sprite> SpriteList = new List<Sprite>();

        #endregion

        public static void Update(GameTime gameTime)
        {
            for (int i = SpriteList.Count - 1; i > 0; i--)
            {
                SpriteList[i].Update(gameTime);

                if (SpriteList[i].IsDead == true)
                {
                    SpriteList.RemoveAt(i);
                }
            }
        }

        #region AddSprites
        public static void AddSprite(Sprite sprite)
        {
            SpriteList.Add(sprite);
        }
        #endregion

        public static int ListCount()
        {
            return SpriteList.Count();
        }

        public static void Draw(SpriteBatch sb)
        {
            for (int i = SpriteList.Count - 1; i > 0; i--)
            {
                SpriteList[i].Draw(sb);
            }
        }

        public static void Clear()
        {
            SpriteList = new List<Sprite>();
        }
    }
}
