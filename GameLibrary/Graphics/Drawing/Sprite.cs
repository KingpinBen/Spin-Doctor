//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Sprite
//--    
//--    Description
//--    ===============
//--    Draw animated and non animated sprites.
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Gave an extra constructor for non-animated sprites so Update won't 
//--           need to be called every cycle unnecessarily.
//--    BenG - Made cloneable.
//--    
//-- 
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.GameLogic.Objects;
using GameLibrary.GameLogic;
using FarseerPhysics.Dynamics;
#endregion

namespace GameLibrary.Graphics.Drawing
{
    public class Sprite : NodeObject
    {
        #region Fields
#if EDITOR

#else
        private float _elapsed = 0.0f;
        private Point _currentFrame = new Point(0, 0);
        private bool _isDead = false;
#endif
        [ContentSerializer(Optional = true)]
        private string _textureAsset;
        [ContentSerializer(Optional = true)]
        private float _rotation = 0.0f;
        [ContentSerializer(Optional = true)]
        private float _rotationSpeed;
        [ContentSerializer(Optional = true)]
        private float _scale = 1.0f;
        [ContentSerializer(Optional = true)]
        private float _scaleFactor;
        [ContentSerializer(Optional = true)]
        private float _alpha = 1.0f;
        [ContentSerializer(Optional = true)]
        private float _alphaDecay;
        [ContentSerializer(Optional = true)]
        private Vector2 _velocity = Vector2.Zero;
        [ContentSerializer(Optional = true)]
        private Color _tint = Color.White;
        [ContentSerializer(Optional = true)]
        private int _timesToPlay = 1;

        private Texture2D _texture;

        //  Animated object fields
        [ContentSerializer(Optional = true)]
        private bool _isAnimated;
        [ContentSerializer(Optional = true)]
        private bool _isAnimating = true;
        [ContentSerializer(Optional = true)]
        private Point _singleFrameDimensions = new Point(0, 0);
        [ContentSerializer(Optional = true)]
        private Point _frameCount = new Point(1, 1);
        
        
        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore]
        public override float Width
        {
            get
            {
                return _singleFrameDimensions.X;
            }
            set
            {
                _singleFrameDimensions.X = (int)value;
            }
        }
        [ContentSerializerIgnore]
        public override float Height
        {
            get
            {
                return _singleFrameDimensions.Y;
            }
            set
            {
                _singleFrameDimensions.Y = (int)value;
            }
        }
#else
        [ContentSerializerIgnore]
        public bool IsDead
        {
            get
            {
                return _isDead;
            }
        }
        [ContentSerializerIgnore]
        public float Width
        {
            get
            {
                return _singleFrameDimensions.X;
            }
            set
            {
                _singleFrameDimensions.X = (int)value;
            }
        }
        [ContentSerializerIgnore]
        public float Height
        {
            get
            {
                return _singleFrameDimensions.Y;
            }
            set
            {
                _singleFrameDimensions.Y = (int)value;
            }
        }
#endif

        
        [ContentSerializerIgnore]
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
        [ContentSerializerIgnore]
        public Color Tint
        {
            get
            {
                return _tint;
            }
            set
            {
                _tint = value;
            }
        }
        [ContentSerializerIgnore]
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }
        [ContentSerializerIgnore]
        public int TimesToPlay
        {
            get
            {
                return _timesToPlay;
            }
            set
            {
                if (!_isAnimated) 
                    return;

                _timesToPlay = value;
            }
        }
        [ContentSerializerIgnore]
        public Vector2 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }
        [ContentSerializerIgnore]
        public float Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = MathHelper.Clamp(value, 0.0f, 1.0f);
            }
        }
        [ContentSerializerIgnore]
        public bool Animate
        {
            get
            {
                return _isAnimated;
            }
            set
            {
                this._isAnimated = value;
            }
        }
        [ContentSerializerIgnore]
        public new float ZLayer
        {
            get
            {
                return this._zLayer;
            }
            set
            {
                this._zLayer = MathHelper.Clamp(value, 0.0f, 0.9f);
            }

        }
        [ContentSerializerIgnore]
        public float RotationSpeed
        {
            get
            {
                return _rotationSpeed;
            }
            set
            {
                _rotationSpeed = value;
            }
        }
        [ContentSerializerIgnore]
        public float ScaleFactor
        {
            get
            {
                return _scaleFactor;
            }
            set
            {
                _scaleFactor = value;
            }
        }
        [ContentSerializerIgnore]
        public float AlphaDecay
        {
            get
            {
                return _alphaDecay;
            }
            set
            {
                _alphaDecay = value;
            }
        }

        #endregion

        #region Constructors and Init

        public Sprite() : base() { }

        /// <summary>
        /// Initializes an animated sprite
        /// </summary>
        /// <param name="frameDimensions">Dimensions of 1 frame of the spritesheet in px</param>
        /// <param name="spriteSheetDims">how many frames on the spritesheet</param>
        /// <param name="timesToPlay">how many times the animation should play. -1 for infinite</param>
        public void Init(Vector2 position, Point frameDimensions, Point spriteSheetDims, int timesToPlay)
        {
            this._position = position;
            this._singleFrameDimensions = frameDimensions;
            this._frameCount = spriteSheetDims;
            this._timesToPlay = timesToPlay;
            this._isAnimated = true;
        }

        public void Init(Vector2 position, string textureAsset)
        {
            this._isAnimated = false;
            this._position = position;
            this._textureAsset = textureAsset;
        }

        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            _texture = content.Load<Texture2D>(_textureAsset);

