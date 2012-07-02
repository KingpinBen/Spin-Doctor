﻿//--------------------------------------------------------------------------------
//--    
//--    Spin Doctor - Saw
//--
//--    
//--    Description
//--    ===============
//--    
//--
//--    
//--    Revision List
//--    ===============
//--    BenG - Initial - replaced Sawblade
//--    BenG - Changed decal zlayer to fixed back.
//--    
//--    TBD
//--    ==============
//--    
//--    
//--    
//-------------------------------------------------------------------------------

//#define Development

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using GameLibrary.Assists;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;
using System.ComponentModel;

namespace GameLibrary.Objects
{
    public class Saw : DynamicObject
    {
        #region Fields
        [ContentSerializer]
        private string _bloodiedTextureAsset;
        [ContentSerializer]
        float _scale;

#if EDITOR

#else
        private Texture2D _bloodiedTexture;
        private Texture2D _wallDecalEnd;
        private Texture2D _wallDecalMiddle;
        private Rectangle _decalRectangle;
        private List<Fixture> TouchingFixtures = new List<Fixture>();
        //private Vector2 
        private bool _touched = false;
        private float _decalRotation;        
#endif
        #endregion

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
                if (MovementDirection == Direction.Horizontal)
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
        public float Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
            }
        }
#else
        [ContentSerializerIgnore]
        public Texture2D TextureToUse
        {
            get
            {
                if (_touched)
                {
                    return _bloodiedTexture;
                }
                else
                {
                    return Texture;
                }
            }
        }

        [ContentSerializerIgnore]
        public string TouchedSawBladeAsset
        {
            get
            {
                return _bloodiedTextureAsset;
            }
        }

        [ContentSerializerIgnore]
        public bool BeenTouched
        {
            get
            {
                return _touched;
            }
        }
#endif

        #endregion

        #region Constructor
        public Saw() : base() { }

        public void Init(Vector2 position, string tex, string texblood)
        {
            base.Init(position, tex);

            this._bloodiedTextureAsset = texblood;
            this._scale = 1.0f;
        }
        #endregion

        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);
#if EDITOR

#else
            this._bloodiedTexture = content.Load<Texture2D>(_bloodiedTextureAsset);
            this._wallDecalEnd = content.Load<Texture2D>("Assets/Images/Textures/Saw/i_sawDecalEndPiece");
            this._wallDecalMiddle = content.Load<Texture2D>("Assets/Images/Textures/Saw/i_sawDecalMiddlePiece");

            #region Sort out the wall indents
            if (_movementDirection == Direction.Horizontal)
            {
                _decalRotation = -MathHelper.PiOver2;

                if (EndPosition.X > Position.X)
                {
                    _decalRectangle = new Rectangle(
                        -(int)Math.Abs(Position.X),
                        -(int)Math.Abs(Position.Y + this._wallDecalEnd.Height / 2),
                        (int)(this._wallDecalEnd.Height),
                        (int)Math.Abs(EndPosition.X - Position.X));
                }
                else
                {
                    _decalRectangle = new Rectangle(
                        -(int)Math.Abs(EndPosition.X),
                        -(int)Math.Abs(EndPosition.Y + this._wallDecalEnd.Height / 2),
                        (int)this._wallDecalEnd.Height,
                        (int)Math.Abs(Position.X - EndPosition.X));
                }
            }
            else
            {
                if (EndPosition.Y > Position.Y)
                {
                    _decalRectangle = new Rectangle(
                        (int)Position.X - this._wallDecalEnd.Width / 2,
                        (int)Position.Y,
                        (int)(this._wallDecalEnd.Width),
                        (int)(EndPosition.Y - Position.Y));
                }
                else
                {
                    _decalRectangle = new Rectangle(
                        (int)EndPosition.X - this._wallDecalEnd.Width / 2,
                        (int)EndPosition.Y,
                        (int)(this._wallDecalEnd.Width),
                        (int)(Position.Y - EndPosition.Y));
                }

            }
            #endregion

            this.SetupPhysics(world);
