//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - HUD
//--    
//--    Description
//--    ===============
//--    Allows spritebatches untouched by the camera transform.
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    
//--    
//--    
//--    TBD
//--    ==============
//--
//--    
//--    
//--------------------------------------------------------------------------

//#define Development

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Managers;
using GameLibrary.Screens.Messages;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Assists;
#endregion

namespace GameLibrary.Drawing
{
    public static class HUD
    {
        #region Fields

        public static bool ShowPopup {get; internal set;}

        private static AnimatedText _overlayText;

        private static ContentManager content;

#if Development
        private static PrimitiveBatch _primBatch = new PrimitiveBatch(Screen_Manager.Graphics);
#endif
        #endregion

        #region Load
        public static void Load()
        {
            content = new ContentManager(Screen_Manager.Game.Services, "Content");
            _overlayText = new AnimatedText("", ImgType.Interact, 0.5f, Alignment.Left);
            _overlayText.TextType = AnimatedType.Flash;
            _overlayText.AnchorPoint = ScreenAnchorLocation.BottomLeft;
            _overlayText.Offset = new Vector2(20, -10);
            _overlayText.Load(content);
            _overlayText.Scale = 0.5f;
        }
        #endregion

        #region Update
        public static void Update(GameTime gameTime)
        {
            if (!ShowPopup) return;

            _overlayText.Update(gameTime);
        }
        #endregion

        #region Draw
        public static void Draw(SpriteBatch sb)
        {
#if Development
            sb.End();

            _primBatch.Begin(PrimitiveType.LineList);

            _primBatch.AddVertex(new Vector2(Screen_Manager.Graphics.Viewport.TitleSafeArea.Left, Screen_Manager.Graphics.Viewport.TitleSafeArea.Top), Color.Red);
            _primBatch.AddVertex(new Vector2(Screen_Manager.Graphics.Viewport.TitleSafeArea.Left, Screen_Manager.Graphics.Viewport.TitleSafeArea.Bottom), Color.Red);

            _primBatch.AddVertex(new Vector2(Screen_Manager.Graphics.Viewport.TitleSafeArea.Left, Screen_Manager.Graphics.Viewport.TitleSafeArea.Bottom), Color.Red);
            _primBatch.AddVertex(new Vector2(Screen_Manager.Graphics.Viewport.TitleSafeArea.Right, Screen_Manager.Graphics.Viewport.TitleSafeArea.Bottom), Color.Red);

            _primBatch.AddVertex(new Vector2(Screen_Manager.Graphics.Viewport.TitleSafeArea.Right, Screen_Manager.Graphics.Viewport.TitleSafeArea.Bottom), Color.Red);
            _primBatch.AddVertex(new Vector2(Screen_Manager.Graphics.Viewport.TitleSafeArea.Right, Screen_Manager.Graphics.Viewport.TitleSafeArea.Top), Color.Red);

            _primBatch.AddVertex(new Vector2(Screen_Manager.Graphics.Viewport.TitleSafeArea.Right, Screen_Manager.Graphics.Viewport.TitleSafeArea.Top), Color.Blue);
            _primBatch.AddVertex(new Vector2(Screen_Manager.Graphics.Viewport.TitleSafeArea.Left, Screen_Manager.Graphics.Viewport.TitleSafeArea.Top), Color.Red);

            _primBatch.AddVertex(new Vector2(Screen_Manager.Graphics.Viewport.TitleSafeArea.Left, Screen_Manager.Graphics.Viewport.TitleSafeArea.Center.Y), Color.Blue);
            _primBatch.AddVertex(new Vector2(Screen_Manager.Graphics.Viewport.TitleSafeArea.Right, Screen_Manager.Graphics.Viewport.TitleSafeArea.Center.Y), Color.Red);


            _primBatch.End();

            sb.Begin();
#endif

            if (!ShowPopup) 
                return;

            _overlayText.Draw(sb);
        }
        #endregion

        #region Message toggle
        public static void ShowOnScreenMessage(bool set)
        {
            ShowOnScreenMessage(set, "");
        }

        public static void ShowOnScreenMessage(bool set, string message)
        {
            if (ShowPopup != set)
                ShowPopup = set;
            else return;

            _overlayText.Reset();

            if (_overlayText.Text != message || message != "")
            {
                _overlayText.Text = message;
                _overlayText.UpdateOrigin();
            }
        }
        #endregion

        public static void RefreshHUD()
        {
            ShowPopup = false;
            _overlayText.Reset();
        }
    }
}
