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
        Rectangle[] frames; //Animation Frames
        int currentFrame = 0;
        private Vector2 frameOrigin;
        private Texture2D _texture;
        private bool _reversePlayback = true;

        /// <summary>
        /// Time between frames.
        /// Spritesheets were rendered at 30fps, halve the time for 60fps
        /// </summary>
        float frameLength = 1 / 30; 
        float timer = 0;
        #endregion

        #region Properties

        public bool PlayOnce { get; set; }

        public int FrameCount
        {
            get
            {
                return frames.Length;
            }
        }

        public Vector2 FrameOrigin
        {
            get { return frameOrigin; }
            set { frameOrigin = value; }
        }

        public float FrameLength
        {
            get { return frameLength; }
            set { frameLength = value; }
        }
        /// <summary>
        /// SourceRect to be used in Draw.
        /// </summary>
        public Rectangle CurrentRect
        {
            get { return frames[currentFrame]; }
        }

        /// <summary>
        /// Frame to return.
        /// </summary>
        public int CurrentFrame
        {
            get { return currentFrame; }
            set
            {
                currentFrame = (int)MathHelper.Clamp(value, 0, frames.Length - 1);
            }
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

        
        #endregion

        #region Constructor
        public FrameAnimation(Texture2D asset, int numOfFrames, Point frameDims, float yOffset, Point frameCount, bool playOnce)
        {
            //Adds the frames
            this.frames = new Rectangle[numOfFrames];
            this.frameOrigin = new Vector2(frameDims.X * 0.5f, (frameDims.Y - yOffset) * 0.5f);
            this._texture = asset;
            this.currentFrame = frames.Length - 1;
            this.PlayOnce = playOnce;

            for (int i = 0; i < frames.Length; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = frameDims.X;
                rect.Height = frameDims.Y;
                rect.X = (i * frameDims.X);
                rect.Y = (i / frameCount.X) * frameDims.Y;

                frames[i] = rect;
            }
        }

        public FrameAnimation(Texture2D asset, int numOfFrames, Point frameDims, float yOffset, Point frameCount, bool playOnce, float framepersecond)
            : this(asset, numOfFrames, frameDims, yOffset, frameCount, playOnce)
        {
            frameLength = 1 / framepersecond;
        }
        #endregion

        public void Update(GameTime gameTime)
        {
            if (PlayOnce && currentFrame == 0)
            {
                return;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

            if (timer >= frameLength)
            {
                if (_reversePlayback)
                {
                    if (currentFrame - 1 < 0)
                    {
                        currentFrame = frames.Length;
                    }

                    currentFrame -= 1;
                }
                else
                {
                    currentFrame = (currentFrame + 1) % frames.Length;
                }

                timer = 0;
            }
        }

        public void ResetCurrentFrame()
        {
            currentFrame = frames.Length - 1;
        }

        public void SetPlayback(bool shouldReverse)
        {
            _reversePlayback = shouldReverse;
        }
    }
}
