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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Dynamics.Contacts;
using System.ComponentModel;
using GameLibrary.Helpers;

namespace GameLibrary.GameLogic.Objects
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
        private List<Fixture> TouchingFixtures = new List<Fixture>();
        //private Vector2 
        private bool _touched = false;     
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
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public new Direction MovementDirection
        {
            get
            {
                return _movementDirection;
            }
            set
            {
                Direction oldDirection = _movementDirection;
                if (oldDirection != value)
                {
                    Vector2 dist = _endPosition - Position;

                    _movementDirection = value;

                    if (oldDirection == Direction.Horizontal)
                    {
                        _endPosition = new Vector2(dist.X, 0);
                    }
                    else
                    {
                        _endPosition = new Vector2(0, dist.Y);
                    }
                }
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
            this.SetupPhysics(world);
#endif
        }

        public override void Update(float delta)
        {
#if !EDITOR
            if (_prismaticJoint.MotorEnabled)
            {
                _rotation += 0.3f;

                if (_rotation >= 4)
                {
                    _rotation -= 4;
                }
            }

            base.Update(delta);
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._texture, this._position, null, this._tint, 0.0f, _origin, _scale, SpriteEffects.None, this._zLayer);
        }
#else
        public override void Draw(SpriteBatch sb, GraphicsDevice graphics)
        {
            sb.Draw(TextureToUse, ConvertUnits.ToDisplayUnits(this.Body.Position), null, this._tint, 
                this._rotation, new Vector2(this.TextureToUse.Width, this.TextureToUse.Height) * 0.5f, _scale, SpriteEffects.None, this._zLayer); 

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

#if !EDITOR
        protected override void SetupPhysics(World world)
        {
            this.Body = BodyFactory.CreateCircle(world,
                ConvertUnits.ToSimUnits((this._texture.Width * 0.5f) * _scale),
                ConvertUnits.ToSimUnits(_mass));

            this.Body.BodyType = BodyType.Dynamic;
            this.Body.Position = ConvertUnits.ToSimUnits(this.Position);
            this.Body.Friction = 1.0f;
            this.Body.Restitution = 0.2f;

            base.SetUpJoint(world);

            this.Body.OnCollision += Body_OnCollision;
            this.Body.OnSeparation += Body_OnSeparation;

            this.Body.CollisionCategories = Category.Cat20;
            this.Body.CollidesWith = Category.All & ~Category.Cat20;

        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!TouchingFixtures.Contains(fixtureB) && fixtureB == Player.Instance.PlayerHitBox)
            {
                TouchingFixtures.Add(fixtureB);
                Player.Instance.Kill();
                _touched = true;
            }
            else
            {
                return false;
            }

            return true;
        }

        protected override void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            TouchingFixtures.Remove(fixtureB);
        }
#endif

        #endregion
    }
}
