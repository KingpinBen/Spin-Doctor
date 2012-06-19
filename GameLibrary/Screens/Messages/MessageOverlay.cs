//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - MessageOverlay
//--
//--    
//--    Description
//--    ===============
//--    Small screen overlay for game stopping text/image events
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    
//--    
//--    TBD
//--    ==============
//--    
//--    
//--    
//-------------------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameLibrary.Assists;
using System.IO;
using System.Xml;
using GameLibrary.Managers;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Drawing;
#endregion

namespace GameLibrary.Screens.Messages
{
    public enum MessageType
    {
        FullScreen, 
        Pop_Up
    }

    public class MessageOverlay : Screen
    {
        
        public string Message { get; set; }
        public MessageType Type { get; protected set; }
        public Vector2 Origin { get; internal set; }
        public Vector2 Position { get; set; }
        public float MessageWidth { get; internal set; }
        public float MessageHeight { get; internal set; }
        public float Alpha { get; set; }
        public int StringID { get; protected set; }

        private ContentManager content;
        private FullscreenPicture fsp;

        public MessageOverlay(MessageType type, int stringID) 
            : base("New Message", 0f)
        {
            this.content = new ContentManager(Screen_Manager.Game.Services, "Content");;
            this.Type = type;
            this.StringID = stringID;
            this.Message = "";
        }

        public override void Load()
        {
            if (StringID > 0) { FSPLoad(); return; }

            this.Position = new Vector2(Screen_Manager.Graphics.Viewport.Width / 2, (Screen_Manager.Graphics.Viewport.Height / 6) * 5);
            this.Origin = new Vector2(Fonts.GameFont.MeasureString(Message).X / 2, Fonts.GameFont.MeasureString("Y").Y / 2);
        }

        public void FSPLoad()
        {
            fsp = new FullscreenPicture();
            //  TODO: LINK TO CORRECT POPUP ASSET LOCATION THINGY
            fsp.Load(content, "Assets/Other/Dev/invite" + StringID.ToString());
            fsp.Scale = ((1 / Screen_Manager.Graphics.Viewport.AspectRatio) * 2) * 0.8f;
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.GP_A || Input.Enter)
                Screen_Manager.DeleteScreen();
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Begin();
            switch (Type)
            {
                case MessageType.FullScreen:
                    {
                        fsp.Draw(sb);
                        break;
                    }
                case MessageType.Pop_Up:
                    {
                        sb.DrawString(Fonts.GameFont, Message, Position, Color.White * Alpha, 0f, Origin, 1f, SpriteEffects.None, 0f);

                        break;
                    }
            }
            sb.End();
        }
    }
}
