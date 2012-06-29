//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - PhysicsObjects
//--    
//--    Description
//--    ===============
//--    Creates character bodies and wheels
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Allows color to be passed in for shading.
//--    BenG - Can pass in a shapetype while TexToPoly collision isnt in.
//--    BenG - Changed how all PhysicsObjects are loaded. Makes loading more
//--           similar between different types.
//--    BenG - Added an orientation. Not sure how I'm going to use it yet though.
//--    BenG - Rearraged and renamed all variables. Added a define for easier editor changes.
//--    BenG - Fixed all collisions on this and all deriving objects.
//--    BenG - Added name for use with triggers and targetting.
//--    BenG - Removed the events from the physicsobject class. They can be added, but it's 
//--           only really used by a few classes.
//--    
//--    TBD
//--    ==============
//--    
//--
//--    
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using GameLibrary.Screens;
using GameLibrary.Assists;
using Microsoft.Xna.Framework.Content;
using GameLibrary.Managers;
using GameLibrary.Drawing;
using FarseerPhysics.Dynamics.Contacts;
using System.ComponentModel;
#endregion

namespace GameLibrary.Objects
{
    public class PhysicsObject : StaticObject
    {
        //[ContentSerializer]
        //protected string _name;

        #region Construct and Initialize
        public PhysicsObject() : base()
        {

        }

        public override void Init(Vector2 position, string tex)
        {
            base.Init(position, tex);
            this._mass = 20.0f;
        }
        #endregion

        #region Load
        /// <summary>
        /// Load object game content
        /// </summary>
        /// <param name="tex">Texture location</param>
        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);
#if EDITOR

#else
            this.SetupPhysics(world);
#endif
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
#if EDITOR

#else
            if (!Camera.LevelRotating)
            {
                return;
            }

            if (!this.Body.Awake)
            {
                this.Body.Awake = true;
            }
#endif
        }

        #region Draw
        /// <summary>
        /// Draw the object obviously.
        /// </summary>
        /// <param name="spriteBatch"></param>
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            //  Doesn't strech image. Tiles instead
            spriteBatch.Draw(_texture, this._position,
                new Rectangle(0, 0, (int)this._width, (int)this._height),
                Tint, TextureRotation, new Vector2(this._texture.Width / 2, this._texture.Height / 2), 1f, SpriteEffects.None, _zLayer);

            
        }
#else
        public override void Draw(SpriteBatch spriteBatch)
        {
            //  Doesn't strech image. Tiles instead
            spriteBatch.Draw(this._texture, ConvertUnits.ToDisplayUnits(this.Body.Position),
                new Rectangle(0, 0, (int)this._width, (int)this._height),
                Tint, this.TextureRotation, new Vector2(this._width / 2, this._height / 2), 1f, SpriteEffects.None, _zLayer);
        }
#endif
        #endregion

        #region Private Methods

        protected override void SetupPhysics(World world)
        {
#if EDITOR

#else
            this.Body.BodyType = BodyType.Dynamic;
#endif
        }



        #endregion
    }
}
