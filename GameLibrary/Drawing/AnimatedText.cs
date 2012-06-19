//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor 
//--
//--    
//--    Description
//--    ===============
//--    Text that has animations
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Made it inherit, removed fields and UpdateOrigin() to parent
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
#endregion

namespace GameLibrary.Drawing
{
    public enum AnimatedType
    {
        Flash,
        Grow,
        Fade
    }

    class AnimatedText : TextString
    {
        #region Fields
        private bool _animationPeak;
        private float _elapsedTimer;
        private float _animationTime;
        private float _textSpacing;
        private AnimatedType _textType;
        private float _minimumAlpha = 0.4f;
        #endregion

        #region Properties
        /// <summary>
        /// Is the moving towards the min or max?
        /// </summary>
        public bool MovingToMinimum
        {
            get
            {
                return _animationPeak;
            }
            protected set
            {
                _animationPeak = value;
            }
        }
        /// <summary>
        /// Time between animation peaks.
        /// </summary>
        public float AnimationTime
        {
            get
            {
                return _animationTime;
            }
        }
        public float ElapsedTimer
        {
            get
            {
                return _elapsedTimer;
            }
        }
        public float TextSpacing
        {
            get
            {
                return _textSpacing;
            }
            internal set
            {
                _textSpacing = value;
            }
        }
        public AnimatedType TextType
        {
            get
            {
                return _textType;
            }
            set
            {
                _textType = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Defaults to Left aligned
        /// </summary>
        /// <param name="TextToDisplay"></param>
        /// <param name="TimeForFade"></param>
        public AnimatedText(string TextToDisplay, ImgType ButtonType, float AnimationTime, Alignment alignment)
            : base(TextToDisplay)
        {
            this._animationPeak = false;
            this._elapsedTimer = 0f;
            this._textAlignment = alignment;
            this._animationTime = AnimationTime;
            this._buttonType = ButtonType;
            this._alpha = _minimumAlpha;

            UpdateOrigin();
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            if (_textType == AnimatedType.Flash)
            {
                float speed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

                if (_animationPeak)
                    _alpha = Math.Max(_alpha - speed, 0.0f);
                else
                    _alpha = Math.Min(_alpha + speed, 1.0f);

                if (_alpha >= 1 || _alpha <= 0.0f)
                    _animationPeak = !_animationPeak;
            }
            else if (TextType == AnimatedType.Grow)
            {
                float speed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

                if (_animationPeak)
                    _textSpacing = Math.Max(_textSpacing - speed, 0.0f);
                else
                    _textSpacing = Math.Min(_textSpacing + speed, 1.0f);

                if (_textSpacing >= 1 || _textSpacing <= 0)
                    _animationPeak = !_animationPeak;

                _font.Spacing = _textSpacing;
                UpdateOrigin();
            }
            else if (TextType == AnimatedType.Fade)
            {
                float speed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

                if (_animationPeak)
                    _alpha = Math.Max(_alpha - speed, 0.0f);
                else
                    _alpha = Math.Min(_alpha + speed, 1.0f);
            }
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
        #endregion

        public void ToggleState()
        {
            if (_animationPeak == true)

            _animationPeak = !_animationPeak;
        }

        public void Reset()
        {
            _animationPeak = true;

            if (TextType == AnimatedType.Fade || TextType == AnimatedType.Flash)
            {
                _alpha = 0.0f;
            }
        }
    }
}
