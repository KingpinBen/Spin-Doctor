//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - MovingPlatform
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
//--    BenG - Moved to DynamicObject    
//--
//--    TBD
//--    ==============
//--    
//--    
//--    
//-------------------------------------------------------------------------------

#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using GameLibrary.Assists;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace GameLibrary.Objects
{
    public class MovingPlatform : DynamicObject
    {
        public MovingPlatform()
            : base()
        {

        }

        public override void Init(Vector2 position, string tex)
        {
            base.Init(position, tex);

            this._movementDirection = Direction.Horizontal;
            this._endPosition = this.Position;
            this._startsMoving = false;
            this._motorSpeed = 3.0f;
        }

        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(_textureAsset);
            this.Origin = new Vector2(this._texture.Width / 2, this._texture.Height / 2);

#if EDITOR
#else
            SetUpPhysics(world);
#endif
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        protected override void SetUpPhysics(World world)
        {
#if EDITOR
#else
            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(this.Width), ConvertUnits.ToSimUnits(this.Height), ConvertUnits.ToSimUnits(_mass));
            this.Body.BodyType = BodyType.Dynamic;
            this.Body.Position = ConvertUnits.ToSimUnits(Position);

            this.SetUpJoint(world);
#endif
        }
    }
}
