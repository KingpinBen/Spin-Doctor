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
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Graphics;
using GameLibrary.GameLogic.Controls;
using GameLibrary.Graphics.UI;
#endregion

namespace GameLibrary.GameLogic.Screens.Messages
{
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

        private FullscreenPicture fsp;

        public MessageOverlay(MessageType type, int stringID, GraphicsDevice graphics) 
            : base("New Message", graphics)
        {
            this.Type = type;
            this.StringID = stringID;
            this.Message = "";
        }

        public override void Load()
        {
            if (StringID > 0)
            {
                fsp = new FullscreenPicture();
                //  TODO: LINK TO CORRECT POPUP ASSET LOCATION THINGY
                fsp.Load(_content, "Assets/Other/Dev/invite" + StringID.ToString(), ScaleType.MaxHeight);
                fsp.Scale = ((1 / this._graphicsDevice.Viewport.AspectRatio) * 2) * 0.8f;

                return;
            }

            this.Position = new Vector2(this._graphicsDevice.Viewport.Width * 0.5f, (this._graphicsDevice.Viewport.Height * 0.17f) * 5);
            this.Origin = FontManager.Instance.GetFont(Graphics.FontList.Game).MeasureString(Message) * 0.5f;
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.GP_A || InputManager.Instance.Enter)
            {
                ScreenManager.DeleteScreen();
            }
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
                        sb.DrawString(FontManager.Instance.GetFont(Graphics.FontList.GUI).Font, Message, Position, Color.White * Alpha, 0f, Origin, 1f, SpriteEffects.None, 0f);

                        break;
                    }
            }
            sb.End();
        }
    }
}
