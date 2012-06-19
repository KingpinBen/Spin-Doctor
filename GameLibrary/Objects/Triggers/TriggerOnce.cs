//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - TriggerOnce
//--    
//--    Description
//--    ===============
//--    Functions are a one-time use trigger.
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - Changed so it uses OnSeparation too otherwise it can't check
//--           for triggers outside of function.
//--    BenG - Fixed collisions and made Update back to inherit.
//--    
//--    
//--    TBD
//--    ==============
//--    Make it work properly with other objects
//--    
//--    
//-------------------------------------------------------------------------------

//#define Development

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using GameLibrary.Assists;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics.Contacts;

namespace GameLibrary.Objects.Triggers
{
    public class TriggerOnce : Trigger
    {
        #region Fields and Variables

        private bool _firedOnce;

#if Development
        private string fired = "NotFired";
#endif

        #endregion

        #region Constructor
        /// <summary>
        /// A trigger that can only be fired once per load.
        /// </summary>
        public TriggerOnce()
            : base()
        {
            
        }
        

        public override void Init(Vector2 position, float tWidth, float tHeight)
        {
            base.Init(position, tWidth, tHeight);

            _firedOnce = false;
            this.ShowHelp = false;
        }
        #endregion

        #region Collisions
        /// <summary>
        /// This took a while to get working. Not sure how it'll handle with other
        /// props. The update method had problems in that it was accessing OnCollision
        /// in update even though it was in an 'if' it shouldnt have been able to get into.
        /// 
        /// Instead it now fires and no longer reacts to collisions. Hopefully the Triggered
        /// will be noticed else we can just use the CollidesWith category in OnSeparation.
        /// </summary>
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (_firedOnce) return true;

            base.Body_OnCollision(fixtureA, fixtureB, contact);
            this._firedOnce = true;

            return true;
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            base.Body_OnSeparation(fixtureA, fixtureB);
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            #region Development
#if Development
            sb.DrawString(Fonts.DebugFont, "Fired: " + _firedOnce.ToString(), ConvertUnits.ToDisplayUnits(Body.Position) + new Vector2(0, Height / 2 + 10), Color.Green, 0f,
                new Vector2(Fonts.DebugFont.MeasureString(fired).X / 2, Fonts.DebugFont.MeasureString("A").Y / 2),
                2f, SpriteEffects.None, zLayer); 
#endif
            #endregion
        }
        #endregion
    }
}
