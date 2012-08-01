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

namespace GameLibrary.GameLogic.Objects
{
    public class Piston : DynamicObject
    {
        #region Fields

#if EDITOR

#else
        private List<FixedPrismaticJoint> _joints = new List<FixedPrismaticJoint>();
        private List<Body> _bodies = new List<Body>();
#endif

        
        
        private Texture2D _endTexture;
        private Vector2 _endOrigin;

        [ContentSerializer(Optional=true)]
        private uint _shaftPieces = 0;
        [ContentSerializer]
        private string _endTextureAsset = String.Empty;

        #endregion

        #region Properties

#if EDITOR
        [ContentSerializerIgnore]
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
        [ContentSerializerIgnore]
        public override Vector2 EndPosition
        {
            get
            {
                return _position - SpinAssist.ModifyVectorByOrientation(new Vector2(0, _texture.Height * _shaftPieces), this._orientation);
            }
            set { }
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
        }

        public override void Load(ContentManager content, World world)
        {
            this._texture = content.Load<Texture2D>(this._textureAsset);
            this._endTexture = content.Load<Texture2D>(this._endTextureAsset);

            this._origin = new Vector2(this._texture.Width, this._texture.Height) * 0.5f;
            this._endOrigin = new Vector2(this._endTexture.Width, this._endTexture.Height) * 0.5f;

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
#endif
        }

        public override void Update(float delta)
        {
#if EDITOR
#else
            base.Update(delta);

            for (int i = 0; i < _joints.Count; i++)
            {
                this.HandleJoint(delta, _joints[i]);
            }
#endif
        }

        #region Draw
#if EDITOR
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, this._position, null, this._tint, this._rotation, this._origin, 1.0f, SpriteEffects.None, this._zLayer);
            spriteBatch.Draw(_endTexture, this.EndPosition, null, this._tint, this._rotation, this._endOrigin, 1.0f, SpriteEffects.None, this._zLayer);
        }
#else
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            for (int i = 0; i < _bodies.Count; i++)
            {
                spriteBatch.Draw(_texture, ConvertUnits.ToDisplayUnits(_bodies[i].Position), null, this._tint, this._bodies[i].Rotation, this._origin, 1.0f, SpriteEffects.None, (float)(this.zLayer + (0.001 * i)));
            }

            spriteBatch.Draw(_endTexture, ConvertUnits.ToDisplayUnits(this.Body.Position), null, this._tint, this.Body.Rotation, this._endOrigin, 1.0f, SpriteEffects.None, (float)(this.zLayer + (0.001 * _shaftPieces)));

            //spriteBatch.DrawString(FontManager.Instance.GetFont(Graphics.FontList.Debug).Font,
            //            "Speed: " + this._prismaticJoint.MotorSpeed +
            //            ".\nUL: " + this._prismaticJoint.UpperLimit.ToString() +
            //            ". LL: " + this._prismaticJoint.LowerLimit.ToString() +
            //            ".\nTransl: " + this._prismaticJoint.JointTranslation.ToString() +
            //            ". Elapsed: " + _elapsedTimer.ToString() +
            //        ".\nMovingToStart: " + this.MovingToStart, ConvertUnits.ToDisplayUnits(this.Body.Position), Color.Red)
        }
#endif
        #endregion

        #region Private Methods

        protected override void SetupPhysics(World world)
        {
#if EDITOR
#else
            float textureWidth = ConvertUnits.ToSimUnits(this._texture.Width);
            float textureHeight = ConvertUnits.ToSimUnits(this._texture.Height);
            Vector2 axis = SpinAssist.ModifyVectorByOrientation(new Vector2(0, -1), _orientation);
            Body body;
            float textureWidthOffset = ConvertUnits.ToSimUnits(2.0f);
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
                    Fixture fixture = FixtureFactory.AttachRectangle(textureWidth - (textureWidthOffset * i), textureHeight, 1.0f, Vector2.Zero, body);
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

                _bodies.Add(body);
            }

            TexVertOutput input = SpinAssist.TexToVert(world, _endTexture, ConvertUnits.ToSimUnits(10), true);

            _endOrigin = ConvertUnits.ToSimUnits(input.Origin);
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
#endif
        }

        void HandleJoint(float delta, FixedPrismaticJoint joint)
        {
#if EDITOR

#else
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
