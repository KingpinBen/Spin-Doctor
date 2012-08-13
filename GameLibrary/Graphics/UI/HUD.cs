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
using Microsoft.Xna.Framework.Content;
using GameLibrary.GameLogic;
using System.Threading;
#endregion

namespace GameLibrary.Graphics.UI
{
    public class HUD
    {
        #region Fields

        private static HUD _singleton = null;
        private static object _singletonLock = new object();
        public static HUD Instance
        {
            get
            {
                if (HUD._singleton == null)
                {
                    object obj;
                    Monitor.Enter(obj = HUD._singletonLock);
                    try
                    {
                        if (HUD._singleton == null)
                        {
                            HUD._singleton = new HUD();
                        }
                    }
                    finally
                    {
                        Monitor.Exit(obj);
                    }
                }

                return HUD._singleton;
            }
        }
        ScreenManager _screenManager;
        bool _showPopup;
        public bool ShowPopup
        {
            get
            {
                return _showPopup;
            }
            internal set
            {
                _showPopup = value;
            }
        }

        private AnimatedText _overlayText;

        private ContentManager _content;

#if Development
        private static PrimitiveBatch _primBatch = new PrimitiveBatch(Screen_Manager.Graphics);
#endif
        #endregion

        private HUD()
        {
        }

        #region Load
        public void Load(ScreenManager screenManager)
        {
            this._screenManager = screenManager;
            _content = new ContentManager(screenManager.Game.Services, "Content");

            _overlayText = new AnimatedText("", ButtonIcon.Interact, 0.5f, TextAlignment.Left);
            _overlayText.SetPosition(ScreenAnchorLocation.BottomLeft, screenManager.GraphicsDevice);
            _overlayText.TextType = AnimatedTextType.Flash;
            _overlayText.AnchorPoint = ScreenAnchorLocation.BottomLeft;
            _overlayText.Offset = new Vector2(20, -20);
            _overlayText.Load(_content);
            _overlayText.Scale = 0.5f;

            this._screenManager.GraphicsDevice.DeviceReset += DeviceReset;
        }
        #endregion

        #region Update
        public void Update(float delta)
        {
            if (!ShowPopup)
            {
                return;
            }

            _overlayText.Update(delta);
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch sb)
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

        public void ShowOnScreenMessage(bool set, string message)
        {
            _overlayText.Reset();

            if (set)
            {
                _showPopup = true;

                _overlayText.Text = message;
                _overlayText.UpdateOrigin();
            }
            else
            {
                _showPopup = false;
            }
        }
        #endregion

        void DeviceReset(object sender, EventArgs e)
        {
            _overlayText.SetPosition(ScreenAnchorLocation.BottomLeft, this._screenManager.GraphicsDevice);
        }

        public void RefreshHUD()
        {
            ShowPopup = false;
            _overlayText.Reset();
        }
    }
}
