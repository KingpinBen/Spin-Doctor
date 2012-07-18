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
        bool _locked;
        bool _inRange;
        Contact _contactPoint;
        #endregion

        public PullableObject()
            : base()
        {
        }

        #region Load
        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);

#if EDITOR

#else
            CircleShape fixture = new CircleShape(ConvertUnits.ToSimUnits(Width / 1.5), 0.0f);
            this.Body.CreateFixture(fixture);
            this.Body.FixtureList[1].IsSensor = true;

            this.Body.FixtureList[1].Body.OnCollision += Body_OnCollision;
            this.Body.FixtureList[1].Body.OnSeparation += Body_OnSeparation;
#endif
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
#if EDITOR
#else
            base.Update(gameTime);

            if (!_inRange || _locked)
            {
                return;
            }
#endif
        }
        #endregion

        #region Draw
#if EDITOR
#else
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(FontManager.Instance.GetFont(Graphics.FontList.Debug).Font, "InRange: " + _inRange, ConvertUnits.ToDisplayUnits(this.Body.Position) + new Vector2(0, -50), Color.Red);
        }
#endif
        #endregion

        #region Collisions
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
        #endregion
    }
}
