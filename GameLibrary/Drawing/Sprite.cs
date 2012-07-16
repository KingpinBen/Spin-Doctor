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
using GameLibrary.Screens;
using GameLibrary.Managers;
using GameLibrary.Assists;
using GameLibrary.Objects;
#endregion

namespace GameLibrary.Drawing
{
    public class Sprite : NodeObject
    {
        #region Fields
#if EDITOR
#else
        private float _elapsed = 0.0f;
#endif
        
        private float _rotation = 0.0f;
        private float _scale = 1.0f;
        private float _alpha = 1.0f;
        private bool _isAnimated;
        private bool _isAnimating = true;
        private bool _isDead = false;
        private bool _isDying = false;
        private int _timesToPlay = 1;
        private Texture2D _spriteTexture;
        private Point _singleFrameDimensions = new Point(0, 0);
        private Point _currentFrame = new Point(0, 0);
        private Point _frameCount = new Point(1, 1);
        private Vector2 _velocity = Vector2.Zero;
        private Color _tint = Color.White;
        
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

        #endregion

        #region Constructors and Init

        public Sprite() { }

        /// <summary>
        /// Initializes an animated sprite
        /// </summary>
        /// <param name="frameDimensions">Dimensions of 1 frame of the spritesheet in px</param>
        /// <param name="spriteSheetDims">how many frames on the spritesheet</param>
        /// <param name="timesToPlay">how many times the animation should play. -1 for infinite</param>
        public void Init(Vector2 position, Point frameDimensions, Point spriteSheetDims, int timesToPlay)
        {
            this._singleFrameDimensions = frameDimensions;
            this._frameCount = spriteSheetDims;
            this._timesToPlay = timesToPlay;
            this._isAnimated = true;
        }

        public void Init()
        {
            this._isAnimated = false;
        }

        #endregion

        #region Load
        /// <summary>
        /// Some textures are passed in through a property. Use this when you want to load it
        /// in Sprite (for separate, not instanced sprites).
        /// </summary>
        /// <param name="tex">The textures content location.</param>
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
            }

            this._isAnimating = true;
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            if (_velocity != Vector2.Zero) _position += _velocity;

            if (_isDying)
            {
                _alpha -= 0.02f;
                if (_alpha <= 0)
                {
                    _isDead = true;
                }
            }

            //  Code below this is for animated sprites - may as well
            //  break out if it's not needed.
            if (_isDead || !_isAnimated) return;

            this._elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

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
        #endregion

        #region Draw
        public override void Draw(SpriteBatch sb)
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
        #endregion

        #region Kill the sprite.
        /// <summary>
        /// Publicly allows killing of the sprite.
        /// </summary>
        /// <param name="fade">Should it fade out?</param>
        public void Kill(bool fade)
        {
            if (fade)
                _isDying = true;
            else
                _isDead = true;
        }
        #endregion

        #region Activate / Deactivate Animation

        public void ToggleAnimating()
        {
            this.SetAnimation(!_isAnimating);
        }

        public void ActivateAnimation()
        {
            if (_isAnimating)
            {
                return;
            }

            this.SetAnimation(true);
        }

        public void DeactivateAnimation()
        {
            if (!_isAnimating)
            {
                return;
            }

            this.SetAnimation(false);
        }

        private void SetAnimation(bool state)
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
