using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using GameLibrary.Audio;
using GameLibrary.GameLogic.Characters;

namespace GameLibrary.GameLogic.Objects
{
    public class CushionedPlatform : StaticObject
    {
        public CushionedPlatform()
            : base()
        {
            this._materialType = Objects.MaterialType.Cushion;
        }

        protected override void SetupPhysics(World world)
        {
#if !EDITOR
            this._materialType = Objects.MaterialType.Cushion;
            base.SetupPhysics(world);
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
#endif
        }

#if !EDITOR
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            //  Get the current amount of touching fixtures to 
            //  compare later.
            int fixCount = _touchingFixtures.Count;

            if (!_touchingFixtures.Contains(fixtureB) &&
                Player.Instance.CheckWheelFixture(fixtureB))
            {
                _touchingFixtures.Add(fixtureB);
            }

            //  If a fixture has been added or removed,
            //  make the bouncing noise.
            if (_touchingFixtures.Count != fixCount)
            {
                AudioManager.Instance.PlayCue("Bounce", true);
            }

            return true;
        }
#endif
    }
}
