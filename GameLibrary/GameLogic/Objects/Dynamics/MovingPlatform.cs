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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;
using GameLibrary.Helpers;
using GameLibrary.Audio;
#endregion

namespace GameLibrary.GameLogic.Objects
{
    public class MovingPlatform : DynamicObject
    {
        #region Properties
#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public override Vector2 EndPosition
        {
            get
            {
                return _endPosition;
            }
            set
            {
                if (_movementDirection == Direction.Horizontal)
                {
                    _endPosition = new Vector2(value.X, _position.Y);
                }
                else
                {
                    _endPosition = new Vector2(_position.X, value.Y);
                }
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public override Direction MovementDirection
        {
            get
            {
                return base.MovementDirection;
            }
            set
            {
                if (value != MovementDirection)
                {
                    if (value == Direction.Horizontal)
                    {
                        float dist = Math.Abs(_endPosition.Y - _position.Y);

                        _endPosition = new Vector2(_position.Y - dist, _position.Y);
                    }
                    else
                    {
                        float dist = Math.Abs(_endPosition.Y - _position.Y);

                        _endPosition = new Vector2(_position.X, _position.X - dist);
                    }
                }
                base.MovementDirection = value;
            }
        }
#else

#endif
        #endregion

        #region Constructor and Load

        public MovingPlatform() : base() { }

        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);
#if !EDITOR
            this._soundEffectAsset = "Cogs_Rotating_On";
            this._soundEffect = AudioManager.Instance.PlayCue(_soundEffectAsset, _isMoving);
#endif
        }


        #endregion

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, this._position,
                new Rectangle(0, 0, (int)this._width, (int)this._height),
                this._tint, this._rotation, new Vector2(this._width, this._height) * 0.5f, 1.0f, SpriteEffects.None, _zLayer);

            spriteBatch.Draw(_texture, this._endPosition,
                new Rectangle(0, 0, (int)this._width, (int)this._height),
                this._tint * 0.4f, this._rotation, new Vector2(this._width, this._height) * 0.5f, 1.0f, SpriteEffects.None, _zLayer);
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            sb.Draw(_texture, ConvertUnits.ToDisplayUnits(Body.Position), new Rectangle(0, 0, (int)_width, (int)_height), _tint,
                Body.Rotation, new Vector2(_width, _height) * 0.5f, 1.0f, SpriteEffects.None, _zLayer);
        }

#endif
        #endregion

        #region Private Methods

        protected override void SetupPhysics(World world)
        {
#if EDITOR
#else
            this.Body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(this._width), ConvertUnits.ToSimUnits(this._height), ConvertUnits.ToSimUnits(_mass));
            this.Body.Rotation = _rotation;
            this.Body.BodyType = BodyType.Dynamic;
            this.Body.Position = ConvertUnits.ToSimUnits(Position);
            this.Body.Friction = 3.0f;
            this.SetUpJoint(world);

            this.Body.CollisionCategories = Category.Cat20;
            this.Body.CollidesWith = Category.All & ~Category.Cat20;
            this.Body.UserData = _materialType;
#endif
        }

        #endregion
    }
}
