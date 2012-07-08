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

        private float elapsed = 0.0f;
        private float rotation = 0.0f;
        private float scale = 1.0f;
        private float alpha = 1.0f;
        private float zLayer = 0.7f;
        private bool isAnimated;
        private bool isAnimating = true;
        private bool isDead = false;
        private bool isDying = false;
        private int timesToPlay = 1;
        private Texture2D spriteTexture;
        private Point singleFrameDimensions = new Point(0, 0);
        private Point currentFrame = new Point(0, 0);
        private Point frameCount = new Point(1, 1);
        private Vector2 position = Vector2.Zero;
        private Vector2 velocity = Vector2.Zero;
        private Color tint = Color.White;
        
        #endregion

        #region Properties

        public float ZLayer
        {
            get
            {
                return zLayer;
            }
            set
            {
                zLayer = value;
            }
        }

        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        public Color Tint
        {
            get
            {
                return tint * alpha;
            }
            set
            {
                tint = value;
            }
        }

        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

        public int TimesToPlay
        {
            get
            {
                return timesToPlay;
            }
            set
            {
                if (!isAnimated) 
                    return;

                timesToPlay = value;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        public Texture2D Texture
        {
            set
            {
                this.spriteTexture = value;
            }
        }

        public float Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = value;
            }
        }

        public bool IsDead
        {
            get
            {
                return isDead;
            }
        }

        #endregion

        #region Constructors and Init

        public Sprite()
        {
            
        }

        /// <summary>
        /// Initializes an animated sprite
        /// </summary>
        /// <param name="frameDimensions">Dimensions of 1 frame of the spritesheet in px</param>
        /// <param name="spriteSheetDims">how many frames on the spritesheet</param>
        /// <param name="timesToPlay">how many times the animation should play. -1 for infinite</param>
        public void Init(Vector2 position, Point frameDimensions, Point spriteSheetDims, int timesToPlay)
        {
            this.singleFrameDimensions = frameDimensions;
            this.frameCount = spriteSheetDims;
            this.timesToPlay = timesToPlay;
            this.isAnimated = true;
        }

        public void Init()
        {
            this.isAnimated = false;
        }

        #endregion

        #region Load
        /// <summary>
        /// Some textures are passed in through a property. Use this when you want to load it
        /// in Sprite (for separate, not instanced sprites).
        /// </summary>
        /// <param name="tex">The textures content location.</param>
        public void Load(ContentManager content, string tex)
        {
            spriteTexture = content.Load<Texture2D>(tex);

            this.Load();
        }

        public void Load()
        {
            if (!isAnimated)
            {
                singleFrameDimensions = new Point((int)spriteTexture.Width, (int)spriteTexture.Height);
            }

            this.isAnimating = true;
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            if (velocity != Vector2.Zero) position += velocity;

            if (isDying)
            {
                alpha -= 0.02f;
                if (alpha <= 0) isDead = true;
            }

            //  Code below this is for animated sprites - may as well
            //  break out if it's not needed.
            if (isDead || !isAnimated) return;

            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

            if (elapsed > 1 / 2)
            {
                ++currentFrame.X;

                if (currentFrame.X >= frameCount.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;

                    if (currentFrame.Y >= frameCount.Y)
                    {
                        currentFrame.Y = 0;

                        //  Allows sprites to indefinitely cycle if set to -1
                        if (TimesToPlay > 0)
                        {
                            TimesToPlay -= 1;
                            if (TimesToPlay == 0)
                            {
                                isDead = true;
                            }
                        }
                    }
                    
                }

                elapsed = 0.0f;
            }
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteTexture, position,
                new Rectangle(
                    currentFrame.X * singleFrameDimensions.X,
                    currentFrame.Y * singleFrameDimensions.Y,
                    singleFrameDimensions.X,
                    singleFrameDimensions.Y), Tint, rotation,
                    new Vector2(singleFrameDimensions.X / 2,
                        singleFrameDimensions.Y / 2), scale, SpriteEffects.None, zLayer);
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
                isDying = true;
            else
                isDead = true;
        }
        #endregion

        #region Activate / Deactivate Animation

        public void ToggleAnimating()
        {
            this.SetAnimation(!isAnimating);
        }

        public void ActivateAnimation()
        {
            if (isAnimating)
            {
                return;
            }

            this.SetAnimation(true);
        }

        public void DeactivateAnimation()
        {
            if (!isAnimating)
            {
                return;
            }

            this.SetAnimation(false);
        }

        private void SetAnimation(bool state)
        {
            isAnimating = state;
        }

        #endregion
    }
}
