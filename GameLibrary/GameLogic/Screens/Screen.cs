//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Screen
//--    
//--    
//--    Description
//--    ===============
//--    Abstract class for screens to be used with the ScreenManager
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial 
//--    BenG - Now has transitions (in/out)
//--    BenG - Better memory handling for certain screens
//--    
//--    
//--    TBD
//--    ==============
//--    Gameplay -> loading doesn't have a proper transition
//--    Maybe have a blur on the transition
//--    
//--------------------------------------------------------------------------


#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;
using GameLibrary.GameLogic;
#endregion

namespace GameLibrary.GameLogic.Screens
{
    public abstract class Screen
    {
        #region Fields and Variables

        public string Name
        {
            get
            {
                return _name;
            }
            internal set
            {
                _name = value;
            }
        }
        private string _name;
        
        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
            protected set
            {
                _isInitialized = value;
            }
        }
        private bool _isInitialized;

        public bool IsExitable
        {
            get
            {
                return _isExitable;
            }
            protected set
            {
                _isExitable = value;
            }
        }
        private bool _isExitable = true;

        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;

        #endregion

        #region Constructor
        public Screen(string name, GraphicsDevice graphics)
        {
            this._name = name;
            this._graphicsDevice = graphics;
            this._content = new ContentManager(ScreenManager.Game.Services, "Content");
        }
        #endregion

        #region Load
        public virtual void Load() { }
        #endregion

        #region Unload
        public virtual void Unload()
        {
            _content.Unload();
        }
        #endregion

        #region Update
        /// <summary>
        /// Override
        /// </summary>
        /// <param name="gameTime">Grab elapsed time</param>
        public virtual void Update(GameTime gameTime)
        {

        }
        #endregion

        #region Draw
        /// <summary>
        /// Override
        /// </summary>
        /// <param name="SpriteBatch">SpriteBatch</param>
        public virtual void Draw(SpriteBatch sb)
        {
        }
        #endregion
    }
}
