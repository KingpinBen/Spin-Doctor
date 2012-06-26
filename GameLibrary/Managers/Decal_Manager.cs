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
using GameLibrary.Drawing;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

//NOTE: Anything in a comment in this is going to be a plan on a better way of drawing everything.

namespace GameLibrary.Managers
{
    public class Decal_Manager
    {
        #region Fields
        [ContentSerializer]
        private List<Decal> _decalList;
        private ContentManager _content;
        private Texture2D _decalMesh;
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

        public Decal_Manager()
        {
            _decalList = new List<Decal>();
        }

        #region Load
        public void Load()
        {
            //  Get a new spriteBatch.
            //SpriteBatch sb = new SpriteBatch(Screen_Manager.Game.GraphicsDevice);

            _content = new ContentManager(Screen_Manager.Game.Services, "Content");

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
            //sb.Draw(_decalMesh, Vector2.Zero, null, Color.White);
        }
        #endregion
    }
}
