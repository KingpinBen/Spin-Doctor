using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace GameLibrary.GameLogic.Objects
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

            this.Body.FixtureList[0].UserData = (int)1;
#endif
        }
    }
}
