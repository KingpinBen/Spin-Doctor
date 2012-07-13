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
using GameLibrary.Assists;
using GameLibrary.Managers;
using Microsoft.Xna.Framework.Content;
#endregion

namespace GameLibrary.Drawing
{
    public enum Alignment
    {
        Left,
        Right,
        Centre
    }

    #region Image Type
    /// <summary>
    /// What type of logo to display?
    /// </summary>
    public enum ImgType
    {
        None,
        Continue,
        Interact,
        Cancel
    }
    #endregion

    public enum ScreenAnchorLocation
    {
        TopLeft, Top, TopRight,
        Left, Centre, Right,
        BottomLeft, Bottom, BottomRight,
    }

    class TextString
    {
        #region Fields
        protected string _text;
        protected ScreenAnchorLocation _anchorPoint;
        protected Vector2 _textOrigin;
        protected Vector2 _offset;
        protected float _scale;
        protected float _alpha;
        protected Color _tint;
        protected ImgType _buttonType;
        protected Texture2D _texture;
        protected SpriteFont _font;
        protected Alignment _textAlignment;
        private Vector2 _textureOffset;

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
        public Vector2 Position
        {
            get
            {
                switch (_anchorPoint)
                {
                    case ScreenAnchorLocation.Top:
                        {
                            return new Vector2(Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Center.X, Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Top);
                        }
                    case ScreenAnchorLocation.TopLeft:
                        {
                            return new Vector2(Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Left, Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Top);
                        }
                    case ScreenAnchorLocation.TopRight:
                        {
                            return new Vector2(Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Right, Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Top);
                        }
                    case ScreenAnchorLocation.Bottom:
                        {
                            return new Vector2(Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Center.X, Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Bottom);
                        }
                    case ScreenAnchorLocation.BottomLeft:
                        {
                            return new Vector2(Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Left, Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Bottom);
                        }
                    case ScreenAnchorLocation.BottomRight:
                        {
                            return new Vector2(Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Right, Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Bottom);
                        }
                    case ScreenAnchorLocation.Left:
                        {
                            return new Vector2(Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Left, Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Center.Y);
                        }
                    case ScreenAnchorLocation.Centre:
                        {
                            return new Vector2(Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Center.X, Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Center.Y);
                        }
                    case ScreenAnchorLocation.Right:
                        {
                            return new Vector2(Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Right, Screen_Manager.GraphicsDevice.Viewport.TitleSafeArea.Center.Y);
                        }
                }
                return Vector2.Zero;
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
        public ImgType ButtonType
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
        public SpriteFont Font
        {
            get
            {
                return _font;
            }
        }
        public Alignment TextAlignment
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

        #region Constructor
        /// <summary>
        /// Display text on screen.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public TextString(string text)
        {
            this._text = text;
            this._scale = 1.0f;
            this._alpha = 1.0f;
            this._tint = Color.White;
            this._font = Fonts.MenuFont;
            this._anchorPoint = ScreenAnchorLocation.Centre;
        }
        #endregion

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
            if (_buttonType == ImgType.Cancel)
            {
                if (InputManager.Instance.isGamePad)
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
            else if (_buttonType == ImgType.Continue)
            {
                if (InputManager.Instance.isGamePad)
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
            else if (_buttonType == ImgType.Interact)
            {
                if (InputManager.Instance.isGamePad)
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

            UpdateOrigin();
        }
        #endregion

        #region Update
        public virtual void Update(GameTime gameTime) { }
        #endregion

        #region Draw
        public virtual void Draw(SpriteBatch sb)
        {
            sb.DrawString(_font, _text, Position + _offset + (this._textureOffset * Scale), _tint * _alpha, 0f, _textOrigin, _scale, SpriteEffects.None, 0.1f);

            if (_buttonType == ImgType.None) 
                return;

            sb.Draw(_texture, Position + _offset, null, Color.White * _alpha, 0f,
                _textOrigin, _scale, SpriteEffects.None, 0.1f); 
        }
        #endregion

        #region Origin Update
        public virtual void UpdateOrigin()
        {
            float width = _font.MeasureString(_text).X;
            float height = _font.MeasureString("Y").Y;

            switch (_textAlignment)
            {
                case Alignment.Centre:
                    _textOrigin = new Vector2(width / 2, height);
                    break;
                case Alignment.Left:
                    _textOrigin = new Vector2(0, height);
                    break;
                case Alignment.Right:
                    _textOrigin = new Vector2(width, height);
                    break;
            }
        }
        #endregion
    }
}
