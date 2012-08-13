//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - FrameAnimation
//--    
//--    
//--    
//--    Description
//--    ===============
//--    Animation - Stores frames and animation timer to return a sourceRect.
//--    
//--    Revision List
//--    ===============
//--    BenP - Initial
//--    BenG - Added new constructor to change the frame length speed.
//--    BenG - Each animation is now stored on a custom texture unique to the
//--           animation.
//--    BenG - Now supports multi-lined spritesheets.
//--    
//--    TBD
//--    ==============
//--    
//--
//--------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Graphics.Animation
{
    public class FrameAnimation
    {
        #region Fields

        private Rectangle[] _frames; //Animation Frames
        private int _currentFrame = 0;
        private Vector2 _frameOrigin;
        private Texture2D _texture;
        private bool _reversePlayback = true;
        private float _animationScale = 0.43f;
        private bool _playOnce;

        /// <summary>
        /// Time between frames.
        /// Spritesheets were rendered at 30fps, halve the time for 60fps
        /// </summary>
        private float _frameLength = 1 / 30;
        private float _timer = 0;
        #endregion

        #region Properties

        public bool PlayOnce
        {
            get
            {
                return _playOnce;
            }
        }

        public int FrameCount
        {
            get
            {
                return _frames.Length;
            }
        }

        public Vector2 FrameOrigin
        {
            get
            {
                return _frameOrigin;
            }
            set
            {
                _frameOrigin = value;
            }
        }

        public float FrameLength
        {
            get
            {
                return _frameLength;
            }

            set
            {
                _frameLength = value;
            }
        }

        /// <summary>
        /// SourceRect to be used in Draw.
        /// </summary>
        public Rectangle CurrentRect
        {
            get { return _frames[_currentFrame]; }
        }

        public Texture2D CurrentAnimationTexture
        {
            get
            {
                return _texture;
            }
        }

        public bool ReversePlayback
        {
            get
            {
                return _reversePlayback;
            }
        }

        public bool Completed
        {
            get
            {
                if (PlayOnce)
                {
                    if ((_reversePlayback && _currentFrame == 0) || (!_reversePlayback && _currentFrame == _frames.Length))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public float Scale
        {
            get
            {
                return _animationScale;
            }
            set
            {
                _animationScale = value;
            }
        }

        
        #endregion

        #region Constructor
        public FrameAnimation(Texture2D asset, int numOfFrames, Point frameDims, float yOffset, Point frameCount, bool playOnce)
        {
            //Adds the frames
            this._frames = new Rectangle[numOfFrames];
            this._frameOrigin = new Vector2(frameDims.X * 0.5f, (frameDims.Y - yOffset) * 0.5f);
            this._texture = asset;
            this._currentFrame = _frames.Length - 1;
            this._playOnce = playOnce;

            for (int i = 0; i < _frames.Length; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = frameDims.X;
                rect.Height = frameDims.Y;
                rect.X = (i * frameDims.X);
                rect.Y = (i / frameCount.X) * frameDims.Y;

                _frames[i] = rect;
            }
        }

        public FrameAnimation(Texture2D asset, int numOfFrames, Point frameDims, float yOffset, Point frameCount, bool playOnce, float framepersecond)
            : this(asset, numOfFrames, frameDims, yOffset, frameCount, playOnce)
        {
            _frameLength = 1 / framepersecond;
        }
        #endregion

        public void Update(float delta)
        {
            if (PlayOnce && _currentFrame == 0)
            {
                return;
            }

            _timer += delta;

            if (_timer >= _frameLength)
            {
                if (_reversePlayback)
                {
                    if (_currentFrame - 1 < 0)
                    {
                        _currentFrame = _frames.Length;
                    }

                    _currentFrame -= 1;
                }
                else
                {
                    _currentFrame = (_currentFrame + 1) % _frames.Length;
                }

                _timer = 0;
            }
        }

        public void ResetCurrentFrame()
        {
            _currentFrame = _frames.Length - 1;
        }

        public void SetPlayback(bool shouldReverse)
        {
            _reversePlayback = shouldReverse;
        }
    }
}
