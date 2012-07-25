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

namespace GameLibrary.GameLogic.Objects
{
    public class Note : Collectable
    {
        public Note()
            : base()
        {

        }

        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);
        }

        public override void Update(float delta)
        {
#if EDITOR

#else
            if (InputManager.Instance.Interact() && Triggered)
            {
                CreatePopUp();
            }
#endif
        }
        private void CreatePopUp()
        {
#if EDITOR
#else
            //MessageOverlay newOverlay = new MessageOverlay(MessageType.FullScreen, 1, ScreenManager.GraphicsDevice);
            //newOverlay.Load();

            //ScreenManager.AddScreen(newOverlay);

            HUD.Instance.ShowOnScreenMessage(false);
            this.Triggered = false;
            this.BeenCollected = true;
#endif
        }
    }
}
