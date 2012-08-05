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
using GameLibrary.Graphics.Camera;
using GameLibrary.GameLogic.Characters;

namespace GameLibrary.GameLogic.Objects
{
    public class Note : Collectable
    {
        #region Fields
        [ContentSerializer]
        private int _noteID;

        #endregion

        #region Properties
#if EDITOR
        [ContentSerializerIgnore]
        public int NoteID
        {
            get
            {
                return _noteID;
            }
            set
            {
                _noteID = value;
            }
        }
#else

#endif
        #endregion

        public Note() : base() { }

        public override void Load(ContentManager content, World world)
        {
            if (GameSettings.Instance.FoundEntries[_noteID])
            {
                Camera.Instance.GetGameScreen().Level.ObjectsList.Remove(this);
            }
            
            base.Load(content, world);
        }

        public override void Update(float delta)
        {
#if !EDITOR
            if (_triggered)
            {
                if (Player.Instance.PlayerState == PlayerState.Dead)
                {
                    _triggered = false;
                    return;
                }

                if (InputManager.Instance.Interact(true))
                {
                    CreatePopUp();
                }
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
            Camera.Instance.GetGameScreen().ScreenManager.AddScreen(new MessageOverlay(Defines.NOTE_DIRECTORY + _noteID), null);
                    
            HUD.Instance.ShowOnScreenMessage(false);
            this._triggered = false;
            this._beenCollected = true;
#endif
        }
    }
}
