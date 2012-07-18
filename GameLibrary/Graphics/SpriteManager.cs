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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Graphics.Drawing;

namespace GameLibrary.Graphics
{
    public class SpriteManager
    {
        private static SpriteManager _singleton = null;
        public static SpriteManager Instance
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new SpriteManager();
                }

                return _singleton;
            }
        }

        #region Variables

        private List<Sprite> SpriteList = new List<Sprite>();

        #endregion

        public void Update(GameTime gameTime)
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
        public void AddSprite(Sprite sprite)
        {
            SpriteList.Add(sprite);
        }
        #endregion

        public int ListCount()
        {
            return SpriteList.Count();
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = SpriteList.Count - 1; i > 0; i--)
            {
                SpriteList[i].Draw(sb);
            }
        }

        public void Clear()
        {
            SpriteList = new List<Sprite>();
        }
    }
}
