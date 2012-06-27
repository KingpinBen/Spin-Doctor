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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle((int)(_position.X - (this.Width * 0.5f)), (int)(_position.Y - (this.Height * 0.5f)), (int)Width, (int)Height),
                new Rectangle(0, 0, (int)this._width, (int)this._height),
                Tint, TextureRotation, Vector2.Zero, SpriteEffects.None, zLayer);
        }
    }
}
