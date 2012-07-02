using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace GameLibrary.Objects
{
    public class CushionedPlatform : StaticObject
    {
        public CushionedPlatform()
            : base()
        { }

        protected override void SetupPhysics(World world)
        {
#if EDITOR
#else
            base.SetupPhysics(world);

            int cushioned = 1;
            this.Body.UserData = cushioned; 
            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
#endif
        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
#if EDITOR
            return true;
#else
            return base.Body_OnCollision(fixtureA, fixtureB, contact);
#endif
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
#if EDITOR
#else
            base.Body_OnSeparation(fixtureA, fixtureB);
#endif
        }
    }
}
