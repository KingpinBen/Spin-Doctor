using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using GameLibrary.Graphics;
using GameLibrary.Helpers;
using System.ComponentModel;
using FarseerPhysics.Dynamics.Contacts;
using GameLibrary.GameLogic.Characters;

namespace GameLibrary.GameLogic.Objects
{
    public class Piston : DynamicObject
    {
        #region Fields

#if EDITOR

#else
        private List<FixedPrismaticJoint> _joints = new List<FixedPrismaticJoint>();
        private List<Body> _shaftBodies = new List<Body>();
#endif

        private Texture2D _crusherTexture;
        private Vector2 _crusherTextureOrigin;

        [ContentSerializer(Optional = true)]
        private uint _shaftPieces = 0;
        [ContentSerializer(Optional = true)]
        private string _endTextureAsset = String.Empty;
        [ContentSerializer(Optional = true)]
        private bool _isLethal = false;

        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public uint ShaftPieces
        {
            get
            {
                return _shaftPieces;
            }
            set
            {
                _shaftPieces = value;
            }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public override Vector2 EndPosition
        {
            get
            {
                return _position - SpinAssist.ModifyVectorByOrientation(new Vector2(0, _texture.Height * _shaftPieces), this._orientation);
            }
            set { }
        }
        [ContentSerializerIgnore, CategoryAttribute("Object Specific")]
        public bool IsLethal
        {
            get
            {
                return _isLethal;
            }
            set
            {
                _isLethal = value;
            }
        }
#else
#endif

        #endregion

        public Piston() : base() { }

        public void Init(Vector2 position, string tex, string texEnd)
        {
            base.Init(position, tex);
            this._endTextureAsset = texEnd;
            this._shaftPieces = 1;
            this._isLethal = false;
        }

        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(this._textureAsset);
            this._crusherTexture = content.Load<Texture2D>(this._endTextureAsset);

#if EDITOR
            if (Width == 0 || Height == 0)
            {
                this.Width = this._texture.Width;
                this.Height = this._texture.Height;
            }
#else
            this._isMoving = this._startsMoving;
            SetupPhysics(world);

            if (!this._isMoving)
            {
                this._prismaticJoint.MotorSpeed = 0.0f;
            }
            else
            {
                this._prismaticJoint.MotorSpeed = MotorSpeed;
            }
            this.RegisterObject();
#endif

            this._crusherTextureOrigin = new Vector2(
                this._crusherTexture.Width, this._crusherTexture.Height) * 0.5f;
        }

        #region Update and Draw

        public override void Update(float delta)
        {
#if !EDITOR
            base.Update(delta);

            if (_isMoving)
            {
                for (int i = 0; i < _joints.Count; i++)
                {
                    this.HandleJoint(delta, _joints[i]);
                }
            }
#endif
        }

        #region Draw

#if EDITOR

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, this._position, null, this._tint, this._rotation, new Vector2(this._texture.Width, this._texture.Height) * 0.5f, 1.0f, SpriteEffects.None, this._zLayer);
            spriteBatch.Draw(_crusherTexture, this.EndPosition, null, this._tint * 0.5f, this._rotation, this._crusherTextureOrigin, 1.0f, SpriteEffects.None, this._zLayer);
        }

#else
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            Vector2 shaftOrigin;
            for (int i = 0; i < _shaftBodies.Count; i++)
            {
                shaftOrigin = new Vector2(_texture.Width - ((this._texture.Width * 0.1f) * i), _texture.Height) * 0.5f;

<<<<<<< HEAD
            spriteBatch.Draw(_endTexture, ConvertUnits.ToDisplayUnits(this.Body.Position), null, this._tint, this.Body.Rotation, this._endOrigin, 1.0f, SpriteEffects.None, (float)(this.zLayer + (0.001 * (_shaftPieces + 1))));
=======
                spriteBatch.Draw(_texture, ConvertUnits.ToDisplayUnits(_shaftBodies[i].Position),
                    new Rectangle(0, 0, (int)(this._texture.Width - ((this._texture.Width * 0.1f) * i)), 
                        (int)_texture.Height), this._tint, this._shaftBodies[i].Rotation, shaftOrigin, 
                        1.0f, SpriteEffects.None, (float)(this.zLayer + (0.001 * i)));
            }
>>>>>>> Tech Doc revisions

            spriteBatch.Draw(_crusherTexture, ConvertUnits.ToDisplayUnits(this.Body.Position), null, this._tint, this.Body.Rotation, this._crusherTextureOrigin, 1.0f, SpriteEffects.None, (float)(this.zLayer + (0.001 * (_shaftPieces + 1))));
        }
