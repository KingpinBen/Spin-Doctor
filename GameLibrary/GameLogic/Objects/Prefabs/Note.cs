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
using System.ComponentModel;
using GameLibrary.Audio;

namespace GameLibrary.GameLogic.Objects
{
    public class Note : Collectable
    {
        #region Fields
        [ContentSerializer]
        private int _noteID = 0;

        #endregion

        #region Properties
#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
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
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public override TriggerType TriggerType
        {
            get
            {
                return _triggerType;
            }
            set { }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public override InteractType InteractType
        {
        get{
        return _interactType;
        }
        set { }
        }
#endif
        #endregion

        #region Constructor and Load

        public Note() : base() { }

        public override void Init(Vector2 Position, string texLoc)
        {
            base.Init(Position, texLoc);

            this._triggerType = TriggerType.PlayerInput;
            this._interactType = Objects.InteractType.PickUp;
            
        }

        public override void Load(ContentManager content, World world)
        {

            //  We need to load all the note before we remove it as it's 
            //  only removed on the next update cycle, so it draws at least 
            //  once.
            base.Load(content, world);

#if !EDITOR
            this._castShadows = false;
            this._triggerType = TriggerType.PlayerInput;
            this._message = " to pick up.";

            if (GameSettings.Instance.FoundEntries[_noteID - 1])
            {
                this._beenCollected = true;
                this.RemoveNote();
            }
#endif
            
            
        }

        #endregion

        #region Update and Draw
        public override void Update(float delta)
        {
#if !EDITOR
            if (_triggered && !_beenCollected)
            {
                if (Player.Instance.PlayerState == PlayerState.Dead)
                {
                    ChangeTriggered(false);
                    return;
                }

                if (InputManager.Instance.Interact(true))
                {
                    this.CreatePopUp();
                    this.RemoveNote();
                }
            }
#endif
        }

        
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, this._position, new Rectangle(0, 0, (int)_width, (int)_height),
                this._tint, this._rotation, new Vector2(this._width, this._height) * 0.5f, 1.0f, SpriteEffects.None, this._zLayer);
        }
#endif
        #endregion

        #region Private Methods
#if !EDITOR
        void CreatePopUp()
        {
            //  We call this before making the note screen incase we end up having text on there too.
            ChangeTriggered(false);

            Camera.Instance.GetGameScreen().ScreenManager.AddScreen(new MessageOverlay(_noteID), null);
            //  We want to set the settings to display as though they've got it.
            //  It'll only stick though when they complete the level with it.
            GameSettings.Instance.FoundEntries[_noteID - 1] = true;
            AudioManager.Instance.PlayCue("Note_Pick_Up", true);
        }

        void RemoveNote()
        {
            Camera.Instance.GetGameScreen().RemoveObject(this);
        }
#endif
        #endregion
    }
}
