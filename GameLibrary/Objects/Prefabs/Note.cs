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
using GameLibrary.Assists;
using GameLibrary.Screens.Messages;
using GameLibrary.Managers;
using GameLibrary.Drawing;

namespace GameLibrary.Objects
{
    public class Note : Collectable
    {
        public Note()
            : base()
        {

        }

        public override void Load(ContentManager content, FarseerPhysics.Dynamics.World world)
        {
            base.Load(content, world);
        }

        public override void Update(GameTime gameTime)
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
            MessageOverlay newOverlay = new MessageOverlay(MessageType.FullScreen, 1, Screen_Manager.GraphicsDevice);
            newOverlay.Load();

            Screen_Manager.AddScreen(newOverlay);

            HUD.ShowOnScreenMessage(false);
            this.Triggered = false;
            this.BeenCollected = true;
#endif
        }
    }
}