#endif
        #endregion



        #endregion

        #region Private Methods

        protected override void SetupPhysics(World world)
        {
#if !EDITOR
            float textureWidth = ConvertUnits.ToSimUnits(this._texture.Width);
            float textureHeight = ConvertUnits.ToSimUnits(this._texture.Height);
            Vector2 axis = SpinAssist.ModifyVectorByOrientation(new Vector2(0, -1), _orientation);
            Body body;
            
            #region Shaft
            for (int i = 0; i < _shaftPieces; i++)
            {
                body = new Body(world);
                body.Position = ConvertUnits.ToSimUnits(this._position);
                body.Rotation = SpinAssist.RotationByOrientation(this._orientation);

                if (i == 0)
                {
                    Fixture fixture = FixtureFactory.AttachRectangle(textureWidth, textureHeight, 1.0f, Vector2.Zero, body);
                    body.BodyType = BodyType.Static;
                }
                else
                {
                    Fixture fixture = FixtureFactory.AttachRectangle(textureWidth - ((textureWidth * 0.1f) * i), textureHeight, 1.0f, Vector2.Zero, body);
                    body.BodyType = BodyType.Dynamic;

                    FixedPrismaticJoint _joint = JointFactory.CreateFixedPrismaticJoint(world, body, ConvertUnits.ToSimUnits(this._position), axis);
                    _joint.MotorEnabled = true;
                    _joint.MaxMotorForce = float.MaxValue;
                    _joint.LimitEnabled = true;
                    _joint.LowerLimit = 0;
                    _joint.UpperLimit = (textureHeight * i);
                    _joints.Add(_joint);
                }

                body.CollisionCategories = Category.Cat2;
                //  Ignore collision with Statics and other pistons.
                body.CollidesWith = Category.All & ~Category.Cat2 & ~Category.Cat20;

                _shaftBodies.Add(body);
            }
#endregion

            #region Endpiece

            TexVertOutput input = SpinAssist.TexToVert(world, _crusherTexture, ConvertUnits.ToSimUnits(10), true);

            _crusherTextureOrigin = -ConvertUnits.ToSimUnits(input.Origin);

            this.Body = input.Body;
            //  TODO: Add proper position.
            this.Body.Position = ConvertUnits.ToSimUnits(this._position);
            this.Body.Rotation = this._rotation;
            this.Body.Friction = 3.0f;
            this.Body.Restitution = 0.0f;
            this.Body.BodyType = BodyType.Dynamic;
            this.Body.Mass = 100.0f;
            this.Body.CollisionCategories = Category.Cat2;
            //  Ignore collision with Statics and other pistons.
            this.Body.CollidesWith = Category.All & ~Category.Cat2 & ~Category.Cat20;

            this._prismaticJoint = JointFactory.CreateFixedPrismaticJoint(world, this.Body, ConvertUnits.ToSimUnits(this._position + new Vector2(0, 40)), axis);
            this._prismaticJoint.UpperLimit = (textureHeight * (_shaftPieces));
            this._prismaticJoint.LowerLimit = (textureHeight * 0.5f);
            this._prismaticJoint.LimitEnabled = true;
            this._prismaticJoint.MotorEnabled = true;
            this._prismaticJoint.MaxMotorForce = float.MaxValue;

#endregion

            if (_isLethal)
            {
                float endHeight = ConvertUnits.ToSimUnits(_crusherTexture.Height);

                Fixture fix = FixtureFactory.AttachRectangle(
                    ConvertUnits.ToSimUnits(_crusherTexture.Width - 20),
                    endHeight * 0.38f,
                    1.0f, new Vector2(0, -endHeight * 0.4f),
                    Body);
                fix.IsSensor = true;
                fix.IgnoreCollisionWith(this.Body.FixtureList[0]);

                fix.Body.OnCollision += TouchedLethal;
                fix.Body.OnSeparation += LeftLethal;
            }
#endif
        }
#if !EDITOR
        bool TouchedLethal(Fixture fixA, Fixture fixB, Contact contact)
        {
            if (fixA == Body.FixtureList[0])
            {
                return false;
            }

            if (Player.Instance.CheckBodyBox(fixB))
            {
                if (!_touchingFixtures.Contains(fixB))
                {
                    _touchingFixtures.Add(fixB);
                    Player.Instance.Kill();
                }
            }

            return true;
        }

        void LeftLethal(Fixture fixA, Fixture fixB)
        {
            if (_touchingFixtures.Contains(fixB))
            {
                _touchingFixtures.Remove(fixB);
            }
        }
#endif

        void HandleJoint(float delta, FixedPrismaticJoint joint)
        {
#if !EDITOR
            if ((joint.JointTranslation >= joint.UpperLimit && !this.MovingToStart) ||
                (joint.JointTranslation <= joint.LowerLimit && this.MovingToStart))
            {
                if (joint.MotorSpeed != 0)
                {
                    joint.MotorSpeed = 0.0f;
                }
            }
            else
            {
                if (joint.MotorSpeed != this._motorSpeed)
                {
                    joint.MotorSpeed = this._motorSpeed;
                }
            }
#endif
        }

        #endregion
    }
}
