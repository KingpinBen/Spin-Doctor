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
using Microsoft.Xna.Framework.Graphics;
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

        public override void Init(Vector2 position, string tex)
        {
            base.Init(position, tex);
            this._mass = 1000.0f;
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);

#if EDITOR

#else
            this.Body.BodyType = BodyType.Static;
            this.Body.Friction = 1.0f;
            this.Body.IgnoreCCD = true;
            this.Body.IgnoreGravity = true;
#endif
        }
        #endregion

#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._texture, this._position,
                new Rectangle(0, 0, (int)_width, (int)_height), this._tint, this.TextureRotation, new Vector2(_width / 2, _height / 2), 1.0f, SpriteEffects.None, this._zLayer);
        }
#else

#endif
    }
}
