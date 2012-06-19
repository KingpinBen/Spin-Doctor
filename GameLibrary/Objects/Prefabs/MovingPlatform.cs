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

        public override void Init(Vector2 position, float width, float height, string tex)
        {
            base.Init(position, width, height, tex);

            this.MovementDirection = Direction.Horizontal;
            this.EndPosition = this.Position;
            this.StartsMoving = false;
            this.MotorSpeed = 3.0f;
        }

        public override void Load(ContentManager content, World world)
        {
            this.Texture = content.Load<Texture2D>(_textureAsset);
            this.Origin = new Vector2(this.Texture.Width / 2, this.Texture.Height / 2);

            SetUpPhysics(world);
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
            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(this.Width), ConvertUnits.ToSimUnits(this.Height), ConvertUnits.ToSimUnits(_mass));
            this.Body.BodyType = BodyType.Dynamic;
            this.Body.Position = ConvertUnits.ToSimUnits(Position);

            this.SetUpJoint(world);
        }
    }
}
