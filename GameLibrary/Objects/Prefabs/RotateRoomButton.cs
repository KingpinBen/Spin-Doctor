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
        public enum RotateDirection
        {
            CW, CCW
        }

        #region Fields

        private bool _aboutToRotate;
        private float _elapsed;

        [ContentSerializer]
        private RotateDirection enumDirection = RotateDirection.CW;
        [ContentSerializer]
        private float _delayBeforeRotate;
        
        #endregion

        #region Properties

        [ContentSerializerIgnore]
        public string RDirection
        {
            get 
            {
                if (enumDirection == RotateDirection.CW)
                {
                    return "clock-wise";
                }

                return "counter clock-wise";
            }
        }

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
#else

        [ContentSerializerIgnore]
        public bool AboutToRotate
        {
            get
            {
                return _aboutToRotate;
            }
        }
#endif
        #region Message
        [ContentSerializerIgnore]
        public override string Message
        {
            get
            {
                return _message;
            }
        }
        #endregion

        #endregion

        #region Constructor

        public RotateRoomButton()
            : base()
        {
        }

        public override void Init(Vector2 position, string tex)
        {
            this.Position = position;
            this._textureAsset = tex;
            this.Tint = Color.White;
            this._message = "AUTO CHOSEN";

            //  The width and height values are redone in Load due to it using the
            //  texture to calculate it's size. We do however need a size so the editor
            //  can manipulate the object (such as translating).
            this.Width = 50;
            this.Height = 50;
        }
        #endregion

        #region Load
        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(_textureAsset);
            this._origin = new Vector2(_width / 2, _height / 2);
            this._message = " to rotate " + RDirection;
            base.Load(content, world);

#if EDITOR
            this._width = this._texture.Width / 2;
            this._height = this._texture.Height / 2;
#else

#endif
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
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
                    if (enumDirection == RotateDirection.CW)
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
        }
        #endregion

        #region Draw

#if EDITOR
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this._texture, this._position, null, this._tint, 0.0f, this._origin, 1.0f, SpriteEffects.None, this._zLayer);
        }

#else
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this.Texture, ConvertUnits.ToDisplayUnits(this.Body.Position), null, this.Tint, 0.0f, this.Origin, 1.0f, SpriteEffects.None, this.zLayer);
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
