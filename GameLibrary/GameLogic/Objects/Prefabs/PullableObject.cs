//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - PullableObject
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
//--    Complete
//--    
//--    
//-------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary.Graphics;
using GameLibrary.Helpers;

namespace GameLibrary.GameLogic.Objects
{
    public class PullableObject : PhysicsObject
    {
        #region Fields

        //
        //  The joint should be stored in the player as it's the only constant
        //  between pullableobjects. Jumping should remove it for instance.
        //  

        [ContentSerializer]
        bool _locked = false;
        
#if EDITOR

#else
        Contact _contactPoint;
        bool _inRange = false;
#endif
        #endregion

        public PullableObject()
            : base()
        {
        }

        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);

#if EDITOR

#else
            CircleShape fixture = new CircleShape(ConvertUnits.ToSimUnits(Width * 0.66667f), 0.0f);
            this.Body.CreateFixture(fixture);
            this.Body.FixtureList[1].IsSensor = true;

            this.Body.FixtureList[1].Body.OnCollision += Body_OnCollision;
            this.Body.FixtureList[1].Body.OnSeparation += Body_OnSeparation;
#endif
        }

        public override void Update(float delta)
        {
#if EDITOR
#else
            base.Update(delta);

            if (!_inRange || _locked)
            {
                return;
            }
#endif
        }

        #region Draw
#if EDITOR
#else
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            base.Draw(spriteBatch, graphics);
        }
#endif
        #endregion

        #region Collisions
#if !EDITOR
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (_locked)
            {
                return true;
            }

            if (fixtureB == Player.Instance.Body.FixtureList[0])
            {
                this._inRange = true;
                this._contactPoint = contact;
            }

            return true;
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB == Player.Instance.Body.FixtureList[0])
            {
                this._inRange = false;
                this._contactPoint = null;
            }
        }
#endif
        #endregion
    }
}
