//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - Note
//--
//--    
//--    Description
//--    ===============
//--    
//--
//--    
//--    Revision List
//--    ===============
//--    
//--    TBD
//--    ==============
//--    Complete - change Collectable to Note
//--    
//--    
//-------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameLibrary.GameLogic.Objects.Triggers;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Graphics.UI;
using GameLibrary.GameLogic.Screens;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Factories;
using GameLibrary.Helpers;

namespace GameLibrary.GameLogic.Objects
{
    public class Note : Collectable
    {
        public Note()
            : base()
        {

        }

        public override void Update(float delta)
        {
#if EDITOR

#else
            if (InputManager.Instance.Interact() && _triggered)
            {
                CreatePopUp();
            }
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, this._position, new Rectangle(0, 0, (int)_width, (int)_height),
                this._tint, this._rotation, new Vector2(this._width, this._height) * 0.5f, 1.0f, SpriteEffects.None, this._zLayer);
        }
#else
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Draw(_texture, this._position, new Rectangle(0, 0, (int)_width, (int)_height), 
                this._tint, this._rotation, this._origin, 1.0f, SpriteEffects.None, this._zLayer);
        }
#endif
        #endregion



        private void CreatePopUp()
        {
#if EDITOR
#else
            //MessageOverlay newOverlay = new MessageOverlay(MessageType.FullScreen, 1, ScreenManager.GraphicsDevice);
            //newOverlay.Load();

            //ScreenManager.AddScreen(newOverlay);

            HUD.Instance.ShowOnScreenMessage(false);
            this._triggered = false;
            this._beenCollected = true;
#endif
        }
    }
}
