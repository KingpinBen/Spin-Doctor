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
#endregion

namespace GameLibrary.Graphics.UI
{
    class AnimatedText : TextString
    {
        #region Fields
        private bool _animationPeak;
        private float _elapsedTimer;
        private float _animationTime;
        private float _textSpacing;
        private AnimatedTextType _textType;
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
        public AnimatedTextType TextType
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
        public AnimatedText(string TextToDisplay, ButtonIcon ButtonType, float AnimationTime, Alignment alignment)
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
            if (_textType == AnimatedTextType.Flash)
            {
                float speed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

                if (_animationPeak)
                    _alpha = Math.Max(_alpha - speed, 0.0f);
                else
                    _alpha = Math.Min(_alpha + speed, 1.0f);

                if (_alpha >= 1 || _alpha <= 0.0f)
                    _animationPeak = !_animationPeak;
            }
            else if (TextType == AnimatedTextType.Grow)
            {
                //  BROKEN WITH THE NEW FONTMANAGER
                float speed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

                if (_animationPeak)
                    _textSpacing = Math.Max(_textSpacing - speed, 0.0f);
                else
                    _textSpacing = Math.Min(_textSpacing + speed, 1.0f);

                if (_textSpacing >= 1 || _textSpacing <= 0)
                    _animationPeak = !_animationPeak;

                //_font.Spacing = _textSpacing;
                UpdateOrigin();
            }
            else if (TextType == AnimatedTextType.Fade)
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

            if (TextType == AnimatedTextType.Fade || TextType == AnimatedTextType.Flash)
            {
                _alpha = 0.0f;
            }
        }
    }
}
