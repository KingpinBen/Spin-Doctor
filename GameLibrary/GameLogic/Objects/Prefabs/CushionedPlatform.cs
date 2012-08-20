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
#endif
        }

#if !EDITOR
        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            int fixCount = _touchingFixtures.Count;

            if (!_touchingFixtures.Contains(fixtureB) && 
                fixtureB == Player.Instance.WheelFixture)
            {
                _touchingFixtures.Add(fixtureB);
            }

            if (_touchingFixtures.Count != fixCount)
            {
                AudioManager.Instance.PlayCue("Bounce", true);
            }

            return true;
        }
#endif
    }
}