#endif
        }

        public override void Update(GameTime gameTime)
        {
#if EDITOR

#else
            if (this._prismaticJoint.MotorSpeed != 0)
            {
                _rotation += 0.3f;

                if (_rotation >= 4)
                {
                    _rotation -= 4;
                }
            }
            base.Update(gameTime);
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._texture, this._position, null, this._tint, 0.0f, _origin, _scale, SpriteEffects.None, this._zLayer);
        }
#else
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureToUse, ConvertUnits.ToDisplayUnits(this.Body.Position), null, Color.White, 
                TextureRotation, Origin, _scale, SpriteEffects.None, zLayer); 

            
            sb.Draw(_wallDecalEnd, this.Position, null, Color.White, _decalRotation,
                new Vector2(this._wallDecalEnd.Width * 0.5f, this._wallDecalEnd.Height * 0.5f), 1.0f, SpriteEffects.None, 0.87f);
            sb.Draw(_wallDecalEnd, this.EndPosition, null, Color.White, (float)Math.PI + _decalRotation,
                new Vector2(this._wallDecalEnd.Width * 0.5f, this._wallDecalEnd.Height * 0.5f), 1.0f, SpriteEffects.None, 0.87f);
            sb.Draw(_wallDecalMiddle, _decalRectangle, null, Color.White, _decalRotation,
                Vector2.Zero, SpriteEffects.None, 0.87f);

#if Development
            sb.DrawString(Fonts.DebugFont, "ToStart: " + this.MovingToStart + ". Speed: " + this.PrismaticJoint.MotorSpeed + ". IsMoving: " + this._isMoving, 
                ConvertUnits.ToDisplayUnits(this.Body.Position) + new Vector2(400, -60), Color.Red);

            sb.DrawString(Fonts.DebugFont, "UpL: " + this.PrismaticJoint.UpperLimit + ". LoL: " + this.PrismaticJoint.LowerLimit,
                ConvertUnits.ToDisplayUnits(this.Body.Position) + new Vector2(400, -45), Color.Red);

            sb.DrawString(Fonts.DebugFont, "Translation: " + this.PrismaticJoint.JointTranslation.ToString(), 
                ConvertUnits.ToDisplayUnits(this.Body.Position) + new Vector2(400, -30), Color.Red);

            sb.DrawString(Fonts.DebugFont, (this.PrismaticJoint.JointTranslation >= this.PrismaticJoint.UpperLimit) + (!this.MovingToStart).ToString(), this.Position - new Vector2(0, 500), Color.Red);
            sb.DrawString(Fonts.DebugFont, (this.PrismaticJoint.JointTranslation <= 0) + this.MovingToStart.ToString(), this.Position - new Vector2(0, 485), Color.Red);
            sb.DrawString(Fonts.DebugFont, this._elapsedTimer.ToString(), this.Position - new Vector2(0, 460), Color.Red);
#endif
        }
#endif
        #endregion

        #region Private Methods

        protected override void SetupPhysics(World world)
        {
#if EDITOR
#else
            this.Body = BodyFactory.CreateCircle(world,
                ConvertUnits.ToSimUnits((this._texture.Width / 2) * _scale),
                ConvertUnits.ToSimUnits(_mass));

            this.Body.BodyType = BodyType.Dynamic;
            this.Body.Position = ConvertUnits.ToSimUnits(this.Position);
            this.Body.Friction = 1.0f;
            this.Body.Restitution = 0.5f;

            base.SetUpJoint(world);

            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
#endif
        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
#if EDITOR
            return true;
#else
            if (!TouchingFixtures.Contains(fixtureB) && 
                (fixtureB == Player.Instance.Body.FixtureList[0] ||
                fixtureB == Player.Instance.WheelBody.FixtureList[0]))
            {
                TouchingFixtures.Add(fixtureB);
                Player.Instance.Kill();
                _touched = true;
            }

            return true;
#endif
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
#if EDITOR

#else
            TouchingFixtures.Remove(fixtureB);
#endif
        }

        #endregion
    }
}
