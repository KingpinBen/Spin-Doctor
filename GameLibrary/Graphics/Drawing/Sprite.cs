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
#endregion

namespace GameLibrary.Graphics.Drawing
{
    public class Sprite : NodeObject
    {
        #region Fields
#if EDITOR
#else
        private float _elapsed = 0.0f;
#endif
        
        private float _rotation = 0.0f;
        private float _rotationSpeed;
        private float _scale = 1.0f;
        private float _scaleFactor;
        private float _alpha = 1.0f;
        private float _alphaDecay;
        private Vector2 _velocity = Vector2.Zero;
        private Color _tint = Color.White;
        private bool _isDead = false;
        private bool _isDying = false;
        private int _timesToPlay = 1;
        private Texture2D _spriteTexture;

        //  Animated object fields
        private bool _isAnimated;
        private bool _isAnimating = true;
        private Point _singleFrameDimensions = new Point(0, 0);
        private Point _currentFrame = new Point(0, 0);
        private Point _frameCount = new Point(1, 1);
        
        
        #endregion

        #region Properties
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
                return _tint * _alpha;
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
        public bool IsDead
        {
            get
            {
                return _isDead;
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
            this._tint = Color.White;
        }

        public override void Init(Vector2 position)
        {
            this._isAnimated = false;
            this._position = position;
            this._tint = Color.White;
        }

        #endregion

        #region Load
        public void Load(Texture2D texture)
        {
            this._spriteTexture = texture;

            this.Load();
        }

        public void Load()
        {
            if (!_isAnimated)
            {
                _singleFrameDimensions = new Point((int)_spriteTexture.Width, (int)_spriteTexture.Height);
                this._isAnimating = true;
            }
        }
        #endregion

        public override void Update(float delta)
        {
#if !EDITOR
            this._position += this._velocity;
            this._scale += this._scaleFactor;
            this._alpha -= this._alphaDecay;
            this._rotation += _rotationSpeed;

            if (this._alpha <= 0.0f)
            {
                this._isDead = true;
                return;
            }

            //  Code below this is for animated sprites - may as well
            //  break out if it's not needed.
            if (!_isAnimated)
            {
                return;
            }

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
#endif
        }

#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            BlendState bstate = graphics.BlendState;
            graphics.BlendState = BlendState.Additive;

            if (this._isAnimated)
            {
                sb.Draw(_spriteTexture, _position,
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
                sb.Draw(this._spriteTexture, this._position,
                    null, this.Tint, this._rotation, 
                    new Vector2(this._spriteTexture.Width * 0.5f, this._spriteTexture.Height * 0.5f), 
                    _scale, SpriteEffects.None, this._zLayer);
            }

            graphics.BlendState = bstate;
        }
#endif

        #region Activate / Deactivate Animation

        public override void Enable()
        {
            if (_isAnimated)
            {
                if (!_isAnimating)
                {
                    this.SetAnimation(true);
                }
            }
            else
            {

            }
        }

        public override void Disable()
        {
            if (_isAnimated)
            {
                if (_isAnimating)
                {
                    this.SetAnimation(false);
                }
            }
            else
            {

            }
        }

        void SetAnimation(bool state)
        {
            _isAnimating = state;
        }

        #endregion

        public void SetTexture(Texture2D texture)
        {
            this._spriteTexture = texture;
        }
    }
}
