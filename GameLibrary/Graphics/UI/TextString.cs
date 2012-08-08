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
        protected string _text;
        protected SpriteFont _font;
        protected ScreenAnchorLocation _anchorPoint;
        protected Vector2 _textOrigin;
        protected Vector2 _offset;
        protected float _scale;
        protected float _alpha;
        protected Color _tint;
        protected ButtonIcon _buttonType;
        protected Texture2D _texture;
        protected TextAlignment _textAlignment;
        private Vector2 _textureOffset;
        protected Vector2 _position;

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
        }
        public Vector2 Origin
        {
            get
            {
                return _textOrigin;
            }
        }
        public Vector2 Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                _offset = value;
            }
        }
        public float Scale
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
        public float Alpha
        {
            get
            {
                return _alpha;
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
        }

        public Texture2D ButtonTexture
        {
            get
            {
                return _texture;
            }
        }

        public TextAlignment TextAlignment
        {
            get
            {
                return _textAlignment;
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
            this._scale = 1.0f;
            this._alpha = 1.0f;
            this._tint = Color.White;
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
            #region Button Texture Load

            #region Cancel
            if (_buttonType == ButtonIcon.Cancel)
            {
                if (InputManager.Instance.IsGamepad)
                {
                    _texture = content.Load<Texture2D>
                        ("Assets/Other/Controls/B");
                }
                else
                {
                    _texture = content.Load<Texture2D>
                        ("Assets/Other/Controls/B");
                }

                this._textureOffset = new Vector2(_texture.Width * 1.2f, 0);
            }
            #endregion

            #region Continue
            else if (_buttonType == ButtonIcon.Continue)
            {
                if (InputManager.Instance.IsGamepad)
                {
                    _texture = content.Load<Texture2D>
                        ("Assets/Other/Controls/A");
                }
                else
                {
                    _texture = content.Load<Texture2D>
                        ("Assets/Other/Controls/A");
                }

                this._textureOffset = new Vector2(_texture.Width * 1.2f, 0);
            }
            #endregion

            #region Interact
            else if (_buttonType == ButtonIcon.Interact)
            {
                if (InputManager.Instance.IsGamepad)
                {
                    _texture = content.Load<Texture2D>
                        ("Assets/Other/Controls/Y");
                }
                else
                {
                    _texture = content.Load<Texture2D>
                        ("Assets/Other/Controls/Y");
                }

                this._textureOffset = new Vector2(_texture.Width * 1.2f, 0);
            }
            #endregion

            else
            {
                this._textureOffset = Vector2.Zero;
            }
            #endregion

            _font = FontManager.Instance.GetFont(FontList.GUI);

            UpdateOrigin();
        }
        #endregion

        public virtual void Update(float delta) { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _text, _position + _offset + (this._textureOffset * Scale), _tint * _alpha, 0f, _textOrigin, _scale, SpriteEffects.None, 0.1f);

            if (_buttonType != ButtonIcon.None)
                spriteBatch.Draw(_texture, _position + _offset, null, Color.White * _alpha, 0f, _textOrigin, _scale, SpriteEffects.None, 0.1f);
        }

        public virtual void UpdateOrigin()
        {
            Vector2 textSize = _font.MeasureString(_text);

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
