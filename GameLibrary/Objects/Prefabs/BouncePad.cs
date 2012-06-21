//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - Bouncepad
//--
//--    
//--    Description
//--    ===============
//--    
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    
//--    TBD
//--    ==============
//--    
//--    
//--    
//-------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using GameLibrary.Assists;

namespace GameLibrary.Objects
{
    public class BouncePad : OneSidedPlatform
    {
        #region Fields
        [ContentSerializer]
        private float _restitution;

        private float lastTouched;

        #endregion

        #region Properties
        [ContentSerializerIgnore]
        public float Restitution
        {
            get
            {
                return _restitution;
            }
#if EDITOR
            set
#else
            protected set
#endif
            {
                _restitution = value;
            }

        }
        #endregion

        #region Constructor
        public BouncePad()
            : base()
        {
        }
        #endregion

        #region Load
        public override void  Load(ContentManager content, World world)
        {
 	        base.Load(content, world);

#if EDITOR

#else
            this.Body.Restitution = _restitution;
#endif
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            if (lastTouched < 0.4)
            {
                lastTouched += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

                if (this.Body.Restitution == _restitution)
                {
                    this.Body.Restitution = 0.0f;
                }
            }
            else
            {
                if (this.Body.Restitution == _restitution)
                {
                    this.Body.Restitution = _restitution;
                }
            }
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Fonts.DebugFont, "Res: " + this.Body.Restitution + ". Last: " + lastTouched.ToString(), this.Position + new Vector2(0,-100), Color.Red);

            base.Draw(spriteBatch);
        }
        #endregion

        #region Collisions
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (lastTouched > 0.4f)
                lastTouched = 0.0f;

            return base.Body_OnCollision(fixtureA, fixtureB, contact);
        }
        #endregion
    }
}
