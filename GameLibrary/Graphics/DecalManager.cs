//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - DecalManager
//--    
//--    
//--    Description
//--    ===============
//--    Holds and draws all the decals.
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Fixed some of the remaining things such as Load.
//--    BenG - Made it so it can be instantiated for serializing. Removed static.
//--    
//--    TBD
//--    ==============
//--    Maybe on load setup a rendertarget and grab them all in one texture. Can then clear everything!
//-------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.GameLogic;
using GameLibrary.GameLogic.Screens;
<<<<<<< HEAD
=======
using GameLibrary.Graphics.Drawing;
>>>>>>> Tech Doc revisions

namespace GameLibrary.Graphics
{
    public class DecalManager
    {
        #region Fields
        [ContentSerializer]
        private List<Decal> _decalList;
        #endregion

        #region Properties
#if EDITOR
        [ContentSerializerIgnore]
        public List<Decal> DecalList
        {
            get
            {
                return _decalList;
            }
            set
            {
                _decalList = value;
            }
        }
#else
        [ContentSerializerIgnore]
        public List<Decal> DecalList
        {
            get
            {
                return _decalList;
            }
        }
#endif

        #endregion

        public DecalManager()
        {
            _decalList = new List<Decal>();
        }

        public void Load(GameplayScreen screen)
        {
            ContentManager _content = new ContentManager(screen.ScreenManager.Game.Services, "Content");

            for (int i = _decalList.Count - 1; i >= 0; i--)
            {
                _decalList[i].Load(_content);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < _decalList.Count; i++)
            {
                _decalList[i].Draw(sb);
            }
        }
    }
}
