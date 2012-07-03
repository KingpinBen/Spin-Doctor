//--------------------------------------------------------------------------
//--    
//--    Spin Doctor - RotateRoomButton
//--    
//--    Description
//--    ===============
//--    An interactable object ingame to initiate a level rotate.
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial
//--    BenG - More common Init function added. Better for editor and initializes
//--           some more data, and allows custom textures.
//--    BenG - Updated to keep in line with Trigger changes.
//--    BenG - Added delay timer before world rotates.
//--    BenG - Fixed some silly mistakes in update
//--    
//--    TBD
//--    ==============
//--    
//--
//--    
//--    
//--------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision.Shapes;
using GameLibrary.Objects.Triggers;
using GameLibrary.Drawing;
using FarseerPhysics.Dynamics.Contacts;
using GameLibrary.Assists;

namespace GameLibrary.Objects
{
    public class RotateRoomButton : Trigger
    {
        #region Fields

#if EDITOR

#else
        private bool _aboutToRotate;
        private float _elapsed;
#endif

        [ContentSerializer]
        private RotateDirection enumDirection = RotateDirection.Clockwise;
        [ContentSerializer]
        private float _delayBeforeRotate;
        
        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore]
        public RotateDirection RotationDirection
        {
            get
            {
                return enumDirection;
            }

            set
            {
                enumDirection = value;
            }
        }
        [ContentSerializerIgnore]
        public override string Message
        {
            get
            {
                return _message;
            }
            set
            {
            }
        }
#else
        [ContentSerializerIgnore]
        public override string Message
        {
            get
            {
                return _message;
            }
            protected set { }
        }
        [ContentSerializerIgnore]
        public bool AboutToRotate
        {
            get
            {
                return _aboutToRotate;
            }
        }
        [ContentSerializerIgnore]
        public string RDirection
        {
            get 
            {
                if (enumDirection == RotateDirection.Clockwise)
                {
                    return "clock-wise";
                }

                return "counter clock-wise";
            }
        }
#endif

        #endregion

        #region Constructor

        public RotateRoomButton() : base() { }

        public override void Init(Vector2 position, string tex)
        {
            this.Position = position;
            this._textureAsset = tex;
            this._message = "AUTO CHOSEN";

            base.Init(position, 25, 25);
        }
        #endregion

        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(_textureAsset);
            this._origin = new Vector2(_width / 2, _height / 2);
            //base.Load(content, world);

#if EDITOR
            if (_width == 0 || _height == 0)
            {
                this._width = this._texture.Width / 2;
                this._height = this._texture.Height / 2;
            }
#else
            this._message = " to rotate " + RDirection;
            SetupTrigger(world);
#endif
        }

        public override void Update(GameTime gameTime)
        {
#if EDITOR

#else
            if (!Triggered && !_aboutToRotate)
            {
                return;
            }

            if (!_aboutToRotate)
            {
                if (Input.Interact())
                {
                    _aboutToRotate = true;
                }
                else
                {
                    return;
                }
            }
            else
            {
                _elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

                if (_elapsed >= _delayBeforeRotate)
                {
                    if (enumDirection == RotateDirection.Clockwise)
                    {
                        Camera.ForceRotateRight();
                    }
                    else
                    {
                        Camera.ForceRotateLeft();
                    }

                    _elapsed = 0.0f;
                    _aboutToRotate = false;
                }
            }
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this._texture, this._position, null, this._tint, this.TextureRotation, this._origin, 1.0f, SpriteEffects.None, this._zLayer);
        }

#else
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this.Texture, ConvertUnits.ToDisplayUnits(this.Body.Position), null, this.Tint, this.TextureRotation, this.Origin, 1.0f, SpriteEffects.None, this.zLayer);
        }
#endif
        #endregion

        #region Private Methods

        #region Collisions
        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            base.Body_OnSeparation(fixtureA, fixtureB);
        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return base.Body_OnCollision(fixtureA, fixtureB, contact);
        }
        #endregion

        #endregion
    }
}
