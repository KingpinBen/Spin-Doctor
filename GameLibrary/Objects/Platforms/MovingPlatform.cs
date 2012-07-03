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

            this._endPosition = this.Position;
        }

        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);

#if EDITOR

#else
            SetupPhysics(world);
#endif
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, this._position,
                new Rectangle(0, 0, (int)this._width, (int)this._height),
                Tint, TextureRotation, new Vector2(this._width / 2, this._height / 2), 1.0f, SpriteEffects.None, _zLayer);

            spriteBatch.Draw(_texture, this._endPosition,
                new Rectangle(0, 0, (int)this._width, (int)this._height),
                Tint * 0.4f, TextureRotation, new Vector2(this._width / 2, this._height / 2), 1.0f, SpriteEffects.None, _zLayer);
        }
#else
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
#endif
        #endregion

        protected override void SetupPhysics(World world)
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
