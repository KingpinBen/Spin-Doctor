//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor 
//--
//--    
//--    Description
//--    ===============
//--    Allows buttons to placed next to text
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Moved fields from AnimatedText and made this parent
//--    BenG - If the object is displaying an icon, it'll account that into the 
//--           position.
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Graphics;
using GameLibrary.GameLogic.Controls;
using GameLibrary.GameLogic;
#endregion

namespace GameLibrary.Graphics.UI
{
    class TextString
    {
        #region Fields
        
        protected SpriteFont _font;
        protected Color _tint;
        protected Vector2 _textOrigin;
        protected Vector2 _position;

        protected float _scale;
        protected float _alpha;
        protected string _text;
        
        protected ButtonIcon _buttonType;
        protected ScreenAnchorLocation _anchorPoint;
        protected Texture2D[] _buttonTextures;
        protected TextAlignment _textAlignment;
        

        #endregion

        #region Properties

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }
        public virtual Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }
        public Vector2 Origin
        {
            get
            {
                return _textOrigin;
            }
        }
        public float ButtonScale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }
        public float TextScale { get; set; }
        public float Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
            }
        }
        public Color Tint
        {
            get
            {
                return _tint;
            }
        }
        public ButtonIcon ButtonType
        {
            get
            {
                return _buttonType;
            }
            set
            {
                _buttonType = value;
            }
        }
        private Texture2D ButtonTexture
        {
            get
            {
                return _buttonTextures[(int)_buttonType];
            }
        }
        public TextAlignment TextAlignment
        {
            get
            {
                return _textAlignment;
            }
            set
            {
                _textAlignment = value;
            }
        }
        public ScreenAnchorLocation AnchorPoint
        {
            get
            {
                return _anchorPoint;
            }
            set
            {
                _anchorPoint = value;
            }
        }

        #endregion

        public TextString(string text)
        {
            this._text = text;
            this._alpha = 1.0f;
            this._tint = Color.White;
            this.TextScale = 1.0f;
            this._anchorPoint = ScreenAnchorLocation.Centre;
        }

        #region Load
        /// <summary>
        /// If you want an icon displayed next to the text, you'll need to make sure you
        /// change the bool DisplayIcon before calling this.
        /// </summary>
        /// <param name="content">Content</param>
        public virtual void Load(ContentManager content)
        {
            InputManager instance = InputManager.Instance;
            this._buttonTextures = instance.ButtonTextures;

            if (instance.IsGamepad)
            {
                this._scale = 0.4f;
            }
            else
            {
                this._scale = 0.6f;
            }

            this._font = FontManager.Instance.GetFont(FontList.GUI);

            UpdateOrigin();
        }
        #endregion

        #region Update and Draw

        public virtual void Update(float delta) { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 textureOffset = Vector2.Zero;
            this.UpdateOrigin();

            if (_buttonType != ButtonIcon.None)
            {
                textureOffset = new Vector2((_buttonTextures[(int)_buttonType].Width * _scale), 0);

                spriteBatch.Draw(_buttonTextures[(int)_buttonType], _position - _textOrigin, null, Color.White * _alpha, 0f, 
                    Vector2.Zero, _scale, SpriteEffects.None, 0.1f);
            }

            spriteBatch.DrawString(_font, _text, _position - _textOrigin + textureOffset, _tint * _alpha, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0.1f);
        }

        #endregion

        public virtual void UpdateOrigin()
        {
            Vector2 textSize = _font.MeasureString(_text);

            if (_buttonType != ButtonIcon.None)
            {
                textSize += new Vector2((_buttonTextures[0].Width * _scale), 0);
            }

            switch (_textAlignment)
            {
                case TextAlignment.Centre:
                    _textOrigin = textSize;
                    _textOrigin.X *= 0.5f;
                    break;
                case TextAlignment.Left:
                    _textOrigin.Y = textSize.Y;
                    break;
                case TextAlignment.Right:
                    _textOrigin = textSize;
                    break;
            }
        }

        public void SetPosition(ScreenAnchorLocation location, GraphicsDevice graphics)
        {
            switch (location)
            {
                case ScreenAnchorLocation.Top:
                    _position = new Vector2(graphics.Viewport.TitleSafeArea.Center.X, graphics.Viewport.TitleSafeArea.Top);
                    break;
                case ScreenAnchorLocation.TopLeft:
                    _position = new Vector2(graphics.Viewport.TitleSafeArea.Left, graphics.Viewport.TitleSafeArea.Top);
                    break;
                case ScreenAnchorLocation.TopRight:
                    _position = new Vector2(graphics.Viewport.TitleSafeArea.Right, graphics.Viewport.TitleSafeArea.Top);
                    break;
                case ScreenAnchorLocation.Bottom:
                    _position = new Vector2(graphics.Viewport.TitleSafeArea.Center.X, graphics.Viewport.TitleSafeArea.Bottom);
                    break;
                case ScreenAnchorLocation.BottomLeft:
                    _position = new Vector2(graphics.Viewport.TitleSafeArea.Left, graphics.Viewport.TitleSafeArea.Bottom);
                    break;
                case ScreenAnchorLocation.BottomRight:
                    _position = new Vector2(graphics.Viewport.TitleSafeArea.Right, graphics.Viewport.TitleSafeArea.Bottom);
                    break;
                case ScreenAnchorLocation.Left:
                    _position = new Vector2(graphics.Viewport.TitleSafeArea.Left, graphics.Viewport.TitleSafeArea.Center.Y);
                    break;
                case ScreenAnchorLocation.Centre:
                    _position = new Vector2(graphics.Viewport.TitleSafeArea.Center.X, graphics.Viewport.TitleSafeArea.Center.Y);
                    break;
                case ScreenAnchorLocation.Right:
                    _position = new Vector2(graphics.Viewport.TitleSafeArea.Right, graphics.Viewport.TitleSafeArea.Center.Y);
                    break;
            }
        }
    }
}
