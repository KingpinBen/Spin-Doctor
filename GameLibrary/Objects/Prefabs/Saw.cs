//--------------------------------------------------------------------------------
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
//--    
//--    TBD
//--    ==============
//--    
//--    
//--    
//-------------------------------------------------------------------------------

#define Development

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

namespace GameLibrary.Objects
{
    public class Saw : DynamicObject
    {
        #region Fields
        [ContentSerializer]
        private string _bloodiedTextureAsset;
        [ContentSerializerIgnore]
        private Texture2D _bloodiedTexture;
        [ContentSerializerIgnore]
        private bool _touched = false;
        [ContentSerializerIgnore]
        private List<Fixture> TouchingFixtures = new List<Fixture>();
        private Texture2D wallDecalEnd;
        private Texture2D wallDecalMiddle;
        private float _decalRotation;
        private Rectangle _decalRectangle;
        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore]
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

        [ContentSerializerIgnore]
        public string TouchedSawBladeAsset
        {
            get
            {
                return _bloodiedTextureAsset;
            }
            set
            {
                _bloodiedTextureAsset = value;
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
            this._motorSpeed = 0;
        }
        #endregion

        public override void Load(ContentManager content, World world)
        {
            base.Load(content, world);
#if EDITOR

#else
            this._bloodiedTexture = content.Load<Texture2D>(_bloodiedTextureAsset);
            this.wallDecalEnd = content.Load<Texture2D>("Assets/Images/Textures/Saw/i_sawDecalEndPiece");
            this.wallDecalMiddle = content.Load<Texture2D>("Assets/Images/Textures/Saw/i_sawDecalMiddlePiece");

            if (_movementDirection == Direction.Horizontal)
            {
                _decalRotation = -MathHelper.PiOver2;

                _decalRectangle = new Rectangle(
                (int)EndPosition.X + this.wallDecalEnd.Width / 2,
                (int)EndPosition.Y + this.wallDecalEnd.Height / 2,
                (int)(this.wallDecalEnd.Height),
                (int)(Position.X - EndPosition.X - this.wallDecalEnd.Width));
            }
            else
            {
                _decalRectangle = new Rectangle(
                (int)EndPosition.X - this.wallDecalEnd.Width / 2,
                (int)EndPosition.Y + this.wallDecalEnd.Height / 2,
                (int)(this.wallDecalEnd.Width),
                (int)(Position.Y - EndPosition.Y) - this.wallDecalEnd.Height);
            }

            this.SetUpPhysics(world);
#endif
        }

        public override void Update(GameTime gameTime)
        {
#if EDITOR

#else
            if (this.PrismaticJoint.MotorSpeed != 0)
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
            base.Draw(spriteBatch);
        }
#else

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureToUse, ConvertUnits.ToDisplayUnits(this.Body.Position), null, Color.White, 
                TextureRotation, Origin, 1.0f, SpriteEffects.None, zLayer); 

            sb.Draw(wallDecalEnd, this.Position, null, Color.White, _decalRotation,
                new Vector2(this.wallDecalEnd.Width / 2, this.wallDecalEnd.Height / 2), 1.0f, SpriteEffects.None, zLayer - 0.01f);
            sb.Draw(wallDecalEnd, this.EndPosition, null, Color.White, (float)Math.PI + _decalRotation,
                new Vector2(this.wallDecalEnd.Width / 2, this.wallDecalEnd.Height / 2), 1.0f, SpriteEffects.None, zLayer - 0.01f);
            sb.Draw(wallDecalMiddle, _decalRectangle, null, Color.White, _decalRotation,
                Vector2.Zero, SpriteEffects.None, zLayer - 0.01f);

#if Development
            sb.DrawString(Fonts.DebugFont, "ToStart: " + this.MovingToStart + ". Speed: " + this.PrismaticJoint.MotorSpeed + ". IsMoving: " + this._isMoving, 
                ConvertUnits.ToDisplayUnits(this.Body.Position) + new Vector2(400, -60), Color.Red);

            sb.DrawString(Fonts.DebugFont, "UpL: " + this.PrismaticJoint.UpperLimit + ". LoL: " + this.PrismaticJoint.LowerLimit,
                ConvertUnits.ToDisplayUnits(this.Body.Position) + new Vector2(400, -45), Color.Red);

            sb.DrawString(Fonts.DebugFont, "Translation: " + this.PrismaticJoint.JointTranslation.ToString(), 
                ConvertUnits.ToDisplayUnits(this.Body.Position) + new Vector2(400, -30), Color.Red);

            sb.DrawString(Fonts.DebugFont, (this.PrismaticJoint.JointTranslation >= this.PrismaticJoint.UpperLimit) + (this.MovingToStart == false).ToString(), this.Position - new Vector2(0, 500), Color.Red);
            sb.DrawString(Fonts.DebugFont, (this.PrismaticJoint.JointTranslation <= this.PrismaticJoint.LowerLimit) + (this.MovingToStart == true).ToString(), this.Position - new Vector2(0, 485), Color.Red);
#endif

        }
#endif
        #endregion

        #region Private Methods

        protected override void SetUpPhysics(World world)
        {
            this.Body = BodyFactory.CreateCircle(world,
                ConvertUnits.ToSimUnits(this._texture.Width / 2),
                ConvertUnits.ToSimUnits(_mass));

            this.Body.BodyType = BodyType.Dynamic;
            this.Body.Position = ConvertUnits.ToSimUnits(this.Position);
            this.Body.Friction = 1.0f;
            this.Body.Restitution = 0.5f;

            base.SetUpJoint(world);

            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;
        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!TouchingFixtures.Contains(fixtureB) && fixtureB == Player.Instance.Body.FixtureList[0])
            {
                TouchingFixtures.Add(fixtureB);
                Player.Instance.Kill();
                _touched = true;
            }

            return true;
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            TouchingFixtures.Remove(fixtureB);
        }

        #endregion
    }
}
