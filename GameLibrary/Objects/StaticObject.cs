//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - StaticObject
//--    
//--    
//--    
//--    Description
//--    ===============
//--    Allows static objects that still have a collision model.
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial 
//--    BenG - Added a second constructor for circles.
//--    BenG - Changed restitution to 0 to stop player bouncing off the walls/floor.
//--           
//--    
//--    
//--    TBD
//--    ==============
//--    The rest of it, durrh
//--    
//--    
//--------------------------------------------------------------------------

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics.Contacts;
#endregion

namespace GameLibrary.Objects
{
    public class StaticObject : PhysicsObject
    {
        #region Constructors
        public StaticObject()
            : base()
        {
        }


        public virtual void Init(Vector2 position, float radius, string tex)
        {
            base.Init(position, radius, tex);
            this._mass = 1000.0f;
        }

        public virtual void Init(Vector2 position, float width, float height, string tex)
        {
            base.Init(position, width, height, tex);
            this._mass = 1000.0f;
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);

#if !EDITOR
            this.Body.BodyType = BodyType.Static;
            this.Body.Friction = 1.0f;
            this.Body.IgnoreCCD = true;
            this.Body.IgnoreGravity = true;
#endif
        }
        #endregion
    }
}
