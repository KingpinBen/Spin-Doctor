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

//NOTE: Anything in a comment in this is going to be a plan on a better way of drawing everything.

namespace GameLibrary.Graphics
{
    public class DecalManager
    {
        #region Fields
        [ContentSerializer]
        private List<Decal> _decalList;
        private ContentManager _content;
        private Texture2D _decalMesh;
        GameplayScreen _gameScreen;
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

        #region Load
        public void Load(GameplayScreen screen)
        {
            _gameScreen = screen;
            _content = new ContentManager(screen.ScreenManager.Game.Services, "Content");

            //  Generate a render target and make it default.

            for (int i = _decalList.Count - 1; i >= 0; i--)
            {
                _decalList[i].Load(_content);
                
                //  if the zLayer is >0.3 (whatever players zLayer is)
                //      Draw it to the rendertarget
                //      Remove it from the list.
            }
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch sb)
        {
            //  Below: Draw everything thats left.
            for (int i = 0; i < _decalList.Count; i++)
            {
                _decalList[i].Draw(sb);
            }
            //  Draw the collage of textures as one large texture.
        }
        #endregion
    }
}
