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

        private ContentManager _content;
        private ScreenManager _screenManager;

        private bool _showPopup;
        private TextString _popupText;

        private bool _tempPopupEnabled = false;
        private bool _tempHold = false;
        private TextString _tempPopupText;
        private float _tempElapsed = 0.0f;
        
        #endregion

        #region Properties

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

        #endregion

        #region Singleton and Load

        private static HUD _singleton = new HUD();
        public static HUD Instance
        {
            get
            {
                return HUD._singleton;
            }
        }

        private HUD()
        {

        }

        public void Load(ScreenManager screenManager)
        {
            this._screenManager = screenManager;
            this._content = new ContentManager(screenManager.Game.Services, "Content");

            this._popupText = new TextString("");
            this._popupText.Load(_content);
            this._popupText.Alpha = 0.0f;
            this._popupText.TextAlignment = TextAlignment.Centre;
            this._popupText.Position = new Vector2(this._screenManager.GraphicsDevice.Viewport.Width * 0.5f, this._screenManager.GraphicsDevice.Viewport.Height * 0.85f);

            this._tempPopupText = new TextString("");
            this._tempPopupText.Load(_content);
            this._tempPopupText.Alpha = 0.0f;
            this._tempPopupText.TextScale = 1.3f;
            this._tempPopupText.TextAlignment = TextAlignment.Centre;
            this._tempPopupText.Position = new Vector2(this._screenManager.GraphicsDevice.Viewport.Width * 0.5f, this._screenManager.GraphicsDevice.Viewport.Height * 0.9f);
        }

        #endregion

        #region Update and Draw
        public void Update(float delta)
        {
            if (_showPopup)
            {
                if (_popupText.Alpha < 1.0f)
                {
                    this._popupText.Alpha = Math.Min(_popupText.Alpha + (delta * 3), 1.0f);
                }
            }
            else
            {
                if (_popupText.Alpha > 0)
                {
                    this._popupText.Alpha = Math.Max(_popupText.Alpha - (delta * 3), 0.0f);
                }
            }

            if (_tempPopupEnabled)
            {
                if (_tempPopupText.Alpha < 1.0f)
                {
                    this._tempPopupText.Alpha = Math.Min(_tempPopupText.Alpha + (delta * 3), 1.0f);
                }
                else
                {
                    this._tempElapsed += delta;

                    if (_tempElapsed >= 5.0f)
                    {
                        this._tempPopupEnabled = false;
                        this._tempElapsed = 0.0f;
                    }
                }
            }
            else
            {
                if (_tempPopupText.Alpha > 0)
                {
                    this._tempPopupText.Alpha = Math.Max(_tempPopupText.Alpha - (delta * 3), 0.0f);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            if (_popupText.Alpha != 0)
            {
                this._popupText.Draw(sb);
            }

            if (_tempPopupText.Alpha != 0)
            {
                this._tempPopupText.Draw(sb);
            }

            sb.End();

        }
        #endregion

        #region Public Methods

        public void RefreshHUD()
        {
            this._showPopup = false;
            this._popupText.Alpha = 0.0f;
        }

        public void ShowOnScreenMessage(bool set, string message, ButtonIcon type)
        {
            if (set)
            {
                _showPopup = true;

                _popupText.ButtonType = type;
                _popupText.Text = message;
                _popupText.UpdateOrigin();
            }
            else
            {
                _showPopup = false;
            }
        }

        public void CreateTemporaryPopup(string message, int i)
        {
            //  Enable the temporary popup
            this._tempPopupEnabled = true;

            //  Assign the icon
            switch (i)
            {
                case 1:
                    this._tempPopupText.ButtonType = ButtonIcon.Action1;
                    break;
                case 2:
                    this._tempPopupText.ButtonType = ButtonIcon.Action2;
                    break;
                case 3:
                    this._tempPopupText.ButtonType  = ButtonIcon.Action3;
                    break;
                case 4:
                    this._tempPopupText.ButtonType = ButtonIcon.Action4;
                    break;
                default:
                    this._tempPopupText.ButtonType = ButtonIcon.None;
                    break;
            }
            

            //  Setup the message
            this._tempPopupText.Text = message;
        }

        #endregion

        #region Private Methods

        void DeviceReset(object sender, EventArgs e)
        {
            this._popupText.SetPosition(ScreenAnchorLocation.BottomLeft, this._screenManager.GraphicsDevice);
        }

        #endregion
    }
}
