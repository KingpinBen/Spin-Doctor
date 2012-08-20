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
using GameLibrary.Audio;

namespace GameLibrary.GameLogic.Objects
{
    public class BouncePad : OneSidedPlatform
    {
        #region Fields
        [ContentSerializer(Optional = true)]
        private float _restitution = 1.0f;

#if EDITOR

#else
        private float _elapsed = 00f;
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
                _restitution = MathHelper.Clamp(value, 0.0f, 2.5f);
            }
        }
#endif
        #endregion

        #region Constructor and Load


        public BouncePad() : base() { }



        public override void  Load(ContentManager content, World world)
        {
            //  It's essentially a one-sided platform, so baseLoad
 	        base.Load(content, world);

#if !EDITOR
            //  Apply the bouncepad specific properties.

            this.Body.Restitution = _restitution;
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
            this.Body.UserData = MaterialType.Cushion;
#endif
        }



        #endregion

        #region Update

        public override void Update(float delta)
        {
#if !EDITOR
            //  Elapsed is set to cooldown timer in OnCollision.

            //  Check if it's recently been bounced upon.
            if (_elapsed > 0)
            {
                //  Force the restitution to 0 so it's a normal
                //  surface
                this.Body.Restitution = 0.0f;

                //  Decrement the counter.
                _elapsed -= delta;
            }
            //  If it's below 0, the timer has ran it's course and 
            //  can reset the pad
            else if (_elapsed < 0)
            {
                //  Check the player has totally left the pad.
                if (_touchingFixtures.Count == 0)
                {
                    //  Reset to zero as to not do anything now in update.
                    this._elapsed = 0.0f;

                    //  and reapply the restitution.
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
            //  Due to the presolve, we only need to check if FixB is the wheel.
            if (fixtureB != Player.Instance.WheelFixture)
            {
                return false;
            }

            //  We want to check if the contact is enabled so the cooldown
            //  only triggers on a bounce, not touch.
            if (!_touchingFixtures.Contains(fixtureB) && contact.Enabled)
            {
                if (_elapsed == 0.0f)
                {
                    AudioManager.Instance.PlayCue("Bounce", true);
                    this._elapsed = _bounceCooldown;
                }

                this._touchingFixtures.Add(fixtureB);
            }

            //  Complete the collision.
            return true;
        }



        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            //  Remove the fixture from the list so we can
            //  reset the timer for the next bounce on 0 count.
            if (_touchingFixtures.Contains(fixtureB))
            {
                _touchingFixtures.Remove(fixtureB);
            }
        }



#endif
        #endregion
    }
}