#if EDITOR
            if (this.Width <= 0 || this.Height <= 0)
            {
                this.Width = _texture.Width;
                this.Height = _texture.Height;
            }
#else
            this.RegisterObject();
#endif
        }

        #endregion

        public override void Update(float delta)
        {
#if !EDITOR
            
            this._position += this._velocity;

            if (_isAnimating)
            {
                this._scale += this._scaleFactor;
                this._alpha -= this._alphaDecay;
                this._rotation += _rotationSpeed;
            }

            if (this._alpha <= 0.0f)
            {
                this._isDead = true;
                return;
            }

            if (_isAnimated && _isAnimating)
            {
                this._elapsed += delta;

                if (_elapsed > 1 / 2)
                {
                    ++_currentFrame.X;

                    if (_currentFrame.X >= _frameCount.X)
                    {
                        _currentFrame.X = 0;
                        ++_currentFrame.Y;

                        if (_currentFrame.Y >= _frameCount.Y)
                        {
                            _currentFrame.Y = 0;

                            //  Allows sprites to indefinitely cycle if set to -1
                            if (TimesToPlay > 0)
                            {
                                TimesToPlay -= 1;

                                if (TimesToPlay == 0)
                                {
                                    _isDead = true;
                                }
                            }
                        }

                    }

                    _elapsed = 0.0f;
                }
            }
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_texture, new Rectangle(
                (int)(_position.X - _singleFrameDimensions.X * 0.5f),
                (int)(_position.Y - _singleFrameDimensions.Y * 0.5f),
                (int)(_singleFrameDimensions.X),
                (int)(_singleFrameDimensions.Y)), 
                null, _tint, _rotation, new Vector2(_singleFrameDimensions.X * 0.5f,
                                _singleFrameDimensions.Y * 0.5f), SpriteEffects.None, _zLayer);
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            if (_enabled)
            {
                BlendState bstate = graphics.BlendState;
                graphics.BlendState = BlendState.Additive;


                if (this._isAnimated)
                {
                    sb.Draw(_texture, _position,
                        new Rectangle(
                            _currentFrame.X * _singleFrameDimensions.X,
                            _currentFrame.Y * _singleFrameDimensions.Y,
                            _singleFrameDimensions.X,
                            _singleFrameDimensions.Y), Tint, _rotation,
                            new Vector2(_singleFrameDimensions.X * 0.5f,
                                _singleFrameDimensions.Y * 0.5f), _scale, SpriteEffects.None, _zLayer);
                }
                else
                {
                    sb.Draw(this._texture, this._position,
                        null, this._tint * _alpha, this._rotation,
                        new Vector2(this._texture.Width, this._texture.Height) * 0.5f,
                        _scale, SpriteEffects.None, this._zLayer);
                }


                graphics.BlendState = bstate;
            }
        }
#endif
        #endregion

        #region Events

#if !EDITOR

        public override void Toggle()
        {
            this._isAnimating = !this._isAnimating;
        }

        public override void Start()
        {
            this._isAnimating = true;
        }

        public override void Stop()
        {
            this._isAnimating = false;
        }

        public override void Change(object sent)
        {
            if (sent is float)
            {
                this._rotationSpeed = (float)sent;
            }
        }
#endif

        void SetAnimation(bool state)
        {
            _isAnimating = state;
        }

        #endregion

        public void SetTexture(Texture2D texture)
        {
            this._texture = texture;
        }
    }
}
