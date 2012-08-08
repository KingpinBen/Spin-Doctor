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
using GameLibrary.GameLogic;

namespace GameLibrary.Graphics
{
    public class SpriteManager
    {
        #region Singleton and Getter


        /// <summary>
        /// The singleton
        /// </summary>
        private static SpriteManager _singleton = new SpriteManager();

        /// <summary>
        /// Gets the singleton instance of the SpriteManager.
        /// </summary>
        public static SpriteManager Instance
        {
            get
            {
                return _singleton;
            }
        }


        #endregion

        #region Fields

        private GraphicsDevice _graphicsDevice;

        /// <summary>
        /// The main list to hold all the sprites
        /// </summary>
        private List<Sprite> _spriteList;

        /// <summary>
        /// We want to draw all the sprites in the list when the draw is called.
        /// If a sprite is removed after, it can throw.
        /// </summary>
        private List<Sprite> _spritesToDraw;

        #endregion

        #region Constructor


        /// <summary>
        /// Instantiated through the singleton, so we can't pass anything in.
        /// </summary>
        private SpriteManager()
        {
            this._spriteList = new List<Sprite>();
            this._spritesToDraw = new List<Sprite>();
        }


        #endregion

        #region Load / Setup the SpriteManager


        public void Load(ScreenManager screenManager)
        {
            //  First we'll make sure that the new SpriteList is empty
            //  incase load has been called from a level change.
            this.Clear();

            //  We'll pass in the ScreenManager mainly for the Game and 
            //  graphicsdevice references.
            _graphicsDevice = screenManager.GraphicsDevice;
        }

        #endregion

        #region Update
        /// <summary>
        /// Update each sprite in the SpriteList
        /// </summary>
        /// <param name="delta">Elapsed time in seconds</param>
        public void Update(float delta)
        {
#if EDITOR

#else
            //  As sprites will be getting removed from the list, 
            //  we'll decrement our way through the spritelist.
            for (int i = this._spriteList.Count - 1; i >= 0; i--)
            {
                //  Update the sprite
                this._spriteList[i].Update(delta);

                //  Then check if it's dead and needs removing.
                if (this._spriteList[i].IsDead == true)
                {
                    //  If it is dead, remove it from the _spritelist
                    this._spriteList.RemoveAt(i);
                }
            }
#endif
        }
        #endregion

        #region Draw


        /// <summary>
        /// Draw whats currently in the SpriteList to screen.
        /// </summary>
        /// <param name="SpriteBatch">Current SpriteBatch to render to</param>
        public void Draw(SpriteBatch spriteBatch)
        {
#if !EDITOR
            //  Make sure the draw sprite list is empty.
            this._spritesToDraw.Clear();

            //  then copy over what's currently in the master sprite list.
            this._spritesToDraw.AddRange(_spriteList);

            //  Go through the list and draw them all
            for (int i = 0; i < _spritesToDraw.Count; i++)
            {
                _spritesToDraw[i].Draw(spriteBatch, _graphicsDevice);
            }
#endif
        }


        #endregion

        #region Extra Methods


        #region Clear the SpriteList


        /// <summary>
        /// Empties the SpriteList.
        /// To be called from Load and maybe elsewhere.
        /// </summary>
        public void Clear()
        {
            this._spriteList = new List<Sprite>();
            this._spritesToDraw = new List<Sprite>();
        }

        #endregion

        #region AddSprites


        /// <summary>
        /// Add a sprite to the SpriteManagers master list
        /// </summary>
        /// <param name="sprite">The sprite to be added. Must be init'd and loaded.</param>
        public void AddSprite(Sprite sprite)
        {
            //  Add the sprite to the list.
            _spriteList.Add(sprite);
        }


        #endregion


        #endregion
    }
}
