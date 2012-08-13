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
using GameLibrary.Graphics;
using GameLibrary.GameLogic.Characters;
using FarseerPhysics.Dynamics.Contacts;

namespace GameLibrary.GameLogic.Objects
{
    public class BouncePad : OneSidedPlatform
    {
        #region Fields
        [ContentSerializer(Optional = true)]
        private float _restitution = 1.0f;

#if EDITOR

#else
        private float lastTouched = -1.0f;
        private const float _bounceCooldown = 2f;
#endif

        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore]
        public float Bounciness
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

        #region Constructor and Load
        public BouncePad() : base() { }

        public override void  Load(ContentManager content, World world)
        {
 	        base.Load(content, world);

#if !EDITOR
            this.Body.Restitution = _restitution;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
#endif
        }
        #endregion

        #region Update
        public override void Update(float delta)
        {
#if !EDITOR

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
                lastTouched += delta;

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

        #region Collisions

#if !EDITOR
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {

            if (fixtureB != Player.Instance.WheelFixture)
            {
                return false;
            }

            if (lastTouched < 0.0f)
            {
                lastTouched = 0.0f;
            }

            return true;
        }
#endif

        #endregion
    }
}
