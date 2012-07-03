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
        private const float _bounceCooldown = 2f;

        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore]
        public float Restitution
        {
            get
            {
                return _restitution;
            }
            set
            {
                _restitution = MathHelper.Clamp(value, 0.0f, 1.0f);
            }
        }
#else
        [ContentSerializerIgnore]
        public float Restitution
        {
            get
            {
                return _restitution;
            }
        }
#endif
        #endregion

        #region Constructor
        public BouncePad()
            : base()
        {
        }

        public override void Init(Vector2 position)
        {
            base.Init(position);

            this._restitution = 1.0f;
        }
        #endregion

        #region Load
        public override void  Load(ContentManager content, World world)
        {
 	        base.Load(content, world);

#if EDITOR

#else
            this.lastTouched = -1.0f;
            this.Body.Restitution = _restitution;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
#endif
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
#if EDITOR

#else
            /*  This sort of gets the right effect but depending on how the 
             * designers want it to work, it'll need redoing.
             * 
             * If they want it to only allow one bounce, then I'll need 'Reset' 
             * it by waiting for a separation of the player from the trigger.
             * 
             * I'll have to ask tomorrow
             * */

            if (lastTouched >= 0)
            {
                lastTouched += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

                if (this.Body.Restitution == _restitution)
                {
                    this.Body.Restitution = 0.0f;
                }


                if (lastTouched >= _bounceCooldown)
                {
                    lastTouched = -1f;
                    this.Body.Restitution = _restitution;
                }
            }
            
#endif
        }
        #endregion

#if !EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Fonts.DebugFont, "Rest: " + this.Body.Restitution + ". LastT: " + lastTouched, this.Position - new Vector2(0, 30), Color.Red);
            base.Draw(spriteBatch);
        }
#endif

        #region Collisions
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
#if EDITOR
#else
            if (fixtureB != Player.Instance.WheelBody.FixtureList[0])
            {
                return false;
            }

            if (lastTouched < 0.0f)
            {
                lastTouched = 0.0f;
            }
#endif
            return true;
        }
        #endregion
    }
}
