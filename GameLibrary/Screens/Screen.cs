//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - Screen
//--    
//--    
//--    Description
//--    ===============
//--    Abstract class for screens to be used with the ScreenManager
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial 
//--    BenG - Now has transitions (in/out)
//--    BenG - Better memory handling for certain screens
//--    
//--    
//--    TBD
//--    ==============
//--    Gameplay -> loading doesn't have a proper transition
//--    Maybe have a blur on the transition
//--    
//--------------------------------------------------------------------------


#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;
using GameLibrary.Assists;
using GameLibrary.Managers;
#endregion

namespace GameLibrary.Screens
{
    //  Hidden is barely used. Check TBD
    public enum State
    {
        FadeIn, FadeOut,
        Show, Hidden
    }

    public abstract class Screen
    {
        #region Fields and Variables

        public string Name
        {
            get
            {
                return _name;
            }
            internal set
            {
                _name = value;
            }
        }
        private string _name;
        
        public State ScreenState
        {
            get
            {
                return _screenState;
            }
            protected set
            {
                _screenState = value;
            }
        }
        private State _screenState;

        /// <summary>
        /// Time used for calculating the alpha
        /// </summary>
        public float TransitionTime
        {
            get
            {
                return _timeLeftTransitioning;
            }
            internal set
            {
                _timeLeftTransitioning = value;
            }
        }
        private float _timeLeftTransitioning;
        
        /// <summary>
        /// Can only be set once. The time used to cause transitions.
        /// </summary>
        public float FadeDuration
        {
            get
            {
                return _fadeDuration;
            }
        }
        private float _fadeDuration = 0;

        public float TransitionAlpha
        {
            get
            {
                return _transitionFadeAlpha;
            }
            internal set
            {
                _transitionFadeAlpha = value;
            }
        }
        private float _transitionFadeAlpha;

        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
            protected set
            {
                _isInitialized = value;
            }
        }
        private bool _isInitialized;

        public bool IsExitable
        {
            get
            {
                return _isExitable;
            }
            protected set
            {
                _isExitable = value;
            }
        }
        private bool _isExitable = true;

        protected ContentManager Content;

        #endregion

        #region Constructor
        public Screen(string name, float fadeDuration)
        {
            this.Name = name;
            this._fadeDuration = fadeDuration;
            this.ScreenState = State.FadeIn;

            this.TransitionTime = fadeDuration;
            this.TransitionAlpha = 1f;
            this.Content = new ContentManager(Screen_Manager.Game.Services, "Content");
        }
        #endregion

        #region Load
        public virtual void Load() { }
        #endregion

        #region Unload
        public virtual void Unload()
        {

        }
        #endregion

        #region Update
        /// <summary>
        /// Override
        /// </summary>
        /// <param name="gameTime">Grab elapsed time</param>
        public virtual void Update(GameTime gameTime)
        {
            if (ScreenState == State.FadeIn ||
                ScreenState == State.FadeOut)
            {
                HandleTransition(gameTime);
            }
            else
            {
                return;
            }
            
        }
        #endregion

        #region Draw
        /// <summary>
        /// Override
        /// </summary>
        /// <param name="SpriteBatch">SpriteBatch</param>
        public virtual void Draw(SpriteBatch sb)
        {
            if (ScreenState == State.Show) return;

            sb.Draw(Screen_Manager.BlackPixel,
                new Rectangle( 0, 0, (int)Screen_Manager.Viewport.X, (int)Screen_Manager.Viewport.Y),
                null, Color.Black * TransitionAlpha);
        }
        #endregion

        #region HandleTransition
        public void HandleTransition(GameTime gameTime)
        {
            // Check if the screen has finished transitioning.
            if (TransitionTime <= 0)
            {
                //  Finished transitioning

                switch (ScreenState)
                {
                    case State.FadeIn:
                        ScreenState = State.Show;
                        break;

                    case State.FadeOut:
                        if (IsExitable)
                        {
                            Screen_Manager.DeleteScreen();
                        }
                        else
                        {
                            ScreenState = State.Hidden;
                        }
                        break;
                }

                
            }
            else
            {
                // Still some time left to transition
                float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;
                TransitionTime -= fadeSpeed;

                if (ScreenState == State.FadeOut)
                    TransitionAlpha = Math.Min(TransitionAlpha + fadeSpeed, 1f);
                else if (ScreenState == State.FadeIn)
                    TransitionAlpha = Math.Max(TransitionAlpha - fadeSpeed, 0f);

                if (TransitionTime < 0)
                {
                    TransitionTime = 0;
                }
            }
        }
        #endregion

        public void FadeOut()
        {
            if (ScreenState == State.Show)
            {
                ScreenState = State.FadeOut;
                TransitionTime = FadeDuration;
            }
        }

        public void FadeIn()
        {
            if (ScreenState == State.Hidden)
            {
                ScreenState = State.FadeIn;
                TransitionTime = FadeDuration;
            }
        }
    }
}
